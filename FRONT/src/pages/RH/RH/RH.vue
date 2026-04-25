<template>
    <div class="rh-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">RH
        </h1>

        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col md="2">
                    <label class="mr-3">Mês / Ano:</label><br />
                    <b-form-input v-model="filtro_MesSelecionado" style="color: white" placeholder="MM/AAAA"
                        v-mask="'##/####'"></b-form-input>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Funcionário:</label><br />
                    <multiselect v-model="filtro_FuncionarioSelecionado" :multiple="false" :options="funcionarios"
                        select-label="Selecionar" placeholder="Selecione um Funcionário" label="nome" track-by="nome">
                    </multiselect>
                </b-col>
                <b-col md="2">

                </b-col>
                <b-col md="2">

                </b-col>
                <b-col md="4">

                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4">

                </b-col>
                <b-col md="4"></b-col>
                <b-col md="4" style="text-align: right;">

                    <button @click="listaFaturamentos()" v-b-modal.cadastro type="button"
                        class="btn width-120 mb-3 mr-4 btn-success">
                        Pesquisar
                    </button>
                </b-col>
            </b-row>
        </Widget>

        <Widget customHeader class="estiloWidget">

            <div class="table-card">
                <table class="estilo-tabela tabela-identidade">
                    <thead>
                        <tr>
                            <th class="estilo-cabecalho">Funcionário</th>
                            <th class="estilo-cabecalho texto-centro">Mês Ref.</th>
                            <th class="estilo-cabecalho texto-centro">Salário</th>
                            <th class="estilo-cabecalho texto-centro">Salário HE</th>
                            <th class="estilo-cabecalho texto-centro">Salário Desconto</th>
                            <th class="estilo-cabecalho texto-centro">Salário Comissão</th>
                            <th class="estilo-cabecalho texto-centro">VR WH</th>
                            <th class="estilo-cabecalho texto-centro">VR Func.</th>
                            <th class="estilo-cabecalho texto-centro">VT WH</th>
                            <th class="estilo-cabecalho texto-centro">Ass. Médica WH</th>
                            <th class="estilo-cabecalho texto-centro">Ass. Médica Func.</th>
                            <th class="estilo-cabecalho texto-centro">Ass. Odont. WH</th>
                            <th class="estilo-cabecalho texto-centro">Ass. Odont. Func.</th>
                            <th class="estilo-cabecalho texto-centro">Seguro WH</th>
                            <th class="estilo-cabecalho texto-centro">Seguro Func.</th>
                            <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(row, index) in rhValores" :key="'rh-' + (row.id || index)">
                            <td class="estilo-celula">{{ row.funcionario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.mesReferencia }}</td>
                            <td class="estilo-celula texto-centro">{{ row.salario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.salarioHoraExtra }}</td>
                            <td class="estilo-celula texto-centro">{{ row.salarioDesconto }}</td>
                            <td class="estilo-celula texto-centro">{{ row.salarioComissao }}</td>
                            <td class="estilo-celula texto-centro">{{ row.valeRefeicaoWH }}</td>
                            <td class="estilo-celula texto-centro">{{ row.valeRefeicaoFuncionario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.valeTransporteWH }}</td>
                            <td class="estilo-celula texto-centro">{{ row.assistenciaMedicaWH }}</td>
                            <td class="estilo-celula texto-centro">{{ row.assistenciaMedicaFuncionario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.assistenciaOdontologicaWH }}</td>
                            <td class="estilo-celula texto-centro">{{ row.assistenciaOdontologicaFuncionario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.seguroWH }}</td>
                            <td class="estilo-celula texto-centro">{{ row.seguroFuncionario }}</td>
                            <td class="estilo-celula texto-centro">{{ row.acoes }}</td>
                        </tr>
                        <tr v-if="!rhValores || rhValores.length === 0">
                            <td class="estilo-celula texto-centro" colspan="16">Nenhum registro encontrado</td>
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
import Multiselect from "vue-multiselect";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: 'RH',
    components: { Widget, Loading, CurrencyInput, Multiselect },
    data() {
        return {
            isLoading: false,

            usuarioLogado: null,

            rhValores: [],

            filtro_FuncionarioSelecionado: null,
            filtro_MesSelecionado: '',

            funcionarios: [],


            columns: ["funcionario", "mesReferencia", "salario", "salarioHoraExtra", "salarioDesconto", "salarioComissao", "valeRefeicaoWH", "valeRefeicaoFuncionario", "valeTransporteWH", "assistenciaMedicaWH", "assistenciaMedicaFuncionario", "assistenciaOdontologicaWH", "assistenciaOdontologicaFuncionario", "seguroWH", "seguroFuncionario", "acoes"],

            options: {
                perPage: 100,
                headings: {
                    funcionario: "Funcionário",
                    mesReferencia: "Mês Ref.",
                    salario: "Salário",
                    salarioHoraExtra: "Salário HE",
                    salarioDesconto: "Salário Desconto",
                    salarioComissao: "Salário Comissão",
                    valeRefeicaoWH: "VR WH",
                    valeRefeicaoFuncionario: "VR Func.",
                    valeTransporteWH: "VT WH",
                    assistenciaMedicaWH: "Ass. Médica WH",
                    assistenciaMedicaFuncionario: "Ass. Médica Func.",
                    assistenciaOdontologicaWH: "Ass. Odont. WH",
                    assistenciaOdontologicaFuncionario: "Ass. Odont. Func.",
                    seguroWH: "Seguro WH",
                    seguroFuncionario: "Seguro Func.",
                    acoes: "Ações"
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
        listaFuncionarios: function () {
            this.filtro_Clientes = [];

            ApiService.getAll("Funcionario", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.funcionarios = result.data;
                }
            });
        },
    },
    mounted() {
        this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaFuncionarios();
    },
};
</script>
  
  <style src="./RH.scss" lang="scss" />
  