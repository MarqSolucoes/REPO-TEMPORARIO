<template>
  <div class="obraDetalhe-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Detalhes da Obra</h1>

    <b-modal :no-close-on-backdrop="true" id="medicao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
      header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalMedicao_Exibir" size="lg">

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
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevista" format="dd/MM/yyyy"
                  type="date" :open.sync="open" @change="calculaDataRecebimento()">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista recebimento</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevistaRecebimento"
                  format="dd/MM/yyyy" type="date" :open.sync="open">
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
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalMedicaoEdicao_Exibir" size="lg">

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
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalMedicaoEdicao_DataRecebimentoPrevista"
                  format="dd/MM/yyyy" type="date" :open.sync="open">
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

    <b-modal :no-close-on-backdrop="true" id="eto" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
      header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalETO_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">ETO</span>
          <h3 class="afm-hero__title">{{ modalETO_Titulo }}</h3>
          <p class="afm-hero__description">Cadastre a data prevista e o valor ajustado do ETO extra.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalETO_DataPrevista" format="dd/MM/yyyy" type="date"
                  :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor previsto ajustado</label>
                <Money v-model="modalETO_ValorPrevistoAjustado" v-bind="money"></Money>
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalETO_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="cadastrarETOExtra()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Cadastrar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="etoEdicao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
      header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalETOEdicao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">ETO</span>
          <h3 class="afm-hero__title">{{ modalETOEdicao_Titulo }}</h3>
          <p class="afm-hero__description">Edite o valor ajustado do ETO.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor previsto ajustado</label>
                <Money v-model="modalETOEdicao_ValorPrevistoAjustado" v-bind="money"></Money>
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalETOEdicao_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="editarETO()" variant="info" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="cadastro" class="modal-dialog modal-md afm-modal" body-bg-variant="modal"
      header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalInformacoes_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Obra</span>
          <h3 class="afm-hero__title">Editar Informações Básicas</h3>
          <p class="afm-hero__description">Atualize os dados cadastrais, financeiros e cronograma da obra.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
        <b-row>
          <b-col>
            <label class="mr-3">Engenheiro Responsável:</label>
            <multiselect v-model="obra_EngenheiroSelecionado" :multiple="true" :options="engenheiros"
              :custom-label="multiselectEngenheiros" select-label="Selecionar" placeholder="Selecione um engenheiro"
              label="nome" track-by="nome" :disabled="controle_ObraCadastrando"></multiselect>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col>
            <label class="mr-3">Diretor Aprovador:</label>
            <multiselect v-model="obra_DiretorAprovador" :multiple="false" :options="diretores"
              :custom-label="multiselectDiretores" select-label="Selecionar" placeholder="Selecione um diretor"
              label="nome" track-by="nome" :disabled="controle_ObraCadastrando"></multiselect>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col>
            <label class="mr-3">Endereço da Obra:</label>
            <b-form-input v-model="obra_Endereco" style="color: white"></b-form-input>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Dias de pagamento:</label>
            <b-form-input v-model="obra_DiasDePagamento" style="color: white"></b-form-input>
          </b-col>
          <b-col md="6">
            <label class="mr-3">Nº Pedido Cliente:</label>
            <b-form-input v-model="obra_NumeroPedidoCliente" style="color: white"></b-form-input>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col>
            <label class="mr-3">Descrição:</label>
            <textarea-autosize class="form-control" :min-height="115" v-model="obra_Descricao" style="color: white" />
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Valor Total:</label>
            <Money v-model="obra_ValorTotal" v-bind="money" :disabled="true" ></Money>
          </b-col>

          <b-col md="6">
            <label class="mr-3">Valor Custo:</label>
            <Money v-model="obra_ValorCusto" v-bind="money" :disabled="true" ></Money>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Valor Não Comissionado:</label>
            <Money v-model="obra_ValorNaoComissionado" :disabled="true" v-bind="money" ></Money>
          </b-col>

          <b-col md="6">
            <label class="mr-3">Valor Material:</label>
            <Money v-model="obra_ValorMaterial" v-bind="money" :disabled="true" ></Money>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Aliquota ISS:</label>
            <Money v-model="obra_AliquotaImpostoISS" v-bind="number" ></Money>
          </b-col>
          <b-col md="6">
            <label class="mr-3">Aliquota INSS:</label>
            <Money v-model="obra_AliquotaImpostoINSS" v-bind="number" ></Money>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Aliquota IR:</label>
            <Money v-model="obra_AliquotaImpostoIR" v-bind="number" ></Money>
          </b-col>
          <b-col md="6">
            <label class="mr-3">Aliquota Art 30:</label>
            <Money v-model="obra_AliquotaImpostoArt30" v-bind="number" ></Money>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Valor Sinal:</label>
            <Money v-model="obra_ValorSinal" v-bind="money" @keydown.native.tab="calculaPercentualSinal" ></Money>
          </b-col>
          <b-col md="6">
            <label class="mr-3">Data Recebimento Sinal:</label>
            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="obra_DataRecebimentoSinal" format="dd/MM/yyyy"
              type="date" :open.sync="open">
            </DatePickerMask>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6">
            <label class="mr-3">Percentual Sinal:</label>
            {{ this.obra_PercentualSinal.toFixed(2) }}%
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col md="6" class="teste">
            <label class="mr-3">Data início obra:</label>
            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="obra_DataInicio" format="dd/MM/yyyy" type="date"
              :open.sync="open" @change="calculaPrazoDias()">
            </DatePickerMask>
          </b-col>

          <br />

          <b-col md="6" class="teste">
            <label class="mr-3">Data fim obra:&nbsp;&nbsp;&nbsp;</label>
            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="obra_DataFim" format="dd/MM/yyyy" type="date"
              :open.sync="open" @change="calculaPrazoDias()" :shortcuts="false">
            </DatePickerMask>
          </b-col>
        </b-row>

        <br />

        <b-row>
          <b-col style="text-align: center">
            <h4 class="text-warning">
              <label class="mr-3" style="text-align: right">Prazo: {{ obra_PrazoDias }} dias</label>
            </h4>
          </b-col>
        </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalInformacoes_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="editarObra()" variant="info" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-row>
      <b-col lg="12" xs="12">
        <h5 class="fw-normal">Informações cadastrais</h5>
        <Widget customHeader class="estiloWidget">
          <b-row>
            <b-col md="6" xs="12" class="text-left">
              <button type="button" @click="modalInformacoes_Exibir = true;"
                class="btn width-150 mb-3 mr-3 btn-outline-info">
                Editar informações
              </button>
              <button type="button" @click="downloadContaCorrenteExcel()"
                class="btn width-150 mb-3 mr-3 btn-outline-success">
                Conta Corrente XLS
              </button>
              <button type="button" @click="downloadContaCorrentePDF()"
                class="btn width-170 mb-3 mr-3 btn-outline-success">
                Conta Corrente PDF
              </button>
              <!-- <button type="button" @click="downloadContaCorrentePDF()"
                class="btn width-170 mb-3 mr-3 btn-outline-success">
                ETO PDF
              </button> -->
              <div class="profileContactContainer">
                <br /><br /><br /><br /><br />
                <h4 class="fw-normal">
                  Código:
                  <span class="fw-semi-bold">{{
                    this.obra != null ? this.obra.codigo : ''
                  }}</span>
                </h4>
                <br />
                <p>
                  Razão social:
                  {{ this.obra != null ? this.obra.cliente.razaoSocial : '' }}
                </p>
                <p>
                  Endereço:
                  {{ this.obra != null ? this.obra.enderecoObra : '' }}
                </p>
                <p>
                  CNPJ: {{ this.obra != null ? this.obra.cliente.cnpj : '' }}
                </p>
                <p>
                  Alíquota ISS:
                  {{
                    this.obra != null
                    ? this.obra.aliquotaImpostoISS.toFixed(2)
                    : ''
                  }}
                  %
                </p>
              </div>
            </b-col>

            <b-col md="6" xs="12">
              <p style="text-align: left">
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Valor Total:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorTotal) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Valor Total Ajustado:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorTotalAjustado) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">ETO:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorCusto) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">ETO Ajustado:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorCustoAjustado) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Valor Material:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorMaterial) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Valor Não Comissionado:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.valorNaoComissionado) : "" }}</a><br /><br />

                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Saldo restante medição:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.saldoRestantePrevisaoMedicao) : "" }}</a><br />
                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px; text-align: left;">Saldo restante custo:
                  {{ this.obra != null ? new Intl.NumberFormat("pt-BR", {
                    style: "currency", currency:
                      "BRL"
                  }).format(this.obra.saldoRestantePrevisaoCusto) : "" }}</a><br /><br />

                <a href="#" class="badge badge-success rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px">Inicio da obra:
                  {{
                    this.obra != null ? this.obra.dataInicioObraFormatada : ''
                  }}</a><br />
                <a href="#" class="badge badge-danger rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px">Fim da obra:
                  {{
                    this.obra != null ? this.obra.dataFimObraFormatada : ''
                  }}</a><br />
                <a href="#" class="badge badge-default rounded-0 ml-2"
                  style="width: 300px; font-size: 14px; margin: 2px">Prazo:
                  {{ this.obra != null ? this.obra.prazoDias : '' }} dias</a>
              </p>
              <p class="lead mt-xlg">
                Descrição: {{ this.obra != null ? this.obra.descricao : '' }}
              </p>
            </b-col>
          </b-row>
        </Widget>
      </b-col>
    </b-row>

    <b-row>
      <b-col lg="12" xs="12">
        <h5 class="fw-normal">Medições</h5>
        <Widget customHeader class="estiloWidget">

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

          <div class="table-card">
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
                <tr v-for="(row, index) in (obra != null ? obra.medicoes : [])" :key="'med-' + (row.id || index)">
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
                <tr v-if="!obra || !obra.medicoes || obra.medicoes.length === 0">
                  <td class="estilo-celula texto-centro" colspan="6">Nenhum registro encontrado</td>
                </tr>
              </tbody>
            </table>
          </div>

          <b-button v-on:click="edicaoValorTotalMedicao = false" v-if="edicaoValorTotalMedicao == true" variant="info"
            class="width-200 mb-3 mr-3">
            <span>Cancelar</span>
          </b-button>
          <b-button v-on:click="salvarMedicoes()" v-if="edicaoValorTotalMedicao == true" variant="success"
            class="width-200 mb-3 mr-3">
            <span>Salvar</span>
          </b-button>
        </Widget>
      </b-col>
    </b-row>

    <b-row>
      <b-col lg="12" xs="12">
        <h5 class="fw-normal">ETO</h5>
        <Widget customHeader class="estiloWidget">

          <b-button v-on:click="edicaoValorTotalETO = true" v-if="edicaoValorTotalETO == false" variant="outline-info"
            class="width-200 mb-3 mr-3">
            <span>Editar Valor Total</span>
          </b-button>
          <b-button v-on:click="modalETO_DataPrevista = null; modalETO_ValorPrevistoAjustado = 0; modalETO_Exibir = true;"
            v-if="edicaoValorTotalETO" variant="outline-success" class="width-200 mb-3 mr-3">
            <span>Novo ETO Extra</span>
          </b-button>

          <br />

          <div v-if="edicaoValorTotalETO">
            <label class="mr-3">Valor Total Ajustado:</label>
            <Money v-model="obra_ValorCustoAjustado" v-bind="money" >
            </Money>

            <br /><br />

            <label class="mr-3">Somatória ETOs ajustados: {{ new Intl.NumberFormat("pt-BR", {
              style: "currency",
              currency: "BRL",
            }).format(somatoriaValoresAjustadosETOs) }}</label>

            <br /><br />

            <label class="mr-3">Saldo ETOs ajustados: {{ new Intl.NumberFormat("pt-BR", {
              style: "currency", currency:
                "BRL",
            }).format(this.obra_ValorCustoAjustado - somatoriaValoresAjustadosETOs) }}</label>
          </div>

          <div class="table-card">
            <table class="estilo-tabela tabela-identidade">
              <thead>
                <tr>
                  <th class="estilo-cabecalho texto-centro">Dt. Prevista</th>
                  <th class="estilo-cabecalho texto-centro">Previsto (R$)</th>
                  <th class="estilo-cabecalho texto-centro">Previsto Ajustado (R$)</th>
                  <th class="estilo-cabecalho texto-centro">Medido (R$)</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in (obra != null ? obra.controleCusto : [])" :key="'cc-' + (row.id || index)">
                  <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevista) }}</td>
                  <td class="estilo-celula texto-centro">{{ row.valorPrevisto != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevisto) : '' }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevistoAjustado) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMedido) }}</td>
                  <td class="estilo-celula texto-centro">
                    <b-button v-if="edicaoValorTotalETO == true && new Date() < new Date(row.dataPrevista)"
                      v-on:click="abreModalEdicaoETO(row)" variant="outline-warning" class="width-40 mb-3 mr-3">
                      <i class="fa fa-pencil" title="Editar"></i>
                    </b-button>

                    <b-button v-if="edicaoValorTotalETO == true && row.valorPrevisto == null" v-on:click="excluiETO(row)"
                      variant="outline-danger" class="width-40 mb-3 mr-3">
                      <i class="fa fa-close" title="Excluir"></i>
                    </b-button>
                  </td>
                </tr>
                <tr v-if="!obra || !obra.controleCusto || obra.controleCusto.length === 0">
                  <td class="estilo-celula texto-centro" colspan="5">Nenhum registro encontrado</td>
                </tr>
              </tbody>
            </table>
          </div>

          <b-button v-on:click="edicaoValorTotalETO = false" v-if="edicaoValorTotalETO == true" variant="info"
            class="width-200 mb-3 mr-3">
            <span>Cancelar</span>
          </b-button>
          <b-button v-on:click="salvarETOs()" v-if="edicaoValorTotalETO == true" variant="success"
            class="width-200 mb-3 mr-3">
            <span>Salvar</span>
          </b-button>
        </Widget>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import moment from 'moment';
import Vue from 'vue';
import Widget from '@/components/Widget/Widget';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import Loading from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.css';
import ToggleButton from 'vue-js-toggle-button';
import Multiselect from 'vue-multiselect';
import DatePicker from 'vue2-datepicker';
import DatePickerMask from 'vue2-datepicker-mask';
import 'vue2-datepicker/index.css';
import 'vue2-datepicker/locale/pt-br';
import ApiService from '@/services/api.service.js';
import CurrencyInput from '../../components/CurrencyInput.vue';
import { Money } from 'v-money';

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

DatePicker.locale('pt-BR');

export default {
  name: 'ObraDetalhe',
  components: {
    Widget,
    Loading,
    Multiselect,
    CurrencyInput,
    DatePicker,
    DatePickerMask,
    Money,
  },

  data() {
    return {
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

      dataMinima: (new Date()).setDate(new Date().getDate() -1),
      
      isLoading: false,

      open: false,
      obra: null,
      obra_Id: 0,
      obra_Endereco: '',
      obra_Descricao: '',
      obra_ValorTotal: 0.0,
      obra_ValorTotalAjustado: 0.0,
      obra_ValorCusto: 0.0,
      obra_ValorCustoAjustado: 0.0,
      obra_ValorMaterial: 0.0,
      obra_ValorNaoComissionado: 0.0,
      obra_DataInicio: null,
      obra_DataFim: null,
      obra_PrazoDias: 0,
      obra_AliquotaImpostoISS: 0.0,
      obra_AliquotaImpostoINSS: 0.0,
      obra_AliquotaImpostoIR: 0.0,
      obra_AliquotaImpostoArt30: 0.0,
      obra_ValorSinal: 0.0,
      obra_DataRecebimentoSinal: null,
      obra_PercentualSinal: 0.0,
      obra_DiretorAprovador: null,
      obra_DiasDePagamento: 0,
      obra_NumeroPedidoCliente: '',

      obra_EngenheiroSelecionado: [],

      medicao_Id: 0,
      medicao_DataPrevista: null,
      medicao_ValorPrevisto: 0.0,
      medicao_DataMedicao: null,
      medicao_ValorMedido: 0.0,
      medicao_DataRecebimento: null,
      medicao_NumeroNotaFiscal: '',
      controleCusto_Id: 0,
      controleCusto_DataPrevista: null,
      controleCusto_ValorPrevisto: 0.0,
      controleCusto_ValorMedido: 0.0,

      modalInformacoes_Exibir: false,
      modalMedicao_Exibir: false,
      modalMedicao_Titulo: 'Nova medição extra',
      modalMedicao_ValorPrevistoAjustado: 0.0,
      modalMedicao_DataPrevista: null,
      modalMedicao_DataPrevistaRecebimento: null,


      modalETO_Exibir: false,
      modalETO_Titulo: 'Novo ETO extra',
      modalETO_ValorPrevistoAjustado: 0.0,
      modalETO_DataPrevista: null,

      modalMedicaoEdicao_Exibir: false,
      modalMedicaoEdicao_Titulo: 'Editar medição',
      modalMedicaoEdicao_ValorPrevistoAjustado: 0.0,
      modalMedicaoEdicao_DataPrevista: null,
      modalMedicaoEdicao_DataRecebimentoPrevista: null,
      modalMedicaoEdicao_Medicao: null,

      modalETOEdicao_Exibir: false,
      modalETOEdicao_Titulo: 'Editar ETO ',
      modalETOEdicao_ValorPrevistoAjustado: 0.0,
      modalETOEdicao_DataPrevista: null,
      modalETOEdicao_ETO: null,





      modalControleCusto_Exibir: false,
      modalControleCusto_Titulo: 'Novo controle de custo',
      modalControleCusto_ExibirBotaoCadastrar: true,
      modalControleCusto_ExibirBotaoEditar: false,
      modalControleCusto_ExibirCamposEdicao: false,
      modalControleCusto_DesabilitarCamposPrevistos: false,

      edicaoValorTotalMedicao: false,
      somatoriaValoresAjustadosMedicoes: 0.0,

      edicaoValorTotalETO: false,
      somatoriaValoresAjustadosETOs: 0.0,

      controle_ObraCadastrando: false,
      controle_ObraEditando: false,

      engenheiros: [],
      diretores: [],

      colunasMedicoes: [
        'dataPrevista',
        'dataPrevistaRecebimento',
        'valorPrevisto',
        'valorPrevistoAjustado',
        'valorFaturado',
        'acoes'
      ],

      opcoesMedicoes: {
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

      colunasControleCusto: [
        'dataPrevista',
        'valorPrevisto',
        'valorPrevistoAjustado',
        'valorMedido',
        'acoes'
      ],

      opcoesControleCusto: {
        perPage: 1000,
        filterable: false,
        headings: {
          dataPrevista: 'Dt. Prevista',
          valorPrevisto: 'Previsto (R$)',
          valorPrevistoAjustado: 'Previsto Ajustado (R$)',
          valorMedido: 'Medido (R$)',
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
    };
  },
  methods: {
    formataDataSemHora: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY");
    },

    multiselectEngenheiros({ nome }) {
      return `${nome}`;
    },

    multiselectDiretores({ nome }) {
      return `${nome}`;
    },

    calculaPercentualSinal() {
      if (this.obra_ValorTotal <= 0)
        this.obra_PercentualSinal = 0;
      else
        this.obra_PercentualSinal = (this.obra_ValorSinal * 100) / this.obra_ValorTotal;
    },

    calculaPrazoDias() {
      if (this.obra_DataInicio != null && this.obra_DataFim != null) {
        this.obra_PrazoDias =
          (this.obra_DataFim - this.obra_DataInicio) / 86400000;

        if (this.obra_PrazoDias < 0) {
          this.obra_DataInicio = this.obra_DataFim;
          this.obra_PrazoDias = 0;

          this.$swal(
            'A data de início da obra deve ser anterior a data de finalização da obra',
            '',
            'error'
          );
        }
      }
    },

    calculaDataRecebimento(){
      this.modalMedicao_DataPrevistaRecebimento = new Date( new Date(this.modalMedicao_DataPrevista).setDate(new Date(this.modalMedicao_DataPrevista).getDate() + this.obra.diasDePagamento));
    },

    verificaValoresInformados() {
      if (parseFloat(this.obra_ValorTotal) < 0) {
        this.$swal('Não são permitidos valores negativos', '', 'error');
        this.obra_ValorTotal = 0.0;
      } else if (parseFloat(this.obra_ValorCusto) < 0) {
        this.$swal('Não são permitidos valores negativos', '', 'error');
        this.obra_ValorCusto = 0.0;
      } else if (parseFloat(this.obra_ValorNaoComissionado) < 0) {
        this.$swal('Não são permitidos valores negativos', '', 'error');
        this.obra_ValorNaoComissionado = 0.0;
      }

      if (parseFloat(this.obra_ValorCusto) > parseFloat(this.obra_ValorTotal)) {
        this.$swal(
          'Valor de custo da obra não pode ser maior que o valor total da obra',
          '',
          'error'
        );
        this.obra_ValorCusto = this.obra_ValorTotal;
      }
    },

    editarObra() {
      let validado = true;

      if (this.obra_EngenheiroSelecionado == null) {
        validado = false;
        this.$swal(
          'Informe um engenheiro responsável para a obra',
          '',
          'error'
        );
      }

      if (this.obra_Endereco == '') {
        validado = false;
        this.$swal('Informe o endereço da obra', '', 'error');
      }

      if (this.obra_DataInicio == null) {
        validado = false;
        this.$swal('Informe a data de início da obra', '', 'error');
      }

      if (this.obra_DataFim == null) {
        validado = false;
        this.$swal('Informe a data de finalização da obra', '', 'error');
      }

      if (this.obra_DiretorAprovador == null) {
        validado = false;
        this.$swal("Informe um diretor aprovador", "", "error");
      }

      var usuariosAprovadores = [];
      this.obra_EngenheiroSelecionado.forEach(x => {
        usuariosAprovadores.push({ idUsuarioAprovacao: x.id });
      });

      if (validado) {
        this.obra.usuariosAprovadores = usuariosAprovadores;
        this.obra.enderecoObra = this.obra_Endereco;
        this.obra.diasDePagamento = this.obra_DiasDePagamento;
        this.obra.numeroPedidoCliente = this.obra_NumeroPedidoCliente;
        this.obra.descricao = this.obra_Descricao;
        this.obra.dataInicio = this.obra_DataInicio;
        this.obra.dataFim = this.obra_DataFim;
        this.obra.prazoDias = this.obra_PrazoDias;
        this.obra.valorTotal = this.obra_ValorTotal;
        this.obra.valorCusto = this.obra_ValorCusto;
        this.obra.valorNaoComissionado = this.obra_ValorNaoComissionado;
        this.obra.valorMaterial = this.obra_ValorMaterial,
          this.obra.idUsuarioDiretorAprovador = this.obra_DiretorAprovador.id;
        this.obra.aliquotaImpostoISS = parseFloat(this.obra_AliquotaImpostoISS);
        this.obra.aliquotaImpostoINSS = parseFloat(this.obra_AliquotaImpostoINSS);
        this.obra.aliquotaImpostoIR = parseFloat(this.obra_AliquotaImpostoIR);
        this.obra.aliquotaImpostoArt30 = parseFloat(this.obra_AliquotaImpostoArt30);
        this.obra.valorSinal = this.obra_ValorSinal;
        this.obra.dataRecebimentoSinal = this.obra_DataRecebimentoSinal;
        this.obra.percentualEquivalenteSinal = this.obra_PercentualSinal;

        ApiService.put('Obra', this.obra, (result) => {
          if (result.status != 201) {
            this.$swal('Erro ao editar obra', result.message, 'error');
          } else {
            this.modalInformacoes_Exibir = false;
            this.obtemObra();
          }
        });
      }
    },

    downloadContaCorrenteExcel() {
      this.isLoading = true;

      ApiService.downloadContaCorrenteExcel(this.obra_Id, (result) => {
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

    downloadContaCorrentePDF() {
      this.isLoading = true;

      ApiService.downloadContaCorrentePDF(this.obra_Id, (result) => {
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





    obtemObra: function () {
      ApiService.get('Obra', this.obra_Id, (result) => {
        if (result.status != 200) {
          this.$swal('', result.message, 'error');
        } else {
          this.obra = result.data;
          this.obra.dataInicioObraFormatada = moment(
            String(new Date(this.obra.dataInicio))
          ).format('DD/MM/YYYY');
          this.obra.dataFimObraFormatada = moment(
            String(new Date(this.obra.dataFim))
          ).format('DD/MM/YYYY');

          this.obra.usuariosAprovadores.forEach(x => {
            this.obra_EngenheiroSelecionado.push(x.usuario);
          });

          this.obra_ValorCusto = this.obra.valorCusto;
          this.obra_ValorTotal = this.obra.valorTotal;
          this.obra_ValorTotalAjustado = this.obra.valorTotalAjustado;
          this.obra_ValorCustoAjustado = this.obra.valorCustoAjustado;
          this.obra_ValorNaoComissionado = this.obra.valorNaoComissionado;
          this.obra_DataInicio = new Date(this.obra.dataInicio);
          this.obra_DataFim = new Date(this.obra.dataFim);
          this.obra_PrazoDias = this.obra.prazoDias;
          this.obra_Descricao = this.obra.descricao;
          this.obra_Endereco = this.obra.enderecoObra;
          this.obra_DiasDePagamento = this.obra.diasDePagamento;
          this.obra_NumeroPedidoCliente = this.obra.numeroPedidoCliente;
          this.obra_AliquotaImpostoISS = this.obra.aliquotaImpostoISS;
          this.obra_AliquotaImpostoINSS = this.obra.aliquotaImpostoINSS;
          this.obra_AliquotaImpostoIR = this.obra.aliquotaImpostoIR;
          this.obra_AliquotaImpostoArt30 = this.obra.aliquotaImpostoArt30;
          this.obra_ValorSinal = this.obra.valorSinal;
          this.obra_PercentualSinal = this.obra.percentualEquivalenteSinal;
          this.obra_DataRecebimentoSinal = this.obra.dataRecebimentoSinal;
          this.obra_DiretorAprovador = this.obra.diretorAprovador;

          var count = 0;
          this.obra.medicoes.forEach(x => {
            x.idInterno = count;
            count++;
          });


          this.obtemSomatoriaMedicoes();
          this.obtemSomatoriaETOs();
        }
      });
    },

    excluiControleCusto: function (id) {

      this.isLoading = true;

      ApiService.delete('ObraControleCusto', id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal('', result.message, 'error');
        } else {
          this.obtemObra();
        }
      });

    },

    listaEngenheiros: function () {
      this.engenheiros = [];

      ApiService.getEngenheiros((result) => {
        if (result.status != 200) {
          this.$swal('', result.message, 'error');
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
        idInterno: this.obra.medicoes.length,
        idObra: this.obra.id,
        dataPrevista: this.modalMedicao_DataPrevista,
        dataPrevistaRecebimento: parseFloat(this.modalMedicao_DataPrevistaRecebimento),
        valorPrevistoAjustado: parseFloat(this.modalMedicao_ValorPrevistoAjustado),
        valorFaturado: 0
      };

      this.obra.medicoes.push(objetoMedicao);

      this.obtemSomatoriaMedicoes();

      this.modalMedicao_Exibir = false;
    },

    excluiMedicao(medicao) {
      var listaMedicoes = [];

      this.obra.medicoes.forEach(x => {
        if (x.idInterno != medicao.idInterno) {
          x.idInterno = listaMedicoes.length;
          listaMedicoes.push(x);
        }
      });

      this.obra.medicoes = listaMedicoes;

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

      this.modalMedicaoEdicao_Medicao.valorPrevistoAjustado = parseFloat(this.modalMedicaoEdicao_ValorPrevistoAjustado);


      this.obra.medicoes.forEach(x => {
        if (x.id == this.modalMedicaoEdicao_Medicao.id) {
          x.valorPrevistoAjustado = parseFloat(this.modalMedicaoEdicao_Medicao.valorPrevistoAjustado);
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

    salvarMedicoes() {

      if (this.obtemSomatoriaMedicoes() != this.obra_ValorTotalAjustado) {
        this.$swal('A soma dos valores ajustados é diferente do valor total ajustado informado', '', 'error');
        return;
      }

      this.isLoading = true;

      ApiService.post('ObraMedicao', this.obra.medicoes, (result) => {

        this.isLoading = false;

        if (result.status != 200) {
          this.$swal('Erro ao salvar medições', result.message, 'error');
        } else {
          this.edicaoValorTotalMedicao = false
          this.obtemObra();

        }
      });



    },

    obtemSomatoriaMedicoes() {

      var result = 0;
      this.obra.medicoes.forEach(x => {
        result += parseFloat(x.valorPrevistoAjustado);
      });

      this.somatoriaValoresAjustadosMedicoes = result;

      return result;
    },

    cadastrarETOExtra() {
      if (this.modalETO_DataPrevista == null) {
        this.$swal("Data da medição inválida", "", "error");
        return;
      }

      if (this.modalETO_ValorPrevistoAjustado <= 0) {
        this.$swal("Valor da medição inválido", "", "error");
        return;
      }

      var objetoETO = {
        id: 0,
        idInterno: this.obra.medicoes.length,
        idObra: this.obra.id,
        dataPrevista: this.modalETO_DataPrevista,
        valorPrevistoAjustado: parseFloat(this.modalETO_ValorPrevistoAjustado),
        valorMedido: 0
      };

      this.obra.controleCusto.push(objetoETO);

      this.obtemSomatoriaETOs();

      this.modalETO_Exibir = false;
    },

    excluiETO(ETO) {
      var listaETOs = [];

      this.obra.controleCusto.forEach(x => {
        if (x.idInterno != ETO.idInterno) {
          x.idInterno = listaETOs.length;
          listaETOs.push(x);
        }
      });

      this.obra.controleCusto = listaETOs;

      this.obtemSomatoriaETOs();
    },

    abreModalEdicaoETO(ETO) {
      this.modalETOEdicao_ETO = ETO;
      this.modalETOEdicao_DataPrevista = ETO.dataPrevista;
      this.modalETOEdicao_ValorPrevistoAjustado = ETO.valorPrevistoAjustado;
      this.modalETOEdicao_Titulo = 'Editar Medição ' + this.formataDataSemHora(ETO.dataPrevista);
      this.modalETOEdicao_Exibir = true;
    },

    editarETO() {

      if (this.modalETOEdicao_ValorPrevistoAjustado < 0) {
        validado = false;
        this.$swal("Valor do ETO inválido", "", "error");
      }

      this.modalETOEdicao_ETO.valorPrevistoAjustado = parseFloat(this.modalETOEdicao_ValorPrevistoAjustado);

      this.obra.controleCusto.forEach(x => {
        if (x.id == this.modalETOEdicao_ETO.id)
          x.valorPrevistoAjustado = parseFloat(this.modalETOEdicao_ETO.valorPrevistoAjustado);
      });

      this.modalETOEdicao_Exibir = false;

      this.obtemSomatoriaETOs();
    },

    salvarETOs() {

      if (this.obtemSomatoriaETOs() != this.obra_ValorCustoAjustado) {
        this.$swal('A soma dos valores ajustados é diferente do valor total ajustado informado', '', 'error');
        return;
      }

      this.isLoading = true;

      ApiService.post('ObraControleCusto', this.obra.controleCusto, (result) => {

        this.isLoading = false;

        if (result.status != 200) {
          this.$swal('Erro ao salvar ETOs', result.message, 'error');
        } else {
          this.obtemObra();
          this.edicaoValorTotalETO=false;
        }
      });
    },

    obtemSomatoriaETOs() {

      var result = 0;
      this.obra.controleCusto.forEach(x => {
        result += parseFloat(x.valorPrevistoAjustado);
      });

      this.somatoriaValoresAjustadosETOs = result;

      return result;
    },






    cadastraControleCusto() {
      let validado = true;

      if (this.controleCusto_DataPrevista == null) {
        validado = false;
        this.$swal(
          'Informe uma data prevista para o controle de custo',
          '',
          'error'
        );
      }

      if (this.controleCusto_ValorPrevisto < 0) {
        validado = false;
        this.$swal(
          'Valor previsto deve ser maior ou igual a zero',
          '',
          'error'
        );
      }

      if (validado) {
        let objetoControleCusto = {
          Id: 0,
          IdObra: this.obra.id,
          DataPrevista: this.controleCusto_DataPrevista,
          ValorPrevisto: this.controleCusto_ValorPrevisto,
        };

        ApiService.post('ObraControleCusto', objetoControleCusto, (result) => {
          if (result.status != 201) {
            this.$swal(
              'Erro ao cadastrar controle de custo',
              result.data,
              'error'
            );
          } else {
            this.obtemObra();
            this.modalControleCusto_Exibir = false;
          }
        });
      }
    },

    editarControleCusto() {
      let objetoControleCusto = {
        Id: this.controleCusto_Id,
        IdObra: this.obra.id,
        ValorMedido: this.controleCusto_ValorMedido,
      };

      ApiService.put('ObraControleCusto', objetoControleCusto, (result) => {
        if (result.status != 201) {
          this.$swal(
            'Erro ao editar controle de custo',
            result.message,
            'error'
          );
        } else {
          this.obtemObra();
          this.modalControleCusto_Exibir = false;
        }
      });
    },
  },
  created() {
    this.obra_Id = this.$route.params.id;
  },
  mounted() {
    this.obtemObra();
    this.listaEngenheiros();
    this.listaDiretores();
  },
};
</script>

<style src="./ObraDetalhe.scss" lang="scss" />
