<template>
  <div class="materialCategoria-page">
    <loading
      :active.sync="isLoading"
      :can-cancel="false"
      :is-full-page="true"
      color="#ffd639"
    ></loading>
    <h1 class="page-title">Categoria de Materiais &nbsp;
    </h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
          <button
            v-if="usuarioDTO != null && usuarioDTO.materialCategoriaCadastrar"
            @click="
              modal_Titulo = 'Cadastro de Categoria';
              materialCategoria_Id = 0;
              materialCategoria_Descricao = '';
              materialCategoria_Ativo = true;
              modal_ExibirBotaoCadastrar = true;
              modal_InativaBotaoCadastrar = false;
              modal_Exibir = true;
            "
            v-b-modal.cadastro
            type="button"
            class="btn width-120 mb-3 mr-4 btn-outline-success"
          >
            Nova Categoria
          </button>

          <b-modal
            id="cadastro"
            class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal"
            header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal"
            v-model="modal_Exibir"
            size="lg"
          >
            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Categoria</span>
                <h3 class="afm-hero__title">{{ modal_Titulo }}</h3>
                <p class="afm-hero__description">Informe a descrição e o status da categoria.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group mb-3">
                  <label class="afm-label">Descrição</label>
                  <b-form-input v-model="materialCategoria_Descricao" style="color: white"></b-form-input>
                </div>

                <div class="afm-field-group">
                  <label class="afm-label">Situação</label>
                  <div>
                    <toggle-button
                      v-model="materialCategoria_Ativo"
                      :color="{
                        checked: '#2D8515',
                        unchecked: '#FF0000',
                        disabled: '#CCCCCC',
                      }"
                      :labels="{ checked: 'Ativo', unchecked: 'Inativo' }"
                      :width="80"
                      :height="25"
                      :font-size="14"
                    />
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
                  <b-button
                    v-show="modal_ExibirBotaoCadastrar"
                    :disabled="modal_InativaBotaoCadastrar"
                    v-on:click="cadastraCategoria()"
                    variant="success"
                    class="mb-0"
                  >
                    <div v-if="controle_MaterialCategoriaCadastrando" class="spinner-border spinner-border-sm"></div>
                    <span v-if="controle_MaterialCategoriaCadastrando"> Aguarde ...</span>
                    <span v-else><i class="fa fa-save mr-1"></i> Cadastrar</span>
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
              <th class="estilo-cabecalho">Descricao</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in categorias" :key="'categ-' + (row.id || index)">
              <td class="estilo-celula">{{ row.descricao }}</td>
              <td class="estilo-celula texto-centro">{{ row.ativo }}</td>
              <td class="estilo-celula texto-centro">
                <button
                  type="button"
                  @click="ativarDesativarCategoria(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-danger"
                  v-if="row.ativo == 'Ativo' && usuarioDTO != null && usuarioDTO.materialCategoriaAtivarDesativar"
                >
                  Desativar
                </button>
                <button
                  type="button"
                  @click="ativarDesativarCategoria(row.id, row.ativo)"
                  class="btn width-100 mb-3 mr-3 btn-outline-success"
                  v-if="row.ativo == 'Inativo' && usuarioDTO != null && usuarioDTO.materialCategoriaAtivarDesativar"
                >
                  Ativar
                </button>
              </td>
            </tr>
            <tr v-if="!categorias || categorias.length === 0">
              <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
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
import ApiService from "@/services/api.service.js";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: 'MaterialCategoria',
  components: { Widget, Loading },
  data() {
    return {
      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Categoria",
      modal_ExibirBotaoCadastrar: true,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,

      usuarioDTO: null,

      materialCategoria_Id: "",
      materialCategoria_Descricao: "",
      materialCategoria_Ativo: true,

      controle_MaterialCategoriaCadastrando: false,

      categorias: [],

      columns: [
        "descricao",
        "ativo",
        "acoes",
      ],

      options: {
        perPage: 10,
        headings: {
          id: 0,
          descricao: "Descricao",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: ["descricao", "ativo", "cnpj"],
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
    cadastraCategoria: function () {
      this.isLoading = true;

      let validado = true;

      if (this.materialCategoria_Descricao.trim() == '') {
        validado = false;
        this.$swal("Informe uma descrição válida", "", "error");
      }

      if (validado) {
        let objCategoria = {
          Id: this.materialCategoria_Id,
          Descricao: this.materialCategoria_Descricao,
          Ativo: this.materialCategoria_Ativo,
        };

        ApiService.post("CategoriaMaterial", objCategoria, (result) => {
          if (result.status != 201) {
            this.$swal("Erro ao cadastrar categoria de material", result.message, "error");
          } else {
            this.listaCategorias();
            this.modal_Exibir = false;
          }
        });
      }

      this.isLoading = false;
    },

    listaCategorias: function () {
      this.categorias = [];

      ApiService.getAll("CategoriaMaterial", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          result.data.forEach((categoria) => {
            categoria.ativo = categoria.ativo ? "Ativo" : "Inativo";
            this.categorias.push(categoria);
          });
        }
      });
    },

    ativarDesativarCategoria: function (idCategoria, ativo) {
      ApiService.ativarDesativar(
        "CategoriaMaterial",
        idCategoria,
        ativo == "Ativo" ? false : true,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar categoria de material",
              result.message,
              "error"
            );
          } else {
            this.listaCategorias();
          }
        }
      );
    },
  },
  mounted () {
    this.isLoading = true;

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    this.listaCategorias();

    this.isLoading = false;
  },
};
</script>

<style src="./MaterialCategoria.scss" lang="scss" />
