<template>
  <div class="fluxoCaixaConsolidado-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>

    <h1 class="page-title">Fluxo de Caixa Consolidado</h1>

    <!-- ====== FILTRO / AÇÕES ====== -->
    <Widget customHeader class="estiloWidget kbar">
      <b-row class="filter-row">
        <b-col md="3">
          <label class="mr-3">Mês/Ano Inicial</label>
          <b-form-input
            v-model="mesSelecionado"
            style="color: white"
            placeholder="MM/AAAA"
            v-mask="'##/####'"
          />
        </b-col>
        <b-col md="3">
          <label class="mr-3">Mês/Ano Final</label>
          <b-form-input
            v-model="mesFinalSelecionado"
            style="color: white"
            placeholder="MM/AAAA"
            v-mask="'##/####'"
          />
        </b-col>
        <b-col md="6" class="actions">
          <b-button @click="obtemFluxoCaixaConsolidado()" variant="success" class="btn-wide">Consultar</b-button>
          <button @click="modalSaldoInicial_Exibir = true;" type="button" class="btn btn-outline-success">Saldos Iniciais</button>
          <button @click="downloadExcel()" type="button" class="btn btn-outline-success">Download Excel</button>
          <button @click="downloadPDF()" type="button" class="btn btn-outline-danger">Download PDF</button>
        </b-col>
      </b-row>
    </Widget>

    <!-- ====== KPIs — SOMENTE REALIZADO ====== -->
    <div class="kpis">
      <div class="kpi">
        <div class="k">Receitas (realizado)</div>
        <div class="v mono text-pos">{{ formatBRL(kpiReceitasRealizadas) }}</div>
      </div>
      <div class="kpi">
        <div class="k">Despesas (realizado)</div>
        <div class="v mono text-neg">{{ formatBRL(kpiDespesasRealizadas) }}</div>
      </div>
      <div class="kpi emph">
        <div class="k">Saldo final (realizado)</div>
        <div class="v mono">{{ formatBRL(kpiSaldoFinalRealizado) }}</div>
      </div>
    </div>

    <!-- ====== TABELA PRINCIPAL (sem scroll horizontal) ====== -->
    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col lg="12">
          <div class="table-card table-card--scroll-x">
            <table class="estilo-tabela tabela-identidade fc-table">
              <thead>
                <tr>
                  <th class="estilo-cabecalho">Dia</th>
                  <th class="estilo-cabecalho texto-centro">OC/PI</th>
                  <th class="estilo-cabecalho texto-centro">NF</th>
                  <th class="estilo-cabecalho texto-centro">ETO</th>
                  <th class="estilo-cabecalho texto-centro">Folha Pag.</th>
                  <th class="estilo-cabecalho texto-centro">Imposto</th>
                  <th class="estilo-cabecalho texto-centro">Despesas Fixas</th>
                  <th class="estilo-cabecalho texto-centro">Reserva</th>
                  <th class="estilo-cabecalho texto-centro">Outros</th>
                  <th class="estilo-cabecalho texto-centro">Transferência</th>
                  <th class="estilo-cabecalho texto-centro">Total Diário</th>
                  <th class="estilo-cabecalho texto-centro">Faturado</th>
                  <th class="estilo-cabecalho texto-centro">A faturar</th>
                  <th class="estilo-cabecalho texto-centro">Estornos/Crédito</th>
                  <th class="estilo-cabecalho texto-centro">Saldo</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in fluxoCaixaConsolidado" :key="'fc-' + index" :class="rowClass(row, 'row')">
                  <td class="estilo-celula">
                    <div class="day-cell">
                      <div class="day">{{ row.dia == null ? '' : formataData(row.dia) }}</div>
                      <div class="note" v-if="row.diaObservacao && row.dia == null">{{ row.diaObservacao }}</div>
                    </div>
                  </td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.ordemCompraPedidoInterno) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.notaFiscal) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.eto) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.folhaPagamento) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.imposto) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.despesasFixas) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.reserva) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.outros) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.transferencia) }}</span></td>
                  <td class="estilo-celula texto-centro"><span :class="['num', 'mono', 'text-neg']">{{ fmt(row.totalDiario) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.aReceberFaturado) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.aReceberAFaturar) }}</span></td>
                  <td class="estilo-celula texto-centro"><span class="num mono">{{ fmt(row.estornos) }}</span></td>
                  <td class="estilo-celula texto-centro"><span :class="['num', 'mono', Number(row.saldo) < 0 ? 'text-neg' : null]">{{ fmt(row.saldo, true) }}</span></td>
                  <td class="estilo-celula texto-centro">
                    <div v-if="row.dia != null" class="actions-cell">
                      <button type="button" @click="abrirModalDetalhe(row, true)" class="btn btn-outline-info btn-icon" :title="'Detalhar créditos do dia'">
                        <i class="fa fa-eye"></i>
                      </button>
                      <button type="button" @click="abrirModalDetalhe(row, false)" class="btn btn-outline-danger btn-icon" :title="'Detalhar débitos do dia'">
                        <i class="fa fa-eye"></i>
                      </button>
                    </div>
                  </td>
                </tr>
                <tr v-if="!fluxoCaixaConsolidado || fluxoCaixaConsolidado.length === 0">
                  <td class="estilo-celula texto-centro" colspan="16">Nenhum registro encontrado</td>
                </tr>
              </tbody>
            </table>
          </div>
        </b-col>
      </b-row>
    </Widget>

    <!-- ==================== MODAIS (mantidos) ==================== -->

    <b-modal :no-close-on-backdrop="true" id="modalDetalhe" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalDetalhe_Exibir"
      size="xl">

      <!-- ── HERO ── -->
      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Fluxo de Caixa</span>
          <h3 class="afm-hero__title">{{ modalDetalhe_Credito ? 'Detalhes de créditos' : 'Detalhes de débitos' }}</h3>
          <p class="afm-hero__description">
            {{ modalDetalhe_Dia ? 'Lançamentos do dia ' + formataData(modalDetalhe_Dia.dia) : '' }}
          </p>
        </div>
        <div class="afm-hero__pill">
          <span>{{ modalDetalhe_Fluxos.length }}</span>
          <small>lançamento(s)</small>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

          <div v-if="modalDetalhe_Fluxos.length > 0" class="table-card table-card--fluid table-card--scroll-x">
            <table class="estilo-tabela tabela-identidade">
              <thead>
                <tr>
                  <th class="estilo-cabecalho">Obra</th>
                  <th class="estilo-cabecalho">DEF</th>
                  <th class="estilo-cabecalho">Ped. Compra</th>
                  <th class="estilo-cabecalho">Ped. Interno</th>
                  <th class="estilo-cabecalho">Cliente</th>
                  <th class="estilo-cabecalho">Fornecedor</th>
                  <th class="estilo-cabecalho">Nota Fiscal</th>
                  <th class="estilo-cabecalho">Valor</th>
                  <th class="estilo-cabecalho">Data Pagamento</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="row in modalDetalhe_Fluxos" :key="row.id">
                  <td class="estilo-celula">{{ row.codigoObra == null ? '—' : row.codigoObra }}</td>
                  <td class="estilo-celula">{{ row.codigoDef == null ? '—' : row.codigoDef }}</td>
                  <td class="estilo-celula">{{ row.codigoPedidoCompra == null ? '—' : row.codigoPedidoCompra }}</td>
                  <td class="estilo-celula">{{ row.codigoPedidoInterno == null ? '—' : row.codigoPedidoInterno }}</td>
                  <td class="estilo-celula">{{ row.idCliente == null ? '—' : row.nomeCliente }}</td>
                  <td class="estilo-celula">{{ row.fornecedor == null ? (row.usuarioBeneficiario == null ? '—' : row.usuarioBeneficiario.nome) : row.fornecedor.nomeFantasia }}</td>
                  <td class="estilo-celula">{{ row.numeroNotaFiscalFaturamento == null ? row.numeroNotaFiscalPedidoCompra : row.numeroNotaFiscalFaturamento }}</td>
                  <td class="estilo-celula mono" style="text-align:right;">
                    {{ row.valor == 0 ? '—' : new Intl.NumberFormat('pt-BR', {style:'currency', currency:'BRL'}).format(row.valor) }}
                  </td>
                  <td class="estilo-celula">{{ row.dataPagamento == null ? '—' : formataDataCompleta(row.dataPagamento) }}</td>
                  <td class="estilo-celula texto-centro">
                    <button type="button" @click="editarRegistroFluxoCaixa(row)" class="btn btn-outline-success btn-icon" title="Editar">
                      <i class="fa fa-pencil"></i>
                    </button>
                    <button type="button"
                      @click="modalDataPagamentoRecebimento_IdFluxoCaixa = row.id; modalDataPagamentoRecebimento_NovaData = null; modalDataPagamentoRecebimento_Exibir = true;"
                      v-if="!row.pagamentoEfetuado"
                      class="btn btn-outline-info btn-icon ml-1"
                      :title="modalDetalhe_Credito">
                      <i class="fa fa-thumbs-o-up"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>

          <div v-else class="afm-empty-state">
            <i class="fa fa-inbox"></i>
            <div>
              <strong>Nenhum lançamento encontrado</strong>
              <p>Não há registros para o dia selecionado.</p>
            </div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary">
            <span>Total de lançamentos:</span>
            <strong>{{ modalDetalhe_Fluxos.length }}</strong>
          </div>
          <div class="afm-footer__actions">
            <b-button @click="modalDetalhe_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalDataPagamentoRecebimento" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalDataPagamentoRecebimento_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Pagamento / Recebimento</span>
          <h3 class="afm-hero__title">Data de pagamento ou recebimento</h3>
          <p class="afm-hero__description">Informe a nova data de efetivação.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Nova data</label>
            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalDataPagamentoRecebimento_NovaData" format="dd/MM/yyyy" type="date" :open.sync="open" />
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalDataPagamentoRecebimento_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
            <b-button v-on:click="informarPagamentoOuRecebimento()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Informar data
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalNovoValorOCPI" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalNovoOCPI_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">OC / PI</span>
          <h3 class="afm-hero__title">Novo Valor</h3>
          <p class="afm-hero__description">Cadastre um novo valor previsto para o item.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalNovoOCPI_DataPrevista" format="dd/MM/yyyy" type="date" :open.sync="open" />
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor previsto ajustado</label>
                <Money v-model="modalNovoOCPI_ValorPrevisto" v-bind="money" class="money-dark" />
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalNovoOCPI_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
            <b-button v-on:click="cadastrarOCPIExtra()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Cadastrar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalEdicao" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalEdicao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Edição</span>
          <h3 class="afm-hero__title">Edição do Fluxo de Caixa</h3>
          <p class="afm-hero__description">Altere DEF, valor e gerencie as previsões lançadas.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
        <b-row>
          <b-col>
            <label class="mr-3">Informe o novo DEF:</label>
            <multiselect v-model="modalEdicao_DefSelecionado" :multiple="false" :options="defs" :custom-label="descricaoDef" select-label="Selecionar" placeholder="Selecione um DEF" />
          </b-col>
        </b-row>

        <br />
        <b-row>
          <b-col>
            <label class="mr-3">Informe o novo valor:</label><br />
            <Money v-model="modalEdicao_Valor" v-bind="money" class="money-dark wide-190" />
          </b-col>
        </b-row>

        <br />
        <Widget customHeader class="estiloWidget">
          <label>Saldo: {{ new Intl.NumberFormat('pt-BR', {style:'currency', currency:'BRL'}).format(this.modalEdicao_somatorioOCPI) }}</label>
          <b-row>
            <b-col>
              <div class="table-card table-card--fluid table-card--scroll-x">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho texto-centro">Data Prevista</th>
                      <th class="estilo-cabecalho texto-centro">Valor Previsto</th>
                      <th class="estilo-cabecalho texto-centro">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(row, index) in modalEdicao_ValoresOCPI" :key="'vocpi-' + (row.id || index)">
                      <td class="estilo-celula texto-centro">{{ formataDataCompleta(row.dataPrevista) }}</td>
                      <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR', { style:'currency', currency:'BRL' }).format(row.valorPrevisto) }}</td>
                      <td class="estilo-celula texto-centro">
                        <button type="button" @click="excluiValorOCPI(row)" class="btn mb-3 mr-3 btn-danger">
                          <i class="fa fa-close" title="Excluir Valor"></i>
                        </button>
                      </td>
                    </tr>
                    <tr v-if="!modalEdicao_ValoresOCPI || modalEdicao_ValoresOCPI.length === 0">
                      <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </b-col>
          </b-row>

          <b-row>
            <b-col style="text-align: right">
              <b-button
                v-on:click="modalNovoOCPI_DataPrevista = null; modalNovoOCPI_ValorPrevisto = 0; modalNovoOCPI_Exibir = true;"
                v-if="this.modalEdicao_somatorioOCPI > 0"
                variant="outline-success"
                class="width-200 mb-3 mr-3"
              ><span>Novo Valor</span></b-button>
            </b-col>
          </b-row>
        </Widget>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
            <b-button @click="editarFluxoCaixa()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalSaldoInicial" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalSaldoInicial_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Fluxo de Caixa</span>
          <h3 class="afm-hero__title">Saldo Inicial</h3>
          <p class="afm-hero__description">Consulte e edite os saldos iniciais por mês.</p>
        </div>
        <div class="afm-hero__pill">
          <span>{{ saldosIniciais ? saldosIniciais.length : 0 }}</span>
          <small>registro(s)</small>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div v-if="saldosIniciais && saldosIniciais.length > 0">
            <div class="table-card table-card--fluid table-card--scroll-x">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Mês</th>
                    <th class="estilo-cabecalho texto-centro">Ano</th>
                    <th class="estilo-cabecalho texto-centro">Saldo</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in saldosIniciais" :key="'si-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.mes }}</td>
                    <td class="estilo-celula texto-centro">{{ row.ano }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR', { style:'currency', currency:'BRL' }).format(row.saldo) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="editarRegistroSaldoInicial(row)" class="btn btn-outline-success afm-action-btn">
                        <i class="fa fa-pencil"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <div v-else class="afm-empty-state">
            <i class="fa fa-calendar-o"></i>
            <div><strong>Nenhum saldo inicial</strong><p>Não há saldos iniciais cadastrados.</p></div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalSaldoInicial_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalSaldoInicialEdicao" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalSaldoInicialEdicao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Saldo Inicial</span>
          <h3 class="afm-hero__title">Edição Saldo Inicial</h3>
          <p class="afm-hero__description">{{ this.modalSaldoInicialEdicao_SaldoInicial != null ? ('Editando ' + this.modalSaldoInicialEdicao_SaldoInicial.mes + '/' + this.modalSaldoInicialEdicao_SaldoInicial.ano) : 'Ajuste o saldo inicial do mês.' }}</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Saldo Inicial</label>
                <Money v-model="modalSaldoInicialEdicao_Saldo" v-bind="money" />
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalSaldoInicialEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
            <b-button @click="editarSaldoInicial()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Salvar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>
  </div>
</template>

<script>
import moment from "moment";
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import Multiselect from "vue-multiselect";
import ToggleButton from "vue-js-toggle-button";
import { VueMaskDirective } from "v-mask";
import CurrencyInput from "../../../components/CurrencyInput.vue";
import { Money } from 'v-money';
import ApiService from "@/services/api.service.js";
import VueTheMask from 'vue-the-mask'
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";

Vue.use(VueTheMask)
Vue.directive("mask", VueMaskDirective);
Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
  name: 'FluxoCaixaConsolidado',
  components: { Widget, Loading, CurrencyInput, Multiselect, Money, DatePickerMask },
  data() {
    return {
      open: false,
      money: { decimal: ',', thousands: '.', prefix: 'R$ ', precision: 2, masked: false },
      dataMinima: (new Date()).setDate(new Date().getDate() -1),
      isLoading: false,

      modalDataPagamentoRecebimento_Exibir: false,
      modalDataPagamentoRecebimento_NovaData: null,
      modalDataPagamentoRecebimento_IdFluxoCaixa: 0,

      modalSaldoInicial_Exibir: false,

      modalSaldoInicialEdicao_Exibir: false,
      modalSaldoInicialEdicao_SaldoInicial: null,
      modalSaldoInicialEdicao_Saldo: 0.0,

      modalDetalhe_Exibir: false,
      modalDetalhe_Dia: null,
      modalDetalhe_Fluxos: [],
      modalDetalhe_Credito: '',

      modalEdicao_Exibir: false,
      modalEdicao_Fluxo: null,
      modalEdicao_DefSelecionado: null,
      modalEdicao_Data: null,
      modalEdicao_Valor: 0.0,
      modalEdicao_ValoresOCPI: [],
      modalEdicao_somatorioOCPI: 0.0,

      modalNovoOCPI_Exibir: false,
      modalNovoOCPI_DataPrevista: null,
      modalNovoOCPI_ValorPrevisto: 0.0,

      fluxoCaixaConsolidado: [],
      mesSelecionado: null,
      mesFinalSelecionado: null,
      defs: [],
      saldosIniciais: [],

      colunasEdicaoValorOCPI: ["dataPrevista", "valorPrevisto", "acoes"],
      opcoesEdicaoValorOCPI: {
        perPage: 1000, perPageValues: [],
        headings: { dataPrevista: "Data Prevista", valorPrevisto: "Valor Previsto", acoes: "Ações" },
        clientSorting: false, sortable: [], filterable: false,
        texts: { filterPlaceholder: "Procurar por", count:"Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
          first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado" }
      },

      colunasSaldoInicial: ["mes", "ano", "saldo", "acoes"],
      opcoesSaldoInicial: {
        perPage: 1000, perPageValues: [],
        headings: { mes: "Mês", ano: "Ano", saldo: "Saldo", acoes: "Ações" },
        clientSorting: false, sortable: [], filterable: false,
        texts: { filterPlaceholder: "Procurar por", count:"Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
          first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado" }
      },

      colunasDetalhe: ["obra", "def", "pedidoCompra", "pedidoInterno", "cliente", "fornecedor", "notaFiscal", "valor", "dataPagamento", "acoes"],
      opcoesDetalhes: {
        perPage: 1000, perPageValues: [],
        headings: {
          obra: "Obra", def: "DEF", pedidoCompra: "Pedido Compra", pedidoInterno: "Pedido Interno",
          cliente: "Cliente", fornecedor: "Fornecedor", notaFiscal: "Nota Fiscal",
          valor: "Valor", dataPagamento: "Data Pagamento", acoes: "Ações"
        },
        clientSorting: false, sortable: [], filterable: false,
        texts: { filterPlaceholder: "Procurar por", count:"Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
          first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado" }
      },

      /* Removida a coluna anterior ao "Dia" */
      fields: [
        { key: 'dia',                      label: 'Dia',              thStyle: { width: '70px',  minWidth: '70px'  } },
        { key: 'ordemCompraPedidoInterno', label: 'OC/PI',            thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'notaFiscal',               label: 'NF',               thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'eto',                      label: 'ETO',              thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'folhaPagamento',           label: 'Folha Pag.',       thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'imposto',                  label: 'Imposto',          thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'despesasFixas',            label: 'Despesas Fixas',   thStyle: { width: '110px', minWidth: '110px' } },
        { key: 'reserva',                  label: 'Reserva',          thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'outros',                   label: 'Outros',           thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'transferencia',            label: 'Transferência',    thStyle: { width: '110px', minWidth: '110px' } },
        { key: 'totalDiario',              label: 'Total Diário',     thStyle: { width: '110px', minWidth: '110px' } },
        { key: 'aReceberFaturado',         label: 'Faturado',         thStyle: { width: '110px', minWidth: '110px' } },
        { key: 'aReceberAFaturar',         label: 'A faturar',        thStyle: { width: '100px', minWidth: '100px' } },
        { key: 'estornos',                 label: 'Estornos/Crédito', thStyle: { width: '120px', minWidth: '120px' } },
        { key: 'saldo',                    label: 'Saldo',            thStyle: { width: '110px', minWidth: '110px' } },
        { key: 'acoes',                    label: 'Ações',            thStyle: { width: '80px',  minWidth: '80px'  } },
      ],
    };
  },
  computed: {
    // Apenas linhas de "dias" (realizado)
    dailyRows() {
      return (this.fluxoCaixaConsolidado || []).filter(r => r && r.dia);
    },
    // Somatórios realizados
    gastosKeys() {
      return ['ordemCompraPedidoInterno','notaFiscal','eto','folhaPagamento','imposto','despesasFixas','reserva','outros','transferencia'];
    },
    receitasKeys() {
      return ['aReceberFaturado','aReceberAFaturar','estornos'];
    },
    kpiDespesasRealizadas() {
      const rows = this.dailyRows;
      return rows.reduce((acc, r) => {
        return acc + this.gastosKeys.reduce((a,k)=> a + (Number(r[k])||0), 0);
      }, 0);
    },
    kpiReceitasRealizadas() {
      const rows = this.dailyRows;
      return rows.reduce((acc, r) => {
        return acc + this.receitasKeys.reduce((a,k)=> a + (Number(r[k])||0), 0);
      }, 0);
    },
    kpiSaldoFinalRealizado() {
      const rows = this.dailyRows;
      const last = rows.length ? rows[rows.length - 1] : null;
      return last ? Number(last.saldo || 0) : 0;
    },
  },
  methods: {
    rowClass(item, type) {
      if (!item || type !== 'row') return
      // Mantém coloração de totais na tabela (como você pediu)
      if (item.diaObservacao === 'Total P.') return 'table-TotalP'
      if (item.diaObservacao === 'Total R.') return 'table-TotalR'
      // separador semanal opcional
      if (item.dia) {
        const d = new Date(item.dia)
        if (d.getDay && d.getDay() === 1) return 'week-sep'
      }
    },
    descricaoDef({ codigo, descricao }) { return `${codigo} - ${descricao}`; },
    formataData(data) { return moment(String(new Date(data))).format("DD/MM"); },
    formataDataCompleta(data) { return moment(String(new Date(data))).format("DD/MM/YYYY"); },

    fmt(val, canZero=false) {
      const n = Number(val || 0);
      if (!canZero && (n === 0 || isNaN(n))) return '';
      return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(n).replace('R$', '');
    },
    formatBRL(n) {
      return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(Number(n||0));
    },

    excluiValorOCPI(valorOCPI) {
      var listaValoresOCPI = [];
      this.modalEdicao_ValoresOCPI.forEach(x => {
        if (x.idInterno != valorOCPI.idInterno) {
          x.idInterno = listaValoresOCPI.length;
          listaValoresOCPI.push(x);
        }
      });
      this.modalEdicao_ValoresOCPI = listaValoresOCPI;
      this.obtemSomatorioOCPI();
    },
    cadastrarOCPIExtra() {
      if (this.modalNovoOCPI_DataPrevista == null) {
        this.$swal("Data da medição inválida", "", "error");
        return;
      }
      if (this.modalNovoOCPI_ValorPrevisto <= 0) {
        this.$swal("Valor da medição inválido", "", "error");
        return;
      }
      var objetoOCPI = {
        idInterno: this.modalEdicao_ValoresOCPI.length,
        dataPrevista: this.modalNovoOCPI_DataPrevista,
        valorPrevisto: this.modalNovoOCPI_ValorPrevisto,
      };
      this.modalEdicao_ValoresOCPI.push(objetoOCPI);
      this.obtemSomatorioOCPI();
      this.modalNovoOCPI_Exibir = false;
    },
    obtemSomatorioOCPI() {
      var result = this.modalEdicao_Valor;
      this.modalEdicao_ValoresOCPI.forEach(x => { result -= x.valorPrevisto; });
      this.modalEdicao_somatorioOCPI = result;
    },
    editarRegistroSaldoInicial(saldoInicial) {
      this.modalSaldoInicialEdicao_SaldoInicial = saldoInicial;
      this.modalSaldoInicialEdicao_Saldo = this.modalSaldoInicialEdicao_SaldoInicial.saldo;
      this.modalSaldoInicialEdicao_Exibir = true;
    },
    informarPagamentoOuRecebimento() {
      if(this.modalDataPagamentoRecebimento_NovaData == null){
        this.$swal('', 'Data inválida', 'error');
        return;
      }
      this.isLoading = true;
      ApiService.informarPagamentoOuRecebimentoFluxoCaixa(this.modalDataPagamentoRecebimento_IdFluxoCaixa, this.modalDataPagamentoRecebimento_NovaData, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalDataPagamentoRecebimento_Exibir = false;
          this.modalDetalhe_Exibir = false;
          this.obtemFluxoCaixaConsolidado();
        }
      });
    },
    editarSaldoInicial() {
      this.isLoading = true;
      this.modalSaldoInicialEdicao_SaldoInicial.saldo = this.modalSaldoInicialEdicao_Saldo;
      ApiService.put('FluxoCaixaSaldoInicial', this.modalSaldoInicialEdicao_SaldoInicial, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalSaldoInicialEdicao_Exibir = false;
          this.modalSaldoInicial_Exibir = false;
          this.listaSaldosIniciais();
        }
      });
    },
    editarFluxoCaixa() {
      console.log(this.modalEdicao_ValoresOCPI);
      console.log(this.modalEdicao_ValoresOCPI.length);
      return;
    },
    editarRegistroFluxoCaixa(fluxoCaixa) {
      this.defs.forEach(x => { if (x.id == fluxoCaixa.idDef) { this.modalEdicao_DefSelecionado = x; } });
      this.modalEdicao_Valor = fluxoCaixa.valor;
      this.modalEdicao_Data = fluxoCaixa.dataPagamento;
      this.modalEdicao_Fluxo = fluxoCaixa;
      this.modalEdicao_ValoresOCPI = [];
      this.modalEdicao_ValoresOCPI.push({ idInterno: 0, dataPrevista: fluxoCaixa.dataPagamento, valorPrevisto: fluxoCaixa.valor });
      this.modalEdicao_Exibir = true;
    },
    abrirModalDetalhe(dia, credito) {
      var data = new Date(dia.dia);
      var objetoRequisicao = { data: data.getDate() + "/" + (data.getMonth() + 1) + "/" + data.getFullYear(), credito: credito };
      ApiService.obtemFluxoCaixaDia(objetoRequisicao, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalDetalhe_Fluxos = result.data;
          this.modalDetalhe_Credito = credito ? 'Informar recebimento' : 'Informar pagamento';
        }
        this.modalDetalhe_Exibir = true;
        this.modalDetalhe_Dia = dia;
      });
    },
    obtemFluxoCaixaConsolidado() {
      this.isLoading = true;
      var objetoRequisicao = { data: this.mesSelecionado, dataFinal: this.mesFinalSelecionado };
      ApiService.obtemFluxoCaixaConsolidado(objetoRequisicao, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.fluxoCaixaConsolidado = result.data;
        }
      });
    },
    downloadExcel() {
      var objetoRequisicao = { data: this.mesSelecionado };
      this.isLoading = true;
      ApiService.downloadExcelFluxoCaixa(objetoRequisicao, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", "Erro ao baixar excel", "error");
        } else {
          const blob = new Blob([result.data], { type: result.contentType });
          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },
    downloadPDF() {
      this.isLoading = true;
      var objetoRequisicao = { data: this.mesSelecionado, dataFinal: this.mesFinalSelecionado };
      ApiService.downloadFluxoCaixaPDF(objetoRequisicao, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", "Erro ao baixar PDF", "error");
        } else {
          const blob = new Blob([result.data], { type: result.contentType });
          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },
    listaDEFs() {
      this.defs = [];
      ApiService.getAll("DEF", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.defs = result.data;
        }
      });
    },
    listaSaldosIniciais() {
      this.saldosIniciais = [];
      ApiService.obtemSaldosIniciais("FluxoCaixaSaldoInicial", (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.saldosIniciais = result.data;
        }
      });
    },
  },
  mounted() {
    this.listaDEFs();
    this.listaSaldosIniciais();
  },
};
</script>

<style scoped>
/* Barra de filtro */
.kbar .filter-row { align-items: end; }
.kbar .actions { text-align: right; display:flex; gap:8px; align-items:center; justify-content:flex-end; flex-wrap: wrap; }
.btn-wide { min-width: 140px; margin-right: 8px; }

/* KPIs — somente realizado */
.kpis {
  display:grid;
  grid-template-columns: repeat(3, minmax(200px, 1fr));
  gap: 10px;
  margin: 12px 0;
}
.kpi {
  border: 1px solid rgba(255,255,255,0.14);
  background: rgba(255,255,255,0.06);
  border-radius: 10px;
  padding: 10px;
  min-height: 76px;
}
.kpi.emph { border-color: #ffd639; background: rgba(255,214,57,0.08); }
.kpi .k { font-size: 12px; color:#A9B1C3; margin-bottom:4px; }
.kpi .v { font-weight: 700; font-size: 16px; }
.mono { font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace; }
.text-pos { color:#10B981; }
.text-neg { color:#F87171; }

/* ======== TABELA: SCROLL HORIZONTAL + LARGURAS FIXAS + GRADES ======== */
::v-deep .b-table-sticky-header {
  max-height: calc(100vh - 240px) !important;
  overflow-y: auto !important;
  overflow-x: auto !important;
}
.fc-table { overflow-x: auto !important; }
::v-deep .fc-table table {
  table-layout: fixed !important;
  border-collapse: collapse !important;
  min-width: max-content !important;
}

/* Grades: bordas em todas as células */
::v-deep .fc-table th,
::v-deep .fc-table td {
  white-space: nowrap !important;
  overflow: hidden !important;
  text-overflow: ellipsis !important;
  vertical-align: middle !important;
  padding: 7px 10px !important;
  border: 1px solid rgba(255, 255, 255, 0.12) !important;
}

/* Cabeçalho com borda mais destacada */
::v-deep .fc-table thead th {
  border-color: rgba(255, 255, 255, 0.25) !important;
  text-align: center !important;
}

/* Linha separadora mais forte entre header e body */
::v-deep .fc-table thead tr:last-child th {
  border-bottom: 2px solid rgba(255, 255, 255, 0.35) !important;
}

.fc-table .num { text-align: right; display: block; }

/* Célula do dia */
.day-cell .day { font-weight: 700; }
.day-cell .note { font-size: 12px; color: #A9B1C3; }

/* ===== Destaques de totais — TABELA (mantido) ===== */
::v-deep tr.table-TotalP,
::v-deep tr.table-TotalP > td,
::v-deep .table-TotalP td {
  background: rgba(239,68,68,0.18) !important;
  font-weight: 700 !important;
}
::v-deep tr.table-TotalR,
::v-deep tr.table-TotalR > td,
::v-deep .table-TotalR td {
  background: rgba(16,185,129,0.18) !important;
  font-weight: 700 !important;
}
::v-deep tr.table-TotalP > td:first-child,
::v-deep tr.table-TotalR > td:first-child {
  box-shadow: inset 4px 0 0 0 var(--bar, transparent);
}
.table-TotalP { --bar: #ef4444; }
.table-TotalR { --bar: #10b981; }

/* Separador semanal */
::v-deep tr.week-sep td { border-top: 3px solid rgba(255,255,255,0.25) !important; }

/* Ações */
.actions-cell { display:flex; gap:6px; }
.btn-icon { padding: 4px 8px; }

/* Campos de dinheiro nos modais */
.money-dark {
  color: white !important;
  background-color: black !important;
  border-width: 0px !important;
}
.wide-190 { width: 190px; }

/* Responsivo */
@media (max-width: 992px) { .kpis { grid-template-columns: 1fr; } }
</style>
