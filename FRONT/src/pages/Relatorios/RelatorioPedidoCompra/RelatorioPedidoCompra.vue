<template>
    <div class="RelatorioPedidosCompra-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
        <h1 class="page-title">Pedidos de Compra
        </h1>

        <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalArquivos_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Anexos</span>
                <h3 class="afm-hero__title">{{ modalArquivos_Titulo }}</h3>
                <p class="afm-hero__description">Consulte, baixe ou envie arquivos vinculados.</p>
              </div>
              <div class="afm-hero__pill">
                <span>{{ modalArquivos_Arquivos ? modalArquivos_Arquivos.length : 0 }}</span>
                <small>arquivo(s)</small>
              </div>
            </div>

            <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input"
              @change="onChange" ref="file" />

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div v-if="modalArquivos_Arquivos && modalArquivos_Arquivos.length > 0">
                  <div class="table-card table-card--fluid">
                    <table class="estilo-tabela tabela-identidade">
                      <thead>
                        <tr>
                          <th class="estilo-cabecalho">Arquivo</th>
                          <th class="estilo-cabecalho">Usuário</th>
                          <th class="estilo-cabecalho texto-centro">Data de envio</th>
                          <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(row, index) in modalArquivos_Arquivos" :key="'arq-' + (row.id || index)">
                          <td class="estilo-celula">{{ row.nome }}</td>
                          <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                          <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                          <td class="estilo-celula texto-centro">
                            <button type="button" @click="downloadFile(row.id, row.nome)" class="btn btn-warning afm-action-btn mr-1">
                              <i class="fa fa-download" title="Download"></i>
                            </button>
                            <button type="button" @click="excluirArquivo(row.id)" class="btn btn-danger afm-action-btn"
                              v-if="usuarioDTO == null ? false : (usuarioDTO.id == row.usuarioCadastro.id ? true : false)">
                              <i class="fa fa-trash" title="Excluir"></i>
                            </button>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <div v-else class="afm-empty-state">
                  <i class="fa fa-inbox"></i>
                  <div><strong>Nenhum arquivo anexado</strong><p>Envie o primeiro arquivo clicando no botão abaixo.</p></div>
                </div>

                <div class="mt-3" style="text-align: center;">
                  <label type="button" class="btn btn-success afm-add-btn" for="fileInput" style="width: auto; min-width: 200px; max-width: 320px;">
                    <i class="fa fa-upload mr-1"></i> Enviar arquivo
                  </label>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalArquivos_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalNotasFiscais" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalNotasFiscais_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Notas Fiscais</span>
          <h3 class="afm-hero__title">{{ modalNotasFiscais_Titulo }}</h3>
          <p class="afm-hero__description">Acompanhe as notas fiscais e cancelamentos de saldo deste pedido.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">


                <h4>Valor do Pedido: {{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", {
                    style:
                        "currency", currency: "BRL",
                }).format(modalNotasFiscais_Pedido.valorTotal) : 'R$ 0,00') }}</h4><br>
                <h4>Total notas: {{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", {
                    style:
                        "currency", currency: "BRL",
                }).format(modalNotasFiscais_Notas.reduce((total, item) => total + (item.aprovada != false ? (item.valor || 0) : 0), 0)) : 'R$ 0,00') }}</h4><br>

                <h4>Saldo: {{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", {
                    style:
                        "currency", currency: "BRL",
                }).format(modalNotasFiscais_Pedido.saldo) : 'R$ 0,00') }}</h4><br>

                <br>
                <Widget customHeader class="estiloWidget">
                    <h2>Notas Fiscais</h2><br />
                    <div class="table-card table-card--fluid">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho">Nome</th>
                                    <th class="estilo-cabecalho texto-centro">Nota Fiscal</th>
                                    <th class="estilo-cabecalho texto-centro">Pedido</th>
                                    <th class="estilo-cabecalho">Fornecedor</th>
                                    <th class="estilo-cabecalho texto-centro">Data Vencimento</th>
                                    <th class="estilo-cabecalho texto-centro">Valor</th>
                                    <th class="estilo-cabecalho texto-centro">Status</th>
                                    <th class="estilo-cabecalho texto-centro">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in modalNotasFiscais_Notas" :key="'nfm-' + (row.id || index)">
                                    <td class="estilo-celula">{{ row.nome }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.numeroNotaFiscal }}</td>
                                    <td class="estilo-celula texto-centro">{{ modalNotasFiscais_Pedido ? modalNotasFiscais_Pedido.codigo : '' }}</td>
                                    <td class="estilo-celula">{{ row.fornecedor ? row.fornecedor.nomeFantasia : '' }}</td>
                                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataVencimento) }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.aprovada == null ? "Pendente" : row.aprovada == true ? "Aprovada" : "Reprovada" }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <button type="button" @click="downloadNotaFiscal(row.arquivo.id, row.arquivo.nome)"
                                            class="btn width-75 mb-3 mr-3 bg-warning">
                                            <i class="fa fa-download"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr v-if="!modalNotasFiscais_Notas || modalNotasFiscais_Notas.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="8">Nenhum registro encontrado</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </Widget>

                <Widget customHeader class="estiloWidget">
                    <h2>Cancelamentos de Saldo</h2><br />
                    <div class="table-card table-card--fluid">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho">Motivo</th>
                                    <th class="estilo-cabecalho texto-centro">Data</th>
                                    <th class="estilo-cabecalho texto-centro">Valor</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in modalNotasFiscais_SaldosCancelados" :key="'sc-' + (row.id || index)">
                                    <td class="estilo-celula">{{ row.motivoDevolucao ? row.motivoDevolucao.descricao : '' }}</td>
                                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                                </tr>
                                <tr v-if="!modalNotasFiscais_SaldosCancelados || modalNotasFiscais_SaldosCancelados.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </Widget>

                    </section>
      </b-container>


            <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalNotasFiscais_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalReabrirSolicitacao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalReabrirSolicitacao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Solicitação</span>
          <h3 class="afm-hero__title">{{ modalReabrirSolicitacao_Titulo }}</h3>
          <p class="afm-hero__description">Reabra a solicitação informando obra, data e itens.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

                <b-row>
                    <b-col>
                        <label class="mr-3">Informe a obra:</label>
                        <multiselect v-model="modalReabrirSolicitacao_ObraSelecionada" :multiple="false"
                            :options="obras" :custom-label="descricaoObra" select-label="Selecionar"
                            placeholder="Selecione uma obra" label="descricao" track-by="descricao">
                        </multiselect>
                    </b-col>
                </b-row>

                <br />

                <b-row>
                    <b-col md="6">
                        <label class="mr-3">Informe a data de entrega:</label>
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                            v-model="modalReabrirSolicitacao_DataEntrega" format="dd/MM/yyyy" type="date"
                            :open.sync="open">
                        </DatePickerMask>
                    </b-col>
                </b-row>

                <br />

                <Widget customHeader class="estiloWidget">
                    <b-row>
                        <b-col>
                            <label>Material / Serviço: </label>
                            <multiselect v-model="modalReabrirSolicitacao_MaterialSelecionado" :multiple="false"
                                :options="materiais" select-label="Selecionar"
                                placeholder="Selecione um material ou serviço" label="descricao" track-by="descricao"
                                :custom-label="descricaoMaterial">
                            </multiselect>
                        </b-col>
                    </b-row>

                    <br />

                    <b-row>
                        <b-col md="6">
                            <label>Quantidade: </label>
                            <Money v-model="modalReabrirSolicitacao_Quantidade" v-bind="number" ref="valorUnitario"
                                @keyup.native.enter="enterQuantidade"></Money>
                        </b-col>

                        <b-col md="6">
                            <label>Valor unitário: </label>
                            <Money v-model="modalReabrirSolicitacao_ValorUnitario" v-bind="money" ref="valorUnitario"
                                @keyup.native.enter="enterValorUnitario"></Money>
                        </b-col>
                    </b-row>

                    <br />

                    <b-row style="text-align: center">
                        <b-col>
                            <label></label>
                            <b-button v-on:click="modalReabrirSolicitacaoAdicionaMaterial()" variant="success"
                                class="width-220 mb-3 mr-3">
                                <span>Adicionar Material / Serviço</span>
                            </b-button>
                        </b-col>
                    </b-row>
                </Widget>


                <b-row>
                    <b-col>

                        <div class="table-card table-card--fluid">
                            <table class="estilo-tabela tabela-identidade">
                                <thead>
                                    <tr>
                                        <th class="estilo-cabecalho texto-centro">Material / Serviço</th>
                                        <th class="estilo-cabecalho texto-centro">QTD</th>
                                        <th class="estilo-cabecalho texto-centro">Valor Un.</th>
                                        <th class="estilo-cabecalho texto-centro">Total</th>
                                        <th class="estilo-cabecalho texto-centro"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(material, index) in modalReabrirSolicitacao_MateriaisAdicionados" :key="'mat-' + (material.id || index)">
                                        <td class="estilo-celula texto-centro">{{ material.material }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <div class="input-table input-table--money">
                                                <Money v-model="material.quantidade" v-bind="number"></Money>
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div class="input-table input-table--money">
                                                <Money v-model="material.valorUnitario" v-bind="number"></Money>
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format((material.valorUnitario * material.quantidade)) }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <button type="button"
                                                @click="modalReabrirSolicitacao_RemoverMaterial(material.id)"
                                                class="btn width-75 mb-3 mr-3 bg-danger">
                                                <i class="fa fa-trash"></i>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr v-if="!modalReabrirSolicitacao_MateriaisAdicionados || modalReabrirSolicitacao_MateriaisAdicionados.length === 0">
                                        <td class="estilo-celula texto-centro" colspan="5">Nenhum registro encontrado</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </b-col>
                </b-row>

                    </section>
      </b-container>


            <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalReabrirSolicitacao_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
        </b-modal>

        <Widget customHeader class="estiloWidget">
            <Widget customHeader class="estiloWidget">
                <b-row>
                    <b-col md="4">
                        <label class="mr-3">Obra:</label><br />
                        <multiselect v-model="filtro_ObraSelecionada" :multiple="true" :options="obras"
                            select-label="Selecionar" placeholder="Selecione uma ou mais Obras" label="codigo"
                            track-by="codigo">
                        </multiselect>
                    </b-col>
                    <b-col md="4">
                        <label class="mr-3">Cliente:</label><br />
                        <multiselect v-model="filtro_ClienteSelecionado" :multiple="true" :options="clientes"
                            select-label="Selecionar" placeholder="Selecione um ou mais Clientes" label="nomeFantasia"
                            track-by="nomeFantasia">
                        </multiselect>
                    </b-col>
                    <b-col md="4">
                        <label class="mr-3">Material:</label>
                        <multiselect v-model="filtro_MateriaisSelecionados" :multiple="true" :options="materiais"
                            select-label="Selecionar" placeholder="Selecione um ou mais materiais" label="descricao"
                            track-by="descricao">
                        </multiselect>
                    </b-col>
                </b-row>

                <br />

                <b-row>
                    <b-col md="4">
                        <label class="mr-3">Fornecedor:</label><br />
                        <multiselect v-model="filtro_FornecedoresSelecionados" :multiple="true" :options="fornecedores"
                            select-label="Selecionar" placeholder="Selecione uma ou mais Fornecedores"
                            label="nomeFantasia" track-by="nomeFantasia">
                        </multiselect>
                    </b-col>
                    <b-col md="4">
                        <label class="mr-3">Data Solicitação Inicial:</label><br />
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                            v-model="filtro_DataInicial" format="dd/MM/yyyy" :clearable='true' type="date">
                        </DatePickerMask>
                        <a @click="filtro_DataInicial = null">limpar</a>
                    </b-col>
                    <b-col md="4">
                        <label class="mr-3">Data Solicitação Final:</label><br />
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                            v-model="filtro_DataFinal" format="dd/MM/yyyy" :clearable='true' type="date">
                        </DatePickerMask>
                        <a @click="filtro_DataFinal = null">limpar</a>
                    </b-col>
                </b-row>

                <br />

                <b-row>
                    <b-col md="4"></b-col>
                    <b-col md="4"></b-col>
                    <b-col md="4" style="text-align: right;">
                        <button @click="limpaFiltros()" v-b-modal.cadastro type="button"
                            class="btn width-120 mb-3 mr-4 btn-danger">
                            Limpar Filtros
                        </button>
                        <button @click="obtemPedidosFinalizadosFiltrados()" v-b-modal.cadastro type="button"
                            class="btn width-120 mb-3 mr-4 btn-success">
                            Pesquisar
                        </button>

                    </b-col>
                </b-row>
            </Widget>

            <div class="table-card">
                <table class="estilo-tabela tabela-identidade">
                    <thead>
                        <tr>
                            <th class="estilo-cabecalho texto-centro">Código</th>
                            <th class="estilo-cabecalho texto-centro">Código Solicitação</th>
                            <th class="estilo-cabecalho texto-centro">Data do Pedido</th>
                            <th class="estilo-cabecalho texto-centro">Data de entrega</th>
                            <th class="estilo-cabecalho">Centro de Custo</th>
                            <th class="estilo-cabecalho">Fornecedor</th>
                            <th class="estilo-cabecalho texto-centro">Valor total cotado</th>
                            <th class="estilo-cabecalho texto-centro">Status</th>
                            <th class="estilo-cabecalho texto-centro">Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(row, index) in pedidosCancelados" :key="'pc-' + (row.id || index)">
                            <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                            <td class="estilo-celula texto-centro">{{ row.solicitacaoCompra ? row.solicitacaoCompra.codigo : '' }}</td>
                            <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                            <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                            <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                            <td class="estilo-celula">{{ row.fornecedor ? row.fornecedor.nomeFantasia : '' }}</td>
                            <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                            <td class="estilo-celula texto-centro">{{ row.statusPedidoCompra ? row.statusPedidoCompra.descricao : '' }}</td>
                            <td class="estilo-celula texto-centro">
                                <button
                                    type="button"
                                    class="btn btn-info"
                                    @click="abrirModalAcoesPedidoCancelado(row)"
                                >
                                    Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                </button>
                            </td>
                        </tr>
                        <tr v-if="!pedidosCancelados || pedidosCancelados.length === 0">
                            <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
                        </tr>
                    </tbody>
                </table>
            </div>
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
import ModalAcoes from "../../../components/ModalAcoes/ModalAcoes.vue";
Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: 'RelatorioPedidosCompra',
    components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask, ModalAcoes },
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

            number: {
                decimal: ',',
                thousands: '.',
                prefix: '',
                precision: 2,
                masked: false,
            },

            decimalInput: {
                decimal: ",",
                thousands: ".",
                prefix: "",
                precision: 2,
                masked: false,
            },

            dataMinima: (new Date()).setDate(new Date().getDate() - 1),

            open: false,
            isLoading: false,

            filtro_ObraSelecionada: [],
            filtro_ClienteSelecionado: [],
            filtro_MateriaisSelecionados: [],
            filtro_DataInicial: null,
            filtro_DataFinal: null,
            filtro_FornecedoresSelecionados: [],

            modalArquivos_Exibir: false,
            modalArquivos_Titulo: 'Arquivos',
            modalArquivos_Arquivos: [],
            modalArquivos_Controller: '',
            modalArquivos_IdSolicitacaoOuPedidoCompra: 0,

            modalNotasFiscais_Titulo: 'Notas Fiscais',
            modalNotasFiscais_Exibir: false,
            modalNotasFiscais_Notas: [],
            modalNotasFiscais_SaldosCancelados: [],
            modalNotasFiscais_Pedido: null,

            modalReabrirSolicitacao_Exibir: false,
            modalReabrirSolicitacao_Titulo: '',
            modalReabrirSolicitacao_ObraSelecionada: null,
            modalReabrirSolicitacao_IdSolicitacao: null,
            modalReabrirSolicitacao_DataEntrega: null,
            modalReabrirSolicitacao_MaterialSelecionado: null,
            modalReabrirSolicitacao_Quantidade: 0,
            modalReabrirSolicitacao_ValorUnitario: 0,
            modalReabrirSolicitacao_ValorTotal: 0,
            modalReabrirSolicitacao_MateriaisAdicionados: [], //Lista de materiais adicionados na solicitação

            obras: [],
            defs: [],
            materiais: [], //Lista de materiais ativos no sistema
            materiaisAdicionados: [], //Lista de materiais adicionados na solicitação
            fornecedores: [],
            clientes: [],
            condicoesPagamento: [],

            solicitacoesParaValidacao: [],
            solicitacoesEmCotacao: [],
            pedidosParaAprovacao: [],
            solicitacoesDevolvidasDiretoria: [],
            pedidosEmCompra: [],
            pedidosCancelados: [],
            solicitacoesCanceladas: [],
            solicitacaoParaCancelamento: null,
            solicitacaoParaValidacao: null,

            colunasTabelaSolicitacaoCompra: [
                "material",
                "quantidade",
                "valorUnitario",
                "valorTotal",
                "acoes",
            ],

            opcoesTabelaSolicitacaoCompra: {
                perPage: 1000,
                headings: {
                    material: "Material / Serviço",
                    quantidade: "Qtd.",
                    valorUnitario: "Valor Un.",
                    valorTotal: "Total",
                    acoes: "",
                },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: false },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count:
                        "Exibindo {from} de {to} de {count} itens|{count} itens|Um item",
                    first: "Primeiro",
                    last: "último",
                    filter: "",
                    limit: "Itens:",
                    page: "Página:",
                    skin: "table table-striped",
                    noResults: "Não encontrado",
                },
            },

            colunasMinhasSolicitacoes: [
                "codigo",
                "nome",
                "dataCadastro",
                "dataEntrega",
                "centroCusto",
                "valorEstimado",
                "status",
                "acoes",
            ],

            opcoesMinhasSolicitacoes: {
                perPage: 10,
                headings: {
                    codigo: "Código Solicitação",
                    nome: "Nome",
                    dataCadastro: "Data da solicitação",
                    dataEntrega: "Data de entrega",
                    centroCusto: "Centro de custo",
                    valorEstimado: "Valor total estimado",
                    status: "Status",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: [
                    "codigo",
                    "dataEntrega",
                    "dataCadastro",
                    "centroCusto",
                    "valorEstimado",
                ],
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

            colunasMeusPedidosInternos: [
                "codigoFormatado",
                "beneficiario",
                "numeroParcelas",
                "dataCadastro",
                "valor",
                "status",
                "acoes",
            ],

            opcoesMeusPedidosInternos: {
                perPage: 10,
                headings: {
                    codigoFormatado: "Código",
                    beneficiario: "Beneficiário",
                    numeroParcelas: "Nº Parcelas",
                    dataCadastro: "Data da solicitação",
                    valor: "Valor",
                    status: "Status",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: [
                    "codigo",
                    "dataEntrega",
                    "dataCadastro",
                    "centroCusto",
                    "valorEstimado",
                ],
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

            colunasConciliados: [
                "nome",
                "numeroNotaFiscal",
                "pedidoCompra",
                "fornecedor",
                "dataVencimento",
                "valor",
                "status",
                "acoes",
            ],

            opcoesConciliados: {
                perPage: 1000,
                headings: {
                    nome: "Nome",
                    numeroNotaFiscal: "Nota Fiscal",
                    pedidoCompra: "Pedido",
                    fornecedor: "Fornecedor",
                    dataVencimento: "Data Vencimento",
                    valor: "Valor",
                    status: "Status",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: [
                    "codigo",
                    "codigoSolicitacao",
                    "dataEntrega",
                    "dataCadastro",
                    "centroCusto",
                    "fornecedor",
                    "valorTotalCotado",
                ],
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

            colunasSaldosCancelados: [
                "motivo",
                "data",
                "valor",
            ],

            opcoesSaldosCancelados: {
                perPage: 1000,
                headings: {
                    motivo: "Motivo",
                    data: "Data",
                    valor: "Valor",
                },
                clientSorting: true,
                sortable: [
                    "codigo",
                    "codigoSolicitacao",
                    "dataEntrega",
                    "dataCadastro",
                    "centroCusto",
                    "fornecedor",
                    "valorTotalCotado",
                ],
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

            colunasTabelaArquivos: ["arquivo", "usuario", "data", "acoes"],

            opcoesTabelaArquivos: {
                perPage: 1000,
                headings: {
                    arquivo: "Arquivo",
                    usuario: "Usuário",
                    data: "Data de envio",
                    acoes: "",
                },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: false },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count:
                        "Exibindo {from} de {to} de {count} itens|{count} itens|Um item",
                    first: "Primeiro",
                    last: "último",
                    filter: "",
                    limit: "Itens:",
                    page: "Página:",
                    skin: "table table-striped",
                    noResults: "Não encontrado",
                },
            },

            colunasPedidosFinalizadosECancelados: [
                "codigo",
                "codigoSolicitacao",
                "dataCadastro",
                "dataEntrega",
                "centroCusto",
                "fornecedor",
                "valorTotalCotado",
                "status",
                "acoes",
            ],

            opcoesPedidosFinalizadosECancelados: {
                perPage: 10,
                headings: {
                    codigo: "Código",
                    codigoSolicitacao: "Código Solicitação",
                    dataCadastro: "Data do Pedido",
                    dataEntrega: "Data de entrega",
                    centroCusto: "Centro de Custo",
                    fornecedor: "Fornecedor",
                    valorTotalCotado: "Valor total cotado",
                    status: "Status",
                    acoes: "Ações",
                },
                clientSorting: true,
                sortable: [
                    "codigo",
                    "codigoSolicitacao",
                    "dataEntrega",
                    "dataCadastro",
                    "centroCusto",
                    "fornecedor",
                    "valorTotalCotado",
                ],
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
        }
    },

    methods: {
        // ============================================================
        // Ações da tabela (usadas pelo <ModalAcoes>).
        // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
        // ============================================================
        montaAcoesPedidoCancelado(row) {
            const u = this.usuarioDTO;
            return [
                {
                    label: "Arquivos",
                    descricao: "Ver anexos deste pedido",
                    icone: "paperclip",
                    onClick: () => {
                        this.modalArquivos_Exibir = true;
                        this.modalArquivos_IdSolicitacaoOuPedidoCompra = row.id;
                        this.modalArquivos_Controller = "PedidoCompra";
                        this.modalArquivos_Arquivos = row.arquivos;
                    },
                    visible: u != null && u.comprasOrdensCompraAnexos,
                },
                {
                    label: "Notas Fiscais",
                    descricao: "Ver notas fiscais do pedido",
                    icone: "file-text-o",
                    onClick: () => this.exibirModalNotasFiscais(row),
                },
                {
                    label: "PDF Pedido Compra",
                    descricao: "Baixar PDF do pedido de compra",
                    icone: "file-pdf-o",
                    variante: "warning",
                    onClick: () => this.downloadPdfArquivoCompra(row.id, row.codigo + ".pdf"),
                    visible: u != null && u.comprasOrdensCompraFinalizadasPDF,
                },
                {
                    label: "PDF Solicitação Compra",
                    descricao: "Baixar PDF da solicitação de compra",
                    icone: "file-pdf-o",
                    variante: "warning",
                    onClick: () =>
                        this.downloadPdfArquivoSolicitacaoCompra(
                            row.idSolicitacaoCompra,
                            row.solicitacaoCompra.codigo + ".pdf"
                        ),
                    visible: u != null && u.comprasOrdensCompraFinalizadasPDF,
                },
                {
                    label: "PDF Cotações",
                    descricao: "Baixar PDF com as cotações",
                    icone: "file-pdf-o",
                    variante: "warning",
                    onClick: () =>
                        this.downloadPdfCotacoes(
                            row.idSolicitacaoCompra,
                            "Cotações " + row.solicitacaoCompra.codigo + ".pdf"
                        ),
                    visible: u != null && u.comprasOrdensCompraFinalizadasPDF,
                },
                {
                    label: "XLS Cotações",
                    descricao: "Baixar planilha com as cotações",
                    icone: "file-excel-o",
                    variante: "success",
                    onClick: () =>
                        this.downloadExcelCotacoes(
                            row.idSolicitacaoCompra,
                            "Cotações " + row.solicitacaoCompra.codigo + ".xlsx"
                        ),
                    visible: u != null && u.comprasOrdensCompraFinalizadasPDF,
                },
                {
                    label: "Clonar",
                    descricao: "Criar nova solicitação a partir desta",
                    icone: "copy",
                    variante: "success",
                    onClick: () => this.abreModalClonarSolicitacao(row.solicitacaoCompra),
                    visible:
                        row.solicitacaoCompra &&
                        row.solicitacaoCompra.idStatusSolicitacaoCompra >= 4,
                },
            ];
        },
        abrirModalAcoesPedidoCancelado(row) {
            this.modalAcoes_Itens = this.montaAcoesPedidoCancelado(row);
            this.modalAcoes_Titulo = "Ações do pedido " + row.codigo;
            this.modalAcoes_Exibir = true;
        },

        formataData: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
        },

        formataDataSemHora: function (data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        onChange() {
            this.upload_Files = [...this.$refs.file.files];

            this.uploadFiles();
        },

        descricaoDef({ codigo, descricao }) {
            return `${codigo} - ${descricao}`;
        },

        descricaoMaterial({ descricao, unidadeMaterial }) {
            return `${descricao} - ${unidadeMaterial.codigo}`;
        },

        nameWithLang({ descricao }) {
            return `${descricao}`;
        },

        descricaoObra({ codigo, cliente }) {
            return `${codigo} - ${cliente.nomeFantasia}`;
        },

        modalReabrirSolicitacao_RemoverMaterial: function (idMaterial) {
            this.modalReabrirSolicitacao_MateriaisAdicionados.forEach((material) => {
                if (material.id == idMaterial) {
                    this.modalReabrirSolicitacao_ValorTotal = this.modalReabrirSolicitacao_ValorTotal - material.valorTotal;
                    this.modalReabrirSolicitacao_MateriaisAdicionados.splice(this.modalReabrirSolicitacao_MateriaisAdicionados.indexOf(material), 1);
                }
            });
        },

        modalReabrirSolicitacaoAdicionaMaterial: function () {
            if (this.modalReabrirSolicitacao_MaterialSelecionado != null && this.modalReabrirSolicitacao_Quantidade > 0) {
                var objetoMaterial = {
                    id: this.modalReabrirSolicitacao_MateriaisAdicionados.length + 1,
                    idMaterial: this.modalReabrirSolicitacao_MaterialSelecionado.id,
                    material: this.modalReabrirSolicitacao_MaterialSelecionado.descricao,
                    quantidade: this.modalReabrirSolicitacao_Quantidade,
                    valorUnitario: this.modalReabrirSolicitacao_ValorUnitario,
                    valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(this.modalReabrirSolicitacao_ValorUnitario),
                    valorTotal: this.modalReabrirSolicitacao_Quantidade *
                        this.modalReabrirSolicitacao_ValorUnitario,
                    valorTotalFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(this.modalReabrirSolicitacao_Quantidade * this.modalReabrirSolicitacao_ValorUnitario),
                };

                this.modalReabrirSolicitacao_ValorTotal = this.modalReabrirSolicitacao_ValorTotal + objetoMaterial.valorTotal;

                this.modalReabrirSolicitacao_MateriaisAdicionados.push(objetoMaterial);

                this.modalReabrirSolicitacao_Quantidade = 0;
                this.modalReabrirSolicitacao_ValorUnitario = 0;
                this.modalReabrirSolicitacao_MaterialSelecionado = null;
            }
            else
                this.$swal("", "Verifique o material selecionado e a quantidade informada", "error");
        },
        abreModalClonarSolicitacao(solicitacao) {
            console.log(solicitacao);
            this.modalReabrirSolicitacao_Titulo = 'Clonar solicitação ' + solicitacao.codigo;
            this.modalReabrirSolicitacao_IdSolicitacao = solicitacao.id;
            this.modalReabrirSolicitacao_Exibir = true;
            this.modalReabrirSolicitacao_ObraSelecionada = null;
            this.modalReabrirSolicitacao_MaterialSelecionado = null;
            this.modalReabrirSolicitacao_Quantidade = 0;
            this.modalReabrirSolicitacao_ValorUnitario = 0;
            this.modalReabrirSolicitacao_ValorTotal = 0;
            this.modalReabrirSolicitacao_MateriaisAdicionados = [];

            solicitacao.materiais.forEach(material => {
                var objetoMaterial = {
                    id: this.modalReabrirSolicitacao_MateriaisAdicionados.length + 1,
                    idMaterial: material.material.id,
                    material: material.material.descricao,
                    quantidade: material.quantidade,
                    valorUnitario: material.valorUnitarioEstimado,
                    valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(material.valorUnitarioEstimado),
                    valorTotal: material.quantidade *
                        material.valorUnitarioEstimado,
                    valorTotalFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(material.quantidade * material.valorUnitarioEstimado),
                    cotacoes: material.cotacoes,
                };

                this.modalReabrirSolicitacao_ValorTotal = this.modalReabrirSolicitacao_ValorTotal + objetoMaterial.valorTotal;

                this.modalReabrirSolicitacao_MateriaisAdicionados.push(objetoMaterial);
            });
        },

        exibirModalNotasFiscais: function (pedido) {
            this.modalNotasFiscais_Titulo = 'Notas Fiscais - ' + pedido.codigo
            this.modalNotasFiscais_Pedido = pedido;
            this.modalNotasFiscais_Notas = pedido.notasFiscais,
                this.modalNotasFiscais_SaldosCancelados = pedido.cancelamentosSaldo;
            this.modalNotasFiscais_Exibir = true;
            this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.valorTotal;

            for (var i = 0; i < this.modalNotasFiscais_Pedido.cancelamentosSaldo.length; i++) {
                this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.saldo - this.modalNotasFiscais_Pedido.cancelamentosSaldo[i].valor;
            }

            for (var i = 0; i < this.modalNotasFiscais_Pedido.notasFiscais.length; i++) {
                if (this.modalNotasFiscais_Pedido.notasFiscais[i].aprovada != false)
                    this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.saldo - this.modalNotasFiscais_Pedido.notasFiscais[i].valor;
            }

        },

        downloadNotaFiscal(id, nomeArquivo) {
            this.isLoading = true;

            ApiService.downloadFile('PedidoCompra', id, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {

                    const blob = new Blob([result.data], {
                        type: result.contentType,
                    });

                    const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
                }
            });
        },

        downloadPdfArquivoSolicitacaoCompra(id, codigo) {
            this.isLoading = true;

            ApiService.downloadPdfArquivoSolicitacaoCompra(id, (result) => {
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

        downloadPdfCotacoes(id, codigo) {
            this.isLoading = true;

            ApiService.downloadPdfCotacoes(id, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", "Erro ao baixar cotações", "error");
                } else {

                    const blob = new Blob([result.data], {
                        type: result.contentType,
                    });

                    const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
                }
            });
        },

        downloadExcelCotacoes(id, codigo) {
            this.isLoading = true;

            ApiService.downloadExcelCotacoes(id, (result) => {
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

        downloadPdfArquivoCompra(id, codigo) {

            var exibirDataPagamento = true;

            this.$swal({
                title: "Atenção",
                text: "Deseja exibir a data do pagamento?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sim",
                cancelButtonText: "Não",
            }).then((result) => {
                if (result.isConfirmed) {
                    exibirDataPagamento = true;
                }
                else {
                    exibirDataPagamento = false;
                }

                this.isLoading = true;

                ApiService.downloadPdfPedidoCompra(id, exibirDataPagamento, (result) => {
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
            });
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

        listaObras: function () {
            this.obras = [];

            ApiService.getAll("Obra", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.obras = result.data;
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

        listaFornecedores: function () {
            this.fornecedores = [];

            ApiService.getAll("Fornecedor", true, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.fornecedores = result.data;
                }
            });
        },

        listaCondicoesDePagamento: function () {
            this.condicoesPagamento = [];

            ApiService.getAll("CondicaoPagamento", true, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.condicoesPagamento = result.data;
                }
            });
        },

        listaClientes: function () {
            this.clientes = [];

            ApiService.getAll("Cliente", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.clientes = result.data;
                }
            });
        },

        downloadFile(id, nomeArquivo) {
            this.isLoading = true;

            ApiService.downloadFile(this.modalArquivos_Controller, id, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {

                    const blob = new Blob([result.data], {
                        type: result.contentType,
                    });

                    const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
                }
            });
        },

        excluirArquivo(id) {
            this.isLoading = true;

            ApiService.deleteArquivo(this.modalArquivos_Controller, id, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                }
                else {
                    this.obtemArquivos(this.modalArquivos_IdSolicitacaoOuPedidoCompra);
                    this.$swal("Arquivo excluído com sucesso", result.message, "success");
                }
            });
        },
        uploadFiles() {
            this.isLoading = true;

            const formData = new FormData();
            this.upload_Files.forEach((file) => {
                formData.append("arquivo", file);
            });

            ApiService.uploadFile(this.modalArquivos_Controller, formData, this.modalArquivos_IdSolicitacaoOuPedidoCompra, (result) => {

                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    this.upload_Files = [];
                    this.obtemArquivos(this.modalArquivos_IdSolicitacaoOuPedidoCompra);
                    this.$swal("Arquivo enviado com sucesso", result.message, "success");
                }
            });
        },

        obtemArquivos: function (idSolicitacaoOuPedidoCompra) {
            ApiService.getArquivos(this.modalArquivos_Controller, idSolicitacaoOuPedidoCompra, (result) => {
                this.isLoading = false;

                if (result.status == 200) {
                    this.modalArquivos_Arquivos = result.data;
                }
            });
        },

        limpaFiltros() {
            this.filtro_ObraSelecionada = [];
            this.filtro_MateriaisSelecionados = [];
            this.filtro_FornecedoresSelecionados = [];
            this.filtro_ClienteSelecionado = [];
            this.filtro_DataInicial = null;
            this.filtro_DataFinal = null;
        },

        obtemPedidosFinalizadosFiltrados() {

            var idsObra = [];
            this.filtro_ObraSelecionada.forEach(x => {
                idsObra.push(x.id);
            });

            var idsCliente = [];
            this.filtro_ClienteSelecionado.forEach(x => {
                idsCliente.push(x.id);
            });

            var idsMaterial = [];
            this.filtro_MateriaisSelecionados.forEach(x => {
                idsMaterial.push(x.id);
            });

            var idsFornecedor = [];
            this.filtro_FornecedoresSelecionados.forEach(x => {
                idsFornecedor.push(x.id);
            });

            var objetoParametros = {
                idsObra: idsObra,
                idsCliente: idsCliente,
                idsMateriais: idsMaterial,
                idsFornecedor: idsFornecedor,
                dataSolicitacaoInicial: this.filtro_DataInicial,
                dataSolicitacaoFinal: this.filtro_DataFinal
            };

            this.isLoading = true;

            ApiService.obtemPedidosFinalizadosFiltrados(objetoParametros, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidosCancelados = result.data;
                }
            });

        },

        reabrirSolicitacao: function () {

            if (this.modalReabrirSolicitacao_DataEntrega == null) {
                this.$swal("", "Selecione uma data de entrega", "error");
                return;
            }

            if (this.modalReabrirSolicitacao_ObraSelecionada == null)
                this.$swal("", "Selecione uma obra", "error");
            else {
                this.isLoading = true;

                var objetoReaberturaSolicitacao = {
                    idSolicitacaoCompra: this.modalReabrirSolicitacao_IdSolicitacao,
                    idObraSelecionada: this.modalReabrirSolicitacao_ObraSelecionada.id,
                    dataEntrega: this.modalReabrirSolicitacao_DataEntrega,
                    materiais: this.modalReabrirSolicitacao_MateriaisAdicionados
                };

                ApiService.reabrirSolicitacaoCompra(objetoReaberturaSolicitacao,
                    (resultAPI) => {
                        this.isLoading = false;
                        console.log(resultAPI);
                        if (resultAPI.status != 200) {
                            this.$swal("", resultAPI.data, "error");
                        } else {
                            this.modalReabrirSolicitacao_Titulo = "";
                            this.modalReabrirSolicitacao_ObraSelecionada = null;
                            this.modalReabrirSolicitacao_IdSolicitacao = 0;

                            this.$swal("", "Solicitação clonada", "success");

                            this.modalReabrirSolicitacao_Exibir = false;
                        }
                    }
                );

            }
        },
    },
    mounted() {
        this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaMateriais();
        this.listaObras();
        this.listaDefs();
        this.listaFornecedores();
        this.listaClientes();
        this.listaCondicoesDePagamento();
    }
}
</script>

<style src="./RelatorioPedidoCompra.scss" lang="scss" />