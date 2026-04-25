<template>
    <div class="relatorioAgenda-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>

        <h1 class="page-title">Agenda</h1>

        <!-- ====== Seleção do tipo de relatório ====== -->
        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col md="3">
                    <label class="mr-3">TIPO DO RELATÓRIO:</label>
                    <multiselect v-model="filtro_TipoRelatorio" :multiple="false" :options="tipoRelatorio"
                        label="descricao" select-label="Selecionar" placeholder="Selecione o TIPO">
                    </multiselect>
                </b-col>
            </b-row>
        </Widget>

        <!-- ====== Filtros ====== -->
        <Widget customHeader class="estiloWidget">
            <b-row>
                <b-col md="4">
                    <label class="mr-3">Obra:</label><br />
                    <multiselect v-model="filtro_ObraSelecionada" :multiple="true" :options="filtro_Obras"
                        select-label="Selecionar" placeholder="Selecione uma ou mais Obras" label="codigo"
                        track-by="codigo">
                    </multiselect>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Cliente:</label><br />
                    <multiselect v-model="filtro_ClienteSelecionado" :multiple="true" :options="filtro_Clientes"
                        select-label="Selecionar" placeholder="Selecione um ou mais Clientes" label="nomeFantasia"
                        track-by="nomeFantasia">
                    </multiselect>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">DEF:</label>
                    <multiselect v-model="filtro_DefsSelecionados" :multiple="true" :options="filtro_Def"
                        select-label="Selecionar" placeholder="Selecione um ou mais DEFs" :custom-label="descricaoDef"
                        label="codigo" track-by="codigo">
                    </multiselect>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="3">
                    <label class="mr-3">Fornecedor:</label>
                    <multiselect v-model="filtro_FornecedoresSelecionados" :multiple="true" :options="filtro_Fornecedor"
                        select-label="Selecionar" placeholder="Selecione um ou mais Fornecedores" label="nomeFantasia"
                        track-by="nomeFantasia">
                    </multiselect>
                </b-col>
                <b-col md="3">
                    <label class="mr-3">Número NF:</label>
                    <b-form-input v-model="filtro_NumeroNF" style="color: white"></b-form-input>
                </b-col>
                <b-col md="3">
                    <label class="mr-3">Data Inicial:</label><br />
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataInicial" format="dd/MM/yyyy"
                        :clearable='true' type="date">
                    </DatePickerMask>
                    <a @click="filtro_DataInicial = null">limpar</a>
                </b-col>
                <b-col md="3">
                    <label class="mr-3">Data Final:</label><br />
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFinal" format="dd/MM/yyyy"
                        type="date">
                    </DatePickerMask>
                    <a @click="filtro_DataFinal = null">limpar</a>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="4">
                    <label class="mr-3">Pedido Interno:</label>
                    <b-form-input v-model="filtro_PedidoInterno" style="color: white"></b-form-input>
                </b-col>
                <b-col md="4">
                    <label class="mr-3">Ordem Compra:</label>
                    <b-form-input v-model="filtro_OrdemCompra" style="color: white"></b-form-input>
                </b-col>
            </b-row>

            <br />

            <b-row>
                <b-col md="12" style="text-align: right;">

                    <button @click="limpaFiltros()" v-b-modal.cadastro type="button"
                        class="btn width-120 mb-3 mr-3 btn-danger">
                        Limpar Filtros
                    </button>
                    <button @click="obtemAgenda()" v-b-modal.cadastro type="button" ref="btnPesquisar"
                        class="btn width-120 mb-3 mr-3 btn-success">
                        Pesquisar
                    </button>

                    <button
                        type="button"
                        class="btn width-120 mb-3 mr-3 btn-info"
                        @click="abrirModalAcoesDownload()"
                    >
                        Download <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                    </button>
                </b-col>
            </b-row>
        </Widget>

        <!-- =================== MODAIS (mantidos) =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalNotasFiscais" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal"
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
                <div class="afm-summary-grid">
                  <div class="afm-summary-card">
                    <span class="afm-summary-card__label">Valor do Pedido</span>
                    <strong class="afm-summary-card__value">{{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalNotasFiscais_Pedido.valorTotal) : 'R$ 0,00') }}</strong>
                  </div>
                  <div class="afm-summary-card">
                    <span class="afm-summary-card__label">Total notas</span>
                    <strong class="afm-summary-card__value">{{(modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalNotasFiscais_Notas.reduce((total, item) => total + (item.aprovada != false ? (item.valor || 0) : 0), 0)) : 'R$ 0,00')}}</strong>
                  </div>
                  <div class="afm-summary-card">
                    <span class="afm-summary-card__label">Saldo</span>
                    <strong class="afm-summary-card__value">{{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalNotasFiscais_Pedido.saldo) : 'R$ 0,00') }}</strong>
                  </div>
                </div>
              </section>

              <section class="afm-section-card">
                <div class="afm-table-badge" style="margin-bottom: 10px;">Notas Fiscais · {{ modalNotasFiscais_Notas ? modalNotasFiscais_Notas.length : 0 }} item(ns)</div>

                <div v-if="modalNotasFiscais_Notas && modalNotasFiscais_Notas.length > 0">
                  <div class="table-card table-card--fluid table-card--scroll-x">
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
                            <button type="button" class="btn btn-success" @click="abrirModalAcoesNotaFiscal(row)">
                              Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                            </button>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <div v-else class="afm-empty-state">
                  <i class="fa fa-file-text-o"></i>
                  <div><strong>Nenhuma nota fiscal</strong><p>Este pedido ainda não possui notas fiscais.</p></div>
                </div>
              </section>

              <section class="afm-section-card">
                <div class="afm-table-badge" style="margin-bottom: 10px;">Cancelamentos de Saldo · {{ modalNotasFiscais_SaldosCancelados ? modalNotasFiscais_SaldosCancelados.length : 0 }} item(ns)</div>

                <div v-if="modalNotasFiscais_SaldosCancelados && modalNotasFiscais_SaldosCancelados.length > 0">
                  <div class="table-card table-card--fluid table-card--scroll-x">
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
                      </tbody>
                    </table>
                  </div>
                </div>
                <div v-else class="afm-empty-state">
                  <i class="fa fa-check-circle-o"></i>
                  <div><strong>Nenhum cancelamento</strong><p>Não há cancelamentos de saldo registrados.</p></div>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalNotasFiscais_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <b-modal :no-close-on-backdrop="true" id="modalAlterarDEF" class="modal-dialog modal-md afm-modal"
            body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
            v-model="modalAlterarDEF_Exibir" size="lg">

            <div class="afm-hero">
              <div>
                <span class="afm-hero__eyebrow">Alteração</span>
                <h3 class="afm-hero__title">Alterar Dados</h3>
                <p class="afm-hero__description">Informe o novo DEF e a nova data de pagamento.</p>
              </div>
            </div>

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div class="afm-field-group mb-3">
                  <label class="afm-label">Novo DEF</label>
                  <multiselect v-model="modalAlterarDEF_DefSelecionado" :multiple="false" :options="filtro_Def"
                    :custom-label="descricaoDef" select-label="Selecionar" placeholder="Selecione um DEF">
                  </multiselect>
                </div>

                <div class="afm-field-group">
                  <label class="afm-label">Nova data de pagamento</label>
                  <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                    v-model="modalAlterarDEF_DataSelecionada" format="dd/MM/yyyy" type="date">
                  </DatePickerMask>
                </div>
              </section>
            </b-container>

            <template #modal-footer>
              <div class="afm-footer">
                <div class="afm-footer__summary"></div>
                <div class="afm-footer__actions">
                  <b-button @click="modalAlterarDEF_Exibir = false" variant="dark" class="mb-0 mr-2">
                    Fechar
                  </b-button>
                  <b-button variant="success" class="mb-0" @click="alterarDef()">
                    <i class="fa fa-save mr-1"></i> Alterar Dados
                  </b-button>
                </div>
              </div>
            </template>
        </b-modal>

        <!-- ======================= MODAL AJUSTE MANUAL ======================= -->
        <b-modal :no-close-on-backdrop="true" id="modalAjusteManual" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalAjusteManual_Exibir" size="xl">
            <div class="afm-hero">
                <div>
                    <span class="afm-hero__eyebrow">Ajuste guiado</span>
                    <h3 class="afm-hero__title">Distribua o valor do pagamento com precisão</h3>
                    <p class="afm-hero__description">
                        Ajuste as datas e os valores manualmente até que a soma informada fique exatamente igual ao limite.
                    </p>
                </div>
                <div class="afm-hero__pill">
                    <span>{{ modalAjusteManual_DataValor.length }}</span>
                    <small>lançamento(s)</small>
                </div>
            </div>

            <b-container fluid class="afm-sections">
                <!-- Seção: Visão geral -->
                <section class="afm-section-card">

                    <div class="afm-summary-grid">
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Limite</span>
                            <strong class="afm-summary-card__value mono">{{ formatBRL(modalAjusteManual_ValorExato) }}</strong>
                        </div>
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Soma informada</span>
                            <strong class="afm-summary-card__value mono" :class="ajusteManualOk ? 'text-ok' : 'text-warn'">{{ formatBRL(ajusteManualTotalInformado) }}</strong>
                        </div>
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Restante</span>
                            <strong class="afm-summary-card__value mono" :class="ajusteManualRestante === 0 ? 'text-ok' : (ajusteManualRestante > 0 ? 'text-info' : 'text-warn')">{{ formatBRL(ajusteManualRestante) }}</strong>
                        </div>
                    </div>

                    <div class="afm-progress">
                        <div class="afm-progress__bar" :style="{ width: ajusteManualPercent + '%' }"></div>
                    </div>

                    <div v-if="!ajusteManualOk" class="afm-alert">
                        O ajuste manual só pode ser salvo quando a soma dos valores for exatamente igual ao limite.
                    </div>
                </section>

                <!-- Seção: Novo lançamento -->
                <section class="afm-section-card">

                    <b-row>
                        <b-col md="4">
                            <div class="afm-field-group">
                                <label class="afm-label">Data de faturamento</label>
                                <div class="afm-input-wrap">
                                    <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                        v-model="modalAjusteManual_Data" format="dd/MM/yyyy" type="date"
                                        class="aj-datepicker afm-datepicker" />
                                </div>
                            </div>
                        </b-col>
                        <b-col md="4">
                            <div class="afm-field-group">
                                <label class="afm-label">Valor</label>
                                <Money v-model="modalAjusteManual_Valor" v-bind="money" class="aj-money afm-money" />
                            </div>
                        </b-col>
                        <b-col md="4">
                            <div class="afm-field-group afm-field-group--action">
                                <label class="afm-label">Ação</label>
                                <button type="button" @click="adicionaNovaDataValorAjusteManual()"
                                    class="btn btn-success afm-add-btn"
                                    :disabled="!modalAjusteManual_Data || !modalAjusteManual_Valor || modalAjusteManual_Valor <= 0"
                                    title="Adicionar lançamento">
                                    <i class="fa fa-plus mr-2"></i>Adicionar lançamento
                                </button>
                            </div>
                        </b-col>
                    </b-row>
                </section>

                <!-- Seção: Lançamentos informados -->
                <section class="afm-section-card">

                    <div v-if="modalAjusteManual_DataValor.length > 0" class="table-card table-card--fluid table-card--scroll-x">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro" style="width: 220px;">Data</th>
                                    <th class="estilo-cabecalho texto-centro" style="width: 220px;">Valor</th>
                                    <th class="estilo-cabecalho texto-centro" style="width: 110px;">Ação</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="dataValor in modalAjusteManual_DataValor" :key="dataValor.id">
                                    <td class="estilo-celula texto-centro">
                                        <div class="input-table input-table--date">
                                            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                                v-model="dataValor.data" format="dd/MM/yyyy" type="date" />
                                        </div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <div class="input-table input-table--money">
                                            <Money v-model="dataValor.valor" v-bind="money" />
                                        </div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <button type="button" @click="removerDataAjusteManual(dataValor.id)"
                                            class="btn btn-danger afm-remove-btn" title="Remover lançamento">
                                            <i class="fa fa-trash"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div v-else class="afm-empty-state">
                        <i class="fa fa-inbox"></i>
                        <div>
                            <strong>Nenhum lançamento adicionado</strong>
                            <p>Informe uma data e um valor e clique em <b>Adicionar lançamento</b>.</p>
                        </div>
                    </div>
                </section>
            </b-container>

            <template #modal-footer>
                <div class="afm-footer">
                    <div class="afm-footer__summary">
                        <span>Status do ajuste:</span>
                        <strong>{{ ajusteManualOk ? 'Conferido e pronto para salvar' : 'Revise a soma para continuar' }}</strong>
                    </div>
                    <div class="afm-footer__actions">
                        <b-button @click="modalAjusteManual_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                        <button type="button" @click="salvarAjusteManual()" class="btn btn-success afm-save-btn"
                            :disabled="!ajusteManualOk || modalAjusteManual_DataValor.length === 0"
                            :title="!ajusteManualOk ? 'A soma dos lançamentos deve ser igual ao limite' : 'Salvar Ajuste Manual'">
                            Salvar Ajuste Manual
                        </button>
                    </div>
                </div>
            </template>
        </b-modal>

        <!-- =================== MODAL AJUSTE MANUAL A FATURAR =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalAjusteManualAFaturar" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalAjusteManualAFaturar_Exibir" size="xl">
            <div class="afm-hero">
                <div>
                    <span class="afm-hero__eyebrow">Ajuste guiado</span>
                    <h3 class="afm-hero__title">Distribua o saldo a faturar com precisão</h3>
                    <p class="afm-hero__description">
                        Ajuste as datas e os valores manualmente até que a soma informada fique exatamente igual ao limite bruto.
                    </p>
                </div>
                <div class="afm-hero__pill">
                    <span>{{ modalAjusteManualAFaturar_Lista.length }}</span>
                    <small>lançamento(s)</small>
                </div>
            </div>


            <b-container fluid class="afm-sections">
                <section class="afm-section-card">

                    <div class="afm-summary-grid">
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Limite bruto</span>
                            <strong class="afm-summary-card__value mono">{{ formatBRL(modalAjusteManualAFaturar_LimiteBruto) }}</strong>
                        </div>
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Soma informada</span>
                            <strong class="afm-summary-card__value mono" :class="modalAjusteManualAFaturar_Saldo === 0 ? 'text-ok' : 'text-warn'">{{ formatBRL(modalAjusteManualAFaturar_SomaValores) }}</strong>
                        </div>
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Saldo</span>
                            <strong class="afm-summary-card__value mono" :class="modalAjusteManualAFaturar_Saldo === 0 ? 'text-ok' : (modalAjusteManualAFaturar_Saldo > 0 ? 'text-info' : 'text-warn')">{{ formatBRL(modalAjusteManualAFaturar_Saldo) }}</strong>
                        </div>
                    </div>

                    <div class="afm-progress">
                        <div class="afm-progress__bar" :style="{ width: modalAjusteManualAFaturar_Percentual + '%' }"></div>
                    </div>

                    <div v-if="!modalAjusteManualAFaturar_Ok" class="afm-alert">
                        O ajuste manual só pode ser salvo quando a soma dos valores for exatamente igual ao limite bruto.
                    </div>
                </section>

                <section class="afm-section-card">

                    <b-row>
                        <b-col md="4">
                            <div class="afm-field-group">
                                <label class="afm-label">Data de pagamento</label>
                                <div class="afm-input-wrap">
                                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalAjusteManualAFaturar_Data"
                                        format="dd/MM/yyyy" type="date" class="aj-datepicker afm-datepicker" />
                                </div>
                            </div>
                        </b-col>
                        <b-col md="4">
                            <div class="afm-field-group">
                                <label class="afm-label">Valor bruto</label>
                                <Money v-model="modalAjusteManualAFaturar_Valor" v-bind="money" class="aj-money afm-money" />
                            </div>
                        </b-col>
                        <b-col md="4">
                            <div class="afm-field-group afm-field-group--action">
                                <label class="afm-label">Ação</label>
                                <button type="button" @click="adicionarItemAjusteManualAFaturar()"
                                    class="btn btn-success afm-add-btn"
                                    :disabled="!modalAjusteManualAFaturar_Data || !modalAjusteManualAFaturar_Valor || modalAjusteManualAFaturar_Valor <= 0"
                                    title="Adicionar lançamento">
                                    <i class="fa fa-plus mr-2"></i>Adicionar lançamento
                                </button>
                            </div>
                        </b-col>
                    </b-row>
                </section>

                <section class="afm-section-card">

                    <div v-if="modalAjusteManualAFaturar_Lista.length > 0" class="table-card table-card--fluid table-card--scroll-x">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro" style="width: 220px;">Data</th>
                                    <th class="estilo-cabecalho texto-centro" style="width: 220px;">Valor bruto</th>
                                    <th class="estilo-cabecalho texto-centro" style="width: 110px;">Ação</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in modalAjusteManualAFaturar_Lista" :key="item._uid">
                                    <td class="estilo-celula texto-centro">
                                        <div class="input-table input-table--date">
                                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="item.dataPagamento"
                                                format="dd/MM/yyyy" type="date" />
                                        </div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <div class="input-table input-table--money">
                                            <Money v-model="item.valor" v-bind="money" />
                                        </div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <button type="button" @click="removerItemAjusteManualAFaturar(item._uid)"
                                            class="btn btn-outline-danger afm-remove-btn" title="Remover lançamento">
                                            <i class="fa fa-trash"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div v-else class="afm-empty-state">
                        <i class="fa fa-calendar-plus-o"></i>
                        <div>
                            <strong>Nenhum lançamento informado</strong>
                            <p>Preencha a data e o valor acima para começar a distribuir o saldo a faturar.</p>
                        </div>
                    </div>
                </section>
            </b-container>

            <template #modal-footer>
                <div class="afm-footer">
                    <div class="afm-footer__summary">
                        <span>Status do ajuste:</span>
                        <strong>{{ modalAjusteManualAFaturar_Ok ? 'Conferido e pronto para salvar' : 'Revise a soma para continuar' }}</strong>
                    </div>
                    <div class="afm-footer__actions">
                        <b-button @click="modalAjusteManualAFaturar_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                        <button type="button" @click="salvarAjusteManualAFaturar()" class="btn btn-success afm-save-btn"
                            :disabled="!modalAjusteManualAFaturar_Ok || modalAjusteManualAFaturar_Lista.length === 0"
                            :title="!modalAjusteManualAFaturar_Ok ? 'A soma dos valores deve ser igual ao limite bruto' : 'Salvar Ajuste Manual'">
                            Salvar Ajuste Manual
                        </button>
                    </div>
                </div>
            </template>
        </b-modal>
        <!-- ===================== MODAL INFORMAR RECEBIMENTO ===================== -->
        <b-modal :no-close-on-backdrop="true" id="modalDataFaturamento" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalDataFaturamento_Exibir" size="lg">

            <div class="afm-hero">
                <div>
                    <span class="afm-hero__eyebrow">Faturamento</span>
                    <h3 class="afm-hero__title">Informar recebimento</h3>
                    <p class="afm-hero__description">Confirme os dados do recebimento desta nota fiscal.</p>
                </div>
                <div class="afm-hero__pill">
                    <span>NF</span>
                    <small>{{ modalDataFaturamento_Faturamento == null ? '—' : modalDataFaturamento_Faturamento.numeroNF }}</small>
                </div>
            </div>

            <b-container fluid class="afm-sections">
                <section class="afm-section-card">
                    <div class="afm-summary-grid fat-summary-2col">
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Valor Bruto</span>
                            <strong class="afm-summary-card__value mono">
                                {{ modalDataFaturamento_Faturamento == null ? '—' :
                                    new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorBruto) }}
                            </strong>
                        </div>
                        <div class="afm-summary-card">
                            <span class="afm-summary-card__label">Valor Líquido</span>
                            <strong class="afm-summary-card__value mono">
                                {{ modalDataFaturamento_Faturamento == null ? '—' :
                                    new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido) }}
                            </strong>
                        </div>
                    </div>
                </section>

                <section class="afm-section-card">
                    <div class="irf-toggle-wrap">
                        <span class="irf-toggle-label" :class="{ 'irf-toggle-label--active': !modalDataFaturamento_TipoTotal }">Parcial</span>
                        <div class="irf-toggle" :class="{ 'irf-toggle--on': modalDataFaturamento_TipoTotal }"
                            @click="modalDataFaturamento_TipoTotal = !modalDataFaturamento_TipoTotal">
                            <div class="irf-toggle__thumb"></div>
                        </div>
                        <span class="irf-toggle-label" :class="{ 'irf-toggle-label--active': modalDataFaturamento_TipoTotal }">Total</span>
                    </div>
                </section>

                <section class="afm-section-card">

                    <b-row class="fat-row">
                        <b-col md="6">
                            <div class="afm-field-group">
                                <label class="afm-label">Data Recebimento</label>
                                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                                    v-model="modalDataFaturamento_DataFaturamento" format="dd/MM/yyyy" type="date"
                                    class="afm-datepicker">
                                </DatePickerMask>
                            </div>
                        </b-col>
                    </b-row>

                    <b-row v-if="!modalDataFaturamento_TipoTotal">
                        <b-col md="6">
                            <div class="afm-field-group">
                                <label class="afm-label">Valor Recebido</label>
                                <Money v-model="modalDataFaturamento_ValorRecebido" v-bind="money"
                                    class="afm-money fat-money"></Money>
                            </div>
                        </b-col>
                        <b-col md="6">
                            <div class="afm-field-group">
                                <label class="afm-label">Data Próximo Recebimento</label>
                                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                                    v-model="modalDataFaturamento_DataProximoRecebimento" format="dd/MM/yyyy" type="date"
                                    class="afm-datepicker">
                                </DatePickerMask>
                            </div>
                        </b-col>
                    </b-row>

                    <div v-if="modalDataFaturamento_TipoTotal && modalDataFaturamento_Faturamento != null" class="irf-total-confirm">
                        <i class="fa fa-check-circle irf-total-confirm__icon"></i>
                        <div>
                            <strong>Recebimento total</strong>
                            <p>O valor líquido de <b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido) }}</b> será automaticamente registrado como recebido.</p>
                        </div>
                    </div>

                    <template v-if="!modalDataFaturamento_TipoTotal && modalDataFaturamento_Faturamento != null">
                        <div v-if="modalDataFaturamento_ValorRecebido <= 0" class="irf-alert irf-alert--warn">
                            <i class="fa fa-exclamation-circle"></i>
                            <span>O valor recebido deve ser maior que zero.</span>
                        </div>
                        <div v-else-if="modalDataFaturamento_ValorRecebido > modalDataFaturamento_Faturamento.valorLiquido" class="irf-alert irf-alert--error">
                            <i class="fa fa-times-circle"></i>
                            <span>O valor informado (<b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_ValorRecebido) }}</b>) excede o valor líquido de <b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido) }}</b>.</span>
                        </div>
                        <div v-else-if="modalDataFaturamento_ValorRecebido > 0" class="irf-alert irf-alert--info">
                            <i class="fa fa-info-circle"></i>
                            <span>Saldo restante após este recebimento: <b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido - modalDataFaturamento_ValorRecebido) }}</b>.</span>
                        </div>
                    </template>
                </section>
            </b-container>

            <template #modal-footer>
                <div class="afm-footer">
                    <div class="afm-footer__summary">
                        <span>Modo:</span>
                        <strong>{{ modalDataFaturamento_TipoTotal ? 'Recebimento total' : 'Recebimento parcial' }}</strong>
                    </div>
                    <div class="afm-footer__actions">
                        <b-button @click="modalDataFaturamento_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                        <button type="button" class="btn btn-success afm-save-btn" v-on:click="informarRecebimento()">
                            <i class="fa fa-check mr-2"></i>Informar recebimento
                        </button>
                    </div>
                </div>
            </template>
        </b-modal>

        <!-- ===================== MODAL ARQUIVOS ===================== -->
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

            <input type="file" name="fileModal" id="fileInputModal" style="display: none" class="hidden-input"
              @change="onChangeModal" ref="fileModal" />

            <b-container fluid class="afm-sections">
              <section class="afm-section-card">
                <div v-if="modalArquivos_Arquivos && modalArquivos_Arquivos.length > 0">
                  <div class="table-card table-card--fluid table-card--scroll-x">
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
                            <button type="button" @click="downloadFile(row.id, row.nome)"
                              class="btn btn-warning afm-action-btn mr-1">
                              <i class="fa fa-download" title="Download"></i>
                            </button>
                            <button type="button" @click="excluirArquivo(row.id)" class="btn btn-danger afm-action-btn"
                              v-if="usuarioLogado == null ? false : (usuarioLogado.id == row.usuarioCadastro.id ? true : false)">
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
                  <label type="button" class="btn btn-success afm-add-btn" for="fileInputModal" style="width: auto; min-width: 200px; max-width: 320px;">
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
        <!-- ===================== /MODAIS ===================== -->

        <!-- ======================= AGENDA PAGAMENTO / FLUXO ======================= -->
        <Widget v-if="filtro_TipoRelatorio && (filtro_TipoRelatorio.id === 1 || filtro_TipoRelatorio.id === 3)"
            customHeader class="estiloWidget">

            <b-row>
                <b-col>
                    <h2>{{ filtro_TipoRelatorio.id === 1 ? 'Agenda Pagamento' : 'Fluxo Caixa' }}</h2>
                </b-col>
            </b-row>

            <b-row>
                <b-col lg="12">
                    <div class="table-card table-card--scroll-x">
                        <table class="estilo-tabela tabela-identidade">
                            <thead>
                                <tr>
                                    <th class="estilo-cabecalho texto-centro">
                                        <input type="checkbox" v-model="selecionarTodos" @change="checkboxTodos" />
                                    </th>
                                    <th class="estilo-cabecalho texto-centro">OBRA</th>
                                    <th class="estilo-cabecalho texto-centro">DEF</th>
                                    <th class="estilo-cabecalho texto-centro">ORDEM COMPRA</th>
                                    <th class="estilo-cabecalho texto-centro">PEDIDO INTERNO</th>
                                    <th class="estilo-cabecalho texto-centro">Nº NOTA FISCAL</th>
                                    <th class="estilo-cabecalho texto-centro">DATA LANÇAMENTO</th>
                                    <th class="estilo-cabecalho texto-centro">DATA PAGAMENTO</th>
                                    <th class="estilo-cabecalho texto-centro">VALOR</th>
                                    <th class="estilo-cabecalho">PAGAR PARA / RECEBER DE</th>
                                    <th class="estilo-cabecalho texto-centro">PAGO / RECEBIDO</th>
                                    <th class="estilo-cabecalho texto-centro">STATUS</th>
                                    <th class="estilo-cabecalho texto-centro"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(row, index) in agendas" :key="'ag-' + (row.id || index)" :class="rowClass(row, 'row')">
                                    <td class="estilo-celula texto-centro">
                                        <div v-if="row.pagamentoEfetuado != true && row.cancelado === false">
                                            <input type="checkbox" v-model="row.selecionado" :checked="false" @click="" />
                                        </div>
                                    </td>
                                    <td class="estilo-celula texto-centro">{{ row.codigoObra }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <div v-if="row.def && row.def.financeiroPodeSerAlterado == true && row.pagamentoEfetuado != true && row.cancelado === false">
                                            <multiselect v-model="row.def" :multiple="false" :options="filtro_Def"
                                                :custom-label="descricaoDef" select-label="Selecionar" placeholder="DEF"
                                                @open="memorizarDefAnterior(row.def)" @input="verificarDef(row)">
                                            </multiselect>
                                        </div>
                                        <div v-else>{{ row.def ? `${row.def.codigo} - ${row.def.descricao}` : '' }}</div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <label style="cursor: pointer;" @click="abreModalAjusteETOPedidoCompra(row.idPedidoCompra)">
                                            {{ row.codigoPedidoCompra }}
                                        </label>
                                    </td>
                                    <td class="estilo-celula texto-centro">{{ row.codigoPedidoInterno }}</td>
                                    <td class="estilo-celula texto-centro">{{ row.numeroNF }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.dataLancamento == null || row.dataLancamento == '2500-01-01T00:00:00') ? "" : formataDataSemHora(row.dataLancamento) }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <div v-if="row.pagamentoEfetuado != true && row.cancelado === false">
                                            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                                                v-model="row.dataPagamento" format="dd/MM/yyyy" type="date"
                                                @change="() => alterarDataIndividual(row.id, row.dataPagamento)"
                                                @keydown.native.enter="() => alterarDataIndividual(row.id, row.dataPagamento)">
                                            </DatePickerMask>
                                        </div>
                                        <div v-else>{{ formataDataSemHora(row.dataPagamento) }}</div>
                                    </td>
                                    <td class="estilo-celula texto-centro">
                                        <div v-if="row.pagamentoEfetuado != true && row.cancelado === false">
                                            <Money v-model="row.valor" v-bind="money"
                                                @keydown.native.enter="alterarValorIndividual(row.id, row.valor)" >
                                            </Money>
                                        </div>
                                        <div v-else>{{ Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</div>
                                    </td>
                                    <td class="estilo-celula">{{ (row.nomeFantasiaFornecedor != '' ? row.nomeFantasiaFornecedor : row.nomeUsuarioBeneficiario != '' ? row.nomeUsuarioBeneficiario : row.nomeCliente) }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.pagamentoEfetuado == true ? 'Sim - ' + formataData(row.dataPagamentoEfetuado) : 'Não') }}</td>
                                    <td class="estilo-celula texto-centro">{{ (row.cancelado == true ? 'CANCELADO' : 'NORMAL') }}</td>
                                    <td class="estilo-celula texto-centro">
                                        <button
                                            type="button"
                                            class="btn btn-success"
                                            v-if="row.cancelado === false"
                                            @click="abrirModalAcoesAgendaPagamento(row)"
                                        >
                                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr v-if="!agendas || agendas.length === 0">
                                    <td class="estilo-celula texto-centro" colspan="13">Não existe nenhum dado</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <button
                        type="button"
                        class="btn btn-info mr-3"
                        v-if="agendas.filter(item => item.selecionado).length > 0"
                        @click="abrirModalAcoesItensSelecionados()"
                    >
                        Itens selecionados <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                    </button>

                </b-col>
            </b-row>

            <br />

            <b-row style="text-align: right;">
                <b-col md="12">
                    <h3>TOTAL FILTRO: {{Intl.NumberFormat("pt-BR", {
                        style: "currency",
                        currency: "BRL"
                    }).format(this.agendas.filter(item => item.cancelado === false).reduce((soma, item) => soma +
                        item.valor, 0))}}</h3>
                </b-col>
                <b-col md="12">
                    <h3>TOTAL PAGAMENTOS SELECIONADOS: {{Intl.NumberFormat("pt-BR", {
                        style: "currency",
                        currency: "BRL"
                    }).format(this.agendas.filter(item => item.selecionado && item.def.tipo === 'P').reduce((soma,
                        item) =>
                        soma +
                        item.valor, 0))}}</h3>
                </b-col>
                <b-col md="12">
                    <h3>TOTAL RECEBIMENTOS SELECIONADOS: {{Intl.NumberFormat("pt-BR", {
                        style: "currency",
                        currency: "BRL"
                    }).format(this.agendas.filter(item => item.selecionado && item.def.tipo === 'R').reduce((soma,
                        item) =>
                        soma +
                        item.valor, 0))}}</h3>
                </b-col>
            </b-row>
        </Widget>

        <!-- ======================= AGENDA FATURAMENTO ======================= -->
        <div v-else-if="filtro_TipoRelatorio && filtro_TipoRelatorio.id === 2">

            <Widget customHeader class="estiloWidget">
                <b-row>
                    <b-col>
                        <h2>A Faturar</h2>
                    </b-col>
                </b-row>

                <b-row>
                    <b-col lg="12">
                        <div class="table-card table-card--scroll-x">
                            <table class="estilo-tabela tabela-identidade">
                                <thead>
                                    <tr>
                                        <th class="estilo-cabecalho texto-centro">Obra</th>
                                        <th class="estilo-cabecalho">Descrição</th>
                                        <th class="estilo-cabecalho texto-centro">CNPJ</th>
                                        <th class="estilo-cabecalho">Cliente</th>
                                        <th class="estilo-cabecalho texto-centro">Bruto (R$)</th>
                                        <th class="estilo-cabecalho texto-centro">INSS</th>
                                        <th class="estilo-cabecalho texto-centro">ISS</th>
                                        <th class="estilo-cabecalho texto-centro">IR</th>
                                        <th class="estilo-cabecalho texto-centro">ART 30</th>
                                        <th class="estilo-cabecalho texto-centro">Liquido (R$)</th>
                                        <th class="estilo-cabecalho texto-centro">Data Faturamento</th>
                                        <th class="estilo-cabecalho texto-centro">Data Receb. Prev.</th>
                                        <th class="estilo-cabecalho texto-centro">Ações</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(row, index) in medicoes" :key="'med-' + (row.idObra || index)">
                                        <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                                        <td class="estilo-celula">{{ row.descricao }}</td>
                                        <td class="estilo-celula texto-centro">{{ row.cnpj }}</td>
                                        <td class="estilo-celula">{{ row.razaoSocial }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.inss) }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.iss) }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.ir) }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.arT30) }}</td>
                                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorLiquido) }}</td>
                                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataLancamento) }}</td>
                                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPagamento) }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <button
                                                type="button"
                                                class="btn btn-success"
                                                @click="abrirModalAcoesAFaturar(row)"
                                            >
                                                Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr v-if="!medicoes || medicoes.length === 0">
                                        <td class="estilo-celula texto-centro" colspan="13">Nenhum registro encontrado</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <!-- ✅ TOTALIZADOR: logo abaixo da coluna de valor (visualmente perto da tabela) -->
                        <div class="af-total-wrap">
                            <div class="af-total-box">
                                <span class="af-total-label">BRUTO</span>
                                <span class="af-total-value">{{ formatCurrency(totalAFaturar) }}</span>
                            </div>
                        </div>

                        <div class="af-total-wrap">
                            <div class="af-total-box">
                                <span class="af-total-label">LIQUIDO</span>
                                <span class="af-total-value">{{ formatCurrency(totalAFaturarLiquido) }}</span>
                            </div>
                        </div>

                    </b-col>
                </b-row>
            </Widget>

            <Widget customHeader class="estiloWidget">
                <b-row>
                    <b-col>
                        <h2>Faturado</h2>
                    </b-col>
                </b-row>

                <b-row>
                    <b-col lg="12">
                        <div class="table-card table-card--scroll-x">
                            <table class="estilo-tabela tabela-identidade">
                                <thead>
                                    <tr>
                                        <th class="estilo-cabecalho texto-centro">OBRA</th>
                                        <th class="estilo-cabecalho">CLIENTE</th>
                                        <th class="estilo-cabecalho texto-centro">NF</th>
                                        <th class="estilo-cabecalho texto-centro">VAL. BRUTO</th>
                                        <th class="estilo-cabecalho texto-centro">INSS</th>
                                        <th class="estilo-cabecalho texto-centro">ISS</th>
                                        <th class="estilo-cabecalho texto-centro">IR</th>
                                        <th class="estilo-cabecalho texto-centro">ART. 30</th>
                                        <th class="estilo-cabecalho texto-centro">DESCONTO</th>
                                        <th class="estilo-cabecalho texto-centro">MATERIAL</th>
                                        <th class="estilo-cabecalho texto-centro">SINAL</th>
                                        <th class="estilo-cabecalho texto-centro">NÃO COMISS.</th>
                                        <th class="estilo-cabecalho texto-centro">VAL. LIQ.</th>
                                        <th class="estilo-cabecalho texto-centro">DT. RECEB. PREV.</th>
                                        <th class="estilo-cabecalho texto-centro">DT. FAT.</th>
                                        <th class="estilo-cabecalho texto-centro">DT. RECEB.</th>
                                        <th class="estilo-cabecalho texto-centro">STATUS</th>
                                        <th class="estilo-cabecalho texto-centro">AÇÕES</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(row, index) in faturamentos" :key="'fat-' + (row.id || index)">
                                        <td class="estilo-celula texto-centro">{{ row.obra ? row.obra.codigo : '' }}</td>
                                        <td class="estilo-celula">{{ (row.obra && row.obra.cliente) ? row.obra.cliente.nomeFantasia : '' }}</td>
                                        <td class="estilo-celula texto-centro">{{ row.numeroNF }}</td>
                                        <td class="estilo-celula texto-centro">{{ formatCurrency(row.valorBruto) }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorINSS) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorINSS" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorINSS, 'INSS')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorISS) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorISS" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorISS, 'ISS')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorIR) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorIR" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorIR, 'IR')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorArt30) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorArt30" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorArt30, 'Art30')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorDesconto) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorDesconto" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorDesconto, 'Desconto')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">{{ formatBRL(row.valorMaterial) }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorSinal) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorSinal" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorSinal, 'Sinal')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formatCurrency(row.valorNaoComissionado) }}</div>
                                            <div v-else>
                                                <Money v-model="row.valorNaoComissionado" v-bind="money" class="fat-edit-money"
                                                    @keydown.native.enter.prevent="alteraValorFaturamento(row.id, row.valorNaoComissionado, 'NaoComissionado')" />
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">{{ formatBRL(row.valorLiquido) }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <div v-if="row.idStatusFaturamento != 2">{{ formataDataSemHora(row.dataRecebimentoPrevisto) }}</div>
                                            <div v-else>
                                                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                                                    v-model="row.dataRecebimentoPrevisto" format="dd/MM/yyyy" type="date"
                                                    class="fat-edit-date"
                                                    @change="() => alterarDataFaturamento(row.id, row.dataRecebimentoPrevisto, 2)"
                                                    @keydown.native.enter.prevent="() => alterarDataFaturamento(row.id, row.dataRecebimentoPrevisto, 2)">
                                                </DatePickerMask>
                                            </div>
                                        </td>
                                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataFaturamento) }}</td>
                                        <td class="estilo-celula texto-centro">{{ row.dataRecebimentoRealizado != null ? formataDataSemHora(row.dataRecebimentoRealizado) : '' }}</td>
                                        <td class="estilo-celula texto-centro">{{ row.status ? row.status.descricao : '' }}</td>
                                        <td class="estilo-celula texto-centro">
                                            <button
                                                type="button"
                                                class="btn btn-success"
                                                @click="abrirModalAcoesFaturamento(row)"
                                            >
                                                Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr v-if="!faturamentos || faturamentos.length === 0">
                                        <td class="estilo-celula texto-centro" colspan="18">Não existe nenhum dado</td>
                                    </tr>
                                    <tr v-if="faturamentos && faturamentos.length > 0" class="linha-totais">
                                        <td class="estilo-celula total-cell" colspan="3" style="text-align: center; font-weight: bold;">Totais:</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorBruto')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorINSS')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorISS')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorIR')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorArt30')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorDesconto')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorMaterial')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorSinal')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorNaoComissionado')) }}</td>
                                        <td class="estilo-celula total-cell texto-centro">{{ formatCurrency(total('valorLiquido')) }}</td>
                                        <td class="estilo-celula total-cell"></td>
                                        <td class="estilo-celula total-cell"></td>
                                        <td class="estilo-celula total-cell"></td>
                                        <td class="estilo-celula total-cell"></td>
                                        <td class="estilo-celula total-cell"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </b-col>
                </b-row>
            </Widget>
        </div>

        <!-- Modal de ações das tabelas -->
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
    name: 'RelatorioAgenda',
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
                decimal: ",",
                thousands: ".",
                prefix: "R$ ",
                precision: 2,
                masked: false,
            },

            // ... (todo seu data original continua)
            // Removi apenas: salvandoFaturamentoId e ultimaEdicaoHash

            modalCarimboNF_IR: 0.0,
            modalCarimboNF_IRSelecionado: 1.0,
            modalCarimboNF_IRBaseSelecionada: true,
            impostoIR: [{ id: 1, descricao: '1.0%', valor: 1.0 }, { id: 2, descricao: '1.5%', valor: 1.5 }],
            modalCarimboNF_IRDataPagamento: null,

            modalCarimboNF_Art30: 0.0,
            modalCarimboNF_Art30Selecionado: 4.65,
            modalCarimboNF_Art30BaseSelecionada: true,
            impostoArt30: [{ id: 1, descricao: '4,65%', valor: 4.65 }],
            modalCarimboNF_Art30DataPagamento: null,

            modalCarimboNF_INSS: 0.0,
            modalCarimboNF_INSSSelecionado: 3.5,
            modalCarimboNF_INSSBaseSelecionada: true,
            impostoINSS: [{ id: 1, descricao: '3,5%', valor: 3.5 }, { id: 2, descricao: '11%', valor: 11 }],
            modalCarimboNF_INSSDataPagamento: null,

            modalCarimboNF_ISS: 0.0,
            modalCarimboNF_ISSBaseSelecionada: true,
            impostoISS: 1.0,
            modalCarimboNF_ISSDataPagamento: null,

            basePagamento: [{ id: 1, descricao: 'Bruto' }, { id: 2, descricao: 'Base Cálculo' }],
            basePagamentoArt30: [{ id: 1, descricao: 'Bruto' }],
            dataPagamentoInicial: new Date(Date.now()),
            dataPagamentoFinal: null,

            modalCarimboNF_Exibir: false,
            modalCarimboNF_Titulo: 'Carimbo de Nota Fiscal',
            modalCarimboNF_ValorBruto: 0.0,
            modalCarimboNF_MaterialAbatido: 0.0,
            modalCarimboNF_BaseCalculo: 0.0,
            modalCarimboNF_ValorLiquido: 0.0,
            modalCarimboNF_Id: 0,
            modalCarimboNF_NotaFiscal: null,

            modalDataFaturamento_Exibir: false,
            modalDataFaturamento_DataFaturamento: null,
            modalDataFaturamento_Faturamento: null,
            modalDataFaturamento_ValorRecebido: null,
            modalDataFaturamento_DataProximoRecebimento: null,
            modalDataFaturamento_TipoTotal: true,

            modalArquivos_Exibir: false,
            modalArquivos_Titulo: 'Arquivos',
            modalArquivos_Arquivos: [],
            modalArquivos_Controller: 'Faturamento',
            modalArquivos_IdFaturamento: 0,

            modalAjusteManual_Exibir: false,
            modalAjusteManual_Titulo: 'Ajuste manual',
            modalAjusteManual_Data: null,
            modalAjusteManual_Valor: 0.0,
            modalAjusteManual_DataValor: [],
            modalAjusteManual_Id: 0,
            modalAjusteManual_ValorExato: 0.0,

            modalAjusteManualAFaturar_Exibir: false,
            modalAjusteManualAFaturar_Titulo: 'Ajuste manual de obras com saldo a faturar',
            modalAjusteManualAFaturar_ObjetoOriginal: null,
            modalAjusteManualAFaturar_Data: null,
            modalAjusteManualAFaturar_Valor: 0.0,
            modalAjusteManualAFaturar_LimiteBruto: 0.0,
            modalAjusteManualAFaturar_Lista: [],
            modalAjusteManualAFaturar_Sequencia: 0,

            tipoRelatorio: [
                { id: 1, descricao: 'Agenda Pagamento' },
                { id: 2, descricao: 'Agenda Faturamento' },
                { id: 3, descricao: 'Fluxo Caixa' }
            ],
            dataMinima: (new Date()).setDate(new Date().getDate() - 1),

            open: false,
            isLoading: false,

            usuarioLogado: null,

            faturamentos: [],
            medicoes: [],

            modalAlterarDEF_Exibir: false,
            modalAlterarDEF_DefSelecionado: null,
            modalAlterarDEF_DataSelecionada: null,

            modalNotasFiscais_Titulo: 'Notas Fiscais',
            modalNotasFiscais_Exibir: false,
            modalNotasFiscais_Notas: [],
            modalNotasFiscais_SaldosCancelados: [],
            modalNotasFiscais_Pedido: null,

            selecionarTodos: false,
            defAnterior: null,

            filtro_TipoRelatorio: { id: 1, descricao: 'Agenda Pagamento' },
            filtro_ObraSelecionada: null,
            filtro_ClienteSelecionado: null,
            filtro_DefsSelecionados: [],
            filtro_FornecedoresSelecionados: [],
            filtro_DataInicial: null,
            filtro_DataFinal: null,
            filtro_NumeroNF: '',
            filtro_PedidoInterno: '',
            filtro_OrdemCompra: '',

            filtro_Obras: [],
            filtro_Clientes: [],
            filtro_Def: [],
            filtro_Fornecedor: [],

            agendas: [],

            fields: [
                { key: 'checkbox', label: '' },
                { key: 'codigoObra', label: 'OBRA' },
                { key: 'codigoDef', label: 'DEF' },
                { key: 'codigoPedidoCompra', label: 'ORDEM COMPRA' },
                { key: 'codigoPedidoInterno', label: 'PEDIDO INTERNO' },
                { key: 'numeroNF', label: 'Nº NOTA FISCAL' },
                { key: 'dataLancamento', label: 'DATA LANÇAMENTO' },
                { key: 'dataPagamento', label: 'DATA PAGAMENTO' },
                { key: 'valor', label: 'VALOR' },
                { key: 'pagarReceber', label: 'PAGAR PARA / RECEBER DE' },
                { key: 'pagoRecebido', label: 'PAGO / RECEBIDO' },
                { key: 'cancelado', label: 'STATUS' },
                { key: 'acoes', label: '' },
            ],

            fieldsFaturado: [
                { key: 'obra', label: 'OBRA', thStyle: { minWidth: '90px' } },
                { key: 'cliente', label: 'CLIENTE', thStyle: { minWidth: '180px' } },
                { key: 'numeroNota', label: 'NF', thStyle: { minWidth: '160px' } },

                { key: 'valorBruto', label: 'VAL. BRUTO', thStyle: { minWidth: '170px' } },
                { key: 'valorINSS', label: 'INSS', thStyle: { minWidth: '160px' } },
                { key: 'valorISS', label: 'ISS', thStyle: { minWidth: '160px' } },
                { key: 'valorIR', label: 'IR', thStyle: { minWidth: '160px' } },
                { key: 'valorArt30', label: 'ART. 30', thStyle: { minWidth: '160px' } },
                { key: 'desconto', label: 'DESCONTO', thStyle: { minWidth: '160px' } },
                { key: 'valorMaterial', label: 'MATERIAL', thStyle: { minWidth: '160px' } },
                { key: 'sinal', label: 'SINAL', thStyle: { minWidth: '160px' } },
                { key: 'naoComissionado', label: 'NÃO COMISS.', thStyle: { minWidth: '180px' } },
                { key: 'valorLiquido', label: 'VAL. LIQ.', thStyle: { minWidth: '170px' } },

                { key: 'dataFaturamentoPrevisto', label: 'DT. RECEB. PREV.', thStyle: { minWidth: '180px' } },
                { key: 'dataFaturamento', label: 'DT. FAT.', thStyle: { minWidth: '160px' } },
                { key: 'dataRecebimento', label: 'DT. RECEB.', thStyle: { minWidth: '170px' } },

                { key: 'status', label: 'STATUS', thStyle: { minWidth: '140px' } },
                { key: 'acoes', label: 'AÇÕES', thStyle: { minWidth: '160px' } },
            ],

            colunasMedicoes: [
        'obra',
        'descricao',
        'cnpj',
        'razaoSocial',
        'valorBruto',
        'inss',
        'iss',
        'ir',
        'art30',
        'valorLiquido',
        'dataFaturamento',
        'dataRecebimentoPrevista',
        'acoes'
      ],

      opcoesMedicoes: {
        perPage: 1000,
        filterable: false,
        headings: {
          obra: 'Obra',
          descricao: 'Descrição',
          cnpj: 'CNPJ',
          razaoSocial: 'Cliente',
          valorBruto: 'Bruto (R$)',
          inss: 'INSS',
          iss: 'ISS',
          ir: 'IR',
          art30: 'ART 30',
          valorLiquido: 'Liquido (R$)',
          dataFaturamento: 'Data Faturamento',
          dataRecebimentoPrevista: 'Data Receb. Prev.',
          acoes: 'Ações'
        },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: true },
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

            // ... colunas/ops dos modais (mantidos)
            colunasConciliados: ["nome", "numeroNotaFiscal", "pedidoCompra", "fornecedor", "dataVencimento", "valor", "status", "acoes"],
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
                sortable: [],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count: "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
                    first: "Primeiro",
                    last: "último",
                    filter: "",
                    limit: "Itens:",
                    page: "Página:",
                    noResults: "Não encontrado",
                },
            },

            colunasSaldosCancelados: ["motivo", "data", "valor"],
            opcoesSaldosCancelados: {
                perPage: 1000,
                headings: { motivo: "Motivo", data: "Data", valor: "Valor" },
                clientSorting: true,
                sortable: [],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count: "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
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
                    count: "Exibindo {from} de {to} de {count} itens|{count} itens|Um item",
                    first: "Primeiro",
                    last: "último",
                    filter: "",
                    limit: "Itens:",
                    page: "Página:",
                    skin: "table table-striped",
                    noResults: "Não encontrado",
                },
            },
        };
    },
    computed: {
        ajusteManualTotalInformado() {
            try {
                return (this.modalAjusteManual_DataValor || []).reduce((soma, item) => soma + (Number(item.valor) || 0), 0);
            } catch { return 0; }
        },
        ajusteManualRestante() {
            const limite = Number(this.modalAjusteManual_ValorExato || 0);
            return Math.max(limite - this.ajusteManualTotalInformado, 0);
        },
        ajusteManualOk() {
            const limite = Number(this.modalAjusteManual_ValorExato || 0);
            return this.ajusteManualTotalInformado === limite && limite > 0;
        },
        ajusteManualPercent() {
            const limite = Number(this.modalAjusteManual_ValorExato || 0);
            if (!limite) return 0;
            const p = (this.ajusteManualTotalInformado / limite) * 100;
            return Math.max(0, Math.min(100, p));
        },

        modalAjusteManualAFaturar_SomaValores() {
            try {
                return (this.modalAjusteManualAFaturar_Lista || []).reduce((soma, item) => soma + (Number(item.valor) || 0), 0);
            } catch { return 0; }
        },

        modalAjusteManualAFaturar_Saldo() {
            return Number(this.modalAjusteManualAFaturar_LimiteBruto || 0) - this.modalAjusteManualAFaturar_SomaValores;
        },

        modalAjusteManualAFaturar_Ok() {
            const limite = Number(this.modalAjusteManualAFaturar_LimiteBruto || 0);
            return limite > 0 && Math.abs(this.modalAjusteManualAFaturar_Saldo) < 0.005;
        },

        modalAjusteManualAFaturar_Percentual() {
            const limite = Number(this.modalAjusteManualAFaturar_LimiteBruto || 0);
            if (!limite) return 0;
            const percentual = (this.modalAjusteManualAFaturar_SomaValores / limite) * 100;
            return Math.max(0, Math.min(100, percentual));
        },

        totalAFaturar() {
            try {
                return (this.medicoes || []).reduce((soma, item) => soma + (Number(item.valor) || 0), 0);
            } catch { return 0; }
        },

        totalAFaturarLiquido() {
            try {
                return (this.medicoes || []).reduce((soma, item) => soma + (Number(item.valorLiquido) || 0), 0);
            } catch { return 0; }
        },
    },
    methods: {
        // ============================================================
        // Ações das tabelas (usadas pelo <ModalAcoes>).
        // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
        // ============================================================
        montaAcoesDownload() {
            return [
                {
                    label: "Agenda Excel",
                    descricao: "Baixar agenda em XLSX",
                    icone: "file-excel-o",
                    variante: "primary",
                    onClick: () => this.downloadExcel(),
                },
                {
                    label: "Agenda Faturamento Excel",
                    descricao: "Baixar agenda de faturamento em XLSX",
                    icone: "file-excel-o",
                    variante: "primary",
                    onClick: () => this.downloadExcelFaturamento(),
                },
                {
                    label: "Agenda Faturamento PDF",
                    descricao: "Baixar agenda de faturamento em PDF",
                    icone: "file-pdf-o",
                    variante: "primary",
                    onClick: () => this.downloadPdfFaturamento(),
                },
                {
                    label: "Fluxo Caixa PDF",
                    descricao: "Baixar fluxo de caixa em PDF",
                    icone: "file-pdf-o",
                    variante: "primary",
                    onClick: () => this.downloadPDF(),
                },
                {
                    label: "Carimbo Excel",
                    descricao: "Baixar carimbo em XLSX",
                    icone: "file-excel-o",
                    onClick: () => this.downloadExcelCarimbo(),
                },
                {
                    label: "Carimbo Arquivo ZIP",
                    descricao: "Baixar carimbos compactados em ZIP",
                    icone: "file-archive-o",
                    onClick: () => this.downloadZipCarimbo(),
                },
            ];
        },
        abrirModalAcoesDownload() {
            this.modalAcoes_Itens = this.montaAcoesDownload();
            this.modalAcoes_Titulo = "Exportar";
            this.modalAcoes_Exibir = true;
        },

        montaAcoesItensSelecionados() {
            return [
                {
                    label: "Alterar DEF",
                    descricao: "Alterar DEF de todos os itens selecionados",
                    icone: "tag",
                    variante: "primary",
                    onClick: () => this.alterarDefEmLote(),
                },
                {
                    label: "Alterar Data Pagamento",
                    descricao: "Alterar a data de pagamento em lote",
                    icone: "calendar",
                    variante: "primary",
                    onClick: () => this.alterarDataPagamentoEmLote(),
                },
                {
                    label: "Informar pagamento / recebimento",
                    descricao: "Registrar pagamento ou recebimento em lote",
                    icone: "check",
                    variante: "success",
                    onClick: () => this.informarPagamentoRecebimentoEmLote(),
                },
            ];
        },
        abrirModalAcoesItensSelecionados() {
            const qtd = this.agendas.filter(item => item.selecionado).length;
            this.modalAcoes_Itens = this.montaAcoesItensSelecionados();
            this.modalAcoes_Titulo = "Ações em lote (" + qtd + " item" + (qtd === 1 ? "" : "s") + ")";
            this.modalAcoes_Exibir = true;
        },

        montaAcoesNotaFiscal(row) {
            return [
                {
                    label: "Download NF",
                    descricao: "Baixar a nota fiscal",
                    icone: "download",
                    variante: "primary",
                    onClick: () => this.downloadNotaFiscal(row.arquivo.id, row.arquivo.nome),
                },
                {
                    label: "Alterar NF",
                    descricao: "Substituir o arquivo da NF",
                    icone: "pencil",
                    variante: "warning",
                    onClick: () => this.alterarNotaFiscal(row.idPedidoCompraArquivo, row.nome),
                },
            ];
        },
        abrirModalAcoesNotaFiscal(row) {
            this.modalAcoes_Itens = this.montaAcoesNotaFiscal(row);
            this.modalAcoes_Titulo = "Ações da nota fiscal";
            this.modalAcoes_Exibir = true;
        },

        montaAcoesAgendaPagamento(row) {
            return [
                {
                    label: "Notas Fiscais",
                    descricao: "Ver notas fiscais do pedido",
                    icone: "file-text-o",
                    onClick: () => this.exibirModalNotasFiscais(row.idPedidoCompra, row.pedidoCompra),
                    visible: row.idPedidoCompra != null,
                },
                {
                    label: "Pedido Compra",
                    descricao: "Baixar PDF do pedido de compra",
                    icone: "file-pdf-o",
                    onClick: () => this.downloadPdfArquivoCompra(row.idPedidoCompra, row.codigoPedidoCompra + ".pdf"),
                    visible: row.idPedidoCompra != null,
                },
                {
                    label: "Ajuste Manual",
                    descricao: "Ajustar o valor manualmente",
                    icone: "sliders",
                    variante: "warning",
                    onClick: () => this.abreModalAjusteValorManual(row.id, row.valor),
                    visible: row.dataPagamentoEfetuado == null,
                },
                {
                    label: "Cancelar Pagamento/Recebimento",
                    descricao: "Reverter o pagamento efetuado",
                    icone: "undo",
                    variante: "danger",
                    onClick: () => this.cancelarPagamentoRecebimento(row.id),
                    visible: row.dataPagamentoEfetuado != null,
                },
                {
                    label: "Carimbo",
                    descricao: "Aplicar carimbo na NF de serviço",
                    icone: "certificate",
                    onClick: () => this.abrirModalCarimbo(row.idPedidoCompraNotaFiscal),
                    visible: row.idPedidoCompraNotaFiscal != null && row.notaFiscalServico == true,
                },
                {
                    label: "NF com Carimbo",
                    descricao: "Baixar NF já carimbada",
                    icone: "download",
                    onClick: () => this.downloadNotaFiscalComCarimbo(row.idPedidoCompraNotaFiscal),
                    visible: row.idPedidoCompraNotaFiscal != null && row.notaFiscalServico == true,
                },
            ];
        },
        abrirModalAcoesAgendaPagamento(row) {
            this.modalAcoes_Itens = this.montaAcoesAgendaPagamento(row);
            this.modalAcoes_Titulo = "Ações";
            this.modalAcoes_Exibir = true;
        },

        montaAcoesAFaturar(row) {
            return [
                {
                    label: "Ajuste Manual",
                    descricao: "Lançar ajuste manual nesta obra",
                    icone: "sliders",
                    variante: "primary",
                    onClick: () => this.abreModalAjusteManualAFaturar(row.idObra),
                },
            ];
        },
        abrirModalAcoesAFaturar(row) {
            this.modalAcoes_Itens = this.montaAcoesAFaturar(row);
            this.modalAcoes_Titulo = "Ações";
            this.modalAcoes_Exibir = true;
        },

        montaAcoesFaturamento(row) {
            return [
                {
                    label: "Observação",
                    descricao: "Ver observação do faturamento",
                    icone: "sticky-note-o",
                    onClick: () => this.$swal("", (row.observacao || ""), "info"),
                },
                {
                    label: "Reverter Recebimento",
                    descricao: "Desfazer o recebimento já confirmado",
                    icone: "undo",
                    variante: "warning",
                    onClick: () => this.reverterRecebimento(row),
                    visible: row.idStatusFaturamento == 1,
                },
                {
                    label: "Informar Recebimento",
                    descricao: "Registrar que o pagamento foi recebido",
                    icone: "check",
                    variante: "success",
                    onClick: () => this.abrirModalDataFaturamento(row),
                    visible: row.idStatusFaturamento == 2,
                },
                {
                    label: "Cancelar",
                    descricao: "Cancelar este faturamento",
                    icone: "ban",
                    variante: "danger",
                    onClick: () => this.abrirCancelamentoFaturamento(row),
                    visible: row.idStatusFaturamento == 2,
                },
                {
                    label: "Arquivos",
                    descricao: "Ver/enviar anexos deste faturamento",
                    icone: "paperclip",
                    onClick: () => {
                        this.modalArquivos_IdFaturamento = row.id;
                        this.modalArquivos_Arquivos = row.arquivos || [];
                        this.modalArquivos_Exibir = true;
                    },
                },
            ];
        },
        abrirModalAcoesFaturamento(row) {
            this.modalAcoes_Itens = this.montaAcoesFaturamento(row);
            this.modalAcoes_Titulo = "Ações do faturamento";
            this.modalAcoes_Exibir = true;
        },

        limpaFiltros() {
            this.filtro_ObraSelecionada = null;
            this.filtro_ClienteSelecionado = null;
            this.filtro_DefsSelecionados = [];
            this.filtro_FornecedoresSelecionados = [];
            this.filtro_DataInicial = null;
            this.filtro_DataFinal = null;
            this.filtro_NumeroNF = '';
            this.filtro_PedidoInterno = '';
            this.filtro_OrdemCompra = '';
            this.selecionarTodos = false;
            (this.agendas || []).forEach(x => x.selecionado = false);

            this.faturamentos = [];
            this.medicoes = [];
        },

        rowClass(item, type) {
            if (type !== 'row' || !item) return null
            return item.cancelado === true ? 'bg-cancelado' : null
        },

        formataData(data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
        },

        formataDataSemHora(data) {
            return moment(String(new Date(data))).format("DD/MM/YYYY");
        },

        formatBRL(v) {
            return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(Number(v || 0));
        },

        total(campo) {
            return (this.faturamentos || []).reduce((soma, item) => {
                const valor = Number(item[campo]) || 0
                return soma + valor
            }, 0)
        },

        formatCurrency(valor) {
            return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(Number(valor || 0))
        },

        sincronizarItensModalAjusteManualAFaturar() {
            (this.modalAjusteManualAFaturar_Lista || []).forEach(item => {
                item.valor = Number(item.valor || 0);
                item.inss = Number(item.inss || 0);
                item.iss = Number(item.iss || 0);
                item.ir = Number(item.ir || 0);
                item.arT30 = Number(item.arT30 || 0);
                this.recalcularValorLiquidoItemAFaturar(item);
            });
        },

        // ✅ AGORA IGUAL A FATURAMENTO.VUE
        alteraValorFaturamento(id, valor, campo) {
            this.isLoading = true;
            ApiService.alteraValorFaturamento(id, valor, campo, (result) => {
                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    this.obtemAgenda(); // recarrega igual prática do sistema
                }
            });
        },

        // ✅ AGORA IGUAL A FATURAMENTO.VUE
        alterarDataFaturamento(id, valor, tipoData) {
            this.isLoading = true;
            ApiService.alterarDataFaturamento(id, valor, tipoData, (result) => {
                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    this.obtemAgenda();
                }
            });
        },

        memorizarDefAnterior(def) {
            this.defAnterior = def;
        },

        abreModalAjusteETOPedidoCompra(idPedidoCompra) {
            console.log(idPedidoCompra);
        },

        alterarDataIndividual(id, valor) {
            this.isLoading = true;

            ApiService.alterarDataAgenda(id, valor, (result) => {

                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    var objParametros = this.montaObjetoBuscaAgenda();

                    ApiService.obtemAgenda(objParametros, (result) => {

                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.message, "error");
                        } else {
                            this.agendas = result.data;
                        }
                    });
                }
            });
        },

        alterarValorIndividual(id, valor) {
            this.isLoading = true;

            ApiService.alteraValorAgenda(id, valor, (result) => {

                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    var objParametros = this.montaObjetoBuscaAgenda();

                    ApiService.obtemAgenda(objParametros, (result) => {

                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.message, "error");
                        } else {
                            this.agendas = result.data;
                        }
                    });
                }
            });
        },

        exibirModalNotasFiscais(idPedidoCompra) {

            var objetoParametros = {
                idsObra: [],
                idsCliente: [],
                idsMateriais: [],
                idsFornecedor: [],
                idsPedidoCompra: [],
                dataSolicitacaoInicial: null,
                dataSolicitacaoFinal: null
            };

            objetoParametros.idsPedidoCompra.push(idPedidoCompra);

            this.isLoading = true;

            ApiService.obtemPedidosFinalizadosFiltrados(objetoParametros, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {

                    var pedidoCompra = result.data[0];

                    this.modalNotasFiscais_Titulo = 'Notas Fiscais - ' + pedidoCompra.codigo
                    this.modalNotasFiscais_Pedido = pedidoCompra;
                    this.modalNotasFiscais_Notas = pedidoCompra.notasFiscais,
                        this.modalNotasFiscais_SaldosCancelados = pedidoCompra.cancelamentosSaldo;
                    this.modalNotasFiscais_Exibir = true;
                    this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.valorTotal;

                    for (var i = 0; i < this.modalNotasFiscais_Pedido.cancelamentosSaldo.length; i++) {
                        this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.saldo - this.modalNotasFiscais_Pedido.cancelamentosSaldo[i].valor;
                    }

                    for (var i = 0; i < this.modalNotasFiscais_Pedido.notasFiscais.length; i++) {
                        if (this.modalNotasFiscais_Pedido.notasFiscais[i].aprovada != false)
                            this.modalNotasFiscais_Pedido.saldo = this.modalNotasFiscais_Pedido.saldo - this.modalNotasFiscais_Pedido.notasFiscais[i].valor;
                    }
                }
            });
        },

        verificarDef(item) {
            if (this.defAnterior && this.defAnterior.id != item.def.id) {
                this.$swal({
                    title: "Atenção",
                    text: `Deseja alterar o def de '${this.defAnterior.codigo}' para '${item.def.codigo}' ?`,
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Sim",
                    cancelButtonText: "Não",
                }).then((result) => {
                    if (result.isConfirmed) {

                        this.$refs.btnPesquisar && this.$refs.btnPesquisar.focus();

                        ApiService.alteraDefAgenda(item.id, item.def.id, (result) => {

                            if (result.status != 200) {
                                this.$swal("", result.message, "error");
                            } else {
                                var objParametros = this.montaObjetoBuscaAgenda();

                                ApiService.obtemAgenda(objParametros, (result) => {

                                    if (result.status != 200) {
                                        this.$swal("", result.message, "error");
                                    } else {
                                        this.agendas = result.data;
                                    }
                                });
                            }
                        });
                    }
                    else {
                        item.def = this.defAnterior;
                    }
                });

            }
        },

        obtemValoresAFaturar(objParametros) {
            ApiService.obtemValoresAFaturar(objParametros, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.medicoes = result.data || [];
                }
            });
        },

        normalizarDataAjusteManualAFaturar(data) {
            if (!data) return null;
            const dt = new Date(data);
            if (Number.isNaN(dt.getTime())) return null;
            return dt;
        },

        normalizarItemAjusteManualAFaturar(item) {
            const valor = Number(item && item.valor ? item.valor : 0);
            return {
                Id: item && item.id != null ? item.id : (item && item.Id != null ? item.Id : 0),
                Codigo: item && (item.codigo || item.Codigo) ? (item.codigo || item.Codigo) : '',
                Descricao: item && (item.descricao || item.Descricao) ? (item.descricao || item.Descricao) : '',
                CNPJ: item && (item.cnpj || item.CNPJ) ? (item.cnpj || item.CNPJ) : '',
                RazaoSocial: item && (item.razaoSocial || item.RazaoSocial) ? (item.razaoSocial || item.RazaoSocial) : '',
                Valor: valor,
                INSS: Number(item && (item.inss || item.INSS) ? (item.inss || item.INSS) : 0),
                ISS: Number(item && (item.iss || item.ISS) ? (item.iss || item.ISS) : 0),
                IR: Number(item && (item.ir || item.IR) ? (item.ir || item.IR) : 0),
                ART30: Number(item && (item.arT30 || item.art30 || item.ART30) ? (item.arT30 || item.art30 || item.ART30) : 0),
                ValorLiquido: Number(item && (item.valorLiquido || item.ValorLiquido) ? (item.valorLiquido || item.ValorLiquido) : valor),
                DataLancamento: this.normalizarDataAjusteManualAFaturar(item && (item.dataLancamento || item.DataLancamento)) || new Date(),
                DataPagamento: this.normalizarDataAjusteManualAFaturar(item && (item.dataPagamento || item.DataPagamento)) || new Date(),
            };
        },

        recalcularValorLiquidoItemAFaturar(item) {
            const bruto = Number(item.valor || 0);
            const descontos = Number(item.inss || 0) + Number(item.iss || 0) + Number(item.ir || 0) + Number(item.arT30 || 0);
            item.valorLiquido = bruto - descontos;
        },

        criarNovoItemAjusteManualAFaturar(data, valor) {
            const base = this.normalizarItemAjusteManualAFaturar(this.modalAjusteManualAFaturar_ObjetoOriginal || {});
            const novoItem = {
                _uid: ++this.modalAjusteManualAFaturar_Sequencia,
                id: base.Id,
                codigo: base.Codigo,
                descricao: base.Descricao,
                cnpj: base.CNPJ,
                razaoSocial: base.RazaoSocial,
                valor: Number(valor || 0),
                inss: Number(base.INSS || 0),
                iss: Number(base.ISS || 0),
                ir: Number(base.IR || 0),
                arT30: Number(base.ART30 || 0),
                valorLiquido: Number(valor || 0) - (Number(base.INSS || 0) + Number(base.ISS || 0) + Number(base.IR || 0) + Number(base.ART30 || 0)),
                dataLancamento: this.normalizarDataAjusteManualAFaturar(base.DataLancamento) || this.normalizarDataAjusteManualAFaturar(data) || new Date(),
                dataPagamento: this.normalizarDataAjusteManualAFaturar(data) || new Date(),
            };

            return novoItem;
        },

        abrirComPrimeiroItemAjusteManualAFaturar(objeto) {
            const dataBase = this.normalizarDataAjusteManualAFaturar(objeto.dataPagamento || objeto.DataPagamento) || new Date();
            const valorBase = Number(objeto.valor || objeto.Valor || 0);
            this.modalAjusteManualAFaturar_Lista = [this.criarNovoItemAjusteManualAFaturar(dataBase, valorBase)];
        },

        normalizarListaAjusteManualAFaturar(dados) {
            if (Array.isArray(dados)) return dados;
            if (dados == null) return [];
            return [dados];
        },

        abrirComListaAjusteManualAFaturar(lista) {
            this.modalAjusteManualAFaturar_Lista = (lista || []).map(item => {
                const normalizado = this.normalizarItemAjusteManualAFaturar(item);
                const linha = {
                    _uid: ++this.modalAjusteManualAFaturar_Sequencia,
                    id: normalizado.Id,
                    codigo: normalizado.Codigo,
                    descricao: normalizado.Descricao,
                    cnpj: normalizado.CNPJ,
                    razaoSocial: normalizado.RazaoSocial,
                    valor: Number(normalizado.Valor || 0),
                    inss: Number(normalizado.INSS || 0),
                    iss: Number(normalizado.ISS || 0),
                    ir: Number(normalizado.IR || 0),
                    arT30: Number(normalizado.ART30 || 0),
                    valorLiquido: Number(normalizado.ValorLiquido || 0),
                    dataLancamento: this.normalizarDataAjusteManualAFaturar(normalizado.DataLancamento) || new Date(),
                    dataPagamento: this.normalizarDataAjusteManualAFaturar(normalizado.DataPagamento) || new Date(),
                };

                this.recalcularValorLiquidoItemAFaturar(linha);
                return linha;
            });
        },

        abreModalAjusteManualAFaturar(idObra) {
            this.isLoading = true;

            ApiService.obtemValoresAFaturarDaObra(idObra, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal('', result.message || result.data || 'Erro ao obter valores a faturar da obra', 'error');
                } else {
                    const listaOriginal = this.normalizarListaAjusteManualAFaturar(result.data);

                    if (listaOriginal.length === 0) {
                        this.$swal('', 'Nenhum valor a faturar foi encontrado para a obra selecionada', 'warning');
                        return;
                    }

                    const primeiroItem = listaOriginal[0] || {};
                    const limiteBruto = listaOriginal.reduce((soma, item) => {
                        return soma + Number((item && (item.valor != null ? item.valor : item.Valor)) || 0);
                    }, 0);

                    this.modalAjusteManualAFaturar_ObjetoOriginal = primeiroItem;
                    this.modalAjusteManualAFaturar_Data = this.normalizarDataAjusteManualAFaturar(primeiroItem.dataPagamento || primeiroItem.DataPagamento);
                    this.modalAjusteManualAFaturar_Valor = 0.0;
                    this.modalAjusteManualAFaturar_LimiteBruto = Number(limiteBruto || 0);
                    this.modalAjusteManualAFaturar_Sequencia = 0;
                    this.modalAjusteManualAFaturar_Titulo = `Ajuste manual - ${primeiroItem.codigo || primeiroItem.Codigo || ''}`;

                    this.abrirComListaAjusteManualAFaturar(listaOriginal);
                    this.modalAjusteManualAFaturar_Exibir = true;
                }
            });
        },

        adicionarItemAjusteManualAFaturar() {
            if (this.modalAjusteManualAFaturar_Data == null) {
                this.$swal('', 'Data inválida', 'error');
                return;
            }

            if (Number(this.modalAjusteManualAFaturar_Valor || 0) <= 0) {
                this.$swal('', 'Valor inválido', 'error');
                return;
            }

            this.modalAjusteManualAFaturar_Lista.push(
                this.criarNovoItemAjusteManualAFaturar(this.modalAjusteManualAFaturar_Data, this.modalAjusteManualAFaturar_Valor)
            );

            this.modalAjusteManualAFaturar_Data = null;
            this.modalAjusteManualAFaturar_Valor = 0.0;
        },

        removerItemAjusteManualAFaturar(uid) {
            const index = this.modalAjusteManualAFaturar_Lista.findIndex(item => item._uid === uid);
            if (index !== -1) {
                this.modalAjusteManualAFaturar_Lista.splice(index, 1);
            }
        },

        montarPayloadAjusteManualAFaturar() {
            const idObra = this.modalAjusteManualAFaturar_ObjetoOriginal &&
                (this.modalAjusteManualAFaturar_ObjetoOriginal.idObra ||
                 this.modalAjusteManualAFaturar_ObjetoOriginal.IdObra ||
                 this.modalAjusteManualAFaturar_ObjetoOriginal.id);

            const datasValores = (this.modalAjusteManualAFaturar_Lista || []).map(item => {
                const data = this.normalizarDataAjusteManualAFaturar(item.dataPagamento || item.dataFaturamento || item.dataLancamento);
                return {
                    dataFaturamento: data ? data.toISOString().slice(0, 10) : null,
                    valor: Number(item.valor || 0)
                };
            });

            return { idObra, datasValores };
        },

        limparModalAjusteManualAFaturar() {
            this.modalAjusteManualAFaturar_Exibir = false;
            this.modalAjusteManualAFaturar_ObjetoOriginal = null;
            this.modalAjusteManualAFaturar_Data = null;
            this.modalAjusteManualAFaturar_Valor = 0.0;
            this.modalAjusteManualAFaturar_LimiteBruto = 0.0;
            this.modalAjusteManualAFaturar_Lista = [];
            this.modalAjusteManualAFaturar_Sequencia = 0;
            this.modalAjusteManualAFaturar_Titulo = 'Ajuste manual de obras com saldo a faturar';
        },

        salvarAjusteManualAFaturar() {
            if (!this.modalAjusteManualAFaturar_Ok) {
                this.$swal('', 'A soma dos valores deve ser exatamente igual ao limite bruto', 'error');
                return;
            }

            const payload = this.montarPayloadAjusteManualAFaturar();
            this.isLoading = true;

            ApiService.salvarAjusteManualAFaturar(payload, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal('', result.message || result.data || 'Erro ao salvar ajuste manual', 'error');
                } else {
                    this.$swal('', 'Ajuste manual salvo com sucesso', 'success');
                    this.limparModalAjusteManualAFaturar();
                    this.obtemAgenda();
                }
            });
        },

        salvarAjusteManual() {
            var valorInformado = this.modalAjusteManual_DataValor.reduce((soma, item) => soma + item.valor, 0);
            if (valorInformado != this.modalAjusteManual_ValorExato) {
                this.$swal("", "Valor informado divergente do valor total", "error");
                return;
            }

            ApiService.salvarAjusteManual(this.modalAjusteManual_DataValor, this.modalAjusteManual_Id, (result) => {
                this.isLoading = false;

                if (result.status == 200) {
                    this.$swal("", "Valores informados !!!", "success");

                    this.modalAjusteManual_Data = null;
                    this.modalAjusteManual_DataValor = [];
                    this.modalAjusteManual_Id = 0;
                    this.modalAjusteManual_Valor = 0.0;
                    this.modalAjusteManual_ValorExato = 0.0;
                    this.modalAjusteManual_Exibir = false;

                    this.obtemAgenda();
                } else {
                    this.$swal("", result.data, "error");
                }
            });
        },

        abreModalAjusteValorManual(id, valor) {
            this.modalAjusteManual_Exibir = true;
            this.modalAjusteManual_Data = null;
            this.modalAjusteManual_Valor = 0.0;
            this.modalAjusteManual_DataValor = [];
            this.modalAjusteManual_Id = id;
            this.modalAjusteManual_ValorExato = valor;
        },

        adicionaNovaDataValorAjusteManual() {
            if (this.modalAjusteManual_Data == null) {
                this.$swal("", "Data inválida", "error");
                return;
            }

            if (this.modalAjusteManual_Valor <= 0.0) {
                this.$swal("", "Valor inválido", "error");
                return;
            }

            const dataFormatada = new Date(this.modalAjusteManual_Data).toISOString().slice(0, 10);

            if (this.modalAjusteManual_DataValor.some(item => new Date(item.data).toISOString().slice(0, 10) === dataFormatada)) {
                this.$swal("", "Já existe um valor informado para a data escolhida", "error");
                return;
            }

            var objeto = {};
            objeto.data = this.modalAjusteManual_Data;
            objeto.valor = this.modalAjusteManual_Valor;
            objeto.id = this.modalAjusteManual_DataValor.length + 1;

            this.modalAjusteManual_DataValor.push(objeto);

            this.modalAjusteManual_Data = null;
            this.modalAjusteManual_Valor = 0.0;
        },

        removerDataAjusteManual(id) {
            const index = this.modalAjusteManual_DataValor.findIndex(item => item.id === id);
            if (index !== -1) {
                this.modalAjusteManual_DataValor.splice(index, 1);
            }
        },

        checkboxTodos() {
            this.agendas.forEach(item => {
                if (item.cancelado === false)
                    item.selecionado = this.selecionarTodos;
            });
        },

        descricaoDef({ codigo, descricao }) {
            return `${codigo} - ${descricao}`;
        },

        downloadPDF() {
            this.isLoading = true;

            var objetoRequisicao = {
                data: null,
                dataFinal: null
            };

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

        downloadExcelFaturamento() {
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                idsObras: []
            };

            
               if (this.filtro_ObraSelecionada != null) {
                this.filtro_ObraSelecionada.forEach(x => objParametros.idsObras.push(x.id));
            }
            ApiService.downloadExcelAgendaFaturamento(objParametros, (result) => {
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

        downloadPdfFaturamento() {
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                idsObras: []
            };

            if (this.filtro_ObraSelecionada != null) {
                this.filtro_ObraSelecionada.forEach(x => objParametros.idsObras.push(x.id));
            }

            ApiService.downloadPDFAgendaFaturamento(objParametros, (result) => {
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

        downloadExcel() {
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => objParametros.idsClientes.push(x.id));

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => objParametros.idsFornecedores.push(x.id));

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => objParametros.idsObras.push(x.id));

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => objParametros.idsDefs.push(x.id));

            ApiService.downloadExcelAgenda(objParametros, (result) => {
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

        downloadExcelCarimbo() {
            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
            };

            ApiService.downloadExcelCarimbo(objParametros, (result) => {
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

        downloadZipCarimbo() {
            this.isLoading = true;

            var objParametros = {
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
            };

            ApiService.downloadZipCarimbo(objParametros, (result) => {
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

         downloadPdfArquivoCompra(id, codigo) {
            ApiService.downloadPdfPedidoCompra(id, true, (result) => {
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

         alterarDefEmLote() {

            const quantidade = this.agendas.filter(item => item.selecionado === true && item.def.financeiroPodeSerAlterado === false).length;

            if (quantidade > 0) {
                this.$swal("", "Existem itens selecionados que não podem sofrer alteração no DEF. Verifique os itens selecionados.", "error");
                return;
            }
            else {
                const opcoesHtml = this.filtro_Def.map(def => `<option value="${def.id}">${def.codigo}</option>`).join('')

                this.$swal({
                    title: 'Selecione o DEF',
                    html: `<select id="def-select" class="swal2-input"><option value="">Escolha um DEF</option>${opcoesHtml}</select>`,
                    showCancelButton: true,
                    confirmButtonText: 'Confirmar',
                    cancelButtonText: 'Cancelar',
                    preConfirm: () => {
                        const defId = document.getElementById('def-select').value
                        if (!defId) {
                            this.$swal.showValidationMessage('Você precisa selecionar um DEF!')
                            return false
                        }

                        return this.filtro_Def.find(def => def.id.toString() === defId)
                    }
                }).then((result) => {
                    if (result.isConfirmed) {

                        var objeto = {
                            ids: this.agendas.filter(agenda => agenda.selecionado).map(agenda => agenda.id),
                            idDef: result.value.id
                        };

                        this.isLoading = true;

                        ApiService.alterarDefEmLote(objeto, (result) => {

                            if (result.status != 200) {
                                this.isLoading = false;
                                this.$swal("", result.message, "error");
                            } else {
                                var objParametros = this.montaObjetoBuscaAgenda();

                                ApiService.obtemAgenda(objParametros, (result) => {

                                    this.isLoading = false;

                                    if (result.status != 200) {
                                        this.$swal("", result.message, "error");
                                    } else {
                                        this.agendas = result.data;
                                    }
                                });
                            }
                        });
                    }
                })
            }
        },

        informarPagamentoRecebimentoEmLote() {
            this.$swal({
                title: "Atenção",
                text: `Deseja informar o pagamento/recebimento para os itens selecionados?`,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sim",
                cancelButtonText: "Não",
            }).then((result) => {
                if (result.isConfirmed) {
                    var objeto = {
                        ids: this.agendas.filter(agenda => agenda.selecionado).map(agenda => agenda.id),
                    };

                    this.isLoading = true;

                    ApiService.informarPagamentoRecebimentoEmLote(objeto, (result) => {

                        if (result.status != 200) {
                            this.isLoading = false;
                            this.$swal("", result.message, "error");
                        } else {
                            var objParametros = this.montaObjetoBuscaAgenda();

                            ApiService.obtemAgenda(objParametros, (result) => {

                                this.isLoading = false;

                                if (result.status != 200) {
                                    this.$swal("", result.message, "error");
                                } else {
                                    this.agendas = result.data;
                                }
                            });
                        }
                    });

                }
            });
        },

        alterarDataPagamentoEmLote() {
            this.$swal({
                title: 'Selecione a data de pagamento',
                html: `<input id="data-input" type="date" class="swal2-input" min="${new Date().toLocaleDateString('en-CA')}">`,
                showCancelButton: true,
                confirmButtonText: 'Confirmar',
                cancelButtonText: 'Cancelar',
                preConfirm: () => {
                    const data = document.getElementById('data-input').value
                    if (!data) {
                        this.$swal.showValidationMessage('Você precisa selecionar uma data!')
                        return false
                    }
                    return data
                }
            }).then((result) => {
                if (result.isConfirmed) {

                    var objeto = {
                        ids: this.agendas.filter(agenda => agenda.selecionado).map(agenda => agenda.id),
                        data: new Date(result.value + ' 00:00:00')
                    };

                    this.isLoading = true;

                    ApiService.alterarDataAgendaEmLote(objeto, (result) => {

                        if (result.status != 200) {
                            this.isLoading = false;
                            this.$swal("", result.message, "error");
                        } else {
                            var objParametros = this.montaObjetoBuscaAgenda();

                            ApiService.obtemAgenda(objParametros, (result) => {

                                this.isLoading = false;

                                if (result.status != 200) {
                                    this.$swal("", result.message, "error");
                                } else {
                                    this.agendas = result.data;
                                }
                            });
                        }
                    });
                }
            })

        },

        downloadNotaFiscalComCarimbo(id) {
            console.log(id);
            ApiService.downloadNotaFiscalComCarimbo(id, (result) => {
                this.isLoading = false;

                console.log(result);

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

        listaObras() {
            this.filtro_Obras = [];
            ApiService.getAll("Obra", false, (result) => {
                if (result.status != 200) this.$swal("", result.message, "error");
                else this.filtro_Obras = result.data;
            });
        },

        listaClientes() {
            this.filtro_Clientes = [];
            ApiService.getAll("Cliente", false, (result) => {
                if (result.status != 200) this.$swal("", result.message, "error");
                else this.filtro_Clientes = result.data;
            });
        },

        listaDEFs() {
            this.filtro_Def = [];
            ApiService.getAll("DEF", false, (result) => {
                if (result.status != 200) this.$swal("", result.message, "error");
                else this.filtro_Def = result.data;
            });
        },

        listaFornecedores() {
            this.filtro_Fornecedor = [];
            ApiService.getAll("Fornecedor", false, (result) => {
                if (result.status != 200) this.$swal("", result.message, "error");
                else this.filtro_Fornecedor = result.data;
            });
        },

        montaObjetoBuscaAgenda() {
            if (this.filtro_TipoRelatorio == null) {
                this.$swal("Atenção", "Informe o tipo do relatório", "error");
                return;
            }

            var objParametros = {
                tipoRelatorio: this.filtro_TipoRelatorio.id,
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal,
                notaFiscal: this.filtro_NumeroNF,
                pedidoInterno: this.filtro_PedidoInterno,
                ordemCompra: this.filtro_OrdemCompra,
                idsClientes: [],
                idsFornecedores: [],
                idsObras: [],
                idsDefs: [],
            };

            if (this.filtro_ClienteSelecionado != null)
                this.filtro_ClienteSelecionado.forEach(x => objParametros.idsClientes.push(x.id));

            if (this.filtro_FornecedoresSelecionados != null)
                this.filtro_FornecedoresSelecionados.forEach(x => objParametros.idsFornecedores.push(x.id));

            if (this.filtro_ObraSelecionada != null)
                this.filtro_ObraSelecionada.forEach(x => objParametros.idsObras.push(x.id));

            if (this.filtro_DefsSelecionados != null)
                this.filtro_DefsSelecionados.forEach(x => objParametros.idsDefs.push(x.id));

            return objParametros;
        },

        abrirModalDataFaturamento(faturamentoDTO) {
            this.modalDataFaturamento_Faturamento = faturamentoDTO;
            this.modalDataFaturamento_DataFaturamento = null;
            this.modalDataFaturamento_ValorRecebido = 0;
            this.modalDataFaturamento_DataProximoRecebimento = null;
            this.modalDataFaturamento_TipoTotal = true;
            this.modalDataFaturamento_Exibir = true;
        },

        informarRecebimento() {
            if (this.modalDataFaturamento_DataFaturamento == null) {
                this.$swal("Data de recebimento inválida", '', "error");
                return;
            }

            var objetoRecebimento;

            if (this.modalDataFaturamento_TipoTotal) {
                objetoRecebimento = {
                    idFaturamento: this.modalDataFaturamento_Faturamento.id,
                    dataRecebimento: this.modalDataFaturamento_DataFaturamento,
                    valorRecebido: this.modalDataFaturamento_Faturamento.valorLiquido,
                    valorRestante: 0,
                    dataProximoRecebimento: null
                };
            } else {
                if (this.modalDataFaturamento_ValorRecebido <= 0) {
                    this.$swal("Valor recebido inválido", '', "error");
                    return;
                }
                if (this.modalDataFaturamento_ValorRecebido > this.modalDataFaturamento_Faturamento.valorLiquido) {
                    this.$swal("Valor recebido maior do que o valor líquido da nota fiscal", '', "error");
                    return;
                }
                if (this.modalDataFaturamento_DataProximoRecebimento == null) {
                    var valorRestante = this.modalDataFaturamento_Faturamento.valorLiquido - this.modalDataFaturamento_ValorRecebido;
                    this.$swal('Atenção', `Existe um valor aberto de R$ ${valorRestante.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })} que precisa de uma data para o próximo recebimento. Favor informar essa data corretamente.`, "error");
                    return;
                }
                objetoRecebimento = {
                    idFaturamento: this.modalDataFaturamento_Faturamento.id,
                    dataRecebimento: this.modalDataFaturamento_DataFaturamento,
                    valorRecebido: this.modalDataFaturamento_ValorRecebido,
                    valorRestante: this.modalDataFaturamento_Faturamento.valorLiquido - this.modalDataFaturamento_ValorRecebido,
                    dataProximoRecebimento: this.modalDataFaturamento_DataProximoRecebimento
                };
            }

            this.isLoading = true;

            ApiService.informaFaturamento(objetoRecebimento, (result) => {
                this.isLoading = false;
                if (result.status != 200) {
                    this.$swal("Erro ao editar faturamento", result.message, "error");
                } else {
                    this.modalDataFaturamento_Exibir = false;
                    this.$swal("Faturamento recebido", '', "success");
                    this.obtemAgenda();
                }
            });
        },

        reverterRecebimento(faturamentoDTO) {
            this.isLoading = true;

            faturamentoDTO.dataRecebimentoRealizado = null;
            faturamentoDTO.idStatusFaturamento = 2;

            ApiService.put('Faturamento', faturamentoDTO, (result) => {
                this.isLoading = false;
                if (result.status != 200) {
                    this.$swal("Erro ao editar faturamento", result.message, "error");
                } else {
                    this.$swal("Faturamento revertido", '', "success");
                    this.obtemAgenda();
                }
            });
        },

        abrirCancelamentoFaturamento(faturamentoDTO) {
            const hoje = new Date().toISOString().split('T')[0];

            this.$swal({
                title: 'Cancelar faturamento',
                html: `
                    <p style="color: #555; margin-bottom: 16px;">
                        Informe a data em que este valor retornará para <b style="color:#b8860b">A Faturar</b>.
                    </p>
                    <input
                        type="date"
                        id="swal-data-retorno"
                        class="swal2-input"
                        value="${hoje}"
                        min="${hoje}"
                        style="color: #333; background: #fff; border: 2px solid #aaa; border-radius: 10px; width: 80%; padding: 8px 12px; font-size: 15px;"
                    />
                `,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Confirmar cancelamento',
                cancelButtonText: 'Voltar',
                confirmButtonColor: '#dc3545',
                preConfirm: () => {
                    const data = document.getElementById('swal-data-retorno').value;
                    if (!data) {
                        this.$swal.showValidationMessage('Informe uma data de retorno.');
                        return false;
                    }
                    return data;
                }
            }).then((result) => {
                if (result.isConfirmed && result.value) {
                    this.cancelarFaturamento(faturamentoDTO, result.value);
                }
            });
        },

        cancelarFaturamento(faturamentoDTO, dataRetorno) {
            this.isLoading = true;

            const payload = {
                idFaturamento: faturamentoDTO.id,
                dataRetornoFaturamento: dataRetorno
            };

            ApiService.cancelarFaturamento(payload, (result) => {
                this.isLoading = false;
                if (result.status != 200) {
                    this.$swal("Erro ao cancelar faturamento", result.data, "error");
                } else {
                    this.$swal("Faturamento cancelado", '', "success");
                    this.obtemAgenda();
                }
            });
        },

        obtemArquivos(idFaturamento) {
            ApiService.getArquivos(this.modalArquivos_Controller, idFaturamento, (result) => {
                this.isLoading = false;
                if (result.status == 200) {
                    this.modalArquivos_Arquivos = result.data;
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
                    const blob = new Blob([result.data], { type: result.contentType });
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
                } else {
                    this.obtemArquivos(this.modalArquivos_IdFaturamento);
                    this.$swal("Arquivo excluído com sucesso", result.message, "success");
                }
            });
        },

        onChangeModal() {
            const files = [...this.$refs.fileModal.files];
            this.isLoading = true;

            const formData = new FormData();
            files.forEach((file) => {
                formData.append("arquivo", file);
            });

            ApiService.uploadFile(this.modalArquivos_Controller, formData, this.modalArquivos_IdFaturamento, (result) => {
                if (result.status != 200) {
                    this.isLoading = false;
                    this.$swal("", result.message, "error");
                } else {
                    this.obtemArquivos(this.modalArquivos_IdFaturamento);
                    this.$swal("Arquivo enviado com sucesso", result.message, "success");
                }
            });
        },

        obtemAgenda() {
            this.isLoading = true;

            if (this.filtro_TipoRelatorio && this.filtro_TipoRelatorio.id === 2) {
                const idObra = (this.filtro_ObraSelecionada && this.filtro_ObraSelecionada.length === 1) ? this.filtro_ObraSelecionada[0].id : null;
                const idCliente = (this.filtro_ClienteSelecionado && this.filtro_ClienteSelecionado.length === 1) ? this.filtro_ClienteSelecionado[0].id : null;

                const objParametros = {
                    idObra: idObra,
                    idCliente: idCliente,
                    idStatusFaturamento: 0,
                    numeroNotaFiscal: this.filtro_NumeroNF,
                    observacao: null,
                    dataFaturamentoInicial: this.filtro_DataInicial,
                    dataFaturamentoFinal: this.filtro_DataFinal,
                    dataRecebimentoInicial: null,
                    dataRecebimentoFinal: null,
                };

                ApiService.obtemFaturamentos(objParametros, (result) => {
                    this.isLoading = false;

                    if (result.status != 200) {
                        this.$swal("", result.message, "error");
                    } else {
                        this.faturamentos = (result.data && result.data.faturamentosDTO) ? result.data.faturamentosDTO : [];
                        this.obtemValoresAFaturar(objParametros);
                    }
                });

            } else {
                const objParametros = this.montaObjetoBuscaAgenda();

                ApiService.obtemAgenda(objParametros, (result) => {
                    this.isLoading = false;

                    if (result.status != 200) {
                        this.$swal("", result.message, "error");
                    } else {
                        this.agendas = result.data || [];
                    }
                });
            }
        },
    },
    watch: {
        filtro_TipoRelatorio(newVal) {
            this.selecionarTodos = false;
            this.agendas = [];
            this.faturamentos = [];
            this.medicoes = [];
            if (newVal && newVal.id) {
                this.obtemAgenda();
            }
        },
        modalAjusteManualAFaturar_Lista: {
            handler() {
                this.sincronizarItensModalAjusteManualAFaturar();
            },
            deep: true,
        }
    },
    mounted() {
        this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO'));

        this.listaClientes();
        this.listaObras();
        this.listaDEFs();
        this.listaFornecedores();

        this.obtemAgenda();
    },
};
</script>

<style src="./RelatorioAgenda.scss" lang="scss" />

<style>
/* fundo vermelho forte na linha inteira */
.bg-cancelado {
    background-color: #ff000050 !important;
    color: #fff;
}
.table-hover tbody tr.bg-cancelado:hover {
    background-color: #ff000050 !important;
}

/* Totais no bottom-row da b-table */
table.b-table tbody tr.b-table-bottom-row>td {
    background-color: #222;
    color: #fff;
    font-weight: 700;
}

/* ====== TOTALIZADOR A FATURAR ====== */
.af-total-wrap {
    display: flex;
    justify-content: flex-end;
    margin-top: 10px;
}
.af-total-box {
    display: inline-flex;
    align-items: center;
    gap: 12px;
    padding: 10px 14px;
    border-radius: 12px;
    background: rgba(255, 255, 255, 0.06);
    border: 1px solid rgba(255, 255, 255, 0.12);
    box-shadow: 0 6px 18px rgba(0, 0, 0, 0.25);
}
.af-total-label {
    font-size: 18px;
    text-transform: uppercase;
    color: #fff;
}
.af-total-value {
    font-size: 18px;
    font-weight: 600;
    color: limegreen;
}

.aj-progress-afaturar {
    margin-top: 14px;
}


</style>
