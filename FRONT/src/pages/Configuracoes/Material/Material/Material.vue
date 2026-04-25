<template>
  <div class="material-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Materiais / Serviços &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <button v-if="usuarioDTO != null && usuarioDTO.materialCadastrar" @click="
            modal_Titulo = 'Cadastro de Material / Serviço';
          material_Id = 0;
          material_Descricao = '';
          material_Ativo = true;
          modal_ExibirBotaoCadastrar = true;
          modal_ExibirBotaoEditar = false;
          modal_InativaBotaoCadastrar = false;
          modal_Exibir = true;
          material_CategoriaSelecionada = null;
          material_UnidadeSelecionada = null;
          " v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
            Novo Material / Serviço
          </button>

          <b-modal :no-close-on-backdrop="true" id="cadastro" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modal_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Material</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Cadastre materiais ou serviços com categoria e unidade.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group mb-3">
                  <label class="afm-label">Tipo</label>
                  <div>
                    <toggle-button v-model="material_Servico"
                      :color="{ checked: '#2D8515', unchecked: '#2D8515', disabled: '#000000' }"
                      :labels="{ checked: 'Serviço', unchecked: 'Material' }" :width="100" :height="25" :font-size="14" />
                  </div>
                </div>

                <b-row>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Categoria do Material / Serviço</label>
                      <multiselect v-model="material_CategoriaSelecionada" :multiple="false" :options="categorias"
                        :custom-label="nameWithLang" select-label="Selecionar" placeholder="Selecione uma categoria"
                        label="descricao" track-by="descricao" :disabled="controle_MaterialCadastrando"></multiselect>
                    </div>
                  </b-col>
                  <b-col md="6">
                    <div class="afm-field-group">
                      <label class="afm-label">Unidade</label>
                      <multiselect v-model="material_UnidadeSelecionada" :multiple="false" :options="unidades"
                        select-label="Selecionar" placeholder="Selecione uma unidade" label="codigo" track-by="codigo">
                      </multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row class="mt-3">
                  <b-col md="12">
                    <div class="afm-field-group">
                      <label class="afm-label">Descrição</label>
                      <b-form-input v-model="material_Descricao" style="color: white"></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <div class="afm-field-group mt-3">
                  <label class="afm-label">Situação</label>
                  <div>
                    <toggle-button v-model="material_Ativo" :color="{
                      checked: '#2D8515',
                      unchecked: '#FF0000',
                      disabled: '#CCCCCC',
                    }" :labels="{ checked: 'Ativo', unchecked: 'Inativo' }" :width="80" :height="25" :font-size="14" />
                  </div>
                </div>
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
                    v-on:click="cadastraMaterial()" variant="success" class="mb-0 mr-2">
                    <div v-if="controle_MaterialCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_MaterialCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
                  </b-button>
                  <b-button v-show="modal_ExibirBotaoEditar" v-on:click="editarMaterial()" variant="info" class="mb-0">
                    <div v-if="controle_MaterialEditando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_MaterialEditando"> Aguarde ...</span>
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
              <th class="estilo-cabecalho">Categoria</th>
              <th class="estilo-cabecalho">Descricao</th>
              <th class="estilo-cabecalho texto-centro">Unidade</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in materiais" :key="'material-' + (row.id || index)">
              <td class="estilo-celula">{{ row.categoriaMaterial ? row.categoriaMaterial.descricao : '' }}</td>
              <td class="estilo-celula">{{ row.descricao }}</td>
              <td class="estilo-celula texto-centro">{{ row.unidadeMaterial ? row.unidadeMaterial.codigo : '' }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo == true ? 'Ativo' : 'Inativo' }}</td>
              <td class="estilo-celula texto-centro">
                <button type="button" @click="ativarDesativarMaterial(row.id, false)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger"
                  v-if="row.ativo == true && usuarioDTO != null && usuarioDTO.materialAtivarDesativar">
                  Desativar
                </button>
                <button type="button" @click="ativarDesativarMaterial(row.id, true)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == false && usuarioDTO != null && usuarioDTO.materialAtivarDesativar">
                  Ativar
                </button>

                <button type="button" @click="
                  (modal_Exibir = true);
                modal_ExibirBotaoCadastrar = false;
                modal_ExibirBotaoEditar = true;
                (material_Id = row.id);
                (material_Descricao = row.descricao);
                (material_Ativo = row.ativo);
                material_CategoriaSelecionada = categorias.filter((categoria) => {
                  return categoria.id == row.categoriaMaterial.id;
                });
                material_UnidadeSelecionada = unidades.find((unidade) => {
                  return unidade.id == row.idUnidadeMaterial;
                });
                " class="btn width-100 mb-3 mr-3 btn-outline-info">
                  Editar
                </button>
              </td>
            </tr>
            <tr v-if="!materiais || materiais.length === 0">
              <td class="estilo-celula texto-centro" colspan="5">Nenhum registro encontrado</td>
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
import { VueMaskDirective } from "v-mask";
import ApiService from "@/services/api.service.js";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: "Material",
  components: { Widget, Loading, Multiselect },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Categoria",
      modal_ExibirBotaoEditar: false,
      modal_ExibirBotaoCadastrar: true,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,

      usuarioDTO: null,

      material_Id: "",
      material_Descricao: "",
      material_Ativo: true,
      material_Servico: false,

      material_CategoriaSelecionada: null,
      material_UnidadeSelecionada: null,

      controle_MaterialCadastrando: false,
      controle_MaterialEditando: false,

      materiais: [],
      unidades: [],
      categorias: [],
    };
  },
  methods: {
    nameWithLang({ descricao }) {
      return `${descricao}`;
    },

    cadastraMaterial: function () {
      this.isLoading = true;

      let validado = true;

      if (this.material_Descricao.trim() == "") {
        validado = false;
        this.$swal("Informe uma descrição válida", "", "error");
      }

      if (this.material_CategoriaSelecionada == null) {
        validado = false;
        this.$swal("Selecione uma categoria", "", "error");
      }

      if (this.material_UnidadeSelecionada == null) {
        validado = false;
        this.$swal("Selecione uma unidade", "", "error");
      }

      if (validado) {
        let objMaterial = {
          Id: this.material_Id,
          IdCategoriaMaterial: Array.isArray(this.material_CategoriaSelecionada)
            ? this.material_CategoriaSelecionada[0].id
            : this.material_CategoriaSelecionada.id,
          IdUnidadeMaterial: this.material_UnidadeSelecionada.id,
          Descricao: this.material_Descricao,
          Ativo: this.material_Ativo,
          Servico: this.material_Servico
        };

        ApiService.post("Material", objMaterial, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao cadastrar material", result.message, "error");
          } else {
            this.listaMateriais();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    editarMaterial: function () {
      this.isLoading = true;

      let validado = true;

      if (this.material_Descricao.trim() == "") {
        validado = false;
        this.$swal("Informe uma descrição válida", "", "error");
      }

      if (this.material_CategoriaSelecionada == null) {
        validado = false;
        this.$swal("Selecione uma categoria", "", "error");
      }

      if (this.material_UnidadeSelecionada == null) {
        validado = false;
        this.$swal("Selecione uma unidade", "", "error");
      }

      if (validado) {
        let objMaterial = {
          Id: this.material_Id,
          IdCategoriaMaterial: Array.isArray(this.material_CategoriaSelecionada)
            ? this.material_CategoriaSelecionada[0].id
            : this.material_CategoriaSelecionada.id,
          IdUnidadeMaterial: this.material_UnidadeSelecionada.id,
          Descricao: this.material_Descricao,
          Ativo: this.material_Ativo,
          Servico: this.material_Servico
        };

        ApiService.put("Material", objMaterial, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao editar material", result.message, "error");
          } else {
            this.listaMateriais();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    listaMateriais() {
      const queryParams = {
        skip: 0,
        take: 99999,
        descricao: ''
      };

      this.isLoading = true;
      this.materiais = [];

      ApiService.obtemMateriaisPaginados(queryParams, (result) => {

        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {

          const data = result.data.data;

          this.materiais = data.items;
        }
      });
    },

    ativarDesativarMaterial: function (idMaterial, ativo) {
      ApiService.ativarDesativar(
        "Material",
        idMaterial,
        ativo,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar material",
              result.message,
              "error"
            );
          } else {
            this.listaMateriais();
          }
        }
      );
    },

    listaCategorias: function () {
      this.categorias = [];

      ApiService.getAll("CategoriaMaterial", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((categoria) => {
            this.categorias.push(categoria);
          });
        }
      });
    },

    listaUnidades: function () {
      this.unidades = [];

      ApiService.getUnidades((result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.unidades = result.data;
        }
      });
    },
  },

  mounted() {

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaCategorias();
    this.listaMateriais();
    this.listaUnidades();
  },
};
</script>

<style src="./Material.scss" lang="scss" />
