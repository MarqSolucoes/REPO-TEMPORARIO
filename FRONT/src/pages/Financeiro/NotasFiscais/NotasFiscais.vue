<template>
    <div class="notasFiscais-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>

        <b-modal :no-close-on-backdrop="true" id="modalPagamentoNF" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" size="xl"
            v-model="modalPagamentoNF_Exibir">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Nota Fiscal</span>
                <h3 class="afm-hero__title">{{ modalPagamentoNF_titulo }}</h3>
                <p class="afm-hero__description">Calcule e registre os impostos e pagamentos da nota fiscal.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                    <b-row>
                        <b-col>
                            <table>
                                <tr>
                                    <td style="padding-right: 20px;">Valor Bruto da Nota Fiscal:</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_ValorBruto" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">Material Abatido:</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_MaterialAbatido" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">Base de Cálculo:</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_BaseCalculo" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-top: 10px;">
                                        <b-button variant="success" class="width-230 mb-3 mr-3"
                                            @click="calculaBaseDeCalculo()"><span>Calcular</span></b-button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">I.R.</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_IR" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_IRSelecionado" :multiple="false"
                                            :options="impostoIR" select-label="Selecionar" placeholder="Alíquota"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_IRBaseSelecionada" :multiple="false"
                                            :options="basePagamento" select-label="Selecionar" placeholder="Base"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                            v-model="modalPagamentoNF_IRDataPagamento" format="dd/MM/yyyy" type="date">
                                        </DatePickerMask>
                                    </td>
                                    <td style="padding-left: 10px; padding-top: 12px;">
                                        <b-button variant="success" class="width-230 mb-3 mr-3"
                                            @click="calculaIR()"><span>Calcular</span></b-button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">Art30</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_Art30" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_Art30Selecionado" :multiple="false"
                                            :options="impostoArt30" select-label="Selecionar" placeholder="Alíquota"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_Art30BaseSelecionada" :multiple="false"
                                            :options="basePagamentoArt30" select-label="Selecionar" placeholder="Base"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                            v-model="modalPagamentoNF_Art30DataPagamento" format="dd/MM/yyyy"
                                            type="date">
                                        </DatePickerMask>
                                    </td>
                                    <td style="padding-left: 10px; padding-top: 12px;">
                                        <b-button variant="success" class="width-230 mb-3 mr-3"
                                            @click="calculaArt30()"><span>Calcular</span></b-button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">INSS</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_INSS" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_INSSSelecionado" :multiple="false"
                                            :options="impostoINSS" select-label="Selecionar" placeholder="Alíquota"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_INSSBaseSelecionada" :multiple="false"
                                            :options="basePagamento" select-label="Selecionar" placeholder="Base"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                            v-model="modalPagamentoNF_INSSDataPagamento" format="dd/MM/yyyy"
                                            type="date">
                                        </DatePickerMask>
                                    </td>
                                    <td style="padding-left: 10px; padding-top: 12px;">
                                        <b-button variant="success" class="width-230 mb-3 mr-3"
                                            @click="calculaINSS()"><span>Calcular</span></b-button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;">ISS</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_ISS" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <Money v-model="impostoISS" v-bind="porcentagem" style="border-width: 0px;" />
                                    </td>
                                    <td style="padding-left: 10px">
                                        <multiselect v-model="modalPagamentoNF_ISSBaseSelecionada" :multiple="false"
                                            :options="basePagamento" select-label="Selecionar" placeholder="Base"
                                            label="descricao" track-by="descricao">
                                        </multiselect>
                                    </td>
                                    <td style="padding-left: 10px">
                                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                            v-model="modalPagamentoNF_ISSDataPagamento" format="dd/MM/yyyy" type="date"
                                            style="width: 100px;"></DatePickerMask>
                                    </td>
                                    <td style="padding-left: 10px; padding-top: 12px;">
                                        <b-button variant="success" class="width-230 mb-3 mr-3"
                                            @click="calculaISS()"><span>Calcular</span></b-button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Valor Líquido a Pagar:</td>
                                    <td>
                                        <Money v-model="modalPagamentoNF_ValorLiquido" v-bind="money"
                                            style="border-width: 0px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-right: 20px;"></td>
                                    <td></td>
                                </tr>
                            </table>
                        </b-col>
                    </b-row>
                </Widget>
              </section>
            </b-container>

            <template #modal-footer>
                <div class="afm-footer">
                  <div class="afm-footer__summary"></div>
                  <div class="afm-footer__actions">
                    <b-button @click="modalPagamentoNF_Exibir = false" variant="dark" class="mb-0 mr-2">
                      Fechar
                    </b-button>
                    <b-button variant="success" class="mb-0" @click="salvaPagamento();">
                      <i class="fa fa-save mr-1"></i> Salvar Pagamento
                    </b-button>
                  </div>
                </div>
            </template>
        </b-modal>

        <h1 class="page-title">Notas Fiscais
        </h1>
        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col>Obra<br /><br />
                    <multiselect v-model="obraSelecionada" :multiple="false" :options="obras" select-label="Selecionar"
                        placeholder="Selecione uma obra" label="descricao" track-by="descricao"
                        :custom-label="descricaoItemFiltro">
                    </multiselect>
                </b-col>
                <b-col>Pedido<br /><br />
                    <multiselect v-model="pedidoSelecionado" :multiple="false" :options="pedidos"
                        select-label="Selecionar" placeholder="Selecione um pedido" label="descricao"
                        track-by="descricao">
                    </multiselect>
                </b-col>
                <b-col>Status<br /><br />
                    <multiselect v-model="statusSelecionado" :multiple="false" :options="status"
                        select-label="Selecionar" placeholder="Selecione um status" label="descricao"
                        track-by="descricao">
                    </multiselect>
                </b-col>
            </b-row>

            <br />
            <b-row>
                <b-col>Pagamento<br /><br />
                    <multiselect v-model="pagamentoSelecionado" :multiple="false" :options="pagamento"
                        select-label="Selecionar" placeholder="Selecione uma opção" label="descricao"
                        track-by="descricao">
                    </multiselect>
                </b-col>
                <b-col>Fornecedor<br /><br />
                    <multiselect v-model="fornecedorSelecionado" :multiple="false" :options="fornecedores"
                        select-label="Selecionar" placeholder="Selecione um fornecedor" label="descricao"
                        track-by="descricao">
                    </multiselect>
                </b-col>
                <b-col>Número Nota<br /><br />
                    <b-form-input v-model="numeroNota" style="color: white;"></b-form-input>
                </b-col>
            </b-row>

            <br />
            <b-row>
                <b-col>
                    <b-row>
                        <b-col>
                            Data Vencimento Inicial<br /><br />

                            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="dataPagamentoInicial"
                                format="dd/MM/yyyy" type="date"></DatePickerMask>
                        </b-col>
                        <b-col>
                            Data Vencimento Final<br /><br />
                            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="dataPagamentoFinal"
                                format="dd/MM/yyyy" type="date"></DatePickerMask>
                        </b-col>
                    </b-row>
                </b-col>
                <b-col>
                </b-col>
                <b-col></b-col>
            </b-row>

            <br />

            <b-row>
                <b-col></b-col>
                <b-col></b-col>
                <b-col>

                </b-col>
                <b-col style="text-align: right;">
                    <button type="button" class="btn width-75 mb-3 mr-3 bg-danger"
                        @click="obtemNotasFiltradas(true);">Limpar
                        Filtros</button>
                    <button type="button" class="btn width-75 mb-3 mr-3 bg-success"
                        @click="obtemNotasFiltradas(false);">Pesquisar</button>
                </b-col>
            </b-row>
        </Widget>

        <br />

        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col>
                    <div class="table-card">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro">Nota Fiscal</th>
                                    <th class="estilo-cabecalho texto-centro">Pedido</th>
                                    <th class="estilo-cabecalho">Fornecedor</th>
                                    <th class="estilo-cabecalho">Obra</th>
                                    <th class="estilo-cabecalho texto-centro">Status</th>
                                    <th class="estilo-cabecalho texto-centro">Pagamento</th>
                                    <th class="estilo-cabecalho texto-centro">Valor</th>
                                    <th class="estilo-cabecalho texto-centro">Data Vencimento</th>
                                    <th class="estilo-cabecalho texto-centro">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in notasFiscais" :key="'nf-' + (row.id || index)">
                                    <td class="estilo-celula texto-centro">{{ row.numeroNota }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.pedido }}</td>
                                    <td class="estilo-celula">{{ row.fornecedor }}</td>
                                    <td class="estilo-celula">{{ row.obra }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.status }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.pagamento }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataVencimento) }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <button type="button" @click="downloadFile(row.idArquivo, row.nome)"
                                            class="btn width-75 mb-3 mr-3 bg-warning">
                                            <i class="fa fa-download" title="Download"></i>
                                        </button>
                                        <button type="button" @click="abrirModalPagamento(row)"
                                            class="btn width-75 mb-3 mr-3 bg-danger">
                                            <i class="fa fa-money" title="Impostos"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr v-if="!notasFiscais || notasFiscais.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
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

import Vue from "vue";
import moment from "moment";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import ToggleButton from "vue-js-toggle-button";
import ApiService from "@/services/api.service.js";
import Multiselect from "vue-multiselect";
import CurrencyInput from "../../../components/CurrencyInput.vue";
import { Money } from "v-money";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";
Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: 'NotasFiscais',
    components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask },
    data() {
        return {
            money: {
                decimal: ",",
                thousands: ".",
                prefix: "R$ ",
                precision: 2,
                masked: false,
            },

            decimalInput: {
                decimal: ",",
                thousands: ".",
                prefix: "",
                precision: 2,
                masked: false,
            },

            porcentagem: {
                decimal: ",",
                thousands: ".",
                prefix: "% ",
                precision: 2,
                masked: false,
            },

            dataMinima: (new Date()).setDate(new Date().getDate() -1),

            isLoading: false,

            numeroNota: '',

            notasFiscais: [],

            obraSelecionada: null,
            obras: [],

            pedidoSelecionado: null,
            pedidos: [],

            statusSelecionado: null,
            status: [{ id: 1, descricao: 'Pendente' }, { id: 2, descricao: 'Aprovada' }, { id: 3, descricao: 'Reprovada' }],

            pagamentoSelecionado: null,
            pagamento: [{ id: 1, descricao: 'Efetuado' }, { id: 2, descricao: 'Não efetuado' }],

            fornecedorSelecionado: null,
            fornecedores: [],

            modalPagamentoNF_IR: 0.0,
            modalPagamentoNF_IRSelecionado: null,
            modalPagamentoNF_IRBaseSelecionada: null,
            impostoIR: [{ id: 1, descricao: '1.0%', valor: 1.0 }, { id: 2, descricao: '1.5%', valor: 1.5 }],
            modalPagamentoNF_IRDataPagamento: null,

            modalPagamentoNF_Art30: 0.0,
            modalPagamentoNF_Art30Selecionado: null,
            modalPagamentoNF_Art30BaseSelecionada: null,
            impostoArt30: [{ id: 1, descricao: '4,65%', valor: 4.65 }],
            modalPagamentoNF_Art30DataPagamento: null,

            modalPagamentoNF_INSS: 0.0,
            modalPagamentoNF_INSSSelecionado: null,
            modalPagamentoNF_INSSBaseSelecionada: null,
            impostoINSS: [{ id: 1, descricao: '3,5%', valor: 3.5 }, { id: 2, descricao: '11%', valor: 11 }],
            modalPagamentoNF_INSSDataPagamento: null,

            modalPagamentoNF_ISS: 0.0,
            modalPagamentoNF_ISSBaseSelecionada: null,
            impostoISS: 1.0,
            modalPagamentoNF_ISSDataPagamento: null,

            basePagamento: [{ id: 1, descricao: 'Bruto' }, { id: 2, descricao: 'Base Cálculo' }],
            basePagamentoArt30: [{ id: 1, descricao: 'Bruto' }],
            dataPagamentoInicial: new Date(Date.now()),
            dataPagamentoFinal: null,

            modalPagamentoNF_Exibir: false,
            modalPagamentoNF_titulo: '',
            modalPagamentoNF_ValorBruto: 0.0,
            modalPagamentoNF_MaterialAbatido: 0.0,
            modalPagamentoNF_BaseCalculo: 0.0,
            modalPagamentoNF_ValorLiquido: 0.0,
            modalPagamentoNF_Id: 0,
            modalPagamento_NotaFiscal: null,

            colunasNotasFiscais: ["numeroNF", "pedido", "fornecedor", "obra", "status", "pagamento", "valor", "dataVencimento", "acoes"],

            opcoesNotasFiscais: {
                perPage: 1000,
                filterable: false,
                headings: {
                    numeroNF: 'Nota Fiscal',
                    pedido: 'Pedido',
                    fornecedor: 'Fornecedor',
                    obra: 'Obra',
                    status: 'Status',
                    pagamento: 'Pagamento',
                    Valor: 'Valor',
                    dataVencimento: 'Data Vencimento',
                    acoes: 'Ações'
                },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: 'Procurar por',
                    count:
                        'Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item',
                    first: 'Primeiro',
                    last: 'último',
                    filter: '',
                    limit: 'Itens:',
                    page: 'Página:',
                    noResults: 'Não encontrado',
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

        descricaoItemFiltro({ descricao }) {
            return `${descricao}`;
        },

        salvaPagamento() {
            this.isLoading = true;

            var objPagamento = {
                id: this.modalPagamentoNF_Id,
                idPedidoCompraNotaFiscal: this.modalPagamento_NotaFiscal.id,
                valorBruto: this.modalPagamentoNF_ValorBruto,
                valorMaterialAbatido: this.modalPagamentoNF_MaterialAbatido,
                valorBaseCalculo: this.modalPagamentoNF_BaseCalculo,
                aliquotaIR: this.modalPagamentoNF_IRSelecionado != null ? this.modalPagamentoNF_IRSelecionado.valor : null,
                aliquotaArt30: this.modalPagamentoNF_Art30Selecionado != null ? this.modalPagamentoNF_Art30Selecionado.valor : null,
                aliquotaINSS: this.modalPagamentoNF_INSSSelecionado != null ? this.modalPagamentoNF_INSSSelecionado.valor : null,
                aliquotaISS: this.impostoISS,
                valorIR: this.modalPagamentoNF_IR,
                valorArt30: this.modalPagamentoNF_Art30,
                valorINSS: this.modalPagamentoNF_INSS,
                valorISS: this.modalPagamentoNF_ISS,
                idTipoCalculoNotaFiscalIR: this.modalPagamentoNF_IRBaseSelecionada != null ? this.modalPagamentoNF_IRBaseSelecionada.id : null,
                idTipoCalculoNotaFiscalArt30: this.modalPagamentoNF_Art30BaseSelecionada != null ? this.modalPagamentoNF_Art30BaseSelecionada.id : null,
                idTipoCalculoNotaFiscalINSS: this.modalPagamentoNF_INSSBaseSelecionada != null ? this.modalPagamentoNF_INSSBaseSelecionada.id : null,
                idTipoCalculoNotaFiscalISS: this.modalPagamentoNF_ISSBaseSelecionada != null ? this.modalPagamentoNF_ISSBaseSelecionada.id : null,
                dataPagamentoIR: this.modalPagamentoNF_IRDataPagamento,
                dataPagamentoArt30: this.modalPagamentoNF_Art30DataPagamento,
                dataPagamentoINSS: this.modalPagamentoNF_INSSDataPagamento,
                dataPagamentoISS: this.modalPagamentoNF_ISSDataPagamento
            };

            ApiService.postNotaFiscalPagamento(objPagamento, (result) => {
                this.isLoading = false;

                if (result.status == 200) {
                    this.$swal("", "Valores informados !!!", "success");
                    this.modalPagamentoNF_Exibir = false
                } else {
                    this.$swal("", result.message, "error");

                }
            });
        },

        abrirModalPagamento(notaFiscal) {

            this.loading = true;

            ApiService.getNotaFiscalPagamento(notaFiscal.id, (result) => {
                this.loading = false;

                if (result.status == 200) {
                    var valores = result.data;

                    this.modalPagamentoNF_IR = valores.valorIR;
                    this.impostoIR.forEach(x => {
                        if (x.valor == valores.aliquotaIR)
                            this.modalPagamentoNF_IRSelecionado = x;
                    });
                    this.basePagamento.forEach(x => {
                        if (x.id == valores.idTipoCalculoNotaFiscalIR)
                            this.modalPagamentoNF_IRBaseSelecionada = x;
                    });
                    this.modalPagamentoNF_IRDataPagamento = valores.dataPagamentoIR;

                    this.modalPagamentoNF_Art30 = valores.valorArt30;
                    this.impostoArt30.forEach(x => {
                        if (x.valor == valores.aliquotaArt30)
                            this.modalPagamentoNF_Art30Selecionado = x;
                    });
                    this.basePagamentoArt30.forEach(x => {
                        if (x.id == valores.idTipoCalculoNotaFiscalArt30)
                            this.modalPagamentoNF_Art30BaseSelecionada = x;
                    });
                    this.modalPagamentoNF_Art30DataPagamento = valores.dataPagamentoArt30;

                    this.modalPagamentoNF_INSS = valores.valorINSS;
                    this.impostoINSS.forEach(x => {
                        if (x.valor == valores.aliquotaINSS)
                            this.modalPagamentoNF_INSSSelecionado = x;
                    });
                    this.basePagamento.forEach(x => {
                        if (x.id == valores.idTipoCalculoNotaFiscalINSS)
                            this.modalPagamentoNF_INSSBaseSelecionada = x;
                    });
                    this.modalPagamentoNF_INSSDataPagamento = valores.dataPagamentoINSS;

                    this.modalPagamentoNF_ISS = valores.valorISS;
                    this.impostoISS = valores.aliquotaISS;
                    this.basePagamento.forEach(x => {
                        if (x.id == valores.idTipoCalculoNotaFiscalISS)
                            this.modalPagamentoNF_ISSBaseSelecionada = x;
                    });
                    this.modalPagamentoNF_ISSDataPagamento = valores.dataPagamentoISS;

                    this.modalPagamentoNF_ValorBruto = valores.valorBruto;
                    this.modalPagamentoNF_MaterialAbatido = valores.valorMaterialAbatido;
                    this.modalPagamentoNF_BaseCalculo = valores.valorBaseCalculo;
                    this.modalPagamentoNF_ValorLiquido = valores.valorLiquido;

                    this.modalPagamentoNF_Id = valores.id;

                    this.calculaBaseDeCalculo();

                    this.modalPagamentoNF_Exibir = true;


                } else {
                    this.modalPagamentoNF_IR = 0.0;
                    this.modalPagamentoNF_IRSelecionado = null;
                    this.modalPagamentoNF_IRBaseSelecionada = null;

                    this.modalPagamentoNF_Art30 = 0.0;
                    this.modalPagamentoNF_Art30Selecionado = null;
                    this.modalPagamentoNF_Art30BaseSelecionada = null;

                    this.modalPagamentoNF_INSS = 0.0;
                    this.modalPagamentoNF_INSSSelecionado = null;
                    this.modalPagamentoNF_INSSBaseSelecionada = null;

                    this.modalPagamentoNF_ISS = 0.0;
                    this.modalPagamentoNF_ISSBaseSelecionada = null;
                    this.impostoISS = 1.0;

                    this.modalPagamentoNF_ValorBruto = notaFiscal.valor;
                    this.modalPagamentoNF_MaterialAbatido = 0.0;
                    this.modalPagamentoNF_BaseCalculo = notaFiscal.valor;
                    this.modalPagamentoNF_ValorLiquido = notaFiscal.valor;

                    this.modalPagamentoNF_Id = 0;

                    this.modalPagamentoNF_Exibir = true;
                }

                this.modalPagamento_NotaFiscal = notaFiscal;
            });


        },

        calculaBaseDeCalculo() {
            this.modalPagamentoNF_BaseCalculo = this.modalPagamentoNF_ValorBruto - this.modalPagamentoNF_MaterialAbatido;
            this.calculaValorLiquido();
        },

        calculaIR() {
            if (this.modalPagamentoNF_IRSelecionado != null && this.modalPagamentoNF_IRBaseSelecionada != null) {
                if (this.modalPagamentoNF_IRBaseSelecionada.id == 1)
                    this.modalPagamentoNF_IR = (this.modalPagamentoNF_ValorBruto / 100) * this.modalPagamentoNF_IRSelecionado.valor;
                else
                    this.modalPagamentoNF_IR = (this.modalPagamentoNF_BaseCalculo / 100) * this.modalPagamentoNF_IRSelecionado.valor;
            }
            else
                this.modalPagamentoNF_IR = 0;

            this.calculaValorLiquido();
        },

        calculaArt30() {
            if (this.modalPagamentoNF_Art30Selecionado != null && this.modalPagamentoNF_Art30BaseSelecionada != null) {
                if (this.modalPagamentoNF_Art30BaseSelecionada.id == 1)
                    this.modalPagamentoNF_Art30 = (this.modalPagamentoNF_ValorBruto / 100) * this.modalPagamentoNF_Art30Selecionado.valor;
                else
                    this.modalPagamentoNF_Art30 = (this.modalPagamentoNF_BaseCalculo / 100) * this.modalPagamentoNF_Art30Selecionado.valor;
            }
            else
                this.modalPagamentoNF_Art30 = 0;

            this.calculaValorLiquido();
        },

        calculaINSS() {
            if (this.modalPagamentoNF_INSSSelecionado != null && this.modalPagamentoNF_INSSBaseSelecionada != null) {
                if (this.modalPagamentoNF_INSSBaseSelecionada.id == 1)
                    this.modalPagamentoNF_INSS = (this.modalPagamentoNF_ValorBruto / 100) * this.modalPagamentoNF_INSSSelecionado.valor;
                else
                    this.modalPagamentoNF_INSS = (this.modalPagamentoNF_BaseCalculo / 100) * this.modalPagamentoNF_INSSSelecionado.valor;
            }
            else
                this.modalPagamentoNF_INSS = 0;

            this.calculaValorLiquido();
        },

        calculaISS() {
            if (this.modalPagamentoNF_ISSBaseSelecionada != null) {
                if (this.modalPagamentoNF_ISSBaseSelecionada.id == 1)
                    this.modalPagamentoNF_ISS = (this.modalPagamentoNF_ValorBruto / 100) * this.impostoISS;
                else
                    this.modalPagamentoNF_ISS = (this.modalPagamentoNF_BaseCalculo / 100) * this.impostoISS;
            }
            else
                this.modalPagamentoNF_ISS = 0;

            this.calculaValorLiquido();
        },

        calculaValorLiquido() {
            this.modalPagamentoNF_ValorLiquido = this.modalPagamentoNF_ValorBruto - this.modalPagamentoNF_IR - this.modalPagamentoNF_Art30 - this.modalPagamentoNF_INSS - this.modalPagamentoNF_ISS;
        },

        listaItens(item) {
            ApiService.getItemFiltro(item, (result) => {
                if (result.status != 200) {

                } else {
                    if (item == "Obra")
                        this.obras = result.data;
                    else if (item == "Pedido")
                        this.pedidos = result.data;
                    else if (item == "Fornecedor")
                        this.fornecedores = result.data;
                }
            });
        },

        obtemNotasFiltradas(limpaFiltro) {

            this.loading = true;

            var objetoFiltro = {
                idObra: null,
                idPedido: null,
                idFornecedor: null,
                idStatus: null,
                idPagamento: null,
                dataPagamentoInicial: null,
                dataPagamentoFinal: null,
                numeroNotaFiscal: ''
            };

            if (!limpaFiltro) {
                objetoFiltro.numeroNotaFiscal = this.numeroNota;

                if (this.obraSelecionada != null)
                    objetoFiltro.idObra = this.obraSelecionada.id;

                if (this.pedidoSelecionado != null)
                    objetoFiltro.idPedido = this.pedidoSelecionado.id;

                if (this.fornecedorSelecionado != null)
                    objetoFiltro.idFornecedor = this.fornecedorSelecionado.id;

                if (this.statusSelecionado != null)
                    objetoFiltro.idStatus = this.statusSelecionado.id;

                if (this.pagamentoSelecionado != null)
                    objetoFiltro.idPagamento = this.pagamentoSelecionado.id;

                if (this.dataPagamentoInicial != null)
                    objetoFiltro.dataPagamentoInicial = this.dataPagamentoInicial;

                if (this.dataPagamentoFinal != null)
                    objetoFiltro.dataPagamentoFinal = this.dataPagamentoFinal;
            }
            else {
                this.obraSelecionada = null;
                this.pedidoSelecionado = null;
                this.statusSelecionado = null;
                this.pagamentoSelecionado = null;
                this.fornecedorSelecionado = null;
                this.dataPagamentoInicial = new Date(Date.now());;
                this.dataPagamentoFinal = null;
                this.numeroNota = '';

                objetoFiltro.dataPagamentoInicial = this.dataPagamentoInicial;
            }

            ApiService.obtemNotasFiltradas(objetoFiltro, (result) => {
                this.loading = false;
                if (result.status != 200) {

                } else {
                    this.notasFiscais = result.data;
                }
            });

        },

        downloadFile(id, nomeArquivo) {
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
    },
    mounted() {
        this.listaItens("Obra");
        this.listaItens("Pedido");
        this.listaItens("Fornecedor");
    },
};
</script>

<style src="./NotasFiscais.scss" lang="scss" />