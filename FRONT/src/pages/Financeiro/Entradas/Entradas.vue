<template>
  <div class="entradas-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Entradas
    </h1>

    <b-modal :no-close-on-backdrop="true" id="modalDetalhePI" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalDetalhePI_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Pedido Interno</span>
          <h3 class="afm-hero__title">{{ modalDetalhePI_Titulo }}</h3>
          <p class="afm-hero__description">Visualize os dados gerais, parcelas e arquivos do pedido interno.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-summary-grid" style="grid-template-columns: repeat(2, minmax(0, 1fr));">
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Solicitado por</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? this.modalDetalhePI_PI.usuarioCadastro.nome : '—' }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Data Solicitação</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? formataData(this.modalDetalhePI_PI.dataCadastro) : '—' }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Aprovado por</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? this.modalDetalhePI_PI.usuarioAprovacao.nome : '—' }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Data Aprovação</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? formataData(this.modalDetalhePI_PI.dataAprovacao) : '—' }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Beneficiário</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? (this.modalDetalhePI_PI.idFornecedorBeneficiario != null ? this.modalDetalhePI_PI.fornecedorBeneficiario.nomeFantasia : this.modalDetalhePI_PI.usuarioBeneficiario.nome) : '—' }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Valor</span>
              <strong class="afm-summary-card__value">{{ this.modalDetalhePI_PI != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(this.modalDetalhePI_PI.valorTotal) : '—' }}</strong>
            </div>
          </div>
        </section>

        <section class="afm-section-card" v-if="modalDetalhePI_PI != null">
          <div class="afm-table-badge" style="margin-bottom: 10px;">Parcelas · {{ modalDetalhePI_PI.parcelas ? modalDetalhePI_PI.parcelas.length : 0 }} item(ns)</div>

          <div v-if="modalDetalhePI_PI.parcelas && modalDetalhePI_PI.parcelas.length > 0">
            <div class="table-card table-card--fluid table-card--scroll-x">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Parcela</th>
                    <th class="estilo-cabecalho texto-centro">Código</th>
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                    <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in modalDetalhePI_PI.parcelas" :key="'parc-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.parcela }}</td>
                    <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataPagamento) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <div v-else class="afm-empty-state">
            <i class="fa fa-calendar-o"></i>
            <div><strong>Nenhuma parcela</strong><p>Este pedido não possui parcelas cadastradas.</p></div>
          </div>
        </section>

        <section class="afm-section-card" v-if="modalDetalhePI_PI != null">
          <div class="afm-table-badge" style="margin-bottom: 10px;">Arquivos · {{ modalDetalhePI_PI.arquivos ? modalDetalhePI_PI.arquivos.length : 0 }} item(ns)</div>

          <div v-if="modalDetalhePI_PI.arquivos && modalDetalhePI_PI.arquivos.length > 0">
            <div class="table-card table-card--fluid table-card--scroll-x">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho">Nome</th>
                    <th class="estilo-cabecalho">Usuário Cadastro</th>
                    <th class="estilo-cabecalho texto-centro">Data Cadastro</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in modalDetalhePI_PI.arquivos" :key="'arq-' + (row.id || index)">
                    <td class="estilo-celula">{{ row.nome }}</td>
                    <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="downloadFile('PedidoInterno', row.id, row.nome)"
                        class="btn btn-warning afm-action-btn">
                        <i class="fa fa-download" title="Download"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <div v-else class="afm-empty-state">
            <i class="fa fa-inbox"></i>
            <div><strong>Nenhum arquivo anexado</strong><p>Este pedido não possui arquivos.</p></div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalDetalhePI_Exibir = false;" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-row>
      <b-col md="12" xs="12">
        <b-tabs class="mb-lg">
          <b-tab @click="obtemEntradas" v-bind:title="'PARA LIBERAÇÃO'" class="estiloWidget"
            :title-link-class="tab1Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'">
            <div class="table-card table-card--scroll-x">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho">Descrição</th>
                    <th class="estilo-cabecalho texto-centro">O.C.</th>
                    <th class="estilo-cabecalho texto-centro">P.I.</th>
                    <th class="estilo-cabecalho">Obra</th>
                    <th class="estilo-cabecalho">Cliente</th>
                    <th class="estilo-cabecalho">Fornecedor</th>
                    <th class="estilo-cabecalho">DEF</th>
                    <th class="estilo-cabecalho texto-centro">Data</th>
                    <th class="estilo-cabecalho texto-centro">Venc.</th>
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in entradas" :key="'ent-' + (row.id || index)">
                    <td class="estilo-celula">{{ row.descricao }}</td>
                    <td class="estilo-celula texto-centro">{{ row.pedidoCompra }}</td>
                    <td class="estilo-celula texto-centro">{{ row.pedidoInterno }}</td>
                    <td class="estilo-celula">{{ row.obra }}</td>
                    <td class="estilo-celula">{{ row.cliente }}</td>
                    <td class="estilo-celula">{{ row.fornecedor }}</td>
                    <td class="estilo-celula">{{ row.def }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataSolicitacao) }}</td>
                    <td class="estilo-celula texto-centro"><span style="white-space: pre-line">{{ row.dataVencimento.replace(/\\n/g, '\n') }}</span></td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-info"
                        @click="abrirModalAcoesEntrada(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!entradas || entradas.length === 0">
                    <td class="estilo-celula texto-centro" colspan="11">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

          <b-tab @click="obtemEntradasRecusadas" v-bind:title="'RECUSADAS'" class="estiloWidget"
            :title-link-class="tab2Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'">
            <div class="table-card table-card--scroll-x">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho">Descrição</th>
                    <th class="estilo-cabecalho texto-centro">O.C.</th>
                    <th class="estilo-cabecalho texto-centro">P.I.</th>
                    <th class="estilo-cabecalho">Obra</th>
                    <th class="estilo-cabecalho">Cliente</th>
                    <th class="estilo-cabecalho">Fornecedor</th>
                    <th class="estilo-cabecalho">DEF</th>
                    <th class="estilo-cabecalho texto-centro">Data</th>
                    <th class="estilo-cabecalho texto-centro">Venc.</th>
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in entradasRecusadas" :key="'rec-' + (row.id || index)">
                    <td class="estilo-celula">{{ row.descricao }}</td>
                    <td class="estilo-celula texto-centro">{{ row.pedidoCompra }}</td>
                    <td class="estilo-celula texto-centro">{{ row.pedidoInterno }}</td>
                    <td class="estilo-celula">{{ row.obra }}</td>
                    <td class="estilo-celula">{{ row.cliente }}</td>
                    <td class="estilo-celula">{{ row.fornecedor }}</td>
                    <td class="estilo-celula">{{ row.def }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataSolicitacao) }}</td>
                    <td class="estilo-celula texto-centro">{{ row.dataVencimento }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-info"
                        @click="abrirModalAcoesEntradaRecusada(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!entradasRecusadas || entradasRecusadas.length === 0">
                    <td class="estilo-celula texto-centro" colspan="11">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>
        </b-tabs>
      </b-col>
    </b-row>

    <!-- Modal de ações das tabelas -->
    <ModalAcoes
      :exibir.sync="modalAcoes_Exibir"
      :titulo="modalAcoes_Titulo"
      :itens="modalAcoes_Itens"
    />

  </div>
</template>

<script>

import moment from "moment";
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import Multiselect from "vue-multiselect";
import { VueMaskDirective } from "v-mask";
import ApiService from "@/services/api.service.js";
import ModalAcoes from "../../../components/ModalAcoes/ModalAcoes.vue";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: 'Entradas',
  components: { Widget, Loading, Multiselect, ModalAcoes },
  data() {
    return {
      // Estado do <ModalAcoes>
      modalAcoes_Exibir: false,
      modalAcoes_Titulo: "",
      modalAcoes_Itens: [],

      isLoading: false,

      usuarioDTO: null,

      modalDetalhePI_Titulo: '',
      modalDetalhePI_Exibir: false,
      modalDetalhePI_PI: null,

      tab1Ativa: true,
      tab2Ativa: false,

      entradas: [],
      entradasRecusadas: [],

      columns: ["descricao", "pedidoCompra", "pedidoInterno", "obra", "cliente", "fornecedor", "def", "dataSolicitacao", "dataVencimento", "valor", "acoes"],

      options: {
        perPage: 1000,
        headings: {
          descricao: "Descrição",
          pedidoCompra: "O.C.",
          pedidoInterno: "P.I.",
          obra: "Obra",
          cliente: "Cliente",
          fornecedor: "Fornecedor",
          def: "DEF",
          dataSolicitacao: "Data",
          dataVencimento: "Venc.",
          valor: "Valor",
          acoes: "Ações",
        },
        clientSorting: false,
        sortable: [],
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

      columnsParcelas: ["parcela", "codigo", "valor", "dataPagamento"],

      optionsParcelas: {
        perPage: 1000,
        headings: {
          parcela: "Parcela",
          codigo: "Código",
          valor: "Valor",
          dataPagamento: "Data Pagamento",
        },
        clientSorting: false,
        sortable: [],
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

      columnsArquivos: ["nome", "usuarioCadastro", "dataCadastro", "acoes"],

      optionsArquivos: {
        perPage: 1000,
        headings: {
          nome: "Nome",
          usuarioCadastro: "Usuário Cadastro",
          dataCadastro: "Data Cadastro",
          acoes: "Ações",
        },
        clientSorting: false,
        sortable: [],
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
    // ============================================================
    // Ações das tabelas (usadas pelo <ModalAcoes>).
    // ============================================================
    // Itens compartilhados pelas tabs "Aprovadas" e "Recusadas".
    // Em "Recusadas", o botão RECUSAR não aparece.
    montaAcoesEntradaBase(row, incluirRecusar) {
      return [
        {
          label: "Informações PI",
          descricao: "Ver detalhes do pedido interno",
          icone: "info-circle",
          variante: "primary",
          onClick: () => this.exibeModalDetalhesPI(row.idPedidoInterno),
          visible: row.idPedidoInterno != null,
        },
        {
          label: "PDF Pedido Compra",
          descricao: "Baixar PDF do pedido de compra",
          icone: "file-pdf-o",
          variante: "warning",
          onClick: () =>
            this.downloadPdfArquivoCompra(row.idPedidoCompra, row.pedidoCompra + ".pdf"),
          visible: row.idPedidoCompra != null,
        },
        {
          label: "PDF Nota Fiscal",
          descricao: "Baixar PDF da nota fiscal",
          icone: "file-pdf-o",
          variante: "warning",
          onClick: () => this.downloadPdfNotaFiscal(row.idPedidoCompraNotaFiscal, row.nomeNF),
          visible: row.idPedidoCompraNotaFiscal != null,
        },
        {
          label: "Aprovar",
          descricao: "Autorizar esta entrada financeira",
          icone: "check",
          variante: "success",
          onClick: () => this.autorizaEntradaFinanceiro(row),
        },
        {
          label: "Recusar",
          descricao: "Recusar esta entrada financeira",
          icone: "ban",
          variante: "danger",
          onClick: () => this.cancelaEntradaFinanceiro(row),
          visible:
            incluirRecusar &&
            (row.idPedidoCompraNotaFiscal != null || row.idPedidoInterno != null),
        },
      ];
    },

    abrirModalAcoesEntrada(row) {
      this.modalAcoes_Itens = this.montaAcoesEntradaBase(row, true);
      this.modalAcoes_Titulo = "Ações da entrada";
      this.modalAcoes_Exibir = true;
    },

    abrirModalAcoesEntradaRecusada(row) {
      this.modalAcoes_Itens = this.montaAcoesEntradaBase(row, false);
      this.modalAcoes_Titulo = "Ações da entrada recusada";
      this.modalAcoes_Exibir = true;
    },

    nameWithLang({ descricao }) {
      return `${descricao}`;
    },

    formataData: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
    },

    exibeModalDetalhesPI(id) {
      ApiService.get('PedidoInterno', id, (result) => {
        if (result.status != 200) {
          this.$swal("Erro ao obter entradas", result.message, "error");
        } else {
          this.modalDetalhePI_PI = result.data;
          this.modalDetalhePI_Titulo = 'Detalhe PI - ' + result.data.codigoFormatado + ' - ' + result.data.descricao;
          this.modalDetalhePI_Exibir = true;

          console.log(this.modalDetalhePI_PI);
        }
      });
    },

    obtemEntradas() {

      this.tab1Ativa = true;
      this.tab2Ativa = false;

      ApiService.obtemEntradasFinanceiro((result) => {
        if (result.status != 200) {
          this.$swal("Erro ao obter entradas", result.message, "error");
        } else {
          this.entradas = result.data;
        }
      });
    },

    obtemEntradasRecusadas() {

      this.tab1Ativa = false;
      this.tab2Ativa = true;

      ApiService.obtemEntradasRecusadasFinanceiro((result) => {
        if (result.status != 200) {
          this.$swal("Erro ao obter entradas", result.message, "error");
        } else {
          this.entradasRecusadas = result.data;
        }
      });
    },

    cancelaEntradaFinanceiro(entrada) {

      if (entrada.idPedidoCompraNotaFiscal != null) {
        this.$swal({
          title: "Atenção",
          text: "Por qual motivo a nota fiscal '" + entrada.nomeNF + "' está sendo recusada?",
          icon: "warning",
          showCancelButton: true,
          showDenyButton: true,
          confirmButtonColor: "#d33",
          denyButtonColor: "#d33",
          cancelButtonColor: "#aaa",
          confirmButtonText: "Nota fiscal divergente",
          denyButtonText: "Fornecedor divergente",
          cancelButtonText: "Cancelar",
        }).then((result) => {
          if (result.isConfirmed) {
            //NF divergente
            entrada.motivoRecusa = 'Nota fiscal divergente';
          }
          else if (result.isDenied) {
            //Fornecedor divergente
            entrada.motivoRecusa = 'Fornecedor divergente';
          }
          else
          {
            return;
          }

          this.$loading = true;

          ApiService.cancelaEntradaFinanceiro(entrada, (result) => {
            this.$loading = false;

            if (result.status != 200) {
              this.$swal("Erro ao cancelar entrada no financeiro", result.message, "error");
            } else {
              this.obtemEntradas();
            }
          });
        });
      }
      else {
        this.$loading = true;

        ApiService.cancelaEntradaFinanceiro(entrada, (result) => {
          this.$loading = false;

          if (result.status != 200) {
            this.$swal("Erro ao cancelar entrada no financeiro", result.message, "error");
          } else {
            this.obtemEntradas();
          }
        });
      }

    },

    autorizaEntradaFinanceiro(entrada) {
      this.isLoading = true;

      ApiService.autorizaEntradaFinanceiro(entrada, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("Erro ao autorizar entrada no financeiro", result.message, "error");
        } else {
          this.obtemEntradas();
          this.obtemEntradasRecusadas();
        }
      });
    },

    downloadPdfNotaFiscal(id, codigo) {
      this.isLoading = true;

      ApiService.downloadFile('PedidoCompra', id, (result) => {
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

    downloadPdfArquivoCompra(id, codigo) {

      var exibirDataPagamento = true;

      this.$swal({
        title: "Atenção",
        text: "Deseja exibir a data do pagamento?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
      }).then((result) => {
        if (result.isConfirmed) {
          exibirDataPagamento = true;
        }
        else {
          exibirDataPagamento = false;
        }

        this.isLoading = true;

        ApiService.downloadPdfPedidoCompra(id, exibirDataPagamento, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", "Erro ao baixar pedido de compra", "error");
          } else {

            const blob = new Blob([result.data], {
              type: result.contentType,
            });

            const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
          }
        });
      });
    },

    downloadFile(controller, id, nomeArquivo) {
      this.isLoading = true;

      ApiService.downloadFile(controller, id, (result) => {
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
  },
  mounted() {
    this.obtemEntradas();
  },
};
</script>

<style src="./Entradas.scss" lang="scss" />