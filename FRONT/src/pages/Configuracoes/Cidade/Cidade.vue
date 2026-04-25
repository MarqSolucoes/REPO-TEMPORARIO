<template>
  <div class="cidade-page">
    <loading
      :active.sync="isLoading"
      :can-cancel="false"
      :is-full-page="true"
      color="#ffd639"
    ></loading>
    <h1 class="page-title">Cidades &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <b-modal
            id="edicao"
            class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal"
            header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal"
            v-model="modal_Exibir"
            size="lg"
          >
            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Cidade</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Edite a alíquota de ISS aplicada à cidade.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group">
                  <label class="afm-label">Alíquota INSS</label>
                  <CurrencyInput
                    v-model="cidade_AliquotaImpostoISS"
                    :options="{
                      currency: 'BRL',
                      autoDecimalDigits: true,
                      hideGroupingSeparatorOnFocus: true,
                      hideCurrencySymbolOnFocus: true,
                      currencyDisplay: 'hidden',
                    }"
                  />
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modal_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-on:click="editarCidade()" variant="info" class="mb-0">
                    <i class="fa fa-save mr-1"></i> Editar
                  </b-button>
                </div>
              </div>
            </template>
          </b-modal>
        </b-col>
      </b-row>

      <div class="table-card">
        <table class="estilo-tabela tabela-identidade">
          <thead>
            <tr>
              <th class="estilo-cabecalho texto-centro">UF</th>
              <th class="estilo-cabecalho">Nome</th>
              <th class="estilo-cabecalho texto-centro">Alíquota ISS (%)</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in cidades" :key="'cidade-' + (row.id || index)">
              <td class="estilo-celula texto-centro">{{ row.uf }}</td>
              <td class="estilo-celula">{{ row.nome }}</td>
              <td class="estilo-celula texto-centro">{{ row.aliquotaImpostoISS }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo ? "Ativa" : "Inativa" }}</td>
              <td class="estilo-celula texto-centro">
                <button
                  type="button"
                  @click="ativarDesativar(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger"
                  v-if="row.ativo == true && usuarioDTO.cidadeAtivarDesativar"
                >
                  Desativar
                </button>
                <button
                  type="button"
                  @click="ativarDesativar(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == false && usuarioDTO.cidadeAtivarDesativar"
                >
                  Ativar
                </button>
                <button
                  type="button"
                  @click="
                    (modal_Exibir = true);
                      (cidade_Id = row.id);
                      (cidade_UF = row.uf);
                      (cidade_Nome = row.nome);
                      (cidade_AliquotaImpostoISS = row.aliquotaImpostoISS);
                      (cidade_Ativo = row.ativo == 'Sim' ? true : false)
                  "
                  class="btn width-100 mb-3 mr-3 btn-outline-info"
                  v-if="usuarioDTO.cidadeEditar"
                >
                  Editar
                </button>
              </td>
            </tr>
            <tr v-if="!cidades || cidades.length === 0">
              <td class="estilo-celula texto-centro" colspan="5">Nenhum registro encontrado</td>
            </tr>
          </tbody>
        </table>
      </div>
    </Widget>
  </div>
</template>
  
  <script>
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import { VueMaskDirective } from "v-mask";
import CurrencyInput from "../../../components/CurrencyInput.vue";
import ApiService from "@/services/api.service.js";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Cargo",
  components: { Widget, Loading, CurrencyInput },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Edição de Cidade",
      modal_ExibirBotaoCadastrar: true,

      usuarioDTO: null,

      cidade_Id: 0,
      cidade_UF: "",
      cidade_Nome: "",
      cidade_AliquotaImpostoISS: 0.0,
      cidade_Ativo: true,

      controle_CargoCadastrando: false,
      controle_CargoEditando: false,

      cidades: [],

      columns: ["uf", "nome", "aliquotaImpostoISS", "status", "acoes"],

      options: {
        perPage: 10,
        headings: {
          uf: "UF",
          nome: "Nome",
          aliquotaImpostoISS: "Alíquota ISS (%)",
          status: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: ["uf", "nome", "aliquotaImpostoISS"],
        pagination: { chunk: 2, dropdown: true },
        texts: {
          filterPlaceholder: "Procurar por",
          count:
            "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
          first: "Primeiro",
          last: "último",
          filter: "",
          limit: "Itens:",
          page: "Página:",
          noResults: "Não encontrado",
        },
      },
    };
  },
  methods: {
    editarCidade: function () {
      this.isLoading = true;

      let validado = true;

      if (this.cidade_AliquotaImpostoISS == "") {
        validado = false;
        this.$swal("Informe uma alíquota válida para ISS", "", "error");
      }

      if (validado) {
        let objetoCidade = {
          Id: this.cidade_Id,
          UF: this.cidade_UF,
          Nome: this.cidade_Nome,
          AliquotaImpostoISS: this.cidade_AliquotaImpostoISS,
          Ativo: this.cidade_Ativo,
        };

        ApiService.put("Cidade", objetoCidade, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao editar cidade", result.message, "error");
          } else {
            this.listaCidades();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    ativarDesativar: function (idCidade, ativo) {
      ApiService.ativarDesativar(
        "Cidade",
        idCidade,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar cidade",
              result.message,
              "error"
            );
          } else {
            this.listaCidades();
            this.modal_Exibir = false;
          }
        }
      );
    },

    listaCidades: function () {
      this.cidades = [];

      ApiService.getAll("Cidade", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
            this.cidades = result.data;
        }
      });
    },

  },
  mounted() {
    this.isLoading = true;

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaCidades();

    this.isLoading = false;
  },
};
</script>
  
  <style src="./Cidade.scss" lang="scss" />
  