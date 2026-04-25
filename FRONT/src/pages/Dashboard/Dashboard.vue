<template>
  <div class="dashboard-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">WH Engenharia &nbsp;</h1>

    <Widget customHeader class="estiloWidget">
      <b-row>
        <b-col cols="auto" class="mr-auto p-3"></b-col>
        <b-col cols="auto" style="padding: 0"> </b-col>

        <!-- =================== MODAL EDIÇÃO DATA NF =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalEdicaoDataNotaFiscal" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalEdicaoDataNotaFiscal_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Nota Fiscal</span>
              <h3 class="afm-hero__title">Edição de data de vencimento</h3>
              <p class="afm-hero__description">Ajuste as datas de vencimento das notas fiscais listadas abaixo.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalEdicaoDataNotaFiscal_Notas.length }}</span>
              <small>nota(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="modalEdicaoDataNotaFiscal_Notas.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Nome</th>
                        <th class="estilo-cabecalho texto-centro">Data de Vencimento</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(nota, index) in modalEdicaoDataNotaFiscal_Notas" :key="'edn-' + index">
                        <td class="estilo-celula texto-centro">{{ nota.nome }}</td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--date">
                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="nota.dataVencimento"
                              :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                            </DatePickerMask>
                          </div>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div>
                  <strong>Nenhuma nota fiscal encontrada</strong>
                  <p>Não há notas fiscais disponíveis para edição.</p>
                </div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total:</span>
                <strong>{{ modalEdicaoDataNotaFiscal_Notas.length }} nota(s)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="editarDataVencimentoNF()" variant="success" class="mb-0">Salvar Edição</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL TIPO NOVA SOLICITAÇÃO COMPRA =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalTipoNovaSolicitacaoCompra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalNovoPedidoCompraTipoPedido_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Nova Solicitação</span>
              <h3 class="afm-hero__title">Selecione o tipo de solicitação</h3>
              <p class="afm-hero__description">Escolha entre materiais ou serviço, ou retome um rascunho existente.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <div class="afm-tipo-grid">
                <button type="button" class="afm-tipo-btn" @click="abreModalNovaSolicitacao(false)">
                  <i class="fa fa-cube"></i>
                  <span>Materiais</span>
                </button>
                <button type="button" class="afm-tipo-btn" @click="abreModalNovaSolicitacao(true)">
                  <i class="fa fa-wrench"></i>
                  <span>Serviço</span>
                </button>
              </div>
            </section>

            <section class="afm-section-card">

              <div v-if="rascunhosSolicitacoes.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Data</th>
                        <th class="estilo-cabecalho">Título</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in rascunhosSolicitacoes" :key="'ras-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                        <td class="estilo-celula">{{ row.titulo }}</td>
                        <td class="estilo-celula texto-centro" style="width: 100px;">
                          <button type="button" @click="continuaRascunho(row)" class="btn btn-info afm-action-btn mr-1">
                            <i class="fa fa-pencil" title="Editar"></i>
                          </button>
                          <button type="button" @click="excluiRascunho(row.id)" class="btn btn-danger afm-action-btn">
                            <i class="fa fa-close" title="Excluir"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div>
                  <strong>Nenhum rascunho salvo</strong>
                  <p>Suas solicitações em rascunho aparecerão aqui.</p>
                </div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions"></div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL NOVA SOLICITAÇÃO SERVIÇO =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalNovoPedidoServico" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="lg" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalNovoPedidoServico_Exibir">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Solicitação de Compra</span>
              <h3 class="afm-hero__title">Nova solicitação de serviço</h3>
              <p class="afm-hero__description">Preencha as etapas abaixo para registrar a solicitação de serviço.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <form-wizard shape="tab" color="#3498db" title="" subtitle="" @on-change="updateProgress"
                @on-loading="updateProgress(1, 1)" style="font-size: 14px !important;">
                <b-progress class="progress-xs" variant="success" :value="progress" :max="4" />
                <b-button slot="prev" variant="default">
                  <i class="fa fa-caret-left" /> Anterior
                </b-button>
                <b-button slot="next" variant="primary">
                  Próximo <i class="fa fa-caret-right" />
                </b-button>
                <b-button slot="finish" variant="success" v-on:click="enviarSolicitacaoServico()">
                  Finalizar <i class="fa fa-check" />
                </b-button>

                <tab-content title="1. Informações Básicas">
                  <b-form>
                    <b-row>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Obra:</label><br />
                        <multiselect v-model="modalNovoPedidoServico_ObraSelecionada" :multiple="false" :options="obras"
                          :custom-label="descricaoObra" select-label="Selecionar" placeholder="Selecione uma obra"
                          label="descricao" track-by="descricao">
                        </multiselect>

                      </b-col>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Data de entrega necessária:</label><br />
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                          v-model="modalNovoPedidoServico_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
                        </DatePickerMask>
                      </b-col>
                    </b-row>

                    <br /><br />

                    <b-row>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Titulo da solicitação:</label><br />
                        <b-form-input v-model="modalNovoPedidoServico_Titulo"
                          style="color: white; background-color: black"></b-form-input>
                      </b-col>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Custo orçado:</label>
                        <Money v-model="modalNovoPedidoServico_CustoOrcado" v-bind="money" ></Money>
                      </b-col>
                    </b-row>
                  </b-form>
                </tab-content>
                <tab-content title="2. Observação">

                  <b-form>

                    <b-row>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Valor Total:</label>
                        <Money v-model="modalNovoPedidoServico_ValorTotal" v-bind="money" ></Money>
                      </b-col>
                    </b-row>


                    <br /><br />

                    <label class="mr-3" style="font-size: 16px !important;">Fornecimento conforme abaixo: </label>
                    <textarea-autosize id="textarea" v-model="modalNovoPedidoServico_Observacao" class="form-control"
                      :min-height="135" style="color:white; font-size: 18px;" />

                  </b-form>
                </tab-content>
                <tab-content title="3. Condição Pagamento">

                  <b-row>
                    <b-col>
                      <label>Comprador responsável: </label>
                      <multiselect v-model="modalNovoPedidoServico_CompradorSelecionado" :multiple="false"
                        :options="usuariosCompradores" select-label="Selecionar" placeholder="Selecione um comprador"
                        label="nome" track-by="nome">
                      </multiselect>
                    </b-col>
                  </b-row>

                  <br />

                  <b-row>
                    <b-col>
                      <label class="mr-3" style="font-size: 16px !important">Fornecedor: </label>
                      <multiselect v-model="modalNovoPedidoServico_FornecedorSelecionado" :multiple="false"
                        :options="fornecedores" select-label="Selecionar" placeholder="Selecione um fornecedor"
                        label="nomeFantasia" track-by="nomeFantasia">
                      </multiselect>
                    </b-col>
                  </b-row>

                  <br /><br />
                  <b-row>
                    <b-col>
                      <label class="mr-3" style="font-size: 16px !important">Condições de pagamento: </label>
                    </b-col>
                  </b-row>

                  <b-row>
                    <b-col md="4">
                      <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalNovoPedidoServico_DataPagamentoServico"
                        :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                      </DatePickerMask>

                    </b-col>
                    <b-col md="4">
                      <Money v-model="modalNovoPedidoServico_ValorPagamentoServico" v-bind="number"></Money>
                    </b-col>

                    <b-col md="4">
                      <b-button v-on:click="adicionaPagamentoServico()" variant="success" class="width-220 mb-3 mr-3">
                        <span>Adicionar Pagamento</span>
                      </b-button>
                    </b-col>
                  </b-row>

                  <div v-if="modalNovoPedidoServico_PagamentosServico.length > 0" style="margin-top:12px; overflow-x: visible;">
                    <div class="table-card table-card--fluid">
                      <table class="estilo-tabela tabela-identidade">
                        <thead>
                          <tr>
                            <th class="estilo-cabecalho texto-centro">Data</th>
                            <th class="estilo-cabecalho texto-centro">Valor</th>
                            <th class="estilo-cabecalho texto-centro"></th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="(pagamento, index) in modalNovoPedidoServico_PagamentosServico" :key="'pgs-' + (pagamento.id || index)">
                            <td class="estilo-celula texto-centro">
                              <div class="input-table input-table--date">
                                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.data"
                                  :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                                </DatePickerMask>
                              </div>
                            </td>
                            <td class="estilo-celula texto-centro">
                              <div class="input-table input-table--money">
                                <Money v-model="pagamento.valor" v-bind="number" style="width:100%;"></Money>
                              </div>
                            </td>
                            <td class="estilo-celula texto-centro">
                              <button type="button" @click="removePagamentoSolicitacaoServico(pagamento.id)"
                                class="btn btn-danger afm-remove-btn">
                                <i class="fa fa-trash"></i>
                              </button>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>

                </tab-content>
                <tab-content title="4. Arquivos">
                  <b-row>
                    <b-col>
                      <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input"
                        @change="onChange" ref="file" multiple="true" />
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
                </tab-content>
              </form-wizard>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <button type="button" @click="salvarRascunhoServico();" class="btn btn-warning mb-0">
                  <i class="fa fa-pencil" title="Salvar Rascunho"></i>&nbsp;Salvar Rascunho
                </button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL NOVA SOLICITAÇÃO MATERIAL =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalNovoPedidoMaterial" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="xl" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalNovoPedidoMaterial_Exibir">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Solicitação de Compra</span>
              <h3 class="afm-hero__title">Nova solicitação de material</h3>
              <p class="afm-hero__description">Preencha as etapas abaixo para registrar a solicitação de material.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <form-wizard shape="tab" color="#3498db" title="" subtitle="" @on-change="updateProgress"
                @on-loading="updateProgress(1, 1)" style="font-size: 14px !important;">
                <b-progress class="progress-xs" variant="success" :value="progress" :max="4" />
                <b-button slot="prev" variant="default">
                  <i class="fa fa-caret-left" /> Anterior
                </b-button>
                <b-button slot="next" variant="primary">
                  Próximo <i class="fa fa-caret-right" />
                </b-button>
                <b-button slot="finish" variant="success" v-on:click="enviarSolicitacaoMaterial()">
                  Finalizar <i class="fa fa-check" />
                </b-button>
                <tab-content title="1. Informações Básicas">
                  <b-form>
                    <b-row>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Obra:</label><br />
                        <multiselect v-model="modalNovoPedidoMaterial_ObraSelecionada" :multiple="false" :options="obras"
                          :custom-label="descricaoObra" select-label="Selecionar" placeholder="Selecione uma obra"
                          label="descricao" track-by="descricao">
                        </multiselect>

                      </b-col>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Data de entrega necessária:</label><br />
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                          v-model="modalNovoPedidoMaterial_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
                        </DatePickerMask>
                      </b-col>
                    </b-row>

                    <br /><br />

                    <b-row>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Titulo da solicitação:</label><br />
                        <b-form-input v-model="modalNovoPedidoMaterial_Titulo"
                          style="color: white; background-color: black"></b-form-input>
                      </b-col>
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Tipo valor:</label><br />
                        <toggle-button v-model="modalNovoPedidoMaterial_TipoValor"
                          :color="{ checked: '#2D8515', unchecked: '#4348F0', disabled: '#000000' }"
                          :labels="{ checked: 'Valor unitário', unchecked: 'Valor total' }" :width="130" :height="25"
                          :font-size="14" />
                      </b-col>
                    </b-row>
                  </b-form>
                </tab-content>

                <tab-content title="2. Materiais">

                  <b-row>
                    <b-col md="12">
                      <label>Material / Serviço: </label>
                      <multiselect v-model="modalNovoPedidoMaterial_MaterialSelecionado" :multiple="false"
                        :options="materiaisFiltradosPorTexto" select-label="Selecionar"
                        placeholder="Selecione um material" label="descricao" track-by="descricao"
                        :custom-label="descricaoMaterial" @search-change="buscaMateriaisComNomeInformado">
                      </multiselect>
                    </b-col>
                  </b-row>

                  <br />

                  <b-row>
                    <b-col md="6">
                      <label>Quantidade: </label>
                      <Money v-model="modalNovoPedidoMaterial_Quantidade" v-bind="number" ref="valorUnitario"
                        @keyup.native.enter="enterQuantidade"></Money>
                    </b-col>

                    <b-col md="6">
                      <label>Valor unitário: </label>
                      <Money v-model="modalNovoPedidoMaterial_ValorUnitario" v-bind="money" ref="valorUnitario"
                        :disabled="!this.modalNovoPedidoMaterial_TipoValor" @keyup.native.enter="enterValorUnitario">
                      </Money>
                    </b-col>
                  </b-row>

                  <br />

                  <b-row style="text-align: center">
                    <b-col>
                      <label></label>
                      <b-button v-on:click="adicionaMaterial()" variant="success" class="width-220 mb-3 mr-3">
                        <span>Adicionar à solicitação</span>
                      </b-button>
                      <b-button v-on:click="redirecionaCadastroMaterial()" variant="warning" class="width-220 mb-3 mr-3">
                        <span>Cadastrar novo</span>
                      </b-button>
                      <b-button v-on:click="listaMateriais()" variant="warning" class="width-220 mb-3 mr-3">
                        <span>Atualizar Lista</span>
                      </b-button>
                    </b-col>
                  </b-row>

                  <b-row>
                    <b-col>
                      <div class="table-card table-card--fluid">
                        <table class="estilo-tabela tabela-identidade">
                          <thead>
                            <tr>
                              <th class="estilo-cabecalho">Material</th>
                              <th class="estilo-cabecalho texto-centro">Qtd.</th>
                              <th class="estilo-cabecalho texto-centro">Valor Un.</th>
                              <th class="estilo-cabecalho texto-centro">Total</th>
                              <th class="estilo-cabecalho texto-centro"></th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr v-for="(row, index) in materiaisAdicionados" :key="'matad-' + (row.id || index)">
                              <td class="estilo-celula">{{ row.material }}</td>
                              <td class="estilo-celula texto-centro">
                                <div class="input-table input-table--money">
                                  <Money v-model="row.quantidade" v-bind="number"
                                    @input="recalculaValorTotalMaterial(row)"
                                    @keyup.native="recalculaValorTotalMaterial(row)" ></Money>
                                </div>
                              </td>
                              <td class="estilo-celula texto-centro">
                                <div class="input-table input-table--money">
                                  <Money v-model="row.valorUnitario" v-bind="money"
                                    @input="recalculaValorTotalMaterial(row)"
                                    @keyup.native="recalculaValorTotalMaterial(row)" ></Money>
                                </div>
                              </td>
                              <td class="estilo-celula texto-centro">
                                <div :style="{ color: 'white', backgroundColor: 'black', padding: '6px 12px', borderRadius: '3px', display: 'inline-block', minWidth: '120px', textAlign: 'right', border: '1px solid #444' }">
                                  {{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format((Number(row.quantidade) || 0) * (Number(row.valorUnitario) || 0)) }}
                                </div>
                              </td>
                              <td class="estilo-celula texto-centro">
                                <button type="button" @click="removerMaterial(row.id)" class="btn width-75 mb-3 mr-3 bg-danger">
                                  <i class="fa fa-trash"></i>
                                </button>
                              </td>
                            </tr>
                            <tr v-if="!materiaisAdicionados || materiaisAdicionados.length === 0">
                              <td class="estilo-celula texto-centro" colspan="5">Nenhum material adicionado</td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                    </b-col>
                  </b-row>

                </tab-content>

                <tab-content title="3. Observação">

                  <b-form>

                    <b-row v-if="!modalNovoPedidoMaterial_TipoValor">
                      <b-col md="6">
                        <label class="mr-3" style="font-size: 16px !important">Valor Total:</label>
                        <Money v-model="modalNovoPedidoMaterial_ValorTotal" v-bind="money" ></Money>
                      </b-col>
                    </b-row>


                    <br /><br />

                    <label class="mr-3" style="font-size: 16px !important;">Observação: </label>
                    <textarea-autosize id="textarea" v-model="modalNovoPedidoMaterial_Observacao" class="form-control"
                      :min-height="135" style="color:white; font-size: 18px;" />

                  </b-form>
                </tab-content>

                <tab-content title="4. Arquivos">
                  <b-row>
                    <b-col>
                      <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input"
                        @change="onChange" ref="file" multiple="true" />
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
                </tab-content>
              </form-wizard>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <button type="button" @click="salvarRascunhoMaterial();" class="btn btn-warning mb-0">
                  <i class="fa fa-pencil" title="Salvar Rascunho"></i>&nbsp;Salvar Rascunho
                </button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL MINHAS SOLICITAÇÕES =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalMinhasSolicitacoes" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="xl" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalMinhasSolicitacoes_Exibir">

          <!-- Hero -->
          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Solicitações</span>
              <h3 class="afm-hero__title">Minhas Solicitações</h3>
              <p class="afm-hero__description">Consulte e gerencie suas solicitações de compra.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalMinhasSolicitacoes_Count }}</span>
              <small>solicitação(ões)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <!-- ========== FILTROS ========== -->
            <section class="afm-section-card afm-filter">
              <div class="afm-filter__header">
                <i class="fa fa-filter"></i>
                <span>Filtros de busca</span>
              </div>

              <div class="afm-filter__body">
                <b-row>
                  <b-col md="6" class="mb-3">
                    <div class="afm-field-group">
                      <label class="afm-label">Obra(s)</label>
                      <multiselect
                        v-model="modalMinhasSolicitacoes_FiltroObrasSelecionadas"
                        :multiple="true"
                        :options="obras"
                        :custom-label="descricaoObra"
                        select-label="Selecionar"
                        placeholder="Selecione uma ou mais obras"
                        label="descricao"
                        track-by="descricao"
                      ></multiselect>
                    </div>
                  </b-col>
                  <b-col md="6" class="mb-3">
                    <div class="afm-field-group">
                      <label class="afm-label">Status</label>
                      <multiselect
                        v-model="modalMinhasSolicitacoes_FiltroStatusSelecionados"
                        :multiple="true"
                        :options="modalMinhasSolicitacoes_OpcoesStatus"
                        select-label="Selecionar"
                        placeholder="Selecione um ou mais status"
                        label="descricao"
                        track-by="descricao"
                      ></multiselect>
                    </div>
                  </b-col>
                </b-row>

                <b-row>
                  <b-col md="3" class="mb-3">
                    <div class="afm-field-group">
                      <label class="afm-label">Código</label>
                      <b-form-input
                        v-model="modalMinhasSolicitacoes_FiltroCodigo"
                        placeholder="Ex: SC001"
                        style="color: white;"
                      ></b-form-input>
                    </div>
                  </b-col>
                  <b-col md="9" class="mb-3">
                    <div class="afm-field-group">
                      <label class="afm-label">Título</label>
                      <b-form-input
                        v-model="modalMinhasSolicitacoes_FiltroTitulo"
                        placeholder="Buscar pelo título"
                        style="color: white;"
                      ></b-form-input>
                    </div>
                  </b-col>
                </b-row>

                <b-row>
                  <b-col md="12" class="mb-2">
                    <label class="afm-label">Data da Solicitação</label>
                    <div class="afm-filter__date-range">
                      <DatePickerMask
                        :append-to-body="true"
                        :editable="true"
                        lang="pt-br"
                        v-model="modalMinhasSolicitacoes_FiltroDataInicio"
                        format="dd/MM/yyyy"
                        type="date"
                        placeholder="Data inicial"
                      ></DatePickerMask>
                      <DatePickerMask
                        :append-to-body="true"
                        :editable="true"
                        lang="pt-br"
                        v-model="modalMinhasSolicitacoes_FiltroDataFim"
                        format="dd/MM/yyyy"
                        type="date"
                        placeholder="Data final"
                      ></DatePickerMask>
                    </div>
                  </b-col>
                </b-row>
              </div>

              <div class="afm-filter__footer">
                <b-button
                  variant="dark"
                  @click="limparFiltrosMinhasSolicitacoes()"
                >
                  <i class="fa fa-eraser mr-1"></i> Limpar
                </b-button>
                <b-button
                  variant="success"
                  @click="filtrarMinhasSolicitacoes()"
                >
                  <i class="fa fa-search mr-1"></i> Filtrar
                </b-button>
              </div>
            </section>

            <!-- ========== RESULTADOS ========== -->
            <section class="afm-section-card">

              <div v-if="modalMinhasSolicitacoes_Solicitacoes.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Código</th>
                        <th class="estilo-cabecalho">Titulo</th>
                        <th class="estilo-cabecalho texto-centro">Tipo</th>
                        <th class="estilo-cabecalho texto-centro">Solicitado</th>
                        <th class="estilo-cabecalho texto-centro">Data entrega</th>
                        <th class="estilo-cabecalho">Obra</th>
                        <th class="estilo-cabecalho texto-centro">Valor estimado</th>
                        <th class="estilo-cabecalho texto-centro">Status</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalMinhasSolicitacoes_Solicitacoes" :key="'ms-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                        <td class="estilo-celula">{{ row.nome }}</td>
                        <td class="estilo-celula texto-centro">{{ row.servico == true ? "Serviço" : "Material" }}</td>
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                        <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorEstimado) }}</td>
                        <td class="estilo-celula texto-centro">{{ (row.statusSolicitacaoCompra ? row.statusSolicitacaoCompra.descricao : '') + (row.motivoCancelamento == null ? '' : ' - ' + row.motivoCancelamento) }}</td>
                        <td class="estilo-celula texto-centro" style="width: 160px;">
                          <button
                            type="button"
                            class="btn btn-success"
                            @click="abrirModalAcoesMinhaSolicitacao(row)"
                          >
                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>

                <!-- ========== PAGINAÇÃO ========== -->
                <div
                  v-if="modalMinhasSolicitacoes_Count > modalMinhasSolicitacoes_Take"
                  class="afm-filter__pagination"
                >
                  <span class="afm-filter__pagination-info">
                    Exibindo <strong>{{ minhasSolicitacoes_InicioPagina }}</strong>
                    a <strong>{{ minhasSolicitacoes_FimPagina }}</strong>
                    de <strong>{{ modalMinhasSolicitacoes_Count }}</strong>
                  </span>
                  <b-pagination
                    v-model="modalMinhasSolicitacoes_PaginaAtual"
                    :total-rows="modalMinhasSolicitacoes_Count"
                    :per-page="modalMinhasSolicitacoes_Take"
                    @change="mudarPaginaMinhasSolicitacoes"
                    first-text="«"
                    prev-text="‹"
                    next-text="›"
                    last-text="»"
                  ></b-pagination>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div>
                  <strong>Nenhuma solicitação encontrada</strong>
                  <p>Você não possui solicitações de compra registradas.</p>
                </div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total:</span>
                <strong>{{ modalMinhasSolicitacoes_Count }} solicitação(ões)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalMinhasSolicitacoes_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>

        </b-modal>

        <!-- =================== MODAL MEUS PEDIDOS INTERNOS =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalMeusPedidosInternos" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="xl" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalMeusPedidosInternos_Exibir">

          <!-- Hero -->
          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedidos Internos</span>
              <h3 class="afm-hero__title">Acompanhe seus pedidos internos</h3>
              <p class="afm-hero__description">
                Visualize o status, o beneficiário e os valores de cada pedido interno vinculado à sua conta.
              </p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalMeusPedidosInternos_Pedidos.length }}</span>
              <small>pedido(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="modalMeusPedidosInternos_Pedidos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Código</th>
                        <th class="estilo-cabecalho">Descrição</th>
                        <th class="estilo-cabecalho">Beneficiário</th>
                        <th class="estilo-cabecalho texto-centro">Nº Parcelas</th>
                        <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro">Status</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalMeusPedidosInternos_Pedidos" :key="'mpi-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                        <td class="estilo-celula">{{ row.descricao }}</td>
                        <td class="estilo-celula">{{ row.idFornecedorBeneficiario != null ? (row.fornecedorBeneficiario ? row.fornecedorBeneficiario.nomeFantasia : '') : row.idUsuarioBeneficiario != null ? (row.usuarioBeneficiario ? row.usuarioBeneficiario.nome : '') : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ row.numeroTotalParcelas }}</td>
                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                        <td class="estilo-celula texto-centro">
                          <span :class="{
                            'afm-badge afm-badge--pending': row.aprovado == null,
                            'afm-badge afm-badge--ok': row.aprovado == true,
                            'afm-badge afm-badge--warn': row.aprovado == false
                          }">
                            {{ row.aprovado == null ? "Pendente Aprovação" : row.aprovado == true ? "Aprovado" : "Reprovado" }}
                          </span>
                        </td>
                        <td class="estilo-celula texto-centro" style="width: 160px;">
                          <button
                            type="button"
                            class="btn btn-info"
                            @click="abrirModalAcoesMeuPedidoInterno(row)"
                          >
                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div>
                  <strong>Nenhum pedido encontrado</strong>
                  <p>Não há pedidos internos vinculados à sua conta no momento.</p>
                </div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total de pedidos:</span>
                <strong>{{ modalMeusPedidosInternos_Pedidos.length }} item(ns)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalMeusPedidosInternos_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>

        </b-modal>

        <!-- =================== MODAL TIPO NOVO PEDIDO INTERNO =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalTipoNovoPedidoInterno" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalNovoPedidoInterno_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Selecione o tipo de pedido interno</h3>
              <p class="afm-hero__description">Escolha a categoria do pedido para prosseguir com o cadastro.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <div class="afm-tipo-grid afm-tipo-grid--4">
                <button type="button" class="afm-tipo-btn" @click="abreModalNovoPedidoInterno('administrativo')">
                  <i class="fa fa-building"></i>
                  <span>Administrativo</span>
                </button>
                <button type="button" class="afm-tipo-btn" @click="abreModalNovoPedidoInterno('geral')">
                  <i class="fa fa-list-alt"></i>
                  <span>Geral</span>
                </button>
                <button type="button" class="afm-tipo-btn" @click="abreModalNovoPedidoInterno('obra')">
                  <i class="fa fa-hard-hat"></i>
                  <span>Obra</span>
                </button>
                <button type="button" class="afm-tipo-btn" @click="abreModalNovoPedidoInterno('garantia')">
                  <i class="fa fa-shield"></i>
                  <span>Garantia Obra</span>
                </button>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions"></div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL PEDIDO INTERNO ADMINISTRATIVO =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalPedidoInternoAdministrativo" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalPedidoInternoAdministrativo_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Nova PI Administrativa</h3>
              <p class="afm-hero__description">Preencha os dados, defina as parcelas e anexe os arquivos necessários.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <!-- Seção: Valores -->
            <section class="afm-section-card">
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor Total</label>
                    <Money v-model="modalPedidoInternoAdministrativo_Valor" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor das parcelas</label>
                    <h4 class="mono" style="margin:0; padding-top:4px;">
                      {{ new Intl.NumberFormat("pt-BR", {style: "currency", currency: "BRL"}).format(modalPedidoInternoAdministrativo_Pagamentos.reduce((total, item) => total + Number(item.valor), 0)) }}
                    </h4>
                  </div>
                </b-col>
              </b-row>
            </section>

            <!-- Seção: Configurações -->
            <section class="afm-section-card">
              <b-row>
                <b-col md="12" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Código DEF</label>
                    <multiselect v-model="modalPedidoInternoAdministrativo_DefSelecionado" :multiple="false"
                      :options="modalPedidoInternoAdministrativo_DEFs" select-label="Selecionar"
                      placeholder="Selecione um código de DEF" :custom-label="descricaoDef" label="codigo" track-by="codigo">
                    </multiselect>
                  </div>
                </b-col>
                <b-col md="12">
                  <div class="afm-field-group">
                    <label class="afm-label">Usuário Aprovador</label>
                    <multiselect v-model="modalPedidoInternoAdministrativo_UsuarioAprovador" :multiple="false"
                      :options="usuariosAprovadores" select-label="Selecionar" placeholder="Selecione um usuário"
                      label="nome" track-by="nome">
                    </multiselect>
                  </div>
                </b-col>
              </b-row>
            </section>

            <!-- Seção: Parcelas -->
            <section class="afm-section-card">
              <b-row class="mb-3">
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Data do pagamento</label>
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br"
                      v-model="modalPedidoInternoAdministrativo_DataPagamento" :not-before="dataMinima"
                      format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor</label>
                    <Money v-model="modalPedidoInternoAdministrativo_ValorPagamento" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group afm-field-group--action">
                    <label class="afm-label">Ação</label>
                    <b-button v-on:click="adicionarPagamentoPedidoInternoAdministrativo()" variant="success" class="afm-add-btn">
                      <i class="fa fa-plus mr-1"></i>Adicionar Parcela
                    </b-button>
                  </div>
                </b-col>
              </b-row>
              <div v-if="modalPedidoInternoAdministrativo_Pagamentos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro" style="width:70px;"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(pagamento, index) in modalPedidoInternoAdministrativo_Pagamentos" :key="'pia-' + (pagamento.id || index)">
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--date">
                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.data"
                              :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                            </DatePickerMask>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="pagamento.valor" v-bind="money" >
                            </Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <b-button v-on:click="excluirPagamentoModalPedidoInternoAdministrativo(pagamento.id)"
                            variant="danger" class="afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </b-button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhuma parcela adicionada</strong><p>Informe data e valor e clique em Adicionar Parcela.</p></div>
              </div>
            </section>

            <!-- Seção: Descrição -->
            <section class="afm-section-card">
              <div class="afm-field-group">
                <textarea-autosize v-model="modalPedidoInternoAdministrativo_Descricao"
                  class="form-control" :min-height="120" style="color:white" />
              </div>
            </section>

            <!-- Seção: Arquivos -->
            <section class="afm-section-card">
              <input type="file" name="filePedidoInterno" id="fileInputPedidoInterno" style="display: none"
                class="hidden-input" @change="onChangePedidoInterno" ref="filePedidoInterno" multiple="true" />
              <ul v-if="upload_FilesPedidoInterno.length > 0" style="padding-left:1.2rem; margin-bottom:1rem;">
                <li v-for="item in upload_FilesPedidoInterno" v-bind:key="item.name">{{ item.name }}</li>
              </ul>
              <div style="display:flex; gap:8px;">
                <label type="button" class="btn btn-success mb-0" for="fileInputPedidoInterno">Escolher Arquivos</label>
                <b-button v-on:click="removerArquivosPedidoInterno()" v-if="upload_FilesPedidoInterno.length > 0"
                  variant="danger" class="mb-0">Remover Arquivos</b-button>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalPedidoInternoAdministrativo_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                <b-button v-on:click="realizaPedidoInternoAdministrativo()" variant="success" class="mb-0">Realizar Pedido</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL PEDIDO INTERNO GERAL =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalPedidoInternoGeral" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
          v-model="modalPedidoInternoGeral_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Nova PI Geral</h3>
              <p class="afm-hero__description">Preencha os dados, defina as parcelas e anexe os arquivos necessários.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor Total</label>
                    <Money v-model="modalPedidoInternoGeral_Valor" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor das parcelas</label>
                    <h4 class="mono" style="margin:0; padding-top:4px;">
                      {{ new Intl.NumberFormat("pt-BR", {style: "currency", currency: "BRL"}).format(modalPedidoInternoGeral_Pagamentos.reduce((total, item) => total + Number(item.valor), 0)) }}
                    </h4>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row>
                <b-col md="12" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Código DEF</label>
                    <multiselect v-model="modalPedidoInternoGeral_DefSelecionado" :multiple="false"
                      :options="modalPedidoInternoGeral_DEFs" select-label="Selecionar"
                      placeholder="Selecione um código de DEF" :custom-label="descricaoDef" label="codigo" track-by="codigo">
                    </multiselect>
                  </div>
                </b-col>
                <b-col md="12">
                  <div class="afm-field-group">
                    <label class="afm-label">Usuário Aprovador</label>
                    <multiselect v-model="modalPedidoInternoGeral_UsuarioAprovador" :multiple="false"
                      :options="usuariosAprovadores" select-label="Selecionar" placeholder="Selecione um usuário"
                      label="nome" track-by="nome">
                    </multiselect>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row class="mb-3">
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Data do pagamento</label>
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalPedidoInternoGeral_DataPagamento"
                      :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor</label>
                    <Money v-model="modalPedidoInternoGeral_ValorPagamento" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group afm-field-group--action">
                    <label class="afm-label">Ação</label>
                    <b-button v-on:click="adicionarPagamentoPedidoInternoGeral()" variant="success" class="afm-add-btn">
                      <i class="fa fa-plus mr-1"></i>Adicionar Parcela
                    </b-button>
                  </div>
                </b-col>
              </b-row>
              <div v-if="modalPedidoInternoGeral_Pagamentos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro" style="width:70px;"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(pagamento, index) in modalPedidoInternoGeral_Pagamentos" :key="'pig-' + (pagamento.id || index)">
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--date">
                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.data"
                              :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                            </DatePickerMask>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="pagamento.valor" v-bind="money" >
                            </Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <b-button v-on:click="excluirPagamentoModalPedidoInternoGeral(pagamento.id)"
                            variant="danger" class="afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </b-button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhuma parcela adicionada</strong><p>Informe data e valor e clique em Adicionar Parcela.</p></div>
              </div>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group">
                <textarea-autosize v-model="modalPedidoInternoGeral_Descricao"
                  class="form-control" :min-height="120" style="color:white" />
              </div>
            </section>

            <section class="afm-section-card">
              <input type="file" name="filePedidoInterno" id="fileInputPedidoInterno" style="display: none"
                class="hidden-input" @change="onChangePedidoInterno" ref="filePedidoInterno" multiple="true" />
              <ul v-if="upload_FilesPedidoInterno.length > 0" style="padding-left:1.2rem; margin-bottom:1rem;">
                <li v-for="item in upload_FilesPedidoInterno" v-bind:key="item.name">{{ item.name }}</li>
              </ul>
              <div style="display:flex; gap:8px;">
                <label type="button" class="btn btn-success mb-0" for="fileInputPedidoInterno">Escolher Arquivos</label>
                <b-button v-on:click="removerArquivosPedidoInterno()" v-if="upload_FilesPedidoInterno.length > 0"
                  variant="danger" class="mb-0">Remover Arquivos</b-button>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalPedidoInternoGeral_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                <b-button v-on:click="realizaPedidoInternoGeral()" variant="success" class="mb-0">Realizar Pedido</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL PEDIDO INTERNO OBRA =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalPedidoInternoObra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
          v-model="modalPedidoInternoObra_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Nova PI Obra</h3>
              <p class="afm-hero__description">Preencha os dados, defina as parcelas e anexe os arquivos necessários.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor Total</label>
                    <Money v-model="modalPedidoInternoObra_Valor" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor das parcelas</label>
                    <h4 class="mono" style="margin:0; padding-top:4px;">
                      {{ new Intl.NumberFormat("pt-BR", {style: "currency", currency: "BRL"}).format(modalPedidoInternoObra_Pagamentos.reduce((total, item) => total + Number(item.valor), 0)) }}
                    </h4>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Código DEF</label>
                    <multiselect v-model="modalPedidoInternoObra_DefSelecionado" :multiple="false"
                      :options="modalPedidoInternoObra_DEFs" select-label="Selecionar"
                      placeholder="Selecione um código de DEF" :custom-label="descricaoDef" label="codigo" track-by="codigo">
                    </multiselect>
                  </div>
                </b-col>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Obra</label>
                    <multiselect v-model="modalPedidoInternoObra_ObraSelecionada" :multiple="false" :options="obras"
                      select-label="Selecionar" placeholder="Selecione uma obra" label="codigo" track-by="codigo">
                    </multiselect>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row class="mb-3">
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Data do pagamento</label>
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalPedidoInternoObra_DataPagamento"
                      :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor</label>
                    <Money v-model="modalPedidoInternoObra_ValorPagamento" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group afm-field-group--action">
                    <label class="afm-label">Ação</label>
                    <b-button v-on:click="adicionarPagamentoPedidoInternoObra()" variant="success" class="afm-add-btn">
                      <i class="fa fa-plus mr-1"></i>Adicionar Parcela
                    </b-button>
                  </div>
                </b-col>
              </b-row>
              <div v-if="modalPedidoInternoObra_Pagamentos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro" style="width:70px;"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(pagamento, index) in modalPedidoInternoObra_Pagamentos" :key="'pio-' + (pagamento.id || index)">
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--date">
                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.data"
                              :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                            </DatePickerMask>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="pagamento.valor" v-bind="money" >
                            </Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <b-button v-on:click="excluirPagamentoModalPedidoInternoObra(pagamento.id)"
                            variant="danger" class="afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </b-button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhuma parcela adicionada</strong><p>Informe data e valor e clique em Adicionar Parcela.</p></div>
              </div>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group">
                <textarea-autosize v-model="modalPedidoInternoObra_Descricao"
                  class="form-control" :min-height="120" style="color:white" />
              </div>
            </section>

            <section class="afm-section-card">
              <input type="file" name="filePedidoInterno" id="fileInputPedidoInterno" style="display: none"
                class="hidden-input" @change="onChangePedidoInterno" ref="filePedidoInterno" multiple="true" />
              <ul v-if="upload_FilesPedidoInterno.length > 0" style="padding-left:1.2rem; margin-bottom:1rem;">
                <li v-for="item in upload_FilesPedidoInterno" v-bind:key="item.name">{{ item.name }}</li>
              </ul>
              <div style="display:flex; gap:8px;">
                <label type="button" class="btn btn-success mb-0" for="fileInputPedidoInterno">Escolher Arquivos</label>
                <b-button v-on:click="removerArquivosPedidoInterno()" v-if="upload_FilesPedidoInterno.length > 0"
                  variant="danger" class="mb-0">Remover Arquivos</b-button>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalPedidoInternoObra_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                <b-button v-on:click="realizaPedidoInternoObra()" variant="success" class="mb-0">Realizar Pedido</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL PEDIDO INTERNO GARANTIA =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalPedidoInternoGarantia" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
          v-model="modalPedidoInternoGarantia_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Nova PI Garantia Obra</h3>
              <p class="afm-hero__description">Preencha os dados, defina as parcelas e anexe os arquivos necessários.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor Total</label>
                    <Money v-model="modalPedidoInternoGarantia_Valor" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor das parcelas</label>
                    <h4 class="mono" style="margin:0; padding-top:4px;">
                      {{ new Intl.NumberFormat("pt-BR", {style: "currency", currency: "BRL"}).format(modalPedidoInternoGarantia_Pagamentos.reduce((total, item) => total + Number(item.valor), 0)) }}
                    </h4>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Código DEF</label>
                    <multiselect v-model="modalPedidoInternoGarantia_DefSelecionado" :multiple="false"
                      :options="modalPedidoInternoGarantia_DEFs" select-label="Selecionar"
                      placeholder="Selecione um código de DEF" :custom-label="descricaoDef" label="codigo" track-by="codigo">
                    </multiselect>
                  </div>
                </b-col>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Usuário Aprovador</label>
                    <multiselect v-model="modalPedidoInternoGarantia_UsuarioAprovador" :multiple="false"
                      :options="usuariosAprovadores" select-label="Selecionar" placeholder="Selecione um usuário"
                      label="nome" track-by="nome">
                    </multiselect>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row class="mb-3">
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Data do pagamento</label>
                    <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalPedidoInternoGarantia_DataPagamento"
                      :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor</label>
                    <Money v-model="modalPedidoInternoGarantia_ValorPagamento" v-bind="money" ></Money>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group afm-field-group--action">
                    <label class="afm-label">Ação</label>
                    <b-button v-on:click="adicionarPagamentoPedidoInternoGarantia()" variant="success" class="afm-add-btn">
                      <i class="fa fa-plus mr-1"></i>Adicionar Parcela
                    </b-button>
                  </div>
                </b-col>
              </b-row>
              <div v-if="modalPedidoInternoGarantia_Pagamentos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro" style="width:70px;"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(pagamento, index) in modalPedidoInternoGarantia_Pagamentos" :key="'pigar-' + (pagamento.id || index)">
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--date">
                            <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.data"
                              :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                            </DatePickerMask>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="pagamento.valor" v-bind="money" >
                            </Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <b-button v-on:click="excluirPagamentoModalPedidoInternoGarantia(pagamento.id)"
                            variant="danger" class="afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </b-button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhuma parcela adicionada</strong><p>Informe data e valor e clique em Adicionar Parcela.</p></div>
              </div>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group">
                <textarea-autosize v-model="modalPedidoInternoGarantia_Descricao"
                  class="form-control" :min-height="120" style="color:white" />
              </div>
            </section>

            <section class="afm-section-card">
              <input type="file" name="filePedidoInterno" id="fileInputPedidoInterno" style="display: none"
                class="hidden-input" @change="onChangePedidoInterno" ref="filePedidoInterno" multiple="true" />
              <ul v-if="upload_FilesPedidoInterno.length > 0" style="padding-left:1.2rem; margin-bottom:1rem;">
                <li v-for="item in upload_FilesPedidoInterno" v-bind:key="item.name">{{ item.name }}</li>
              </ul>
              <div style="display:flex; gap:8px;">
                <label type="button" class="btn btn-success mb-0" for="fileInputPedidoInterno">Escolher Arquivos</label>
                <b-button v-on:click="removerArquivosPedidoInterno()" v-if="upload_FilesPedidoInterno.length > 0"
                  variant="danger" class="mb-0">Remover Arquivos</b-button>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalPedidoInternoGarantia_Exibir = false" variant="dark" class="mb-0 mr-2">Cancelar</b-button>
                <b-button v-on:click="realizaPedidoInternoGarantia()" variant="success" class="mb-0">Realizar Pedido</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL ARQUIVOS =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalArquivos_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Documentos</span>
              <h3 class="afm-hero__title">Arquivos anexados</h3>
              <p class="afm-hero__description">Visualize e faça o download dos arquivos vinculados a este registro.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalArquivos_Arquivos.length }}</span>
              <small>arquivo(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="modalArquivos_Arquivos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho">Arquivo</th>
                        <th class="estilo-cabecalho">Usuário</th>
                        <th class="estilo-cabecalho texto-centro">Data</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalArquivos_Arquivos" :key="'arq-' + (row.id || index)">
                        <td class="estilo-celula">{{ row.nome }}</td>
                        <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">
                          <button type="button" @click="downloadFile('SolicitacaoCompra', row.id, row.nome)"
                            class="btn btn-warning afm-action-btn">
                            <i class="fa fa-download" title="Download"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhum arquivo encontrado</strong><p>Não há arquivos anexados a este registro.</p></div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total:</span>
                <strong>{{ modalArquivos_Arquivos.length }} arquivo(s)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalArquivos_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL COMENTÁRIOS =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalComentarios" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalComentarios_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Comunicação</span>
              <h3 class="afm-hero__title">Comentários</h3>
              <p class="afm-hero__description">Histórico de comentários e observações vinculadas a este registro.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalComentarios_Comentarios.length }}</span>
              <small>comentário(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="modalComentarios_Comentarios.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho">Comentário</th>
                        <th class="estilo-cabecalho">Usuário</th>
                        <th class="estilo-cabecalho texto-centro">Data</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalComentarios_Comentarios" :key="'com-' + (row.id || index)">
                        <td class="estilo-celula">{{ row.observacao }}</td>
                        <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-comments-o"></i>
                <div><strong>Nenhum comentário encontrado</strong><p>Não há comentários registrados para este item.</p></div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total:</span>
                <strong>{{ modalComentarios_Comentarios.length }} comentário(s)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalComentarios_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL REABRIR SOLICITAÇÃO =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalReabrirSolicitacao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalReabrirSolicitacao_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Solicitação de Compra</span>
              <h3 class="afm-hero__title">{{ modalReabrirSolicitacao_Titulo }}</h3>
              <p class="afm-hero__description">Selecione a obra, a data de entrega e adicione os materiais necessários.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <b-row>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Obra</label>
                    <multiselect v-model="modalReabrirSolicitacao_ObraSelecionada" :multiple="false" :options="obras"
                      :custom-label="descricaoObra" select-label="Selecionar" placeholder="Selecione uma obra"
                      label="descricao" track-by="descricao">
                    </multiselect>
                  </div>
                </b-col>
                <b-col md="6" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Data de entrega</label>
                    <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                      v-model="modalReabrirSolicitacao_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <b-row>
                <b-col md="12" class="mb-3">
                  <div class="afm-field-group">
                    <label class="afm-label">Material / Serviço</label>
                    <multiselect v-model="modalReabrirSolicitacao_MaterialSelecionado" :multiple="false"
                      :options="materiaisFiltradosPorTexto" select-label="Selecionar" placeholder="Selecione um material"
                      label="descricao" track-by="descricao" :custom-label="descricaoMaterial"
                      @search-change="buscaMateriaisComNomeInformado">
                    </multiselect>
                  </div>
                </b-col>
              </b-row>
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Quantidade</label>
                    <Money v-model="modalReabrirSolicitacao_Quantidade" v-bind="number" ref="valorUnitario"
                      @keyup.native.enter="enterQuantidade"></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor unitário</label>
                    <Money v-model="modalReabrirSolicitacao_ValorUnitario" v-bind="money" ref="valorUnitario"
                      @keyup.native.enter="enterValorUnitario"></Money>
                  </div>
                </b-col>
              </b-row>
              <div style="text-align:right; margin-top:12px;">
                <b-button v-on:click="modalReabrirSolicitacaoAdicionaMaterial()" variant="success" class="afm-add-btn">
                  <i class="fa fa-plus mr-1"></i>Adicionar Material / Serviço
                </b-button>
              </div>
            </section>

            <section class="afm-section-card">

              <div v-if="modalReabrirSolicitacao_MateriaisAdicionados.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho">Material</th>
                        <th class="estilo-cabecalho texto-centro">Qtd.</th>
                        <th class="estilo-cabecalho texto-centro">Valor Un.</th>
                        <th class="estilo-cabecalho texto-centro">Total</th>
                        <th class="estilo-cabecalho texto-centro"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalReabrirSolicitacao_MateriaisAdicionados" :key="'rsm-' + (row.id || index)">
                        <td class="estilo-celula">{{ row.material }}</td>
                        <td class="estilo-celula texto-centro">{{ row.quantidade }}</td>
                        <td class="estilo-celula texto-centro">{{ row.valorUnitarioFormatado }}</td>
                        <td class="estilo-celula texto-centro">{{ row.valorTotalFormatado }}</td>
                        <td class="estilo-celula texto-centro">
                          <button type="button" @click="modalReabrirSolicitacao_RemoverMaterial(row.id)"
                            class="btn btn-danger afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhum material adicionado</strong><p>Adicione materiais ou serviços usando o painel acima.</p></div>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Itens:</span>
                <strong>{{ modalReabrirSolicitacao_MateriaisAdicionados.length }}</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalReabrirSolicitacao_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
                <b-button variant="success" class="mb-0" @click="reabrirSolicitacao()">
                  {{ modalReabrirSolicitacao_Titulo }}
                </b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL APROVAÇÃO NF =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalAprovacaoNF" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalAprovacaoNF_Exibir" size="xl">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Aprovação</span>
              <h3 class="afm-hero__title">Notas fiscais pendentes de aprovação</h3>
              <p class="afm-hero__description">Analise e aprove ou reprove as notas fiscais listadas abaixo.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ modalAprovacaoNF_NotasFiscais.length }}</span>
              <small>nota(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="modalAprovacaoNF_NotasFiscais.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">OC</th>
                        <th class="estilo-cabecalho texto-centro">Obra</th>
                        <th class="estilo-cabecalho">Fornecedor</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro">Data</th>
                        <th class="estilo-cabecalho texto-centro">NF</th>
                        <th class="estilo-cabecalho texto-centro">Valor NF</th>
                        <th class="estilo-cabecalho texto-centro">Venc. NF</th>
                        <th class="estilo-cabecalho texto-centro">Saldo Pedido</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalAprovacaoNF_NotasFiscais" :key="'anf-' + (row.id || index)" :class="rowClass(row, 'row')">
                        <td class="estilo-celula texto-centro">{{ row.pedidoCompraNotaFiscal && row.pedidoCompraNotaFiscal.pedidoCompra ? row.pedidoCompraNotaFiscal.pedidoCompra.codigo : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ row.pedidoCompraNotaFiscal && row.pedidoCompraNotaFiscal.pedidoCompra && row.pedidoCompraNotaFiscal.pedidoCompra.centroCustoObra ? row.pedidoCompraNotaFiscal.pedidoCompra.centroCustoObra.codigo : '' }}</td>
                        <td class="estilo-celula">{{ row.pedidoCompraNotaFiscal && row.pedidoCompraNotaFiscal.pedidoCompra && row.pedidoCompraNotaFiscal.pedidoCompra.fornecedor ? row.pedidoCompraNotaFiscal.pedidoCompra.fornecedor.nomeFantasia : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorPedido) }}</td>
                        <td class="estilo-celula texto-centro">{{ row.pedidoCompraNotaFiscal && row.pedidoCompraNotaFiscal.pedidoCompra ? formataDataSemHora(row.pedidoCompraNotaFiscal.pedidoCompra.dataCadastro) : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ row.pedidoCompraNotaFiscal ? row.pedidoCompraNotaFiscal.numeroNotaFiscal : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ row.pedidoCompraNotaFiscal ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.pedidoCompraNotaFiscal.valor) : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ row.datasPagamento }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.saldoPedido) }}</td>
                        <td class="estilo-celula texto-centro">
                          <button
                            type="button"
                            class="btn btn-info"
                            @click="abrirModalAcoesAprovacaoNF(row)"
                          >
                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-check-circle-o"></i>
                <div><strong>Nenhuma nota fiscal pendente</strong><p>Não há notas fiscais aguardando aprovação.</p></div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total pendente:</span>
                <strong>{{ modalAprovacaoNF_NotasFiscais.length }} nota(s)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalAprovacaoNF_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL APROVAÇÃO PI =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalAprovacaoPI" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalAprovacaoPI_Exibir" size="xl">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Aprovação</span>
              <h3 class="afm-hero__title">Pedidos internos pendentes de aprovação</h3>
              <p class="afm-hero__description">Analise e aprove ou reprove os pedidos internos listados abaixo.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ pedidosInternosParaAprovacao.length }}</span>
              <small>pedido(s)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="pedidosInternosParaAprovacao.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Código</th>
                        <th class="estilo-cabecalho">Descrição</th>
                        <th class="estilo-cabecalho">Beneficiário</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro">Nº Parcelas</th>
                        <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                        <th class="estilo-cabecalho texto-centro">Data Vencimento</th>
                        <th class="estilo-cabecalho">Usuário</th>
                        <th class="estilo-cabecalho texto-centro">Aprovado</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in pedidosInternosParaAprovacao" :key="'piap-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                        <td class="estilo-celula">{{ row.descricao }}</td>
                        <td class="estilo-celula">{{ row.idFornecedorBeneficiario != null ? (row.fornecedorBeneficiario ? row.fornecedorBeneficiario.nomeFantasia : '') : row.idUsuarioBeneficiario != null ? (row.usuarioBeneficiario ? row.usuarioBeneficiario.nome : '') : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                        <td class="estilo-celula texto-centro">{{ row.numeroTotalParcelas }}</td>
                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">
                          <div v-for="(parcela, pidx) in row.parcelas" :key="'par-' + (parcela.id || pidx)">
                            {{ formataDataSemHora(parcela.dataPagamento) }}
                          </div>
                        </td>
                        <td class="estilo-celula">{{ row.usuarioCadastro != null ? row.usuarioCadastro.nome : '' }}</td>
                        <td class="estilo-celula texto-centro">
                          <span :class="{
                            'afm-badge afm-badge--pending': row.aprovado == null,
                            'afm-badge afm-badge--ok': row.aprovado == true,
                            'afm-badge afm-badge--warn': row.aprovado == false
                          }">
                            {{ row.aprovado == null ? 'Pendente' : row.aprovado ? 'Aprovado' : 'Reprovado' }}
                          </span>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <button
                            type="button"
                            class="btn btn-info"
                            @click="abrirModalAcoesAprovacaoPI(row)"
                          >
                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-check-circle-o"></i>
                <div><strong>Nenhum pedido pendente</strong><p>Não há pedidos internos aguardando aprovação.</p></div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total pendente:</span>
                <strong>{{ pedidosInternosParaAprovacao.length }} pedido(s)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalAprovacaoPI_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL APROVAÇÃO SC =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalAprovacaoSC" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalAprovacaoSC_Exibir" size="xl">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Aprovação</span>
              <h3 class="afm-hero__title">Solicitações de compra pendentes</h3>
              <p class="afm-hero__description">Gerencie, aprove ou reprove as solicitações de compra listadas abaixo.</p>
            </div>
            <div class="afm-hero__pill">
              <span>{{ solicitacoesCompraParaAprovacao.length }}</span>
              <small>solicitação(ões)</small>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">

              <div v-if="solicitacoesCompraParaAprovacao.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Código</th>
                        <th class="estilo-cabecalho">Usuário solicitante</th>
                        <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                        <th class="estilo-cabecalho texto-centro">Data de entrega</th>
                        <th class="estilo-cabecalho">Centro de custo</th>
                        <th class="estilo-cabecalho texto-centro">Valor total estimado</th>
                        <th class="estilo-cabecalho">Comprador</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in solicitacoesCompraParaAprovacao" :key="'scap-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                        <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                        <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorEstimado) }}</td>
                        <td class="estilo-celula">{{ row.usuarioComprador != null ? row.usuarioComprador.nome : "" }}</td>
                        <td class="estilo-celula texto-centro">
                          <button
                            type="button"
                            class="btn btn-success"
                            @click="abrirModalAcoesSolicitacaoParaAprovacao(row)"
                          >
                            Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-check-circle-o"></i>
                <div><strong>Nenhuma solicitação pendente</strong><p>Não há solicitações aguardando aprovação.</p></div>
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary">
                <span>Total pendente:</span>
                <strong>{{ solicitacoesCompraParaAprovacao.length }} solicitação(ões)</strong>
              </div>
              <div class="afm-footer__actions">
                <b-button @click="modalAprovacaoSC_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL GERENCIAR SOLICITAÇÃO COMPRA =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalSolicitacaoCompra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="xl" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalGerenciarSolicitacaoCompra_Exibir">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Solicitação de Compra</span>
              <h3 class="afm-hero__title">Gerenciar solicitação {{ modalGerenciarSolicitacaoCompra_Codigo }}</h3>
              <p class="afm-hero__description">Edite os dados, materiais e observações antes de salvar ou aprovar.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <b-row>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor Total</label>
                    <Money v-model="modalGerenciarSolicitacaoCompra_ValorTotal" v-bind="money" :disabled="true"></Money>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Data de entrega</label>
                    <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                      v-model="modalGerenciarSolicitacaoCompra_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
                    </DatePickerMask>
                  </div>
                </b-col>
                <b-col md="4">
                  <div class="afm-field-group">
                    <label class="afm-label">Título da Solicitação</label>
                    <b-form-input v-model="modalGerenciarSolicitacaoCompra_Nome"
                      style="color: white; background-color: black"></b-form-input>
                  </div>
                </b-col>
              </b-row>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group">
                <label class="afm-label">Comprador</label>
                <multiselect v-model="compradorSelecionado_AprovacaoSolicitacaoCompra" :multiple="false"
                  :options="usuariosCompradores" select-label="Selecionar" placeholder="Selecione um comprador"
                  label="nome" track-by="nome">
                </multiselect>
              </div>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group mb-3">
                <label class="afm-label">Material</label>
                <multiselect v-model="materialSelecionado_AprovacaoSolicitacaoCompra" :multiple="false"
                  :options="materiaisFiltradosPorTexto" select-label="Selecionar" placeholder="Selecione um material"
                  label="descricao" track-by="descricao" :custom-label="descricaoMaterial"
                  @search-change="buscaMateriaisComNomeInformado">
                </multiselect>
              </div>
              <b-row>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Quantidade</label>
                    <Money v-model="modalGerenciarSolicitacaoCompra_Quantidade" v-bind="decimalInput"></Money>
                  </div>
                </b-col>
                <b-col md="6">
                  <div class="afm-field-group">
                    <label class="afm-label">Valor unitário</label>
                    <Money v-model="modalGerenciarSolicitacaoCompra_ValorUnitario" v-bind="money"></Money>
                  </div>
                </b-col>
              </b-row>
              <div style="text-align:center; margin-top:12px;">
                <b-button v-on:click="adicionaMaterial_AprovacaoSolicitacaoCompra()" variant="success" class="afm-add-btn" style="width: auto; min-width: 200px; max-width: 320px;">
                  <i class="fa fa-plus mr-1"></i>Adicionar Material
                </b-button>
              </div>
            </section>

            <section class="afm-section-card">

              <div v-if="materiaisAdicionados.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Material / Serviço</th>
                        <th class="estilo-cabecalho texto-centro">QTD</th>
                        <th class="estilo-cabecalho texto-centro">Valor Un.</th>
                        <th class="estilo-cabecalho texto-centro" style="width:130px;">Total</th>
                        <th class="estilo-cabecalho texto-centro" style="width:60px;"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(material, index) in materiaisAdicionados" :key="'gscm-' + (material.id || index)">
                        <td class="estilo-celula texto-centro">{{ material.material }}</td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="material.quantidade" v-bind="number" style="max-width: 100px;"></Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">
                          <div class="input-table input-table--money">
                            <Money v-model="material.valorUnitario" v-bind="number" style="max-width: 100px;"></Money>
                          </div>
                        </td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(material.valorUnitario * material.quantidade) }}</td>
                        <td class="estilo-celula texto-centro">
                          <button type="button" @click="modalGerenciarSolicitacaoCompra_RemoverMaterial(material.id)"
                            class="btn btn-danger afm-remove-btn">
                            <i class="fa fa-trash"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhum material adicionado</strong><p>Use o painel acima para adicionar itens.</p></div>
              </div>
            </section>

            <section class="afm-section-card">
              <div class="afm-field-group">
                <textarea-autosize v-model="modalGerenciarSolicitacaoCompra_Observacao"
                  class="form-control" :min-height="120" style="color: white" />
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <button
                  type="button"
                  class="btn btn-success mb-0"
                  @click="abrirModalAcoesGerenciarSolicitacao()"
                >
                  Solicitação de Compra <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                </button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL CANCELAR SOLICITAÇÃO DE COMPRA =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalCancelarSolicitacao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalCancelarSolicitacao_Exibir" size="lg">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Cancelamento</span>
              <h3 class="afm-hero__title">{{ modalCancelarSolicitacao_Titulo }}</h3>
              <p class="afm-hero__description">Informe o motivo do cancelamento. Esta ação não poderá ser desfeita.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">
            <section class="afm-section-card">
              <div class="afm-field-group">
                <label class="afm-label">Motivo <span style="color: #ff6b6b;">*</span></label>
                <textarea-autosize v-model="modalCancelarSolicitacao_Motivo"
                  class="form-control" :min-height="100"
                  placeholder="Ex.: solicitação duplicada, material não será mais necessário, etc."
                  style="color: white" />
              </div>
            </section>
          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalCancelarSolicitacao_Exibir = false" variant="dark" class="mb-0 mr-2">
                  Fechar
                </b-button>
                <b-button @click="confirmarCancelamentoSolicitacao()" variant="danger" class="mb-0">
                  <i class="fa fa-ban mr-1"></i> Cancelar Solicitação
                </b-button>
              </div>
            </div>
          </template>
        </b-modal>

        <!-- =================== MODAL DETALHE PI =================== -->
        <b-modal :no-close-on-backdrop="true" id="modalDetalhePI" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
          footer-bg-variant="bodyModal" v-model="modalDetalhePI_Exibir" size="xl">

          <div class="afm-hero">
            <div>
              <span class="afm-hero__eyebrow">Pedido Interno</span>
              <h3 class="afm-hero__title">Detalhes do pedido interno</h3>
              <p class="afm-hero__description">Informações completas sobre o pedido, parcelas e arquivos vinculados.</p>
            </div>
          </div>

          <b-container fluid class="afm-sections">

            <section class="afm-section-card">
              <div class="afm-summary-grid">
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Solicitado por</span>
                  <strong class="afm-summary-card__value">{{ modalDetalhePI_PI != null ? modalDetalhePI_PI.usuarioCadastro.nome : '' }}</strong>
                </div>
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Data da solicitação</span>
                  <strong class="afm-summary-card__value">{{ modalDetalhePI_PI != null ? formataData(modalDetalhePI_PI.dataCadastro) : '' }}</strong>
                </div>
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Aprovado por</span>
                  <strong class="afm-summary-card__value">{{ modalDetalhePI_PI != null ? modalDetalhePI_PI.usuarioAprovacao.nome : '' }}</strong>
                </div>
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Data de aprovação</span>
                  <strong class="afm-summary-card__value">{{ (modalDetalhePI_PI != null && modalDetalhePI_PI.dataAprovacao != null) ? formataData(modalDetalhePI_PI.dataAprovacao) : '—' }}</strong>
                </div>
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Beneficiário</span>
                  <strong class="afm-summary-card__value">{{ modalDetalhePI_PI != null ? (modalDetalhePI_PI.idFornecedorBeneficiario != null ? modalDetalhePI_PI.fornecedorBeneficiario.nomeFantasia : modalDetalhePI_PI.usuarioBeneficiario.nome) : '' }}</strong>
                </div>
                <div class="afm-summary-card">
                  <span class="afm-summary-card__label">Valor total</span>
                  <strong class="afm-summary-card__value mono">{{ modalDetalhePI_PI != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalDetalhePI_PI.valorTotal) : '' }}</strong>
                </div>
              </div>
            </section>

            <section class="afm-section-card" v-if="modalDetalhePI_PI != null">

              <div v-if="modalDetalhePI_PI.parcelas.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho texto-centro">Parcela</th>
                        <th class="estilo-cabecalho texto-centro">Código</th>
                        <th class="estilo-cabecalho texto-centro">Valor</th>
                        <th class="estilo-cabecalho texto-centro">Data Pagamento</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalDetalhePI_PI.parcelas" :key="'pip-' + (row.id || index)">
                        <td class="estilo-celula texto-centro">{{ row.parcela }}</td>
                        <td class="estilo-celula texto-centro">{{ row.codigoFormatado }}</td>
                        <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valor) }}</td>
                        <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataPagamento) }}</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhuma parcela encontrada</strong></div>
              </div>
            </section>

            <section class="afm-section-card" v-if="modalDetalhePI_PI != null">

              <div v-if="modalDetalhePI_PI.arquivos.length > 0">
                <div class="table-card table-card--fluid">
                  <table class="estilo-tabela tabela-identidade">
                    <thead>
                      <tr>
                        <th class="estilo-cabecalho">Nome</th>
                        <th class="estilo-cabecalho">Usuário</th>
                        <th class="estilo-cabecalho texto-centro">Data</th>
                        <th class="estilo-cabecalho texto-centro">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(row, index) in modalDetalhePI_PI.arquivos" :key="'pia-' + (row.id || index)">
                        <td class="estilo-celula">{{ row.nome }}</td>
                        <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                        <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                        <td class="estilo-celula texto-centro">
                          <button type="button" @click="downloadFile('PedidoInterno', row.id, row.nome)"
                            class="btn btn-warning afm-action-btn">
                            <i class="fa fa-download" title="Download"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <div v-else class="afm-empty-state">
                <i class="fa fa-inbox"></i>
                <div><strong>Nenhum arquivo encontrado</strong></div>
              </div>
            </section>

          </b-container>

          <template #modal-footer>
            <div class="afm-footer">
              <div class="afm-footer__summary"></div>
              <div class="afm-footer__actions">
                <b-button @click="modalDetalhePI_Exibir = false;" variant="dark" class="mb-0">Fechar</b-button>
              </div>
            </div>
          </template>
        </b-modal>

      </b-row>

      <b-row v-if="this.usuarioDTO != null && this.usuarioDTO.globalSolicitacaoCompra">
        <b-col style="text-align: center" md="6"
          @click="upload_Files = []; modalNovoPedidoCompraTipoPedido_Exibir = true">
          <Widget class="h-100 mb-0" title="Solicitação de compra" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <span class="glyphicon glyphicon-shopping-cart" style="font-size: 70px" />
            </div>
          </Widget>
        </b-col>
        <b-col style="text-align: center" md="6" @click="abrirModalMinhasSolicitacoes()">
          <Widget class="h-100 mb-0" title="Minhas solicitações" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg" style="position: relative; display: inline-block;">
              <span class="glyphicon glyphicon-list" style="font-size: 70px" />
              <span
                v-if="badgeSolicitacoesStatus8 > 0"
                class="sc-notification-badge sc-notification-badge--orange"
              >{{ badgeSolicitacoesStatus8 }}</span>
            </div>
          </Widget>
        </b-col>
      </b-row>

      <br />

      <b-row v-if="this.usuarioDTO != null && this.usuarioDTO.globalPedidoInterno">
        <b-col style="text-align: center" md="6"
          @click="obtemProximoCodigoPedidoInterno(); modalNovoPedidoInterno_Exibir = true">
          <Widget class="h-100 mb-0" title="Pedido Interno" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <span class="glyphicon glyphicon-repeat" style="font-size: 70px" />
            </div>
          </Widget>
        </b-col>
        <b-col style="text-align: center" md="6" @click="modalMeusPedidosInternos_Exibir = true">
          <Widget class="h-100 mb-0" title="Meus Pedidos Internos" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <span class="glyphicon glyphicon-list" style="font-size: 70px" />
            </div>
          </Widget>
        </b-col>
      </b-row>

      <br />

      <b-row v-if="this.usuarioDTO != null">
        <b-col style="text-align: center" md="6" v-if="this.usuarioDTO.globalAprovaCotacao"
          @click="redirecionaAprovarCotacoes()">
          <Widget class="h-100 mb-0" title="Aprovações de Cotação" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <div style="font-size: 90px; color: lime;">{{ this.modalAprovacaoCotacao_Cotacoes }}</div>
            </div>
          </Widget>
        </b-col>
        <b-col style="text-align: center" md="6" v-if="this.usuarioDTO.globalAprovaNotaFiscal"
          @click="modalAprovacaoNF_Exibir = true">
          <Widget class="h-100 mb-0" title="Aprovações de Notas Fiscais" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <div style="font-size: 90px; color: lime;">{{ this.modalAprovacaoNF_NotasFiscais == [] ? 0 :
                this.modalAprovacaoNF_NotasFiscais.length }}</div>
            </div>
          </Widget>
        </b-col>
      </b-row>

      <br />

      <b-row v-if="this.usuarioDTO != null">
        <b-col style="text-align: center" md="6" v-if="this.usuarioDTO.globalAprovaCotacao"
          @click="modalAprovacaoPI_Exibir = true">
          <Widget class="h-100 mb-0" title="Aprovações de PI" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <div style="font-size: 90px; color: lime;">{{ this.pedidosInternosParaAprovacao == [] ? 0 :
                this.pedidosInternosParaAprovacao.length }}</div>
            </div>
          </Widget>
        </b-col>

        <b-col style="text-align: center" md="6" v-if="this.usuarioDTO.globalAprovaCotacao"
          @click="modalAprovacaoSC_Exibir = true">
          <Widget class="h-100 mb-0" title="Aprovações de SC" role="button">
            <br />
            <div class="justify-content-between align-items-center mb-lg">
              <div style="font-size: 90px; color: lime;">{{ this.solicitacoesCompraParaAprovacao == [] ? 0 :
                this.solicitacoesCompraParaAprovacao.length }}</div>
            </div>
          </Widget>
        </b-col>
      </b-row>

      <div class="pb-xlg" style="text-align: center"></div>
    </Widget>

    <!-- Modal de ações das tabelas -->
    <ModalAcoes
      :exibir.sync="modalAcoes_Exibir"
      :titulo="modalAcoes_Titulo"
      :itens="modalAcoes_Itens"
    />
  </div>
</template>

<script>
import Vue from 'vue';
import moment from "moment";
import ApiService from "@/services/api.service.js";
import Multiselect from "vue-multiselect";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import CurrencyInput from "../../components/CurrencyInput.vue";
import { Money } from 'v-money';
import DatePickerMask from 'vue2-datepicker-mask';
import "vue2-datepicker/index.css";
import "vue2-datepicker/locale/pt-br";
import ModalAcoes from "../../components/ModalAcoes/ModalAcoes.vue";

export default {
  name: "Dashboard",
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

      searchTimeout: null,

      progress: 1,

      dataMinima: (new Date()).setDate(new Date().getDate() - 1),

      usuarioDTO: null,

      open: false,
      isLoading: false,

      dataEntrega: null,
      nome: '',
      centroDeCustoObra: true,
      exibeValorUnitario: true,
      materialSelecionado: null,
      materialSelecionado_AprovacaoSolicitacaoCompra: null,
      compradorSelecionado_AprovacaoSolicitacaoCompra: null,
      defSelecionado: null,
      obraSelecionada: null,

      tipoNovaSolicitacao: '',

      rascunho: null,
      rascunhosSolicitacoes: [],

      modalNovoPedidoMaterial_Exibir: false,
      modalNovoPedidoMaterial_ObraSelecionada: null,
      modalNovoPedidoMaterial_DataEntrega: null,
      modalNovoPedidoMaterial_Titulo: '',
      modalNovoPedidoMaterial_CustoOrcado: 0.0,
      modalNovoPedidoMaterial_ValorTotal: 0.0,
      modalNovoPedidoMaterial_Observacao: '',
      modalNovoPedidoMaterial_MaterialSelecionado: null,
      modalNovoPedidoMaterial_Quantidade: 0,
      modalNovoPedidoMaterial_ValorUnitario: 0,
      modalNovoPedidoMaterial_TipoValor: true,

      modalNovoPedidoServico_Exibir: false,
      modalNovoPedidoServico_ObraSelecionada: null,
      modalNovoPedidoServico_DataEntrega: null,
      modalNovoPedidoServico_Titulo: '',
      modalNovoPedidoServico_CustoOrcado: 0.0,
      modalNovoPedidoServico_ValorTotal: 0.0,
      modalNovoPedidoServico_Observacao: '',
      modalNovoPedidoServico_CompradorSelecionado: null,
      modalNovoPedidoServico_FornecedorSelecionado: null,
      modalNovoPedidoServico_DataPagamentoServico: null,
      modalNovoPedidoServico_ValorPagamentoServico: 0.0,
      modalNovoPedidoServico_PagamentosServico: [],
      modalNovoPedidoServico_UsuarioAprovador: null,

      modalAprovacaoPI_Exibir: false,
      modalAprovacaoPI_Titulo: 'Aprovação de Pedido Interno',

      modalAprovacaoSC_Exibir: false,
      modalAprovacaoSC_Titulo: 'Aprovação de Solicitação de Compra',

      solicitacaoParaValidacao: null,

      modalNovoPedidoCompraTipoPedido_Exibir: false,
      modalNovoPedidoCompraExibir: false,
      modalMinhasSolicitacoes_Exibir: false,
      modalMinhasSolicitacoes_Solicitacoes: [],
      modalMinhasSolicitacoes_Count: 0,
      modalMinhasSolicitacoes_Skip: 0,
      modalMinhasSolicitacoes_Take: 10,
      modalMinhasSolicitacoes_PaginaAtual: 1,

      // ===== FILTROS: Minhas Solicitações =====
      // Valores selecionados pelo usuário
      modalMinhasSolicitacoes_FiltroObrasSelecionadas: [],
      modalMinhasSolicitacoes_FiltroCodigo: '',
      modalMinhasSolicitacoes_FiltroTitulo: '',
      modalMinhasSolicitacoes_FiltroDataInicio: null,
      modalMinhasSolicitacoes_FiltroDataFim: null,
      modalMinhasSolicitacoes_FiltroStatusSelecionados: [],
      // Listas para popular os multiselects (preencher via API)
      modalMinhasSolicitacoes_OpcoesStatus: [],

      modalMeusPedidosInternos_Exibir: false,
      modalMeusPedidosInternos_Pedidos: [],

      modalPedidoInternoAdministrativo_Exibir: false,
      modalPedidoInternoAdministrativo_Valor: 0.0,
      modalPedidoInternoAdministrativo_DefSelecionado: null,
      modalPedidoInternoAdministrativo_DEFs: [],
      modalPedidoInternoAdministrativo_DataPagamento: null,
      modalPedidoInternoAdministrativo_ValorPagamento: 0.0,
      modalPedidoInternoAdministrativo_Pagamentos: [],
      modalPedidoInternoAdministrativo_Descricao: '',
      modalPedidoInternoAdministrativo_UsuarioAprovador: null,

      modalPedidoInternoGeral_Exibir: false,
      modalPedidoInternoGeral_Valor: 0.0,
      modalPedidoInternoGeral_DefSelecionado: null,
      modalPedidoInternoGeral_DEFs: [],
      modalPedidoInternoGeral_DataPagamento: null,
      modalPedidoInternoGeral_ValorPagamento: 0.0,
      modalPedidoInternoGeral_Pagamentos: [],
      modalPedidoInternoGeral_Descricao: '',
      modalPedidoInternoGeral_UsuarioAprovador: null,

      modalPedidoInternoGarantia_Exibir: false,
      modalPedidoInternoGarantia_Valor: 0.0,
      modalPedidoInternoGarantia_DefSelecionado: null,
      modalPedidoInternoGarantia_DEFs: [],
      modalPedidoInternoGarantia_DataPagamento: null,
      modalPedidoInternoGarantia_ValorPagamento: 0.0,
      modalPedidoInternoGarantia_Pagamentos: [],
      modalPedidoInternoGarantia_Descricao: '',
      modalPedidoInternoGarantia_UsuarioAprovador: null,

      modalPedidoInternoObra_Exibir: false,
      modalPedidoInternoObra_Valor: 0.0,
      modalPedidoInternoObra_DefSelecionado: null,
      modalPedidoInternoObra_DEFs: [],
      modalPedidoInternoObra_DataPagamento: null,
      modalPedidoInternoObra_ValorPagamento: 0.0,
      modalPedidoInternoObra_Pagamentos: [],
      modalPedidoInternoObra_Descricao: '',
      modalPedidoInternoObra_UsuarioAprovador: null,
      modalPedidoInternoObra_ObraSelecionada: null,

      modalNovoPedidoInterno_Exibir: false,
      modalNovoPedidoInternoObra_Exibir: false,

      modalPedidoInterno_Titulo: '',
      modalPedidoInterno_Exibir: false,
      modalPedidoInterno_DefsSelecionados: [],
      modalPedidoInterno_DefsSelecionadosComValor: [],
      modalPedidoInterno_DefSelecionado: null,
      modalPedidoInterno_DefSelecionadoValor: 0.0,
      modalPedidoInterno_DataPagamentoMinimaParcela: new Date((new Date()).setDate(new Date().getDate() - 1)),
      modalPedidoInterno_DataPagamentoParcela: null,
      modalPedidoInterno_Descricao: '',
      modalPedidoInterno_Valor: 0.0,
      modalPedidoInterno_HabilitaSelecaoObra: false,
      modalPedidoInterno_ObraSelecionada: null,
      modalPedidoInterno_UsuarioAprovador: null,
      modalPedidoInterno_FornecedorEhBeneficiario: false,
      modalPedidoInterno_UsuarioBeneficiario: null,
      modalPedidoInterno_FornecedorBeneficiario: null,
      modalPedidoInterno_NumeroDaParcela: 1,
      modalPedidoInterno_ValorParcela: 0.0,
      modalPedidoInterno_ValorTotalParcelas: 0.0,
      modalPedidoInterno_Parcelas: [],
      modalPedidoInterno_ValorObra: 0,
      modalPedidoInterno_ValorTotalObras: 0.0,
      modalPedidoInterno_Obras: [],
      modalPedidoInterno_ObraSelecionadaParcela: null,
      modalPedidoInterno_ParcelaSelecionadaDefParcela: null,
      modalPedidoInterno_ParcelaSelecionadaParcela: null,
      modalPedidoInterno_ValorObraParcela: null,
      modalPedidoInterno_ObrasParcelas: [],
      modalPedidoInterno_ProximoCodigo: '',

      modalGerenciarSolicitacaoCompra_Exibir: false,
      modalGerenciarSolicitacaoCompra_ValorTotal: 0.0,
      modalGerenciarSolicitacaoCompra_Quantidade: 0.0,
      modalGerenciarSolicitacaoCompra_ValorUnitario: 0.0,
      modalGerenciarSolicitacaoCompra_Observacao: "",
      modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada: null,
      modalGerenciarSolicitacaoCompra_DataEntrega: null,
      modalGerenciarSolicitacaoCompra_Nome: '',
      modalGerenciarSolicitacaoCompra_Codigo: '',
      modalGerenciarSolicitacaoCompra_Solicitacao: null,
      modalGerenciarSolicitacaoCompra_PagamentosManual: [],
      modalGerenciarSolicitacaoCompra_NovaPgtoData: null,
      modalGerenciarSolicitacaoCompra_NovaPgtoValor: 0,
      modalGerenciarSolicitacaoCompra_CotacoesOriginais: [], // [{idMaterial, id_cotacao, cotacao_completa}]
      solicitacaoCompra_Servico: false,

      modalCancelarSolicitacao_Exibir: false,
      modalCancelarSolicitacao_Titulo: '',
      modalCancelarSolicitacao_Motivo: '',
      solicitacaoParaCancelamento: null,

      modalArquivos_Exibir: false,
      modalArquivos_Titulo: 'Arquivos',
      modalArquivos_Arquivos: [],

      modalComentarios_Exibir: false,
      modalComentarios_Titulo: 'Comentários',
      modalComentarios_Comentarios: [],

      modalAprovacaoNF_Titulo: 'Aprovação de notas fiscais',
      modalAprovacaoNF_Exibir: false,
      modalAprovacaoNF_NotasFiscais: [],

      modalAprovacaoCotacao_Cotacoes: 0,

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

      modalEdicaoDataNotaFiscal_Notas: [],
      modalEdicaoDataNotaFiscal_Exibir: false,

      modalDetalhePI_Titulo: '',
      modalDetalhePI_Exibir: false,
      modalDetalhePI_PI: null,

      obras: [],
      defs: [],
      defsPedidoInterno: [],
      usuarios: [],
      fornecedores: [],
      usuariosAprovadores: [],
      usuariosCompradores: [],
      materiais: [], //Lista de materiais ativos no sistema
      materiaisFiltradosPorTexto: [],
      materiaisAdicionados: [], //Lista de materiais adicionados na solicitação
      pedidosInternosParaAprovacao: [],
      solicitacoesCompraParaAprovacao: [],

      valorTotal: 0,
      quantidadeTotal: 0,
      materiaisSelecionados: [],

      upload_Files: [],
      upload_FilesPedidoInterno: [],

      colunasAprovacaoPI: ["codigo", "descricao", "beneficiario", "valor", "numeroParcelas", "dataCadastro", "dataVencimento", "usuarioCadastro", "acoes"],

      opcoesAprovacaoPI: {
        perPage: 10,
        headings: {
          codigo: "Código",
          descricao: "Descrição",
          beneficiario: "Beneficiário",
          valor: "Valor",
          numeroParcelas: "Parcelas",
          dataCadastro: "Data Solicitação",
          dataVencimento: "Data Vencimento",
          usuarioCadastro: "Usuário",
          acoes: "Ações",
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

      colunasAprovacaoSC: [
        "codigo",
        "nome",
        "usuario",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorEstimado",
        "comprador",
        "acoes",],

      opcoesAprovacaoSC: {
        perPage: 10,
        headings: {
          codigo: "Código Solicitação",
          nome: "Nome",
          usuario: "Usuário solicitante",
          dataCadastro: "Data da solicitação",
          dataEntrega: "Data de entrega",
          centroCusto: "Centro de custo",
          valorEstimado: "Valor total estimado",
          comprador: "Comprador",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [
          "codigo",
          "usuario",
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

      colunasTabelaSolicitacaoCompra: [
        "material",
        "quantidade",
        "valorUnitarioFormatado",
        "valorTotalFormatado",
        "acoes",
      ],

      opcoesTabelaSolicitacaoCompra: {
        perPage: 1000,
        headings: {
          material: "Material / Serviço",
          quantidade: "Qtd.",
          valorUnitarioFormatado: "Valor Un.",
          valorTotalFormatado: "Total",
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
        "tipo",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorEstimado",
        "status",
        "acoes",
      ],

      opcoesMinhasSolicitacoes: {
        perPage: 1000,
        headings: {
          codigo: "Código",
          nome: "Titulo",
          tipo: "Tipo",
          dataCadastro: "Solicitado",
          dataEntrega: "Data entrega",
          centroCusto: "Obra",
          valorEstimado: "Valor estimado",
          status: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [
        ],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
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
        "descricao",
        "beneficiario",
        "numeroParcelas",
        "dataCadastro",
        "valor",
        "status",
        "acoes",
      ],

      opcoesMeusPedidosInternos: {
        perPage: 1000,
        headings: {
          codigoFormatado: "Código",
          descricao: "Descrição",
          beneficiario: "Beneficiário",
          numeroParcelas: "Nº Parcelas",
          dataCadastro: "Data da solicitação",
          valor: "Valor",
          status: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        sortable: [],
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

      colunasRascunho: ["data", "titulo", "acoes"],

      opcoesRascunho: {
        perPage: 1000,
        headings: {
          titulo: "Título",
          data: "Data",
          acoes: "Ações"
        },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
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

      colunasTabelaComentarios: ["observacao", "usuario", "data"],

      opcoesTabelaComentarios: {
        perPage: 1000,
        headings: {
          observacao: "Comentário",
          usuario: "Usuário",
          data: "Data",
        },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
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

      colunasTabelaNotaFiscal: ["oc", "obra", "fornecedor", "valor", "data", "nf", "valorNF", "vencimentoNF", "saldoPedido", "acoes"],

      opcoesTabelanotaFiscal: {
        perPage: 1000,
        headings: {
          oc: "OC",
          obra: "Obra",
          fornecedor: "Fornecedor",
          valor: "Valor Pedido",
          data: "Data Emissão Pedido",
          nf: "NF",
          valorNF: "Valor NF",
          vencimentoNF: "Vencimento NF",
          saldoPedido: "Saldo Pedido",
          acoes: "Ações"
        },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
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

      columnsParcelas: ["parcela", "codigo", "valor", "dataPagamento"],

      optionsParcelas: {
        perPage: 1000,
        headings: {
          parcela: "Parcela",
          codigo: "Código",
          valor: "Valor",
          dataPagamento: "Data Pagamento",
        },
        clientSorting: false,
        sortable: [],
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

      columnsArquivos: ["nome", "usuarioCadastro", "dataCadastro", "acoes"],

      optionsArquivos: {
        perPage: 1000,
        headings: {
          nome: "Nome",
          usuarioCadastro: "Usuário Cadastro",
          dataCadastro: "Data Cadastro",
          acoes: "Ações",
        },
        clientSorting: false,
        sortable: [],
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
  computed: {
    badgeSolicitacoesStatus8: function () {
      return this.modalMinhasSolicitacoes_Solicitacoes.filter(
        (s) => s.idStatusSolicitacaoCompra == 8
      ).length;
    },
    // Número do primeiro item visível na página atual (1-indexed). Ex: página 2, take 10 → 11
    minhasSolicitacoes_InicioPagina: function () {
      if (this.modalMinhasSolicitacoes_Count === 0) return 0;
      return (this.modalMinhasSolicitacoes_PaginaAtual - 1) * this.modalMinhasSolicitacoes_Take + 1;
    },
    // Número do último item visível na página atual. Ex: página 2, take 10, count 15 → 15
    minhasSolicitacoes_FimPagina: function () {
      var fim = this.modalMinhasSolicitacoes_PaginaAtual * this.modalMinhasSolicitacoes_Take;
      return fim > this.modalMinhasSolicitacoes_Count ? this.modalMinhasSolicitacoes_Count : fim;
    },
  },
  methods: {
    // ============================================================
    // Ações das tabelas (usadas pelo <ModalAcoes>).
    // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
    // ============================================================
    montaAcoesGerenciarSolicitacao() {
      const u = this.usuarioDTO;
      const s = this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada;
      const podeValidar =
        u != null &&
        u.comprasOrdensCompraValidacaoValidar &&
        s != null &&
        s.idStatusSolicitacaoCompra == 1 &&
        s.idUsuarioCadastro != u.id;
      return [
        {
          label: "Cancelar Solicitação",
          descricao: "Cancelar a solicitação de compra",
          icone: "times",
          variante: "danger",
          onClick: () => { this.abrirModalCancelarSolicitacao(); },
          visible: u != null && u.comprasOrdensCompraCancelar && s != null,
        },
        {
          label: "Salvar Solicitação",
          descricao: "Gravar as alterações da solicitação",
          icone: "save",
          variante: "success",
          onClick: () => this.editarSolicitacaoCompra(),
        },
        {
          label: "Aprovar",
          descricao: "Aprovar a solicitação",
          icone: "check",
          variante: "success",
          onClick: () => {
            this.solicitacaoParaValidacao = s;
            this.validarSolicitacao(s.id, s.codigo);
          },
          visible: podeValidar,
        },
        {
          label: "Enviar para Revisão",
          descricao: "Devolver para o solicitante revisar",
          icone: "reply",
          variante: "warning",
          onClick: () => this.enviarParaRevisao(s.id),
          visible:
            s != null &&
            s.idStatusSolicitacaoCompra == 1 &&
            u != null &&
            s.idUsuarioCadastro != u.id,
        },
        {
          label: "Enviar para Aprovação",
          descricao: "Submeter a solicitação ao aprovador",
          icone: "paper-plane",
          variante: "primary",
          onClick: () => this.enviarParaAprovacao(s.id),
          visible: s != null && s.idStatusSolicitacaoCompra == 8,
        },
      ];
    },
    abrirModalAcoesGerenciarSolicitacao() {
      this.modalAcoes_Itens = this.montaAcoesGerenciarSolicitacao();
      this.modalAcoes_Titulo = "Solicitação de Compra";
      this.modalAcoes_Exibir = true;
    },

    abrirModalCancelarSolicitacao() {
      const s = this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada;
      if (s == null) {
        this.$swal("Atenção", "Nenhuma solicitação selecionada.", "warning");
        return;
      }
      this.solicitacaoParaCancelamento = s;
      this.modalCancelarSolicitacao_Motivo = "";
      this.modalCancelarSolicitacao_Titulo = "Cancelar solicitação " + s.codigo;
      this.modalCancelarSolicitacao_Exibir = true;
    },

    confirmarCancelamentoSolicitacao() {
      if (this.solicitacaoParaCancelamento == null) {
        this.$swal("Atenção", "Nenhuma solicitação selecionada.", "warning");
        return;
      }

      const motivo = (this.modalCancelarSolicitacao_Motivo || "").trim();
      if (motivo.length === 0) {
        this.$swal("Atenção", "Informe o motivo do cancelamento.", "warning");
        return;
      }

      this.$swal.fire({
        title: "Atenção",
        text: "Deseja realmente cancelar a solicitação " + this.solicitacaoParaCancelamento.codigo + "?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sim, cancelar",
        cancelButtonText: "Voltar",
        reverseButtons: true,
        allowOutsideClick: false,
      }).then((res) => {
        if (!res.isConfirmed && !res.value) return;

        this.isLoading = true;

        ApiService.cancelarSolicitacaoCompra(
          this.solicitacaoParaCancelamento.id,
          motivo,
          (result) => {
            this.isLoading = false;

            if (result.status != 200) {
              this.$swal("", result.message, "error");
              return;
            }

            this.$swal(
              "Solicitação cancelada",
              "A solicitação " + this.solicitacaoParaCancelamento.codigo + " foi cancelada com sucesso.",
              "success"
            );

            // limpa estado do modal de cancelamento
            this.modalCancelarSolicitacao_Exibir = false;
            this.modalCancelarSolicitacao_Motivo = "";
            this.modalCancelarSolicitacao_Titulo = "";
            this.solicitacaoParaCancelamento = null;

            // fecha o modal de gerenciar
            this.modalGerenciarSolicitacaoCompra_Exibir = false;

            // atualiza a listagem de solicitações pendentes e o contador de quantidades
            this.obtemSolicitacoesCompraParaValidacao();
            if (typeof this.obtemQuantidades === "function") {
              this.obtemQuantidades();
            }
          }
        );
      });
    },

    montaAcoesMinhaSolicitacao(row) {
      return [
        {
          label: "Editar",
          descricao: "Editar a solicitação",
          icone: "pencil",
          variante: "primary",
          onClick: () => {
            this.modalGerenciarSolicitacaoCompra_Exibir = true;
            this.preencherModalGerenciarSolicitacaoCompra(row);
          },
          visible:
            row.idStatusSolicitacaoCompra == 1 ||
            (row.idStatusSolicitacaoCompra == 8 && row.servico == false),
        },
        {
          label: "Editar",
          descricao: "Editar o serviço",
          icone: "pencil",
          variante: "primary",
          onClick: () => {
            this.modalGerenciarSolicitacaoCompra_Exibir = true;
            this.redirecionarParaEdicaoDeServico(row);
          },
          visible: row.idStatusSolicitacaoCompra == 8 && row.servico == true,
        },
        {
          label: "PDF Solicitação",
          descricao: "Baixar PDF da solicitação",
          icone: "file-pdf-o",
          onClick: () =>
            this.downloadPdfArquivoSolicitacaoCompra(row.id, row.codigo + ".pdf"),
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos",
          icone: "paperclip",
          onClick: () => {
            this.modalArquivos_Exibir = true;
            this.modalArquivos_IdSolicitacaoOuPedidoCompra = row.id;
            this.modalArquivos_Controller = "SolicitacaoCompra";
            this.modalArquivos_Arquivos = row.arquivos;
          },
        },
        {
          label: "Comentários",
          descricao: "Ver/adicionar comentários",
          icone: "comment",
          onClick: () => {
            this.modalComentarios_Exibir = true;
            this.modalComentarios_Controller = "SolicitacaoCompra";
            this.modalComentarios_IdSolicitacaoOuPedidoCompra = row.id;
            this.modalComentarios_Comentarios = row.comentarios;
          },
        },
        {
          label: "Enviar para aprovação",
          descricao: "Submeter a solicitação ao aprovador",
          icone: "paper-plane",
          variante: "success",
          onClick: () => this.enviarParaAprovacao(row.id),
          visible: row.idStatusSolicitacaoCompra == 6,
        },
      ];
    },
    abrirModalAcoesMinhaSolicitacao(row) {
      this.modalAcoes_Itens = this.montaAcoesMinhaSolicitacao(row);
      this.modalAcoes_Titulo = "Ações da solicitação " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    montaAcoesMeuPedidoInterno(row) {
      return [
        {
          label: "Informações PI",
          descricao: "Ver detalhes do pedido interno",
          icone: "info-circle",
          variante: "primary",
          onClick: () => this.exibeModalDetalhesPI(row.id),
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos",
          icone: "paperclip",
          onClick: () => {
            this.modalArquivos_Exibir = true;
            this.modalArquivos_IdSolicitacaoOuPedidoCompra = row.id;
            this.modalArquivos_Controller = "PedidoInterno";
            this.modalArquivos_Arquivos = row.arquivos;
          },
        },
        {
          label: "PDF",
          descricao: "Baixar PDF do pedido",
          icone: "file-pdf-o",
          onClick: () => this.downloadPdfPedidoInterno(row.id, true),
        },
      ];
    },
    abrirModalAcoesMeuPedidoInterno(row) {
      this.modalAcoes_Itens = this.montaAcoesMeuPedidoInterno(row);
      this.modalAcoes_Titulo = "Ações do pedido interno";
      this.modalAcoes_Exibir = true;
    },

    montaAcoesAprovacaoNF(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar data vencimento",
          descricao: "Alterar vencimento da NF",
          icone: "calendar",
          variante: "primary",
          onClick: () => this.abrirModalEdicaoDataVencimentoNotasFiscais(row),
        },
        {
          label: "Aprovar",
          descricao: "Aprovar a nota fiscal",
          icone: "check",
          variante: "success",
          onClick: () => this.aprovarReprovarNotaFiscal(row.pedidoCompraNotaFiscal.id, true),
        },
        {
          label: "Reprovar",
          descricao: "Reprovar a nota fiscal",
          icone: "times",
          variante: "danger",
          onClick: () => this.aprovarReprovarNotaFiscal(row.pedidoCompraNotaFiscal.id, false),
        },
        {
          label: "Arquivo Nota Fiscal",
          descricao: "Baixar arquivo da NF",
          icone: "download",
          onClick: () =>
            this.downloadFile(
              "PedidoCompra",
              row.pedidoCompraNotaFiscal.idPedidoCompraArquivo,
              row.pedidoCompraNotaFiscal.nome
            ),
          visible: u != null && u.comprasOrdensCompraEmCompraPDF,
        },
        {
          label: "PDF Pedido Compra",
          descricao: "Baixar PDF do pedido de compra",
          icone: "file-pdf-o",
          onClick: () =>
            this.downloadPdfArquivoCompra(
              row.pedidoCompraNotaFiscal.idPedidoCompra,
              "PedidoCompra.pdf"
            ),
          visible: u != null && u.comprasOrdensCompraEmCompraPDF,
        },
      ];
    },
    abrirModalAcoesAprovacaoNF(row) {
      this.modalAcoes_Itens = this.montaAcoesAprovacaoNF(row);
      this.modalAcoes_Titulo = "Ações da nota fiscal";
      this.modalAcoes_Exibir = true;
    },

    montaAcoesAprovacaoPI(row) {
      return [
        {
          label: "Informações PI",
          descricao: "Ver detalhes do pedido interno",
          icone: "info-circle",
          variante: "primary",
          onClick: () => this.exibeModalDetalhesPI(row.id),
        },
        {
          label: "Aprovar",
          descricao: "Aprovar o pedido interno",
          icone: "check",
          variante: "success",
          onClick: () => this.aprovarReprovarPI(row.id, true),
        },
        {
          label: "Reprovar",
          descricao: "Reprovar o pedido interno",
          icone: "times",
          variante: "danger",
          // NOTA: no original, "Reprovar" também chamava aprovarReprovarPI(row.id, true)
          // — provável bug, mas preservado para não mudar comportamento sem aviso.
          onClick: () => this.aprovarReprovarPI(row.id, true),
        },
      ];
    },
    abrirModalAcoesAprovacaoPI(row) {
      this.modalAcoes_Itens = this.montaAcoesAprovacaoPI(row);
      this.modalAcoes_Titulo = "Ações do pedido interno";
      this.modalAcoes_Exibir = true;
    },

    montaAcoesSolicitacaoParaAprovacao(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Gerenciar",
          descricao: "Abrir para gerenciar a solicitação",
          icone: "cog",
          variante: "primary",
          onClick: () => {
            this.modalGerenciarSolicitacaoCompra_Exibir = true;
            this.preencherModalGerenciarSolicitacaoCompra(row);
          },
          visible: u != null && u.comprasOrdensCompraValidacaoGerenciar,
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos",
          icone: "paperclip",
          onClick: () => {
            this.modalArquivos_Exibir = true;
            this.modalArquivos_IdSolicitacaoOuPedidoCompra = row.id;
            this.modalArquivos_Controller = "SolicitacaoCompra";
            this.modalArquivos_Arquivos = row.arquivos;
          },
          visible: u != null && u.comprasOrdensCompraAnexos,
        },
        {
          label: "Comentários",
          descricao: "Ver/adicionar comentários",
          icone: "comment",
          onClick: () => {
            this.modalComentarios_Exibir = true;
            this.modalComentarios_Controller = "SolicitacaoCompra";
            this.modalComentarios_IdSolicitacaoOuPedidoCompra = row.id;
            this.modalComentarios_Comentarios = row.comentarios;
          },
          visible: u != null && u.comprasOrdensCompraComentarios,
        },
      ];
    },
    abrirModalAcoesSolicitacaoParaAprovacao(row) {
      this.modalAcoes_Itens = this.montaAcoesSolicitacaoParaAprovacao(row);
      this.modalAcoes_Titulo = "Ações da solicitação " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    updateProgress(prevIndex, nextIndex) {
      if (nextIndex >= 0) {
        Vue.set(this, 'progress', nextIndex + 1);
      }
    },

    rowClass(item, type) {
      if (!item || type !== 'row') return
      if (item.vencimentoProximo === true) return 'table-VencimentoProximo'
    },

    enterQuantidade: function () {
      this.$refs.valorUnitario.$el.focus()
    },

    downloadPdfPedidoInterno(id) {
      this.isLoading = true;

      ApiService.downloadPdfPedidoInterno(id, (result) => {
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

    enterValorUnitario: function () {
      this.adicionaMaterial();
    },

    abreModalNovoPedidoInterno(tipo) {
      if (tipo == 'administrativo') {
        this.modalPedidoInternoAdministrativo_DEFs = this.defs.filter(item => { return item.idTipoPedidoInterno == 2 });
        this.modalPedidoInternoAdministrativo_Exibir = true;
      }
      else if(tipo=='geral')
      {
        this.modalPedidoInternoGeral_DEFs = this.defs.filter(item => { return item.idTipoPedidoInterno == 4 });
        this.modalPedidoInternoGeral_Exibir = true;
      }
      else if(tipo=='obra')
      {
        this.modalPedidoInternoObra_DEFs = this.defs.filter(item => { return item.idTipoPedidoInterno == 5 });
        this.modalPedidoInternoObra_Exibir = true;
      }
      else if(tipo=='garantia')
      {
        this.modalPedidoInternoGarantia_DEFs = this.defs.filter(item => { return item.idTipoPedidoInterno == 3 });
        this.modalPedidoInternoGarantia_Exibir = true;
      }
    },

    adicionarPagamentoPedidoInternoAdministrativo() {
      if (this.modalPedidoInternoAdministrativo_ValorPagamento <= 0) {
        this.$swal("Atenção", "Valor inválido", "error");
        return;
      }

      if (this.modalPedidoInternoAdministrativo_DataPagamento == null) {
        this.$swal("Atenção", "Data inválida", "error");
        return;
      }

      this.modalPedidoInternoAdministrativo_Pagamentos.push({
        id: this.modalPedidoInternoAdministrativo_Pagamentos.length + 1,
        data: this.modalPedidoInternoAdministrativo_DataPagamento,
        valor: this.modalPedidoInternoAdministrativo_ValorPagamento
      });

      this.modalPedidoInternoAdministrativo_ValorPagamento = 0.0;
      this.modalPedidoInternoAdministrativo_DataPagamento = null;
    },

    excluirPagamentoModalPedidoInternoAdministrativo(id)
    {
       this.modalPedidoInternoAdministrativo_Pagamentos = this.modalPedidoInternoAdministrativo_Pagamentos.filter(p => p.id !== id);

      this.modalPedidoInternoAdministrativo_Pagamentos.forEach((p, index) => {
        p.id = index + 1;
      });
    },

    realizaPedidoInternoAdministrativo: function () {

      if (this.modalPedidoInternoAdministrativo_DefSelecionado == null) {
        this.$swal("", "DEF inválido", "error");
        return;
      }

      var valorTotal= this.modalPedidoInternoAdministrativo_Valor;
      var valorParcelas = this.modalPedidoInternoAdministrativo_Pagamentos.reduce((total, item) => total + Number(item.valor), 0);

      if (valorTotal != valorParcelas) {
        this.$swal("", "Valor das parcelas diferente do valor total", "error");
        return;
      }

      if (this.modalPedidoInternoAdministrativo_Valor <= 0) {
        this.$swal("", "Valor total inválido", "error");
        return;
      }

      if (this.modalPedidoInternoAdministrativo_Descricao == '') {
          this.$swal("", "Descrição inválida", "error");
          return;

      }

      if(this.modalPedidoInternoAdministrativo_UsuarioAprovador == null)
      {
        this.$swal("", "Selecione um usuário aprovador", "error");
          return;
      }

      var pedidoInterno = {
        id: 0,
        idDef: this.modalPedidoInternoAdministrativo_DefSelecionado.id,
        idUsuarioAprovacao: this.modalPedidoInternoAdministrativo_UsuarioAprovador.id,
        idFornecedorBeneficiario: null,
        idUsuarioBeneficiario: this.usuarioDTO.id,
        idTipoPedidoInterno: 2,
        codigo: 0,
        descricao: this.modalPedidoInternoAdministrativo_Descricao,
        numeroTotalParcelas: this.modalPedidoInternoAdministrativo_Pagamentos.length,
        valorTotal: this.modalPedidoInternoAdministrativo_Valor,
        aprovado: null,
        parcelas: [],
        obras: [],
      };

      // this.modalPedidoInterno_Parcelas.forEach(x => {

      //   var objetoParcela = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     parcela: x.numeroParcela,
      //     valor: x.valor,
      //     dataPagamento: x.data,
      //     pagamentoEfetuado: false,
      //     parcelaObras: [],
      //     parcelaDEFs: []
      //   }

      //   this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaObras.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idObra: y.idObra,
      //       valor: y.valor
      //     })
      //   });

      //   this.modalPedidoInterno_DefsSelecionadosComValor.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaDEFs.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idDef: y.idDef,
      //       valor: y.valor
      //     })
      //   });

      //   pedidoInterno.parcelas.push(objetoParcela);
      // });

      // this.modalPedidoInterno_Obras.forEach(x => {

      //   var objetoObra = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     idObra: x.idObra,
      //     valor: x.valor
      //   };

      //   pedidoInterno.obras.push(objetoObra);
      // });

      this.modalPedidoInternoAdministrativo_Pagamentos.forEach(x=>{
        pedidoInterno.parcelas.push(
          {
            parcela: pedidoInterno.parcelas.length + 1,
            valor: x.valor,
            dataPagamento: x.data
          })
      });

      this.isLoading = true;

      ApiService.post("PedidoInterno", pedidoInterno, (result) => {
        this.isLoading = false;
        if (result.status != 201) {
          this.$swal("", result.message, "error");
        } else {
          this.upload_FilesPedidoInterno.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('PedidoInterno', formData, result.data.id, () => { });
          });
          this.$swal("", 'Pedido interno solicitado', "success");

          this.modalPedidoInternoAdministrativo_DataPagamento=null;
          this.modalPedidoInternoAdministrativo_DefSelecionado=null;
          this.modalPedidoInternoAdministrativo_Descricao='';
          this.modalPedidoInternoAdministrativo_Pagamentos=[];
          this.modalPedidoInternoAdministrativo_UsuarioAprovador=null;
          this.modalPedidoInternoAdministrativo_Valor=0.0;
          this.modalPedidoInternoAdministrativo_ValorPagamento=0.0;
          this.modalPedidoInternoAdministrativo_Exibir = false;
        }
      });

    },

    realizaPedidoInternoGeral: function () {

      if (this.modalPedidoInternoGeral_DefSelecionado == null) {
        this.$swal("", "DEF inválido", "error");
        return;
      }

      var valorTotal= this.modalPedidoInternoGeral_Valor;
      var valorParcelas = this.modalPedidoInternoGeral_Pagamentos.reduce((total, item) => total + Number(item.valor), 0);

      if (valorTotal != valorParcelas) {
        this.$swal("", "Valor das parcelas diferente do valor total", "error");
        return;
      }

      if (this.modalPedidoInternoGeral_Valor <= 0) {
        this.$swal("", "Valor total inválido", "error");
        return;
      }

      if (this.modalPedidoInternoGeral_Descricao == '') {
          this.$swal("", "Descrição inválida", "error");
          return;

      }

      if(this.modalPedidoInternoGeral_UsuarioAprovador == null)
      {
        this.$swal("", "Selecione um usuário aprovador", "error");
          return;
      }

      var pedidoInterno = {
        id: 0,
        idDef: this.modalPedidoInternoGeral_DefSelecionado.id,
        idUsuarioAprovacao: this.modalPedidoInternoGeral_UsuarioAprovador.id,
        idFornecedorBeneficiario: null,
        idUsuarioBeneficiario: this.usuarioDTO.id,
        idTipoPedidoInterno: 2,
        codigo: 0,
        descricao: this.modalPedidoInternoGeral_Descricao,
        numeroTotalParcelas: this.modalPedidoInternoGeral_Pagamentos.length,
        valorTotal: this.modalPedidoInternoGeral_Valor,
        aprovado: null,
        parcelas: [],
        obras: [],
      };

      // this.modalPedidoInterno_Parcelas.forEach(x => {

      //   var objetoParcela = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     parcela: x.numeroParcela,
      //     valor: x.valor,
      //     dataPagamento: x.data,
      //     pagamentoEfetuado: false,
      //     parcelaObras: [],
      //     parcelaDEFs: []
      //   }

      //   this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaObras.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idObra: y.idObra,
      //       valor: y.valor
      //     })
      //   });

      //   this.modalPedidoInterno_DefsSelecionadosComValor.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaDEFs.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idDef: y.idDef,
      //       valor: y.valor
      //     })
      //   });

      //   pedidoInterno.parcelas.push(objetoParcela);
      // });

      // this.modalPedidoInterno_Obras.forEach(x => {

      //   var objetoObra = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     idObra: x.idObra,
      //     valor: x.valor
      //   };

      //   pedidoInterno.obras.push(objetoObra);
      // });

      this.modalPedidoInternoGeral_Pagamentos.forEach(x=>{
        pedidoInterno.parcelas.push(
          {
            parcela: pedidoInterno.parcelas.length + 1,
            valor: x.valor,
            dataPagamento: x.data
          })
      });

      this.isLoading = true;

      ApiService.post("PedidoInterno", pedidoInterno, (result) => {
        this.isLoading = false;
        if (result.status != 201) {
          this.$swal("", result.message, "error");
        } else {
          this.upload_FilesPedidoInterno.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('PedidoInterno', formData, result.data.id, () => { });
          });
          this.$swal("", 'Pedido interno solicitado', "success");

          this.modalPedidoInternoGeral_DataPagamento=null;
          this.modalPedidoInternoGeral_DefSelecionado=null;
          this.modalPedidoInternoGeral_Descricao='';
          this.modalPedidoInternoGeral_Pagamentos=[];
          this.modalPedidoInternoGeral_UsuarioAprovador=null;
          this.modalPedidoInternoGeral_Valor=0.0;
          this.modalPedidoInternoGeral_ValorPagamento=0.0;
          this.modalPedidoInternoGeral_Exibir = false;
        }
      });

    },

    realizaPedidoInternoObra: function () {

      if (this.modalPedidoInternoObra_DefSelecionado == null) {
        this.$swal("", "DEF inválido", "error");
        return;
      }

      if (this.modalPedidoInternoObra_ObraSelecionada == null) {
        this.$swal("", "Obra inválida", "error");
        return;
      }

      var valorTotal= this.modalPedidoInternoObra_Valor;
      var valorParcelas = this.modalPedidoInternoObra_Pagamentos.reduce((total, item) => total + Number(item.valor), 0);

      if (valorTotal != valorParcelas) {
        this.$swal("", "Valor das parcelas diferente do valor total", "error");
        return;
      }

      if (this.modalPedidoInternoObra_Valor <= 0) {
        this.$swal("", "Valor total inválido", "error");
        return;
      }

      if (this.modalPedidoInternoObra_Descricao == '') {
          this.$swal("", "Descrição inválida", "error");
          return;

      }

      var pedidoInterno = {
        id: 0,
        idDef: this.modalPedidoInternoObra_DefSelecionado.id,
        idObra: this.modalPedidoInternoObra_ObraSelecionada.id,
        idUsuarioAprovacao: 0,
        idFornecedorBeneficiario: null,
        idUsuarioBeneficiario: this.usuarioDTO.id,
        idTipoPedidoInterno: 2,
        codigo: 0,
        descricao: this.modalPedidoInternoObra_Descricao,
        numeroTotalParcelas: this.modalPedidoInternoObra_Pagamentos.length,
        valorTotal: this.modalPedidoInternoObra_Valor,
        aprovado: null,
        parcelas: [],
        obras: [],
      };

      // this.modalPedidoInterno_Parcelas.forEach(x => {

      //   var objetoParcela = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     parcela: x.numeroParcela,
      //     valor: x.valor,
      //     dataPagamento: x.data,
      //     pagamentoEfetuado: false,
      //     parcelaObras: [],
      //     parcelaDEFs: []
      //   }

      //   this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaObras.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idObra: y.idObra,
      //       valor: y.valor
      //     })
      //   });

      //   this.modalPedidoInterno_DefsSelecionadosComValor.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaDEFs.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idDef: y.idDef,
      //       valor: y.valor
      //     })
      //   });

      //   pedidoInterno.parcelas.push(objetoParcela);
      // });

      // this.modalPedidoInterno_Obras.forEach(x => {

      //   var objetoObra = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     idObra: x.idObra,
      //     valor: x.valor
      //   };

      //   pedidoInterno.obras.push(objetoObra);
      // });

      this.modalPedidoInternoObra_Pagamentos.forEach(x=>{
        pedidoInterno.parcelas.push(
          {
            parcela: pedidoInterno.parcelas.length + 1,
            valor: x.valor,
            dataPagamento: x.data
          })
      });

      this.isLoading = true;

      ApiService.post("PedidoInterno", pedidoInterno, (result) => {
        this.isLoading = false;
        if (result.status != 201) {
          this.$swal("", result.message, "error");
        } else {
          this.upload_FilesPedidoInterno.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('PedidoInterno', formData, result.data.id, () => { });
          });
          this.$swal("", 'Pedido interno solicitado', "success");

          this.modalPedidoInternoObra_DataPagamento=null;
          this.modalPedidoInternoObra_DefSelecionado=null;
          this.modalPedidoInternoObra_ObraSelecionada=null;
          this.modalPedidoInternoObra_Descricao='';
          this.modalPedidoInternoObra_Pagamentos=[];
          this.modalPedidoInternoObra_UsuarioAprovador=null;
          this.modalPedidoInternoObra_Valor=0.0;
          this.modalPedidoInternoObra_ValorPagamento=0.0;
          this.modalPedidoInternoObra_Exibir = false;
        }
      });

    },

    realizaPedidoInternoGarantia: function () {

      if (this.modalPedidoInternoGarantia_DefSelecionado == null) {
        this.$swal("", "DEF inválido", "error");
        return;
      }

      var valorTotal= this.modalPedidoInternoGarantia_Valor;
      var valorParcelas = this.modalPedidoInternoGarantia_Pagamentos.reduce((total, item) => total + Number(item.valor), 0);

      if (valorTotal != valorParcelas) {
        this.$swal("", "Valor das parcelas diferente do valor total", "error");
        return;
      }

      if (this.modalPedidoInternoGarantia_Valor <= 0) {
        this.$swal("", "Valor total inválido", "error");
        return;
      }

      if (this.modalPedidoInternoGarantia_Descricao == '') {
          this.$swal("", "Descrição inválida", "error");
          return;

      }

      if(this.modalPedidoInternoGarantia_UsuarioAprovador == null)
      {
        this.$swal("", "Selecione um usuário aprovador", "error");
          return;
      }

      var pedidoInterno = {
        id: 0,
        idDef: this.modalPedidoInternoGarantia_DefSelecionado.id,
        idUsuarioAprovacao: this.modalPedidoInternoGarantia_UsuarioAprovador.id,
        idFornecedorBeneficiario: null,
        idUsuarioBeneficiario: this.usuarioDTO.id,
        idTipoPedidoInterno: 2,
        codigo: 0,
        descricao: this.modalPedidoInternoGarantia_Descricao,
        numeroTotalParcelas: this.modalPedidoInternoGarantia_Pagamentos.length,
        valorTotal: this.modalPedidoInternoGarantia_Valor,
        aprovado: null,
        parcelas: [],
        obras: [],
      };

      // this.modalPedidoInterno_Parcelas.forEach(x => {

      //   var objetoParcela = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     parcela: x.numeroParcela,
      //     valor: x.valor,
      //     dataPagamento: x.data,
      //     pagamentoEfetuado: false,
      //     parcelaObras: [],
      //     parcelaDEFs: []
      //   }

      //   this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaObras.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idObra: y.idObra,
      //       valor: y.valor
      //     })
      //   });

      //   this.modalPedidoInterno_DefsSelecionadosComValor.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
      //     objetoParcela.parcelaDEFs.push({
      //       id: 0,
      //       idPedidoInternoParcela: 0,
      //       idDef: y.idDef,
      //       valor: y.valor
      //     })
      //   });

      //   pedidoInterno.parcelas.push(objetoParcela);
      // });

      // this.modalPedidoInterno_Obras.forEach(x => {

      //   var objetoObra = {
      //     id: 0,
      //     idPedidoInterno: 0,
      //     idObra: x.idObra,
      //     valor: x.valor
      //   };

      //   pedidoInterno.obras.push(objetoObra);
      // });

      this.modalPedidoInternoGarantia_Pagamentos.forEach(x=>{
        pedidoInterno.parcelas.push(
          {
            parcela: pedidoInterno.parcelas.length + 1,
            valor: x.valor,
            dataPagamento: x.data
          })
      });

      this.isLoading = true;

      ApiService.post("PedidoInterno", pedidoInterno, (result) => {
        this.isLoading = false;
        if (result.status != 201) {
          this.$swal("", result.message, "error");
        } else {
          this.upload_FilesPedidoInterno.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('PedidoInterno', formData, result.data.id, () => { });
          });
          this.$swal("", 'Pedido interno solicitado', "success");

          this.modalPedidoInternoGarantia_DataPagamento=null;
          this.modalPedidoInternoGarantia_DefSelecionado=null;
          this.modalPedidoInternoGarantia_Descricao='';
          this.modalPedidoInternoGarantia_Pagamentos=[];
          this.modalPedidoInternoGarantia_UsuarioAprovador=null;
          this.modalPedidoInternoGarantia_Valor=0.0;
          this.modalPedidoInternoGarantia_ValorPagamento=0.0;
          this.modalPedidoInternoGarantia_Exibir = false;
        }
      });

    },

    adicionarPagamentoPedidoInternoGeral() {
      if (this.modalPedidoInternoGeral_ValorPagamento <= 0) {
        this.$swal("Atenção", "Valor inválido", "error");
        return;
      }

      if (this.modalPedidoInternoGeral_DataPagamento == null) {
        this.$swal("Atenção", "Data inválida", "error");
        return;
      }

      this.modalPedidoInternoGeral_Pagamentos.push({
        id: this.modalPedidoInternoGeral_Pagamentos.length + 1,
        data: this.modalPedidoInternoGeral_DataPagamento,
        valor: this.modalPedidoInternoGeral_ValorPagamento
      });

      this.modalPedidoInternoGeral_ValorPagamento = 0.0;
      this.modalPedidoInternoGeral_DataPagamento = null;
    },

    adicionarPagamentoPedidoInternoObra(){
      if (this.modalPedidoInternoObra_ValorPagamento <= 0) {
        this.$swal("Atenção", "Valor inválido", "error");
        return;
      }

      if (this.modalPedidoInternoObra_DataPagamento == null) {
        this.$swal("Atenção", "Data inválida", "error");
        return;
      }

      this.modalPedidoInternoObra_Pagamentos.push({
        id: this.modalPedidoInternoObra_Pagamentos.length + 1,
        data: this.modalPedidoInternoObra_DataPagamento,
        valor: this.modalPedidoInternoObra_ValorPagamento
      });

      this.modalPedidoInternoObra_ValorPagamento = 0.0;
      this.modalPedidoInternoObra_DataPagamento = null;
    },

    adicionarPagamentoPedidoInternoGarantia(){
      if (this.modalPedidoInternoGarantia_ValorPagamento <= 0) {
        this.$swal("Atenção", "Valor inválido", "error");
        return;
      }

      if (this.modalPedidoInternoGarantia_DataPagamento == null) {
        this.$swal("Atenção", "Data inválida", "error");
        return;
      }

      this.modalPedidoInternoGarantia_Pagamentos.push({
        id: this.modalPedidoInternoGarantia_Pagamentos.length + 1,
        data: this.modalPedidoInternoGarantia_DataPagamento,
        valor: this.modalPedidoInternoGarantia_ValorPagamento
      });

      this.modalPedidoInternoGarantia_ValorPagamento = 0.0;
      this.modalPedidoInternoGarantia_DataPagamento = null;
    },

    excluirPagamentoModalPedidoInternoObra(id)
    {
       this.modalPedidoInternoObra_Pagamentos = this.modalPedidoInternoObra_Pagamentos.filter(p => p.id !== id);

      this.modalPedidoInternoObra_Pagamentos.forEach((p, index) => {
        p.id = index + 1;
      });
    },

    excluirPagamentoModalPedidoInternoGarantia(id)
    {
       this.modalPedidoInternoGarantia_Pagamentos = this.modalPedidoInternoGarantia_Pagamentos.filter(p => p.id !== id);

      this.modalPedidoInternoGarantia_Pagamentos.forEach((p, index) => {
        p.id = index + 1;
      });
    },

    excluirPagamentoModalPedidoInternoGeral(id)
    {
       this.modalPedidoInternoGeral_Pagamentos = this.modalPedidoInternoGeral_Pagamentos.filter(p => p.id !== id);

      this.modalPedidoInternoGeral_Pagamentos.forEach((p, index) => {
        p.id = index + 1;
      });
    },

    buscaMateriaisComNomeInformado(searchTerm) {
      // Cancela qualquer timeout anterior
      clearTimeout(this.searchTimeout);

      // Se o termo for vazio, não faz requisição
      if (!searchTerm || searchTerm.length < 2) {
        this.materiaisFiltradosPorTexto = [];
        return;
      }

      // Define novo timeout de 3 segundos
      this.searchTimeout = setTimeout(() => {

        this.isLoading = true;

        ApiService.buscaMaterialPorTexto(searchTerm, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", "Erro ao buscar materiais", "error");
          } else {

            this.materiaisFiltradosPorTexto = result.data;

          }
        });

      }, 500);
    },

    continuaRascunho(objetoRascunho) {

      this.rascunho = objetoRascunho;

      var objetoSerializado = JSON.parse(objetoRascunho.objetoSerializado);

      if (objetoSerializado.servico) {
        this.modalNovoPedidoServico_ObraSelecionada = objetoSerializado.obra;
        this.modalNovoPedidoServico_DataEntrega = objetoSerializado.dataEntrega;
        this.modalNovoPedidoServico_Titulo = objetoSerializado.titulo;
        this.modalNovoPedidoServico_CustoOrcado = objetoSerializado.custoOrcado;
        this.modalNovoPedidoServico_ValorTotal = objetoSerializado.valorTotal;
        this.modalNovoPedidoServico_Observacao = objetoSerializado.descricao;
        this.modalNovoPedidoServico_CompradorSelecionado = objetoSerializado.comprador;
        this.modalNovoPedidoServico_FornecedorSelecionado = objetoSerializado.fornecedor;
        this.modalNovoPedidoServico_PagamentosServico = objetoSerializado.pagamentos;

        this.modalNovoPedidoServico_Exibir = true;

      }
      else {
        this.modalNovoPedidoMaterial_ObraSelecionada = objetoSerializado.obra;
        this.modalNovoPedidoMaterial_DataEntrega = objetoSerializado.dataEntrega;
        this.modalNovoPedidoMaterial_Titulo = objetoSerializado.titulo;
        this.modalNovoPedidoMaterial_TipoValor = objetoSerializado.tipoValor;
        this.modalNovoPedidoMaterial_ValorTotal = objetoSerializado.valorTotal;
        this.materiaisAdicionados = objetoSerializado.materiais;

        this.modalNovoPedidoMaterial_Exibir = true;
      }

    },

    excluiRascunho(id) {
      this.isLoading = true;

      ApiService.excluiRascunho(id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.data, "error");
        } else {
          this.obtemRascunhos();
        }
      });
    },

    salvarRascunhoServico() {
      var objetoRascunho = {};
      objetoRascunho.servico = true;
      objetoRascunho.obra = this.modalNovoPedidoServico_ObraSelecionada;
      objetoRascunho.dataEntrega = this.modalNovoPedidoServico_DataEntrega;
      objetoRascunho.titulo = this.modalNovoPedidoServico_Titulo;
      objetoRascunho.custoOrcado = this.modalNovoPedidoServico_CustoOrcado;
      objetoRascunho.valorTotal = this.modalNovoPedidoServico_ValorTotal;
      objetoRascunho.descricao = this.modalNovoPedidoServico_Observacao;
      objetoRascunho.comprador = this.modalNovoPedidoServico_CompradorSelecionado;
      objetoRascunho.fornecedor = this.modalNovoPedidoServico_FornecedorSelecionado;
      objetoRascunho.pagamentos = this.modalNovoPedidoServico_PagamentosServico;

      var objeto = {};

      if (this.rascunho != null)
        objeto = this.rascunho;

      objeto.titulo = this.modalNovoPedidoServico_Titulo;
      objeto.objetoSerializado = JSON.stringify(objetoRascunho);

      this.isLoading = true;

      if (this.rascunho == null) {
        ApiService.salvaRascunho(objeto, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", result.data, "error");
          } else {
            this.obtemRascunhos();
            this.rascunho = null;
            this.modalNovoPedidoServico_Exibir = false;
          }
        });
      }
      else {
        ApiService.atualizaRascunho(objeto, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", result.data, "error");
          } else {
            this.obtemRascunhos();
            this.rascunho = null;
            this.modalNovoPedidoServico_Exibir = false;
          }
        });
      }
    },

    salvarRascunhoMaterial() {
      var objetoRascunho = {};
      objetoRascunho.servico = false;
      objetoRascunho.obra = this.modalNovoPedidoMaterial_ObraSelecionada;
      objetoRascunho.dataEntrega = this.modalNovoPedidoMaterial_DataEntrega;
      objetoRascunho.titulo = this.modalNovoPedidoMaterial_Titulo;
      objetoRascunho.tipoValor = this.modalNovoPedidoMaterial_TipoValor;
      objetoRascunho.valorTotal = this.modalNovoPedidoMaterial_ValorTotal;
      objetoRascunho.materiais = this.materiaisAdicionados;

      var objeto = {};

      if (this.rascunho != null)
        objeto = this.rascunho;

      objeto.titulo = this.modalNovoPedidoMaterial_Titulo;
      objeto.objetoSerializado = JSON.stringify(objetoRascunho);

      this.isLoading = true;

      if (this.rascunho == null) {
        ApiService.salvaRascunho(objeto, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", result.data, "error");
          } else {
            this.obtemRascunhos();
            this.rascunho = null;
            this.modalNovoPedidoMaterial_Exibir = false;
          }
        });
      }
      else {
        ApiService.atualizaRascunho(objeto, (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", result.data, "error");
          } else {
            this.obtemRascunhos();
            this.rascunho = null;
            this.modalNovoPedidoMaterial_Exibir = false;
          }
        });
      }
    },

    obtemRascunhos() {

      this.isLoading = true;

      ApiService.obtemRascunhos((result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.data, "error");
        } else {
          this.rascunhosSolicitacoes = result.data;
        }
      });
    },

    abrirModalEdicaoDataVencimentoNotasFiscais(row) {
      this.isLoading = true;

      ApiService.obtemNotasFiscaisAlteracaoData(row.pedidoCompraNotaFiscal.id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", "Erro ao baixar pedido de compra", "error");
        } else {

          this.modalEdicaoDataNotaFiscal_Notas = result.data.data;
          this.modalEdicaoDataNotaFiscal_Exibir = true;
        }
      });
    },

    abreModalClonarSolicitacao(solicitacao) {
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

    abreModalNovaSolicitacao(servicoOuMaterial) {

      this.rascunho = null;

      this.materiaisSelecionados = [];
      this.materiaisAdicionados=[];
      this.modalNovoPedidoMaterial_ObraSelecionada = null;
      this.modalNovoPedidoMaterial_DataEntrega = null;
      this.modalNovoPedidoMaterial_Titulo = '';
      this.modalNovoPedidoMaterial_CustoOrcado = 0.0;
      this.modalNovoPedidoMaterial_ValorTotal = 0.0;
      this.modalNovoPedidoMaterial_Observacao = '';
      this.modalNovoPedidoMaterial_MaterialSelecionado = null;
      this.modalNovoPedidoMaterial_Quantidade = 0;
      this.modalNovoPedidoMaterial_ValorUnitario = 0;
      this.modalNovoPedidoMaterial_TipoValor = true;

      this.modalNovoPedidoServico_ObraSelecionada = null;
      this.modalNovoPedidoServico_DataEntrega = null;
      this.modalNovoPedidoServico_Titulo = '';
      this.modalNovoPedidoServico_CustoOrcado = 0.0;
      this.modalNovoPedidoServico_ValorTotal = 0.0;
      this.modalNovoPedidoServico_Observacao = '';
      this.modalNovoPedidoServico_CompradorSelecionado = null;
      this.modalNovoPedidoServico_FornecedorSelecionado = null;
      this.modalNovoPedidoServico_DataPagamentoServico = null;
      this.modalNovoPedidoServico_ValorPagamentoServico = 0.0;
      this.modalNovoPedidoServico_PagamentosServico = [];

      this.tipoNovaSolicitacao = servicoOuMaterial ? 'Servico' : 'Material';


      if (servicoOuMaterial == true) {
        this.modalNovoPedidoServico_Exibir = true;
      }
      else {
        this.modalNovoPedidoMaterial_Exibir = true;
      }
    },

    redirecionaAprovarCotacoes() {
      let routeData = this.$router.resolve({
        name: "OrdemCompra",
        params: { tag: "ParaAprovacao" },
      });
      window.open(routeData.href, '_self');
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

    onChange() {
      this.upload_Files = [...this.$refs.file.files];
    },

    onChangePedidoInterno() {
      this.upload_FilesPedidoInterno = [...this.$refs.filePedidoInterno.files];
    },

    defEscolhido() {

      this.modalPedidoInterno_HabilitaSelecaoObra = false;

      this.modalPedidoInterno_DefsSelecionados.forEach(x => {
        if (x.codigo == '03.01' || x.codigo == '03.02' || x.codigo == '03.03')
          this.modalPedidoInterno_HabilitaSelecaoObra = true;
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

    removerArquivos() {
      this.upload_Files = [];
    },

    removerArquivosPedidoInterno() {
      this.upload_FilesPedidoInterno = [];
    },

    formataData: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
    },

    formataDataSemHora: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY");
    },

    adicionaPagamentoServico() {

      if (this.modalNovoPedidoServico_DataPagamentoServico == null) {
        this.$swal("", "Informe uma data válida", "error");
        return;
      }

      if (this.modalNovoPedidoServico_ValorPagamentoServico <= 0) {
        this.$swal("", "Informe um valor válido", "error");
        return;
      }

      this.modalNovoPedidoServico_PagamentosServico.push(
        {
          id: this.modalNovoPedidoServico_PagamentosServico.length + 1,
          data: this.modalNovoPedidoServico_DataPagamentoServico,
          valor: this.modalNovoPedidoServico_ValorPagamentoServico
        }
      );

      this.modalNovoPedidoServico_DataPagamentoServico = null;
      this.modalNovoPedidoServico_ValorPagamentoServico = 0.0;

    },

    removePagamentoSolicitacaoServico(id) {
      this.modalNovoPedidoServico_PagamentosServico.forEach((pagamento) => {
        if (pagamento.id == id) {

          this.modalNovoPedidoServico_PagamentosServico.splice(this.modalNovoPedidoServico_PagamentosServico.indexOf(pagamento), 1);
        }
      });
    },

    downloadFile(controller, id, nomeArquivo) {
      this.isLoading = true;

      ApiService.downloadFile(controller, id, (result) => {
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

    exibeModalDetalhesPI(id) {
      ApiService.get('PedidoInterno', id, (result) => {
        if (result.status != 200) {
          this.$swal("Erro ao obter entradas", result.message, "error");
        } else {
          this.modalDetalhePI_PI = result.data;
          this.modalDetalhePI_Titulo = 'Detalhe PI - ' + result.data.codigoFormatado
          this.modalDetalhePI_Exibir = true;

          console.log(this.modalDetalhePI_PI);
        }
      });
    },

    adicionarObra() {
      if (this.modalPedidoInterno_ValorTotalObras + this.modalPedidoInterno_ValorObra > this.modalPedidoInterno_Valor) {
        this.$swal("", "Valor das obras maior que o valor total do pedido", "error");
        return;
      }

      if (this.modalPedidoInterno_ValorObra == 0) {
        this.$swal("", "Valor da obra inválido", "error");
        return;
      }

      var obraJaAdicionada = this.modalPedidoInterno_Obras.filter(item => { return item.idObra == this.modalPedidoInterno_ObraSelecionada.id });

      if (obraJaAdicionada.length > 0) {
        this.$swal("", "Obra já adicionada", "error");
        return;
      }

      this.modalPedidoInterno_Obras.push({ idObra: this.modalPedidoInterno_ObraSelecionada.id, valor: this.modalPedidoInterno_ValorObra, codigo: this.modalPedidoInterno_ObraSelecionada.codigo });
      this.modalPedidoInterno_ValorTotalObras = this.modalPedidoInterno_ValorTotalObras + this.modalPedidoInterno_ValorObra;
      this.modalPedidoInterno_ValorObra = 0;
      this.modalPedidoInterno_ObraSelecionada = null;
    },

    adicionarValorAoDef() {

      if (this.modalPedidoInterno_ParcelaSelecionadaDefParcela == null) {
        this.$swal("", "Informe uma parcela válida", "error");
        return;
      }

      if (this.modalPedidoInterno_DefSelecionado == null) {
        this.$swal("", "Informe um DEF válido", "error");
        return;
      }

      if (this.modalPedidoInterno_DefSelecionadoValor <= 0) {
        this.$swal("", "Informe um valor válido", "error");
        return;
      }

      var defExistente = false;

      this.modalPedidoInterno_DefsSelecionadosComValor.forEach(x => {
        if (x.idDef == this.modalPedidoInterno_DefSelecionado.id && x.numeroParcela == this.modalPedidoInterno_ParcelaSelecionadaDefParcela.numeroParcela)
          defExistente = true;
      });

      if (defExistente) {
        this.$swal("", "DEF já existente para a parcela", "error");
        return;
      }

      this.modalPedidoInterno_DefSelecionado.valor = this.modalPedidoInterno_DefSelecionadoValor;
      this.modalPedidoInterno_DefsSelecionadosComValor.push({
        def: this.modalPedidoInterno_DefSelecionado.codigo,
        idDef: this.modalPedidoInterno_DefSelecionado.id,
        numeroParcela: this.modalPedidoInterno_ParcelaSelecionadaDefParcela.numeroParcela,
        valor: this.modalPedidoInterno_DefSelecionadoValor
      });

      this.modalPedidoInterno_ParcelaSelecionadaDefParcela = null;
      this.modalPedidoInterno_DefSelecionado = null;
      this.modalPedidoInterno_DefSelecionadoValor = 0;

    },

    adicionarParcela() {

      if (this.modalPedidoInterno_ValorTotalParcelas + this.modalPedidoInterno_ValorParcela > this.modalPedidoInterno_Valor) {
        this.$swal("", "Valor das parcelas maior que o valor total do pedido", "error");
        return;
      }

      if (this.modalPedidoInterno_DataPagamentoParcela == null) {
        this.$swal("", "Informe uma data de pagamento válida", "error");
        return;
      }
      this.modalPedidoInterno_Parcelas.push({ id: this.modalPedidoInterno_NumeroDaParcela, numeroParcela: this.modalPedidoInterno_NumeroDaParcela, data: this.modalPedidoInterno_DataPagamentoParcela, valor: this.modalPedidoInterno_ValorParcela });
      this.modalPedidoInterno_ValorTotalParcelas = this.modalPedidoInterno_ValorTotalParcelas + this.modalPedidoInterno_ValorParcela;
      this.modalPedidoInterno_NumeroDaParcela++;
      this.modalPedidoInterno_DataPagamentoMinimaParcela = this.modalPedidoInterno_DataPagamentoParcela;
      this.modalPedidoInterno_ValorParcela = 0;

    },

    adicionarParcelaValorObra() {
      var obraParcelaJaAdicionada = this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == this.modalPedidoInterno_ParcelaSelecionadaParcela.numeroParcela && item.idObra == this.modalPedidoInterno_ObraSelecionadaParcela.idObra });

      if (obraParcelaJaAdicionada.length > 0) {

        this.$swal("", "Já existe um valor cadastrado para esta obra e parcela", "error");
        return;
      }

      this.modalPedidoInterno_ObrasParcelas.push({ numeroParcela: this.modalPedidoInterno_ParcelaSelecionadaParcela.numeroParcela, idObra: this.modalPedidoInterno_ObraSelecionadaParcela.idObra, codigoObra: this.modalPedidoInterno_ObraSelecionadaParcela.codigo, valor: this.modalPedidoInterno_ValorObraParcela });
      this.modalPedidoInterno_ParcelaSelecionadaParcela = null;
      this.modalPedidoInterno_ObraSelecionadaParcela = null;
      this.modalPedidoInterno_ValorObraParcela = 0.0;
    },

    limparValoresDef() {
      this.modalPedidoInterno_DefsSelecionadosComValor = [];
    },

    limparParcelas() {
      this.modalPedidoInterno_Parcelas = [];
      this.modalPedidoInterno_ValorParcela = 0;
      this.modalPedidoInterno_NumeroDaParcela = 1;
      this.modalPedidoInterno_ValorTotalParcelas = 0.0;
      this.modalPedidoInterno_DataPagamentoParcela = null;
      this.modalPedidoInterno_DataPagamentoMinimaParcela = new Date((new Date()).setDate(new Date().getDate() - 1));
    },

    limparObras() {
      this.modalPedidoInterno_Obras = [];
      this.modalPedidoInterno_ValorTotalObras = 0.0;
    },

    obtemProximoCodigoPedidoInterno() {

      this.isLoading = true;

      ApiService.obtemProximoCodigoPedidoInterno((result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.data, "error");
        } else {
          this.modalPedidoInterno_Titulo = 'Novo Pedido Interno - ' + result.data;
        }
      });
    },

    postPedidoInterno: function () {

      if (this.modalPedidoInterno_DefsSelecionados == null) {
        this.$swal("", "DEF inválido", "error");
        return;
      }

      if (this.modalPedidoInterno_HabilitaSelecaoObra == true && this.modalPedidoInterno_Obras.length == 0) {
        this.$swal("", "Informe ao menos uma obra", "error");
        return;
      }

      if (this.modalPedidoInterno_Valor <= 0) {
        this.$swal("", "Valor inválido", "error");
        return;
      }

      if (this.modalPedidoInterno_UsuarioAprovador == null) {
        this.$swal("", "Usuário aprovador inválido", "error");
        return;
      }

      if (this.modalPedidoInterno_FornecedorEhBeneficiario == true) {
        if (this.modalPedidoInterno_FornecedorBeneficiario == null) {
          this.$swal("", "Fornecedor beneficiário inválido", "error");
          return;
        }
      }
      else {
        if (this.modalPedidoInterno_UsuarioBeneficiario == null) {
          this.$swal("", "Usuário beneficiário inválido", "error");
          return;
        }
      }

      if (this.modalPedidoInterno_Parcelas.length == 0) {
        this.$swal("", "Informe ao menos uma parcela", "error");
        return;
      }

      if (this.modalPedidoInterno_HabilitaSelecaoObra == true && this.modalPedidoInterno_ObrasParcelas.length == 0) {
        this.$swal("", "Informe corretamente os valores de obras para as parcelas", "error");
        return;
      }

      if (this.modalPedidoInterno_Descricao == '') {
        this.$swal("", "Descrição inválida", "error");
        return;
      }

      if (this.modalPedidoInterno_ValorTotalParcelas > this.modalPedidoInterno_Valor) {
        this.$swal("", "Valor total das parcelas é maior que o valor do pedido", "error");
        return;
      }


      var pedidoInterno = {
        id: 0,
        idDef: this.modalPedidoInterno_DefsSelecionados.id,
        idUsuarioAprovacao: this.modalPedidoInterno_UsuarioAprovador.id,
        idFornecedorBeneficiario: this.modalPedidoInterno_FornecedorEhBeneficiario ? this.modalPedidoInterno_FornecedorBeneficiario.id : null,
        idUsuarioBeneficiario: this.modalPedidoInterno_FornecedorEhBeneficiario == false ? this.modalPedidoInterno_UsuarioBeneficiario.id : null,
        codigo: 0,
        descricao: this.modalPedidoInterno_Descricao,
        numeroTotalParcelas: this.modalPedidoInterno_Parcelas.length,
        valorTotal: this.modalPedidoInterno_Valor,
        aprovado: null,
        parcelas: [],
        obras: [],
      };

      this.modalPedidoInterno_Parcelas.forEach(x => {

        var objetoParcela = {
          id: 0,
          idPedidoInterno: 0,
          parcela: x.numeroParcela,
          valor: x.valor,
          dataPagamento: x.data,
          pagamentoEfetuado: false,
          parcelaObras: [],
          parcelaDEFs: []
        }

        this.modalPedidoInterno_ObrasParcelas.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
          objetoParcela.parcelaObras.push({
            id: 0,
            idPedidoInternoParcela: 0,
            idObra: y.idObra,
            valor: y.valor
          })
        });

        this.modalPedidoInterno_DefsSelecionadosComValor.filter(item => { return item.numeroParcela == x.numeroParcela }).forEach(y => {
          objetoParcela.parcelaDEFs.push({
            id: 0,
            idPedidoInternoParcela: 0,
            idDef: y.idDef,
            valor: y.valor
          })
        });

        pedidoInterno.parcelas.push(objetoParcela);
      });

      this.modalPedidoInterno_Obras.forEach(x => {

        var objetoObra = {
          id: 0,
          idPedidoInterno: 0,
          idObra: x.idObra,
          valor: x.valor
        };

        pedidoInterno.obras.push(objetoObra);
      });

      this.isLoading = true;

      ApiService.post("PedidoInterno", pedidoInterno, (result) => {
        this.isLoading = false;
        if (result.status != 201) {
          this.$swal("", result.message, "error");
        } else {
          this.upload_FilesPedidoInterno.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('PedidoInterno', formData, result.data.id, () => { });
          });
          this.$swal("", 'Pedido interno solicitado', "success");
          this.modalPedidoInterno_Exibir = false;
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

        console.log(objetoReaberturaSolicitacao);

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

    adicionaMaterial: function () {
      if (this.modalNovoPedidoMaterial_MaterialSelecionado != null && this.modalNovoPedidoMaterial_Quantidade > 0) {

        // Verifica se o material já foi adicionado
        var materialJaAdicionado = this.materiaisAdicionados.some(
          (m) => m.idMaterial === this.modalNovoPedidoMaterial_MaterialSelecionado.id
        );

        if (materialJaAdicionado) {
          this.$swal("", "Este material já foi adicionado na solicitação", "warning");
          return;
        }

        var objetoMaterial = {
          id: this.materiaisAdicionados.length + 1,
          idMaterial: this.modalNovoPedidoMaterial_MaterialSelecionado.id,
          material: this.modalNovoPedidoMaterial_MaterialSelecionado.descricao,
          quantidade: this.modalNovoPedidoMaterial_Quantidade,
          valorUnitario: this.modalNovoPedidoMaterial_ValorUnitario,
          valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(this.modalNovoPedidoMaterial_ValorUnitario),
          valorTotal: this.modalNovoPedidoMaterial_Quantidade * this.modalNovoPedidoMaterial_ValorUnitario,
          valorTotalFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(this.modalNovoPedidoMaterial_Quantidade * this.modalNovoPedidoMaterial_ValorUnitario),
        };

        this.modalNovoPedidoMaterial_ValorTotal = this.modalNovoPedidoMaterial_ValorTotal + objetoMaterial.valorTotal;

        this.materiaisAdicionados.push(objetoMaterial);

        this.modalNovoPedidoMaterial_Quantidade = 0;
        this.modalNovoPedidoMaterial_ValorUnitario = 0;
        this.modalNovoPedidoMaterial_MaterialSelecionado = null;
      }
      else
        this.$swal("", "Verifique o material selecionado e a quantidade informada", "error");
    },

    // Recalcula o valorTotal de uma linha de material sempre que a quantidade ou o valor unitário forem editados
    // diretamente na tabela, e atualiza o total geral da solicitação a partir da soma de todos os itens.
    recalculaValorTotalMaterial: function (row) {
      var qtd = Number(row.quantidade) || 0;
      var vu  = Number(row.valorUnitario) || 0;
      var total = qtd * vu;
      var formatado = new Intl.NumberFormat("pt-BR", {
        style: "currency",
        currency: "BRL",
      }).format(total);

      // Atualiza a row do slot (v-client-table pode expor uma cópia, então atualizamos ela também)
      row.valorTotal = total;
      row.valorTotalFormatado = formatado;

      // Localiza o item original no array e atualiza com $set para garantir reatividade
      var idx = this.materiaisAdicionados.findIndex(function (m) { return m.id === row.id; });
      if (idx !== -1) {
        this.$set(this.materiaisAdicionados[idx], "quantidade", qtd);
        this.$set(this.materiaisAdicionados[idx], "valorUnitario", vu);
        this.$set(this.materiaisAdicionados[idx], "valorTotal", total);
        this.$set(this.materiaisAdicionados[idx], "valorTotalFormatado", formatado);
      }

      // Recalcula o total geral da solicitação a partir da lista (fonte única de verdade)
      this.modalNovoPedidoMaterial_ValorTotal = this.materiaisAdicionados.reduce(
        function (acc, m) { return acc + (Number(m.valorTotal) || 0); },
        0
      );

      // Força o Vue a re-renderizar a tabela imediatamente (contorna possível cache do v-client-table)
      this.$forceUpdate();
    },

    adicionaMaterial_AprovacaoSolicitacaoCompra: function () {
      if (
        this.materialSelecionado_AprovacaoSolicitacaoCompra != null &&
        this.modalGerenciarSolicitacaoCompra_Quantidade > 0
      ) {

        // Verifica se o material já foi adicionado
        var materialJaAdicionado = this.materiaisAdicionados.some(
          (m) => m.idMaterial === this.materialSelecionado_AprovacaoSolicitacaoCompra.id
        );

        if (materialJaAdicionado) {
          this.$swal("", "Este material já foi adicionado na solicitação", "warning");
          return;
        }

        var objetoMaterial = {
          id: this.materiaisAdicionados.length + 1,
          idMaterial: this.materialSelecionado_AprovacaoSolicitacaoCompra.id,
          material: this.materialSelecionado_AprovacaoSolicitacaoCompra.descricao,
          quantidade: this.modalGerenciarSolicitacaoCompra_Quantidade,
          valorUnitario: this.modalGerenciarSolicitacaoCompra_ValorUnitario,
          valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
          }).format(this.modalGerenciarSolicitacaoCompra_ValorUnitario),
          valorTotal:
            this.modalGerenciarSolicitacaoCompra_Quantidade *
            this.modalGerenciarSolicitacaoCompra_ValorUnitario,
          valorTotalFormatado: new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
          }).format(
            this.modalGerenciarSolicitacaoCompra_Quantidade *
            this.modalGerenciarSolicitacaoCompra_ValorUnitario
          ),
        };

        this.modalGerenciarSolicitacaoCompra_ValorTotal =
          this.modalGerenciarSolicitacaoCompra_ValorTotal +
          objetoMaterial.valorTotal;

        this.materiaisAdicionados.push(objetoMaterial);

        this.modalGerenciarSolicitacaoCompra_Quantidade = 0;
        this.modalGerenciarSolicitacaoCompra_ValorUnitario = 0;
        this.materialSelecionado_AprovacaoSolicitacaoCompra = null;
      }
      else
        this.$swal("", "Verifique o material selecionado e a quantidade informada", "error");
    },

    modalGerenciarSolicitacaoCompra_RemoverMaterial: function (idMaterial) {
      this.materiaisAdicionados.forEach((material) => {
        if (material.id == idMaterial) {
          this.materiaisAdicionados.splice(this.materiaisAdicionados.indexOf(material), 1);
        }
      });
    },

    modalReabrirSolicitacao_RemoverMaterial: function (idMaterial) {
      this.modalReabrirSolicitacao_MateriaisAdicionados.forEach((material) => {
        if (material.id == idMaterial) {
          this.modalReabrirSolicitacao_ValorTotal = this.modalReabrirSolicitacao_ValorTotal - material.valorTotal;
          this.modalReabrirSolicitacao_MateriaisAdicionados.splice(this.modalReabrirSolicitacao_MateriaisAdicionados.indexOf(material), 1);
        }
      });
    },

    removerMaterial: function (idMaterial) {
      this.materiaisAdicionados.forEach((material) => {
        if (material.id == idMaterial) {
          this.solicitacaoCompra_ValorTotal = this.solicitacaoCompra_ValorTotal - material.valorTotal;
          this.materiaisAdicionados.splice(this.materiaisAdicionados.indexOf(material), 1);
        }
      });
    },

    redirecionaCadastroMaterial() {
      window.open('#/app/configuracoes/material/material', '_blank');
    },

    listaMateriais: function () {
      this.materiais = [];

      this.isLoading = true;

      ApiService.getAll("Material", true, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.materiais = result.data;
        }
      });
    },

    listaObras: function () {
      this.obras = [];

      ApiService.getAll("Obra", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.obras = result.data.filter(x=>x.cancelada === false);
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
          this.defsPedidoInterno = this.defs.filter(item => { return item.podeAbrirPedidoInterno == true });
        }
      });
    },

    listaUsuarios: function () {
      this.usuarios = [];
      this.usuariosAprovadores = [];

      ApiService.getAll("Usuario", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.usuarios = result.data;
          this.usuariosAprovadores = result.data;
          console.log(this.usuariosAprovadores);

          this.usuariosCompradores = this.usuarios.filter(u => u.idCargo === 4);
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

    enviarSolicitacaoServico: function () {


      if (this.modalNovoPedidoServico_FornecedorSelecionado != null && this.modalNovoPedidoServico_PagamentosServico.length == 0) {
        this.$swal("", 'Favor informar as datas de pagamento', "error");
        return;
      }

      if (this.modalNovoPedidoServico_FornecedorSelecionado == null && this.modalNovoPedidoServico_PagamentosServico.length > 0) {
        this.$swal("", 'Favor informar um fornecedor', "error");
        return;
      }

      if (this.modalNovoPedidoServico_PagamentosServico.length > 0) {
        var somaValoresPagamento = (this.modalNovoPedidoServico_PagamentosServico.reduce((sum, item) => sum + item.valor, 0));

        if (somaValoresPagamento != this.modalNovoPedidoServico_ValorTotal) {
          this.$swal("", 'Valor total diferente do valor das parcelas', "error");
          return;
        }
      }

      var objetoSolicitacaoCompra = {
        id: 0,
        idStatusSolicitacaoCompra: 1,
        idTipoCentroCusto: 1,
        idCentroCustoObra: this.modalNovoPedidoServico_ObraSelecionada == null ? 0 : this.modalNovoPedidoServico_ObraSelecionada.id,
        idCentroCustoDEF: null,
        observacao: this.modalNovoPedidoServico_Observacao,
        valorEstimado: this.modalNovoPedidoServico_CustoOrcado,
        dataEntrega: this.modalNovoPedidoServico_DataEntrega,
        nome: this.modalNovoPedidoServico_Titulo,
        servico: true,
        materiais: [],
        idUsuarioComprador: this.modalNovoPedidoServico_CompradorSelecionado == null ? null : this.modalNovoPedidoServico_CompradorSelecionado.id,
        idFornecedor: this.modalNovoPedidoServico_FornecedorSelecionado == null ? 0 : this.modalNovoPedidoServico_FornecedorSelecionado.id,
        pagamentos: this.modalNovoPedidoServico_PagamentosServico
      }

      this.isLoading = true;

      ApiService.post('SolicitacaoCompra', objetoSolicitacaoCompra, (result) => {
        if (result.status != 201) {
          this.isLoading = false;
          this.$swal("Erro ao cadastrar solicitação de compra", result.message, "error");
        } else {
          this.upload_Files.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('SolicitacaoCompra', formData, result.data.id, () => { });
          });

          this.modalNovoPedidoServico_CustoOrcado = 0.0;
          this.modalNovoPedidoServico_DataEntrega = null;
          this.modalNovoPedidoServico_DataPagamentoServico = null;
          this.modalNovoPedidoServico_Exibir = false;
          this.modalNovoPedidoServico_CompradorSelecionado = null;
          this.modalNovoPedidoServico_FornecedorSelecionado = null;
          this.modalNovoPedidoServico_ObraSelecionada = null;
          this.modalNovoPedidoServico_Observacao = '';
          this.modalNovoPedidoServico_PagamentosServico = [];
          this.modalNovoPedidoServico_Titulo = '';
          this.modalNovoPedidoServico_ValorPagamentoServico = 0.0;
          this.modalNovoPedidoServico_ValorTotal = 0.0;

          this.isLoading = false;

          this.listaMinhasSolicitacoes();
        }
      });
    },

    enviarSolicitacaoMaterial: function () {
      this.isLoading = true;


      var objetoSolicitacaoCompra = {
        id: 0,
        idStatusSolicitacaoCompra: 1,
        idTipoCentroCusto: 1,
        idCentroCustoObra: this.modalNovoPedidoMaterial_ObraSelecionada == null ? 0 : this.modalNovoPedidoMaterial_ObraSelecionada.id,
        idCentroCustoDEF: null,
        observacao: this.modalNovoPedidoMaterial_Observacao,
        valorEstimado: this.modalNovoPedidoMaterial_ValorTotal,
        dataEntrega: this.modalNovoPedidoMaterial_DataEntrega,
        nome: this.modalNovoPedidoMaterial_Titulo,
        servico: false,
        materiais: [],
        idFornecedor: 0,
        pagamentos: []
      }

      this.materiaisAdicionados.forEach((material) => {
        objetoSolicitacaoCompra.materiais.push({
          id: 0,
          idSolicitacaoCompra: 0,
          idMaterial: material.idMaterial,
          quantidade: material.quantidade,
          valorUnitarioEstimado: material.valorUnitario
        })
      });

      ApiService.post('SolicitacaoCompra', objetoSolicitacaoCompra, (result) => {
        if (result.status != 201) {
          this.isLoading = false;
          this.$swal("Erro ao cadastrar solicitação de compra", result.message, "error");
        } else {
          this.upload_Files.forEach((file) => {
            const formData = new FormData();
            formData.append("arquivo", file);
            ApiService.uploadFile('SolicitacaoCompra', formData, result.data.id, () => { });
          });

          this.materiaisAdicionados = [];
          this.modalNovoPedidoMaterial_MaterialSelecionado = null;
          this.modalNovoPedidoMaterial_ObraSelecionada = null;
          this.modalNovoPedidoMaterial_Quantidade = 0;
          this.modalNovoPedidoMaterial_ValorUnitario = 0;
          this.modalNovoPedidoMaterial_ValorTotal = 0.0;
          this.modalNovoPedidoMaterial_Observacao = '';
          this.modalNovoPedidoMaterial_DataEntrega = null;
          this.nome = '';

          this.isLoading = false;


          this.modalNovoPedidoMaterial_Exibir = false;

          this.listaMinhasSolicitacoes();
        }
      });
    },

    // Monta o payload de filtros a partir do estado atual do componente.
    // skip é passado explicitamente pois pode variar (paginação vs. primeira página).
    _buildPayloadMinhasSolicitacoes: function (skip) {
      return {
        idsObra: (this.modalMinhasSolicitacoes_FiltroObrasSelecionadas || []).map(o => o.id),
        idsStatus: (this.modalMinhasSolicitacoes_FiltroStatusSelecionados || []).map(s => s.id),
        codigo: this.modalMinhasSolicitacoes_FiltroCodigo && this.modalMinhasSolicitacoes_FiltroCodigo.trim() !== ''
          ? this.modalMinhasSolicitacoes_FiltroCodigo.trim()
          : null,
        titulo: this.modalMinhasSolicitacoes_FiltroTitulo && this.modalMinhasSolicitacoes_FiltroTitulo.trim() !== ''
          ? this.modalMinhasSolicitacoes_FiltroTitulo.trim()
          : null,
        dataSolicitacaoInicial: this.modalMinhasSolicitacoes_FiltroDataInicio || null,
        dataSolicitacaoFinal: this.modalMinhasSolicitacoes_FiltroDataFim || null,
        skip: skip,
        take: this.modalMinhasSolicitacoes_Take
      };
    },

    // Carrega solicitações com estado atual (filtros + página). Usado por list/filter/pagination.
    _carregarSolicitacoesComEstadoAtual: function () {
      var skip = (this.modalMinhasSolicitacoes_PaginaAtual - 1) * this.modalMinhasSolicitacoes_Take;
      this.modalMinhasSolicitacoes_Skip = skip;
      var filtros = this._buildPayloadMinhasSolicitacoes(skip);

      this.isLoading = true;
      ApiService.getSolicitacoesCompraUsuario(filtros, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalMinhasSolicitacoes_Solicitacoes = result.data.solicitacoes || [];
          this.modalMinhasSolicitacoes_Count = result.data.count || 0;
        }
      });
    },

    listaMinhasSolicitacoes: function () {
      this.defs = [];
      // Volta para página 1 e recarrega respeitando filtros já aplicados
      this.modalMinhasSolicitacoes_PaginaAtual = 1;
      this._carregarSolicitacoesComEstadoAtual();
    },

    // Popula a lista de status de solicitação de compra para o multiselect de filtros
    listaStatusSolicitacaoCompra: function () {
      ApiService.getAll("StatusSolicitacaoCompra", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalMinhasSolicitacoes_OpcoesStatus = result.data;
        }
      });
    },

    // Abre o modal "Minhas Solicitações" garantindo que as listas dos filtros
    // (obras e status) estejam carregadas
    abrirModalMinhasSolicitacoes: function () {
      if (!this.obras || this.obras.length === 0) {
        this.listaObras();
      }
      if (!this.modalMinhasSolicitacoes_OpcoesStatus || this.modalMinhasSolicitacoes_OpcoesStatus.length === 0) {
        this.listaStatusSolicitacaoCompra();
      }
      this.modalMinhasSolicitacoes_Exibir = true;
    },

    // ===== FILTROS: Minhas Solicitações =====
    // Aplica os filtros selecionados no estado atual do componente e carrega a primeira página.
    filtrarMinhasSolicitacoes: function () {
      this.modalMinhasSolicitacoes_PaginaAtual = 1;
      this._carregarSolicitacoesComEstadoAtual();
    },

    // Muda de página preservando os filtros selecionados.
    // b-pagination emite o novo número de página como argumento do @change.
    mudarPaginaMinhasSolicitacoes: function (pagina) {
      this.modalMinhasSolicitacoes_PaginaAtual = pagina;
      this._carregarSolicitacoesComEstadoAtual();
    },

    // Limpa todos os filtros e recarrega a lista completa
    limparFiltrosMinhasSolicitacoes: function () {
      this.modalMinhasSolicitacoes_FiltroObrasSelecionadas = [];
      this.modalMinhasSolicitacoes_FiltroCodigo = '';
      this.modalMinhasSolicitacoes_FiltroTitulo = '';
      this.modalMinhasSolicitacoes_FiltroDataInicio = null;
      this.modalMinhasSolicitacoes_FiltroDataFim = null;
      this.modalMinhasSolicitacoes_FiltroStatusSelecionados = [];
      this.listaMinhasSolicitacoes();
    },

    listaMeusPedidosInternos: function () {
      this.defs = [];

      ApiService.getPedidosInternosUsuario((result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalMeusPedidosInternos_Pedidos = result.data;

        }
      });
    },

    obtemNotasFiscaisParaAprovacao: function () {
      this.modalAprovacaoNF_NotasFiscais = [];

      ApiService.obtemNotasFiscaisParaAprovacao((result) => {

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {

          this.modalAprovacaoNF_NotasFiscais = result.data;

          console.log(this.modalAprovacaoNF_NotasFiscais);  
        }
      });
    },

    obtemCotacoesParaAprovacao: function () {
      this.modalAprovacaoCotacao_Cotacoes = 0;

      ApiService.obtemCotacoesParaAprovacao((result) => {

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalAprovacaoCotacao_Cotacoes = result.data;
        }
      });
    },

    obtemPedidosInternosParaAprovacao: function () {
      this.pedidosInternosParaAprovacao = [];

      ApiService.obtemPedidosInternosParaAprovacao(this.usuarioDTO.id, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.pedidosInternosParaAprovacao = result.data;
        }
      });
    },

    obtemSolicitacoesCompraParaValidacao: function () {

      this.solicitacoesCompraParaAprovacao = [];

      ApiService.getSolicitacoesCompraStatus(1, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.solicitacoesCompraParaAprovacao = result.data;
        }
      });
    },

    aprovarReprovarNotaFiscal: function (idPedidoCompraNotaFiscal, aprovada) {
    var self = this;

    self.$swal({
      title: aprovada ? 'Aprovar nota fiscal?' : 'Reprovar nota fiscal?',
      text: 'Confirme para continuar.',
      type: 'question',           // <- compatível com versões antigas (em vez de "icon")
      showCancelButton: true,
      confirmButtonText: aprovada ? 'Sim, aprovar' : 'Sim, reprovar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true,
      allowOutsideClick: false,
      allowEscapeKey: false
    }).then(function (res) {
      // Versões antigas retornam "value" (não "isConfirmed")
      if (!res || !res.value) return;

      if (typeof self.isLoading !== 'undefined') self.isLoading = true;

      ApiService.aprovarReprovarNotaFiscal(idPedidoCompraNotaFiscal, aprovada, function (result) {
        if (typeof self.isLoading !== 'undefined') self.isLoading = false;

        if (!result || result.status !== 200) {
          self.$swal('', (result && result.message) ? result.message : 'Erro ao processar.', 'error');
          return;
        }

        self.$swal('', 'Operação concluída com sucesso.', 'success');
        self.obtemNotasFiscaisParaAprovacao();
      });
    }).catch(function () {
      // usuário fechou o modal de confirmação; não fazer nada
    });
  },

    aprovarReprovarPI: function (idPedidoInterno, valor) {
      ApiService.aprovarReprovarPedidoInterno(idPedidoInterno, valor, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.obtemPedidosInternosParaAprovacao();
        }
      });
    },

    adicionarPagamentoManualGerenciar() {
      if (!this.modalGerenciarSolicitacaoCompra_NovaPgtoData || !this.modalGerenciarSolicitacaoCompra_NovaPgtoValor || this.modalGerenciarSolicitacaoCompra_NovaPgtoValor <= 0) return;
      this.modalGerenciarSolicitacaoCompra_PagamentosManual.push({
        id: 0,
        idSolicitacaoCompraMaterialCotacao: 0,
        data: this.modalGerenciarSolicitacaoCompra_NovaPgtoData,
        valor: this.modalGerenciarSolicitacaoCompra_NovaPgtoValor,
        _localId: Date.now() + Math.random(),
      });
      this.modalGerenciarSolicitacaoCompra_NovaPgtoData = null;
      this.modalGerenciarSolicitacaoCompra_NovaPgtoValor = 0;
    },

    removerPagamentoManualGerenciar(localId) {
      this.modalGerenciarSolicitacaoCompra_PagamentosManual =
        this.modalGerenciarSolicitacaoCompra_PagamentosManual.filter(p => p._localId !== localId);
    },

    redirecionarParaEdicaoDeServico: function(solicitacao){
      let routeData = this.$router.resolve({
        name: "Cotacao",
        params: { id: solicitacao.id },
      });
      window.open(routeData.href, "_self");
    },
    
    preencherModalGerenciarSolicitacaoCompra: function (solicitacao) {

      this.materiaisAdicionados = [];
      this.modalGerenciarSolicitacaoCompra_ValorTotal = 0;
      this.modalGerenciarSolicitacaoCompra_PagamentosManual = [];
      this.modalGerenciarSolicitacaoCompra_NovaPgtoData = null;
      this.modalGerenciarSolicitacaoCompra_NovaPgtoValor = 0;
      this.modalGerenciarSolicitacaoCompra_CotacoesOriginais = [];

      this.modalGerenciarSolicitacaoCompra_Observacao = solicitacao.observacao;
      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada = solicitacao;
      this.modalGerenciarSolicitacaoCompra_DataEntrega = solicitacao.dataEntrega;
      this.modalGerenciarSolicitacaoCompra_Nome = solicitacao.nome;
      this.modalGerenciarSolicitacaoCompra_Codigo = solicitacao.codigo;
      this.modalGerenciarSolicitacaoCompra_ValorTotal = solicitacao.valorEstimado;

      this.compradorSelecionado_AprovacaoSolicitacaoCompra =
        this.usuariosCompradores.find(u => u.id === Number(solicitacao.idUsuarioComprador)) || null;

      solicitacao.materiais.forEach((m) => {
        // Sempre salvar cotações na variável separada (fonte da verdade no save)
        this.modalGerenciarSolicitacaoCompra_CotacoesOriginais.push({
          idMaterial: m.idMaterial,
          idSolicitacaoCompraMaterial: m.id,
          cotacoes: JSON.parse(JSON.stringify(m.cotacoes || [])),
        });

        // Carregar pagamentos manuais da cotação principal para o array editável
        if (m.cotacoes && m.cotacoes.length > 0) {
          const cotacaoPrincipal = m.cotacoes.find(c => c.cotacaoFinal) || m.cotacoes[0];
          if (cotacaoPrincipal && cotacaoPrincipal.pagamentoManual) {
            cotacaoPrincipal.pagamentoManual.forEach((pgto) => {
              this.modalGerenciarSolicitacaoCompra_PagamentosManual.push({
                id: pgto.id,
                idSolicitacaoCompraMaterialCotacao: pgto.idSolicitacaoCompraMaterialCotacao,
                idCotacao: cotacaoPrincipal.id,
                data: pgto.data,
                valor: pgto.valor,
                _localId: Date.now() + Math.random(),
              });
            });
          }
        }

        this.materiaisAdicionados.push({
          id: this.materiaisAdicionados.length + 1,
          idSolicitacaoCompraMaterial: m.id,
          idMaterial: m.idMaterial,
          material: m.material ? m.material.descricao : '',
          quantidade: m.quantidade,
          valorUnitario: m.valorUnitarioEstimado,
          valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(m.valorUnitarioEstimado),
          valorTotal: m.quantidade * m.valorUnitarioEstimado,
          valorTotalFormatado: new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(m.quantidade * m.valorUnitarioEstimado),
        });
      });
    },

    enviarParaAprovacao(id) {
      this.isLoading = true;

      ApiService.enviarParaAprovacao(id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("Erro ao enviar para revisão a solicitação de compra", result.message, "error");
        } else {

          this.listaMinhasSolicitacoes();
          this.modalGerenciarSolicitacaoCompra_Exibir = false;
        }
      });
    },

    enviarParaRevisao(id) {
      this.isLoading = true;

      ApiService.enviarParaRevisao(id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("Erro ao enviar para revisão a solicitação de compra", result.message, "error");
        } else {

          this.obtemSolicitacoesCompraParaValidacao();
          this.modalGerenciarSolicitacaoCompra_Exibir = false;
        }
      });
    },

    editarSolicitacaoCompra: function () {

      this.isLoading = true;

      const solicitacao = this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada;

      if (this.compradorSelecionado_AprovacaoSolicitacaoCompra != null) {
        solicitacao.idUsuarioComprador = this.compradorSelecionado_AprovacaoSolicitacaoCompra.id;
      }

      solicitacao.observacao  = this.modalGerenciarSolicitacaoCompra_Observacao;
      solicitacao.dataEntrega = this.modalGerenciarSolicitacaoCompra_DataEntrega;
      solicitacao.nome        = this.modalGerenciarSolicitacaoCompra_Nome;

      // A lista enviada para a API é reconstruída a partir de materiaisAdicionados,
      // que é a fonte de verdade da UI. Assim itens adicionados também são enviados
      // e itens removidos no modal deixam de ser enviados.
      // - Itens pré-existentes: partimos do objeto original (preservando cotações,
      //   material aninhado etc.) e sobrescrevemos quantidade/valor.
      // - Itens novos: id = 0 para o backend fazer INSERT.
      const materiaisOriginais = solicitacao.materiais || [];
      var valorDosMateriais = 0.0;
      var novaListaMateriais = this.materiaisAdicionados.map((ma) => {
        valorDosMateriais += ma.quantidade * ma.valorUnitario;

        if (ma.idSolicitacaoCompraMaterial) {
          const original = materiaisOriginais.find(m => m.id === ma.idSolicitacaoCompraMaterial);
          if (original) {
            return {
              ...original,
              quantidade: ma.quantidade,
              valorUnitarioEstimado: ma.valorUnitario,
            };
          }
        }

        return {
          id: 0,
          idSolicitacaoCompra: solicitacao.id,
          idMaterial: ma.idMaterial,
          quantidade: ma.quantidade,
          valorUnitarioEstimado: ma.valorUnitario,
          cotacoes: [],
        };
      });

      solicitacao.materiais = novaListaMateriais;

      if (valorDosMateriais > 0)
        solicitacao.valorEstimado = valorDosMateriais;

      // Aplica pagamentos manuais à cotação principal. Localizamos o material
      // correspondente na lista reconstruída pelo seu idSolicitacaoCompraMaterial
      // (em vez de assumir índice 0), já que o usuário pode ter removido itens.
      const primeiraEntrada = this.modalGerenciarSolicitacaoCompra_CotacoesOriginais[0];
      if (primeiraEntrada && primeiraEntrada.cotacoes.length > 0) {
        const materialAlvo = solicitacao.materiais.find(m =>
          m.id === primeiraEntrada.idSolicitacaoCompraMaterial
        );
        if (materialAlvo) {
          const cotacao = primeiraEntrada.cotacoes[0];
          cotacao.pagamentoManual = this.modalGerenciarSolicitacaoCompra_PagamentosManual.map(p => ({
            id: p.id || 0,
            idSolicitacaoCompraMaterialCotacao: cotacao.id,
            data: p.data,
            valor: p.valor,
          }));
          materialAlvo.cotacoes = primeiraEntrada.cotacoes;
        }
      }

      ApiService.put(
        "SolicitacaoCompra",
        solicitacao,
        (result) => {
          this.isLoading = false;
          if (result.status != 201) {
            this.$swal("Erro ao editar solicitação de compra", result.message, "error");
          } else {
            this.$swal("Solicitação salva.", result.message, "success");
            this.listaMinhasSolicitacoes();
            this.obtemSolicitacoesCompraParaValidacao();
          }
        }
      );
    },


    validarSolicitacao: function (id, codigo) {
      this.$swal({
        title: "Atenção",
        text: "Deseja validar a solicitação " + codigo + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, validar !",
        cancelButtonText: "Não",
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.validarSolicitacaoCompra(
            this.solicitacaoParaValidacao.id,
            (this.compradorSelecionado_AprovacaoSolicitacaoCompra != null ? this.compradorSelecionado_AprovacaoSolicitacaoCompra.id:0),
            (resultAPI) => {
              this.isLoading = false;
              if (resultAPI.status != 200) {
                this.$swal("", resultAPI.data, "error");
              } else {
                this.solicitacaoParaValidacao = null;

                this.obtemSolicitacoesCompraParaValidacao();
                this.modalAprovacaoSC_Exibir = false;
                this.modalGerenciarSolicitacaoCompra_Exibir = false;
              }
            }
          );
        }
      });
    },

    reprovarSolicitacao: function (id, codigo) {
      this.$swal({
        title: "Atenção",
        text: "Deseja reprovar a solicitação " + codigo + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, reprovar !",
        cancelButtonText: "Não",
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.reprovarSolicitacaoCompra(
            this.solicitacaoParaValidacao.id,
            (resultAPI) => {
              this.isLoading = false;
              if (resultAPI.status != 200) {
                this.$swal("", resultAPI.message, "error");
              } else {
                this.$swal("", 'Solicitação reprovada', "success");
                this.solicitacaoParaValidacao = null;

                this.obtemSolicitacoesCompraParaValidacao();
              }
            }
          );
        }
      });
    },

    editarDataVencimentoNF() {
      this.isLoading = true;

      ApiService.editarDataVencimentoNF(
        this.modalEdicaoDataNotaFiscal_Notas,
        (resultAPI) => {
          this.isLoading = false;
          if (resultAPI.status != 200) {
            this.$swal("", resultAPI.message, "error");
          } else {
            window.location.reload();
          }
        }
      );

    },

  },
  mounted: function () {
    this.listaMateriais();
    this.listaObras();
    this.listaDefs();
    this.listaUsuarios();
    this.listaFornecedores();
    this.obtemRascunhos();

    this.usuarioDTO = JSON.parse(localStorage.getItem("usuarioDTO"));

    if (this.usuarioDTO.globalSolicitacaoCompra)
      this.listaMinhasSolicitacoes();

    if (this.usuarioDTO.globalAprovaCotacao)
      this.obtemNotasFiscaisParaAprovacao();

    if (this.usuarioDTO.globalAprovaPedidoInterno)
      this.obtemPedidosInternosParaAprovacao();

    this.obtemCotacoesParaAprovacao();
    this.obtemSolicitacoesCompraParaValidacao();
    this.listaMeusPedidosInternos();
  },


};
</script>





<style src="./Dashboard.scss" lang="scss" />

<style scoped>
/* Balões de notificação - Minhas Solicitações */
.sc-notification-badge {
  position: absolute;
  top: -10px;
  right: -22px;
  min-width: 22px;
  height: 22px;
  padding: 0 6px;
  border-radius: 50px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 11px;
  font-weight: 700;
  color: #fff;
  line-height: 1;
  box-shadow: 0 2px 6px rgba(0,0,0,0.25);
  pointer-events: none;
}
.sc-notification-badge--orange {
  background-color: #f57c00;
  right: -23px;
}
</style>
