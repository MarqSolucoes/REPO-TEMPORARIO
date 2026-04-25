<template>
    <div class="relatorioETO-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Ajuste de ETO
        </h1>

        <b-modal :no-close-on-backdrop="true" id="eto" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalETO_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">ETO</span>
                <h3 class="afm-hero__title">{{ modalETO_Titulo }}</h3>
                <p class="afm-hero__description">Cadastre a data prevista e o valor ajustado do ETO extra.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <b-row>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Data prevista</label>
                      <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalETO_DataPrevista" format="dd/MM/yyyy"
                        type="date" :open.sync="open">
                      </DatePickerMask>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Valor previsto ajustado</label>
                      <Money v-model="modalETO_ValorPrevistoAjustado" v-bind="money"></Money>
                    </div>
                  </b-col>
                </b-row>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalETO_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-on:click="cadastrarETOExtra()" variant="success" class="mb-0">
                    <i class="fa fa-save mr-1"></i> Cadastrar
                  </b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="etoEdicao" class="modal-dialog modal-md afm-modal"
          body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
          v-model="modalETOEdicao_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">ETO</span>
              <h3 class="afm-hero__title">{{ modalETOEdicao_Titulo }}</h3>
              <p class="afm-hero__description">Edite o valor ajustado do ETO.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor previsto ajustado</label>
                    <Money v-model="modalETOEdicao_ValorPrevistoAjustado" v-bind="money"></Money>
                  </div>
                </b-col>
              </b-row>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalETOEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">
                  Cancelar
                </b-button>
                <b-button v-on:click="editarETO()" variant="info" class="mb-0">
                  <i class="fa fa-save mr-1"></i> Editar
                </b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalAjuste" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
            header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalAjuste_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">ETO</span>
          <h3 class="afm-hero__title">{{ modalAjuste_Titulo }}</h3>
          <p class="afm-hero__description">Ajuste os valores previstos dos ETOs e cadastre entradas extras.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">



                <b-row>
                    <b-col lg="12" xs="12">
                        <Widget customHeader class="estiloWidget">

                            <b-button v-on:click="edicaoValorTotalETO = true" v-if="edicaoValorTotalETO == false"
                                variant="outline-info" class="width-200 mb-3 mr-3">
                                <span>Editar Valor Total</span>
                            </b-button>
                            <b-button
                                v-on:click="modalETO_DataPrevista = null; modalETO_ValorPrevistoAjustado = 0; modalETO_Exibir = true;"
                                v-if="edicaoValorTotalETO" variant="outline-success" class="width-200 mb-3 mr-3">
                                <span>Novo ETO Extra</span>
                            </b-button>

                            <br />

                            <div v-if="edicaoValorTotalETO">
                                <label class="mr-3">Valor Total Ajustado:</label>
                                <Money v-model="obra_ValorCustoAjustado" v-bind="money" >
                                </Money>

                                <br /><br />

                                <label class="mr-3">Somatória ETOs ajustados: {{ new Intl.NumberFormat("pt-BR", {
                                    style: "currency",
                                    currency: "BRL",
                                }).format(somatoriaValoresAjustadosETOs) }}</label>

                                <br /><br />

                                <label class="mr-3">Saldo ETOs ajustados: {{ new Intl.NumberFormat("pt-BR", {
                                    style: "currency", currency:
                                        "BRL",
                                }).format(this.obra_ValorCustoAjustado - somatoriaValoresAjustadosETOs) }}</label>
                            </div>

                            <div class="table-card table-card--fluid">
                                <table class="estilo-tabela tabela-identidade">
                                    <thead>
                                        <tr>
                                            <th class="estilo-cabecalho texto-centro">Dt. Prevista</th>
                                            <th class="estilo-cabecalho texto-centro">Previsto (R$)</th>
                                            <th class="estilo-cabecalho texto-centro">Previsto Ajustado (R$)</th>
                                            <th class="estilo-cabecalho texto-centro">Medido (R$)</th>
                                            <th class="estilo-cabecalho texto-centro">Ações</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="(row, index) in (obra != null ? obra.controleCusto : [])" :key="'cc-' + (row.id || index)">
                                            <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevista) }}</td>
                                            <td class="estilo-celula texto-centro">{{ row.valorPrevisto != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevisto) : '' }}</td>
                                            <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevistoAjustado) }}</td>
                                            <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMedido) }}</td>
                                            <td class="estilo-celula texto-centro">
                                                <b-button v-if="edicaoValorTotalETO == true && new Date() < new Date(row.dataPrevista)"
                                                    v-on:click="abreModalEdicaoETO(row)" variant="outline-warning"
                                                    class="width-40 mb-3 mr-3">
                                                    <i class="fa fa-pencil" title="Editar"></i>
                                                </b-button>

                                                <b-button v-if="edicaoValorTotalETO == true && row.valorPrevisto == null"
                                                    v-on:click="excluiETO(row)" variant="outline-danger" class="width-40 mb-3 mr-3">
                                                    <i class="fa fa-close" title="Excluir"></i>
                                                </b-button>
                                            </td>
                                        </tr>
                                        <tr v-if="!obra || !obra.controleCusto || obra.controleCusto.length === 0">
                                            <td class="estilo-celula texto-centro" colspan="5">Nenhum registro encontrado</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <b-button v-on:click="edicaoValorTotalETO = false" v-if="edicaoValorTotalETO == true"
                                variant="info" class="width-200 mb-3 mr-3">
                                <span>Cancelar</span>
                            </b-button>
                            <b-button v-on:click="salvarETOs()" v-if="edicaoValorTotalETO == true" variant="success"
                                class="width-200 mb-3 mr-3">
                                <span>Salvar</span>
                            </b-button>
                        </Widget>
                    </b-col>
                </b-row>
                    </section>
      </b-container>


            <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalAjuste_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalAjusteComentario" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalAjusteComentario_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Comentário</span>
          <h3 class="afm-hero__title">{{ modalAjusteComentario_Titulo }}</h3>
          <p class="afm-hero__description">Adicione um comentário descrevendo o ajuste realizado.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

                <Widget customHeader class="estiloWidget">
                    <b-row>
                        <b-col md="12">
                            <div class="table-card table-card--fluid">
                                <table class="estilo-tabela tabela-identidade">
                                    <thead>
                                        <tr>
                                            <th class="estilo-cabecalho texto-centro">Data</th>
                                            <th class="estilo-cabecalho">Observação</th>
                                            <th class="estilo-cabecalho texto-centro">Visualizado</th>
                                            <th class="estilo-cabecalho texto-centro">Ações</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="(row, index) in modalAjusteComentario_Ajustes" :key="'aj-' + (row.id || index)">
                                            <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataCadastro) }}</td>
                                            <td class="estilo-celula">{{ row.observacao }}</td>
                                            <td class="estilo-celula texto-centro">{{ row.visualizado ? "Sim" : "Não" }}</td>
                                            <td class="estilo-celula texto-centro">
                                                <button type="button" class="btn mb-3 mr-3 btn-success"
                                                    @click="visualizaComentarioAjuste(row.id)" v-if="!row.visualizado">
                                                    <i class="fa fa-thumbs-o-up" title="Marcar como Visualizado"></i>
                                                </button>
                                            </td>
                                        </tr>
                                        <tr v-if="!modalAjusteComentario_Ajustes || modalAjusteComentario_Ajustes.length === 0">
                                            <td class="estilo-celula texto-centro" colspan="4">Nenhum registro encontrado</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-col>
                    </b-row>
                </Widget>
                    </section>
      </b-container>


            <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalAjusteComentario_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
        </b-modal>

        <b-row>
            <b-col md="4"></b-col>
            <b-col md="4"></b-col>
            <b-col md="4" style="text-align: right;">
                <button @click="downloadExcelComAjuste()" v-b-modal.cadastro type="button"
                    class="btn width-120 mb-3 mr-4 btn-outline-success">
                    Download Excel com Ajuste
                </button>
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
                <b-col md="6">
                    <h2>Detalhes</h2>
                </b-col>
                <b-col md="6" style="text-align: right;">
                    <div style="font-size: 18px;">Período de ajuste
                        <toggle-button v-model="periodoAjuste" :color="{
                            checked: '#2D8515',
                            unchecked: '#FF0000',
                            disabled: '#000000',
                        }" :labels="{ checked: 'Sim', unchecked: 'Não' }" :width="83" :height="25" :font-size="14" />
                    </div><br />

                    <div style="font-size: 18px;">Obra Bloqueada
                    <toggle-button v-model="filtroObraBloqueada" :color="{
                        checked: '#2D8515',
                        unchecked: '#FF0000',
                        disabled: '#000000',
                    }" :labels="{ checked: 'Sim', unchecked: 'Não' }" :width="83" :height="25" :font-size="14" @change="listaETOs()" />
                    </div>
                </b-col>
            </b-row>
            <b-row>
                <b-col lg="12">
                    <div class="table-card">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro">Obra</th>
                                    <th class="estilo-cabecalho">Nome</th>
                                    <th class="estilo-cabecalho">Escopo</th>
                                    <th class="estilo-cabecalho texto-centro">ETO Obra</th>
                                    <th class="estilo-cabecalho texto-centro">Gasto</th>
                                    <th class="estilo-cabecalho texto-centro">Saldo ETO</th>
                                    <th class="estilo-cabecalho texto-centro">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in etos" :key="'eto-' + (row.idObra || index)">
                                    <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                                    <td class="estilo-celula">{{ row.cliente }}</td>
                                    <td class="estilo-celula">{{ row.descricao }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.custo) }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.gasto) }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.saldo) }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <button
                                            type="button"
                                            class="btn btn-info"
                                            v-if="row.idObra > 0"
                                            @click="abrirModalAcoesETO(row)"
                                        >
                                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr v-if="!etos || etos.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="7">Nenhum registro encontrado</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </b-col>
            </b-row>
        </Widget>

        <!-- Modal de ações da tabela -->
        <ModalAcoes
          :exibir.sync="modalAcoes_Exibir"
          :titulo="modalAcoes_Titulo"
          :itens="modalAcoes_Itens"
        />
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
import ModalAcoes from "../../../components/ModalAcoes/ModalAcoes.vue";

export default {
    name: 'RelatorioETO',
    components: {
        Loading,
        Multiselect,
        CurrencyInput,
        Money,
        DatePickerMask,
        ModalAcoes
    },
    data() {
        return {
            // Estado do <ModalAcoes>
            modalAcoes_Exibir: false,
            modalAcoes_Titulo: "",
            modalAcoes_Itens: [],

            money: {
                decimal: ',',
                thousands: '.',
                prefix: 'R$ ',
                precision: 2,
                masked: false,
            },

            modalAjuste_Titulo: 'Ajuste de ETO',
            modalAjuste_Exibir: false,

            modalETO_Exibir: false,
            modalETO_Titulo: 'Novo ETO extra',
            modalETO_ValorPrevistoAjustado: 0.0,
            modalETO_DataPrevista: null,

            obra: null,
            edicaoValorTotalETO: false,
            obra_ValorCustoAjustado: 0.0,
            somatoriaValoresAjustadosETOs: 0.0,


            modalAjusteComentario_Titulo: 'Comentários Ajuste de ETO',
            modalAjusteComentario_Exibir: false,
            modalAjusteComentario_Ajustes: [],

            open: false,
            isLoading: false,

            usuarioDTO: null,

            etos: [],

            periodoAjuste: false,
            filtroObraBloqueada: true,

            columns: ["obra", "nome", "escopo", "eto", "gasto", "saldo", "acoes"],

            options: {
                perPage: 1000,
                headings: {
                    obra: "Obra",
                    nome: "Nome",
                    escopo: "Escopo",
                    eto: "ETO Obra",
                    gasto: "Gasto",
                    saldo: "Saldo ETO",
                    acoes: "Ações",
                },
                clientSorting: true,
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

            colunasControleCusto: [
                'dataPrevista',
                'valorPrevisto',
                'valorPrevistoAjustado',
                'valorMedido',
                'acoes'
            ],

            opcoesControleCusto: {
                perPage: 1000,
                filterable: false,
                headings: {
                    dataPrevista: 'Dt. Prevista',
                    valorPrevisto: 'Previsto (R$)',
                    valorPrevistoAjustado: 'Previsto Ajustado (R$)',
                    valorMedido: 'Medido (R$)',
                    acoes: 'Ações'
                },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: false },
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

            colunasAjusteComentario: ["data", "observacao", "visualizado", "acoes"],

            opcoesColunasAjusteComentario: {
                perPage: 1000,
                headings: {
                    data: "Data",
                    observacao: "Observação",
                    visualizado: "Visualizado",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: [],
                filterable: false,
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
        // Ações da tabela (usadas pelo <ModalAcoes>).
        // ============================================================
        montaAcoesETO(row) {
            const u = this.usuarioDTO;
            return [
                {
                    label: "Bloquear",
                    descricao: "Bloquear movimentações da obra",
                    icone: "ban",
                    variante: "danger",
                    onClick: () => this.bloquearDesbloquearObra(row.idObra, true),
                    visible: u != null && u.obraBloquear && !row.bloqueada,
                },
                {
                    label: "Desbloquear",
                    descricao: "Liberar movimentações da obra",
                    icone: "check-circle",
                    variante: "success",
                    onClick: () => this.bloquearDesbloquearObra(row.idObra, false),
                    visible: u != null && u.obraBloquear && row.bloqueada,
                },
                {
                    label: "Ajuste",
                    descricao: "Lançar ajuste no ETO",
                    icone: "calendar",
                    variante: "primary",
                    onClick: () => this.carregaModalAjuste(row),
                    visible: u != null && u.relatorioControleETOAjustar,
                },
                {
                    label: "Comentários",
                    descricao: "Ver comentários dos ajustes (há novos)",
                    icone: "comment",
                    variante: "success",
                    onClick: () => {
                        this.modalAjusteComentario_Exibir = true;
                        this.modalAjusteComentario_Ajustes = row.ajustes;
                    },
                    visible: row.comentariosNaoVisualizados == true,
                },
                {
                    label: "Comentários",
                    descricao: "Ver comentários dos ajustes",
                    icone: "comment-o",
                    onClick: () => {
                        this.modalAjusteComentario_Exibir = true;
                        this.modalAjusteComentario_Ajustes = row.ajustes;
                    },
                    visible: row.comentariosNaoVisualizados == false,
                },
            ];
        },
        abrirModalAcoesETO(row) {
            this.modalAcoes_Itens = this.montaAcoesETO(row);
            this.modalAcoes_Titulo = "Ações de " + (row.codigo || "") + " - " + (row.descricao || "");
            this.modalAcoes_Exibir = true;
        },

        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        carregaModalAjuste(objetoETO) {

            ApiService.get('Obra', objetoETO.idObra, (result) => {
                if (result.status != 200) {
                    this.$swal('', result.message, 'error');
                } else {
                    this.obra = result.data;

                    this.obtemSomatoriaETOs();

                    this.obra_ValorCustoAjustado = this.obra.valorCustoAjustado;
                    this.modalAjuste_Titulo = 'Ajuste de ETO - ' + objetoETO.codigo;
                    this.modalAjuste_Exibir = true;
                }
            });
        },

        cadastrarETOExtra() {
            if (this.modalETO_DataPrevista == null) {
                this.$swal("Data da medição inválida", "", "error");
                return;
            }

            if (this.modalETO_ValorPrevistoAjustado <= 0) {
                this.$swal("Valor da medição inválido", "", "error");
                return;
            }

            var objetoETO = {
                id: 0,
                idInterno: this.obra.medicoes.length,
                idObra: this.obra.id,
                dataPrevista: this.modalETO_DataPrevista,
                valorPrevistoAjustado: this.modalETO_ValorPrevistoAjustado,
                valorMedido: 0
            };

            this.obra.controleCusto.push(objetoETO);

            this.obtemSomatoriaETOs();

            this.modalETO_Exibir = false;
        },

        obtemSomatoriaETOs() {

            var result = 0;
            this.obra.controleCusto.forEach(x => {
                result += x.valorPrevistoAjustado;
            });

            this.somatoriaValoresAjustadosETOs = result;

            return result;
        },

        salvarETOs() {

            if (this.obtemSomatoriaETOs() != this.obra_ValorCustoAjustado) {
                this.$swal('A soma dos valores ajustados é diferente do valor total ajustado informado', '', 'error');
                return;
            }

            this.isLoading = true;

            ApiService.post('ObraControleCusto', this.obra.controleCusto, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal('Erro ao salvar ETOs', result.message, 'error');
                } else {
                    this.$swal('ETO Ajustado', '', 'success');
                    this.modalAjuste_Exibir = false;
                }
            });
        },

        abreModalEdicaoETO(ETO) {
            this.modalETOEdicao_ETO = ETO;
            this.modalETOEdicao_DataPrevista = ETO.dataPrevista;
            this.modalETOEdicao_ValorPrevistoAjustado = ETO.valorPrevistoAjustado;
            this.modalETOEdicao_Titulo = 'Editar Medição ' + this.formataDataSemHora(ETO.dataPrevista);
            this.modalETOEdicao_Exibir = true;
        },

        visualizaComentarioAjuste: function (idAjuste) {
            this.isLoading = true;

            ApiService.visualizaComentarioAjuste(idAjuste, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.$swal("", "Comentário Visualizado", "success");
                    this.modalAjusteComentario_Exibir = false;
                    this.listaETOs();
                }
            });
        },

        downloadExcelComAjuste: function () {
            this.isLoading = true;

            ApiService.downloadRelatorioETOExcelComAjuste((result) => {
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

            this.isLoading = true;

            ApiService.downloadRelatorioETOExcel((result) => {
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

        downloadPDF: function () {

            this.isLoading = true;

            ApiService.downloadRelatorioETOPDF((result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("Erro ao baixar PDF", result.data, "error");
                } else {

                    const blob = new Blob([result.data], {
                        type: result.contentType,
                    });

                    const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
                }
            });
        },

        bloquearDesbloquearObra: function (idObra, valor) {

            this.isLoading = true;

            ApiService.ativarDesativar('Obra', idObra, valor, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.listaETOs();
                }
            });
        },

        listaETOs: function () {
            this.isLoading = true;

            this.etos = [];

            ApiService.obtemRelatorioControleETO(this.filtroObraBloqueada, (result) => {

                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.etos = result.data;
                }
            });
        },
    },
    mounted() {
        this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaETOs();

    },
};
</script>
  
  <style src="./RelatorioETO.scss" lang="scss" />
  