<template>
  <div class="cliente-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Clientes &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <button v-if="usuarioDTO != null && usuarioDTO.clienteCadastrar" @click="
            modal_Titulo = 'Cadastro de Cliente';
          cliente_Id = 0;
          cliente_RazaoSocial = '';
          cliente_NomeFantasia = '';
          cliente_CNPJ = '';
          cliente_IE = '';
          (cliente_Endereco = ''), (cliente_EmailFinanceiro = '');
          cliente_EmailComercial = '';
          cliente_Ativo = true;
          cliente_CidadeSelecionada = null;
          cliente_CEP = '';
          cliente_ResponsavelComercial = '';
          cliente_TelefoneCelular = '';
          cliente_TelefoneFixo = '';
          cliente_Observacao = '';
          cliente_DiasDePagamento = 0,
          modal_ExibirBotaoCadastrar = true;
          modal_ExibirBotaoEditar = false;
          modal_InativaBotaoCadastrar = false;
          modal_InativaBotaoEditar = true;
          modal_InativaCampoCNPJ = false;
          modal_InativaEscolhaAtivoInativo = false,
            modal_Exibir = true;
          " v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
            Novo Cliente
          </button>

          <b-modal :no-close-on-backdrop="true" id="cadastro" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
            header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modal_Exibir" size="xl">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Cliente</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Preencha os dados cadastrais do cliente.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <b-row>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Razão Social</label>
                      <b-form-input v-model="cliente_RazaoSocial" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Nome Fantasia</label>
                      <b-form-input v-model="cliente_NomeFantasia" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">CNPJ</label>
                      <b-form-input :disabled="modal_InativaCampoCNPJ" v-model="cliente_CNPJ"
                        style="color: white; background-color: #040620;" v-mask="'##.###.###/####-##'"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Inscrição Estadual</label>
                      <b-form-input v-model="cliente_IE" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Cidade</label>
                      <multiselect v-model="cliente_CidadeSelecionada" :multiple="false" :options="cidades"
                        :custom-label="multiselectCidades" select-label="Selecionar" placeholder="Selecione uma cidade"
                        label="nome" track-by="nome" :disabled="controle_ClienteCadastrando"></multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">CEP</label>
                      <b-form-input v-model="cliente_CEP" style="color: white" v-mask="'#####-###'"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6" class="afm-field-group--action">
                    <b-button @click="buscaCep()" variant="success" class="afm-add-btn">
                      <i class="fa fa-search mr-1"></i> Buscar CEP
                    </b-button>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="8">
                    <div class="afm-field-group">
                      <label class="afm-label">Endereço</label>
                      <b-form-input v-model="cliente_Endereco" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="4">
                    <div class="afm-field-group">
                      <label class="afm-label">Bairro</label>
                      <b-form-input v-model="cliente_Bairro" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Email Financeiro</label>
                      <b-form-input v-model="cliente_EmailFinanceiro" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Email Comercial</label>
                      <b-form-input v-model="cliente_EmailComercial" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Responsável Comercial</label>
                      <b-form-input v-model="cliente_ResponsavelComercial" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Telefone Fixo</label>
                      <b-form-input v-model="cliente_TelefoneFixo" style="color: white"
                        v-mask="'(##) ####-####'"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Celular</label>
                      <b-form-input v-model="cliente_TelefoneCelular" style="color: white" v-mask="'(##) # ####-####'"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Dias de pagamento</label>
                      <b-form-input v-model="cliente_DiasDePagamento" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Situação</label>
                      <div>
                        <toggle-button v-model="cliente_Ativo" :disabled="modal_InativaEscolhaAtivoInativo" :color="{
                          checked: '#2D8515',
                          unchecked: '#FF0000',
                          disabled: '#000000',
                        }" :labels="{ checked: 'Ativo', unchecked: 'Inativo' }" :width="83" :height="25" :font-size="14" />
                      </div>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Observação</label>
                      <textarea-autosize class="form-control" :min-height="115" v-model="cliente_Observacao"
                        style="color: white" />
                    </div>
                  </b-col>
                </b-row>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button :disabled="modal_InativaBotaoCancelar" @click="modal_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoCadastrar" :disabled="modal_InativaBotaoCadastrar"
                    v-on:click="cadastraCliente()" variant="success" class="mb-0 mr-2">
                    <div v-if="controle_ClienteCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_ClienteCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoEditar" :disabled="modal_InativaBotaoEditar"
                    v-on:click="editarCliente()" variant="info" class="mb-0">
                    <div v-if="controle_ClienteEditando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_ClienteEditando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Editar</span>
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
              <th class="estilo-cabecalho">Razão Social</th>
              <th class="estilo-cabecalho">Nome Fantasia</th>
              <th class="estilo-cabecalho texto-centro">CNPJ</th>
              <th class="estilo-cabecalho texto-centro">Inscrição Estadual</th>
              <th class="estilo-cabecalho">Endereço</th>
              <th class="estilo-cabecalho">Email Financeiro</th>
              <th class="estilo-cabecalho">Email Comercial</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in clientes" :key="'cliente-' + (row.id || index)">
              <td class="estilo-celula">{{ row.razaoSocial }}</td>
              <td class="estilo-celula">{{ row.nomeFantasia }}</td>
              <td class="estilo-celula texto-centro">{{ row.cnpj }}</td>
              <td class="estilo-celula texto-centro">{{ row.inscricaoEstadual }}</td>
              <td class="estilo-celula">{{ row.endereco }}</td>
              <td class="estilo-celula">{{ row.emailFinanceiro }}</td>
              <td class="estilo-celula">{{ row.emailComercial }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo }}</td>
              <td class="estilo-celula texto-centro">
                <button type="button" v-if="usuarioDTO != null && usuarioDTO.clienteEditar" @click="
                  modal_Titulo = 'Edição de Cliente';
                cliente_Id = row.id;
                cliente_RazaoSocial = row.razaoSocial;
                cliente_NomeFantasia = row.nomeFantasia;
                cliente_CNPJ = row.cnpj;
                cliente_IE = row.inscricaoEstadual;
                cliente_Endereco = row.endereco,
                  cliente_CEP = row.cep,
                  cliente_Bairro = row.bairro,
                  cliente_EmailFinanceiro = row.emailFinanceiro;
                cliente_EmailComercial = row.emailComercial;
                cliente_ResponsavelComercial = row.responsavelComercial;
                cliente_TelefoneCelular = row.telefoneCelular;
                cliente_DiasDePagamento = row.diasDePagamento;
                cliente_TelefoneFixo = row.telefoneFixo;
                cliente_Observacao = row.observacao;
                cliente_Ativo = row.ativo == 'Ativo' ? true : false;
                cliente_CidadeSelecionada = cidades.filter((cidade) => {
                  return cidade.id == row.cidade.id;
                })[0];
                modal_ExibirBotaoCadastrar = false;
                modal_ExibirBotaoEditar = true;
                modal_InativaBotaoCadastrar = true;
                modal_InativaBotaoEditar = false;
                modal_InativaCampoCNPJ = true;
                modal_InativaEscolhaAtivoInativo = true;
                modal_Exibir = true;
                " class="btn width-100 mb-3 mr-3 btn-outline-info">
                  Editar
                </button>
                <button type="button" @click="ativarDesativarCliente(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger" v-if="row.ativo == 'Ativo' && usuarioDTO != null && usuarioDTO.clienteAtivarDesativar">
                  Desativar
                </button>
                <button type="button" @click="ativarDesativarCliente(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == 'Inativo' && usuarioDTO != null && usuarioDTO.clienteAtivarDesativar">
                  Ativar
                </button>
              </td>
            </tr>
            <tr v-if="!clientes || clientes.length === 0">
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
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import { VueMaskDirective } from "v-mask";
import Multiselect from "vue-multiselect";

import ApiService from "@/services/api.service.js";
import ViaCepService from "@/services/viacep.service.js";

import VueTheMask from 'vue-the-mask'
Vue.use(VueTheMask)

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Cliente",
  components: { Widget, Loading, Multiselect },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Cliente",
      modal_ExibirBotaoCadastrar: true,
      modal_ExibirBotaoEditar: false,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,
      modal_InativaBotaoEditar: true,
      modal_InativaCampoCNPJ: false,
      modal_InativaEscolhaAtivoInativo: false,

      usuarioDTO: null,

      cliente_Id: 0,
      cliente_RazaoSocial: "",
      cliente_NomeFantasia: "",
      cliente_CNPJ: "",
      cliente_IE: "",
      cliente_Endereco: "",
      cliente_Bairro: "",
      cliente_CEP: "",
      cliente_EmailFinanceiro: "",
      cliente_EmailComercial: "",
      cliente_ResponsavelComercial: "",
      cliente_TelefoneFixo: "",
      cliente_TelefoneCelular: "",
      cliente_DiasDePagamento: 0,
      cliente_Observacao: "",
      cliente_Ativo: true,

      controle_ClienteCadastrando: false,
      controle_ClienteEditando: false,

      cliente_CidadeSelecionada: null,

      clientes: [],
      cidades: [],

      columns: [
        "razaoSocial",
        "nomeFantasia",
        "cnpj",
        "inscricaoEstadual",
        "endereco",
        "emailFinanceiro",
        "emailComercial",
        "ativo",
        "acoes",
      ],

      options: {
        perPage: 10,
        headings: {
          id: 0,
          razaoSocial: "Razão Social",
          nomeFantasia: "Nome Fantasia",
          cnpj: "CNPJ",
          inscricaoEstadual: "Inscrição Estadual",
          endereco: "Endereço",
          emailFinanceiro: "Email Financeiro",
          emailComercial: "Email Comercial",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: ["razaoSocial", "nomeFantasia", "cnpj", "ativo"],
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

    multiselectCidades({ nome }) {
      return `${nome}`;
    },

    cadastraCliente: function () {
      this.isLoading = true;

      let validado = true;

      if (this.cliente_RazaoSocial.trim() == '') {
        validado = false;
        this.$swal("Razão social inválida", "", "error");
      }

      if (this.cliente_NomeFantasia.trim() == '') {
        validado = false;
        this.$swal("Nome fantasia inválido", "", "error");
      }

      if (this.cliente_CNPJ.trim() == "" || this.cliente_CNPJ.length < 18) {
        validado = false;
        this.$swal("CNPJ inválido", "", "error");
      }

      if (this.cliente_CidadeSelecionada == null) {
        validado = false;
        this.$swal("Informe uma cidade para o cliente", "", "error");
      }

      if (validado) {
        let objetoCliente = {
          Id: this.cliente_Id,
          IdCidade: this.cliente_CidadeSelecionada.id,
          RazaoSocial: this.cliente_RazaoSocial,
          NomeFantasia: this.cliente_NomeFantasia,
          CNPJ: this.cliente_CNPJ,
          InscricaoEstadual: this.cliente_IE,
          EmailFinanceiro: this.cliente_EmailFinanceiro,
          EmailComercial: this.cliente_EmailComercial,
          ResponsavelComercial: this.cliente_ResponsavelComercial,
          TelefoneCelular: this.cliente_TelefoneCelular,
          DiasDePagamento: this.cliente_DiasDePagamento,
          TelefoneFixo: this.cliente_TelefoneFixo,
          CEP: this.cliente_CEP,
          Endereco: this.cliente_Endereco,
          Bairro: this.cliente_Bairro,
          Observacao: this.cliente_Observacao,
          Ativo: this.cliente_Ativo,
        };

        ApiService.post("Cliente", objetoCliente, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao cadastrar cliente", result.message, "error");
          } else {
            this.listaClientes();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    editarCliente: function () {
      this.isLoading = true;

      let validado = true;

      if (this.cliente_RazaoSocial.trim() == '') {
        validado = false;
        this.$swal("Razão social inválida", "", "error");
      }

      if (this.cliente_NomeFantasia.trim() == '') {
        validado = false;
        this.$swal("Nome fantasia inválido", "", "error");
      }

      if (this.cliente_CNPJ.trim() == "" || this.cliente_CNPJ.length < 18) {
        validado = false;
        this.$swal("CNPJ inválido", "", "error");
      }

      if (this.cliente_CidadeSelecionada == null) {
        validado = false;
        this.$swal("Informe uma cidade para o cliente", "", "error");
      }

      if (validado) {
        let objetoCliente = {
          Id: this.cliente_Id,
          IdCidade: this.cliente_CidadeSelecionada.id,
          RazaoSocial: this.cliente_RazaoSocial,
          NomeFantasia: this.cliente_NomeFantasia,
          CNPJ: this.cliente_CNPJ,
          InscricaoEstadual: this.cliente_IE,
          EmailFinanceiro: this.cliente_EmailFinanceiro,
          EmailComercial: this.cliente_EmailComercial,
          ResponsavelComercial: this.cliente_ResponsavelComercial,
          TelefoneCelular: this.cliente_TelefoneCelular,
          TelefoneFixo: this.cliente_TelefoneFixo,
          DiasDePagamento: this.cliente_DiasDePagamento,
          CEP: this.cliente_CEP,
          Endereco: this.cliente_Endereco,
          Bairro: this.cliente_Bairro,
          Observacao: this.cliente_Observacao,
          Ativo: this.cliente_Ativo,
        };

        ApiService.put("Cliente", objetoCliente, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao editar cliente", result.message, "error");
          } else {
            this.listaClientes();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    ativarDesativarCliente: function (idCliente, ativo) {
      ApiService.ativarDesativar(
        "Cliente",
        idCliente,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar cliente",
              result.message,
              "error"
            );
          } else {
            this.listaClientes();
          }
        }
      );
    },

    listaClientes: function () {
      this.clientes = [];

      ApiService.getAll("Cliente", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((cliente) => {
            cliente.ativo = cliente.ativo ? "Ativo" : "Inativo";
            this.clientes.push(cliente);
          });
        }
      });
    },

    listaCidades: function () {
      this.cidades = [];

      ApiService.getAll("Cidade", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.cidades = result.data;
        }
      });
    },

    buscaCep: function () {
      this.isLoading = true;

      ViaCepService.buscaCep(this.cliente_CEP, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao buscar CEP", "error");
        } else {
          this.cliente_Bairro = result.data.bairro;
          this.cliente_Endereco = result.data.logradouro;
        }

      });
    },

  },
  mounted() {
    this.isLoading = true;

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaClientes();
    this.listaCidades();

    this.isLoading = false;
  },
};
</script>





<style src="./Cliente.scss" lang="scss" />
