<template>
    <div class="pedidoInterno-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Pedidos Internos &nbsp;</h1>

        <b-modal :no-close-on-backdrop="true" id="modalPedidoInternoEdicao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
            header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalPedidoInternoEdicao_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Parcela</span>
                <h3 class="afm-hero__title">Editar Parcela</h3>
                <p class="afm-hero__description">Altere a data de pagamento e o valor da parcela.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <b-row>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Data do Pagamento</label>
                      <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalPedidoInternoEdicao_DataPagamento"
                        format="dd/MM/yyyy" type="date" :open.sync="open">
                      </DatePickerMask>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Valor</label>
                      <Money v-model="modalPedidoInternoEdicao_Valor" v-bind="money"
                        :style="{ 'color': 'white', 'background-color': 'black', 'border-width': '1px' }"></Money>
                    </div>
                  </b-col>
                </b-row>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalPedidoInternoEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-on:click="alterarParcela()" variant="success" class="mb-0">
                    <i class="fa fa-save mr-1"></i> Alterar Parcela
                  </b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalPedidoInterno" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalPedidoInterno_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Pedido Interno</span>
                <h3 class="afm-hero__title">{{ modalPedidoInterno_Titulo }}</h3>
                <p class="afm-hero__description">Consulte e gerencie as parcelas deste pedido interno.</p>
              </div>
              <div class="afm-hero__pill">
                <span>{{ modalPedidoInterno_Parcelas ? modalPedidoInterno_Parcelas.length : 0 }}</span>
                <small>parcela(s)</small>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div v-if="modalPedidoInterno_Parcelas && modalPedidoInterno_Parcelas.length > 0">
                  <div class="table-card table-card--fluid">
                    <table class="estilo-tabela tabela-identidade">
                      <thead>
                        <tr>
                          <th class="estilo-cabecalho texto-centro">Código</th>
                          <th class="estilo-cabecalho texto-centro">Parcela</th>
                          <th class="estilo-cabecalho texto-centro">Valor</th>
                          <th class="estilo-cabecalho texto-centro">Data Pag.</th>
                          <th class="estilo-cabecalho texto-centro">Pag. Efetuado</th>
                          <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(row, index) in modalPedidoInterno_Parcelas" :key="'parc-' + (row.id || index)">
                          <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                          <td class="estilo-celula texto-centro">{{ row.parcela }}</td>
                          <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                          <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPagamento) }}</td>
                          <td class="estilo-celula texto-centro">{{ row.pagamentoEfetuado ? 'Sim' : 'Não' }}</td>
                          <td class="estilo-celula texto-centro">
                            <button type="button" @click="definirParcelaComoPaga(row.id)" v-if="!row.pagamentoEfetuado"
                              class="btn btn-outline-success afm-action-btn mr-1">
                              <i class="fa fa-thumbs-up" title="Informar como Pago"></i>
                            </button>
                            <button type="button" @click="
                              modalPedidoInternoEdicao_DataPagamento = row.dataPagamento;
                              modalPedidoInternoEdicao_IdParcela = row.id;
                              modalPedidoInternoEdicao_Valor = row.valor;
                              modalPedidoInternoEdicao_Exibir = true;
                              modalPedidoInterno_Exibir = false;" v-if="!row.pagamentoEfetuado"
                              class="btn btn-outline-success afm-action-btn">
                              <i class="fa fa-pencil" title="Editar"></i>
                            </button>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <div v-else class="afm-empty-state">
                  <i class="fa fa-calendar-o"></i>
                  <div><strong>Nenhuma parcela</strong><p>Este pedido não possui parcelas.</p></div>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button v-on:click="modalPedidoInterno_Exibir = false" variant="dark" class="mb-0">
                    Fechar
                  </b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalArquivos_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Anexos</span>
                <h3 class="afm-hero__title">{{ modalArquivos_Titulo }}</h3>
                <p class="afm-hero__description">Consulte, baixe ou envie arquivos vinculados.</p>
              </div>
              <div class="afm-hero__pill">
                <span>{{ modalArquivos_Arquivos ? modalArquivos_Arquivos.length : 0 }}</span>
                <small>arquivo(s)</small>
              </div>
            </div>

            <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input" @change="onChange"
              ref="file" />

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div v-if="modalArquivos_Arquivos && modalArquivos_Arquivos.length > 0">
                  <div class="table-card table-card--fluid">
                    <table class="estilo-tabela tabela-identidade">
                      <thead>
                        <tr>
                          <th class="estilo-cabecalho">Arquivo</th>
                          <th class="estilo-cabecalho">Usuário</th>
                          <th class="estilo-cabecalho texto-centro">Data de envio</th>
                          <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(row, index) in modalArquivos_Arquivos" :key="'arq-' + (row.id || index)">
                          <td class="estilo-celula">{{ row.nome }}</td>
                          <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                          <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                          <td class="estilo-celula texto-centro">
                            <button type="button" @click="downloadFile(row.id, row.nome)"
                              class="btn btn-warning afm-action-btn mr-1">
                              <i class="fa fa-download" title="Download"></i>
                            </button>
                            <button type="button" @click="excluirArquivo(row.id)" class="btn btn-danger afm-action-btn"
                              v-if="usuarioLogado == null ? false : (usuarioLogado.id == row.usuarioCadastro.id ? true : false)">
                              <i class="fa fa-trash" title="Excluir"></i>
                            </button>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <div v-else class="afm-empty-state">
                  <i class="fa fa-inbox"></i>
                  <div><strong>Nenhum arquivo anexado</strong><p>Envie o primeiro arquivo clicando no botão abaixo.</p></div>
                </div>

                <div class="mt-3" style="text-align: center;">
                  <label type="button" class="btn btn-success afm-add-btn" for="fileInput" style="width: auto; min-width: 200px; max-width: 320px;">
                    <i class="fa fa-upload mr-1"></i> Enviar arquivo
                  </label>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalArquivos_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <Widget customHeader class="estiloWidget">
            <div class="table-card">
                <table class="estilo-tabela tabela-identidade">
                    <thead>
                        <tr>
                            <th class="estilo-cabecalho texto-centro">Código</th>
                            <th class="estilo-cabecalho">Descrição</th>
                            <th class="estilo-cabecalho texto-centro">Beneficiário</th>
                            <th class="estilo-cabecalho texto-centro">Valor</th>
                            <th class="estilo-cabecalho texto-centro">Aprovado</th>
                            <th class="estilo-cabecalho texto-centro">Parcelas</th>
                            <th class="estilo-cabecalho texto-centro">Data Solicitação</th>
                            <th class="estilo-cabecalho">Usuário</th>
                            <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(row, index) in pedidosInternos" :key="'pi-' + (row.id || index)">
                            <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                            <td class="estilo-celula">{{ row.descricao }}</td>
                            <td class="estilo-celula texto-centro">{{ row.idFornecedorBeneficiario == null ? (row.usuarioBeneficiario ? row.usuarioBeneficiario.nome : '') : (row.fornecedorBeneficiario ? row.fornecedorBeneficiario.nomeFantasia : '') }}</td>
                            <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                            <td class="estilo-celula texto-centro">{{ row.aprovado == null ? 'Pendente' : row.aprovado ? 'Sim' : 'Não' }}</td>
                            <td class="estilo-celula texto-centro">{{ row.numeroTotalParcelas }}</td>
                            <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataCadastro) }}</td>
                            <td class="estilo-celula">{{ row.usuarioCadastro != null ? row.usuarioCadastro.nome : '' }}</td>
                            <td class="estilo-celula texto-centro">
                                <button type="button" @click="abrirModalDetalhe(row)"
                                    class="btn width-75 mb-3 mr-3 btn-outline-success">
                                    <i class="fa fa-eye"></i>
                                </button>
                                <button type="button" @click="modalArquivos_IdPedidoInterno = row.id;
                                modalArquivos_Arquivos = row.arquivos;
                                modalArquivos_Exibir = true" class="btn width-75 mb-3 mr-3 btn-outline-info">
                                    <i class="fa fa-file" title="Arquivos"></i>
                                </button>
                            </td>
                        </tr>
                        <tr v-if="!pedidosInternos || pedidosInternos.length === 0">
                            <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </Widget>
    </div>
</template>
  
<script>
import Vue from "vue";
import moment from "moment";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import { VueMaskDirective } from "v-mask";
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";
import { Money } from 'v-money';

import ApiService from "@/services/api.service.js";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: "PedidoInterno",
    components: { Widget, Loading, DatePickerMask, Money },
    data() {
        return {
            money: {
                decimal: ',',
                thousands: '.',
                prefix: 'R$ ',
                precision: 2,
                masked: false,
            },

            dataMinima: (new Date()).setDate(new Date().getDate() -1),

            isLoading: false,

            open: false,

            usuarioLogado: null,

            permissao_Cadastrar: true,
            permissao_AtivarDesativar: true,

            modalPedidoInterno_Exibir: false,
            modalPedidoInterno_Titulo: 'Pedido Interno',
            modalPedidoInterno_Parcelas: [],

            modalPedidoInternoEdicao_Exibir: false,
            modalPedidoInternoEdicao_DataPagamento: null,
            modalPedidoInternoEdicao_IdParcela: 0,
            modalPedidoInternoEdicao_Valor: 0,

            modalArquivos_Exibir: false,
            modalArquivos_Titulo: 'Arquivos',
            modalArquivos_Arquivos: [],
            modalArquivos_Controller: 'PedidoInterno',
            modalArquivos_IdPedidoInterno: 0,

            upload_Files: [],

            pedidosInternos: [],

            columns: ["codigo", "descricao", "beneficiario", "valor", "aprovado", "numeroParcelas", "dataCadastro", "usuarioCadastro", "acoes"],

            options: {
                perPage: 10,
                headings: {
                    codigo: "Código",
                    descricao: "Descrição",
                    beneficiario: "Beneficiário",
                    valor: "Valor",
                    aprovado: "Aprovado",
                    numeroParcelas: "Parcelas",
                    dataCadastro: "Data Solicitação",
                    usuarioCadastro: "Usuário",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: ["descricao", "ativo"],
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

            columnsTabelaParcelas: ["codigoFormatado", "parcela", "valor", "dataPagamento", "pagamentoEfetuado", "acoes"],

            optionsTabelaParcelas: {
                perPage: 100,
                headings: {
                    codigoFormatado: "Código",
                    parcela: "Parcela",
                    valor: "Valor",
                    dataPagamento: "Data Pag.",
                    pagamentoEfetuado: "Pag. Efetuado",
                    acoes: "Ações",
                },
                clientSorting: false,
                sortable: [],
                pagination: { chunk: 2, dropdown: false },
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
            colunasTabelaArquivos: ["arquivo", "usuario", "data", "acoes"],

            opcoesTabelaArquivos: {
                perPage: 1000,
                headings: {
                    arquivo: "Arquivo",
                    usuario: "Usuário",
                    data: "Data de envio",
                    acoes: "",
                },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: false },
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
        formataData: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
        },

        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        abrirModalDetalhe(pedidoInterno) {
            this.modalPedidoInterno_Titulo = 'Pedido Interno ' + pedidoInterno.codigoFormatado;
            this.modalPedidoInterno_Exibir = true;
            this.modalPedidoInterno_Parcelas = pedidoInterno.parcelas;
        },

        alterarParcela() {

            var objParcela = {
                dataPagamento: this.modalPedidoInternoEdicao_DataPagamento,
                valor: this.modalPedidoInternoEdicao_Valor,
                idParcela: this.modalPedidoInternoEdicao_IdParcela
            };

            this.isLoading = true;

            ApiService.alterarParcela(objParcela, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.data, "error");
                } else {
                    this.getAll();
                    this.modalPedidoInternoEdicao_Exibir = false;
                }
            });
        },

        definirParcelaComoPaga(idParcela) {

            this.isLoading = true;

            ApiService.definirParcelaComoPaga(idParcela, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.data, "error");
                } else {
                    this.getAll();
                    this.modalPedidoInterno_Exibir = false;
                }
            });
        },

        onChange() {
            this.upload_Files = [...this.$refs.file.files];

            this.uploadFiles();
        },

        uploadFiles() {
            this.isLoading = true;

            const formData = new FormData();
            this.upload_Files.forEach((file) => {
                formData.append("arquivo", file);
            });

            ApiService.uploadFile(this.modalArquivos_Controller, formData, this.modalArquivos_IdPedidoInterno, (result) => {
                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    this.upload_Files = [];
                    this.obtemArquivos(this.modalArquivos_IdPedidoInterno);
                    this.$swal("Arquivo enviado com sucesso", result.message, "success");
                }
            });
        },

        obtemArquivos: function (idSolicitacaoOuPedidoCompra) {
            ApiService.getArquivos(this.modalArquivos_Controller, idSolicitacaoOuPedidoCompra, (result) => {
                this.isLoading = false;

                if (result.status == 200) {
                    this.modalArquivos_Arquivos = result.data;
                }
            });
        },

        downloadFile(id, nomeArquivo) {
            this.isLoading = true;

            ApiService.downloadFile(this.modalArquivos_Controller, id, (result) => {
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

        excluirArquivo(id) {
            this.isLoading = true;

            ApiService.deleteArquivo(this.modalArquivos_Controller, id, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                }
                else {
                    this.obtemArquivos(this.modalArquivos_IdPedidoInterno);
                    this.$swal("Arquivo excluído com sucesso", result.message, "success");
                }
            });
        },

        getAll: function () {
            this.isLoading = true;
            this.pedidosInternos = [];

            ApiService.getAllPedidosInternos('PedidoInterno', (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidosInternos = result.data;
                    
                }
            });
        },
    },
    mounted() {
        //   var perfil = JSON.parse(localStorage.getItem("usuarioDTO")).perfil;
        //   this.permissao_Cadastrar = perfil.cargoCadastrar;
        //   this.permissao_AtivarDesativar = perfil.cargoAtivarDesativar;

        this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.getAll();
    },
};
</script>
  
  <style src="./PedidoInterno.scss" lang="scss" />
  