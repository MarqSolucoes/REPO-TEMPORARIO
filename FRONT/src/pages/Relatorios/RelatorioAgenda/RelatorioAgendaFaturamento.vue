<template>
    <div class="relatorioAgendaFaturamento-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Agenda Faturamento
        </h1>

        <b-modal :no-close-on-backdrop="true" id="modalAlterarDEF" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalAlterarDEF_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Alteração</span>
                <h3 class="afm-hero__title">Alterar Dados</h3>
                <p class="afm-hero__description">Informe o novo DEF e a nova data de pagamento.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group mb-3">
                  <label class="afm-label">Novo DEF</label>
                  <multiselect v-model="modalAlterarDEF_DefSelecionado" :multiple="false" :options="filtro_Def"
                    :custom-label="descricaoDef" select-label="Selecionar" placeholder="Selecione um DEF">
                  </multiselect>
                </div>

                <div class="afm-field-group">
                  <label class="afm-label">Nova data de pagamento</label>
                  <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalAlterarDEF_DataSelecionada"
                    format="dd/MM/yyyy" type="date" :open.sync="open">
                  </DatePickerMask>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalAlterarDEF_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Fechar
                  </b-button>
                  <b-button variant="success" class="mb-0" @click="alterarDef()">
                    <i class="fa fa-save mr-1"></i> Alterar Dados
                  </b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col md="4">
                    <label class="mr-3">Obra:</label><br />
                    <multiselect v-model="filtro_ObraSelecionada" :multiple="true" :options="filtro_Obras"
                        select-label="Selecionar" placeholder="Selecione uma ou mais Obras" label="codigo"
                        track-by="codigo">
                    </multiselect>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Cliente:</label><br />
                    <multiselect v-model="filtro_ClienteSelecionado" :multiple="true" :options="filtro_Clientes"
                        select-label="Selecionar" placeholder="Selecione um ou mais Clientes" label="nomeFantasia"
                        track-by="nomeFantasia">
                    </multiselect>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">DEF:</label>
                    <multiselect v-model="filtro_DefsSelecionados" :multiple="true" :options="filtro_Def"
                        select-label="Selecionar" placeholder="Selecione um ou mais DEFs" :custom-label="descricaoDef"
                        label="codigo" track-by="codigo">
                    </multiselect>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4">
                    <label class="mr-3">Fornecedor:</label>
                    <multiselect v-model="filtro_FornecedoresSelecionados" :multiple="true" :options="filtro_Fornecedor"
                        select-label="Selecionar" placeholder="Selecione um ou mais Fornecedores" label="nomeFantasia"
                        track-by="nomeFantasia">
                    </multiselect>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Número NF:</label>
                    <b-form-input v-model="filtro_NumeroNF" style="color: white"></b-form-input>
                </b-col>
                <b-col md="2">
                    <label class="mr-3">Data Inicial:</label><br />

                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataInicial" format="dd/MM/yyyy"
                        :clearable='true' type="date">
                    </DatePickerMask>
                    <a @click="filtro_DataInicial = null">limpar</a>
                </b-col>
                <b-col md="2">
                    <label class="mr-3">Data Final:</label><br />
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFinal" format="dd/MM/yyyy"
                        type="date">
                    </DatePickerMask>
                    <a @click="filtro_DataFinal = null">limpar</a>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4">
                    <label class="mr-3">Pedido Interno:</label>
                    <b-form-input v-model="filtro_PedidoInterno" style="color: white"></b-form-input>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Ordem Compra:</label>
                    <b-form-input v-model="filtro_OrdemCompra" style="color: white"></b-form-input>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4"></b-col>
                <b-col md="4"></b-col>
                <b-col md="4" style="text-align: right;">
                    <button @click="limpaFiltros()" v-b-modal.cadastro type="button"
                        class="btn width-120 mb-3 mr-4 btn-danger">
                        Limpar Filtros
                    </button>
                    <button @click="obtemAgenda()" v-b-modal.cadastro type="button"
                        class="btn width-120 mb-3 mr-4 btn-success">
                        Pesquisar
                    </button>

                </b-col>
            </b-row>
        </Widget>

        <b-row>
            <b-col md="4"></b-col>
            <b-col md="4"></b-col>
            <b-col md="4" style="text-align: right;">
                <button @click="downloadExcel()" v-b-modal.cadastro type="button"
                    class="btn width-120 mb-3 mr-4 btn-outline-success">
                    Download Excel
                </button>
                <button @click="downloadPDF()" v-b-modal.cadastro type="button"
                    class="btn width-120 mb-3 mr-4 btn-outline-danger">
                    Download PDF
                </button>

            </b-col>
        </b-row>
        <br />

        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col>
                    <h2>Detalhes</h2>
                </b-col>
            </b-row>
            <b-row>
                <b-col lg="12">

                    <div class="table-card">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro">OBRA</th>
                                    <th class="estilo-cabecalho texto-centro">DEF</th>
                                    <th class="estilo-cabecalho texto-centro">ORDEM COMPRA</th>
                                    <th class="estilo-cabecalho texto-centro">PEDIDO INTERNO</th>
                                    <th class="estilo-cabecalho texto-centro">Nº NOTA FISCAL</th>
                                    <th class="estilo-cabecalho texto-centro">DATA LANÇAMENTO</th>
                                    <th class="estilo-cabecalho texto-centro">DATA PAGAMENTO</th>
                                    <th class="estilo-cabecalho texto-centro">VALOR</th>
                                    <th class="estilo-cabecalho">PAGAR AO FORNECEDOR</th>
                                    <th class="estilo-cabecalho">PAGAR AO COLABORADOR</th>
                                    <th class="estilo-cabecalho texto-centro">AÇÕES</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in agendas" :key="'ag-' + index" :class="rowClass(row, 'row')">
                                    <td class="estilo-celula texto-centro">{{ row.codigoObra }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.codigoDef }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.codigoPedidoCompra }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.codigoPedidoInterno }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.numeroNF }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.dataLancamento == null || row.dataLancamento == '2500-01-01T00:00:00') ? "" : formataDataSemHora(row.dataLancamento) }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.dataPagamento == null || row.dataPagamento == '2500-01-01T00:00:00') ? "" : formataDataSemHora(row.dataPagamento) }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.valor == null || row.valor == 0) ? "" : new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL", }).format(row.valor).replace('R$', '') }}</td>
                                    <td class="estilo-celula">{{ row.nomeFantasiaFornecedor }}</td>
                                    <td class="estilo-celula">{{ row.nomeUsuarioBeneficiario }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <button type="button" v-if="row.codigoObra != 'TOTAL'" @click=" modalAlterarDEF_Exibir = true; modalAlterarDEF_Id = row.id; modalAlterarDEF_DataSelecionada = row.dataPagamento, modalAlterarDEF_DefSelecionado = null" class="btn mb-3 mr-3 btn-info">
                                            <i class="fa fa-pencil" title="Alterar Dados"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr v-if="!agendas || agendas.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="11">Nenhum registro encontrado</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </b-col>
            </b-row>
        </Widget>
    </div>
</template>

<script>

import moment from "moment";
import ApiService from "@/services/api.service.js";
import Multiselect from "vue-multiselect";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import CurrencyInput from "../../../components/CurrencyInput.vue";
import { Money } from 'v-money';
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";

export default {
    name: 'RelatorioAgenda',
    components: {
        Loading,
        Multiselect,
        CurrencyInput,
        Money,
        DatePickerMask
    },
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

            open: false,
            isLoading: false,

            usuarioLogado: null,

            modalAlterarDEF_Exibir: false,
            modalAlterarDEF_DefSelecionado: null,
            modalAlterarDEF_DataSelecionada: null,

            filtro_ObraSelecionada: null,
            filtro_ClienteSelecionado: null,
            filtro_DefsSelecionados: [],
            filtro_FornecedoresSelecionados: [],
            filtro_DataInicial: null,
            filtro_DataFinal: null,
            filtro_NumeroNF: '',
            filtro_PedidoInterno: '',
            filtro_OrdemCompra: '',

            filtro_Obras: [],
            filtro_Clientes: [],
            filtro_Def: [],
            filtro_Fornecedor: [],

            agendas: [],

            fields: [
                { key: 'codigoObra', label: 'OBRA' },
                { key: 'codigoDef', label: 'DEF' },
                { key: 'codigoPedidoCompra', label: 'ORDEM COMPRA' },
                { key: 'codigoPedidoInterno', label: 'PEDIDO INTERNO' },
                { key: 'numeroNF', label: 'Nº NOTA FISCAL' },
                { key: 'dataLancamento', label: 'DATA LANÇAMENTO' },
                { key: 'dataPagamento', label: 'DATA PAGAMENTO' },
                { key: 'valor', label: 'VALOR' },
                { key: 'nomeFantasiaFornecedor', label: 'PAGAR AO FORNECEDOR' },
                { key: 'nomeUsuarioBeneficiario', label: 'PAGAR AO COLABORADOR' },
                { key: 'acoes', label: 'AÇÕES' },
            ],

          
        };
    },
    methods: {

        rowClass(item, type) {
            if (!item || type !== 'row') return
            if (item.codigoObra === 'TOTAL') return 'table-Total'
        },

        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        descricaoDef({ codigo, descricao }) {
            return `${codigo} - ${descricao}`;
        },

        downloadPDF: function(){
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => {
                    objParametros.idsClientes.push(x.id);
                });

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objParametros.idsFornecedores.push(x.id);
                });

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => {
                    objParametros.idsObras.push(x.id);
                });

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => {
                    objParametros.idsDefs.push(x.id);
                });

            ApiService.downloadPDFAgendaFaturamento(objParametros, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", "Erro ao baixar PDF", "error");
                } else {

                    const blob = new Blob([result.data], {
                        type: result.contentType,
                    });

                    const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
                }
            });
        },


        downloadPDFFaturamento: function(){
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => {
                    objParametros.idsClientes.push(x.id);
                });

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objParametros.idsFornecedores.push(x.id);
                });

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => {
                    objParametros.idsObras.push(x.id);
                });

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => {
                    objParametros.idsDefs.push(x.id);
                });

            ApiService.downloadPDFAgendaFaturamento(objParametros, (result) => {
                this.isLoading = false;

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

        downloadExcel: function () {
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => {
                    objParametros.idsClientes.push(x.id);
                });

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objParametros.idsFornecedores.push(x.id);
                });

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => {
                    objParametros.idsObras.push(x.id);
                });

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => {
                    objParametros.idsDefs.push(x.id);
                });

            ApiService.downloadExcelAgendaFaturamento(objParametros, (result) => {
                this.isLoading = false;

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

        downloadPDFFaturamento: function(){
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => {
                    objParametros.idsClientes.push(x.id);
                });

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objParametros.idsFornecedores.push(x.id);
                });

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => {
                    objParametros.idsObras.push(x.id);
                });

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => {
                    objParametros.idsDefs.push(x.id);
                });

            ApiService.downloadPDFAgendaFaturamento(objParametros, (result) => {
                this.isLoading = false;

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

        alterarDef: function () {
            if (this.modalAlterarDEF_DefSelecionado == null) {
                this.$swal("", "Informe um DEF válido", "error");
                return;
            }

            if(this.modalAlterarDEF_DataSelecionada == null)
            {
                this.$swal("", "Informe uma data válida", "error");
                return;
            }

            this.isLoading = true;

            var objParametros = {
                id: this.modalAlterarDEF_Id,
                idDef: this.modalAlterarDEF_DefSelecionado.id,
                dataPagamento: this.modalAlterarDEF_DataSelecionada
            };

            ApiService.alteraDefAgenda(objParametros, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.modalAlterarDEF_Exibir = false;
                    this.obtemAgenda();
                }
            });
        },

        listaObras: function () {
            this.filtro_Obras = [];

            ApiService.getAll("Obra", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.filtro_Obras = result.data;
                }
            });
        },

        listaClientes: function () {
            this.filtro_Clientes = [];

            ApiService.getAll("Cliente", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.filtro_Clientes = result.data;
                }
            });
        },

        listaDEFs: function () {
            this.filtro_Def = [];

            ApiService.getAll("DEF", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.filtro_Def = result.data;
                }
            });
        },

        listaFornecedores: function () {
            this.filtro_Fornecedor = [];

            ApiService.getAll("Fornecedor", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.filtro_Fornecedor = result.data;
                }
            });
        },

        obtemAgenda: function () {

            this.isLoading = true;

            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => {
                    objParametros.idsClientes.push(x.id);
                });

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objParametros.idsFornecedores.push(x.id);
                });

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => {
                    objParametros.idsObras.push(x.id);
                });

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => {
                    objParametros.idsDefs.push(x.id);
                });

            ApiService.obtemAgendaFaturamento(objParametros, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    console.log(result.data);
                    this.agendas = result.data;
                }
            });

        },
    },
    mounted() {
        this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaClientes();
        this.listaObras();
        this.listaDEFs();
        this.listaFornecedores();
    },
};
</script>

<style src="./RelatorioAgenda.scss" lang="scss" />