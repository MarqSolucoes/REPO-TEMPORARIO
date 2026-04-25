<template>
  <div class="eto-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">ETO - AJUSTE
    </h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col md="6">
          <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input" @change="onChange"
            ref="file" multiple="false" />

          <b-button @click="downloadExcelAjusteETO()" variant="success" class=" mr-3"><i class="fa fa-download"
              title="Download"></i> Download Excel de ETOs
          </b-button>

          <label type="button" class="btn width-75 mt-2 bg-warning" for="fileInput">
            <i class="fa fa-upload" title="Upload"></i> Upload Excel de ETOs
          </label>
        </b-col>
        <b-col md="6">

        </b-col>
      </b-row>
    </Widget>

     <Widget customHeader class="estiloWidget">
    <b-row>
      <b-col lg="12">
        <div class="table-card">
          <table class="estilo-tabela tabela-identidade">
            <thead>
              <tr>
                <th class="estilo-cabecalho">Usuário</th>
                <th class="estilo-cabecalho texto-centro">Data Atualização</th>
                <th class="estilo-cabecalho texto-centro">Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(row, index) in ajustes" :key="'eto-' + (row.id || index)">
                <td class="estilo-celula">{{ row.usuario ? row.usuario.nome : '' }}</td>
                <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                <td class="estilo-celula texto-centro">
                  <button
                    type="button"
                    class="btn btn-info"
                    @click="abrirModalAcoesETO(row)"
                  >
                    Arquivos <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                  </button>
                </td>
              </tr>
              <tr v-if="!ajustes || ajustes.length === 0">
                <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
              </tr>
            </tbody>
          </table>
        </div>
      </b-col>
    </b-row>
    </Widget>

    <!-- Modal de ações da tabela -->
    <ModalAcoes
      :exibir.sync="modalAcoes_Exibir"
      :titulo="modalAcoes_Titulo"
      :itens="modalAcoes_Itens"
    />
  </div>
</template>

<script>

import moment from "moment";
import ApiService from "@/services/api.service.js";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ModalAcoes from "../../../components/ModalAcoes/ModalAcoes.vue";

export default {
  name: 'ETO',
  components: { Loading, ModalAcoes },
  data() {
    return {
      // Estado do <ModalAcoes>
      modalAcoes_Exibir: false,
      modalAcoes_Titulo: "",
      modalAcoes_Itens: [],

      isLoading: false,

      upload_Files: [],

      ajustes:[],

      colunas: ["Usuario", "Data", "acoes"],

      opcoes: {
        perPage: 1000,
        headings: {
          usuario: "Usuário",
          data: "Data Atualização",
          acoes: "Ações"
        },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
        texts: {
          filterPlaceholder: "Procurar por",
          count:
            "Exibindo {from} de {to} de {count} itens|{count} itens|Um item",
          first: "Primeiro",
          last: "último",
          filter: "",
          limit: "Itens:",
          page: "Página:",
          skin: "table table-striped",
          noResults: "Não encontrado",
        },
      },


    };
  },
  methods: {
    // ============================================================
    // Ações da tabela (usadas pelo <ModalAcoes>).
    // ============================================================
    montaAcoesETO(row) {
      return [
        {
          label: "Arquivo Enviado",
          descricao: "Baixar o arquivo enviado",
          icone: "download",
          variante: "primary",
          onClick: () => this.downloadFile(row.id, 1, row.nomeLogicoDepois),
        },
      ];
    },
    abrirModalAcoesETO(row) {
      this.modalAcoes_Itens = this.montaAcoesETO(row);
      this.modalAcoes_Titulo = "Arquivos";
      this.modalAcoes_Exibir = true;
    },

    formataData: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
    },

    onChange() {
      this.upload_Files = [...this.$refs.file.files];

      this.isLoading = true;


      this.upload_Files.forEach((file) => {
        const formData = new FormData();
        formData.append("arquivo", file);
        ApiService.uploadAjusteETO(formData, () => {

          this.isLoading = false;

          this.$swal("", "Processamento concluído", "success");
        });
      });

    },

    downloadFile(id,tipo, nomeArquivo) {
      this.isLoading = true;

      ApiService.downloadArquivoAjusteETO(id, tipo, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {

          const blob = new Blob([result.data], {
            type: result.contentType,
          });

          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },

    downloadExcelAjusteETO: function () {
      ApiService.downloadExcelAjusteETO((result) => {
        if (result.status != 200) {
          this.$swal("", "Erro ao baixar excel", "error");
        } else {

          const blob = new Blob([result.data], {
            type: result.contentType,
          });

          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },

    obtemAjustes()
    {
       ApiService.obtemAjustes((result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.data, "error");
        } else {
          this.ajustes = result.data;
        }
      });
    }
  },
  mounted() { 

    this.obtemAjustes();
  },
};
</script>

<style src="./ETO.scss" lang="scss" />
