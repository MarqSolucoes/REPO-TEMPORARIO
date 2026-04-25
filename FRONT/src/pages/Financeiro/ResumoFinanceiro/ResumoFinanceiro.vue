<template>
    <div class="resumoFinanceiro-page">
        <h1 class="page-title">Resumo Financeiro
        </h1>
        <b-row>
            <b-col lg="12">

            </b-col>
        </b-row>
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
import pdf from 'vue-pdf'

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: 'ResumoFinanceiro',
    components: { Widget, Loading, CurrencyInput, pdf },
    data() {
        return {};
    },
    methods: {
        downloadPDF() {
            this.isLoading = true;

            ApiService.downloadPdfRelatorioFinanceiro((result) => {
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
        },
    },
    mounted() {
        this.downloadPDF();
     },
};
</script>
  
  <style src="./ResumoFinanceiro.scss" lang="scss" />
  