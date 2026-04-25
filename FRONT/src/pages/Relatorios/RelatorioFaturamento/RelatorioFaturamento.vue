<template>
  <div class="relatorio-faturamento-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Faturamento
    </h1>

    <b-modal :no-close-on-backdrop="true" id="modalDataFaturamento" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalDataFaturamento_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Faturamento</span>
          <h3 class="afm-hero__title">Data do faturamento</h3>
          <p class="afm-hero__description">Informe a data em que o faturamento foi recebido.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data Faturamento</label>
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalDataFaturamento_DataFaturamento"
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
            <b-button @click="modalDataFaturamento_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="informarRecebimento()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Informar recebimento
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalFaturamento" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalFaturamento_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Faturamento</span>
          <h3 class="afm-hero__title">{{ modalFaturamento_Titulo }}</h3>
          <p class="afm-hero__description">Consulte e gerencie faturamentos desta obra.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">


        <Widget customHeader class="estiloWidget">

          <b-row>
            <b-col md="6">
              <label class="mr-3">Valor Bruto:</label><br />
              <Money v-model="modalFaturamento_ValorBruto" v-bind="money" ref="valorBruto"
                @keydown.native.tab="funcaoValorBruto" ></Money>
            </b-col>
            <b-col md="6">
              <div v-if="!modalFaturamento_Edicao">
                <label class="mr-3">Obra:</label><br />
                <multiselect v-model="modalFaturamento_ObraSelecionada" :multiple="false"
                  :options="modalFaturamento_Obras" select-label="Selecionar" placeholder="Selecione uma Obra"
                  label="codigo" track-by="codigo" @input="obraSelecionada()">
                </multiselect>
              </div>
            </b-col>

          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">INSS:</label><br />
              <Money v-model="modalFaturamento_ValorINSS" v-bind="money" ref="valorINSS"
                @keydown.native.tab="calculaValorLiquido" ></Money>
            </b-col>
            <b-col md="6">
              <label class="mr-3">ISS:</label><br />
              <Money v-model="modalFaturamento_ValorISS" v-bind="money" ref="valorISS"
                @keydown.native.tab="calculaValorLiquido" ></Money>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">IR:</label><br />
              <Money v-model="modalFaturamento_ValorIR" v-bind="money" ref="valorIR"
                @keydown.native.tab="calculaValorLiquido" ></Money>
            </b-col>
            <b-col md="6">
              <label class="mr-3">Art 30:</label><br />
              <Money v-model="modalFaturamento_ValorArt30" v-bind="money" ref="valorArt30"
                @keydown.native.tab="calculaValorLiquido" ></Money>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">Material:</label><br />
              <Money v-model="modalFaturamento_Material" v-bind="money" ></Money>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">Desconto:</label><br />
              <Money v-model="modalFaturamento_ValorDesconto" v-bind="money" ref="valorDesconto"
                @keydown.native.tab="calculaValorLiquido" ></Money>
            </b-col>
            <b-col md="6">
              <label class="mr-3">Desconto Sinal:</label><br />
              <Money v-model="modalFaturamento_ValorDescontoSinal" v-bind="money" ></Money>
            </b-col>
          </b-row>

          <br />
          <b-row>
            <b-col md="6">
              <label class="mr-3">Valor Liquido:</label><br />
              <Money v-model="modalFaturamento_ValorLiquido" v-bind="money" ref="valorLiquido" :disabled="true"
                :style="{ 'color': 'white', 'background-color': 'transparent', 'border-width': '0px', }"></Money>
            </b-col>
            <b-col md="6">
              <label class="mr-3">Valor Liquido Sinal:</label><br />
              <Money v-model="modalFaturamento_ValorLiquidoSinal" v-bind="money" ref="valorLiquido" :disabled="true"
                :style="{ 'color': 'white', 'background-color': 'transparent', 'border-width': '0px', }"></Money>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">Data Faturamento:</label><br />
              <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalFaturamento_DataFaturamento" format="dd/MM/yyyy"
                type="date" :open.sync="open">
              </DatePickerMask>
            </b-col>
            <b-col md="6">
              <label class="mr-3">Data Recebimento:</label><br />
              <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalFaturamento_DataRecebimento" format="dd/MM/yyyy"
                type="date" :open.sync="open">
              </DatePickerMask>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col md="6">
              <label class="mr-3">Número NF:</label>
              <b-form-input v-model="modalFaturamento_NumeroNF" style="color: white"></b-form-input>
            </b-col>
          </b-row>

          <br />

          <b-row>
            <b-col>
              <label class="mr-3">Observação:</label><br />
              <textarea-autosize id="textarea" v-model="modalFaturamento_Observacao" class="form-control"
                :min-height="135" style="color:white" />
            </b-col>
          </b-row>
        </Widget>

        <Widget customHeader class="estiloWidget" v-if="!modalFaturamento_Edicao">
          <b-row>
            <b-col>
              <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input" @change="onChange"
                ref="file" multiple="true" />
              <br />
              Arquivos anexados: {{ this.upload_Files.length }} arquivo(s)<br /><br />
              <ul id="example-2">
                <li v-for="item in this.upload_Files" v-bind:key="item.name">
                  {{ item.name }}
                </li>
              </ul>
              <label type="button" class="btn width-75 mb-3 mr-3 bg-success" for="fileInput">
                Escolher Arquivos
              </label>

              <b-button v-on:click="removerArquivos()" v-if="this.upload_Files.length > 0" variant="danger"
                class="width-75 mb-3 mr-3">
                <span>Remover Arquivos</span>
              </b-button>
            </b-col>
          </b-row>
        </Widget>
              </section>
      </b-container>


      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalFaturamento_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalArquivos_Exibir" size="lg">

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

    <b-modal :no-close-on-backdrop="true" id="modalDetalheObra" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalDetalheObra_Exibir" size="xl">

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
              <label class="mr-3">Cidade: {{ this.filtro_ObraSelecionada != null ? this.filtro_ObraSelecionada.cidade.nome
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
              }).format(this.filtro_ObraSelecionada.valorCusto) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor Material: {{ this.filtro_ObraSelecionada != null ? new Intl.NumberFormat("pt-BR",
                { style: "currency", currency: "BRL" }).format(this.filtro_ObraSelecionada.valorMaterial) : '' }}</label>
            </b-col>
          </b-row>
          <b-row>
            <b-col md="12">
              <label class="mr-3">Valor não Comissionado: {{ this.filtro_ObraSelecionada != null ? new
                Intl.NumberFormat("pt-BR", {
                  style: "currency", currency: "BRL"
                }).format(this.filtro_ObraSelecionada.valorNaoComissionado) : '' }}</label>
            </b-col>
          </b-row>
        </Widget>

        <Widget customHeader class="estiloWidget"
          v-if="this.filtro_ObraSelecionada != null && this.filtro_ObraSelecionada.medicoes != null">
          <h2>Medições</h2><br />
          <b-row>
            <b-col md="12">
              <ul id="example-2">
                <li v-for="item in this.filtro_ObraSelecionada.medicoes" v-bind:key="item.name">
                  Data Prevista: {{ formataDataSemHora(item.dataPrevista) }} - Valor Previsto: {{ new
                    Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(item.valorPrevisto) }}
                </li>
              </ul>
            </b-col>
          </b-row>

          <br />

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
          <!-- <b-row>
            <b-col md="12">
              Saldo a faturar: INFORMAR VALOR
            </b-col>
          </b-row> -->
        </Widget>

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

              <div class="table-card table-card--fluid">
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

    <b-modal :no-close-on-backdrop="true" id="modalDetalheCliente" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalDetalheCliente_Exibir" size="lg">

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
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalMedicao_Exibir" size="lg">

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
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevista" format="dd/MM/yyyy"
                  type="date" :open.sync="open" @change="calculaDataRecebimento()">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Data prevista recebimento</label>
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalMedicao_DataPrevistaRecebimento"
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
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalMedicaoEdicao_DataRecebimentoPrevista"
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

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col md="4">
          <b-row>
            <b-col md="12">
              <label class="mr-3">Obra:</label><br />
              <multiselect v-model="filtro_ObraSelecionada" :multiple="false" :options="filtro_Obras"
                select-label="Selecionar" placeholder="Selecione uma Obra" label="codigo" track-by="codigo">
              </multiselect>
            </b-col>
            <!-- <b-col md="2" style="vertical-align: bottom; height: 100%;">
              <label class="mr-3">&nbsp;</label><br />
              <button type="button" class="btn width-75 mb-3 mr-3 btn-info" @click="exibirModalDetalheObra()">
                <i class="fa fa-info" title="Detalhes"></i>
              </button>
            </b-col> -->
          </b-row>


        </b-col>
        <b-col md="4">
          <b-row>
            <b-col md="12">
              <label class="mr-3">Cliente:</label><br />
              <multiselect v-model="filtro_ClienteSelecionado" :multiple="false" :options="filtro_Clientes"
                select-label="Selecionar" placeholder="Selecione um Cliente" label="nomeFantasia" track-by="nomeFantasia">
              </multiselect>
            </b-col>
            <!-- <b-col md="2">
              <label class="mr-3">&nbsp;</label><br />
              <button type="button" class="btn width-75 mb-3 mr-3 btn-info" @click="exibirModalDetalheCliente()">
                <i class="fa fa-info" title="Detalhes"></i>
              </button>
            </b-col> -->
          </b-row>

        </b-col>
        <b-col md="4">
          <label class="mr-3">Número NF:</label>
          <b-form-input v-model="filtro_NumeroNF" style="color: white"></b-form-input>
        </b-col>
      </b-row>

      <br />

      <b-row>
        <b-col md="2">
          <label class="mr-3">Data Faturamento Inicial:</label><br />

          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFaturamentoInicial" format="dd/MM/yyyy"
            type="date">
          </DatePickerMask>
        </b-col>
        <b-col md="2">
          <label class="mr-3">Data Faturamento Final:</label><br />
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataFaturamentoFinal" format="dd/MM/yyyy"
            type="date">
          </DatePickerMask>
        </b-col>
        <b-col md="2">
          <label class="mr-3">Data Recebimento Inicial:</label><br />

          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataRecebimentoInicial" format="dd/MM/yyyy"
            type="date">
          </DatePickerMask>
        </b-col>
        <b-col md="2">
          <label class="mr-3">Data Recebimento Final:</label><br />
          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="filtro_DataRecebimentoFinal" format="dd/MM/yyyy"
            type="date">
          </DatePickerMask>
        </b-col>
        <b-col md="4">
          <label class="mr-3">Observação:</label>
          <b-form-input v-model="filtro_Observacao" style="color: white"></b-form-input>
        </b-col>
      </b-row>

      <br />

      <b-row>
        <b-col md="4">
          <label class="mr-3">Status:</label><br />
          <multiselect v-model="filtro_StatusFaturamentoSelecionado" :multiple="false" :options="filtro_StatusFaturamento"
            select-label="Selecionar" placeholder="Selecione um Status" label="descricao" track-by="descricao">
          </multiselect>
        </b-col>
        <b-col md="4"></b-col>
        <b-col md="4" style="text-align: right;">
          <button @click="downloadExcel()" v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-outline-success">
            Download Excel
          </button>
          <button @click="limpaFiltros()" v-b-modal.cadastro type="button" class="btn width-120 mb-3 mr-4 btn-danger">
            Limpar Filtros
          </button>
          <button @click="listaFaturamentos()" v-b-modal.cadastro type="button"
            class="btn width-120 mb-3 mr-4 btn-success">
            Pesquisar
          </button>
        </b-col>
      </b-row>
    </Widget>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col>
          <label class="mr-3">A faturar previsto período: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency: "BRL"
          }).format(this.somaAFaturar) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <label class="mr-3">Valor faturado a receber: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.somaFaturadoAReceber) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <label class="mr-3">Valor faturado recebido: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.somaFaturadoRecebido) }}</label>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <label class="mr-3">Saldo a Faturar: {{ new Intl.NumberFormat("pt-BR", {
            style: "currency", currency:
              "BRL"
          }).format(this.saldoAFaturar) }}</label>
        </b-col>
      </b-row>
    </Widget>


    <Widget customHeader class="estiloWidget" v-if="medicoes.length > 0">
      <b-row>
        <b-col>
          <h2>A Faturar</h2>
        </b-col>
      </b-row>
      <b-row>
        <b-col lg="12">
          <div class="table-card">
            <table class="estilo-tabela tabela-identidade">
              <thead>
                <tr>
                  <th class="estilo-cabecalho texto-centro">Obra</th>
                  <th class="estilo-cabecalho texto-centro">Data Prevista</th>
                  <th class="estilo-cabecalho texto-centro">Valor Previsto</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in medicoes" :key="'med-' + index">
                  <td class="estilo-celula texto-centro">{{ row.obraDaMedicao ? row.obraDaMedicao.codigo : '' }}</td>
                  <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPrevista) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPrevistoAjustado) }}</td>
                </tr>
                <tr v-if="!medicoes || medicoes.length === 0">
                  <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
                </tr>
              </tbody>
            </table>
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
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0">
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
            class="btn width-120 mb-3 mr-4 btn-outline-success">
            Entrada Faturamento
          </button>
        </b-col>
      </b-row>

      <b-row>
        <b-col lg="12">
          <div class="table-card">
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
                  <th class="estilo-cabecalho texto-centro">Art. 30</th>
                  <th class="estilo-cabecalho texto-centro">Desconto</th>
                  <th class="estilo-cabecalho texto-centro">Val. Liquido</th>
                  <th class="estilo-cabecalho texto-centro">DT. RECEB. PREV.</th>
                  <th class="estilo-cabecalho texto-centro">Dt. Fat. Real.</th>
                  <th class="estilo-cabecalho texto-centro">Dt. Recebimento</th>
                  <th class="estilo-cabecalho">Obs.</th>
                  <th class="estilo-cabecalho texto-centro">Status</th>
                  <th class="estilo-cabecalho texto-centro">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in faturamentos" :key="'fat-' + (row.id || index)">
                  <td class="estilo-celula texto-centro">{{ row.obra ? row.obra.codigo : '' }}</td>
                  <td class="estilo-celula">{{ (row.obra && row.obra.cliente) ? row.obra.cliente.nomeFantasia : '' }}</td>
                  <td class="estilo-celula texto-centro">{{ row.numeroNF }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorBruto) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorINSS) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorISS) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorIR) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorArt30) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorDesconto) }}</td>
                  <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorLiquido) }}</td>
                  <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataRecebimentoPrevisto) }}</td>
                  <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataFaturamento) }}</td>
                  <td class="estilo-celula texto-centro">{{ row.dataRecebimentoRealizado != null ? formataDataSemHora(row.dataRecebimentoRealizado) : '' }}</td>
                  <td class="estilo-celula">{{ row.observacao }}</td>
                  <td class="estilo-celula texto-centro">{{ row.status ? row.status.descricao : '' }}</td>
                  <td class="estilo-celula texto-centro">
                    <button
                      type="button"
                      class="btn btn-info"
                      @click="abrirModalAcoesFaturamento(row)"
                    >
                      Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                    </button>
                  </td>
                </tr>
                <tr v-if="!faturamentos || faturamentos.length === 0">
                  <td class="estilo-celula texto-centro" colspan="16">Nenhum registro encontrado</td>
                </tr>
              </tbody>
            </table>
          </div>
        </b-col>
      </b-row>
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
  name: 'RelatorioFaturamento',
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

      open: false,
      isLoading: false,

      usuarioLogado: null,

      modalDataFaturamento_Exibir: false,
      modalDataFaturamento_DataFaturamento: null,

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
      saldoAFaturar: 0.0,

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
                { key: 'valorLiquido', label: 'VAL. LIQUIDO' },
                { key: 'dataFaturamentoPrevisto', label: 'DT. RECEB. PREV.' },
                { key: 'dataFaturamento', label: 'Dt. Fat. Real' },
                { key: 'dataRecebimento', label: 'Dt. Recebimento' },
                { key: 'observacao', label: 'OBS' },
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

      colunasMedicoes: ["obra", "dataPrevista", "valorPrevisto"],

      opcoesMedicoes: {
        perPage: 10,
        headings: {
          obra: "Obra",
          dataPrevista: "Data Prevista",
          valorPrevisto: "Valor Previsto",
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
  methods: {
    // ============================================================
    // Ações da tabela (usadas pelo <ModalAcoes>).
    // ============================================================
    montaAcoesFaturamento(row) {
      return [
        {
          label: "Reverter Recebimento",
          descricao: "Desfazer o recebimento já confirmado",
          icone: "reply",
          variante: "warning",
          onClick: () => this.reverterRecebimento(row),
          visible: row.idStatusFaturamento == 1,
        },
        {
          label: "Informar Recebimento",
          descricao: "Registrar que o pagamento foi recebido",
          icone: "thumbs-up",
          variante: "success",
          onClick: () => this.abrirModalDataFaturamento(row),
          visible: row.idStatusFaturamento == 2,
        },
        {
          label: "Cancelar",
          descricao: "Cancelar este faturamento",
          icone: "ban",
          variante: "danger",
          onClick: () => this.cancelarFaturamento(row),
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

    downloadExcel: function () {
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

      ApiService.downloadRelatorioFaturamento(objParametros, (result) => {
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

    calculaDataRecebimento(){
      this.modalMedicao_DataPrevistaRecebimento = new Date( new Date(this.modalMedicao_DataPrevista).setDate(new Date(this.modalMedicao_DataPrevista).getDate() + this.filtro_ObraSelecionada.diasDePagamento));
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
        valorPrevistoAjustado: this.modalMedicao_ValorPrevistoAjustado,
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
      this.filtro_StatusFaturamentoSelecionado = { id: 2, descricao: 'Faturado a Receber' };
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

          this.modalDetalheObra_Titulo = 'Detalhe da obra ' + this.filtro_ObraSelecionada.codigo;
          this.filtro_ObraSelecionada.valorAFaturar = 0.0;

          var count =0;

          this.filtro_ObraSelecionada.medicoes.forEach(x => {
            this.filtro_ObraSelecionada.valorAFaturar = this.filtro_ObraSelecionada.valorAFaturar + x.valorPrevisto;
            x.idInterno = count;
            count++;
          });

          this.filtro_ObraSelecionada.faturamentos.forEach(x => {
            this.filtro_ObraSelecionada.valorAFaturar = this.filtro_ObraSelecionada.valorAFaturar - x.valorBruto;
          });

          this.obra_ValorTotalAjustado = this.filtro_ObraSelecionada.valorTotalAjustado;

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

    cancelarFaturamento: function (faturamentoDTO) {
      this.isLoading = true;

      faturamentoDTO.idStatusFaturamento = 3;

      ApiService.put('Faturamento', faturamentoDTO, (result) => {

        this.isLoading = false;

        if (result.status != 200) {

          this.$swal("Erro ao editar faturamento", result.data, "error");
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

    abrirModalDataFaturamento: function (faturamentoDTO) {
      this.modalDataFaturamento_Faturamento = faturamentoDTO;
      this.modalDataFaturamento_Exibir = true;
    },

    informarRecebimento: function () {

      if (this.modalDataFaturamento_DataFaturamento == null) {
        this.$swal("Data inválida", '', "error");
        return;
      }

      this.isLoading = true;

      this.modalDataFaturamento_Faturamento.dataRecebimentoRealizado = this.modalDataFaturamento_DataFaturamento;
      this.modalDataFaturamento_Faturamento.idStatusFaturamento = 1;

      ApiService.put('Faturamento', this.modalDataFaturamento_Faturamento, (result) => {

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

      console.log(this.modalFaturamento_ObraSelecionada);

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
          this.modalFaturamento_Obras = result.data;
        }
      });
    },

    listaObrasFiltro: function () {
      this.filtro_Obras = [];

      ApiService.getAll("Obra", false, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.filtro_Obras = result.data;
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

          console.log(result);

          this.faturamentos = result.data.faturamentosDTO;
          this.medicoes = result.data.medicoesDTO;
          this.somaAFaturar = result.data.somaAFaturar;
          this.somaFaturadoRecebido = result.data.somaFaturadoRecebido;
          this.somaFaturadoAReceber = result.data.somaFaturadoAReceber;
          this.saldoAFaturar = this.somaAFaturar - this.somaFaturadoAReceber - this.somaFaturadoRecebido;
        }
      });


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
      this.modalFaturamento_Faturamento.valorLiquido = this.modalFaturamento_ValorLiquido;


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
        valorLiquido: this.modalFaturamento_ValorLiquido
      };

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
        }
      });
    },
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

<style src="./RelatorioFaturamento.scss" lang="scss" />




//No caso de não relização das previsões, pegar o saldo restante, obter a média de dias entre os faturamentos e criar um novo faturamento. Qdo houver apenas 1 parcela, colocar para 15 dias.