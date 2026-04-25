<template>
  <div class="obras-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <div class="page-header">
      <div>
        <h1 class="page-title">Obras</h1>
        <p class="page-subtitle">Cadastre novas obras com mais clareza visual, agrupamento de informações e feedback imediato dos principais dados.</p>
      </div>
    </div>

    <Widget customHeader class="estiloWidget obras-widget">
      <div class="top-actions-bar">
        <div class="top-actions-bar__info">
          <span class="top-actions-bar__badge">Gestão de obras</span>
          <span class="top-actions-bar__text">Use os atalhos abaixo para cadastrar, exportar e acompanhar obras com mais rapidez.</span>
        </div>
        <div class="top-actions-bar__buttons">
          <button
            v-if="usuarioDTO != null && usuarioDTO.obraCadastrar"
            @click="
              modal_Titulo = 'Cadastro de Obra';
              obra_Id = 0;
              obra_IdCliente = 0;
              obra_IdUsuarioAprovacao = 0;
              obra_IdCidade = 0;
              obra_Codigo = '';
              obra_CodigoSequencia = '';
              obra_CodigoAno = '';
              obra_CodigoProposta = '';
              obra_NumeroPedidoCliente = '';
              obra_Endereco = '';
              obra_EnderecoDeEntrega = '';
              obra_Descricao = '';
              obra_DataProposta = null;
              obra_DataInicio = null;
              obra_DataFim = null;
              obra_PrazoDias = 0;
              obra_ValorTotal = 0;
              obra_ValorCusto = 0.0;
              obra_ValorNaoComissionado = 0.0;
              obra_ValorMaterial = 0.0;
              obra_ValorSinal = 0.0;
              obra_Ativo = true;
              obra_AprovacaoAutomatica = false;
              obra_ClienteSelecionado = null;
              obra_EngenheiroSelecionado = [];
              obra_CidadeSelecionada = null;
              obra_Cep = '';
              obra_PercentualSinal = 0.0;
              obra_DataRecebimentoSinal = null;
              obra_AliquotaImpostoArt30 = 0.0;
              obra_AliquotaImpostoINSS = 0.0;
              obra_AliquotaImpostoIR = 0.0;
              obra_AliquotaImpostoISS = 0.0;
              obra_Medicoes = [];
              obra_ETOs = [];
              obra_DiretorAprovador = null;
              modalMedicao_Exibir = true;
              modalMedicao_DataPrevista = null;
              modalMedicao_DataPrevistaRecebimento = null;
              modalMedicao_ValorPrevisto = 0;
              modalMedicao_SaldoMedicao = 0;
              modalETO_Exibir = true;
              modalETO_DataPrevista = null;
              modalETO_ValorPrevisto = 0;
              modalETO_SaldoETO = 0;
              modal_ExibirBotaoCadastrar = true;
              modal_ExibirBotaoEditar = false;
              modal_InativaBotaoCadastrar = false;
              modal_InativaBotaoEditar = true;
              modal_Exibir = true;
            "
            v-b-modal.cadastro
            type="button"
            class="btn btn-primary btn-hero-action"
          >
            <span class="btn-hero-action__icon">＋</span>
            <span>Nova Obra</span>
          </button>
          <button type="button" @click="downloadResumoETOExcel()" class="btn btn-outline-success top-action-btn">
            Resumo ETO XLS
          </button>
          <button type="button" @click="downloadResumoETOPDF()" class="btn btn-outline-success top-action-btn">
            Resumo ETO PDF
          </button>
        </div>
      </div>

      <b-modal
        :no-close-on-backdrop="true"
        id="cadastro"
        class="modal-dialog modal-md cadastro-modal"
        v-bind:title="modal_Titulo"
        body-bg-variant="modal"
        header-bg-variant="bodyModal"
        footer-bg-variant="bodyModal"
        v-model="modal_Exibir"
        size="xl"
      >
        <div class="cadastro-modal__hero">
          <div>
            <span class="cadastro-modal__eyebrow">Cadastro guiado</span>
            <h3 class="cadastro-modal__title">Preencha a nova obra por blocos</h3>
            <p class="cadastro-modal__description">
              A organização abaixo prioriza os campos mais usados e reduz o retrabalho no momento do cadastro.
            </p>
          </div>
        </div>

        <div class="cadastro-steps">
          <div class="cadastro-step is-active"><span>1</span><small>Dados principais</small></div>
          <div class="cadastro-step is-active"><span>2</span><small>Financeiro</small></div>
          <div class="cadastro-step is-active"><span>3</span><small>Previsões</small></div>
        </div>

        <b-container fluid class="cadastro-sections">
          <section class="form-section-card">
            <div class="form-section-card__header">
              <div>
                <h4>Dados principais</h4>
                <p>Concentre aqui cliente, responsáveis, localização, identificação e cronograma da obra.</p>
              </div>
              <div class="prazo-highlight">
                <span>Prazo calculado</span>
                <strong>{{ obra_PrazoDias }} dias</strong>
              </div>
            </div>

            <b-row>
              <b-col md="8">
                <div class="form-field-group">
                  <label class="form-label">Cliente</label>
                  <multiselect
                    v-model="obra_ClienteSelecionado"
                    :multiple="false"
                    :options="clientes"
                    :custom-label="multiselectClientes"
                    select-label="Selecionar"
                    placeholder="Selecione um cliente"
                    :disabled="controle_ObraCadastrando"
                    @select="buscaCliente"
                  ></multiselect>
                  <small class="form-helper-text">Ao selecionar o cliente, endereço, CEP, cidade e prazo de pagamento podem ser preenchidos automaticamente.</small>
                </div>
              </b-col>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Dias de pagamento</label>
                  <b-form-input v-model="obra_DiasDePagamento" class="form-input-dark"></b-form-input>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Cidade</label>
                  <multiselect
                    v-model="obra_CidadeSelecionada"
                    :multiple="false"
                    :options="cidades"
                    :custom-label="multiselectCidades"
                    select-label="Selecionar"
                    placeholder="Selecione uma cidade"
                    label="nome"
                    track-by="nome"
                    :disabled="controle_ObraCadastrando"
                    @select="preencheAliquotaISS"
                  ></multiselect>
                </div>
              </b-col>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Aprovadores</label>
                  <multiselect
                    v-model="obra_DiretorAprovador"
                    :multiple="false"
                    :options="diretores"
                    :custom-label="multiselectDiretores"
                    select-label="Selecionar"
                    placeholder="Selecione um aprovador"
                    label="nome"
                    track-by="nome"
                    :disabled="controle_ObraCadastrando"
                  ></multiselect>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col>
                <div class="form-field-group">
                  <label class="form-label">Engenheiro responsável</label>
                  <multiselect
                    v-model="obra_EngenheiroSelecionado"
                    :multiple="true"
                    :options="engenheiros"
                    :custom-label="multiselectEngenheiros"
                    select-label="Selecionar"
                    placeholder="Selecione um engenheiro"
                    label="nome"
                    track-by="nome"
                    :disabled="controle_ObraCadastrando"
                  ></multiselect>
                </div>
              </b-col>
            </b-row>

            <div class="section-subtitle">Endereços e identificação</div>

            <b-row>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Código proposta</label>
                  <b-form-input v-model="obra_CodigoProposta" class="form-input-dark"></b-form-input>
                </div>
              </b-col>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Nº pedido cliente</label>
                  <b-form-input v-model="obra_NumeroPedidoCliente" class="form-input-dark"></b-form-input>
                </div>
              </b-col>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">CEP da obra</label>
                  <b-form-input v-model="obra_Cep" class="form-input-dark" v-mask="'#####-###'"></b-form-input>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Endereço da obra</label>
                  <b-form-input v-model="obra_EnderecoDeEntrega" class="form-input-dark"></b-form-input>
                </div>
              </b-col>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Endereço de faturamento</label>
                  <b-form-input v-model="obra_Endereco" class="form-input-dark"></b-form-input>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col>
                <div class="form-field-group">
                  <label class="form-label">Descrição</label>
                  <textarea-autosize class="form-control form-input-dark form-textarea-dark" :min-height="115" v-model="obra_Descricao" />
                </div>
              </b-col>
            </b-row>

            <div class="section-subtitle">Cronograma</div>

            <b-row>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Data proposta</label>
                  <div class="date-input-wrapper">
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="obra_DataProposta" format="dd/MM/yyyy" type="date" :open.sync="open" @change="calculaPrazoDias()"></DatePickerMask>
                  </div>
                </div>
              </b-col>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Data início obra</label>
                  <div class="date-input-wrapper">
                    <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="obra_DataInicio" format="dd/MM/yyyy" type="date" :open.sync="open" @change="calculaPrazoDias()"></DatePickerMask>
                  </div>
                </div>
              </b-col>
              <b-col md="4">
                <div class="form-field-group">
                  <label class="form-label">Data fim obra</label>
                  <div class="date-input-wrapper">
                    <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="obra_DataFim" format="dd/MM/yyyy" type="date" :open.sync="open" @change="calculaPrazoDias()"></DatePickerMask>
                  </div>
                </div>
              </b-col>
            </b-row>
          </section>

          <section class="form-section-card">
            <div class="form-section-card__header">
              <div>
                <h4>Valores e condições</h4>
                <p>Os campos financeiros ficam agrupados para reduzir erros de digitação e facilitar comparação.</p>
              </div>
            </div>

            <div class="financial-summary-grid">
              <div class="financial-summary-card">
                <span>Valor total</span>
                <strong>{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(obra_ValorTotal || 0) }}</strong>
              </div>
              <div class="financial-summary-card">
                <span>Valor custo</span>
                <strong>{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(obra_ValorCusto || 0) }}</strong>
              </div>
              <div class="financial-summary-card">
                <span>% do sinal</span>
                <strong>{{ this.obra_PercentualSinal.toFixed(2) }}%</strong>
              </div>
            </div>

            <b-row>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Valor total</label>
                  <Money v-model="obra_ValorTotal" v-bind="money" class="money-input-dark"></Money>
                </div>
              </b-col>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Valor custo</label>
                  <Money v-model="obra_ValorCusto" v-bind="money" class="money-input-dark"></Money>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Valor não comissionado</label>
                  <Money v-model="obra_ValorNaoComissionado" v-bind="money" class="money-input-dark"></Money>
                </div>
              </b-col>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Valor material</label>
                  <Money v-model="obra_ValorMaterial" v-bind="money" class="money-input-dark"></Money>
                </div>
              </b-col>
            </b-row>

            <b-row>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Valor sinal</label>
                  <Money v-model="obra_ValorSinal" v-bind="money" class="money-input-dark" @keydown.native.tab="calculaPercentualSinal"></Money>
                </div>
              </b-col>
              <b-col md="6">
                <div class="form-field-group">
                  <label class="form-label">Data recebimento sinal</label>
                  <div class="date-input-wrapper">
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="obra_DataRecebimentoSinal" format="dd/MM/yyyy" type="date" :open.sync="open"></DatePickerMask>
                  </div>
                </div>
              </b-col>
            </b-row>

            <div class="taxes-grid">
              <div class="tax-card">
                <label class="form-label">Alíquota ISS</label>
                <Money v-model="obra_AliquotaImpostoISS" v-bind="number" class="money-input-dark"></Money>
              </div>
              <div class="tax-card">
                <label class="form-label">Alíquota INSS</label>
                <Money v-model="obra_AliquotaImpostoINSS" v-bind="number" class="money-input-dark"></Money>
              </div>
              <div class="tax-card">
                <label class="form-label">Alíquota IR</label>
                <Money v-model="obra_AliquotaImpostoIR" v-bind="number" class="money-input-dark"></Money>
              </div>
              <div class="tax-card">
                <label class="form-label">Alíquota Art 30</label>
                <Money v-model="obra_AliquotaImpostoArt30" v-bind="number" class="money-input-dark"></Money>
              </div>
            </div>

            <div class="toggle-grid">
              <div class="toggle-card">
                <div>
                  <label class="form-label">Status da obra</label>
                  <small class="form-helper-text">Defina se a obra entra ativa no sistema.</small>
                </div>
                <toggle-button v-model="obra_Ativo" :color="{ checked: '#2D8515', unchecked: '#FF0000', disabled: '#000000' }" :labels="{ checked: 'Ativa', unchecked: 'Inativa' }" :width="83" :height="25" :font-size="14" />
              </div>
              <div class="toggle-card">
                <div>
                  <label class="form-label">Aprovação automática</label>
                  <small class="form-helper-text">Controle se a obra deve seguir fluxo automático de aprovação.</small>
                </div>
                <toggle-button v-model="obra_AprovacaoAutomatica" :color="{ checked: '#2D8515', unchecked: '#FF0000', disabled: '#000000' }" :labels="{ checked: 'Sim', unchecked: 'Não' }" :width="83" :height="25" :font-size="14" />
              </div>
            </div>
          </section>

          <section class="form-section-card">
            <div class="form-section-card__header form-section-card__header--compact">
              <div>
                <h4>Previsões de faturamento</h4>
                <p>Adicione as previsões na própria lista para manter o cadastro rápido e intuitivo.</p>
              </div>
              <div class="section-balance-card" :class="{ 'is-balanced': ((obra_ValorTotal || 0) - obtemSomatoriaMedicoes()) === 0, 'is-warning': ((obra_ValorTotal || 0) - obtemSomatoriaMedicoes()) !== 0 }">
                <span>Saldo restante</span>
                <strong>{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format((obra_ValorTotal || 0) - obtemSomatoriaMedicoes()) }}</strong>
              </div>
            </div>

            <div class="simple-entry-block">
              <div class="simple-entry-block__topbar simple-entry-block__topbar--summary-only">
                <small class="form-helper-text mb-0">Total lançado: {{ obra_Medicoes.length }} {{ obra_Medicoes.length === 1 ? 'item' : 'itens' }}</small>
              </div>

              <div class="compact-inline-form compact-inline-form--always-open">
                <div class="compact-inline-form__grid compact-inline-form__grid--three">
                  <div class="compact-inline-form__field">
                    <label class="form-label">Data faturamento</label>
                    <div class="date-input-wrapper">
                      <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevista" format="dd/MM/yyyy" type="date" :open.sync="open" @change="calculaDataRecebimento()"></DatePickerMask>
                    </div>
                  </div>
                  <div class="compact-inline-form__field">
                    <label class="form-label">Data recebimento</label>
                    <div class="date-input-wrapper">
                      <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevistaRecebimento" format="dd/MM/yyyy" type="date" :open.sync="open"></DatePickerMask>
                    </div>
                    <small class="form-helper-text">Preenchida automaticamente com base na data de faturamento + dias de pagamento, mas pode ser ajustada manualmente.</small>
                  </div>
                  <div class="compact-inline-form__field">
                    <label class="form-label">Valor previsto</label>
                    <Money v-model="modalMedicao_ValorPrevisto" v-bind="money" class="money-input-dark"></Money>
                  </div>
                </div>
                <div class="compact-inline-form__actions compact-inline-form__actions--single">
                  <b-button v-on:click="adicionaMedicao()" variant="success" class="width-200 mb-0 mr-0"><span>Adicionar previsão</span></b-button>
                </div>
              </div>

              <div class="table-card table-card--fluid">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho texto-centro">Dt. prevista Faturamento</th>
                      <th class="estilo-cabecalho texto-centro">Dt. prevista Recebimento</th>
                      <th class="estilo-cabecalho texto-centro">Previsto (R$)</th>
                      <th class="estilo-cabecalho texto-centro">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(row, index) in obra_Medicoes" :key="'med-' + (row.id || index)">
                      <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.data) }}</td>
                      <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevistaRecebimento) }}</td>
                      <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(row.valor) }}</td>
                      <td class="estilo-celula texto-centro">
                        <button type="button" @click="removeMedicao(row);" class="btn width-100 mb-3 mr-3 btn-outline-danger">Excluir</button>
                      </td>
                    </tr>
                    <tr v-if="!obra_Medicoes || obra_Medicoes.length === 0">
                      <td class="estilo-celula texto-centro" colspan="4">Nenhum registro encontrado</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </section>

          <section class="form-section-card">
            <div class="form-section-card__header form-section-card__header--compact">
              <div>
                <h4>ETO</h4>
                <p>Faça os lançamentos direto na lista, sem abrir outras etapas dentro do cadastro.</p>
              </div>
              <div class="section-balance-card" :class="{ 'is-balanced': ((obra_ValorCusto || 0) - obtemSomatoriaETOs()) === 0, 'is-warning': ((obra_ValorCusto || 0) - obtemSomatoriaETOs()) !== 0 }">
                <span>Saldo ETO</span>
                <strong>{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format((obra_ValorCusto || 0) - obtemSomatoriaETOs()) }}</strong>
              </div>
            </div>

            <div class="simple-entry-block">
              <div class="simple-entry-block__topbar simple-entry-block__topbar--summary-only">
                <small class="form-helper-text mb-0">Total lançado: {{ obra_ETOs.length }} {{ obra_ETOs.length === 1 ? 'item' : 'itens' }}</small>
              </div>

              <div class="compact-inline-form compact-inline-form--always-open">
                <div class="compact-inline-form__grid compact-inline-form__grid--two">
                  <div class="compact-inline-form__field">
                    <label class="form-label">Data prevista</label>
                    <div class="date-input-wrapper">
                      <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalETO_DataPrevista" format="dd/MM/yyyy" type="date" :open.sync="open"></DatePickerMask>
                    </div>
                  </div>
                  <div class="compact-inline-form__field">
                    <label class="form-label">Valor previsto</label>
                    <Money v-model="modalETO_ValorPrevisto" v-bind="money" class="money-input-dark"></Money>
                  </div>
                </div>
                <div class="compact-inline-form__actions compact-inline-form__actions--single">
                  <b-button v-on:click="adicionaETO()" variant="success" class="width-200 mb-0 mr-0"><span>Adicionar ETO</span></b-button>
                </div>
              </div>

              <div class="table-card table-card--fluid">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho texto-centro">Dt. prevista</th>
                      <th class="estilo-cabecalho texto-centro">Previsto (R$)</th>
                      <th class="estilo-cabecalho texto-centro">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(row, index) in obra_ETOs" :key="'eto-' + (row.id || index)">
                      <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.data) }}</td>
                      <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(row.valor) }}</td>
                      <td class="estilo-celula texto-centro">
                        <button type="button" @click="removeETO(row);" class="btn width-100 mb-3 mr-3 btn-outline-danger">Excluir</button>
                      </td>
                    </tr>
                    <tr v-if="!obra_ETOs || obra_ETOs.length === 0">
                      <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </section>
        </b-container>

        <template #modal-footer>
          <div class="cadastro-modal-footer cadastro-modal-footer--actions-only">
            <div class="cadastro-modal-footer__actions">
              <b-button :disabled="modal_InativaBotaoCancelar" @click="modal_Exibir = false" variant="dark" class="width-100 mb-0 mr-3">Cancelar</b-button>
              <b-button v-show="modal_ExibirBotaoCadastrar" :disabled="modal_InativaBotaoCadastrar" v-on:click="cadastraObra()" variant="success" class="width-200 mb-0 mr-3">
                <div v-if="controle_ObraCadastrando" class="spinner-border spinner-border-sm"></div>
                <span v-if="controle_ObraCadastrando"> Aguarde ...</span>
                <span v-else>Cadastrar</span>
              </b-button>
              <b-button v-show="modal_ExibirBotaoEditar" :disabled="modal_InativaBotaoEditar" v-on:click="editarCliente()" variant="info" class="width-150 mb-0 mr-3">
                <div v-if="controle_ObraEditando" class="spinner-border spinner-border-sm"></div>
                <span v-if="controle_ObraEditando"> Aguarde ...</span>
                <span v-else>Editar</span>
              </b-button>
            </div>
          </div>
        </template>
      </b-modal>

      <div class="table-card">
        <table class="estilo-tabela tabela-identidade">
          <thead>
            <tr>
              <th class="estilo-cabecalho texto-centro">Código</th>
              <th class="estilo-cabecalho texto-centro">Código Proposta</th>
              <th class="estilo-cabecalho">Cliente</th>
              <th class="estilo-cabecalho">Cidade</th>
              <th class="estilo-cabecalho texto-centro">Data início</th>
              <th class="estilo-cabecalho texto-centro">Data Fim</th>
              <th class="estilo-cabecalho texto-centro">Prazo (dias)</th>
              <th class="estilo-cabecalho texto-centro">Status</th>
              <th class="estilo-cabecalho texto-centro">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in obras" :key="'obra-' + (row.id || index)">
              <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
              <td class="estilo-celula texto-centro">{{ row.codigoProposta }}</td>
              <td class="estilo-celula">{{ row.clienteFormatado }}</td>
              <td class="estilo-celula">{{ row.cidadeFormatada }}</td>
              <td class="estilo-celula texto-centro">{{ row.dataInicioObraFormatada }}</td>
              <td class="estilo-celula texto-centro">{{ row.dataFimObraFormatada }}</td>
              <td class="estilo-celula texto-centro">{{ row.prazoDias }}</td>
              <td class="estilo-celula texto-centro">{{ row.bloqueada == true ? 'Bloqueada' : 'Desbloqueada' }} - {{ row.finalizada == true ? 'Finalizada' : 'Ativa' }} - {{ row.cancelada == true ? 'Cancelada' : 'Ativa' }}</td>
              <td class="estilo-celula texto-centro">
                <button
                  type="button"
                  class="btn btn-success"
                  @click="abrirModalAcoesObra(row)"
                >
                  Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                </button>
              </td>
            </tr>
            <tr v-if="!obras || obras.length === 0">
              <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
            </tr>
          </tbody>
        </table>
      </div>
    </Widget>

    <!-- Modal de ações da tabela de obras -->
    <ModalAcoes
      :exibir.sync="modalAcoes_Exibir"
      :titulo="modalAcoes_Titulo"
      :itens="modalAcoes_Itens"
    />
  </div>
</template>

<style>
.my-picker-class {
  background-color: transparent;
  border: solid !important;
  border-color: white;
  border-block-color: white;
}

.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 20px;
}

.page-subtitle {
  margin: 6px 0 0;
  color: rgba(255, 255, 255, 0.72);
  max-width: 880px;
}

.obras-widget {
  padding-top: 8px;
}

.top-actions-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  flex-wrap: wrap;
  margin-bottom: 18px;
  padding: 18px 20px;
  border-radius: 16px;
  background: linear-gradient(135deg, rgba(255, 214, 57, 0.12), rgba(255, 214, 57, 0.04));
  border: 1px solid rgba(255, 214, 57, 0.16);
}

.top-actions-bar__info {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.top-actions-bar__badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 6px 12px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  background: rgba(255, 214, 57, 0.18);
  color: #ffd639;
}

.top-actions-bar__text {
  color: rgba(255, 255, 255, 0.8);
}

.top-actions-bar__buttons {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.btn-hero-action {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 10px 18px;
  border-radius: 12px;
  font-weight: 700;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.22);
}

.btn-hero-action__icon {
  font-size: 18px;
  line-height: 1;
}

.top-action-btn {
  min-width: 148px;
  border-radius: 12px;
}

.cadastro-modal__hero {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 18px;
  padding: 8px 4px 4px;
  margin-bottom: 18px;
}

.cadastro-modal__eyebrow {
  display: inline-block;
  margin-bottom: 10px;
  padding: 4px 10px;
  border-radius: 999px;
  background: rgba(255, 214, 57, 0.14);
  color: #ffd639;
  font-size: 12px;
  font-weight: 700;
  text-transform: uppercase;
}

.cadastro-modal__title {
  margin: 0;
  color: #fff;
}

.cadastro-modal__description {
  margin: 8px 0 0;
  color: rgba(255, 255, 255, 0.72);
  max-width: 760px;
}


.cadastro-steps {
  display: flex;
  gap: 12px;
  margin-bottom: 18px;
  overflow-x: auto;
}

.cadastro-step {
  display: flex;
  align-items: center;
  gap: 10px;
  min-width: 180px;
  padding: 10px 14px;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.06);
  color: rgba(255, 255, 255, 0.78);
}

.cadastro-step span {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: rgba(255, 214, 57, 0.16);
  color: #ffd639;
  font-weight: 700;
}

.cadastro-step small {
  font-size: 13px;
  font-weight: 600;
}

.form-section-card {
  margin-bottom: 18px;
  padding: 20px;
  border-radius: 18px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  box-shadow: 0 14px 32px rgba(0, 0, 0, 0.12);
}

.form-section-card__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 18px;
}

.form-section-card__header h4 {
  margin: 0;
  color: #fff;
}

.form-section-card__header p {
  margin: 6px 0 0;
  color: rgba(255, 255, 255, 0.68);
}

.form-section-card__header--compact {
  margin-bottom: 14px;
}

.form-field-group {
  margin-bottom: 18px;
}

.form-label {
  display: inline-block;
  margin-bottom: 8px;
  color: #fff;
  font-weight: 600;
}

.form-helper-text {
  display: block;
  margin-top: 8px;
  color: rgba(255, 255, 255, 0.58);
  font-size: 12px;
}

.section-subtitle {
  margin: 4px 0 14px;
  padding-top: 4px;
  color: rgba(255, 255, 255, 0.82);
  font-size: 13px;
  font-weight: 700;
  letter-spacing: 0.03em;
  text-transform: uppercase;
}

.date-input-wrapper {
  width: 100%;
}

.date-input-wrapper .mx-datepicker,
.date-input-wrapper .mx-input-wrapper {
  width: 100%;
}

.date-input-wrapper .mx-input {
  width: 100%;
  color: white !important;
  background-color: rgba(0, 0, 0, 0.55) !important;
  border: 1px solid rgba(255, 255, 255, 0.08) !important;
  border-radius: 12px !important;
  min-height: 44px;
  padding: 10px 40px 10px 14px !important;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
  box-shadow: none !important;
}

.date-input-wrapper .mx-input:focus {
  border-color: rgba(255, 214, 57, 0.55) !important;
  box-shadow: 0 0 0 3px rgba(255, 214, 57, 0.12) !important;
}

.date-input-wrapper .mx-icon-calendar,
.date-input-wrapper .mx-icon-clear {
  color: rgba(255, 255, 255, 0.75);
}

.simple-entry-block {
  padding: 16px;
  border-radius: 16px;
  background: rgba(0, 0, 0, 0.16);
  border: 1px solid rgba(255, 255, 255, 0.06);
}

.simple-entry-block__topbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
  margin-bottom: 14px;
}

.simple-entry-block__topbar--summary-only {
  justify-content: flex-end;
}

.compact-inline-form {
  margin-bottom: 18px;
  padding: 16px;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px dashed rgba(255, 214, 57, 0.22);
}

.compact-inline-form--always-open {
  background: rgba(255, 255, 255, 0.025);
}

.compact-inline-form__grid {
  display: grid;
  gap: 16px;
  margin-bottom: 16px;
}

.compact-inline-form__grid--three {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.compact-inline-form__grid--two {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.compact-inline-form__field .form-label {
  margin-bottom: 8px;
}

.compact-inline-form__actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  flex-wrap: wrap;
}

.compact-inline-form__actions--single {
  justify-content: flex-end;
}

.form-input-dark,
.form-textarea-dark,
.money-input-dark {
  width: 100%;
  color: white !important;
  background-color: rgba(0, 0, 0, 0.55) !important;
  border: 1px solid rgba(255, 255, 255, 0.08) !important;
  border-radius: 12px !important;
  min-height: 44px;
  padding: 10px 14px !important;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

.form-input-dark:focus,
.form-textarea-dark:focus,
.money-input-dark:focus {
  border-color: rgba(255, 214, 57, 0.55) !important;
  box-shadow: 0 0 0 3px rgba(255, 214, 57, 0.12) !important;
}

.form-textarea-dark {
  min-height: 115px;
}

.form-input-dark--readonly {
  opacity: 1 !important;
  cursor: default;
  color: rgba(255, 255, 255, 0.92) !important;
  background-color: rgba(255, 255, 255, 0.06) !important;
  border-style: dashed !important;
}

.prazo-highlight,
.section-balance-card,
.signal-highlight-box {
  padding: 14px 16px;
  border-radius: 14px;
  background: rgba(255, 214, 57, 0.08);
  border: 1px solid rgba(255, 214, 57, 0.16);
  min-width: 180px;
}

.prazo-highlight span,
.section-balance-card span,
.signal-highlight-box span {
  display: block;
  margin-bottom: 6px;
  font-size: 12px;
  color: rgba(255, 255, 255, 0.72);
}

.prazo-highlight strong,
.section-balance-card strong,
.signal-highlight-box strong {
  color: #ffd639;
  font-size: 20px;
}

.section-balance-card.is-balanced {
  background: rgba(45, 133, 21, 0.12);
  border-color: rgba(45, 133, 21, 0.28);
}

.section-balance-card.is-balanced strong {
  color: #7df07a;
}

.section-balance-card.is-warning {
  background: rgba(255, 214, 57, 0.08);
  border-color: rgba(255, 214, 57, 0.16);
}

.financial-summary-grid,
.taxes-grid,
.toggle-grid {
  display: grid;
  gap: 16px;
  margin-bottom: 18px;
}

.financial-summary-grid {
  grid-template-columns: repeat(3, minmax(0, 1fr));
}

.financial-summary-card,
.tax-card,
.toggle-card {
  padding: 16px;
  border-radius: 14px;
  background: rgba(0, 0, 0, 0.18);
  border: 1px solid rgba(255, 255, 255, 0.06);
}

.financial-summary-card span {
  display: block;
  margin-bottom: 8px;
  color: rgba(255, 255, 255, 0.62);
  font-size: 12px;
}

.financial-summary-card strong {
  color: #fff;
  font-size: 20px;
}

.taxes-grid {
  grid-template-columns: repeat(4, minmax(0, 1fr));
}

.toggle-grid {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.toggle-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 14px;
}


.section-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
  margin-bottom: 12px;
}

.section-toolbar__button {
  border-radius: 12px;
}

.nested-widget {
  margin-bottom: 0;
  background: transparent;
  box-shadow: none;
}

.cadastro-modal-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  width: 100%;
  padding-top: 8px;
}

.cadastro-modal-footer__summary {
  display: flex;
  align-items: center;
  gap: 14px;
  flex-wrap: wrap;
  color: rgba(255, 255, 255, 0.8);
}

.cadastro-modal-footer__summary strong {
  color: #fff;
}

.cadastro-modal-footer__actions {
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

@media (max-width: 991px) {
  .cadastro-modal__hero,
  .form-section-card__header,
  .cadastro-modal-footer,
  .toggle-card,
  .simple-entry-block__topbar,
  .compact-inline-form__actions {
    flex-direction: column;
    align-items: stretch;
  }

  .financial-summary-grid,
  .taxes-grid,
  .toggle-grid,
  .compact-inline-form__grid--three,
  .compact-inline-form__grid--two {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 767px) {
  .top-actions-bar,
  .top-actions-bar__info,
  .top-actions-bar__buttons,
  .section-toolbar {
    flex-direction: column;
    align-items: stretch;
  }

}
</style>
<script>
import moment from "moment";
import Vue from "vue";
import Widget from "@/components/Widget/Widget";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ToggleButton from "vue-js-toggle-button";
import Multiselect from "vue-multiselect";
import { VueMaskDirective } from "v-mask";
import DatePicker from "vue2-datepicker";
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";
import ApiService from "@/services/api.service.js";
import CurrencyInput from "../../components/CurrencyInput.vue";
import { Money } from 'v-money';
import ModalAcoes from "../../components/ModalAcoes/ModalAcoes.vue";

Vue.directive("mask", VueMaskDirective);

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

DatePicker.locale("pt-BR");

export default {
  name: "Obras",
  components: { DatePicker, Widget, Loading, Multiselect, CurrencyInput, DatePickerMask, Money, ModalAcoes },
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

      dataMinima: (new Date()).setDate(new Date().getDate() - 1),

      open: false,

      initOptions: {
        renderer: "canvas",
      },

      isLoading: false,

      modal_Exibir: false,
      modal_Titulo: "Cadastro de Cliente",
      modal_ExibirBotaoCadastrar: true,
      modal_ExibirBotaoEditar: false,
      modal_InativaBotaoCancelar: false,
      modal_InativaBotaoCadastrar: false,
      modal_InativaBotaoEditar: true,

      modalMedicao_Exibir: false,
      modalMedicao_Titulo: 'Nova previsão de faturamento',
      modalMedicao_DataPrevista: null,
      modalMedicao_DataPrevistaRecebimento: null,
      modalMedicao_ValorPrevisto: 0,
      modalMedicao_SaldoMedicao: 0,

      modalETO_Exibir: false,
      modalETO_Titulo: 'Novo ETO',
      modalETO_DataPrevista: null,
      modalETO_ValorPrevisto: 0,
      modalETO_SaldoETO: 0,


      usuarioDTO: null,

      obra: null,

      obra_ClienteSelecionado: null,
      obra_EngenheiroSelecionado: null,
      obra_CidadeSelecionada: null,

      obra_Id: 0,
      obra_IdCliente: 0,
      obra_IdUsuarioAprovacao: 0,
      obra_IdCidade: 0,
      obra_Codigo: "",
      obra_CodigoSequencia: "",
      obra_CodigoAno: "",
      obra_CodigoProposta: "",
      obra_NumeroPedidoCliente: "",
      obra_Endereco: "",
      obra_EnderecoDeEntrega: "",
      obra_Descricao: "",
      obra_DataProposta: null,
      obra_DataInicio: null,
      obra_DataFim: null,
      obra_PrazoDias: 0,
      obra_ValorTotal: 0.0,
      obra_ValorCusto: 0.0,
      obra_ValorMaterial: 0.0,
      obra_ValorNaoComissionado: 0.0,
      obra_AliquotaImpostoISS: 0.0,
      obra_AliquotaImpostoINSS: 0.0,
      obra_AliquotaImpostoIR: 0.0,
      obra_AliquotaImpostoArt30: 0.0,
      obra_Ativo: true,
      obra_AprovacaoAutomatica: false,
      obra_ValorSinal: 0.0,
      obra_DataRecebimentoSinal: null,
      obra_PercentualSinal: 0.0,
      obra_Cep: '',
      obra_Medicoes: [],
      obra_ETOs: [],
      obra_DiretorAprovador: null,
      obra_DiasDePagamento: 0,

      controle_ObraCadastrando: false,
      controle_ObraEditando: false,

      obras: [],
      clientes: [],
      engenheiros: [],
      diretores: [],
      cidades: [],

      columns: [
        "codigo",
        "codigoProposta",
        "clienteFormatado",
        "cidadeFormatada",
        "dataInicioObraFormatada",
        "dataFimObraFormatada",
        "prazoDias",
        "ativo",
        "acoes",
      ],

      optionsTable: {
        perPage: 10,
        headings: {
          codigo: "Código",
          codigoProposta: "Código Proposta",
          clienteFormatado: "Cliente",
          cidadeFormatada: "Cidade",
          dataInicioObraFormatada: "Data início",
          dataFimObraFormatada: "Data Fim",
          prazoDias: "Prazo (dias)",
          ativo: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [
          "codigo",
          "codigoProposta",
          "cliente",
          "cidade",
          "dataInicioObraFormatada",
          "dataFimObraFormatada",
          "prazoDias",
          "ativo",
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

      colunasMedicoes: [
        'dataPrevista',
        'dataPrevistaRecebimento',
        'valorPrevisto',
        'acoes',
      ],

      opcoesMedicoes: {
        perPage: 1000,
        filterable: false,
        headings: {
          dataPrevista: 'Dt. prevista Faturamento',
          dataPrevistaRecebimento: 'Dt. prevista Recebimento',
          valorPrevisto: 'Previsto (R$)',
          acoes: 'Ações',
        },
        clientSorting: true,
        sortable: [
          'dataPrevista',
          'valorPrevisto',
          'valorMedido',
          'numeroNotaFiscal',
        ],
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

      colunasControleCusto: [
        'dataPrevista',
        'valorPrevisto',
        'acoes',
      ],

      opcoesControleCusto: {
        perPage: 1000,
        headings: {
          dataPrevista: 'Dt. prevista',
          valorPrevisto: 'Previsto (R$)',
          acoes: 'Ações',
        },
        clientSorting: true,
        filterable: false,
        sortable: [
          'dataPrevista',
          'valorPrevisto',
          'valorMedido',
        ],
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
    };
  },
  watch: {
    obra_DiasDePagamento() {
      this.calculaDataRecebimento();
    },
    modalMedicao_DataPrevista() {
      this.calculaDataRecebimento();
    }
  },

  methods: {
    // ============================================================
    // Ações da tabela de obras (usadas pelo <ModalAcoes>).
    // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
    // ============================================================
    montaAcoesObra(row) {
      const u = this.usuarioDTO;
      const podeEditar = u != null && u.obraEditar;
      return [
        {
          label: "Habilitar aprovação automática",
          descricao: "Aprovar solicitações automaticamente",
          icone: "magic",
          variante: "primary",
          onClick: () => this.habilitarAprovacaoAutomatica(row.id),
          visible: !row.cancelada && podeEditar && !row.aprovacaoAutomatica,
        },
        {
          label: "Desabilitar aprovação automática",
          descricao: "Voltar a aprovar manualmente",
          icone: "magic",
          variante: "warning",
          onClick: () => this.habilitarAprovacaoAutomatica(row.id),
          visible: !row.cancelada && podeEditar && row.aprovacaoAutomatica,
        },
        {
          label: "Editar",
          descricao: "Editar dados da obra",
          icone: "pencil",
          variante: "primary",
          onClick: () => this.redirecionaDetalhes(row.id),
          visible: !row.cancelada && podeEditar,
        },
        {
          label: "Bloquear",
          descricao: "Bloquear movimentações na obra",
          icone: "lock",
          variante: "warning",
          onClick: () => this.bloquearDesbloquearObra(row.id, true),
          visible: !row.cancelada && row.bloqueada == false && podeEditar,
        },
        {
          label: "Desbloquear",
          descricao: "Permitir movimentações novamente",
          icone: "unlock",
          variante: "success",
          onClick: () => this.bloquearDesbloquearObra(row.id, false),
          visible: !row.cancelada && row.bloqueada == true && podeEditar,
        },
        {
          label: "Finalizar",
          descricao: "Encerrar a obra",
          icone: "check",
          variante: "success",
          onClick: () => this.finalizarReativarObra(row.id, true),
          visible: !row.cancelada && row.finalizada == false && podeEditar,
        },
        {
          label: "Reativar",
          descricao: "Reabrir a obra finalizada",
          icone: "refresh",
          variante: "primary",
          onClick: () => this.finalizarReativarObra(row.id, false),
          visible: !row.cancelada && row.finalizada == true && podeEditar,
        },
        {
          label: "Cancelar",
          descricao: "Cancelar a obra",
          icone: "ban",
          variante: "danger",
          onClick: () => this.cancelarObra(row.id),
          visible: row.cancelada == false && podeEditar,
        },
        {
          label: "Rel. ETO",
          descricao: "Baixar relatório ETO em PDF",
          icone: "file-pdf-o",
          onClick: () => this.downloadEtoObraPdf(row.id, row.codigo),
        },
        {
          label: "Rel. CC PDF",
          descricao: "Baixar conta corrente em PDF",
          icone: "file-pdf-o",
          onClick: () => this.downloadContaCorrentePDF(row.id, row.codigo),
        },
        {
          label: "Rel. CC XLS",
          descricao: "Baixar conta corrente em planilha",
          icone: "file-excel-o",
          onClick: () => this.downloadContaCorrenteExcel(row.id, row.codigo),
        },
      ];
    },

    abrirModalAcoesObra(row) {
      this.modalAcoes_Itens = this.montaAcoesObra(row);
      this.modalAcoes_Titulo = "Ações da obra " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    formataDataSemHora: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY");
    },

    normalizaData(valor) {
      if (!valor) return null;
      if (valor instanceof Date) return isNaN(valor.getTime()) ? null : new Date(valor.getTime());
      if (typeof valor === 'string') {
        const partes = valor.includes('/') ? valor.split('/') : null;
        if (partes && partes.length === 3) {
          const [dia, mes, ano] = partes.map(Number);
          const data = new Date(ano, mes - 1, dia);
          return isNaN(data.getTime()) ? null : data;
        }
        const data = new Date(valor);
        return isNaN(data.getTime()) ? null : data;
      }
      const data = new Date(valor);
      return isNaN(data.getTime()) ? null : data;
    },

    formataDataCampo(data) {
      const dataNormalizada = this.normalizaData(data);
      return dataNormalizada ? moment(dataNormalizada).format('DD/MM/YYYY') : '';
    },

    finalizarReativarObra(idObra, ativo) {
      ApiService.ativarDesativar(
        "Obra",
        idObra,
        ativo,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar obra",
              result.message,
              "error"
            );
          } else {
            this.listaObras();
          }
        }
      );
    },

    bloquearDesbloquearObra: function (idObra, bloqueada) {
      ApiService.bloquearDesbloquearObra(
        idObra,
        bloqueada,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao ativar/desativar obra",
              result.message,
              "error"
            );
          } else {
            this.listaObras();
          }
        }
      );
    },

    habilitarAprovacaoAutomatica: function (idObra) {
      ApiService.habilitarAprovacaoAutomatica(
        idObra,
        (result) => {
          if (result.status != 200) {
            this.$swal(
              "Erro ao habilitar/desabilitar aprovação automática",
              result.message,
              "error"
            );
          } else {
            this.listaObras();
          }
        }
      );
    },

    async cancelarObra(idObra) {
  // 1) Confirmação
  const { isConfirmed } = await this.$swal({
    title: 'Cancelar obra?',
    text: 'Essa ação não poderá ser desfeita.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sim, cancelar',
    cancelButtonText: 'Não'
  })

  if (!isConfirmed) return

  // 3) Chamada da API
  ApiService.cancelarObra(
    idObra,
    (result) => {
      this.$swal.close()

      if (!result || result.status !== 200) {
        this.$swal(
          'Erro ao cancelar obra',
          (result && result.message) || 'Tente novamente.',
          'error'
        )
      } else {
        this.$swal('Sucesso', 'Obra cancelada.', 'success')
        this.listaObras()
      }
    }
  )
},


    calculaDataRecebimento() {
      const dataBase = this.normalizaData(this.modalMedicao_DataPrevista);
      const diasPagamento = parseInt(this.obra_DiasDePagamento, 10);

      if (!dataBase) {
        this.modalMedicao_DataPrevistaRecebimento = null;
        return;
      }

      const dataRecebimento = new Date(dataBase.getTime());
      dataRecebimento.setHours(0, 0, 0, 0);
      dataRecebimento.setDate(dataRecebimento.getDate() + (isNaN(diasPagamento) ? 0 : diasPagamento));
      this.modalMedicao_DataPrevistaRecebimento = dataRecebimento;
    },

    funcaoDataPrevista() {
      console.log('teste');
    },


    obtemSomatoriaMedicoes() {
      var result = 0.0;
      this.obra_Medicoes.forEach(x => {
        result = result + parseFloat(parseFloat(x.valor).toFixed(2));
      });

      return Number(result).toFixed(2);
    },

    abreModalNovaMedicao() {
      this.modalMedicao_ValorPrevisto = 0;
      this.modalMedicao_DataPrevista = null;
      this.modalMedicao_DataPrevistaRecebimento = null;
      this.modalMedicao_SaldoMedicao = this.obra_ValorTotal - this.obtemSomatoriaMedicoes();
      this.modalMedicao_Exibir = true;
    },

    adicionaMedicao() {
      this.modalMedicao_SaldoMedicao = this.obra_ValorTotal - this.obtemSomatoriaMedicoes();
      this.calculaDataRecebimento();

      if (this.obra_ValorTotal <= 0) {
        this.$swal("Informe um valor total para a obra", "", "error");
        return;
      }

      if (!this.modalMedicao_DataPrevista) {
        this.$swal("Informe a data de faturamento", "", "error");
        return;
      }

      if (!this.modalMedicao_DataPrevistaRecebimento) {
        this.$swal("Não foi possível calcular a data de recebimento", "", "error");
        return;
      }

      if (parseFloat(this.modalMedicao_ValorPrevisto) <= 0) {
        this.$swal("Informe um valor previsto maior que zero", "", "error");
        return;
      }

      if (parseFloat(this.modalMedicao_SaldoMedicao) >= parseFloat(this.modalMedicao_ValorPrevisto)) {
        this.obra_Medicoes.push(
          {
            id: this.obra_Medicoes.length,
            data: this.modalMedicao_DataPrevista,
            dataPrevistaRecebimento: this.modalMedicao_DataPrevistaRecebimento,
            valor: parseFloat(this.modalMedicao_ValorPrevisto)
          });

        this.modalMedicao_SaldoMedicao = parseFloat((parseFloat(this.modalMedicao_SaldoMedicao) - parseFloat(this.modalMedicao_ValorPrevisto)).toString()).toFixed(5);
        this.modalMedicao_DataPrevista = null;
        this.modalMedicao_DataPrevistaRecebimento = null;
        this.modalMedicao_ValorPrevisto = 0.0;
        return;
      }

      this.$swal("O valor informado ultrapassa o saldo restante", "", "error");
    },

    removeMedicao(medicao) {
      var novaListaMedicoes = [];

      this.obra_Medicoes.forEach(x => {
        if (x.id != medicao.id) {
          x.id = novaListaMedicoes.length;
          novaListaMedicoes.push(x);
        }
      });

      this.obra_Medicoes = novaListaMedicoes;

      this.modalMedicao_SaldoMedicao = this.obra_ValorTotal - this.obtemSomatoriaMedicoes();

    },

    obtemSomatoriaETOs() {

      var result = 0;
      this.obra_ETOs.forEach(x => {
        result = result + parseFloat(parseFloat(x.valor).toFixed(2));
      });

      return Number(result).toFixed(2);
    },

    abreModalETO() {
      this.modalETO_ValorPrevisto = 0;
      this.modalETO_DataPrevista = null;
      this.modalETO_SaldoETO = this.obra_ValorCusto - this.obtemSomatoriaETOs();
      this.modalETO_Exibir = true;
    },

    adicionaETO() {
      this.modalETO_SaldoETO = this.obra_ValorCusto - this.obtemSomatoriaETOs();

      if (this.obra_ValorCusto <= 0) {
        this.$swal("Informe um valor de custo para a obra", "", "error");
        return;
      }

      if (!this.modalETO_DataPrevista) {
        this.$swal("Informe a data prevista do ETO", "", "error");
        return;
      }

      if (parseFloat(this.modalETO_ValorPrevisto) <= 0) {
        this.$swal("Informe um valor previsto maior que zero", "", "error");
        return;
      }

      if (parseFloat(this.modalETO_SaldoETO) >= parseFloat(this.modalETO_ValorPrevisto)) {
        this.obra_ETOs.push({
          id: this.obra_ETOs.length,
          data: this.modalETO_DataPrevista,
          valor: parseFloat(this.modalETO_ValorPrevisto)
        });

        this.modalETO_SaldoETO = parseFloat((this.modalETO_SaldoETO - this.modalETO_ValorPrevisto).toString()).toFixed(5);
        this.modalETO_DataPrevista = null;
        this.modalETO_ValorPrevisto = 0.0;
        return;
      }

      this.$swal("O valor informado ultrapassa o saldo de ETO", "", "error");
    },

    removeETO(eto) {
      var novaListaETOs = [];

      this.obra_ETOs.forEach(x => {
        if (x.id != eto.id) {
          x.id = novaListaETOs.length;
          novaListaETOs.push(x);
        }
      });

      this.obra_ETOs = novaListaETOs;

      this.modalETO_SaldoETO = this.obra_ValorCusto - this.obtemSomatoriaETOs();

    },












    calculaPercentualSinal() {
      if (this.obra_ValorTotal <= 0)
        this.obra_PercentualSinal = 0;
      else
        this.obra_PercentualSinal = (this.obra_ValorSinal * 100) / this.obra_ValorTotal;
    },

    buscaCliente(cliente) {
      this.obra_Endereco = cliente.endereco;
      this.obra_EnderecoDeEntrega = cliente.endereco;
      this.obra_Cep = cliente.cep;
      this.obra_CidadeSelecionada = this.cidades.filter((cidade) => {
        return cidade.id == cliente.idCidade;
      })[0];
      this.obra_AliquotaImpostoISS = this.obra_CidadeSelecionada.aliquotaImpostoISS;
      this.obra_DiasDePagamento = cliente.diasDePagamento;
    },

    redirecionaDetalhes(idObra) {
      let routeData = this.$router.resolve({
        name: "ObraDetalhe",
        params: { id: idObra },
      });
      window.open(routeData.href);
    },

    multiselectCidades({ nome }) {
      return `${nome}`;
    },

    multiselectClientes({ nomeFantasia, razaoSocial, cnpj }) {
      return `${nomeFantasia} - ${cnpj}`;
    },

    multiselectEngenheiros({ nome }) {
      return `${nome}`;
    },

    multiselectDiretores({ nome }) {
      return `${nome}`;
    },

    preencheAliquotaISS(valor) {
      this.obra_AliquotaImpostoISS = valor.aliquotaImpostoISS;
    },

    calculaPrazoDias() {
      if (this.obra_DataInicio != null && this.obra_DataFim != null) {
        this.obra_PrazoDias =
          (this.obra_DataFim - this.obra_DataInicio) / 86400000;

        if (this.obra_PrazoDias < 0) {
          this.obra_DataInicio = this.obra_DataFim;
          this.obra_PrazoDias = 0;

          this.$swal(
            "A data de início da obra deve ser anterior a data de finalização da obra",
            "",
            "error"
          );
        }
      }
    },

    verificaValoresInformados() {
      if (parseFloat(this.obra_ValorTotal) < 0) {
        this.$swal("Não são permitidos valores negativos", "", "error");
        this.obra_ValorTotal = 0.0;
      } else if (parseFloat(this.obra_ValorCusto) < 0) {
        this.$swal("Não são permitidos valores negativos", "", "error");
        this.obra_ValorCusto = 0.0;
      } else if (parseFloat(this.obra_ValorNaoComissionado) < 0) {
        this.$swal("Não são permitidos valores negativos", "", "error");
        this.obra_ValorNaoComissionado = 0.0;
      }

      if (parseFloat(this.obra_ValorCusto) > parseFloat(this.obra_ValorTotal)) {
        this.$swal(
          "Valor de custo da obra não pode ser maior que o valor total da obra",
          "",
          "error"
        );
        this.obra_ValorCusto = this.obra_ValorTotal;
      }
    },


    cadastraObra() {
      let validado = true;

      var valorSomaMedicoes = this.obtemSomatoriaMedicoes();

      console.log(valorSomaMedicoes);
      console.log(this.obra_ValorTotal);

      if (valorSomaMedicoes != this.obra_ValorTotal) {
        this.$swal("Valores informados para as medições é diferente do valor total da obra", "", "error");
        return;
      }

      var valorSomaETOs = this.obtemSomatoriaETOs();

      console.log(valorSomaETOs);
      console.log(this.obra_ValorCusto);

      if (valorSomaETOs != this.obra_ValorCusto) {
        this.$swal("Valores informados de ETO é diferente do valor de custo da obra", "", "error");
        return;
      }

      if (this.obra_Cep == '') {
        this.$swal("Informe um CEP válido", "", "error");
        return;
      }

      if (this.obra_ClienteSelecionado == null) {
        validado = false;
        this.$swal("Informe um cliente para a obra", "", "error");
      }

      if (this.obra_CidadeSelecionada == null) {
        validado = false;
        this.$swal("Informe uma cidade para a obra", "", "error");
      }

      if (this.obra_EngenheiroSelecionado == null) {
        validado = false;
        this.$swal(
          "Informe um engenheiro responsável para a obra",
          "",
          "error"
        );
      }

      if (this.obra_CodigoProposta == "") {
        validado = false;
        this.$swal("Informe um código de proposta para a obra", "", "error");
      }

      if (this.obra_DataProposta == null) {
        validado = false;
        this.$swal("Informe a data da proposta para a obra", "", "error");
      }

      if (this.obra_Endereco == "") {
        validado = false;
        this.$swal("Informe o endereço de faturamento", "", "error");
      }

      if(this.obra_EnderecoDeEntrega == "")
      {
        validado = false;
        this.$swal("Informe o endereço de entrega", "", "error");
      }

      if (this.obra_DataInicio == null) {
        validado = false;
        this.$swal("Informe a data de início da obra", "", "error");
      }

      if (this.obra_DataFim == null) {
        validado = false;
        this.$swal("Informe a data de finalização da obra", "", "error");
      }

      if (this.obra_DiretorAprovador == null) {
        validado = false;
        this.$swal("Informe um aprovador", "", "error");
      }

      var usuariosAprovadores = [];
      this.obra_EngenheiroSelecionado.forEach(x => {
        usuariosAprovadores.push({ idUsuarioAprovacao: x.id });
      });

      if (validado) {
        let objetoObra = {
          Id: 0,
          IdCliente: this.obra_ClienteSelecionado.id,
          UsuariosAprovadores: usuariosAprovadores,
          IdCidade: this.obra_CidadeSelecionada.id,
          CEP: this.obra_Cep,
          Codigo: "",
          CodigoSequencia: 0,
          CodigoAno: 0,
          CodigoProposta: this.obra_CodigoProposta,
          NumeroPedidoCliente: this.obra_NumeroPedidoCliente,
          DiasDePagamento: this.obra_DiasDePagamento,
          EnderecoObra: this.obra_Endereco,
          EnderecoEntrega: this.obra_EnderecoDeEntrega,
          Descricao: this.obra_Descricao,
          DataProposta: this.obra_DataProposta,
          DataInicio: this.obra_DataInicio,
          DataFim: this.obra_DataFim,
          IdUsuarioDiretorAprovador: this.obra_DiretorAprovador.id,
          PrazoDias: this.obra_PrazoDias,
          ValorTotal: this.obra_ValorTotal,
          ValorMaterial: this.obra_ValorMaterial,
          ValorNaoComissionado: this.obra_ValorNaoComissionado,
          AliquotaImpostoISS: parseFloat(this.obra_AliquotaImpostoISS),
          AliquotaImpostoINSS: parseFloat(this.obra_AliquotaImpostoINSS),
          AliquotaImpostoIR: parseFloat(this.obra_AliquotaImpostoIR),
          AliquotaImpostoArt30: parseFloat(this.obra_AliquotaImpostoArt30),
          Ativo: this.obra_Ativo,
          AprovacaoAutomatica: this.obra_AprovacaoAutomatica,
          ValorSinal: this.obra_ValorSinal,
          DataRecebimentoSinal: this.obra_DataRecebimentoSinal,
          PercentualEquivalenteSinal: this.obra_PercentualSinal,
          Faturamentos: this.obra_Medicoes,
          ETOs: this.obra_ETOs
        };

        this.isLoading = true;

        ApiService.post("Obra", objetoObra, (result) => {

          this.isLoading = false;

          if (result.status != 201) {
            this.$swal("Erro ao cadastrar obra", result.message, "error");
          } else {
            this.listaObras();
            this.modal_Exibir = false;
          }
        });
      }
    },

    listaClientes: function () {
      this.clientes = [];

      ApiService.getAll("Cliente", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.clientes = result.data;
        }
      });
    },

    listaEngenheiros: function () {
      this.engenheiros = [];

      ApiService.getEngenheiros((result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.engenheiros = result.data;
        }
      });
    },

    listaDiretores: function () {
      this.diretores = [];

      ApiService.getDiretores((result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.diretores = result.data;
        }
      });
    },

    listaObras: function () {
      this.obras = [];

      ApiService.getAll("Obra", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.data, "error");
        } else {
          result.data.forEach((obra) => {

            obra.clienteFormatado = obra.cliente.razaoSocial;
            obra.cidadeFormatada = obra.cidade.nome;
            obra.dataInicioObraFormatada = moment(
              String(new Date(obra.dataInicio))
            ).format("DD/MM/YYYY");
            obra.dataFimObraFormatada = moment(
              String(new Date(obra.dataFim))
            ).format("DD/MM/YYYY");
            this.obras.push(obra);
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

    downloadResumoETOExcel() {
      this.isLoading = true;

      ApiService.downloadResumoETOExcel((result) => {
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

    downloadResumoETOPDF() {
      this.isLoading = true;

      ApiService.downloadResumoETOPDF((result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao baixar resumo de ETO", "error");
        } else {

          const blob = new Blob([result.data], {
            type: result.contentType,
          });

          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },

    downloadEtoObraPdf(idObra, codigoObra) {
      this.isLoading = true;

      ApiService.downloadEtoObraPdf(idObra, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao baixar ETO da Obra", "error");
        } else {

          const blob = new Blob([result.data], {
            type: result.contentType,
          });

          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },

    downloadContaCorrenteExcel(idObra, codigoObra) {
      this.isLoading = true;

      ApiService.downloadContaCorrenteExcel(idObra, (result) => {
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

    downloadContaCorrentePDF(idObra, codigoObra) {
      this.isLoading = true;

      ApiService.downloadContaCorrentePDF(idObra, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao baixar resumo de ETO", "error");
        } else {

          const blob = new Blob([result.data], {
            type: result.contentType,
          });

          const fileURL = URL.createObjectURL(blob);
          window.open(fileURL, '_blank');
        }
      });
    },

  },
  mounted() {
    this.listaClientes();
    this.listaEngenheiros();
    this.listaDiretores();
    this.listaObras();
    this.listaCidades();

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));
  },
};
</script>



<style src="./Obras.scss" lang="scss" />
