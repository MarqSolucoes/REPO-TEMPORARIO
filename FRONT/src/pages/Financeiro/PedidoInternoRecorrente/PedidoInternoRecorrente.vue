<template>
    <div class="pedidoInternoRecorrente-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Pedido Interno Fixo &nbsp;</h1>

        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col cols="auto" class="mr-auto p-3"></b-col>
                <b-col cols="auto" style="padding: 0">
                    <button @click="
                        modalPedidoInternoRecorrente_Titulo = 'Cadastro de Pedido Interno Recorrente';
                    modalPedidoInternoRecorrente_Exibir = true;
                    modalPedidoInternoRecorrente_ExibirBotaoCadastrar = true;
                    modalPedidoInternoRecorrente_Id = 0;
                    modalPedidoInternoRecorrente_Ativo = true;
                    modalPedidoInternoRecorrente_DataLimiteGeracao = null;
                    modalPedidoInternoRecorrente_DefSelecionado = null;
                    modalPedidoInternoRecorrente_Descricao = '';
                    modalPedidoInternoRecorrente_DiaSelecionado = 1;
                    modalPedidoInternoRecorrente_NecessitaConfirmacao = false;
                    modalPedidoInternoRecorrente_Valor = 0;
                    " v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
                        Novo Pedido Interno Recorrente
                    </button>

                    <b-modal :no-close-on-backdrop="true" id="cadastro" class="modal-dialog modal-md afm-modal"
                        body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
                        v-model="modalPedidoInternoRecorrente_Exibir" size="lg">

                        <div class="afm-hero">
                          <div>
                            <span class="afm-hero__eyebrow">PI Recorrente</span>
                            <h3 class="afm-hero__title">{{ modalPedidoInternoRecorrente_Titulo }}</h3>
                            <p class="afm-hero__description">Configure um pedido interno que será gerado periodicamente.</p>
                          </div>
                        </div>

                        <b-container fluid class="afm-sections">
                          <section class="afm-section-card">
                            <div class="afm-field-group mb-3">
                              <label class="afm-label">Descrição</label>
                              <b-form-input v-model="modalPedidoInternoRecorrente_Descricao" style="color: white"></b-form-input>
                            </div>

                            <b-row>
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">DEF</label>
                                  <multiselect v-model="modalPedidoInternoRecorrente_DefSelecionado" :multiple="false"
                                    :options="defs" select-label="Selecionar" placeholder="Selecione um DEF"
                                    label="codigo" track-by="codigo">
                                  </multiselect>
                                </div>
                              </b-col>
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">Dia Vencimento</label>
                                  <multiselect v-model="modalPedidoInternoRecorrente_DiaSelecionado" :multiple="false"
                                    :options="dias" select-label="Selecionar" placeholder="Selecione um Dia">
                                  </multiselect>
                                </div>
                              </b-col>
                            </b-row>

                            <b-row class="mt-3">
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">Data Limite</label>
                                  <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" clearable="true"
                                    v-model="modalPedidoInternoRecorrente_DataLimiteGeracao" format="dd/MM/yyyy"
                                    type="date" :open.sync="open">
                                  </DatePickerMask>
                                  <a @click="modalPedidoInternoRecorrente_DataLimiteGeracao = null" style="display:inline-block;margin-top:6px;">Limpar Data</a>
                                </div>
                              </b-col>
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">Valor</label>
                                  <Money v-model="modalPedidoInternoRecorrente_Valor" v-bind="money"
                                    style="border-width: 0px; background-color: black;"></Money>
                                </div>
                              </b-col>
                            </b-row>

                            <b-row class="mt-3">
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">Necessita Confirmação</label>
                                  <div>
                                    <toggle-button v-model="modalPedidoInternoRecorrente_NecessitaConfirmacao" :color="{
                                      checked: '#2D8515',
                                      unchecked: '#FF0000',
                                      disabled: '#000000',
                                    }" :labels="{ checked: 'Sim', unchecked: 'Não' }" :width="73" :height="25" :font-size="14" />
                                  </div>
                                </div>
                              </b-col>
                              <b-col md="6">
                                <div class="afm-field-group">
                                  <label class="afm-label">Situação</label>
                                  <div>
                                    <toggle-button v-model="modalPedidoInternoRecorrente_Ativo" :color="{
                                      checked: '#2D8515',
                                      unchecked: '#FF0000',
                                      disabled: '#000000',
                                    }" :labels="{ checked: 'Ativo', unchecked: 'Inativo' }" :width="83" :height="25" :font-size="14" />
                                  </div>
                                </div>
                              </b-col>
                            </b-row>
                          </section>
                        </b-container>

                        <template #modal-footer>
                          <div class="afm-footer">
                            <div class="afm-footer__summary"></div>
                            <div class="afm-footer__actions">
                              <b-button @click="modalPedidoInternoRecorrente_Exibir = false" variant="dark" class="mb-0 mr-2">
                                Cancelar
                              </b-button>
                              <b-button v-show="modalPedidoInternoRecorrente_ExibirBotaoCadastrar" v-on:click="post()" variant="success" class="mb-0">
                                <i class="fa fa-save mr-1"></i> Cadastrar
                              </b-button>
                              <b-button v-show="!modalPedidoInternoRecorrente_ExibirBotaoCadastrar" v-on:click="put()" variant="success" class="mb-0">
                                <i class="fa fa-save mr-1"></i> Editar
                              </b-button>
                            </div>
                          </div>
                        </template>
                    </b-modal>
                </b-col>
            </b-row>

            <div class="table-card">
                <table class="estilo-tabela tabela-identidade">
                    <thead>
                        <tr>
                            <th class="estilo-cabecalho texto-centro">DEF</th>
                            <th class="estilo-cabecalho">Descrição</th>
                            <th class="estilo-cabecalho texto-centro">Dia de Vencimento</th>
                            <th class="estilo-cabecalho texto-centro">Data Limite</th>
                            <th class="estilo-cabecalho texto-centro">Valor</th>
                            <th class="estilo-cabecalho texto-centro">Necessita Confirmação</th>
                            <th class="estilo-cabecalho texto-centro">Status</th>
                            <th class="estilo-cabecalho">Usuário</th>
                            <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(row, index) in pedidosInternosRecorrentes" :key="'pir-' + (row.id || index)">
                            <td class="estilo-celula texto-centro">{{ row.def ? row.def.codigo : '' }}</td>
                            <td class="estilo-celula">{{ row.descricao }}</td>
                            <td class="estilo-celula texto-centro">{{ row.diaGeracao }}</td>
                            <td class="estilo-celula texto-centro">{{ row.dataLimiteGeracao != null ? formataDataSemHora(row.dataLimiteGeracao) : '' }}</td>
                            <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                            <td class="estilo-celula texto-centro">{{ row.necessitaConfirmacao ? 'Sim' : 'Não' }}</td>
                            <td class="estilo-celula texto-centro">{{ row.ativo ? 'Ativo' : 'Inativo' }}</td>
                            <td class="estilo-celula">{{ row.usuarioCadastro != null ? row.usuarioCadastro.nome : '' }}</td>
                            <td class="estilo-celula texto-centro">
                                <button type="button" @click="ativarDesativar(row.id, row.ativo)"
                                    class="btn width-100 mb-3 mr-3 btn-outline-danger" v-if="row.ativo == true">
                                    Desativar
                                </button>
                                <button type="button" @click="ativarDesativar(row.id, row.ativo)"
                                    class="btn width-100 mb-3 mr-3 btn-outline-success" v-if="row.ativo == false">
                                    Ativar
                                </button>
                                <button type="button" @click="
                                    modalPedidoInternoRecorrente_Exibir = true;
                                modalPedidoInternoRecorrente_ExibirBotaoCadastrar = false;
                                modalPedidoInternoRecorrente_Id = row.id;
                                modalPedidoInternoRecorrente_Ativo = row.ativo;
                                modalPedidoInternoRecorrente_DataLimiteGeracao = row.dataLimiteGeracao;
                                modalPedidoInternoRecorrente_DefSelecionado = row.def;
                                modalPedidoInternoRecorrente_Descricao = row.descricao;
                                modalPedidoInternoRecorrente_DiaSelecionado = row.diaGeracao;
                                modalPedidoInternoRecorrente_NecessitaConfirmacao = row.necessitaConfirmacao;
                                modalPedidoInternoRecorrente_Valor = row.valor;" class="btn width-100 mb-3 mr-3 btn-outline-info">
                                    Editar
                                </button>
                            </td>
                        </tr>
                        <tr v-if="!pedidosInternosRecorrentes || pedidosInternosRecorrentes.length === 0">
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
import Multiselect from "vue-multiselect";
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
    name: "Cargo",
    components: { Widget, Loading, Multiselect, Money, DatePickerMask },
    data() {
        return {
            money: {
                decimal: ',',
                thousands: '.',
                prefix: 'R$ ',
                precision: 2,
                masked: false,
            },

            open: false,
            isLoading: false,

            modalPedidoInternoRecorrente_Exibir: false,
            modalPedidoInternoRecorrente_Titulo: "Cadastro de Pedido Interno",
            modalPedidoInternoRecorrente_ExibirBotaoCadastrar: true,

            usuarioDTO: null,

            modalPedidoInternoRecorrente_Id: 0,
            modalPedidoInternoRecorrente_Descricao: "",
            modalPedidoInternoRecorrente_Ativo: true,
            modalPedidoInternoRecorrente_DefSelecionado: null,
            modalPedidoInternoRecorrente_DiaSelecionado: 1,
            modalPedidoInternoRecorrente_DataLimiteGeracao: null,
            modalPedidoInternoRecorrente_Valor: 0,
            modalPedidoInternoRecorrente_NecessitaConfirmacao: false,

            controle_CargoCadastrando: false,
            controle_CargoEditando: false,

            defs: [],
            pedidosInternosRecorrentes: [],
            dias: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30],

            columns: ["def", "descricao", "diaGeracao", "dataLimiteGeracao", "valor", "necessitaConfirmacao", "ativo", "usuarioCadastro", "acoes"],

            options: {
                perPage: 10,
                headings: {
                    def: "DEF",
                    descricao: "Descrição",
                    diaGeracao: "Dia de Vencimento",
                    dataLimiteGeracao: "Data Limite",
                    valor: "Valor",
                    necessitaConfirmacao: "Necessita Confirmação",
                    ativo: "Status",
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
        };
    },
    methods: {
        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        post: function () {

            if (this.modalPedidoInternoRecorrente_DefSelecionado == null) {
                this.$swal("", "DEF inválido", "error");
                return;
            }

            if (this.modalPedidoInternoRecorrente_Descricao == null) {
                this.$swal("", "Descrição inválida", "error");
                return;
            }

            this.isLoading = true;

            let objPedidoInternoRecorrente = {
                Id: 0,
                IdDEF: this.modalPedidoInternoRecorrente_DefSelecionado.id,
                Descricao: this.modalPedidoInternoRecorrente_Descricao,
                DiaGeracao: this.modalPedidoInternoRecorrente_DiaSelecionado,
                DataLimiteGeracao: this.modalPedidoInternoRecorrente_DataLimiteGeracao,
                Valor: this.modalPedidoInternoRecorrente_Valor,
                NecessitaConfirmacao: this.modalPedidoInternoRecorrente_NecessitaConfirmacao,
                Ativo: this.modalPedidoInternoRecorrente_Ativo,
            };

            ApiService.post('PedidoInternoRecorrente', objPedidoInternoRecorrente, (result) => {
                this.isLoading = false;

                if (result.status != 201) {
                    this.$swal("Erro ao cadastrar pedido interno recorrente", result.message, "error");
                } else {
                    this.getAll();
                    this.modalPedidoInternoRecorrente_Exibir = false;
                }
            });


        },

        put: function () {

            if (this.modalPedidoInternoRecorrente_DefSelecionado == null) {
                this.$swal("", "DEF inválido", "error");
                return;
            }

            if (this.modalPedidoInternoRecorrente_Descricao == null) {
                this.$swal("", "Descrição inválida", "error");
                return;
            }

            this.isLoading = true;

            let objPedidoInternoRecorrente = {
                Id: this.modalPedidoInternoRecorrente_Id,
                IdDEF: this.modalPedidoInternoRecorrente_DefSelecionado.id,
                Descricao: this.modalPedidoInternoRecorrente_Descricao,
                DiaGeracao: this.modalPedidoInternoRecorrente_DiaSelecionado,
                DataLimiteGeracao: this.modalPedidoInternoRecorrente_DataLimiteGeracao,
                Valor: this.modalPedidoInternoRecorrente_Valor,
                NecessitaConfirmacao: this.modalPedidoInternoRecorrente_NecessitaConfirmacao,
                Ativo: this.modalPedidoInternoRecorrente_Ativo,
            };

            ApiService.put('PedidoInternoRecorrente', objPedidoInternoRecorrente, (result) => {
                this.isLoading = false;

                if (result.status != 201) {
                    this.$swal("Erro ao cadastrar pedido interno recorrente", result.message, "error");
                } else {
                    this.getAll();
                    this.modalPedidoInternoRecorrente_Exibir = false;
                }
            });


        },

        ativarDesativar: function (idCargo, ativo) {
            ApiService.ativarDesativar(
                'PedidoInternoRecorrente',
                idCargo,
                ativo == true ? false : true,
                (result) => {
                    if (result.status != 200) {
                        this.$swal(
                            "Erro ao ativar/desativar pedido interno recorrente",
                            result.message,
                            "error"
                        );
                    } else {
                        this.getAll();
                        this.modalPedidoInternoRecorrente_Exibir = false;
                    }
                }
            );
        },

        getAll: function () {
            this.pedidosInternosRecorrentes = [];

            ApiService.getAllPedidosInternosRecorrentes('PedidoInternoRecorrente', (result) => {

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidosInternosRecorrentes = result.data;
                }

            });
        },

        listaDefs: function () {
            this.defs = [];

            ApiService.getAll("Def", true, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.defs = result.data;
                }
            });
        },
    },
    mounted() {
        this.isLoading = true;

        this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

        this.getAll();
        this.listaDefs();

        this.isLoading = false;
    },
};
</script>
  
  <style src="./PedidoInternoRecorrente.scss" lang="scss" />
  