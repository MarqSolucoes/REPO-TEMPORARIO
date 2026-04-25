<template>
  <div class="faturamento-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Faturamento
    </h1>

    <b-modal :no-close-on-backdrop="true" id="modalDataFaturamento" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalDataFaturamento_Exibir" size="lg">

      <!-- ── HERO ── -->
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

        <!-- ── SEÇÃO 1 · Resumo do faturamento ── -->
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

        <!-- ── SEÇÃO 2 · Tipo de recebimento ── -->
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

        <!-- ── SEÇÃO 3 · Dados do recebimento ── -->
        <section class="afm-section-card">

          <b-row class="fat-row">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data Recebimento</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalDataFaturamento_DataFaturamento" format="dd/MM/yyyy" type="date" :open.sync="open"
                  class="afm-datepicker">
                </DatePickerMask>
              </div>
            </b-col>
          </b-row>

          <!-- Campos extras apenas no modo parcial -->
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
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalDataFaturamento_DataProximoRecebimento" format="dd/MM/yyyy" type="date" :open.sync="open"
                  class="afm-datepicker">
                </DatePickerMask>
              </div>
            </b-col>
          </b-row>

          <!-- Confirmação visual no modo total -->
          <div v-if="modalDataFaturamento_TipoTotal && modalDataFaturamento_Faturamento != null" class="irf-total-confirm">
            <i class="fa fa-check-circle irf-total-confirm__icon"></i>
            <div>
              <strong>Recebimento total</strong>
              <p>O valor líquido de <b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido) }}</b> será automaticamente registrado como recebido — não é necessário informar o valor manualmente.</p>
            </div>
          </div>

          <!-- Alertas de validação em tempo real no modo parcial -->
          <template v-if="!modalDataFaturamento_TipoTotal && modalDataFaturamento_Faturamento != null">
            <!-- Valor inválido: zero ou negativo -->
            <div v-if="modalDataFaturamento_ValorRecebido <= 0" class="irf-alert irf-alert--warn">
              <i class="fa fa-exclamation-circle"></i>
              <span>O valor recebido deve ser maior que zero.</span>
            </div>
            <!-- Valor excede o líquido -->
            <div v-else-if="modalDataFaturamento_ValorRecebido > modalDataFaturamento_Faturamento.valorLiquido" class="irf-alert irf-alert--error">
              <i class="fa fa-times-circle"></i>
              <span>O valor informado (<b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_ValorRecebido) }}</b>) excede o valor líquido de <b>{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDataFaturamento_Faturamento.valorLiquido) }}</b>.</span>
            </div>
            <!-- Valor válido: mostra saldo restante -->
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

    <b-modal :no-close-on-backdrop="true" id="modalFaturamento" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalFaturamento_Exibir" size="lg">

      <!-- ── HERO ── -->
      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">{{ modalFaturamento_Edicao ? 'Edição de registro' : 'Novo registro' }}</span>
          <h3 class="afm-hero__title">{{ modalFaturamento_Edicao ? 'Editar faturamento' : 'Entrada de faturamento' }}</h3>
          <p class="afm-hero__description">
            {{ modalFaturamento_Edicao
              ? 'Altere os dados do faturamento selecionado e salve para confirmar.'
              : 'Preencha os dados financeiros e fiscais para registrar um novo faturamento.' }}
          </p>
        </div>
        <div class="afm-hero__pill" v-if="!modalFaturamento_Edicao">
          <span>{{ upload_Files.length }}</span>
          <small>arquivo(s)</small>
        </div>
      </div>

      <b-container fluid class="afm-sections">

        <!-- ── SEÇÃO 1 · Identificação ── -->
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor Bruto</label>
                <Money v-model="modalFaturamento_ValorBruto" v-bind="money" ref="valorBruto"
                  @keydown.native.tab="funcaoValorBruto" class="afm-money fat-money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group" v-if="!modalFaturamento_Edicao">
                <label class="afm-label">Obra</label>
                <multiselect v-model="modalFaturamento_ObraSelecionada" :multiple="false"
                  :options="modalFaturamento_Obras" select-label="Selecionar" placeholder="Selecione uma Obra"
                  label="codigo" track-by="codigo" @input="obraSelecionada()">
                </multiselect>
              </div>
            </b-col>
          </b-row>
        </section>

        <!-- ── SEÇÃO 2 · Impostos e Descontos ── -->
        <section class="afm-section-card">
          <b-row class="fat-row">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">INSS</label>
                <Money v-model="modalFaturamento_ValorINSS" v-bind="money" ref="valorINSS"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">ISS</label>
                <Money v-model="modalFaturamento_ValorISS" v-bind="money" ref="valorISS"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
          </b-row>
          <b-row class="fat-row">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">IR</label>
                <Money v-model="modalFaturamento_ValorIR" v-bind="money" ref="valorIR"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Art 30</label>
                <Money v-model="modalFaturamento_ValorArt30" v-bind="money" ref="valorArt30"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
          </b-row>
          <b-row class="fat-row">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Material</label>
                <Money v-model="modalFaturamento_Material" v-bind="money" class="afm-money fat-money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor Não Comissionado</label>
                <Money v-model="modalFaturamento_ValorNaoComissionado" v-bind="money" class="afm-money fat-money"></Money>
              </div>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Desconto</label>
                <Money v-model="modalFaturamento_ValorDesconto" v-bind="money" ref="valorDesconto"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Desconto Sinal</label>
                <Money v-model="modalFaturamento_ValorDescontoSinal" v-bind="money"
                  class="afm-money fat-money"></Money>
              </div>
            </b-col>
          </b-row>
        </section>

        <!-- ── SEÇÃO 3 · Resumo Financeiro ── -->
        <section class="afm-section-card">
          <div class="afm-summary-grid fat-summary-2col">
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Valor Líquido</span>
              <strong class="afm-summary-card__value mono">
                {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalFaturamento_ValorLiquido) }}
              </strong>
              <Money v-model="modalFaturamento_ValorLiquido" v-bind="money" ref="valorLiquido" :disabled="true"
                style="display:none"></Money>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Valor Líquido com Sinal</span>
              <strong class="afm-summary-card__value mono">
                {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalFaturamento_ValorLiquidoSinal) }}
              </strong>
              <Money v-model="modalFaturamento_ValorLiquidoSinal" v-bind="money" ref="valorLiquido" :disabled="true"
                style="display:none"></Money>
            </div>
          </div>
        </section>

        <!-- ── SEÇÃO 4 · Datas e Nota Fiscal ── -->
        <section class="afm-section-card">
          <b-row class="fat-row">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data Faturamento</label>
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                  v-model="modalFaturamento_DataFaturamento" format="dd/MM/yyyy" type="date" :open.sync="open"
                  class="afm-datepicker">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data Recebimento</label>
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                  v-model="modalFaturamento_DataRecebimento" format="dd/MM/yyyy" type="date" :open.sync="open"
                  class="afm-datepicker">
                </DatePickerMask>
              </div>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Número NF</label>
                <b-form-input v-model="modalFaturamento_NumeroNF" class="fat-input" placeholder="Ex: 000123"></b-form-input>
              </div>
            </b-col>
          </b-row>
        </section>

        <!-- ── SEÇÃO 5 · Observação ── -->
        <section class="afm-section-card">
          <div class="afm-field-group">
            <textarea-autosize id="textarea" v-model="modalFaturamento_Observacao"
              class="form-control fat-textarea" :min-height="100"
              placeholder="Adicione uma observação (opcional)…" />
          </div>
        </section>

        <!-- ── SEÇÃO 6 · Anexos (só no cadastro) ── -->
        <section class="afm-section-card" v-if="!modalFaturamento_Edicao">
          <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input"
            @change="onChange" ref="file" multiple="true" />
          <div v-if="upload_Files.length > 0" class="fat-file-list">
            <div class="fat-file-item" v-for="item in upload_Files" :key="item.name">
              <i class="fa fa-file-o fat-file-icon"></i>
              <span>{{ item.name }}</span>
            </div>
          </div>
          <div v-else class="afm-empty-state">
            <i class="fa fa-paperclip"></i>
            <div>
              <strong>Nenhum arquivo selecionado</strong>
              <p>Clique em <b>Escolher Arquivos</b> para adicionar documentos.</p>
            </div>
          </div>
          <div class="fat-file-actions">
            <label type="button" class="btn btn-success fat-choose-btn" for="fileInput">
              <i class="fa fa-folder-open mr-2"></i>Escolher Arquivos
            </label>
            <b-button v-on:click="removerArquivos()" v-if="upload_Files.length > 0"
              variant="outline-danger" class="fat-remove-files-btn">
              <i class="fa fa-trash mr-2"></i>Remover Arquivos
            </b-button>
          </div>
        </section>

      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary">
            <span>Modo:</span>
            <strong>{{ modalFaturamento_Edicao ? 'Edição de faturamento existente' : 'Novo faturamento' }}</strong>
          </div>
          <div class="afm-footer__actions">
            <b-button @click="modalFaturamento_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
            <button type="button" class="btn btn-success afm-save-btn" v-on:click="enviarFaturamento()"
              v-if="!modalFaturamento_Edicao">
              <i class="fa fa-check mr-2"></i>Cadastrar Faturamento
            </button>
            <button type="button" class="btn btn-success afm-save-btn" v-on:click="editarFaturamento()" v-else>
              <i class="fa fa-save mr-2"></i>Salvar Alterações
            </button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalArquivos_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Anexos</span>
          <h3 class="afm-hero__title">{{ modalArquivos_Titulo }}</h3>
          <p class="afm-hero__description">Consulte, baixe ou envie arquivos vinculados.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

        <input type="file" name="fileModal" id="fileInputModal" style="display: none" class="hidden-input"
          @change="onChangeModal" ref="fileModal" />

        <Widget customHeader class="estiloWidget">
          <b-row>
            <b-col>
              <div class="table-card table-card--fluid table-card--scroll-x">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho">Arquivo</th>
                      <th class="estilo-cabecalho">Usuário</th>
                      <th class="estilo-cabecalho texto-centro">Data de envio</th>
                      <th class="estilo-cabecalho texto-centro"></th>
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
                    <tr v-if="!modalArquivos_Arquivos || modalArquivos_Arquivos.length === 0">
                      <td class="estilo-celula texto-centro" colspan="4">Nenhum arquivo</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </b-col>
          </b-row>
        </Widget>

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

    <b-modal :no-close-on-backdrop="true" id="modalDetalheObra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalDetalheObra_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Obra</span>
          <h3 class="afm-hero__title">{{ modalDetalheObra_Titulo }}</h3>
          <p class="afm-hero__description">Detalhes financeiros e indicadores da obra.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">


        <Widget customHeader class="estiloWidget">
          <b-row>
            <b-col md="12">
              <label class="mr-3">Cliente: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.cliente.nomeFantasia : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">CNPJ: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.cliente.cnpj : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Inscrição Estadual: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.cliente.inscricaoEstadual : '' }}</label>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="12">
              <label class="mr-3">Endereço: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.enderecoObra : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Cidade: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.cidade.nome
                : '' }}</label>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="12">
              <label class="mr-3">Proposta: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.codigoProposta : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Data Proposta: {{ this.filtro_ObraSelecionada != null ?
                formataDataSemHora(this.filtro_ObraSelecionada.dataProposta) : '' }}</label>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="12">
              <label class="mr-3">Prazo Obra: {{ this.filtro_ObraSelecionada != null ?
                this.filtro_ObraSelecionada.prazoDias : '' }} dias</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Data Inicio da Obra: {{ this.filtro_ObraSelecionada != null ?
                formataDataSemHora(this.filtro_ObraSelecionada.dataInicio) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Data Fim da Obra: {{ this.filtro_ObraSelecionada != null ?
                formataDataSemHora(this.filtro_ObraSelecionada.dataFim) : '' }}</label>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Total: {{ this.filtro_ObraSelecionada != null ? new Intl.NumberFormat("pt-BR", {
                style: "currency", currency: "BRL"
              }).format(this.filtro_ObraSelecionada.valorTotal) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Custo: {{ this.filtro_ObraSelecionada != null ? new Intl.NumberFormat("pt-BR", {
                style: "currency", currency: "BRL"
              }).format(this.filtro_ObraSelecionada.eto) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Material: {{ this.filtro_ObraSelecionada != null ? new
                Intl.NumberFormat("pt-BR",
                  { style: "currency", currency: "BRL" }).format(this.filtro_ObraSelecionada.valorMaterial) : ''
              }}</label>
            </b-col>
          </b-row>

        </Widget>

        <!-- <Widget customHeader class="estiloWidget"
          v-if="this.filtro_ObraSelecionada != null && this.filtro_ObraSelecionada.medicoes != null">


          <h2>Faturamentos</h2><br />

          <b-row>
            <b-col md="12">
              <ul id="example-2">
                <li v-for="item in this.filtro_ObraSelecionada.faturamentos" v-bind:key="item.name">
                  Data Faturamento: {{ formataDataSemHora(item.dataFaturamento) }} - Data Recebimento Previsto: {{
                    formataDataSemHora(item.dataRecebimentoPrevisto) }} - Valor Bruto: {{ new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency: "BRL"
                  }).format(item.valorBruto) }}
                </li>
              </ul>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="12">
              Valor a receber: {{ new Intl.NumberFormat("pt-BR", {
                style: "currency", currency: "BRL"
              }).format(this.filtro_ObraSelecionada.valorAFaturar) }}
            </b-col>
          </b-row>
          
        </Widget>

        <b-row>
          <b-col lg="12" xs="12">
            <Widget customHeader class="estiloWidget">

              <h2>Medições</h2><br />

              <b-button v-on:click="edicaoValorTotalMedicao = true" v-if="edicaoValorTotalMedicao == false"
                variant="outline-info" class="width-200 mb-3 mr-3">
                <span>Editar Valor Total</span>
              </b-button>
              <b-button
                v-on:click="modalMedicao_DataPrevista = null; modalMedicao_ValorPrevistoAjustado = 0; modalMedicao_Exibir = true;"
                v-if="edicaoValorTotalMedicao" variant="outline-success" class="width-200 mb-3 mr-3">
                <span>Nova Medição Extra</span>
              </b-button>

              <br />

              <div v-if="edicaoValorTotalMedicao">
                <label class="mr-3">Valor Total Ajustado:</label>
                <Money v-model="obra_ValorTotalAjustado" v-bind="money" >
                </Money>

                <br /><br />

                <label class="mr-3">Somatória medições ajustadas: {{ new Intl.NumberFormat("pt-BR", {
                  style: "currency",
                  currency: "BRL",
                }).format(somatoriaValoresAjustadosMedicoes) }}</label>

                <br /><br />

                <label class="mr-3">Saldo medições ajustadas: {{ new Intl.NumberFormat("pt-BR", {
                  style: "currency", currency:
                    "BRL",
                }).format(this.obra_ValorTotalAjustado - somatoriaValoresAjustadosMedicoes) }}</label>
              </div>

              <div class="table-card table-card--fluid table-card--scroll-x">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho texto-centro">Dt. Prevista Faturamento</th>
                      <th class="estilo-cabecalho texto-centro">Dt. Prevista Recebimento</th>
                      <th class="estilo-cabecalho texto-centro">Previsto (R$)</th>
                      <th class="estilo-cabecalho texto-centro">Previsto Ajustado (R$)</th>
                      <th class="estilo-cabecalho texto-centro">Faturado (R$)</th>
                      <th class="estilo-cabecalho texto-centro">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(row, index) in (filtro_ObraSelecionada != null ? filtro_ObraSelecionada.medicoes : [])" :key="'medE-' + (row.id || index)">
                      <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevista) }}</td>
                      <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevistaRecebimento) }}</td>
                      <td class="estilo-celula texto-centro">{{ row.valorPrevisto != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevisto) : '' }}</td>
                      <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevistoAjustado) }}</td>
                      <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorFaturado) }}</td>
                      <td class="estilo-celula texto-centro">
                        <b-button v-if="edicaoValorTotalMedicao == true && new Date() < new Date(row.dataPrevista)"
                          v-on:click="abreModalEdicaoMedicao(row)" variant="outline-warning" class="width-40 mb-3 mr-3">
                          <i class="fa fa-pencil" title="Editar"></i>
                        </b-button>
                        <b-button v-if="edicaoValorTotalMedicao == true && row.valorPrevisto == null"
                          v-on:click="excluiMedicao(row)" variant="outline-danger" class="width-40 mb-3 mr-3">
                          <i class="fa fa-close" title="Excluir"></i>
                        </b-button>
                      </td>
                    </tr>
                    <tr v-if="!filtro_ObraSelecionada || !filtro_ObraSelecionada.medicoes || filtro_ObraSelecionada.medicoes.length === 0">
                      <td class="estilo-celula texto-centro" colspan="6">Nenhum registro encontrado</td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <b-button v-on:click="edicaoValorTotalMedicao = false" v-if="edicaoValorTotalMedicao == true"
                variant="info" class="width-200 mb-3 mr-3">
                <span>Cancelar</span>
              </b-button>
              <b-button v-on:click="salvarMedicoes()" v-if="edicaoValorTotalMedicao == true" variant="success"
                class="width-200 mb-3 mr-3">
                <span>Salvar</span>
              </b-button>
            </Widget>
          </b-col>
        </b-row> -->

              </section>
      </b-container>


      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalDetalheObra_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalDetalheCliente" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalDetalheCliente_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Cliente</span>
          <h3 class="afm-hero__title">{{ modalDetalheCliente_Titulo }}</h3>
          <p class="afm-hero__description">Informações e indicadores financeiros do cliente.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">


        <Widget customHeader class="estiloWidget">
          <b-row>
            <b-col md="12">
              <label class="mr-3">Cliente: {{ this.filtro_ClienteSelecionado != null ?
                this.filtro_ClienteSelecionado.nomeFantasia : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">CNPJ: {{ this.filtro_ClienteSelecionado != null ?
                this.filtro_ClienteSelecionado.cnpj : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Inscrição Estadual: {{ this.filtro_ClienteSelecionado != null ?
                this.filtro_ClienteSelecionado.inscricaoEstadual : '' }}</label>
            </b-col>
          </b-row>

        </Widget>

        <Widget customHeader class="estiloWidget"
          v-if="(this.filtro_ClienteSelecionado != null && this.filtro_ClienteSelecionado.detalhesObras != null)">
          <h2>Obras</h2><br />

          <b-row>
            <b-col md="12">
              <label class="mr-3">Obras Ativas: {{ this.filtro_ClienteSelecionado.detalhesObras != null ?
                this.filtro_ClienteSelecionado.detalhesObras.numeroObrasAtivas : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Total das Obras: {{ this.filtro_ClienteSelecionado.detalhesObras != null ?
                new Intl.NumberFormat("pt-BR", {
                  style: "currency", currency: "BRL"
                }).format(this.filtro_ClienteSelecionado.detalhesObras.valorTotalObrasAtivas) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor a Faturar: {{ this.filtro_ClienteSelecionado.detalhesObras != null ?
                new Intl.NumberFormat("pt-BR", {
                  style: "currency", currency: "BRL"
                }).format(this.filtro_ClienteSelecionado.detalhesObras.valorAFaturarObrasAtivas) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Faturado a Receber: {{ this.filtro_ClienteSelecionado.detalhesObras != null ?
                new Intl.NumberFormat("pt-BR", {
                  style: "currency", currency: "BRL"
                }).format(this.filtro_ClienteSelecionado.detalhesObras.valorFaturadoAReceberObrasAtivas) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Faturado Recebido: {{ this.filtro_ClienteSelecionado.detalhesObras != null ?
                new Intl.NumberFormat("pt-BR", {
                  style: "currency", currency: "BRL"
                }).format(this.filtro_ClienteSelecionado.detalhesObras.valorFaturadoRecebidoObrasAtivas) : '' }}</label>
            </b-col>
          </b-row>
        </Widget>

              </section>
      </b-container>


      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalDetalheCliente_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="medicao" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalMedicao_Exibir"
      size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Medição</span>
          <h3 class="afm-hero__title">{{ modalMedicao_Titulo }}</h3>
          <p class="afm-hero__description">Informe datas previstas e valor ajustado da medição extra.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista faturamento</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevista"
                  format="dd/MM/yyyy" type="date" :open.sync="open" @change="calculaDataRecebimento()">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista recebimento</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalMedicao_DataPrevistaRecebimento" format="dd/MM/yyyy" type="date" :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Valor previsto ajustado</label>
                <Money v-model="modalMedicao_ValorPrevistoAjustado" v-bind="money"></Money>
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalMedicao_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="cadastrarMedicaoExtra()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Cadastrar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="medicaoEdicao" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalMedicaoEdicao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Medição</span>
          <h3 class="afm-hero__title">{{ modalMedicaoEdicao_Titulo }}</h3>
          <p class="afm-hero__description">Edite o valor ajustado e a data de recebimento prevista.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor previsto ajustado</label>
                <Money v-model="modalMedicaoEdicao_ValorPrevistoAjustado" v-bind="money"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data Recebimento Prevista</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalMedicaoEdicao_DataRecebimentoPrevista" format="dd/MM/yyyy" type="date" :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalMedicaoEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="editarMedicao()" variant="info" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <Widget customHeader class="estiloWidget fat-filter-widget">
      <b-row class="fat-filter-row">
        <b-col sm="12" md="6" lg="4" class="fat-filter-col">
          <label class="fat-filter-label">Obra:</label>
          <div class="fat-input-with-action">
            <div class="fat-input-with-action__field">
              <multiselect v-model="filtro_ObraSelecionada" :multiple="false" :options="filtro_Obras"
                select-label="Selecionar" placeholder="Selecione uma Obra" label="codigo" track-by="codigo">
              </multiselect>
            </div>
            <button type="button" class="btn btn-info fat-info-btn" @click="exibirModalDetalheObra()" title="Detalhes">
              <i class="fa fa-info"></i>
            </button>
          </div>
        </b-col>
        <b-col sm="12" md="6" lg="4" class="fat-filter-col">
          <label class="fat-filter-label">Cliente:</label>
          <div class="fat-input-with-action">
            <div class="fat-input-with-action__field">
              <multiselect v-model="filtro_ClienteSelecionado" :multiple="false" :options="filtro_Clientes"
                select-label="Selecionar" placeholder="Selecione um Cliente" label="nomeFantasia"
                track-by="nomeFantasia">
              </multiselect>
            </div>
            <button type="button" class="btn btn-info fat-info-btn" @click="exibirModalDetalheCliente()" title="Detalhes">
              <i class="fa fa-info"></i>
            </button>
          </div>
        </b-col>
        <b-col sm="12" md="12" lg="4" class="fat-filter-col">
          <label class="fat-filter-label">Número NF:</label>
          <b-form-input v-model="filtro_NumeroNF" style="color: white"></b-form-input>
        </b-col>
      </b-row>

      <b-row class="fat-filter-row">
        <b-col sm="6" md="3" class="fat-filter-col">
          <label class="fat-filter-label">Data Faturamento Inicial:</label>
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFaturamentoInicial" format="dd/MM/yyyy"
            type="date" class="fat-datepicker">
          </DatePickerMask>
        </b-col>
        <b-col sm="6" md="3" class="fat-filter-col">
          <label class="fat-filter-label">Data Faturamento Final:</label>
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFaturamentoFinal" format="dd/MM/yyyy"
            type="date" class="fat-datepicker">
          </DatePickerMask>
        </b-col>
        <b-col sm="6" md="3" class="fat-filter-col">
          <label class="fat-filter-label">Data Recebimento Inicial:</label>
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataRecebimentoInicial" format="dd/MM/yyyy"
            type="date" class="fat-datepicker">
          </DatePickerMask>
        </b-col>
        <b-col sm="6" md="3" class="fat-filter-col">
          <label class="fat-filter-label">Data Recebimento Final:</label>
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataRecebimentoFinal" format="dd/MM/yyyy"
            type="date" class="fat-datepicker">
          </DatePickerMask>
        </b-col>
      </b-row>

      <b-row class="fat-filter-row">
        <b-col sm="12" md="8" class="fat-filter-col">
          <label class="fat-filter-label">Observação:</label>
          <b-form-input v-model="filtro_Observacao" style="color: white"></b-form-input>
        </b-col>
        <b-col sm="12" md="4" class="fat-filter-col">
          <label class="fat-filter-label">Status:</label>
          <multiselect v-model="filtro_StatusFaturamentoSelecionado" :multiple="false"
            :options="filtro_StatusFaturamento" select-label="Selecionar" placeholder="Selecione um Status"
            label="descricao" track-by="descricao">
          </multiselect>
        </b-col>
      </b-row>

      <b-row class="fat-filter-row fat-filter-actions">
        <b-col sm="12" class="fat-filter-buttons">
          <button @click="limpaFiltros()" v-b-modal.cadastro type="button" class="btn width-120 mb-0 btn-danger">
            Limpar Filtros
          </button>
          <button @click="listaFaturamentos()" v-b-modal.cadastro type="button"
            class="btn width-120 mb-0 btn-success">
            Pesquisar
          </button>
        </b-col>
      </b-row>
    </Widget>

    <Widget customHeader class="estiloWidget fat-summary-widget">
      <b-row>
        <b-col sm="12" md="6">
          <label class="mr-3">A faturar previsto período: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.somaAFaturar) }}</label>
        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Sinal Recebido: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.sinalRecebido) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" md="6">
          <label class="mr-3">Valor faturado bruto a receber: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.somaFaturadoAReceber) }}</label>
        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Sinal Descontado: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.sinalDescontado) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" md="6">
          <label class="mr-3">Valor faturado liquido a receber: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.somaFaturadoAReceberLiquido) }}</label>
        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Saldo a Descontar: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.saldoSinal) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" md="6">
          <label class="mr-3">Valor faturado recebido: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.somaFaturadoRecebido) }}</label>
        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Valor não Comissionado: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.valorNaoComissionado) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" md="6">
          <label class="mr-3">Saldo a Faturar: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.saldoAFaturar) }}</label>
        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Valor não Comissionado Utilizado: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.valorNaoComissionadoUtilizado) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" md="6" class="d-none d-md-block">

        </b-col>
        <b-col sm="12" md="6">
          <label class="mr-3">Saldo do Valor não Comissionado: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.saldoValorNaoComissionado) }}</label>
        </b-col>
      </b-row>
    </Widget>


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
                  <th class="estilo-cabecalho texto-centro">Líquido (R$)</th>
                  <th class="estilo-cabecalho texto-centro">Data Faturamento</th>
                  <th class="estilo-cabecalho texto-centro">Data Receb. Prev.</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in medicoes" :key="'af-' + (row.id || index)">
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
      <b-row class="fat-entrada-row">
        <b-col sm="12" class="fat-entrada-col">
          <button @click="modalFaturamento_Exibir = true;
          modalFaturamento_Edicao = false;
          modalFaturamento_ValorBruto = 0.0;
          modalFaturamento_ValorINSS = 0.0;
          modalFaturamento_ValorISS = 0.0;
          modalFaturamento_ValorIR = 0.0;
          modalFaturamento_ValorArt30 = 0.0;
          modalFaturamento_ValorDesconto = 0.0;
          modalFaturamento_ValorLiquido = 0.0;
          modalFaturamento_DataFaturamento = null;
          modalFaturamento_DataRecebimento = null;
          modalFaturamento_NumeroNF = '';
          modalFaturamento_Observacao = '';
          modalFaturamento_Faturamento = null;" v-b-modal.cadastro type="button"
            class="btn width-120 mb-3 btn-outline-success">
            Entrada Faturamento
          </button>
        </b-col>
      </b-row>

      <b-row>
        <b-col lg="12">
          <div class="table-card table-card--scroll-x">
            <table class="estilo-tabela tabela-identidade">
              <thead>
                <tr>
                  <th class="estilo-cabecalho texto-centro">Obra</th>
                  <th class="estilo-cabecalho">Cliente</th>
                  <th class="estilo-cabecalho texto-centro">NF</th>
                  <th class="estilo-cabecalho texto-centro">Val. Bruto</th>
                  <th class="estilo-cabecalho texto-centro">INSS</th>
                  <th class="estilo-cabecalho texto-centro">ISS</th>
                  <th class="estilo-cabecalho texto-centro">IR</th>
                  <th class="estilo-cabecalho texto-centro">ART. 30</th>
                  <th class="estilo-cabecalho texto-centro">Desconto</th>
                  <th class="estilo-cabecalho texto-centro">Material</th>
                  <th class="estilo-cabecalho texto-centro">Sinal</th>
                  <th class="estilo-cabecalho texto-centro">Não Comissionado</th>
                  <th class="estilo-cabecalho texto-centro">Val. Líquido</th>
                  <th class="estilo-cabecalho texto-centro">Dt. Receb. Prev.</th>
                  <th class="estilo-cabecalho texto-centro">Dt. Fat. Real</th>
                  <th class="estilo-cabecalho texto-centro">Dt. Recebimento</th>
                  <th class="estilo-cabecalho texto-centro">Status</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in faturamentos" :key="'fat-' + (row.id || index)">
                  <td class="estilo-celula texto-centro">{{ row.obra.codigo }}</td>
                  <td class="estilo-celula">{{ row.obra.cliente.nomeFantasia }}</td>
                  <td class="estilo-celula texto-centro">{{ row.numeroNF }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorBruto) }}</td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorINSS) }}
                    </template>
                    <Money v-else v-model="row.valorINSS" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorINSS, 'INSS')" />
                  </td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorISS) }}
                    </template>
                    <Money v-else v-model="row.valorISS" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorISS, 'ISS')" />
                  </td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorIR) }}
                    </template>
                    <Money v-else v-model="row.valorIR" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorIR, 'IR')" />
                  </td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorArt30) }}
                    </template>
                    <Money v-else v-model="row.valorArt30" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorArt30, 'Art30')" />
                  </td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorDesconto) }}
                    </template>
                    <Money v-else v-model="row.valorDesconto" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorDesconto, 'Desconto')" />
                  </td>

                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMaterial) }}</td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorSinal) }}
                    </template>
                    <Money v-else v-model="row.valorSinal" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorSinal, 'Sinal')" />
                  </td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorNaoComissionado) }}
                    </template>
                    <Money v-else v-model="row.valorNaoComissionado" v-bind="money" class="input-table input-table--money"
                      @keydown.native.enter="alteraValorFaturamento(row.id, row.valorNaoComissionado, 'NaoComissionado')" />
                  </td>

                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorLiquido) }}</td>

                  <td class="estilo-celula texto-centro">
                    <template v-if="row.idStatusFaturamento != 2">
                      {{ formataDataSemHora(row.dataRecebimentoPrevisto) }}
                    </template>
                    <DatePickerMask v-else :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                      v-model="row.dataRecebimentoPrevisto" format="dd/MM/yyyy" type="date" :open.sync="open"
                      class="input-table input-table--date"
                      @change="() => alterarDataFaturamento(row.id, row.dataRecebimentoPrevisto, 2)"
                      @keydown.native.enter="() => alterarDataFaturamento(row.id, row.dataRecebimentoPrevisto, 2)">
                    </DatePickerMask>
                  </td>

                  <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataFaturamento) }}</td>
                  <td class="estilo-celula texto-centro">{{ row.dataRecebimentoRealizado != null ? formataDataSemHora(row.dataRecebimentoRealizado) : '' }}</td>
                  <td class="estilo-celula texto-centro">{{ row.status.descricao }}</td>

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
                  <td class="estilo-celula texto-centro" colspan="18">Nenhum registro encontrado</td>
                </tr>

                <tr v-if="faturamentos && faturamentos.length > 0" class="linha-totais">
                  <td colspan="3" class="estilo-celula total-cell">Totais:</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorBruto')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorINSS')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorISS')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorIR')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorArt30')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorDesconto')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorMaterial')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorSinal')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorNaoComissionado')) }}</td>
                  <td class="estilo-celula texto-centro total-cell">{{ formatCurrency(total('valorLiquido')) }}</td>
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
                    :open.sync="open" class="aj-datepicker afm-datepicker" />
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
                        v-model="dataValor.data" format="dd/MM/yyyy" type="date"
                        :open.sync="open" />
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
                <label class="afm-label">Data de faturamento</label>
                <div class="afm-input-wrap">
                  <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                    v-model="modalAjusteManualAFaturar_Data" format="dd/MM/yyyy" type="date"
                    :open.sync="open" class="aj-datepicker afm-datepicker" />
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
                      <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                        v-model="item.dataPagamento" format="dd/MM/yyyy" type="date"
                        :open.sync="open" />
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
    <!-- ===================== /MODAIS ===================== -->

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
  name: 'Faturamento',
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

      dataMinima: (new Date()).setDate(new Date().getDate() - 1),

      open: false,
      isLoading: false,

      usuarioLogado: null,

      modalDataFaturamento_Exibir: false,
      modalDataFaturamento_DataFaturamento: null,
      modalDataFaturamento_Faturamento: null,
      modalDataFaturamento_ValorRecebido: null,
      modalDataFaturamento_DataProximoRecebimento: null,
      modalDataFaturamento_TipoTotal: true,

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

      modalFaturamento_Exibir: false,
      modalFaturamento_Titulo: 'Entrada de Faturamento',
      modalFaturamento_Obras: [],
      modalFaturamento_ObraSelecionada: null,
      modalFaturamento_ValorBruto: 0.0,
      modalFaturamento_ValorINSS: 0.0,
      modalFaturamento_ValorISS: 0.0,
      modalFaturamento_ValorIR: 0.0,
      modalFaturamento_ValorArt30: 0.0,
      modalFaturamento_ValorDesconto: 0.0,
      modalFaturamento_ValorLiquido: 0.0,
      modalFaturamento_ValorLiquidoSinal: 0.0,
      modalFaturamento_DataFaturamento: null,
      modalFaturamento_DataRecebimento: null,
      modalFaturamento_NumeroNF: '',
      modalFaturamento_Observacao: '',
      modalFaturamento_Edicao: false,
      modalFaturamento_Faturamento: null,
      modalFaturamento_ValorDescontoSinal: 0.0,
      modalFaturamento_Material: 0.0,
      modalFaturamento_ValorNaoComissionado: 0.0,

      obra_ValorTotalAjustado: 0.0,
      edicaoValorTotalMedicao: false,
      somatoriaValoresAjustadosMedicoes: 0.0,
      modalDetalheObra_Titulo: 'Nova medição extra',
      modalDetalheObra_ValorPrevistoAjustado: 0.0,
      modalDetalheObra_DataPrevista: null,
      modalDetalheObra_DataPrevistaRecebimento: null,
      modalDetalheObra_Exibir: false,
      modalDetalheObra_Titulo: '',

      modalInformacoes_Exibir: false,
      modalMedicao_Exibir: false,
      modalMedicao_Titulo: 'Nova medição extra',
      modalMedicao_ValorPrevistoAjustado: 0.0,
      modalMedicao_DataPrevista: null,
      modalMedicao_DataPrevistaRecebimento: null,

      modalDetalheCliente_Exibir: false,
      modalDetalheCliente_Titulo: '',

      modalArquivos_Exibir: false,
      modalArquivos_Titulo: 'Arquivos',
      modalArquivos_Arquivos: [],
      modalArquivos_Controller: 'Faturamento',
      modalArquivos_IdFaturamento: 0,

      modalMedicaoEdicao_Exibir: false,
      modalMedicaoEdicao_Titulo: 'Editar medição',
      modalMedicaoEdicao_ValorPrevistoAjustado: 0.0,
      modalMedicaoEdicao_DataPrevista: null,
      modalMedicaoEdicao_DataRecebimentoPrevista: null,
      modalMedicaoEdicao_Medicao: null,

      upload_Files: [],
      upload_FilesModal: [],

      faturamentos: [],
      medicoes: [],
      somaAFaturar: 0.0,
      somaFaturadoRecebido: 0.0,
      somaFaturadoAReceber: 0.0,
      somaFaturadoAReceberLiquido: 0.0,
      saldoAFaturar: 0.0,
      sinalRecebido: 0.0,
      sinalDescontado: 0.0,
      saldoSinal: 0.0,
      valorNaoComissionado: 0.0,
      valorNaoComissionadoUtilizado: 0.0,
      saldoValorNaoComissionado: 0.0,

      filtro_ObraSelecionada: null,
      filtro_Obras: [],
      filtro_ClienteSelecionado: null,
      filtro_Clientes: [],
      filtro_NumeroNF: '',
      filtro_DataFaturamentoInicial: null,
      filtro_DataFaturamentoFinal: null,
      filtro_DataRecebimentoInicial: null,
      filtro_DataRecebimentoFinal: null,
      filtro_Observacao: '',
      filtro_StatusFaturamento: [{ id: 0, descricao: 'Todos' }, { id: -1, descricao: 'A Faturar' }, { id: 1, descricao: 'Faturado Recebido' }, { id: 2, descricao: 'Faturado a Receber' }, { id: 3, descricao: 'Cancelado' }],
      filtro_StatusFaturamentoSelecionado: { id: 0, descricao: 'Todos' },

      fields: [
        { key: 'obra', label: 'OBRA' },
        { key: 'cliente', label: 'CLIENTE' },
        { key: 'numeroNota', label: 'NF' },
        { key: 'valorBruto', label: 'VAL. BRUTO' },
        { key: 'valorINSS', label: 'INSS' },
        { key: 'valorISS', label: 'ISS' },
        { key: 'valorIR', label: 'IR' },
        { key: 'valorArt30', label: 'ART. 30' },
        { key: 'desconto', label: 'DESCONTO' },
        { key: 'valorMaterial', label: 'MATERIAL'},
        { key: 'sinal', label: 'SINAL' },
        { key: 'naoComissionado', label: 'NÃO COMISSIONADO' },
        { key: 'valorLiquido', label: 'VAL. LIQUIDO' },
        { key: 'dataFaturamentoPrevisto', label: 'DT. RECEB. PREV.' },
        { key: 'dataFaturamento', label: 'Dt. Fat. Real' },
        { key: 'dataRecebimento', label: 'Dt. Recebimento' },
        { key: 'status', label: 'STATUS' },
        { key: 'acoes', label: 'ACOES' },
      ],

      columns: ["obra", "cliente", "numeroNota", "valorBruto", "valorINSS", "valorISS", "valorIR", "valorArt30", "desconto", "valorLiquido", "dataFaturamentoPrevisto", "dataFaturamento", "dataRecebimento", "observacao", "status", "acoes"],

      options: {
        perPage: 10,
        headings: {
          obra: "Obra",
          cliente: "Cliente",
          numeroNota: "NF",
          valorBruto: "Val. Bruto",
          valorINSS: "INSS",
          valorISS: "ISS",
          valorIR: "IR",
          valorArt30: "Art. 30",
          valorLiquido: "Val. Liquido",
          desconto: "Desconto",
          dataFaturamentoPrevisto: "DT. RECEB. PREV.",
          dataFaturamento: "Dt. Fat. Real.",
          dataRecebimento: "Dt. Recebimento",
          observacao: "Obs.",
          acoes: "Ações", //Informar recebimento, cancelamento fatura
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

      colunasMedicoesEdicao: [
        'dataPrevista',
        'dataPrevistaRecebimento',
        'valorPrevisto',
        'valorPrevistoAjustado',
        'valorFaturado',
        'acoes'
      ],

      opcoesMedicoesEdicao: {
        perPage: 1000,
        filterable: false,
        headings: {
          dataPrevista: 'Dt. Prevista Faturamento',
          dataPrevistaRecebimento: 'Dt. Prevista Recebimento',
          valorPrevisto: 'Previsto (R$)',
          valorPrevistoAjustado: 'Previsto Ajustado (R$)',
          valorFaturado: 'Faturado (R$)',
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
  },
  methods: {
    // ============================================================
    // Ações das tabelas (usadas pelo <ModalAcoes>).
    // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
    // ============================================================
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
          onClick: () => this.$swal("", row.observacao, "info"),
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
            this.modalArquivos_Arquivos = row.arquivos;
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

    formataData: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
    },

    formataDataSemHora: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY");
    },

    onChange() {
      this.upload_Files = [...this.$refs.file.files];
    },

    formatBRL(v) {
      return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(Number(v || 0));
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
          
          this.obtemValoresAFaturar();
        } else {
          this.$swal("", result.data, "error");

        }
      });
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

    recalcularValorLiquidoItemAFaturar(item) {
      const bruto = Number(item.valor || 0);
      const descontos = Number(item.inss || 0) + Number(item.iss || 0) + Number(item.ir || 0) + Number(item.arT30 || 0);
      item.valorLiquido = bruto - descontos;
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

    criarNovoItemAjusteManualAFaturar(data, valor) {
      const base = this.normalizarItemAjusteManualAFaturar(this.modalAjusteManualAFaturar_ObjetoOriginal || {});
      return {
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
          this.obtemValoresAFaturar();
        }
      });
    },

    onChangeModal() {
      this.upload_FilesModal = [...this.$refs.fileModal.files];

      this.isLoading = true;

      const formData = new FormData();
      this.upload_FilesModal.forEach((file) => {
        formData.append("arquivo", file);
      });

      ApiService.uploadFile(this.modalArquivos_Controller, formData, this.modalArquivos_IdFaturamento, (result) => {
        if (result.status != 200) {
          this.isLoading = false;
          this.$swal("", result.message, "error");
        } else {
          this.upload_Files = [];
          this.obtemArquivos(this.modalArquivos_IdFaturamento);
          this.$swal("Arquivo enviado com sucesso", result.message, "success");
        }
      });
    },

    obtemValoresAFaturar(){
      const objParametros = {
                    idObra: this.filtro_ObraSelecionada != null ? this.filtro_ObraSelecionada.id : null,
                    idCliente: this.filtro_ClienteSelecionado != null ? this.filtro_ClienteSelecionado.id : null,
                    idStatusFaturamento: 0,
                    numeroNotaFiscal: this.filtro_NumeroNF,
                    observacao: null,
                    dataFaturamentoInicial: this.filtro_DataFaturamentoInicial,
                    dataFaturamentoFinal: this.filtro_DataFaturamentoFinal,
                    dataRecebimentoInicial: null,
                    dataRecebimentoFinal: null,
                };
      ApiService.obtemValoresAFaturar(objParametros, (result) => {

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {

          this.medicoes = result.data;
          console.log(this.medicoes);
        }
      });
    },

    alteraValorFaturamento(id, valor, campo) {
      this.isLoading = true;

      ApiService.alteraValorFaturamento(id, valor, campo, (result) => {

        if (result.status != 200) {
          this.isLoading = false;
          this.$swal("", result.message, "error");
        } else {
          this.listaFaturamentos();
        }
      });
    },

    alterarDataFaturamento(id, valor, tipoData) {
      this.isLoading = true;

      ApiService.alterarDataFaturamento(id, valor, tipoData, (result) => {

        if (result.status != 200) {
          this.isLoading = false;
          this.$swal("", result.message, "error");
        } else {

          this.listaFaturamentos();
        }
      });
    },

    total(campo) {
      return this.faturamentos.reduce((soma, item) => {
        const valor = Number(item[campo]) || 0
        return soma + valor
      }, 0)
    },

    formatCurrency(valor) {
      return new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(valor)
    },

    calculaDataRecebimento() {
      this.modalMedicao_DataPrevistaRecebimento = new Date(new Date(this.modalMedicao_DataPrevista).setDate(new Date(this.modalMedicao_DataPrevista).getDate() + this.filtro_ObraSelecionada.diasDePagamento));
    },

    obtemSomatoriaMedicoes() {

      var result = 0;
      this.filtro_ObraSelecionada.medicoes.forEach(x => {
        result += x.valorPrevistoAjustado;
      });

      this.somatoriaValoresAjustadosMedicoes = result;

      return result;
    },

    salvarMedicoes() {

      if (this.obtemSomatoriaMedicoes() != this.obra_ValorTotalAjustado) {
        this.$swal('A soma dos valores ajustados é diferente do valor total ajustado informado', '', 'error');
        return;
      }

      this.isLoading = true;

      ApiService.post('ObraMedicao', this.filtro_ObraSelecionada.medicoes, (result) => {

        this.isLoading = false;

        if (result.status != 200) {
          this.$swal('Erro ao salvar medições', result.message, 'error');
        } else {
          this.edicaoValorTotalMedicao = false
        }
      });
    },

    cadastrarMedicaoExtra() {
      if (this.modalMedicao_DataPrevista == null) {
        this.$swal("Data da medição inválida", "", "error");
        return;
      }

      if (this.modalMedicao_ValorPrevistoAjustado <= 0) {
        this.$swal("Valor da medição inválido", "", "error");
        return;
      }

      var objetoMedicao = {
        id: 0,
        idInterno: this.filtro_ObraSelecionada.medicoes.length,
        idObra: this.filtro_ObraSelecionada.id,
        dataPrevista: this.modalMedicao_DataPrevista,
        dataPrevistaRecebimento: this.modalMedicao_DataPrevistaRecebimento,
        valorPrevistoAjustado: parseFloat(this.modalMedicao_ValorPrevistoAjustado),
        valorFaturado: 0
      };

      this.filtro_ObraSelecionada.medicoes.push(objetoMedicao);

      this.obtemSomatoriaMedicoes();

      this.modalMedicao_Exibir = false;
    },

    excluiMedicao(medicao) {

      var listaMedicoes = [];

      this.filtro_ObraSelecionada.medicoes.forEach(x => {
        if (x.idInterno != medicao.idInterno) {
          x.idInterno = listaMedicoes.length;
          listaMedicoes.push(x);
        }
      });

      this.filtro_ObraSelecionada.medicoes = listaMedicoes;

      this.obtemSomatoriaMedicoes();
    },

    abreModalEdicaoMedicao(medicao) {
      this.modalMedicaoEdicao_Medicao = medicao;
      this.modalMedicaoEdicao_DataPrevista = medicao.dataPrevista;
      this.modalMedicaoEdicao_DataRecebimentoPrevista = medicao.dataPrevistaRecebimento;
      this.modalMedicaoEdicao_ValorPrevistoAjustado = medicao.valorPrevistoAjustado;
      this.modalMedicaoEdicao_Titulo = 'Editar Medição ' + this.formataDataSemHora(medicao.dataPrevista);
      this.modalMedicaoEdicao_Exibir = true;
    },

    editarMedicao() {

      if (this.modalMedicaoEdicao_ValorPrevistoAjustado < 0) {
        validado = false;
        this.$swal("Valor da medição inválido", "", "error");
      }

      this.modalMedicaoEdicao_Medicao.valorPrevistoAjustado = this.modalMedicaoEdicao_ValorPrevistoAjustado;


      this.filtro_ObraSelecionada.medicoes.forEach(x => {
        if (x.id == this.modalMedicaoEdicao_Medicao.id) {
          x.valorPrevistoAjustado = this.modalMedicaoEdicao_Medicao.valorPrevistoAjustado;
          x.dataPrevistaRecebimento = this.modalMedicaoEdicao_DataRecebimentoPrevista;

        }
      });

      this.modalMedicaoEdicao_Exibir = false;

      this.obtemSomatoriaMedicoes();

      // this.isLoading = true;

      // ApiService.put('ObraMedicao', this.modalMedicaoEdicao_Medicao, (result) => {
      //   this.isLoading = false;

      //   if (result.status != 201) {
      //     this.$swal('Erro ao editar medição', result.message, 'error');
      //   } else {
      //     this.modalMedicaoEdicao_Exibir = false;
      //     this.obtemObra();

      //   }
      // });
    },

    calculaImpostos() {
      console.log('oioio');
    },

    removerArquivos() {
      this.upload_Files = [];
    },

    limpaFiltros() {
      this.filtro_ObraSelecionada = null;
      this.filtro_ClienteSelecionado = null;
      this.filtro_NumeroNF = '';
      this.filtro_DataFaturamentoInicial = null;
      this.filtro_DataFaturamentoFinal = null;
      this.filtro_DataRecebimentoInicial = null;
      this.filtro_DataRecebimentoFinal = null;
      this.filtro_Observacao = '';
      this.filtro_StatusFaturamentoSelecionado = { id: 0, descricao: 'Todos' };
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
          this.obtemArquivos(this.modalArquivos_IdFaturamento);
          this.$swal("Arquivo excluído com sucesso", result.message, "success");
        }
      });
    },

    exibirModalDetalheCliente() {

      if (this.filtro_ClienteSelecionado == null) {
        this.$swal("Selecione um cliente", '', "error");
        return;
      }

      ApiService.obtemDetalheClienteFaturamento(this.filtro_ClienteSelecionado.id, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        }
        else {
          this.filtro_ClienteSelecionado.detalhesObras = result.data;

          console.log(this.filtro_ClienteSelecionado);

          this.modalDetalheCliente_Titulo = 'Detalhe do cliente ' + this.filtro_ClienteSelecionado.nomeFantasia;
          this.modalDetalheCliente_Exibir = true;
        }
      });


    },

    exibirModalDetalheObra() {

      if (this.filtro_ObraSelecionada == null) {
        this.$swal("Selecione uma obra", '', "error");
        return;
      }

      ApiService.obtemFaturamentoObra(this.filtro_ObraSelecionada.id, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        }
        else {
          this.filtro_ObraSelecionada.faturamentos = result.data;

          // this.modalDetalheObra_Titulo = 'Detalhe da obra ' + this.filtro_ObraSelecionada.codigo;
          // this.filtro_ObraSelecionada.valorAFaturar = 0.0;

          // var count = 0;

          // this.filtro_ObraSelecionada.medicoes.forEach(x => {
          //   this.filtro_ObraSelecionada.valorAFaturar = this.filtro_ObraSelecionada.valorAFaturar + x.valorPrevisto;
          //   x.idInterno = count;
          //   count++;
          // });

          // this.filtro_ObraSelecionada.faturamentos.forEach(x => {
          //   this.filtro_ObraSelecionada.valorAFaturar = this.filtro_ObraSelecionada.valorAFaturar - x.valorBruto;
          // });

          // this.obra_ValorTotalAjustado = this.filtro_ObraSelecionada.valorTotalAjustado;

          this.modalDetalheObra_Exibir = true;
        }
      });
    },

    obraSelecionada() {
      if (this.modalFaturamento_ObraSelecionada != null) {
        this.modalFaturamento_ValorINSS = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoINSS;
        this.modalFaturamento_ValorISS = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoISS;
        this.modalFaturamento_ValorIR = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoIR;
        this.modalFaturamento_ValorArt30 = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoArt30;
        this.modalFaturamento_Material = this.modalFaturamento_ObraSelecionada.valorMaterial;
      }
    },

    abrirCancelamentoFaturamento: function (faturamentoDTO) {
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

    cancelarFaturamento: function (faturamentoDTO, dataRetorno) {
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
          this.listaFaturamentos();
        }
      });

    },

    reverterRecebimento: function (faturamentoDTO) {
      this.isLoading = true;

      faturamentoDTO.dataRecebimentoRealizado = null;
      faturamentoDTO.idStatusFaturamento = 2;

      ApiService.put('Faturamento', faturamentoDTO, (result) => {

        this.isLoading = false;

        if (result.status != 200) {

          this.$swal("Erro ao editar faturamento", result.message, "error");
        } else {
          this.$swal("Faturamento revertido", '', "success");
          this.listaFaturamentos();
        }
      });

    },

    abrirModalDataFaturamento(faturamentoDTO) {
      this.modalDataFaturamento_Faturamento = faturamentoDTO;
      this.modalDataFaturamento_DataFaturamento = null;
      this.modalDataFaturamento_ValorRecebido = 0;
      this.modalDataFaturamento_DataProximoRecebimento = null;
      this.modalDataFaturamento_TipoTotal = true;
      this.modalDataFaturamento_Exibir = true;

      console.log(faturamentoDTO);
    },

    informarRecebimento: function () {

      if (this.modalDataFaturamento_DataFaturamento == null) {
        this.$swal("Data de recebimento inválida", '', "error");
        return;
      }

      if (this.modalDataFaturamento_TipoTotal) {
        // Recebimento total: usa o valor líquido completo, sem próximo recebimento
        var objetoRecebimento = {
          idFaturamento: this.modalDataFaturamento_Faturamento.id,
          dataRecebimento: this.modalDataFaturamento_DataFaturamento,
          valorRecebido: this.modalDataFaturamento_Faturamento.valorLiquido,
          valorRestante: 0,
          dataProximoRecebimento: null
        };
      } else {
        // Recebimento parcial: valida campos adicionais
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

        var objetoRecebimento = {
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
          this.listaFaturamentos();
        }
      });

    },

    obtemArquivos: function (idFaturamento) {
      ApiService.getArquivos(this.modalArquivos_Controller, idFaturamento, (result) => {
        this.isLoading = false;

        if (result.status == 200) {
          this.modalArquivos_Arquivos = result.data;
        }
      });
    },

    funcaoValorBruto: function () {
      if (this.modalFaturamento_ObraSelecionada != null && this.modalFaturamento_ValorBruto > 0) {
        this.modalFaturamento_ValorDescontoSinal = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.percentualEquivalenteSinal;

        this.modalFaturamento_ValorINSS = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoINSS;
        this.modalFaturamento_ValorISS = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoISS;
        this.modalFaturamento_ValorIR = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoIR;
        this.modalFaturamento_ValorArt30 = (this.modalFaturamento_ValorBruto / 100) * this.modalFaturamento_ObraSelecionada.aliquotaImpostoArt30;
      }

      this.calculaValorLiquido();
    },

    calculaValorLiquido: function () {
      this.modalFaturamento_ValorLiquido = this.modalFaturamento_ValorBruto - this.modalFaturamento_ValorINSS - this.modalFaturamento_ValorISS - this.modalFaturamento_ValorIR - this.modalFaturamento_ValorArt30 - this.modalFaturamento_ValorDesconto;
      this.modalFaturamento_ValorLiquidoSinal = this.modalFaturamento_ValorBruto - this.modalFaturamento_ValorINSS - this.modalFaturamento_ValorISS - this.modalFaturamento_ValorIR - this.modalFaturamento_ValorArt30 - this.modalFaturamento_ValorDesconto - this.modalFaturamento_ValorDescontoSinal;
    },

    listaObrasNovoFaturamento: function () {
      this.modalFaturamento_Obras = [];

      ApiService.getAll("Obra", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalFaturamento_Obras = result.data.filter(x => x.cancelada === false);
        }
      });
    },

    listaObrasFiltro: function () {
      this.filtro_Obras = [];

      ApiService.getAll("Obra", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.filtro_Obras = result.data.filter(x => x.cancelada === false);
        }
      });
    },

    listaClientes: function () {
      this.filtro_Clientes = [];

      ApiService.getAll("Cliente", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.filtro_Clientes = result.data;
        }
      });
    },

    listaFaturamentos: function () {
      this.isLoading = true;

      var objParametros = {
        idObra: this.filtro_ObraSelecionada != null ? this.filtro_ObraSelecionada.id : null,
        idCliente: this.filtro_ClienteSelecionado != null ? this.filtro_ClienteSelecionado.id : null,
        idStatusFaturamento: this.filtro_StatusFaturamentoSelecionado != null ? this.filtro_StatusFaturamentoSelecionado.id : 0,
        numeroNotaFiscal: this.filtro_NumeroNF,
        observacao: this.filtro_Observacao,
        dataFaturamentoInicial: this.filtro_DataFaturamentoInicial,
        dataFaturamentoFinal: this.filtro_DataFaturamentoFinal,
        dataRecebimentoInicial: this.filtro_DataRecebimentoInicial,
        dataRecebimentoFinal: this.filtro_DataRecebimentoFinal,
      };

      ApiService.obtemFaturamentos(objParametros, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {

          this.faturamentos = result.data.faturamentosDTO;
          this.somaAFaturar = result.data.somaAFaturar;
          this.somaFaturadoRecebido = result.data.somaFaturadoRecebido;
          this.somaFaturadoAReceber = result.data.somaFaturadoAReceber;
          this.somaFaturadoAReceberLiquido = result.data.somaFaturadoAReceberLiquido;
          this.saldoAFaturar = this.somaAFaturar - this.somaFaturadoAReceber - this.somaFaturadoRecebido;
          this.sinalRecebido = result.data.sinalRecebido;
          this.sinalDescontado = result.data.sinalDescontado;
          this.saldoSinal = result.data.saldoSinal;

          this.valorNaoComissionado = result.data.valorNaoComissionado;
          this.valorNaoComissionadoUtilizado = result.data.valorNaoComissionadoUtilizado;
          this.saldoValorNaoComissionado = result.data.saldoValorNaoComissionado;

          if (objParametros.idStatusFaturamento == -1) {
            this.somaFaturadoRecebido = 0;
            this.somaFaturadoAReceber = 0;
            this.somaFaturadoAReceberLiquido = 0;
          }
        }
      });

      
      this.obtemValoresAFaturar(objParametros);

    },

    editarFaturamento: function () {

      this.isLoading = true;

      this.modalFaturamento_Faturamento.numeroNF = this.modalFaturamento_NumeroNF;
      this.modalFaturamento_Faturamento.dataFaturamento = this.modalFaturamento_DataFaturamento;
      this.modalFaturamento_Faturamento.dataRecebimentoPrevisto = this.modalFaturamento_DataRecebimento;
      this.modalFaturamento_Faturamento.observacao = this.modalFaturamento_Observacao;
      this.modalFaturamento_Faturamento.valorBruto = this.modalFaturamento_ValorBruto;
      this.modalFaturamento_Faturamento.valorINSS = this.modalFaturamento_ValorINSS;
      this.modalFaturamento_Faturamento.valorISS = this.modalFaturamento_ValorISS;
      this.modalFaturamento_Faturamento.valorIR = this.modalFaturamento_ValorIR;
      this.modalFaturamento_Faturamento.valorArt30 = this.modalFaturamento_ValorArt30;
      this.modalFaturamento_Faturamento.valorDesconto = this.modalFaturamento_ValorDesconto;
      this.modalFaturamento_Faturamento.valorLiquido = this.modalFaturamento_ValorLiquidoSinal;


      ApiService.put('Faturamento', this.modalFaturamento_Faturamento, (result) => {
        this.isLoading = false;

        if (result.status != 200) {

          this.$swal("Erro ao editar faturamento", result.message, "error");
        } else {
          this.$swal("Faturamento editado com sucesso", '', "success");

          this.modalFaturamento_DataFaturamento = null;
          this.modalFaturamento_DataRecebimento = null;
          this.modalFaturamento_NumeroNF = '';
          this.modalFaturamento_ObraSelecionada = null;
          this.modalFaturamento_Observacao = '';
          this.modalFaturamento_ValorArt30 = 0.0;
          this.modalFaturamento_ValorBruto = 0.0;
          this.modalFaturamento_ValorDesconto = 0.0;
          this.modalFaturamento_ValorINSS = 0.0;
          this.modalFaturamento_ValorIR = 0.0;
          this.modalFaturamento_ValorISS = 0.0;
          this.modalFaturamento_ValorLiquido = 0.0;
          this.modalFaturamento_Exibir = false;
        }
      });
    },

    enviarFaturamento: function () {
      if (this.modalFaturamento_ObraSelecionada == null) {
        this.$swal("", "Obra inválida", "error");
        return;
      }

      if (this.modalFaturamento_ValorLiquido <= 0) {
        this.$swal("", "Valor líquido inválido", "error");
        return;
      }

      if (this.modalFaturamento_DataFaturamento == null) {
        this.$swal("", "Data de faturamento inválida ", "error");
        return;
      }

      if (this.modalFaturamento_DataRecebimento == null) {
        this.$swal("", "Data de recebimento inválida ", "error");
        return;
      }

      if (this.modalFaturamento_DataRecebimento < this.modalFaturamento_DataFaturamento) {
        this.$swal("", "Data de recebimento não pode ser menor a data de faturamento ", "error");
        return;
      }

      if (this.modalFaturamento_NumeroNF == null) {
        this.$swal("", "Número da nota fiscal inválido", "error");
        return;
      }

      // Monta os dados da obra para exibir na confirmação
      var obra = this.modalFaturamento_ObraSelecionada;
      var nomeCliente = obra.cliente
        ? (obra.cliente.nomeFantasia || obra.cliente.razaoSocial || '-')
        : '-';
      var nomeCidade = obra.cidade ? obra.cidade.nome : '-';
      var valorFaturamentoFormatado = Number(this.modalFaturamento_ValorLiquidoSinal || 0)
        .toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

      var htmlConfirmacao =
        '<div style="text-align:left; font-size:14px; line-height:1.6;">' +
        '<p>Confirme se o faturamento está sendo lançado na obra correta:</p>' +
        '<hr style="margin:8px 0;">' +
        '<p><b>Código:</b> ' + (obra.codigo || '-') + '</p>' +
        '<p><b>Descrição:</b> ' + (obra.descricao || '-') + '</p>' +
        '<p><b>Cliente:</b> ' + nomeCliente + '</p>' +
        '<p><b>Endereço:</b> ' + (obra.enderecoObra || '-') + '</p>' +
        '<p><b>Cidade:</b> ' + nomeCidade + '</p>' +
        '<hr style="margin:8px 0;">' +
        '<p><b>Valor do faturamento:</b> ' + valorFaturamentoFormatado + '</p>' +
        '</div>';

      this.$swal({
        title: 'Confirmar obra do faturamento',
        html: htmlConfirmacao,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sim, confirmar',
        cancelButtonText: 'Cancelar',
        reverseButtons: true,
        focusCancel: true
      }).then((resultConfirmacao) => {
        if (!resultConfirmacao.isConfirmed) {
          return;
        }

        // Após confirmar a obra, verifica saldo disponível a faturar
        this.isLoading = true;
        ApiService.obtemValoresAFaturarDaObra(obra.id, (resultSaldo) => {
          this.isLoading = false;

          var valoresAFaturar = [];
          if (resultSaldo && Array.isArray(resultSaldo.data)) {
            valoresAFaturar = resultSaldo.data;
          } else if (Array.isArray(resultSaldo)) {
            valoresAFaturar = resultSaldo;
          }

          var somaValorLiquidoDisponivel = valoresAFaturar.reduce(function (acc, item) {
            return acc + (Number(item.valorLiquido) || 0);
          }, 0);

          var valorLiquidoInformado = Number(this.modalFaturamento_ValorLiquidoSinal) || 0;

          if (somaValorLiquidoDisponivel >= valorLiquidoInformado) {
            // Há saldo suficiente, segue direto
            this.submeterFaturamento();
            return;
          }

          // Saldo insuficiente: pergunta ao usuário se deseja faturar mesmo assim
          var saldoFormatado = somaValorLiquidoDisponivel
            .toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
          var informadoFormatado = valorLiquidoInformado
            .toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

          var htmlSaldo =
            '<div style="text-align:left; font-size:14px; line-height:1.6;">' +
            '<p>O valor informado para o faturamento é <b>maior</b> que o saldo disponível a faturar desta obra.</p>' +
            '<hr style="margin:8px 0;">' +
            '<p><b>Saldo disponível (líquido):</b> ' + saldoFormatado + '</p>' +
            '<p><b>Valor informado (líquido):</b> ' + informadoFormatado + '</p>' +
            '<hr style="margin:8px 0;">' +
            '<p>Deseja realmente prosseguir com o faturamento?</p>' +
            '</div>';

          this.$swal({
            title: 'Saldo insuficiente',
            html: htmlSaldo,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sim, faturar',
            cancelButtonText: 'Não',
            reverseButtons: true,
            focusCancel: true
          }).then((resultSaldoConfirm) => {
            if (resultSaldoConfirm.isConfirmed) {
              this.submeterFaturamento();
            }
          });
        });
      });
    },

submeterFaturamento: function () {
  var objetoFaturamento = {
    id: 0,
    idObra: this.modalFaturamento_ObraSelecionada.id,
    idStatusFaturamento: 2,
    numeroNF: this.modalFaturamento_NumeroNF,
    dataFaturamento: this.modalFaturamento_DataFaturamento,
    dataRecebimentoPrevisto: this.modalFaturamento_DataRecebimento,
    observacao: this.modalFaturamento_Observacao,
    valorBruto: this.modalFaturamento_ValorBruto,
    valorINSS: this.modalFaturamento_ValorINSS,
    valorISS: this.modalFaturamento_ValorISS,
    valorIR: this.modalFaturamento_ValorIR,
    valorArt30: this.modalFaturamento_ValorArt30,
    valorDesconto: this.modalFaturamento_ValorDesconto,
    valorMaterial: this.modalFaturamento_Material,
    valorLiquido: this.modalFaturamento_ValorLiquidoSinal,
    valorSinal: this.modalFaturamento_ValorDescontoSinal,
    valorNaoComissionado: this.modalFaturamento_ValorNaoComissionado
  };
  this.isLoading = true;

  ApiService.post('Faturamento', objetoFaturamento, (result) => {
    if (result.status != 201) {
      this.isLoading = false;
      this.$swal("Erro ao cadastrar faturamento", result.message, "error");
    } else {
      this.upload_Files.forEach((file) => {
        const formData = new FormData();
        formData.append("arquivo", file);
        ApiService.uploadFile('Faturamento', formData, result.data.id, () => { });
      });

      this.$swal("Faturamento cadastrado com sucesso", result.message, "success");

      this.removerArquivos();
      this.modalFaturamento_DataFaturamento = null;
      this.modalFaturamento_DataRecebimento = null;
      this.modalFaturamento_NumeroNF = '';
      this.modalFaturamento_ObraSelecionada = null;
      this.modalFaturamento_Observacao = '';
      this.modalFaturamento_ValorArt30 = 0.0;
      this.modalFaturamento_ValorBruto = 0.0;
      this.modalFaturamento_ValorDesconto = 0.0;
      this.modalFaturamento_ValorINSS = 0.0;
      this.modalFaturamento_ValorIR = 0.0;
      this.modalFaturamento_ValorISS = 0.0;
      this.modalFaturamento_ValorLiquido = 0.0;
      this.modalFaturamento_Exibir = false;

      this.listaFaturamentos();
    }
  });
}, 
  
  
  },
  watch: {
    modalAjusteManualAFaturar_Lista: {
      handler() {
        this.sincronizarItensModalAjusteManualAFaturar();
      },
      deep: true,
    },
    modalFaturamento_ValorBruto()        { this.calculaValorLiquido(); },
    modalFaturamento_ValorINSS()         { this.calculaValorLiquido(); },
    modalFaturamento_ValorISS()          { this.calculaValorLiquido(); },
    modalFaturamento_ValorIR()           { this.calculaValorLiquido(); },
    modalFaturamento_ValorArt30()        { this.calculaValorLiquido(); },
    modalFaturamento_ValorDesconto()     { this.calculaValorLiquido(); },
    modalFaturamento_ValorDescontoSinal(){ this.calculaValorLiquido(); },
  },
  mounted() {

    this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO'));

    this.listaClientes();
    this.listaObrasFiltro();
    this.listaObrasNovoFaturamento();
    this.listaFaturamentos();
  },
};
</script>

<style src="./Faturamento.scss" lang="scss" />

<style scoped>
/* ====== MODAL INFORMAR RECEBIMENTO — toggle e confirmação ====== */
.irf-toggle-wrap {
  display: flex;
  align-items: center;
  gap: 14px;
}
.irf-toggle {
  position: relative;
  width: 52px;
  height: 28px;
  border-radius: 999px;
  background: rgba(255, 214, 57, 0.2);
  border: 1px solid rgba(255, 214, 57, 0.3);
  cursor: pointer;
  transition: background 0.22s, border-color 0.22s;
  flex-shrink: 0;
}
.irf-toggle--on {
  background: rgba(40, 167, 69, 0.25);
  border-color: rgba(101, 211, 126, 0.4);
}
.irf-toggle__thumb {
  position: absolute;
  top: 3px;
  left: 3px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: #ffd639;
  transition: transform 0.22s, background 0.22s;
  box-shadow: 0 2px 6px rgba(0,0,0,0.3);
}
.irf-toggle--on .irf-toggle__thumb {
  transform: translateX(24px);
  background: #65d37e;
}
.irf-toggle-label {
  color: rgba(255,255,255,0.4);
  font-size: 14px;
  font-weight: 600;
  transition: color 0.2s;
  user-select: none;
}
.irf-toggle-label--active {
  color: #fff;
}
.irf-total-confirm {
  display: flex;
  align-items: flex-start;
  gap: 14px;
  margin-top: 16px;
  padding: 16px;
  border-radius: 14px;
  background: rgba(40, 167, 69, 0.1);
  border: 1px solid rgba(101, 211, 126, 0.2);
  color: rgba(255,255,255,0.85);
}
.irf-total-confirm__icon {
  font-size: 24px;
  color: #65d37e;
  margin-top: 2px;
  flex-shrink: 0;
}
.irf-total-confirm p {
  margin: 4px 0 0;
  color: rgba(255,255,255,0.65);
  font-size: 13px;
}
.irf-alert {
  display: flex;
  align-items: flex-start;
  gap: 10px;
  margin-top: 16px;
  padding: 13px 16px;
  border-radius: 12px;
  font-size: 13px;
  line-height: 1.5;
}
.irf-alert i {
  font-size: 16px;
  flex-shrink: 0;
  margin-top: 1px;
}
.irf-alert--warn {
  background: rgba(255, 214, 57, 0.1);
  border: 1px solid rgba(255, 214, 57, 0.2);
  color: #ffd639;
}
.irf-alert--error {
  background: rgba(220, 53, 69, 0.1);
  border: 1px solid rgba(220, 53, 69, 0.25);
  color: #f07080;
}
.irf-alert--info {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: rgba(255, 255, 255, 0.8);
}
/* ====== / MODAL INFORMAR RECEBIMENTO ====== */

/* ====== MODAL FATURAMENTO — estilos novos ====== */
.fat-row {
  margin-bottom: 16px;
}
.fat-summary-2col {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}
.fat-input {
  color: #fff;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 12px;
  min-height: 42px;
}
.fat-input:focus {
  background: rgba(255, 255, 255, 0.08);
  border-color: rgba(255, 255, 255, 0.28);
  color: #fff;
  box-shadow: none;
}
.fat-textarea {
  color: #fff !important;
  background: rgba(255, 255, 255, 0.05) !important;
  border: 1px solid rgba(255, 255, 255, 0.12) !important;
  border-radius: 12px;
  resize: none;
}
.fat-textarea:focus {
  background: rgba(255, 255, 255, 0.08) !important;
  border-color: rgba(255, 255, 255, 0.28) !important;
  box-shadow: none !important;
}
.fat-file-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-bottom: 14px;
}
.fat-file-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 14px;
  border-radius: 10px;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: rgba(255, 255, 255, 0.85);
  font-size: 13px;
}
.fat-file-icon {
  color: #ffd639;
  font-size: 16px;
}
.fat-file-actions {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 12px;
}
.fat-choose-btn {
  min-height: 40px;
  font-size: 13px;
  width: auto;
  padding: 0 20px;
}
.fat-remove-files-btn {
  min-height: 40px;
  font-size: 13px;
}
/* ====== / MODAL FATURAMENTO ====== */

/* ====== UTILITÁRIOS ====== */
.mono {
  font-family: 'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, monospace;
}
.text-ok  { color: #65d37e !important; }
.text-warn { color: #ffd639 !important; }
.text-info { color: #63b3ed !important; }

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


/* ====== FILTROS DA PÁGINA FATURAMENTO (responsivo) ====== */
.fat-filter-widget .fat-filter-row {
  margin-bottom: 14px;
}
.fat-filter-widget .fat-filter-row:last-child {
  margin-bottom: 0;
}
.fat-filter-col {
  margin-bottom: 12px;
}
.fat-filter-label {
  display: block;
  margin-bottom: 6px;
  color: #fff;
  font-weight: 500;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.fat-input-with-action {
  display: flex;
  align-items: stretch;
  gap: 8px;
  width: 100%;
}
.fat-input-with-action__field {
  flex: 1 1 auto;
  min-width: 0;
}
.fat-info-btn {
  flex: 0 0 auto;
  min-width: 42px;
  height: 40px;
  padding: 0 12px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  margin: 0;
}
.fat-datepicker,
.fat-datepicker .mx-input,
.fat-datepicker .mx-datepicker {
  width: 100% !important;
}
.fat-filter-actions {
  margin-top: 4px;
}
.fat-filter-buttons {
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-end;
  gap: 12px;
}
.fat-filter-buttons .btn {
  margin: 0;
}

/* Botão "Entrada Faturamento" à direita */
.fat-entrada-row {
  justify-content: flex-end;
}
.fat-entrada-col {
  display: flex;
  justify-content: flex-end;
}

/* Ajustes para telas menores */
@media (max-width: 1199.98px) {
  .fat-filter-label {
    font-size: 13px;
  }
}
@media (max-width: 767.98px) {
  .fat-filter-buttons {
    justify-content: stretch;
  }
  .fat-filter-buttons .btn {
    flex: 1 1 auto;
    width: auto;
  }
  .fat-entrada-col {
    justify-content: stretch;
  }
  .fat-entrada-col .btn {
    width: 100%;
  }
  .fat-summary-widget label {
    font-size: 13px;
    white-space: normal;
  }
}
</style>


//No caso de não relização das previsões, pegar o saldo restante, obter a média de dias entre os faturamentos e criar um
novo faturamento. Qdo houver apenas 1 parcela, colocar para 15 dias.