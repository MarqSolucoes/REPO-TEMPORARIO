<template>
  <div class="ordemCompra-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>
    <h1 class="page-title">Ordens de Compras</h1>

    <!-- =================== MODAL REABRIR SOLICITAÇÃO =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalReabrirSolicitacao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalReabrirSolicitacao_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Solicitação</span>
          <h3 class="afm-hero__title">{{ modalReabrirSolicitacao_Titulo }}</h3>
          <p class="afm-hero__description">Informe a obra e a data de entrega para reabrir a solicitação.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

          <b-row>
            <b-col md="12">
              <div class="afm-field-group">
                <label class="afm-label">Obra</label>
                <multiselect v-model="modalReabrirSolicitacao_ObraSelecionada" :multiple="false" :options="obras"
                  :custom-label="descricaoObra" select-label="Selecionar" placeholder="Selecione uma obra" label="descricao"
                  track-by="descricao">
                </multiselect>
              </div>
            </b-col>
          </b-row>

          <b-row class="mt-3">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data de entrega</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalReabrirSolicitacao_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
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
            <b-button @click="modalReabrirSolicitacao_Exibir = false" variant="dark" class="mb-0 mr-2">
              Fechar
            </b-button>
            <b-button variant="success" class="mb-0" @click="reabrirSolicitacao()">
              <i class="fa fa-refresh mr-1"></i> {{ modalReabrirSolicitacao_Titulo }}
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL CANCELAMENTO =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalCancelamento" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
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
            <b-form-input v-model="modalCancelarSolicitacao_Motivo" style="color: white"></b-form-input>
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
            <b-button variant="danger" class="mb-0" @click="cancelarSolicitacao()">
              <i class="fa fa-ban mr-1"></i> {{ modalCancelarSolicitacao_Titulo }}
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL GERENCIAR SOLICITAÇÃO DE COMPRA =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalSolicitacaoCompra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" size="xl" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalGerenciarSolicitacaoCompra_Exibir">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Solicitação de Compra</span>
          <h3 class="afm-hero__title">Gerenciar solicitação de compra</h3>
          <p class="afm-hero__description">Ajuste valor, data de entrega e os materiais da solicitação.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Valor Total</label>
                <Money v-model="modalGerenciarSolicitacaoCompra_ValorTotal" v-bind="money" :disabled="true"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Data de entrega</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                  v-model="modalGerenciarSolicitacaoCompra_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>
          </b-row>
        </section>

        <section class="afm-section-card">
          <div class="afm-field-group mb-3">
            <label class="afm-label">Material</label>
            <multiselect v-model="materialSelecionado" :multiple="false" :options="materiais"
              :custom-label="nameWithLang" select-label="Selecionar" placeholder="Selecione um material"
              label="descricao" track-by="descricao">
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
            <b-button v-on:click="adicionaMaterial()" variant="success" class="afm-add-btn" style="width: auto; min-width: 200px; max-width: 320px;">
              <i class="fa fa-plus mr-1"></i> Adicionar Material
            </b-button>
          </div>
        </section>

        <section class="afm-section-card">

          <div v-if="materiaisAdicionados && materiaisAdicionados.length > 0">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho">Material</th>
                    <th class="estilo-cabecalho texto-centro">Qtd.</th>
                    <th class="estilo-cabecalho texto-centro">Valor Un.</th>
                    <th class="estilo-cabecalho texto-centro">Total</th>
                    <th class="estilo-cabecalho texto-centro" style="width:60px;"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in materiaisAdicionados" :key="'mat-' + (row.id || index)">
                    <td class="estilo-celula">{{ row.material }}</td>
                    <td class="estilo-celula texto-centro">{{ row.quantidade }}</td>
                    <td class="estilo-celula texto-centro">{{ row.valorUnitarioFormatado }}</td>
                    <td class="estilo-celula texto-centro">{{ row.valorTotalFormatado }}</td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="removerMaterial(row.id)" class="btn btn-danger afm-remove-btn">
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
            <textarea-autosize id="textarea" v-model="modalGerenciarSolicitacaoCompra_Observacao" class="form-control"
              :min-height="135" style="color: white" />
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalGerenciarSolicitacaoCompra_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="editarSolicitacaoCompra()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar Solicitação
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL COTAÇÃO =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalCotacao" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalCotacao_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Cotação</span>
          <h3 class="afm-hero__title">{{ modalCotacao_Titulo }}</h3>
          <p class="afm-hero__description">Selecione um material e informe as cotações recebidas dos fornecedores.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Material ou serviço</label>
            <multiselect v-model="modalCotacao_MaterialSelecionado" @input="
              modalCotacao_Quantidade =
              modalCotacao_MaterialSelecionado == null
                ? 0
                : modalCotacao_MaterialSelecionado.quantidade;
            obtemCotacoes(
              modalCotacao_MaterialSelecionado == null
                ? 0
                : modalCotacao_MaterialSelecionado.id
            );
            " :multiple="false" :options="modalCotacao_Materiais" :custom-label="labelMaterialCotacao"
              select-label="Selecionar" placeholder="Selecione um item">
            </multiselect>
          </div>
        </section>

        <section class="afm-section-card" v-if="!this.modalCotacao_Visualizar">

          <div class="afm-field-group mb-3">
            <label class="afm-label">Fornecedor</label>
            <multiselect v-model="modalCotacao_FornecedorSelecionado" :multiple="false" :options="fornecedores"
              select-label="Selecionar" placeholder="Selecione um fornecedor" label="nomeFantasia"
              track-by="nomeFantasia">
            </multiselect>
          </div>

          <b-row>
            <b-col md="3">
              <div class="afm-field-group">
                <label class="afm-label">Quantidade</label>
                <Money v-model="modalCotacao_Quantidade" v-bind="decimalInput" :disabled="true" />
              </div>
            </b-col>

            <b-col md="3">
              <div class="afm-field-group">
                <label class="afm-label">Valor unitário</label>
                <Money v-model="modalCotacao_ValorUnitario" v-bind="money"></Money>
              </div>
            </b-col>

            <b-col md="3">
              <div class="afm-field-group">
                <label class="afm-label">Data de entrega</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalCotacao_DataEntrega"
                  format="dd/MM/yyyy" type="date" :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>

            <b-col md="3">
              <div class="afm-field-group">
                <label class="afm-label">Cotação principal</label>
                <toggle-button v-model="modalCotacao_CotacaoPrincipal" :color="{
                  checked: '#2D8515',
                  unchecked: '#FF0000',
                  disabled: '#CCCCCC',
                }" :labels="{ checked: 'SIM', unchecked: 'NÃO' }" :width="80" :height="25" :font-size="14" />
              </div>
            </b-col>
          </b-row>

          <div style="text-align:center; margin-top:12px;">
            <b-button v-on:click="adicionarCotacao()" variant="success" class="afm-add-btn" style="width: auto; min-width: 200px; max-width: 320px;">
              <i class="fa fa-plus mr-1"></i> Adicionar Cotação
            </b-button>
          </div>
        </section>

        <section class="afm-section-card">

          <div v-if="modalCotacao_Cotacoes && modalCotacao_Cotacoes.length > 0">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Vl. Unitário</th>
                    <th class="estilo-cabecalho">Fornecedor</th>
                    <th class="estilo-cabecalho texto-centro">Data de Entrega</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in modalCotacao_Cotacoes" :key="'cot-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorUnitarioCotado) }}</td>
                    <td class="estilo-celula">{{ row.fornecedor ? row.fornecedor.razaoSocial : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="definirCotacaoPrincipal(row.id, row.idSolicitacaoCompraMaterial)"
                        class="btn btn-warning afm-action-btn mr-1" v-if="!row.cotacaoFinal && !modalCotacao_Visualizar">
                        <i class="fa fa-star" title="Definir fornecedor principal"></i>
                      </button>

                      <div v-if="modalCotacao_Visualizar">
                        <i class="fa fa-star" v-if="!row.cotacaoFinal"></i>
                        <i class="fa fa-star" v-else style="color: yellow;"></i>
                      </div>

                      <button type="button" @click="excluirCotacao(row.id, row.idSolicitacaoCompraMaterial)"
                        class="btn btn-danger afm-action-btn" v-if="!modalCotacao_Visualizar">
                        <i class="fa fa-trash"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div v-else class="afm-empty-state">
            <i class="fa fa-tags"></i>
            <div><strong>Nenhuma cotação adicionada</strong><p>Selecione um material e registre a primeira cotação.</p></div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalCotacao_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL GERENCIAMENTO DE PEDIDO =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalGerencimentoPedido" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalPedido_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Pedido de Compra</span>
          <h3 class="afm-hero__title">{{ modalPedido_Titulo }}</h3>
          <p class="afm-hero__description">Edite fornecedor, condição de pagamento, impostos, frete e materiais do pedido.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

          <div class="afm-field-group mb-3">
            <label class="afm-label">Fornecedor</label>
            <multiselect v-model="modalPedido_Fornecedor" :multiple="false" :options="fornecedores"
              select-label="Selecionar" placeholder="Selecione um fornecedor" label="nomeFantasia"
              track-by="nomeFantasia">
            </multiselect>
          </div>

          <div class="afm-field-group">
            <label class="afm-label">Condição de pagamento</label>
            <multiselect v-model="modalPedido_CondicaoPagamento" :multiple="false" :options="condicoesPagamento"
              select-label="Selecionar" placeholder="Selecione uma condição de pagamento" label="descricao"
              track-by="descricao">
            </multiselect>
          </div>
        </section>

        <section class="afm-section-card"
          v-if="this.modalPedido_CondicaoPagamento != null && this.modalPedido_CondicaoPagamento.descricao == 'Manual' ? true : false">

          <b-row>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Data do pagamento</label>
                <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="modalPedido_DataPagamentoServico"
                  :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Valor</label>
                <Money v-model="modalPedido_ValorPagamentoServico" v-bind="number"></Money>
              </div>
            </b-col>
            <b-col md="4" class="afm-field-group--action">
              <b-button v-on:click="adicionaPagamentoServico()" variant="success" class="afm-add-btn">
                <i class="fa fa-plus mr-1"></i> Adicionar Pagamento
              </b-button>
            </b-col>
          </b-row>

          <div v-if="this.modalPedido_PedidoCompra.faturas.length > 0" class="mt-3">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Data</th>
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                    <th class="estilo-cabecalho texto-centro" style="width:60px;"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(pagamento, index) in modalPedido_PedidoCompra.faturas" :key="'fat-' + index">
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--date">
                        <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pagamento.dataFatura"
                          :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open">
                        </DatePickerMask>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--money">
                        <Money v-model="pagamento.valor" v-bind="number" style="max-width: 100px;"></Money>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="removePagamentoSolicitacaoServico(pagamento)"
                        class="btn btn-danger afm-remove-btn">
                        <i class="fa fa-trash"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </section>

        <section class="afm-section-card">

          <div class="afm-field-group mb-3">
            <label class="afm-label">Endereço de entrega</label>
            <b-form-input v-model="modalPedido_EnderecoEntrega" style="color: white"></b-form-input>
          </div>

          <b-row>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Imposto</label>
                <Money v-model="modalPedido_Imposto" v-bind="number"></Money>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Frete</label>
                <Money v-model="modalPedido_Frete" v-bind="number"></Money>
              </div>
            </b-col>
          </b-row>

          <b-row class="mt-3">
            <b-col md="6">
              <div class="afm-field-group">
                <label class="afm-label">Prazo de entrega</label>
                <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="modalPedido_PrazoEntrega"
                  format="dd/MM/yyyy" :clearable='true' type="date">
                </DatePickerMask>
              </div>
            </b-col>
            <b-col md="6">
              <div class="afm-summary-card" style="padding:14px 16px;">
                <span class="afm-summary-card__label">Valor Total do Pedido</span>
                <strong class="afm-summary-card__value">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(modalPedido_Frete + modalPedido_Imposto + modalPedido_Materiais.reduce((soma, x) => soma + (+(x.valorUnitario * x.quantidade) || 0), 0)) }}</strong>
              </div>
            </b-col>
          </b-row>
        </section>

        <section class="afm-section-card">

          <div v-if="modalPedido_Materiais && modalPedido_Materiais.length > 0">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Material</th>
                    <th class="estilo-cabecalho texto-centro" style="max-width: 80px; min-width: 80px; width: 80px;">Qtd</th>
                    <th class="estilo-cabecalho texto-centro" style="max-width: 80px; min-width: 80px; width: 80px;">Valor Un.</th>
                    <th class="estilo-cabecalho texto-centro" style="max-width: 80px; min-width: 80px; width: 80px;">Total</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(material, index) in modalPedido_Materiais" :key="'matp-' + (material.id || index)">
                    <td class="estilo-celula">{{ material.material ? material.material.descricao : '' }}</td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--money">
                        <Money v-model="modalPedido_Materiais[index].quantidade" v-bind="decimalInput" />
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--money">
                        <Money v-model="modalPedido_Materiais[index].valorUnitario" v-bind="decimalInput" />
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(material.quantidade * material.valorUnitario) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div v-else class="afm-empty-state">
            <i class="fa fa-inbox"></i>
            <div><strong>Nenhum material no pedido</strong><p>Este pedido não possui materiais cadastrados.</p></div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalPedido_Exibir = false" variant="dark" class="mb-0 mr-2">
              Cancelar
            </b-button>
            <b-button v-on:click="editarPedidoCompra()" variant="success" class="mb-0">
              <i class="fa fa-save mr-1"></i> Editar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL ARQUIVOS =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalArquivos" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalArquivos_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Anexos</span>
          <h3 class="afm-hero__title">{{ modalArquivos_Titulo }}</h3>
          <p class="afm-hero__description">Consulte, baixe ou envie novos arquivos vinculados a este registro.</p>
        </div>
        <div class="afm-hero__pill">
          <span>{{ modalArquivos_Arquivos ? modalArquivos_Arquivos.length : 0 }}</span>
          <small>arquivo(s)</small>
        </div>
      </div>

      <input type="file" name="file" id="fileInput" style="display: none" class="hidden-input" @change="onChange"
        ref="file" />

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
                      <button type="button" @click="downloadFile(row.id, row.nome)"
                        class="btn btn-warning afm-action-btn mr-1">
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
          <div class="afm-footer__summary">
            <span>Total:</span>
            <strong>{{ modalArquivos_Arquivos ? modalArquivos_Arquivos.length : 0 }} arquivo(s)</strong>
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
          <h3 class="afm-hero__title">{{ modalComentarios_Titulo }}</h3>
          <p class="afm-hero__description">Histórico de comentários e observações vinculadas a este registro.</p>
        </div>
        <div class="afm-hero__pill">
          <span>{{ modalComentarios_Comentarios ? modalComentarios_Comentarios.length : 0 }}</span>
          <small>comentário(s)</small>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

          <div v-if="modalComentarios_Comentarios && modalComentarios_Comentarios.length > 0">
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
            <i class="fa fa-comment-o"></i>
            <div><strong>Nenhum comentário</strong><p>Seja o primeiro a adicionar um comentário abaixo.</p></div>
          </div>
        </section>

        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Comentário</label>
            <textarea-autosize id="textarea" v-model="modalComentarios_Comentario" class="form-control"
              :min-height="135" style="color: white" />
          </div>
          <div class="mt-3" style="text-align: center;">
            <b-button @click="enviarComentario()" variant="success" class="afm-add-btn" style="width: auto; min-width: 200px; max-width: 320px;">
              <i class="fa fa-plus mr-1"></i> Adicionar comentário
            </b-button>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary">
            <span>Total:</span>
            <strong>{{ modalComentarios_Comentarios ? modalComentarios_Comentarios.length : 0 }} comentário(s)</strong>
          </div>
          <div class="afm-footer__actions">
            <b-button @click="modalComentarios_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL TROCA OBRA NOTA FISCAL =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalTrocaObraNotaFiscal" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalTrocaObra_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Nota Fiscal</span>
          <h3 class="afm-hero__title">Troca de obra</h3>
          <p class="afm-hero__description">Selecione a nova obra para associar esta nota fiscal.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-summary-grid" style="grid-template-columns: repeat(2, minmax(0, 1fr));">
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Pedido de Compra</span>
              <strong class="afm-summary-card__value">{{ modalTrocaObra_PedidoCompra }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Nota Fiscal</span>
              <strong class="afm-summary-card__value">{{ modalTrocaObra_NotaFiscal }}</strong>
            </div>
          </div>
        </section>

        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Obra</label>
            <multiselect v-model="modalTrocaObra_ObraSelecionada" :multiple="false" :options="obras"
              select-label="Selecionar" placeholder="Selecione uma obra" label="codigo" track-by="codigo">
            </multiselect>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalTrocaObra_Exibir = false" variant="dark" class="mb-0 mr-2">
              Fechar
            </b-button>
            <b-button variant="success" class="mb-0" @click="alterarObraNF()">
              <i class="fa fa-refresh mr-1"></i> Alterar obra da NF
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <!-- =================== MODAL NOTAS FISCAIS =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalNotasFiscais" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalNotasFiscais_Exibir" size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Nota Fiscal</span>
          <h3 class="afm-hero__title">{{ modalNotasFiscais_Titulo }}</h3>
          <p class="afm-hero__description">Acompanhe as notas fiscais e cancelamentos de saldo deste pedido.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-summary-grid" style="grid-template-columns: repeat(2, minmax(0, 1fr));">
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Valor do Pedido</span>
              <strong class="afm-summary-card__value">{{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalNotasFiscais_Pedido.valorTotal) : 'R$ 0,00') }}</strong>
            </div>
            <div class="afm-summary-card">
              <span class="afm-summary-card__label">Saldo</span>
              <strong class="afm-summary-card__value">{{ (modalNotasFiscais_Pedido != null ? new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(modalNotasFiscais_Pedido.saldo) : 'R$ 0,00') }}</strong>
            </div>
          </div>
        </section>

        <section class="afm-section-card">

          <div v-if="modalNotasFiscais_Notas && modalNotasFiscais_Notas.length > 0">
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
                      <button
                        type="button"
                        class="btn btn-success"
                        @click="abrirModalAcoesNotaFiscal(row)"
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
            <i class="fa fa-file-text-o"></i>
            <div><strong>Nenhuma nota fiscal</strong><p>Este pedido ainda não possui notas fiscais emitidas.</p></div>
          </div>
        </section>

        <section class="afm-section-card">

          <div v-if="modalNotasFiscais_SaldosCancelados && modalNotasFiscais_SaldosCancelados.length > 0">
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
                </tbody>
              </table>
            </div>
          </div>

          <div v-else class="afm-empty-state">
            <i class="fa fa-check-circle-o"></i>
            <div><strong>Nenhum cancelamento registrado</strong><p>Não há cancelamentos de saldo para este pedido.</p></div>
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

    <!-- =================== MODAL FILTRO =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalFiltro" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal" v-model="modalFiltro_Exibir"
      size="xl">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Filtros</span>
          <h3 class="afm-hero__title">Filtro de pedidos</h3>
          <p class="afm-hero__description">Refine a listagem por obra, solicitante, engenheiro, comprador ou diretor.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">

          <b-row>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Obra</label>
                <multiselect v-model="modalFiltro_ObraSelecionada" :multiple="false" :options="obras"
                  select-label="Selecionar" placeholder="Selecione uma obra" label="codigo" track-by="codigo">
                </multiselect>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Solicitante</label>
                <multiselect v-model="modalFiltro_Solicitante" :multiple="false" :options="usuarios"
                  select-label="Selecionar" placeholder="Selecione um solicitante" label="nome" track-by="nome">
                </multiselect>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Eng. Aprovador</label>
                <multiselect v-model="modalFiltro_EngenheiroAprovador" :multiple="false" :options="usuariosAprovadores"
                  select-label="Selecionar" placeholder="Selecione um engenheiro" label="nome" track-by="nome">
                </multiselect>
              </div>
            </b-col>
          </b-row>

          <b-row class="mt-3">
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Comprador</label>
                <multiselect v-model="modalFiltro_CompradorSelecionado" :multiple="false" :options="usuariosCompradores"
                  select-label="Selecionar" placeholder="Selecione um comprador" label="nome" track-by="nome">
                </multiselect>
              </div>
            </b-col>
            <b-col md="4">
              <div class="afm-field-group">
                <label class="afm-label">Diretor Aprovador</label>
                <multiselect v-model="modalFiltro_DiretorAprovador" :multiple="false" :options="diretoresAprovadores"
                  select-label="Selecionar" placeholder="Selecione um diretor" label="nome" track-by="nome">
                </multiselect>
              </div>
            </b-col>
          </b-row>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalFiltro_Exibir = false" variant="dark" class="mb-0 mr-2">
              Fechar
            </b-button>
            <b-button v-on:click="filtrar()" variant="success" class="mb-0">
              <i class="fa fa-filter mr-1"></i> Aplicar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-row>
      <b-col md="12" xs="12">
        <b-tabs class="mb-lg">

          <b-tab @click="obtemSolicitacoesCompraEmCotacao" v-bind:title="'EM COTAÇÃO [' +
            (this.quantidades == null ? 0 : this.quantidades.qtdEmCotacao) +
            ']'
            " class="estiloWidget"
            :title-link-class="tab2Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
            v-if="usuarioDTO != null && usuarioDTO.comprasOrdensCompraEmCotacao">

            <b-button variant="success" class="width-100 mb-3 mr-3" @click="exibirModalFiltros(1)">FILTROS</b-button>

            <div class="table-card">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Código Solicitação</th>
                    <th class="estilo-cabecalho">Usuário solicitante</th>
                    <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                    <th class="estilo-cabecalho texto-centro">Data de entrega</th>
                    <th class="estilo-cabecalho">Centro de custo</th>
                    <th class="estilo-cabecalho texto-centro">Valor total estimado</th>
                    <th class="estilo-cabecalho texto-centro">Menor valor total cotado</th>
                    <th class="estilo-cabecalho">Comprador</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in solicitacoesEmCotacao" :key="'sec-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                    <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorEstimado) }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMelhorCotacao) }}</td>
                    <td class="estilo-celula">{{ row.usuarioComprador ? row.usuarioComprador.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-success"
                        @click="abrirModalAcoesEmCotacao(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!solicitacoesEmCotacao || solicitacoesEmCotacao.length === 0">
                    <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

          <b-tab ref="tabParaAprovacao" @click="obtemSolicitacoesCompraParaAprovacao" v-bind:title="'PARA APROVAÇÃO [' +
            (this.quantidades == null
              ? 0
              : this.quantidades.qtdParaAprovacao) +
            ']'
            " class="estiloWidget"
            :title-link-class="tab3Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
            v-if="usuarioDTO != null && usuarioDTO.comprasOrdensCompraParaAprovacao" :active="tab3Ativa">

            <b-button variant="success" class="width-100 mb-3 mr-3" @click="exibirModalFiltros(2)">FILTROS</b-button>

            <div class="table-card">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Código Solicitação</th>
                    <th class="estilo-cabecalho">Usuário solicitante</th>
                    <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                    <th class="estilo-cabecalho texto-centro">Data de entrega</th>
                    <th class="estilo-cabecalho">Centro de Custo</th>
                    <th class="estilo-cabecalho texto-centro">Valor total cotado</th>
                    <th class="estilo-cabecalho">Usuário Finalização Cotação</th>
                    <th class="estilo-cabecalho texto-centro">Data Finalização Cotação</th>
                    <th class="estilo-cabecalho">Comprador</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in pedidosParaAprovacao" :key="'pa-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                    <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMelhorCotacao) }}</td>
                    <td class="estilo-celula">{{ row.usuarioFinalizacaoCotacao ? row.usuarioFinalizacaoCotacao.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataFinalizacaoCotacao) }}</td>
                    <td class="estilo-celula">{{ row.usuarioComprador ? row.usuarioComprador.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-success"
                        @click="abrirModalAcoesParaAprovacao(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!pedidosParaAprovacao || pedidosParaAprovacao.length === 0">
                    <td class="estilo-celula texto-centro" colspan="10">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

          <b-tab @click="obtemSolicitacoesDevolvidasDiretoria" v-bind:title="'DEVOLVIDAS DIRETORIA [' +
            (this.quantidades == null ? 0 : this.quantidades.qtdDevolvidasDiretoria) +
            ']'
            " class="estiloWidget"
            :title-link-class="tab7Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
            v-if="usuarioDTO != null && usuarioDTO.comprasOrdensCompraEmCotacao">

            <b-button variant="success" class="width-100 mb-3 mr-3" @click="exibirModalFiltros(3)">FILTROS</b-button>

            <div class="table-card">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho texto-centro">Código Solicitação</th>
                    <th class="estilo-cabecalho">Usuário solicitante</th>
                    <th class="estilo-cabecalho texto-centro">Data da solicitação</th>
                    <th class="estilo-cabecalho texto-centro">Data de entrega</th>
                    <th class="estilo-cabecalho">Centro de custo</th>
                    <th class="estilo-cabecalho texto-centro">Valor total estimado</th>
                    <th class="estilo-cabecalho texto-centro">Menor valor total cotado</th>
                    <th class="estilo-cabecalho">Comprador</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in solicitacoesDevolvidasDiretoria" :key="'sdd-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                    <td class="estilo-celula">{{ row.usuarioCadastro ? row.usuarioCadastro.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorEstimado) }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorMelhorCotacao) }}</td>
                    <td class="estilo-celula">{{ row.usuarioComprador ? row.usuarioComprador.nome : '' }}</td>
                    <td class="estilo-celula texto-centro" style="white-space: nowrap;">
                      <button
                        type="button"
                        class="btn btn-success"
                        @click="abrirModalAcoesDiretoria(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>

                      <span v-if="row.comentarios && row.comentarios.length > 0" class="comentarios-badge">
                        {{ row.comentarios.length }}
                      </span>
                    </td>
                  </tr>
                  <tr v-if="!solicitacoesDevolvidasDiretoria || solicitacoesDevolvidasDiretoria.length === 0">
                    <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

          <b-tab @click="obtemPedidosEmCompra" v-bind:title="'EM COMPRA [' +
            (this.quantidades == null ? 0 : this.quantidades.qtdEmCompra) +
            ']'
            " class="estiloWidget"
            :title-link-class="tab4Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
            v-if="usuarioDTO != null && usuarioDTO.comprasOrdensCompraEmCompra">

            <b-button variant="success" class="width-100 mb-3 mr-3" @click="exibirModalFiltros(4)">FILTROS</b-button>

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
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                    <th class="estilo-cabecalho">Comprador</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in pedidosEmCompra" :key="'ec-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ 'P' + row.codigoSequencia }}</td>
                    <td class="estilo-celula texto-centro">{{ row.solicitacaoCompra ? row.solicitacaoCompra.codigo : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                    <td class="estilo-celula">{{ row.fornecedor ? row.fornecedor.nomeFantasia : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                    <td class="estilo-celula">{{ row.usuarioComprador ? row.usuarioComprador.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-info"
                        @click="abrirModalAcoesEmCompra(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!pedidosEmCompra || pedidosEmCompra.length === 0">
                    <td class="estilo-celula texto-centro" colspan="9">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

          <b-tab v-bind:title="'PEDIDOS FINALIZADOS E CANCELADOS'
            " class="estiloWidget"
            :title-link-class="tab5Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
            v-if="usuarioDTO != null && usuarioDTO.comprasOrdensCompraFinalizadas">
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
                    select-label="Selecionar" placeholder="Selecione uma ou mais Fornecedores" label="nomeFantasia"
                    track-by="nomeFantasia">
                  </multiselect>
                </b-col>
                <b-col md="4">
                  <label class="mr-3">Data Solicitação Inicial:</label><br />
                  <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="filtro_DataInicial"
                    format="dd/MM/yyyy" :clearable='true' type="date">
                  </DatePickerMask>
                  <a @click="filtro_DataInicial = null">limpar</a>
                </b-col>
                <b-col md="4">
                  <label class="mr-3">Data Solicitação Final:</label><br />
                  <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br" v-model="filtro_DataFinal"
                    format="dd/MM/yyyy" :clearable='true' type="date">
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
                    <th class="estilo-cabecalho texto-centro">Menor valor total cotado</th>
                    <th class="estilo-cabecalho texto-centro">Status</th>
                    <th class="estilo-cabecalho">Comprador</th>
                    <th class="estilo-cabecalho texto-centro">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in pedidosCancelados" :key="'pfc-' + (row.id || index)">
                    <td class="estilo-celula texto-centro">{{ row.codigo }}</td>
                    <td class="estilo-celula texto-centro">{{ row.solicitacaoCompra ? row.solicitacaoCompra.codigo : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.dataCadastro) }}</td>
                    <td class="estilo-celula texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                    <td class="estilo-celula">{{ row.idCentroCustoDEF != null ? (row.centroCustoDEF ? row.centroCustoDEF.descricao : '') : ((row.centroCustoObra ? row.centroCustoObra.codigo : '') + " - " + (row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '')) }}</td>
                    <td class="estilo-celula">{{ row.fornecedor ? row.fornecedor.nomeFantasia : '' }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(row.valorTotal) }}</td>
                    <td class="estilo-celula texto-centro">{{ row.statusPedidoCompra ? row.statusPedidoCompra.descricao : '' }}</td>
                    <td class="estilo-celula">{{ row.usuarioComprador ? row.usuarioComprador.nome : '' }}</td>
                    <td class="estilo-celula texto-centro">
                      <button
                        type="button"
                        class="btn btn-info"
                        @click="abrirModalAcoesCancelados(row)"
                      >
                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!pedidosCancelados || pedidosCancelados.length === 0">
                    <td class="estilo-celula texto-centro" colspan="10">Nenhum registro encontrado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </b-tab>

        </b-tabs>
      </b-col>
    </b-row>

    <!-- Modal de ações compartilhado pelas tabs. Os callbacks e a lista
         de itens são montados nos métodos `abrirModalAcoes*(row)`. -->
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
  name: "OrdemCompra",
  components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask, ModalAcoes },
  data() {
    return {
      // Estado do <ModalAcoes> compartilhado pelas tabs
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

      usuarioDTO: null,

      isLoading: false,

      open: false,

      quantidades: null,

      tagParaRedirecionar: '',

      tab1Ativa: true,
      tab2Ativa: false,
      tab3Ativa: false,
      tab4Ativa: false,
      tab5Ativa: false,
      tab6Ativa: false,
      tab7Ativa: false,

      filtro_ObraSelecionada: [],
      filtro_ClienteSelecionado: [],
      filtro_MateriaisSelecionados: [],
      filtro_DataInicial: null,
      filtro_DataFinal: null,
      filtro_FornecedoresSelecionados: [],

      modalTrocaObra_Exibir: false,
      modalTrocaObra_NotaFiscal: '',
      modalTrocaObra_PedidoCompra: '',
      modalTrocaObra_IdNotaFiscal: 0,
      modalTrocaObra_IdNovaObra: 0,
      modalTrocaObra_ObraSelecionada: null,

      modalNotasFiscais_Titulo: 'Notas Fiscais',
      modalNotasFiscais_Exibir: false,
      modalNotasFiscais_Notas: [],
      modalNotasFiscais_SaldosCancelados: [],
      modalNotasFiscais_Pedido: null,

      modalGerenciarSolicitacaoCompra_Exibir: false,
      modalGerenciarSolicitacaoCompra_ValorTotal: 0.0,
      modalGerenciarSolicitacaoCompra_Quantidade: 0.0,
      modalGerenciarSolicitacaoCompra_ValorUnitario: 0.0,
      modalGerenciarSolicitacaoCompra_Observacao: "",
      modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada: null,
      modalGerenciarSolicitacaoCompra_DataEntrega: null,

      modalCancelarSolicitacao_Titulo: "Cancelar solicitação",
      modalCancelarSolicitacao_Exibir: false,
      modalCancelarSolicitacao_Motivo: "",
      modalCancelarSolicitacao_IdTab: 0,

      modalCotacao_Titulo: "Editar cotação",
      modalCotacao_Exibir: false,
      modalCotacao_Visualizar: false,
      modalCotacao_Solicitacao: null,
      modalCotacao_MaterialSelecionado: null,
      modalCotacao_FornecedorSelecionado: null,
      modalCotacao_Quantidade: 0,
      modalCotacao_ValorUnitario: 0.0,
      modalCotacao_CotacaoPrincipal: true,
      modalCotacao_DataEntrega: null,
      modalCotacao_Cotacoes: [],
      modalCotacao_Materiais: [],
      modalCotacao_Itens: [],

      modalPedido_Titulo: "Gerenciar pedido",
      modalPedido_Exibir: false,
      modalPedido_Materiais: [],
      modalPedido_IdPedidoCompra: 0,
      modalPedido_Arquivos: [],
      modalPedido_Fornecedor: null,
      modalPedido_CondicaoPagamento: null,
      modalPedido_EnderecoEntrega: '',
      modalPedido_PedidoCompra: null,
      modalPedido_PrazoEntrega: null,
      modalPedido_Frete: 0.0,
      modalPedido_Imposto: 0.0,
      modalPedido_DataPagamentoServico: null,
      modalPedido_ValorPagamentoServico: 0.0,
      modalPedido_PagamentosServico: [],

      modalFiltro_Exibir: false,
      modalFiltro_ObraSelecionada: null,
      modalFiltro_EngenheiroAprovador: null,
      modalFiltro_DiretorAprovador: null,
      modalFiltro_Solicitante: null,
      modalFiltro_CompradorSelecionado: null,
      modalFiltro_SolicitacaoCompra: '',
      modalFiltro_PedidoCompra: '',
      modalFiltro_IdAba: 1,

      modalArquivos_Exibir: false,
      modalArquivos_Titulo: 'Arquivos',
      modalArquivos_Arquivos: [],
      modalArquivos_Controller: '',
      modalArquivos_IdSolicitacaoOuPedidoCompra: 0,

      modalComentarios_Exibir: false,
      modalComentarios_Titulo: 'Comentários',
      modalComentarios_Comentarios: [],
      modalComentarios_Controller: '',
      modalComentarios_IdSolicitacaoOuPedidoCompra: 0,
      modalComentarios_Comentario: '',

      modalReabrirSolicitacao_Exibir: false,
      modalReabrirSolicitacao_Titulo: '',
      modalReabrirSolicitacao_ObraSelecionada: null,
      modalReabrirSolicitacao_IdSolicitacao: null,
      modalReabrirSolicitacao_DataEntrega: null,

      solicitacoesParaValidacao: [],
      solicitacoesEmCotacao: [],
      pedidosParaAprovacao: [],
      solicitacoesDevolvidasDiretoria: [],
      pedidosEmCompra: [],
      pedidosCancelados: [],
      solicitacoesCanceladas: [],
      solicitacaoParaCancelamento: null,
      solicitacaoParaValidacao: null,

      obras: [],
      defs: [],
      materiais: [], //Lista de materiais ativos no sistema
      materiaisAdicionados: [], //Lista de materiais adicionados na solicitação
      fornecedores: [],
      clientes: [],
      condicoesPagamento: [],

      usuarios: [],
      usuariosAprovadores: [],
      usuariosCompradores: [],
      diretoresAprovadores: [],

      materialSelecionado: null,
      defSelecionado: null,
      obraSelecionada: null,

      upload_ExibirBotoes: false,
      upload_Files: [],

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

      colunasTabelaCotacao: ["valorUnitario", "fornecedor", "dataEntrega", "acoes"],

      opcoesTabelaCotacoes: {
        perPage: 1000,
        headings: {
          fornecedor: "Fornecedor",
          valorUnitario: "Vl. Unitário",
          dataEntrega: "Data de Entrega",
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

      colunasTabelaDetalhePedido: [
        "material", "quantidade", "valorUnitario", "valorTotal"
      ],

      opcoesTabelaDetalhePedido: {
        perPage: 1000,
        headings: {
          material: "Material",
          quantidade: "Qtd.",
          valorUnitario: "Valor Un.",
          valorTotal: "Total",
        },
        clientSorting: true,
        sortable: ["material"],
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
          material: "Material",
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

      colunasParaValidacao: [
        "codigo",
        "usuario",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorEstimado",
        "acoes",
      ],

      opcoesParaValidacao: {
        perPage: 1000,
        headings: {
          codigo: "Código Solicitação",
          usuario: "Usuário solicitante",
          dataCadastro: "Data da solicitação",
          dataEntrega: "Data de entrega",
          centroCusto: "Centro de custo",
          valorEstimado: "Valor total estimado",
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

      colunasEmCotacao: [
        "codigo",
        "usuario",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorEstimado",
        "valorCotado",
        "comprador",
        "acoes",
      ],

      opcoesEmCotacao: {
        perPage: 1000,
        headings: {
          codigo: "Código Solicitação",
          usuario: "Usuário solicitante",
          dataCadastro: "Data da solicitação",
          dataEntrega: "Data de entrega",
          centroCusto: "Centro de custo",
          valorEstimado: "Valor total estimado",
          valorCotado: "Menor valor total cotado",
          comprador: "Comprador",
          acoes: "Ações",
        },
        clientSorting: false,
        filterable: false,
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

      colunasParaAprovacao: [
        "codigo",
        "usuario",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorTotalCotado",
        "usuarioFinalizacaoCotacao",
        "dataFinalizacaoCotacao",
        "comprador",
        "acoes",
      ],

      opcoesParaAprovacao: {
        perPage: 10,
        headings: {
          codigo: "Código Solicitação",
          usuario: "Usuário solicitante",
          dataEntrega: "Data de entrega",
          dataCadastro: "Data da solicitação",
          centroCusto: "Centro de Custo",
          valorTotalCotado: "Menor valor total cotado",
          usuarioFinalizacaoCotacao: "Usuário finalizou cotação",
          dataFinalizacaoCotacao: "Data finalização da cotação",
          comprador: "Comprador",
          acoes: "Ações",
        },
        clientSorting: true,
        filterable: false,
        sortable: [
          "codigo",
          "usuario",
          "dataEntrega",
          "dataCadastro",
          "centroCusto",
          "valorTotalCotado",
          "usuarioFinalizacaoCotacao",
          "dataFinalizacaoCotacao",
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

      colunasEmCompra: [
        "codigo",
        "codigoSolicitacao",

        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "fornecedor",
        "valorTotalCotado",
        "comprador",
        "acoes",
      ],

      opcoesEmCompra: {
        perPage: 10,
        headings: {
          codigo: "Código",
          codigoSolicitacao: "Código Solicitação",
          dataCadastro: "Data do Pedido",
          dataEntrega: "Data de entrega",
          centroCusto: "Centro de Custo",
          fornecedor: "Fornecedor",
          valorTotalCotado: "Valor",
          comprador: "Comprador",
          acoes: "Ações",
        },
        clientSorting: true,
        filterable: false,
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

      colunasPedidosFinalizadosECancelados: [
        "codigo",
        "codigoSolicitacao",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "fornecedor",
        "valorTotalCotado",
        "status",
        "comprador",
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
          valorTotalCotado: "Menor valor total cotado",
          status: "Status",
          comprador: "Comprador",
          acoes: "Ações",
        },
        clientSorting: true,
        filterable: false,
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

      colunasSolicitacoesCanceladas: [
        "codigo",
        "usuario",
        "dataCadastro",
        "dataEntrega",
        "centroCusto",
        "valorCotado",
        "status",
        "acoes",
      ],

      opcoesSolicitacoesCanceladas: {
        perPage: 10,
        headings: {
          codigo: "Código Solicitação",
          usuario: "Usuário solicitante",
          dataCadastro: "Data da solicitação",
          dataEntrega: "Data de entrega",
          centroCusto: "Centro de custo",
          valorCotado: "Menor valor total cotado",
          status: "Status",
          acoes: "Ações",
        },
        clientSorting: true,
        filterable: false,
        sortable: [
          "codigo",
          "usuario",
          "dataEntrega",
          "dataCadastro",
          "centroCusto",
          "valorEstimado",
          "valorCotado"
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

    };
  },
  methods: {
    // ============================================================
    // Itens dos menus de ações das tabelas (usados pelo <ModalAcoes>).
    // Cada método retorna um array de
    // { label, onClick, visible, icone?, variante?, descricao? }.
    // Ver components/ModalAcoes/ModalAcoes.vue para a API completa.
    // ============================================================
    montaAcoesEmCotacao(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar Cotação",
          descricao: "Abrir a cotação para edição",
          icone: "pencil",
          variante: "primary",
          onClick: () => this.redirecionaDetalhes(row.id),
          visible: u != null && u.comprasOrdensCompraEmCotacaoEditar,
        },
        {
          label: "Finalizar",
          descricao: "Encerrar a cotação e gerar pedido",
          icone: "check",
          variante: "success",
          onClick: () => this.finalizarCotacao(row.id, row.codigo),
          visible: u != null && u.comprasOrdensCompraEmCotacaoFinalizar,
        },
        {
          label: "Cancelar",
          descricao: "Cancelar a solicitação",
          icone: "ban",
          variante: "danger",
          onClick: () => {
            this.solicitacaoParaCancelamento = row;
            this.modalCancelarSolicitacao_Titulo = "Cancelar cotação " + row.codigo;
            this.modalCancelarSolicitacao_Exibir = true;
            this.modalCancelarSolicitacao_IdTab = 2;
          },
          visible: u != null && u.comprasOrdensCompraEmCotacaoCancelar,
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos desta solicitação",
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
        {
          label: "PDF Solicitação Compra",
          descricao: "Baixar PDF com os dados da solicitação",
          icone: "file-pdf-o",
          onClick: () =>
            this.downloadPdfArquivoSolicitacaoCompra(row.id, row.codigo + ".pdf"),
          visible: u != null && u.comprasOrdensCompraEmCotacaoPDF,
        },
        {
          label: "XLS Cotações",
          descricao: "Baixar planilha com as cotações",
          icone: "file-excel-o",
          onClick: () =>
            this.downloadExcelCotacoes(row.id, "Cotações " + row.codigo + ".xlsx"),
          visible: u != null && u.comprasOrdensCompraFinalizadasPDF,
        },
      ];
    },

    // Ações disponíveis para uma nota fiscal dentro do modal "Notas Fiscais".
    // Usa o mesmo padrão (<ModalAcoes>) das tabelas principais, substituindo
    // os 3 botões de ícone (download / trocar obra / cancelar) por um único
    // botão "Ações" que abre um modal com os itens empilhados.
    montaAcoesNotaFiscal(row) {
      return [
        {
          label: "Download",
          descricao: "Baixar o arquivo da nota fiscal",
          icone: "download",
          variante: "warning",
          onClick: () => this.downloadNotaFiscal(row.arquivo.id, row.arquivo.nome),
          visible: !!row.arquivo,
        },
        {
          label: "Trocar Obra",
          descricao: "Alterar a obra vinculada a esta nota fiscal",
          icone: "refresh",
          variante: "success",
          onClick: () => {
            this.modalTrocaObra_IdNotaFiscal = row.id;
            this.modalTrocaObra_NotaFiscal = row.numeroNotaFiscal;
            this.modalTrocaObra_PedidoCompra = this.modalNotasFiscais_Pedido
              ? this.modalNotasFiscais_Pedido.codigo
              : "";
            this.modalTrocaObra_Exibir = true;
          },
        },
        {
          label: "Cancelar Nota Fiscal",
          descricao: "Cancelar esta nota fiscal",
          icone: "ban",
          variante: "danger",
          onClick: () =>
            this.cancelarNotaFiscal(
              row.id,
              row.arquivo ? row.arquivo.nome : ""
            ),
        },
      ];
    },

    // Abre o <ModalAcoes> com as ações da nota fiscal `row`.
    abrirModalAcoesNotaFiscal(row) {
      this.modalAcoes_Itens = this.montaAcoesNotaFiscal(row);
      this.modalAcoes_Titulo = "Ações da nota fiscal " + (row.numeroNotaFiscal || "");
      this.modalAcoes_Exibir = true;
    },

    // Abre o <ModalAcoes> com as ações da tab "Em Cotação" para a linha `row`.
    abrirModalAcoesEmCotacao(row) {
      this.modalAcoes_Itens = this.montaAcoesEmCotacao(row);
      this.modalAcoes_Titulo = "Ações da solicitação " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    montaAcoesParaAprovacao(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar Cotação",
          descricao: "Abrir a cotação para edição",
          icone: "pencil",
          variante: "primary",
          onClick: () => this.redirecionaDetalhes(row.id),
          visible: u != null && u.comprasOrdensCompraEmCotacaoEditar,
        },
        {
          label: "PDF Solicitação Compra",
          descricao: "Baixar PDF com os dados da solicitação",
          icone: "file-pdf-o",
          onClick: () =>
            this.downloadPdfArquivoSolicitacaoCompra(row.id, row.codigo + ".pdf"),
          visible: u != null && u.comprasOrdensCompraEmCotacaoPDF,
        },
        {
          label: "Cancelar",
          descricao: "Cancelar a solicitação",
          icone: "ban",
          variante: "danger",
          onClick: () => {
            this.solicitacaoParaCancelamento = row;
            this.modalCancelarSolicitacao_Titulo = "Cancelar cotação " + row.codigo;
            this.modalCancelarSolicitacao_Exibir = true;
            this.modalCancelarSolicitacao_IdTab = 2;
          },
          visible: u != null && u.comprasOrdensCompraParaAprovacaoCancelar,
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos desta solicitação",
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

    // Abre o <ModalAcoes> com as ações da tab "Para Aprovação" para a linha `row`.
    abrirModalAcoesParaAprovacao(row) {
      this.modalAcoes_Itens = this.montaAcoesParaAprovacao(row);
      this.modalAcoes_Titulo = "Ações da solicitação " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    montaAcoesDiretoria(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar Cotação",
          descricao: "Abrir a cotação para edição",
          icone: "pencil",
          variante: "primary",
          onClick: () => this.redirecionaDetalhes(row.id),
          visible: u != null && u.comprasOrdensCompraEmCotacaoEditar,
        },
        {
          label: "Finalizar",
          descricao: "Encerrar a cotação e gerar pedido",
          icone: "check",
          variante: "success",
          onClick: () => this.finalizarCotacao(row.id, row.codigo),
          visible: u != null && u.comprasOrdensCompraEmCotacaoFinalizar,
        },
        {
          label: "Cancelar Cotação",
          descricao: "Cancelar a cotação",
          icone: "ban",
          variante: "danger",
          onClick: () => {
            this.solicitacaoParaCancelamento = row;
            this.modalCancelarSolicitacao_Titulo = "Cancelar cotação " + row.codigo;
            this.modalCancelarSolicitacao_Exibir = true;
            this.modalCancelarSolicitacao_IdTab = 2;
          },
          visible: u != null && u.comprasOrdensCompraEmCotacaoCancelar,
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos desta solicitação",
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
        {
          label: "PDF Solicitação de Compra",
          descricao: "Baixar PDF com os dados da solicitação",
          icone: "file-pdf-o",
          onClick: () =>
            this.downloadPdfArquivoSolicitacaoCompra(row.id, row.codigo + ".pdf"),
          visible: u != null && u.comprasOrdensCompraEmCotacaoPDF,
        },
      ];
    },

    // Abre o <ModalAcoes> com as ações da tab "Diretoria" para a linha `row`.
    abrirModalAcoesDiretoria(row) {
      this.modalAcoes_Itens = this.montaAcoesDiretoria(row);
      this.modalAcoes_Titulo = "Ações da solicitação " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    montaAcoesEmCompra(row) {
      const u = this.usuarioDTO;
      return [
        {
          label: "Editar informações",
          descricao: "Gerenciar dados do pedido",
          icone: "pencil",
          variante: "primary",
          // Não havia v-if no original — sempre visível.
          onClick: () => {
            this.modalPedido_Exibir = true;
            this.modalPedido_Arquivos = row.arquivos;
            this.modalPedido_Materiais = row.materiais;
            this.modalPedido_IdPedidoCompra = row.id;
            this.modalPedido_Titulo = "Gerenciar Pedido " + row.codigo;
            this.modalPedido_Fornecedor = row.fornecedor;
            this.modalPedido_CondicaoPagamento = row.condicaoPagamento;
            this.modalPedido_EnderecoEntrega = row.enderecoEntrega;
            this.modalPedido_PrazoEntrega = row.dataEntrega;
            this.modalPedido_Frete = row.frete;
            this.modalPedido_Imposto = row.imposto;
            this.modalPedido_PedidoCompra = row;
          },
        },
        {
          label: "Finalizar",
          descricao: "Encerrar o pedido",
          icone: "check",
          variante: "success",
          onClick: () => this.finalizarPedidoCompra(row.id, row.codigo),
          visible: u != null && u.comprasOrdensCompraEmCompraFinalizar,
        },
        {
          label: "Cancelar",
          descricao: "Cancelar o pedido",
          icone: "ban",
          variante: "danger",
          onClick: () => this.cancelarPedidoCompra(row.id, row.codigo),
          visible: u != null && u.comprasOrdensCompraEmCompraCancelar,
        },
        {
          label: "Arquivos",
          descricao: "Ver/enviar anexos deste pedido",
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
          label: "PDF Pedido Compra",
          descricao: "Baixar PDF com os dados do pedido",
          icone: "file-pdf-o",
          onClick: () => this.downloadPdfArquivoCompra(row.id, row.codigo + ".pdf"),
          visible: u != null && u.comprasOrdensCompraEmCompraPDF,
        },
      ];
    },

    // Abre o <ModalAcoes> com as ações da tab "Em Compra" para a linha `row`.
    abrirModalAcoesEmCompra(row) {
      this.modalAcoes_Itens = this.montaAcoesEmCompra(row);
      this.modalAcoes_Titulo = "Ações do pedido " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    // ------ Cancelados / Finalizados ------
    montaAcoesCancelados(row) {
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
      ];
    },
    abrirModalAcoesCancelados(row) {
      this.modalAcoes_Itens = this.montaAcoesCancelados(row);
      this.modalAcoes_Titulo = "Ações do pedido " + row.codigo;
      this.modalAcoes_Exibir = true;
    },

    labelMaterialCotacao({ material }) {
      return `${material.descricao}`;
    },

    itemCotacao({ descricao }) {
      return `${descricao}`;
    },

    nameWithLang({ descricao }) {
      return `${descricao}`;
    },

    descricaoObra({ codigo, descricao }) {
      return `${codigo} - ${descricao}`;
    },

    formataData: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY HH:mm:ss");
    },

    formataDataSemHora: function (data) {
      return moment(String(new Date(data))).format("DD/MM/YYYY");
    },

    adicionaPagamentoServico() {

      if (this.modalPedido_DataPagamentoServico == null) {
        this.$swal("", "Informe uma data válida", "error");
        return;
      }

      if (this.modalPedido_ValorPagamentoServico <= 0) {
        this.$swal("", "Informe um valor válido", "error");
        return;
      }

      this.modalPedido_PedidoCompra.faturas.push(
        {
          id: this.modalPedido_PedidoCompra.faturas.length + 1,
          dataFatura: this.modalPedido_DataPagamentoServico,
          valor: this.modalPedido_ValorPagamentoServico
        }
      );

      this.modalPedido_DataPagamentoServico = null;
      this.modalPedido_ValorPagamentoServico = 0.0;

    },

    removePagamentoSolicitacaoServico(pagamento) {

      this.modalPedido_PedidoCompra.faturas.splice(this.modalPedido_PedidoCompra.faturas.indexOf(pagamento), 1);

    },

    limpaFiltros() {
      this.filtro_ObraSelecionada = [];
      this.filtro_MateriaisSelecionados = [];
      this.filtro_FornecedoresSelecionados = [];
      this.filtro_ClienteSelecionado = [];
      this.filtro_DataInicial = null;
      this.filtro_DataFinal = null;
    },

    exibirModalFiltros(id) {
      this.modalFiltro_IdAba = id;
      this.modalFiltro_Exibir = true;
    },

    filtrar() {
      var objeto = {
        idObra: this.modalFiltro_ObraSelecionada != null ? this.modalFiltro_ObraSelecionada.id : 0,
        idComprador: this.modalFiltro_CompradorSelecionado != null ? this.modalFiltro_CompradorSelecionado : 0,
        idSolicitante: this.modalFiltro_Solicitante != null ? this.modalFiltro_Solicitante.id : 0,
        idEngenheiroAprovador: this.modalFiltro_EngenheiroAprovador != null ? this.modalFiltro_EngenheiroAprovador.id : 0,
        idDiretorAprovador: this.modalFiltro_DiretorAprovador != null ? this.modalFiltro_DiretorAprovador.id : 0
      };

      if (this.modalFiltro_IdAba == 1) {
        this.obtemSolicitacoesCompraEmCotacao();
      }
      else if (this.modalFiltro_IdAba == 2) {
        this.obtemSolicitacoesCompraParaAprovacao();
      }
      else if (this.modalFiltro_IdAba == 3) {
        this.obtemSolicitacoesDevolvidasDiretoria();
      }
      else if (this.modalFiltro_IdAba == 4) {
        this.obtemPedidosEmCompra();
      }

      this.modalFiltro_Exibir = false;
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

    preencherModalGerenciarSolicitacaoCompra: function (solicitacao) {
      this.materiaisAdicionados = [];
      this.modalGerenciarSolicitacaoCompra_ValorTotal = 0;

      this.modalGerenciarSolicitacaoCompra_Observacao = solicitacao.observacao;
      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada = solicitacao;
      this.modalGerenciarSolicitacaoCompra_DataEntrega = solicitacao.dataEntrega;

      solicitacao.materiais.forEach((material) => {
        var objetoMaterial = {
          id: this.materiaisAdicionados.length + 1,
          idMaterial: material.idMaterial,
          material: this.materiais.filter((x) => {
            return x.id == material.idMaterial;
          })[0].descricao,
          quantidade: material.quantidade,
          valorUnitario: material.valorUnitarioEstimado,
          valorUnitarioFormatado: new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
          }).format(material.valorUnitarioEstimado),
          valorTotal: material.quantidade * material.valorUnitarioEstimado,
          valorTotalFormatado: new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
          }).format(material.quantidade * material.valorUnitarioEstimado),
        };

        this.modalGerenciarSolicitacaoCompra_ValorTotal =
          this.modalGerenciarSolicitacaoCompra_ValorTotal +
          objetoMaterial.valorTotal;
        this.materiaisAdicionados.push(objetoMaterial);
      });
    },

    editarSolicitacaoCompra: function () {
      this.isLoading = true;

      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada.observacao =
        this.modalGerenciarSolicitacaoCompra_Observacao;
      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada.valorEstimado =
        this.modalGerenciarSolicitacaoCompra_ValorTotal;
      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada.materiais =
        [];
      this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada.dataEntrega = this.modalGerenciarSolicitacaoCompra_DataEntrega;

      this.materiaisAdicionados.forEach((material) => {
        var objetoMaterial = {
          id: 0,
          idSolicitacaoCompra: 0,
          idMaterial: material.idMaterial,
          quantidade: material.quantidade,
          valorUnitarioEstimado: material.valorUnitario,
        };
        this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada.materiais.push(
          objetoMaterial
        );

        valorDosMateriais += parseFloat(material.valorUnitario.toString().replace(',', '.'))
      });

      ApiService.put(
        "SolicitacaoCompra",
        this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada,
        (result) => {
          this.isLoading = false;

          if (result.status != 201) {
            this.$swal(
              "Erro ao editar solicitação de compra",
              result.message,
              "error"
            );
          } else {
            this.obtemSolicitacoesCompraParaValidacao();

            this.modalGerenciarSolicitacaoCompra_SolicitacaoSelecionada = null;
            this.modalGerenciarSolicitacaoCompra_Observacao = "";
            this.modalGerenciarSolicitacaoCompra_ValorTotal = 0.0;
            this.modalGerenciarSolicitacaoCompra_Quantidade = 0.0;
            this.modalGerenciarSolicitacaoCompra_ValorUnitario = 0.0;
            this.modalGerenciarSolicitacaoCompra_Exibir = false;
            this.modalGerenciarSolicitacaoCompra_DataEntrega = null;
          }
        }
      );
    },

    reabrirSolicitacao: function () {

      if (this.modalReabrirSolicitacao_DataEntrega == null) {
        this.$swal("", "Selecione uma data de entrega", "error");
        return;
      }

      if (this.modalReabrirSolicitacao_ObraSelecionada == null)
        this.$swal("", "Selecione um novo centro de custo", "error");
      else {
        this.isLoading = true;

        var objetoReaberturaSolicitacao = {
          idSolicitacaoCompra: this.modalReabrirSolicitacao_IdSolicitacao,
          idObraSelecionada: this.modalReabrirSolicitacao_ObraSelecionada.id,
          dataEntrega: this.modalReabrirSolicitacao_DataEntrega
        };

        ApiService.reabrirSolicitacaoCompra(objetoReaberturaSolicitacao,
          (resultAPI) => {
            this.isLoading = false;
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

    montaListaMateriaisParaCotacao: function (solicitacao) {
      this.modalCotacao_MaterialSelecionado = null;
      this.modalCotacao_Quantidade = 0;
      this.modalCotacao_ValorUnitario = 0;
      this.modalCotacao_FornecedorSelecionado = null;
      this.modalCotacao_Solicitacao = solicitacao;
      this.modalCotacao_Materiais = [];

      this.modalCotacao_Solicitacao.materiais.forEach((material) => {
        this.modalCotacao_Materiais.push(material);
      });
    },

    adicionarCotacao: function () {
      this.isLoading = true;

      var idSolicitacaoCompraMaterial =
        this.modalCotacao_MaterialSelecionado == null
          ? 0
          : this.modalCotacao_MaterialSelecionado.id;
      var idFornecedor =
        this.modalCotacao_FornecedorSelecionado == null
          ? 0
          : this.modalCotacao_FornecedorSelecionado.id;
      var valorUnitario = this.modalCotacao_ValorUnitario;

      let objCotacao = {
        id: 0,
        idSolicitacaoCompraMaterial: idSolicitacaoCompraMaterial,
        idFornecedor: idFornecedor,
        valorUnitarioCotado: valorUnitario,
        cotacaoFinal: this.modalCotacao_CotacaoPrincipal,
        dataEntrega: this.modalCotacao_DataEntrega,
      };

      ApiService.cadastrarCotacao(objCotacao, (result) => {
        this.isLoading = false;

        if (result.status != 201) {
          this.$swal("Erro ao cadastrar cotação", result.data, "error");
        } else {
          this.modalCotacao_ValorUnitario = 0.0;
          this.modalCotacao_DataEntrega = null;
          this.obtemCotacoes(idSolicitacaoCompraMaterial);
        }
      });
    },

    obtemCotacoes: function (idSolicitacaoCompraMaterial) {
      this.isLoading = true;
      this.modalCotacao_Cotacoes = [];

      ApiService.obtemCotacoes(idSolicitacaoCompraMaterial, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalCotacao_Cotacoes = result.data;
        }
      });
    },

    obtemCotacoesPedido: function (idSolicitacaoCompraMaterial) {
      this.isLoading = true;
      this.modalPedido_Cotacoes = [];

      ApiService.obtemCotacoes(idSolicitacaoCompraMaterial, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalPedido_Cotacoes = result.data;
        }
      });
    },

    excluirCotacao: function (id, idSolicitacaoCompraMaterial) {
      this.isLoading = true;

      ApiService.delete('SolicitacaoCompra/Cotacao', id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.obtemCotacoes(idSolicitacaoCompraMaterial);
        }
      });
    },

    definirCotacaoPrincipal: function (id, idSolicitacaoCompraMaterial) {
      this.isLoading = true;

      ApiService.definirCotacaoPrincipal('SolicitacaoCompra/Cotacao/Principal', id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.obtemCotacoes(idSolicitacaoCompraMaterial);
        }
      });
    },

    redirecionaDetalhes(idSolicitacao) {
      let routeData = this.$router.resolve({
        name: "Cotacao",
        params: { id: idSolicitacao },
      });
      window.open(routeData.href, "_self");
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
            (resultAPI) => {
              this.isLoading = false;
              if (resultAPI.status != 200) {
                this.$swal("", resultAPI.message, "error");
              } else {
                this.solicitacaoParaValidacao = null;
                this.obtemQuantidades();
                this.obtemSolicitacoesCompraParaValidacao();
              }
            }
          );
        }
      });
    },

    finalizarCotacao: function (id, codigo) {
      this.$swal({
        title: "Atenção",
        text: "Deseja finalizar a cotação da solicitação " + codigo + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, finalizar !",
        cancelButtonText: "Não",
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.finalizarCotacao(
            "SolicitacaoCompra/Cotacao/Finalizar/",
            id,
            (result) => {
              this.isLoading = false;
              if (result.status != 200) {

                this.$swal("", result.message, "error");
              } else {
                this.obtemQuantidades();
                this.obtemSolicitacoesCompraEmCotacao();
              }
            }
          );

        }
      });
    },

    aprovarPedido: function (id, codigo) {
      this.$swal({
        title: "Atenção",
        text: "Deseja aprovar a cotação " + codigo + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, aprovar !",
        cancelButtonText: "Não",
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.finalizarCotacao(
            "SolicitacaoCompra/Cotacao/Aprovar/",
            id,
            (result) => {
              this.isLoading = false;
              if (result.status != 200) {
                this.$swal("", result.message, "error");
              } else {
                this.$swal("Cotação" + codigo + " aprovada com sucesso", result.message, "success");
                this.obtemQuantidades();
                this.obtemSolicitacoesCompraParaAprovacao();
              }
            }
          );

        }
      });
    },

    cancelarSolicitacao: function () {
      this.isLoading = true;

      ApiService.cancelarSolicitacaoCompra(
        this.solicitacaoParaCancelamento.id,
        this.modalCancelarSolicitacao_Motivo,
        (result) => {
          this.isLoading = false;

          if (result.status != 200) {
            this.$swal("", result.message, "error");
          } else {
            this.obtemQuantidades();
            if (this.modalCancelarSolicitacao_IdTab == 1)
              this.obtemSolicitacoesCompraParaValidacao();
            else if (this.modalCancelarSolicitacao_IdTab == 2)
              this.obtemSolicitacoesCompraEmCotacao();
            else if (this.modalCancelarSolicitacao_IdTab == 3)
              this.obtemSolicitacoesCompraParaAprovacao();

            this.modalCancelarSolicitacao_IdTab = 0;
            this.solicitacaoParaCancelamento = null;
            this.modalCancelarSolicitacao_Motivo = "";
            this.modalCancelarSolicitacao_Exibir = false;
          }
        }
      );
    },

    finalizarPedidoCompra: function (id, codigo) {
      this.$swal.fire({
        title: "Atenção",
        text: "Deseja finalizar o pedido " + codigo + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: 'Sim, finalizar',
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !this.$swal.isLoading()
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.finalizarPedidoCompra(id, (resultAPI) => {
            this.isLoading = false;

            if (resultAPI.status != 200) {
              this.$swal("", resultAPI.message, "error");
            } else {
              this.obtemPedidosEmCompra();
            }
          });


        }
      })
    },

    cancelarPedidoCompra: function (id, codigo) {
      this.$swal.fire({
        title: "Atenção",
        text: "Deseja cancelar o pedido " + codigo + " ?",
        icon: "warning",
        input: 'text',
        inputAttributes: {
          autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Sim, cancelar',
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !this.$swal.isLoading()
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.cancelarPedidoCompra(id, result.value, (result) => {
            this.isLoading = false;

            if (result.status != 200) {
              this.$swal("", result.message, "error");
            } else {
              this.obtemPedidosEmCompra();
            }
          });


        }
      })
    },

    reprovarCotacao: function (id, codigo) {
      this.$swal.fire({
        title: "Atenção",
        text: "Deseja reprovar a cotação " + codigo + " ?",
        icon: "warning",
        input: 'text',
        inputAttributes: {
          autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Sim, reprovar',
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !this.$swal.isLoading()
      }).then((result) => {
        if (result.isConfirmed) {
          this.isLoading = true;

          ApiService.reprovarCotacao(id, result.value, (result) => {
            this.isLoading = false;

            if (result.status != 200) {
              this.$swal("", result.message, "error");
            } else {
              this.obtemSolicitacoesCompraParaAprovacao();
            }
          });


        }
      })
    },

    obtemSolicitacoesCompraParaValidacao: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = true;
      this.tab3Ativa = false;
      this.tab4Ativa = false;
      this.tab5Ativa = false;
      this.tab6Ativa = false;

      this.isLoading = true;
      this.obtemQuantidades();
      this.solicitacoesParaValidacao = [];

      ApiService.getSolicitacoesCompraStatus(1, (result) => {
        this.isLoading = false;
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.solicitacoesParaValidacao = result.data;
        }
      });
    },

    obtemSolicitacoesDevolvidasDiretoria: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = false;
      this.tab3Ativa = false;
      this.tab4Ativa = false;
      this.tab5Ativa = false;
      this.tab6Ativa = false;
      this.tab7Ativa = true;

      this.isLoading = true;
      this.obtemQuantidades();
      this.solicitacoesDevolvidasDiretoria = [];

      ApiService.getSolicitacoesCompraStatus(7, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.solicitacoesDevolvidasDiretoria = result.data;
        }
      });
    },

    obtemSolicitacoesCompraEmCotacao: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = true;
      this.tab3Ativa = false;
      this.tab4Ativa = false;
      this.tab5Ativa = false;
      this.tab6Ativa = false;
      this.tab7Ativa = false;

      this.isLoading = true;
      this.obtemQuantidades();
      this.solicitacoesEmCotacao = [];

      ApiService.getSolicitacoesCompraStatus(2, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.solicitacoesEmCotacao = result.data;
        }
      });
    },

    obtemSolicitacoesCompraParaAprovacao: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = false;
      this.tab3Ativa = true;
      this.tab4Ativa = false;
      this.tab5Ativa = false;
      this.tab6Ativa = false;

      this.isLoading = true;
      this.obtemQuantidades();
      this.pedidosParaAprovacao = [];

      ApiService.getSolicitacoesCompraStatus(3, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.pedidosParaAprovacao = result.data;
        }
      });
    },

    obtemPedidosEmCompra: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = false;
      this.tab3Ativa = false;
      this.tab4Ativa = true;
      this.tab5Ativa = false;
      this.tab6Ativa = false;

      this.isLoading = true;
      this.obtemQuantidades();
      this.pedidosEmCompra = [];

      ApiService.getPedidosCompraStatus(1, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.pedidosEmCompra = result.data;

          this.pedidosEmCompra.forEach(x => {

            var i = 0;
            x.faturas.forEach(f => {
              f.id = i;
              i++;
            })
          });
          console.log(result.data);
        }
      });
    },

    obtemSolicitacoesFinalizadasCanceladas: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = false;
      this.tab3Ativa = false;
      this.tab4Ativa = false;
      this.tab5Ativa = false;
      this.tab6Ativa = true;

      this.isLoading = true;
      this.obtemQuantidades();
      this.solicitacoesCanceladas = [];

      ApiService.getSolicitacoesCompraStatus(4, (resultA) => {
        if (resultA.status != 200) {

          this.isLoading = false;
          this.$swal("", resultA.message, "error");

        } else {
          ApiService.getSolicitacoesCompraStatus(5, (resultB) => {
            this.isLoading = false;

            if (resultB.status != 200) {
              this.$swal("", resultB.message, "error");
            } else {
              this.solicitacoesCanceladas = resultA.data;
              resultB.data.forEach((solicitacao) => {
                this.solicitacoesCanceladas.push(solicitacao);
              });
            }
          });
        }
      });
    },

    obtemPedidosFinalizadosCancelados: function () {
      this.tab1Ativa = false;
      this.tab2Ativa = false;
      this.tab3Ativa = false;
      this.tab4Ativa = false;
      this.tab5Ativa = true;
      this.tab6Ativa = false;

      this.isLoading = true;
      this.obtemQuantidades();
      this.pedidosCancelados = [];

      ApiService.getPedidosCompraStatus(2, (resultA) => {
        if (resultA.status != 200) {

          this.isLoading = false;
          this.$swal("", resultA.message, "error");

        } else {
          ApiService.getPedidosCompraStatus(3, (resultB) => {
            this.isLoading = false;

            if (resultB.status != 200) {
              this.$swal("", resultB.message, "error");
            } else {
              this.pedidosCancelados = resultA.data;
              resultB.data.forEach((solicitacao) => {
                this.pedidosCancelados.push(solicitacao);
              });
            }
          });
        }
      });
    },

    obtemQuantidades: function () {
      ApiService.obtemQuantidades((result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.quantidades = result.data;
        }
      });
    },

    adicionaMaterial: function () {
      if (
        this.materialSelecionado != null &&
        this.modalGerenciarSolicitacaoCompra_Quantidade > 0
      ) {
        var objetoMaterial = {
          id: this.materiaisAdicionados.length + 1,
          idMaterial: this.materialSelecionado.id,
          material: this.materialSelecionado.descricao,
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
        this.materialSelecionado = null;
      }
    },

    removerMaterial: function (idMaterial) {
      this.materiaisAdicionados.forEach((material) => {
        if (material.id == idMaterial) {
          this.modalGerenciarSolicitacaoCompra_ValorTotal =
            this.modalGerenciarSolicitacaoCompra_ValorTotal -
            material.valorTotal;
          this.materiaisAdicionados.splice(
            this.materiaisAdicionados.indexOf(material),
            1
          );
        }
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

    obtemArquivos: function (idSolicitacaoOuPedidoCompra) {
      ApiService.getArquivos(this.modalArquivos_Controller, idSolicitacaoOuPedidoCompra, (result) => {
        this.isLoading = false;

        if (result.status == 200) {
          this.modalArquivos_Arquivos = result.data;
        }
      });
    },


    onChange() {
      this.upload_Files = [...this.$refs.file.files];

      this.uploadFiles();
    },

    removeFileToUpload() {
      this.upload_Files = [];
      this.upload_ExibirBotoes = false;
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

    enviarComentario() {
      this.isLoading = true;

      ApiService.enviarComentario(this.modalComentarios_Controller, this.modalComentarios_IdSolicitacaoOuPedidoCompra, this.modalComentarios_Comentario, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalComentarios_Comentario = '';
          this.obtemComentarios();
          this.$swal("Comentário enviado com sucesso", result.message, "success");
        }
      });

    },

    editarPedidoCompra() {

      if (this.modalPedido_Fornecedor == null) {
        this.$swal("Fornecedor inválido", "", "error");
        return;
      }

      if (this.modalPedido_CondicaoPagamento == null) {
        this.$swal("Condição de pagamento inválida", "", "error");
        return;
      }

      if (this.modalPedido_EnderecoEntrega == '') {
        this.$swal("Endereço de entrega inválido", "", "error");
        return;
      }

      var valorFaturas = (this.modalPedido_PedidoCompra && this.modalPedido_PedidoCompra.faturas || []).reduce((soma, x) => soma + (+x.valor || 0), 0);
      var valorMateriais = (this.modalPedido_PedidoCompra && this.modalPedido_PedidoCompra.materiais || []).reduce((soma, x) => soma + ((+x.quantidade || 0) * (+x.valorUnitario || 0)), 0);

      if (this.modalPedido_CondicaoPagamento.id == 7 && ((valorMateriais + this.modalPedido_Frete + this.modalPedido_Imposto) != valorFaturas)) {
        this.$swal("O valor das faturas está diferente do valor dos materiais com imposto e frete", "", "error");
        return;
      }

      var pedidoCompra = this.modalPedido_PedidoCompra;
      pedidoCompra.idFornecedor = this.modalPedido_Fornecedor.id;
      pedidoCompra.idCondicaoPagamento = this.modalPedido_CondicaoPagamento.id;
      pedidoCompra.enderecoEntrega = this.modalPedido_EnderecoEntrega;
      pedidoCompra.dataEntrega = this.modalPedido_PrazoEntrega;
      pedidoCompra.frete = this.modalPedido_Frete;
      pedidoCompra.imposto = this.modalPedido_Imposto;

      this.isLoading = true;

      ApiService.put('PedidoCompra', pedidoCompra, (result) => {

        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        }
        else {

          this.obtemPedidosEmCompra();
          this.$swal("Pedido de compra editado com sucesso", result.message, "success");
          this.modalPedido_Exibir = false;
        }
      });
    },

    obtemComentarios() {
      ApiService.obtemComentarios(this.modalComentarios_Controller, this.modalComentarios_IdSolicitacaoOuPedidoCompra, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalComentarios_Comentarios = result.data;
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

    cancelarNotaFiscal(id, nomeArquivo) {
      this.isLoading = true;

      ApiService.cancelarNotaFiscal(id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalNotasFiscais_Exibir = false;
        }
      });
    },

    alterarObraNF() {
      if (this.modalTrocaObra_ObraSelecionada == null) {
        this.$swal("Selecione uma obra", "", "error");
        return;
      }

      ApiService.alterarObraNF(this.modalTrocaObra_IdNotaFiscal, this.modalTrocaObra_ObraSelecionada.id, (result) => {
        this.isLoading = false;

        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.modalNotasFiscais_Exibir = false;
        }
      });
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

    listaUsuarios: function () {
      this.usuarios = [];
      this.usuariosAprovadores = [];

      ApiService.getAll("Usuario", true, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.usuarios = result.data;
          this.usuariosAprovadores = result.data;

          this.usuariosCompradores = this.usuarios.filter(u => u.idCargo === 4);
          this.diretoresAprovadores = this.usuarios.filter(u => u.idCargo === 3);
        }
      });
    },

  },
  created() {
    this.tagParaRedirecionar = this.$route.params.tag;
  },
  mounted() {

    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'));

    this.obtemQuantidades();
    this.obtemSolicitacoesCompraEmCotacao();
    this.listaMateriais();
    this.listaObras();
    this.listaDefs();
    this.listaFornecedores();
    this.listaClientes();
    this.listaCondicoesDePagamento();
    this.listaUsuarios();

    if (this.tagParaRedirecionar == 'ParaAprovacao') {
      this.obtemSolicitacoesCompraParaAprovacao();

      console.log('ojboibvob');
      console.log(this.$refs);
      // this.$refs.tabParaAprovacao.click;
    }
  },
};
</script>


<style src="./OrdemCompra.scss" lang="scss" />
