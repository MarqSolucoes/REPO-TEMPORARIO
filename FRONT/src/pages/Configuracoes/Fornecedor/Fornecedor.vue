<template>
  <div class="fornecedor-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Fornecedor &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          
          <b-modal id="cadastro" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
            header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modal_Exibir" size="xl">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Fornecedor</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Preencha os dados cadastrais do fornecedor.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group mb-3">
                  <label class="afm-label">Tipo de Fornecedor</label>
                  <div>
                    <toggle-button v-model="controle_ExibePessoaFisica" :disabled="modal_InativaEscolhaTipoFornecedor"
                      :color="{
                        checked: '#2D8515',
                        unchecked: '#4348F0',
                        disabled: '#000000',
                      }" :labels="{
                        checked: 'Pessoa Física',
                        unchecked: 'Pessoa Jurídica',
                      }" :width="140" :height="25" :font-size="14" />
                  </div>
                </div>

                <div v-if="controle_ExibePessoaFisica">
                  <b-row>
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">Nome</label>
                        <b-form-input v-model="fornecedor_Nome" style="color: white"></b-form-input>
                      </div>
                    </b-col>
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">CPF</label>
                        <b-form-input v-model="fornecedor_CPF" :disabled="modal_InativaCampoCPF"
                          style="color: white; background-color: #040620" v-mask="'###.###.###-##'"></b-form-input>
                      </div>
                    </b-col>
                  </b-row>
                </div>

                <div v-if="!controle_ExibePessoaFisica">
                  <b-row>
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">Razão Social</label>
                        <b-form-input v-model="fornecedor_RazaoSocial" style="color: white"></b-form-input>
                      </div>
                    </b-col>
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">Nome Fantasia</label>
                        <b-form-input v-model="fornecedor_NomeFantasia" style="color: white"></b-form-input>
                      </div>
                    </b-col>
                  </b-row>

                  <b-row class="mt-3">
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">CNPJ</label>
                        <b-form-input :disabled="modal_InativaCampoCNPJ" v-model="fornecedor_CNPJ"
                          style="color: white; background-color: black" v-mask="'##.###.###/####-##'"></b-form-input>
                      </div>
                    </b-col>
                    <b-col md="6">
                      <div class="afm-field-group">
                        <label class="afm-label">Inscrição Estadual</label>
                        <b-form-input v-model="fornecedor_InscricaoEstadual" style="color: white"></b-form-input>
                      </div>
                    </b-col>
                  </b-row>
                </div>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Condição de Pagamento Padrão</label>
                      <multiselect v-model="fornecedor_CondicaoDePagamentoSelecionada" :multiple="false"
                        :options="condicoesDePagamento" select-label="Selecionar" placeholder="Selecione uma condição"
                        label="descricao" track-by="descricao" :disabled="controle_FornecedorCadastrando"></multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Número Cadastral</label>
                      <b-form-input v-model="fornecedor_NumeroCadastral" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Email</label>
                      <b-form-input v-model="fornecedor_Email" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Nome Vendedor</label>
                      <b-form-input v-model="fornecedor_NomeVendedor" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Telefone Fixo</label>
                      <b-form-input v-model="fornecedor_TelefoneFixo" style="color: white"
                        v-mask="'(##) ####-####'"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Celular</label>
                      <b-form-input v-model="fornecedor_TelefoneCelular" style="color: white"
                        v-mask="'(##) # ####-####'"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">CEP</label>
                      <b-form-input v-model="fornecedor_CEP" style="color: white" v-mask="'#####-###'"></b-form-input>
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
                      <b-form-input v-model="fornecedor_Endereco" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="4">
                    <div class="afm-field-group">
                      <label class="afm-label">Bairro</label>
                      <b-form-input v-model="fornecedor_Bairro" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Cidade</label>
                      <multiselect v-model="fornecedor_CidadeSelecionada" :multiple="false" :options="cidades"
                        :custom-label="multiselectCidades" select-label="Selecionar" placeholder="Selecione uma cidade"
                        label="nome" track-by="nome" :disabled="controle_FornecedorCadastrando"></multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Banco</label>
                      <b-form-input v-model="fornecedor_Banco" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Tipo de Conta</label>
                      <multiselect v-model="fornecedor_TipoContaCorrente" :multiple="false" :options="tipoContaCorrente"
                        select-label="Selecionar" placeholder="Selecione uma condição" label="descricao"
                        track-by="descricao" :disabled="controle_FornecedorCadastrando"></multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Agência</label>
                      <b-form-input v-model="fornecedor_Agencia" style="color: white;"></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Conta Corrente</label>
                      <b-form-input v-model="fornecedor_ContaCorrente" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Observação</label>
                      <textarea-autosize class="form-control" :min-height="115" v-model="fornecedor_Observacao"
                        style="color: white" />
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col>
                    <div class="afm-field-group">
                      <label class="afm-label">Situação</label>
                      <div>
                        <toggle-button v-model="fornecedor_Ativo" :disabled="modal_InativaEscolhaAtivoInativo" :color="{
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
                  <b-button :disabled="modal_InativaBotaoCancelar" @click="modal_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Cancelar
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoCadastrar" :disabled="modal_InativaBotaoCadastrar"
                    v-on:click="cadastraFornecedor()" variant="success" class="mb-0 mr-2">
                    <div v-if="controle_FornecedorCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_FornecedorCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoEditar" :disabled="modal_InativaBotaoEditar"
                    v-on:click="editarFornecedor()" variant="info" class="mb-0">
                    <div v-if="controle_FornecedorEditando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_FornecedorEditando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Editar</span>
                  </b-button>
                </div>
              </div>
            </template>
          </b-modal>
        </b-col>
      </b-row>

      <Widget customHeader class="estiloWidget">
        <b-row>
          <b-col md="3">
            <label class="mr-3">Razão Social:</label><br />
            <b-form-input v-model="filtro_RazaoSocial" style="color: white;"></b-form-input>
          </b-col>
          <b-col md="3">
            <label class="mr-3">Nome Fantasia:</label><br />
            <b-form-input v-model="filtro_NomeFantasia" style="color: white;"></b-form-input>
          </b-col>
          <b-col md="3">
            <label class="mr-3">CNPJ:</label><br />
            <b-form-input v-model="filtro_CNPJ" style="color: white;" v-mask="'##.###.###/####-##'"></b-form-input>
          </b-col>
          <b-col md="3">
            <label class="mr-3">Endereço:</label><br />
            <b-form-input v-model="filtro_Endereco" style="color: white"></b-form-input>
          </b-col>
        </b-row>

        <br/>

        <b-row>
          <b-col md="4"></b-col>
          <b-col md="4"></b-col>
          <b-col md="4" style="text-align: right;">
            <button @click="limpaFiltros()" type="button" class="btn width-120 mb-3 mr-4 btn-danger">
              Limpar Filtros
            </button>
            <button @click="listaFornecedoresFiltrados()" type="button"
              class="btn width-120 mb-3 mr-4 btn-success">
              Pesquisar
            </button>
          </b-col>
        </b-row>
      </Widget>

      <button v-if="usuarioDTO != null && usuarioDTO.fornecedorCadastrar" @click="
            modal_Titulo = 'Cadastro de Fornecedor';
          fornecedor_Id = 0;
          fornecedor_IdTipoFornecedor = 2;
          fornecedor_RazaoSocial = '';
          fornecedor_NomeFantasia = '';
          fornecedor_CNPJ = '';
          fornecedor_InscricaoEstadual = '';
          fornecedor_Nome = '';
          fornecedor_CPF = '';
          fornecedor_NumeroCadastral = '';
          fornecedor_Email = '';
          fornecedor_NomeVendedor = '';
          fornecedor_TelefoneCelular = '';
          fornecedor_TelefoneFixo = '';
          fornecedor_Endereco = '';
          fornecedor_CEP = '';
          fornecedor_Observacao = '';
          fornecedor_Ativo = true;
          fornecedor_Bairro = null;
          fornecedor_Banco = '';
          fornecedor_Agencia = '';
          fornecedor_ContaCorrente = '';
          fornecedor_TipoContaCorrente = null;
          fornecedor_CidadeSelecionada = null;
          modal_ExibirBotaoCadastrar = true;
          modal_ExibirBotaoEditar = false;
          modal_InativaBotaoCadastrar = false;
          modal_InativaBotaoEditar = true;
          modal_InativaCampoCNPJ = false;
          modal_InativaCampoCPF = false;
          modal_InativaEscolhaTipoFornecedor = false;
          modal_InativaEscolhaAtivoInativo = false;
          modal_Exibir = true;
          controle_ExibePessoaFisica = false;
          fornecedor_CondicaoDePagamentoSelecionada = null;
          " v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
            Novo Fornecedor
          </button>

      <div class="table-card">
        <table class="estilo-tabela tabela-identidade">
          <thead>
            <tr>
              <th class="estilo-cabecalho">Tipo Fornecedor</th>
              <th class="estilo-cabecalho">Razão Social</th>
              <th class="estilo-cabecalho">Nome Fantasia</th>
              <th class="estilo-cabecalho texto-centro">CNPJ</th>
              <th class="estilo-cabecalho texto-centro">Inscrição Estadual</th>
              <th class="estilo-cabecalho">Nome</th>
              <th class="estilo-cabecalho texto-centro">Telefone</th>
              <th class="estilo-cabecalho">Email</th>
              <th class="estilo-cabecalho">Endereço</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in fornecedores" :key="'fornecedor-' + (row.id || index)">
              <td class="estilo-celula">{{ row.tipoFornecedor }}</td>
              <td class="estilo-celula">{{ row.razaoSocial }}</td>
              <td class="estilo-celula">{{ row.nomeFantasia }}</td>
              <td class="estilo-celula texto-centro">{{ row.cnpj }}</td>
              <td class="estilo-celula texto-centro">{{ row.inscricaoEstadual }}</td>
              <td class="estilo-celula">{{ row.nomeVendedor }}</td>
              <td class="estilo-celula texto-centro">{{ row.telefoneFixo }}</td>
              <td class="estilo-celula">{{ row.email }}</td>
              <td class="estilo-celula">{{ row.endereco + ', ' + row.bairro + ' - ' + row.cep + ' - ' + (row.cidade ? row.cidade.nome : '') }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo }}</td>
              <td class="estilo-celula texto-centro">
                <button type="button" v-if="usuarioDTO != null && usuarioDTO.fornecedorEditar" @click="
                  modal_Titulo = 'Cadastro de Fornecedor';
                fornecedor_Id = row.id;
                fornecedor_IdTipoFornecedor = row.idTipoFornecedor;
                fornecedor_RazaoSocial = row.razaoSocial;
                fornecedor_NomeFantasia = row.nomeFantasia;
                fornecedor_CNPJ = row.cnpj;
                fornecedor_InscricaoEstadual = row.inscricaoEstadual;
                fornecedor_Nome = row.nome;
                fornecedor_CPF = row.cpf;
                fornecedor_NumeroCadastral = row.numeroCadastral;
                fornecedor_Email = row.email;
                fornecedor_NomeVendedor = row.nomeVendedor;
                fornecedor_TelefoneCelular = row.telefoneCelular;
                fornecedor_TelefoneFixo = row.telefoneFixo;
                fornecedor_Endereco = row.endereco;
                fornecedor_Bairro = row.bairro;
                fornecedor_CEP = row.cep;
                fornecedor_Observacao = row.observacao;
                fornecedor_Banco = row.banco;
                fornecedor_Agencia = row.agencia;
                fornecedor_ContaCorrente = row.conta;
                fornecedor_TipoContaCorrente = row.tipoConta == null ? null : { descricao: row.tipoConta };
                fornecedor_Ativo = row.ativo == 'Ativo' ? true : false;
                fornecedor_CidadeSelecionada = cidades.filter((cidade) => {
                  return cidade.id == row.idCidade;
                })[0];
                fornecedor_CondicaoDePagamentoSelecionada = condicoesDePagamento.filter((condicaoPagamento) => {
                  return condicaoPagamento.id == row.idCondicaoPagamento;
                })[0];
                modal_ExibirBotaoCadastrar = false;
                modal_ExibirBotaoEditar = true;
                modal_InativaBotaoCadastrar = true;
                modal_InativaBotaoEditar = false;
                modal_InativaCampoCNPJ = true;
                modal_InativaCampoCPF = true;
                modal_InativaEscolhaTipoFornecedor = true;
                modal_InativaEscolhaAtivoInativo = true;
                modal_Exibir = true;
                controle_ExibePessoaFisica =
                  row.idTipoFornecedor == 1 ? true : false;
                " class="btn width-100 mb-3 mr-3 btn-outline-info">
                  Editar
                </button>
                <button type="button" @click="ativarDesativarFornecedor(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger"
                  v-if="row.ativo == 'Ativo' && usuarioDTO != null && usuarioDTO.fornecedorAtivarDesativar">
                  Desativar
                </button>
                <button type="button" @click="ativarDesativarFornecedor(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == 'Inativo' && usuarioDTO != null && usuarioDTO.fornecedorAtivarDesativar">
                  Ativar
                </button>
              </td>
            </tr>
            <tr v-if="!fornecedores || fornecedores.length === 0">
              <td class="estilo-celula texto-centro" colspan="11">Nenhum registro encontrado</td>
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
import Multiselect from "vue-multiselect";

import ApiService from "@/services/api.service.js";
import ViaCepService from "@/services/viacep.service.js";

import VueTheMask from "vue-the-mask";
Vue.use(VueTheMask);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Fornecedor",
  components: { Widget, Loading, Multiselect },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Fornecedor",
      modal_ExibirBotaoCadastrar: true,
      modal_ExibirBotaoEditar: false,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,
      modal_InativaBotaoEditar: true,
      modal_InativaCampoCNPJ: false,
      modal_InativaCampoCPF: false,
      modal_InativaEscolhaTipoFornecedor: false,
      modal_InativaEscolhaAtivoInativo: false,

      usuarioDTO: null,

      fornecedor_Id: 0,
      fornecedor_IdTipoFornecedor: 2,
      fornecedor_RazaoSocial: "",
      fornecedor_NomeFantasia: "",
      fornecedor_CNPJ: "",
      fornecedor_InscricaoEstadual: "",
      fornecedor_NumeroCadastral: "",
      fornecedor_Nome: "",
      fornecedor_CPF: "",
      fornecedor_Email: "",
      fornecedor_NomeVendedor: "",
      fornecedor_TelefoneCelular: "",
      fornecedor_TelefoneFixo: "",
      fornecedor_CEP: "",
      fornecedor_Bairro: "",
      fornecedor_Endereco: "",
      fornecedor_Observacao: "",
      fornecedor_CidadeSelecionada: null,
      fornecedor_Ativo: true,
      fornecedor_CondicaoDePagamentoSelecionada: null,
      fornecedor_Banco: null,
      fornecedor_Agencia: null,
      fornecedor_ContaCorrente: null,
      fornecedor_TipoContaCorrente: null,

      filtro_CNPJ: '',
      filtro_Endereco: '',
      filtro_RazaoSocial: '',
      filtro_NomeFantasia: '',

      controle_ExibePessoaFisica: false,
      controle_FornecedorCadastrando: false,
      controle_FornecedorEditando: false,

      fornecedores: [],
      cidades: [],
      condicoesDePagamento: [],
      tipoContaCorrente: [{ descricao: 'Pessoa Jurídica' }, { descricao: 'Pessoa Física' }],

      columns: [
        "tipoFornecedor",
        "razaoSocial",
        "nomeFantasia",
        "cnpj",
        "inscricaoEstadual",
        "nome",
        "telefone",
        "email",
        "endereco",
        "ativo",
        "acoes",
      ],

      options: {
        perPage: 100,
        headings: {
          id: 0,
          tipoFornecedor: "Tipo Fornecedor",
          razaoSocial: "Razão Social",
          nomeFantasia: "Nome Fantasia",
          cnpj: "CNPJ",
          inscricaoEstadual: "Inscrição Estadual",
          nome: "Nome",
          telefone: "Telefone",
          email: "Email",
          endereco: "Endereço",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [
          "tipoFornecedor",
          "razaoSocial",
          "nomeFantasia",
          "cnpj",
          "inscricaoEstadual",
          "nome",
          "cpf",
          "ativo",
        ],
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
    multiselectCidades({ nome }) {
      return `${nome}`;
    },

    limpaFiltros() {
      this.filtro_CNPJ = '';
      this.filtro_Endereco = '';
      this.filtro_RazaoSocial='';
      this.filtro_NomeFantasia = '';
    },

    listaFornecedoresFiltrados: function () {

      this.fornecedores=[];
      this.isLoading = true;

      var objFiltro={
        razaoSocial: this.filtro_RazaoSocial,
        nomeFantasia: this.filtro_NomeFantasia,
        cnpj: this.filtro_CNPJ,
        endereco: this.filtro_Endereco
      };

      ApiService.listaFornecedoresFiltrados(objFiltro, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("Erro ao filtrar fornecedores", result.message, "error");
        } else {
          
          result.data.forEach((fornecedor) => {
            fornecedor.tipoFornecedor =
              fornecedor.idTipoFornecedor == 1
                ? "Pessoa Física"
                : "Pessoa Jurídica";
            fornecedor.ativo = fornecedor.ativo ? "Ativo" : "Inativo";
            this.fornecedores.push(fornecedor);
          });
        }
      });
    },

    cadastraFornecedor: function () {

      let validado = true;

      if (this.fornecedor_CondicaoDePagamentoSelecionada == null) {
        this.$swal("Condição de pagamento inválida", "Informe uma condição de pagamento padrão", "error");
        return;
      }

      if (this.controle_ExibePessoaFisica) {
        //Cadstro de fornecedor como pessoa física

        this.fornecedor_IdTipoFornecedor = 1;
        if (this.fornecedor_Nome.trim() == "") {
          validado = false;
          this.$swal("Nome inválido", "", "error");
        }

        if (
          this.fornecedor_CPF.trim() == "" ||
          this.fornecedor_CPF.length < 14
        ) {
          validado = false;
          this.$swal("CPF inválido", "", "error");
        }
      } else {
        //Cadastro de fornecedor como pessoa jurídica
        this.fornecedor_IdTipoFornecedor = 2;
        if (this.fornecedor_RazaoSocial.trim() == "") {
          validado = false;
          this.$swal("Razão social inválida", "", "error");
        }

        if (this.fornecedor_NomeFantasia.trim() == "") {
          validado = false;
          this.$swal("Nome fantasia inválido", "", "error");
        }

        if (
          this.fornecedor_CNPJ.trim() == "" ||
          this.fornecedor_CNPJ.length < 18
        ) {
          validado = false;
          this.$swal("CNPJ inválido", "", "error");
        }
      }

      if (this.fornecedor_CidadeSelecionada == null) {
        validado = false;
        this.$swal("Informe uma cidade para o fornecedor", "", "error");
      }

      if (validado) {
        let objFornecedor = {
          Id: this.fornecedor_Id,
          IdCidade: this.fornecedor_CidadeSelecionada.id,
          IdTipoFornecedor: this.fornecedor_IdTipoFornecedor,
          RazaoSocial:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_RazaoSocial,
          NomeFantasia:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_NomeFantasia,
          CNPJ:
            this.fornecedor_IdTipoFornecedor == 1 ? null : this.fornecedor_CNPJ,
          InscricaoEstadual:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_InscricaoEstadual,
          Nome:
            this.fornecedor_IdTipoFornecedor == 2 ? null : this.fornecedor_Nome,
          CPF:
            this.fornecedor_IdTipoFornecedor == 2 ? null : this.fornecedor_CPF,
          NumeroCadastral: this.fornecedor_NumeroCadastral,
          Email: this.fornecedor_Email,
          NomeVendedor: this.fornecedor_NomeVendedor,
          TelefoneCelular: this.fornecedor_TelefoneCelular,
          TelefoneFixo: this.fornecedor_TelefoneFixo,
          CEP: this.fornecedor_CEP,
          Bairro: this.fornecedor_Bairro,
          Endereco: this.fornecedor_Endereco,
          Observacao: this.fornecedor_Observacao,
          Ativo: this.fornecedor_Ativo,
          IdCondicaoPagamento: this.fornecedor_CondicaoDePagamentoSelecionada.id,
          Banco: this.fornecedor_Banco,
          Agencia: this.fornecedor_Agencia,
          Conta: this.fornecedor_ContaCorrente,
          TipoConta: this.fornecedor_TipoContaCorrente != null ? this.fornecedor_TipoContaCorrente.descricao : null,
        };

        this.isLoading = true;

        ApiService.post("Fornecedor", objFornecedor, (result) => {

          this.isLoading = false;

          if (result.status != 201) {
            this.$swal("Erro ao cadastrar fornecedor", result.message, "error");
          } else {
            this.listaFornecedores();
            this.modal_Exibir = false;
          }
        });
      }
    },

    editarFornecedor: function () {
      this.isLoading = true;

      let validado = true;

      if (this.fornecedor_CondicaoDePagamentoSelecionada == null) {
        this.$swal("Condição de pagamento inválida", "Informe uma condição de pagamento padrão", "error");
        return;
      }

      if (this.controle_ExibePessoaFisica) {
        //Edição de fornecedor como pessoa física

        this.fornecedor_IdTipoFornecedor = 1;

        if (this.fornecedor_Nome.trim() == "") {
          validado = false;
          this.$swal("Nome inválido", "", "error");
        }
      } else {
        //Edição de fornecedor como pessoa jurídica
        this.fornecedor_IdTipoFornecedor = 2;

        if (this.fornecedor_RazaoSocial.trim() == "") {
          validado = false;
          this.$swal("Razão social inválida", "", "error");
        }

        if (this.fornecedor_NomeFantasia.trim() == "") {
          validado = false;
          this.$swal("Nome fantasia inválido", "", "error");
        }
      }

      if (this.fornecedor_CidadeSelecionada == null) {
        validado = false;
        this.$swal("Informe uma cidade para o fornecedor", "", "error");
      }

      if (validado) {
        let objFornecedor = {
          Id: this.fornecedor_Id,
          IdCidade: this.fornecedor_CidadeSelecionada.id,
          IdTipoFornecedor: this.fornecedor_IdTipoFornecedor,
          RazaoSocial:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_RazaoSocial,
          NomeFantasia:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_NomeFantasia,
          CNPJ:
            this.fornecedor_IdTipoFornecedor == 1 ? null : this.fornecedor_CNPJ,
          InscricaoEstadual:
            this.fornecedor_IdTipoFornecedor == 1
              ? null
              : this.fornecedor_InscricaoEstadual,
          Nome:
            this.fornecedor_IdTipoFornecedor == 2 ? null : this.fornecedor_Nome,
          CPF:
            this.fornecedor_IdTipoFornecedor == 2 ? null : this.fornecedor_CPF,
          NumeroCadastral: this.fornecedor_NumeroCadastral,
          Email: this.fornecedor_Email,
          NomeVendedor: this.fornecedor_NomeVendedor,
          TelefoneCelular: this.fornecedor_TelefoneCelular,
          TelefoneFixo: this.fornecedor_TelefoneFixo,
          CEP: this.fornecedor_CEP,
          Endereco: this.fornecedor_Endereco,
          Bairro: this.fornecedor_Bairro,
          Observacao: this.fornecedor_Observacao,
          Ativo: this.fornecedor_Ativo,
          IdCondicaoPagamento: this.fornecedor_CondicaoDePagamentoSelecionada.id,
          Banco: this.fornecedor_Banco,
          Agencia: this.fornecedor_Agencia,
          Conta: this.fornecedor_ContaCorrente,
          TipoConta: this.fornecedor_TipoContaCorrente != null ? this.fornecedor_TipoContaCorrente.descricao : null,
        };

        ApiService.put("Fornecedor", objFornecedor, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao editar fornecedor", result.message, "error");
          } else {
            this.listaFornecedores();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    ativarDesativarFornecedor: function (idFornecedor, ativo) {
      ApiService.ativarDesativar(
        "Fornecedor",
        idFornecedor,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar fornecedor",
              result.message,
              "error"
            );
          } else {
            this.listaFornecedores();
          }
        }
      );
    },

    listaCondicoesDePagamento: function () {
      this.condicoesDePagamento = [];

      ApiService.getAll("CondicaoPagamento", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.condicoesDePagamento = result.data;
        }
      });
    },

    listaFornecedores: function () {
      this.fornecedores = [];

      ApiService.getAll("Fornecedor", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((fornecedor) => {
            fornecedor.tipoFornecedor =
              fornecedor.idTipoFornecedor == 1
                ? "Pessoa Física"
                : "Pessoa Jurídica";
            fornecedor.ativo = fornecedor.ativo ? "Ativo" : "Inativo";
            this.fornecedores.push(fornecedor);
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

      ViaCepService.buscaCep(this.fornecedor_CEP, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao buscar CEP", "error");
        } else {
          this.fornecedor_Bairro = result.data.bairro;
          this.fornecedor_Endereco = result.data.logradouro;

          this.fornecedor_CidadeSelecionada = this.cidades.filter((cidade) => cidade.nome == `${result.data.uf} - ${result.data.localidade}`)
        }

      });
    },
  },
  mounted() {
    this.isLoading = true;

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaFornecedores();
    this.listaCidades();
    this.listaCondicoesDePagamento();

    this.isLoading = false;
  },
};
</script>

<style src="./Fornecedor.scss" lang="scss" />
