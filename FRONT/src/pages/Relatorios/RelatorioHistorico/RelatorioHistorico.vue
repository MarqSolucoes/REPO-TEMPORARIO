<template>
    <div class="RelatorioHistorico-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Histórico
        </h1>

        <Widget customHeader class="estiloWidget">
            <Widget customHeader class="estiloWidget">
                <b-row>
                    <b-col md="4">
                        <label class="mr-3">Escolha o tipo:</label><br />
                        <multiselect v-model="filtro_TipoHistorico" :multiple="false" :options="tipoHistorico"
                            select-label="Selecionar" placeholder="Selecione uma opção" label="descricao"
                            track-by="descricao" @input="onTipoHistoricoSelecionado">
                        </multiselect>
                    </b-col>
                    <b-col md="4">
                        <label class="mr-3">Código:</label><br />
                        <!-- <multiselect v-model="filtro_Codigo" :multiple="false" :options="codigos"
                            select-label="Selecionar" placeholder="Selecione uma opção" @input="buscaHistorico">
                        </multiselect> -->
                        <b-form-input v-model="filtro_Codigo" style="color: white"></b-form-input>
                    </b-col>
                    <b-col md="4">

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
                        <button @click="buscaHistorico()" v-b-modal.cadastro type="button"
                            class="btn width-120 mb-3 mr-4 btn-success">
                            Pesquisar
                        </button>

                    </b-col>
                </b-row>
            </Widget>

            <div class="table-card">
                <table class="estilo-tabela tabela-identidade">
                    <thead>
                        <tr>
                            <th class="estilo-cabecalho texto-centro">SC/OC/PI</th>
                            <th class="estilo-cabecalho texto-centro">Data</th>
                            <th class="estilo-cabecalho">Informação</th>
                            <th class="estilo-cabecalho">Valor Antigo</th>
                            <th class="estilo-cabecalho">Valor Novo</th>
                            <th class="estilo-cabecalho">Usuário</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(row, index) in historico" :key="'h-' + index">
                            <td class="estilo-celula texto-centro">{{ row.valorSolicitacaoOuPedidoOuPI }}</td>
                            <td class="estilo-celula texto-centro">{{ formataData(row.data) }}</td>
                            <td class="estilo-celula">{{ row.campo }}</td>
                            <td class="estilo-celula">{{ row.valorAntigo }}</td>
                            <td class="estilo-celula">{{ row.valorNovo }}</td>
                            <td class="estilo-celula">{{ row.usuario }}</td>
                        </tr>
                        <tr v-if="!historico || historico.length === 0">
                            <td class="estilo-celula texto-centro" colspan="6">Nenhum registro encontrado</td>
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
    name: 'RelatorioPedidosCompra',
    components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask },
    data() {
        return {
            money: {
                decimal: ',',
                thousands: '.',
                prefix: 'R$ ',
                precision: 2,
                masked: false,
            },

            number: {
                decimal: ',',
                thousands: '.',
                prefix: '',
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

            dataMinima: (new Date()).setDate(new Date().getDate() - 1),

            open: false,
            isLoading: false,

            filtro_TipoHistorico: null,
            filtro_Codigo: '',
            tipoHistorico: [{ id: 1, descricao: 'Solicitação de Compra' }, { id: 2, descricao: 'Pedido de Compra' }, { id: 3, descricao: 'Pedido Interno' }],

            historico: [],
            codigos: [],

            colunasHistorico: [
                "scocpi",
                "data",
                "campo",
                "valorAntigo",
                "valorNovo",
                "usuario",
            ],

            opcoesHistorico: {
                perPage: 1000,
                headings: {
                    scocpi: "SC/OC/PI",
                    data: "Data",
                    campo: "Informação",
                    valorAntigo: "Valor Antigo",
                    valorNovo: "Valor Novo",
                    usuario: "Usuário",
                },
                clientSorting: true,
                sortable: [],
                filterable: false,
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
        }
    },

    methods: {

        formataData: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
        },

        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        limpaFiltros() {
            this.filtro_TipoHistorico = null;
            this.filtro_Codigo = '';
            this.codigos=[];
        },

        onTipoHistoricoSelecionado(selectedOption) {
            // if (selectedOption != null) {

            //     this.isLoading = true;

            //     ApiService.obtemCodigosHistorico(selectedOption.id, (result) => {
            //     this.isLoading = false;

            //     if (result.status != 200) {
            //         this.$swal("", result.data, "error");
            //     } else {

            //         this.codigos = result.data;
            //     }
            // });
            // }
            // else{
            //     this.codigos=[];
            //     this.filtro_Codigo='';
            // }
        },

        buscaHistorico() {
            if (this.filtro_TipoHistorico == null) {
                this.$swal("", "Informe o tipo", "error");
                return;
            }

            var objeto = {
                solicitacaoCompra: false,
                pedidoCompra: false,
                pedidoInterno: false,
                codigo: this.filtro_Codigo
            };

            if (this.filtro_TipoHistorico.id == 1) {
                objeto.solicitacaoCompra = true;
            }
            else if (this.filtro_TipoHistorico.id == 2) {
                objeto.pedidoCompra = true;
            }
            else if (this.filtro_TipoHistorico.id == 3) {
                objeto.pedidoInterno = true;
            }

            ApiService.obtemHistorico(objeto, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.data, "error");
                } else {

                    this.historico = result.data;
                    console.log(this.historico);
                }
            });

        }
    },
    mounted() {
        this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'));

    }
}
</script>
