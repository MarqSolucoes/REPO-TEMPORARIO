<template>
    <div class="RelatorioMaterial-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Relatório de Material
        </h1>

        <Widget customHeader class="estiloWidget">
            <b-row>

                <b-col md="8">
                    <label class="mr-3">Material:</label>
                    <multiselect v-model="filtro_MaterialSelecionado" :multiple="false" :options="materiais"
                        select-label="Selecionar" placeholder="Selecione um ou mais materiais" label="descricao"
                        track-by="descricao">
                    </multiselect>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4" style="text-align: left;">

                    <button @click="obtemHistoricoDeCompra()" v-b-modal.cadastro type="button"
                        class="btn width-120 mb-3 mr-4 btn-success">
                        Pesquisar
                    </button>

                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="12">
                    <div class="table-card">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho">Obra</th>
                                    <th class="estilo-cabecalho">Cliente</th>
                                    <th class="estilo-cabecalho texto-centro">Pedido</th>
                                    <th class="estilo-cabecalho">Fornecedor</th>
                                    <th class="estilo-cabecalho">Material</th>
                                    <th class="estilo-cabecalho texto-centro">Data</th>
                                    <th class="estilo-cabecalho texto-centro">Quantidade Pedida</th>
                                    <th class="estilo-cabecalho texto-centro">Quantidade Conciliada</th>
                                    <th class="estilo-cabecalho texto-centro">Valor</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in historicoDeCompra" :key="'hc-' + (row.id || index)">
                                    <td class="estilo-celula">{{ row.obra }}</td>
                                    <td class="estilo-celula">{{ row.cliente }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.pedido }}</td>
                                    <td class="estilo-celula">{{ row.fornecedor }}</td>
                                    <td class="estilo-celula">{{ row.material }}</td>
                                    <td class="estilo-celula texto-centro">{{ formataData(row.data) }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.quantidadePedida }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.quantidadeConciliada }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                                </tr>
                                <tr v-if="!historicoDeCompra || historicoDeCompra.length === 0">
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
    name: 'RelatorioMaterial',
    components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask },
    data() {
        return {

            usuarioDTO: null,
            isLoading: false,

            filtro_MaterialSelecionado: null,
            materiais: [],

            historicoDeCompra: [],

            fields: [
                { key: "obra", label: "Obra" },
                { key: "cliente", label: "Cliente" },
                { key: "pedido", label: "Pedido" },
                { key: "fornecedor", label: "Fornecedor" },
                { key: "material", label: "Material" },
                { key: "dataConciliacao", label: "Data" },
                { key: "quantidadePedida", label: "Quantidade Pedida" },
                { key: "quantidadeConciliada", label: "Quantidade Conciliada" },
                { key: "valor", label: "Valor" },
            ],

            options: {
                perPage: 10,
            },
        }
    },

    methods: {

        formataData: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
        },

        listaMateriais: function () {
            this.materiais = [];

            ApiService.getAll("Material", true, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.materiais = result.data;
                }
            });
        },

        obtemHistoricoDeCompra() {

            if (this.filtro_MaterialSelecionado == null) {
                this.$swal("Erro", "Material não selecionado", "error");
                return;
            }

            let idMaterial = this.filtro_MaterialSelecionado.id;

            ApiService.obtemHistoricoDeCompra(idMaterial, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.historicoDeCompra = result.data;

                    console.log(this.historicoDeCompra);
                }
            });
        },

    },
    mounted() {
        this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaMateriais();
    }
}
</script>
