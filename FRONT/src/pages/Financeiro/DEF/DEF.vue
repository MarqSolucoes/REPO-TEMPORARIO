<template>
  <div class="def-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">DEF
    </h1>
    <Widget customHeader class="estiloWidget">

      <div class="table-card">
        <table class="estilo-tabela tabela-identidade">
          <thead>
            <tr>
              <th class="estilo-cabecalho texto-centro">Código</th>
              <th class="estilo-cabecalho">Descrição</th>
              <th class="estilo-cabecalho texto-centro">Abrir Pedido Interno</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in defs" :key="'def-' + (row.id || index)">
              <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
              <td class="estilo-celula">{{ row.descricao }}</td>
              <td class="estilo-celula texto-centro">{{ row.podeAbrirPedidoInterno == true ? 'Sim' : 'Não' }}</td>
              <td class="estilo-celula texto-centro">
                <button type="button" v-if="row.podeAbrirPedidoInterno" @click="habilitarDesabilitarPedidoInterno(row.id, false)"
                  class="btn width-120 mb-3 mr-3 btn-outline-danger">
                  Desabilitar PI
                </button>
                <button type="button" v-else @click="habilitarDesabilitarPedidoInterno(row.id, true)"
                  class="btn width-120 mb-3 mr-3 btn-outline-success">
                  Habilitar PI
                </button>
              </td>
            </tr>
            <tr v-if="!defs || defs.length === 0">
              <td class="estilo-celula texto-centro" colspan="4">Nenhum registro encontrado</td>
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
  name: 'DEF',
  components: { Widget, Loading, CurrencyInput },
  data() {
    return {
      isLoading: false,

      defs: [],

      columns: ["codigo", "descricao", "pedidoInterno", "acoes"],

      options: {
        perPage: 100,
        headings: {
          codigo: "Código",
          descricao: "Descrição",
          pedidoInterno: "Abrir Pedido Interno",
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
    listaDefs: function () {
      this.defs = [];

      ApiService.getAll("DEF", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.defs = result.data;
        }
      });
    },

    habilitarDesabilitarPedidoInterno: function (idDef, ativo) {
      ApiService.habilitarDesabilitarPedidoInterno(
        "DEF",
        idDef,
        ativo,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao habilitar/desabilitar pedido interno",
              result.message,
              "error"
            );
          } else {
            this.listaDefs();
          }
        }
      );
    },
  },
  mounted() {
    this.listaDefs();
  },
};
</script>

<style src="./DEF.scss" lang="scss" />
