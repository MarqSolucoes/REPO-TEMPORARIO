<template>
  <div class="cotacao-page">
    <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>

    <!-- ============== CABEÇALHO (mantido, mais limpo) ============== -->
    <h1 class="page-title" v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 ||solicitacaoCompra.idStatusSolicitacaoCompra == 3)">Cotação</h1>
    <h1 class="page-title" v-if="solicitacaoCompra && solicitacaoCompra.idStatusSolicitacaoCompra == 8">Revisão</h1>

    <section class="header-card">
      <div class="top-summary">
        <div class="summary-grid">
          <div class="kv"><span class="k">Solicitação</span><span class="v">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.codigo) || '—' }}</span></div>
          <div class="kv"><span class="k">Data Entrega</span><span class="v">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.dataEntrega) ? formataDataSemHora(solicitacaoCompraParaCotacao.dataEntrega) : 'Não informada' }}</span></div>
          <div class="kv"><span class="k">Obra</span><span class="v">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.obra) || '—' }}</span></div>
          <div class="kv"><span class="k">Cliente</span><span class="v">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.cliente) || '—' }}</span></div>
          <div class="kv"><span class="k">Valor Estimado</span><span class="v">{{ solicitacaoCompraParaCotacao ? new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(solicitacaoCompraParaCotacao.valorEstimado) : '—' }}</span></div>
        </div>
        <div class="summary-actions">
          <button class="details-toggle" @click="headerExpanded = !headerExpanded" :title="headerExpanded ? 'Ocultar detalhes' : 'Mostrar detalhes'">
            <i :class="['fa', headerExpanded ? 'fa-chevron-up' : 'fa-chevron-down']"></i>
            <span>{{ headerExpanded ? 'Ocultar detalhes' : 'Mostrar detalhes' }}</span>
          </button>
        </div>
      </div>

      <!-- Detalhes editáveis (endereço/observações) alinhados na MESMA linha -->
      <transition name="fade">
        <div v-show="headerExpanded" class="details-grid">
          <div class="field mini-card">
            <div class="mini-card__header">Endereço de entrega</div>
            <b-form-textarea
              v-if="solicitacaoCompraParaCotacao"
              v-model="solicitacaoCompraParaCotacao.endereco"
              rows="3"
              :placeholder="'Ex.: Rua X, nº 123, Bairro, Cidade/UF, CEP'"
            />
          </div>

          <div class="field mini-card">
            <div class="mini-card__header">Observação da solicitação <span class="muted">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.observacao ? solicitacaoCompraParaCotacao.observacao.length : 0) }} / 1000</span></div>
            <b-form-textarea
              v-if="solicitacaoCompraParaCotacao"
              v-model="solicitacaoCompraParaCotacao.observacao"
              rows="3"
              :maxlength="1000"
              :placeholder="'Contexto interno da cotação, critérios, restrições, etc.'"
            />
          </div>

          <div class="field mini-card">
            <div class="mini-card__header">Observação para o fornecedor <span class="muted">{{ (solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.observacaoParaFornecedor ? solicitacaoCompraParaCotacao.observacaoParaFornecedor.length : 0) }} / 500</span></div>
            <b-form-textarea
              v-if="solicitacaoCompraParaCotacao"
              v-model="solicitacaoCompraParaCotacao.observacaoParaFornecedor"
              rows="3"
              :maxlength="500"
              :placeholder="'Instruções objetivas ao fornecedor (prazo, acesso, etc.)'"
            />
          </div>
        </div>
      </transition>
    </section>

    <!-- ============== BARRA DE AÇÕES (mesmas regras) ============== -->
    <b-row class="mb-3">
      <b-col lg="6">
        <!-- Comentado: já existe no menu lateral -->
        <!--
        <button type="button" class="btn width-75 mb-3 mr-3 bg-success"
          @click="modalNovoFornecedor_Exibir = true;"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 7)">
          Adicionar novo fornecedor
        </button>
        -->

        <button type="button" class="btn width-75 mb-3 mr-3 bg-danger" @click="removerItensSelecionados()"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 7)">
          Remover itens selecionados para novo pedido
        </button>
        <button type="button" class="btn width-75 mb-3 mr-3 bg-warning" @click="duplicarItensSelecionados()"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 7)">
          Duplicar itens selecionados para novo pedido
        </button>
      </b-col>
      <b-col lg="6" style="text-align: right;">
        <button type="button" class="btn width-75 mb-3 mr-3 bg-warning" style="font-weight: bold;"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 3 || solicitacaoCompra.idStatusSolicitacaoCompra == 7|| solicitacaoCompra.idStatusSolicitacaoCompra == 8)"
          @click="editarCotacao();">
          Salvar edição
        </button>

        <button type="button" class="btn width-75 mb-3 mr-3 bg-warning" style="font-weight: bold;"
          v-if="solicitacaoCompra && solicitacaoCompra.idStatusSolicitacaoCompra == 8 && solicitacaoCompra.servico == true"
          @click="enviarDiretoria();">
          Enviar Diretoria
        </button>

        <button type="button" class="btn width-75 mb-3 mr-3 bg-warning" style="font-weight: bold;"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 7)"
          @click="enviarDiretoria();">
          Enviar Diretoria
        </button>

        <button type="button" @click="finalizarCotacao()" class="btn mb-3 mr-3 btn-success"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 7)">
          <i class="fa fa-thumbs-o-up" title="Finalizar"></i>&nbsp;Finalizar
        </button>

        <button type="button" class="btn width-75 mb-3 mr-3 bg-warning" style="font-weight: bold;"
          v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 3)"
          @click="devolverCompras();">
          Devolver Compras
        </button>

        <button type="button" class="btn mb-3 mr-3 btn-info bg-warning" style="font-weight: bold;"
          v-if="solicitacaoCompra && solicitacaoCompra.servico == true && solicitacaoCompra.valorTotalCotado != null && solicitacaoCompra.idStatusSolicitacaoCompra == 3"
          @click="refazerOrcamento();">
          Refazer Orçamento
        </button>

        <button type="button"
          @click="downloadExcelCotacoes(solicitacaoCompra.id, 'Cotações ' + solicitacaoCompra.codigo + '.xlsx')"
          class="btn mb-3 mr-3 btn-success bg-success" v-if="usuarioDTO && usuarioDTO.comprasOrdensCompraFinalizadasPDF">
          Excel Cotações
        </button>

        <!-- DEPOIS -->
        <span class="comentarios-btn-wrapper">
          <button type="button" @click="modalComentarios_Exibir = true; modalComentarios_Controller = 'SolicitacaoCompra'; modalComentarios_IdSolicitacaoOuPedidoCompra = solicitacaoCompra.id; modalComentarios_Comentarios = solicitacaoCompra.comentarios;" class="btn mb-3 mr-3 btn-info">
            <i class="fa fa-comment" title="Comentários"></i>
          </button>
          <span
            v-if="solicitacaoCompra && solicitacaoCompra.comentarios && solicitacaoCompra.comentarios.length > 0"
            class="comentarios-badge">
            {{ solicitacaoCompra.comentarios.length }}
          </span>
        </span>
      </b-col>
    </b-row>

    <!-- ============== LAYOUT PRINCIPAL: fornecedor (esquerda) × materiais (direita) ============== -->
    <div class="layout">
      <!-- Lista de fornecedores -->
      <aside class="suppliers">
        <div class="suppliers-header">
          <div class="title">Fornecedores</div>
          <button class="btn btn-sm" @click="modalNovoFornecedor_Exibir = true"
            v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra >= 7)">+ Adicionar</button>
        </div>
        <ul class="supplier-list">
          <li v-for="f in ((solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.fornecedores) || [])" :key="f.id"
              :class="{ active: f.id === fornecedorSelecionadoId }"
              @click="selecionarFornecedor(f.id)">
            <div class="name">{{ f.nomeFantasia || f.razaoSocial }}</div>
            <div class="cnpj">{{ f.cnpj }}</div>
            <div class="cond-pag" v-if="f.condicaoPagamento && f.condicaoPagamento.descricao">{{ f.condicaoPagamento.descricao }}</div>
            <div class="total">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(totalFornecedor(f.id)) }}</div>
          </li>
        </ul>
      </aside>

      <!-- Painel do fornecedor selecionado -->
      <section class="materials" v-if="fornecedorSelecionado">
        <div class="materials-header">
          <div class="left">
            <h3>{{ fornecedorSelecionado.nomeFantasia || fornecedorSelecionado.razaoSocial }}</h3>
            <small>{{ fornecedorSelecionado.cnpj }}</small>
          </div>
          <div class="right">
            <button type="button" @click="removeFornecedor(fornecedorSelecionado.id)" class="btn btn-outline-danger btn-sm">
              <i class="fa fa-trash"></i> Remover fornecedor
            </button>
          </div>
        </div>

        <!-- GRID: Tabela compacta (60%) + Painel lateral (40%) -->
        <div class="materials-grid">
          <!-- Tabela de itens COMPACTA -->
          <div class="items-table">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho" style="min-width: 260px;"># / Item / Descrição</th>
                    <th class="estilo-cabecalho texto-centro">Un</th>
                    <th class="estilo-cabecalho texto-centro">Qtd</th>
                    <th class="estilo-cabecalho texto-centro">Orçado</th>
                    <th class="estilo-cabecalho texto-centro">Vlr unit.</th>
                    <th class="estilo-cabecalho texto-centro">Vlr unit. desconto</th>
                    <th class="estilo-cabecalho texto-centro" v-if="solicitacaoCompra && solicitacaoCompra.idStatusSolicitacaoCompra == 7">Qtd cot.</th>
                    <th class="estilo-cabecalho texto-centro">Entrega</th>
                    <th class="estilo-cabecalho texto-centro">Subtotal</th>
                    <th class="estilo-cabecalho texto-centro">&nbsp;</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(material, index) in ((solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.materiais) || [])" :key="'cot-' + (material.idMaterial || index)">
                    <td class="estilo-celula">
                      | {{ index + 1 }} |
                      <input type="checkbox" v-model="material.itemSelecionado"/>
                      <a @click="abrirModalHistoricoDePreco(material.idMaterial)">{{ material.descricao }}</a>
                    </td>
                    <td class="estilo-celula texto-centro">{{ (material.unidadeMaterial && (material.unidadeMaterial.codigo || material.unidadeMaterial.descricao)) || 'UN' }}</td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--money">
                        <Money v-model="material.quantidade" v-bind="decimalInput" class="input-compact"/>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(material.valorUnitarioEstimado || 0) }}</td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--money">
                        <Money v-model="getCotacao(material, fornecedorSelecionado.id).valorUnitarioCotado" v-bind="money" class="input-compact"/>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(getCotacaoComDesconto(material, fornecedorSelecionado.id).valorComDesconto || 0) }}</td>
                    <td class="estilo-celula texto-centro" v-if="solicitacaoCompra && solicitacaoCompra.idStatusSolicitacaoCompra == 7">
                      <div class="input-table input-table--money">
                        <Money v-model="getCotacao(material, fornecedorSelecionado.id).quantidadeCotado" v-bind="decimalInput" class="input-compact"/>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">
                      <div class="input-table input-table--date">
                        <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
                          v-model="getCotacao(material, fornecedorSelecionado.id).dataEntrega"
                          format="dd/MM/yyyy" type="date" :open.sync="open" class="date-compact"/>
                      </div>
                    </td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(subtotalItem(material, fornecedorSelecionado.id)) }}</td>
                    <td class="estilo-celula texto-centro">
                      <button type="button" @click="abrirModalHistoricoDePreco(material.idMaterial)" class="btn btn-success btn-icon" :title="'Histórico Preços'" v-if="solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.servico == false">
                        <i class="fa fa-money"></i>
                      </button>
                      &nbsp;
                      <button type="button" @click="excluirMaterial(material, solicitacaoCompraParaCotacao.id)" class="btn btn-outline-danger btn-icon" :title="'Excluir Material'" v-if="solicitacaoCompraParaCotacao && solicitacaoCompraParaCotacao.servico == false">
                        <i class="fa fa-trash"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="!solicitacaoCompraParaCotacao || !solicitacaoCompraParaCotacao.materiais || solicitacaoCompraParaCotacao.materiais.length === 0">
                    <td class="estilo-celula texto-centro" :colspan="solicitacaoCompra && solicitacaoCompra.idStatusSolicitacaoCompra == 7 ? 10 : 9">Nenhum material adicionado</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <!-- Painel lateral: condição, totais e (se Manual) parcelas -->
          <div class="side-panel">
            <div class="payment-box">
              <div class="row-2">
                <div>
                  <label>Condição</label>
                  <multiselect v-model="fornecedorSelecionado.condicaoPagamento" :multiple="false"
                    :options="condicoesDePagamento" select-label="Selecionar"
                    placeholder="Selecione uma condição" :custom-label="customLabelCondicaoPagamento"/>
                </div>
                <div>
                  <label>Desconto [%]</label>
                  <Money v-model="fornecedorSelecionado.desconto" v-bind="number" class="input-compact"/>
                </div>
                <div>
                  <label>Frete</label>
                  <Money v-model="fornecedorSelecionado.frete" v-bind="number" class="input-compact"/>
                </div>
                <div>
                  <label>Imposto</label>
                  <Money v-model="fornecedorSelecionado.imposto" v-bind="number" class="input-compact"/>
                </div>
              </div>

              <div class="totais">
                <div class="total-line">
                  <span>Materiais (s/ desc., s/ frete/imposto)</span>
                  <strong>{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(somaMateriaisFornecedor(fornecedorSelecionado.id)) }}</strong>
                </div>
                <div class="total-line emph" :class="{ mismatch: isManualMismatch }">
                  <span>Total do fornecedor</span>
                  <strong>{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(totalFornecedor(fornecedorSelecionado.id)) }}</strong>
                </div>
                <div v-if="fornecedorSelecionado && fornecedorSelecionado.condicaoPagamento && fornecedorSelecionado.condicaoPagamento.descricao === 'Manual'" class="total-line manual-total" :class="{ mismatch: isManualMismatch }">
                  <span>Total das parcelas (manual)</span>
                  <strong>{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(totalManualSelecionado) }}</strong>
                </div>
                <div v-if="fornecedorSelecionado && fornecedorSelecionado.condicaoPagamento && fornecedorSelecionado.condicaoPagamento.descricao === 'Manual'" class="hint" :class="{ 'text-ok': !isManualMismatch, 'text-error': isManualMismatch }">
                  {{ isManualMismatch ? 'A soma das parcelas precisa ser IGUAL ao total do fornecedor.' : 'OK: soma das parcelas igual ao total.' }}
                </div>
              </div>
            </div>

            <!-- ===== COTAÇÃO MANUAL ===== -->
            <div
              class="manual-box"
              v-if="fornecedorSelecionado && fornecedorSelecionado.condicaoPagamento && fornecedorSelecionado.condicaoPagamento.descricao === 'Manual'">

              <div class="manual-header">
                <div class="title">Cotação Manual</div>
                <button class="btn bg-success btn-sm" @click="adicionaPagamentoManualSelecionado()">
                  + Adicionar Pagamento
                </button>
              </div>

              <div class="table-card table-card--fluid">
                <table class="estilo-tabela tabela-identidade">
                  <thead>
                    <tr>
                      <th class="estilo-cabecalho texto-centro" style="width: 140px;">Data</th>
                      <th class="estilo-cabecalho texto-centro" style="width: 160px;">Valor</th>
                      <th class="estilo-cabecalho texto-centro" style="width: 70px;">Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(pg, idx) in (fornecedorSelecionado.pagamentoManual || [])" :key="'pg-' + idx">
                      <td class="estilo-celula texto-centro">
                        <div class="input-table input-table--date">
                          <DatePickerMask :append-to-body="true" :editable="true" lang="pt-br" v-model="pg.data"
                            :not-before="dataMinima" format="dd/MM/yyyy" type="date" :open.sync="open" class="date-compact"/>
                        </div>
                      </td>
                      <td class="estilo-celula texto-centro">
                        <div class="input-table input-table--money">
                          <Money v-model="pg.valor" v-bind="number" class="input-compact"/>
                        </div>
                      </td>
                      <td class="estilo-celula texto-centro">
                        <button class="btn btn-outline-danger btn-sm" @click="removePagamentoManualSelecionado(idx)">
                          <i class="fa fa-trash"></i>
                        </button>
                      </td>
                    </tr>
                    <tr v-if="!fornecedorSelecionado.pagamentoManual || fornecedorSelecionado.pagamentoManual.length === 0">
                      <td class="estilo-celula texto-centro" colspan="3">Nenhum pagamento adicionado.</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            <!-- ===== /COTAÇÃO MANUAL ===== -->
          </div>
        </div>
      </section>

      <section class="materials empty" v-else>
        <div class="placeholder">Selecione um fornecedor para iniciar a cotação.</div>
      </section>
    </div>

    <!-- ==================== MODAIS (mantidos do seu fluxo) ==================== -->
    <!-- =================== MODAL COMENTÁRIOS =================== -->
    <b-modal :no-close-on-backdrop="true" id="modalComentarios" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalComentarios_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Comunicação</span>
          <h3 class="afm-hero__title">Comentários</h3>
          <p class="afm-hero__description">Histórico de comentários e observações vinculadas a esta cotação.</p>
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
            <div>
              <strong>Nenhum comentário encontrado</strong>
              <p>Seja o primeiro a comentar utilizando o campo abaixo.</p>
            </div>
          </div>
        </section>

        <section class="afm-section-card">
          <textarea-autosize v-model="modalComentarios_Comentario"
            class="form-control"
            :min-height="120"
            style="color: white; background-color: #1a1a1a; width: 100%; display: block; box-sizing: border-box;" />
        </section>

      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary">
            <span>Total:</span>
            <strong>{{ modalComentarios_Comentarios.length }} comentário(s)</strong>
          </div>
          <div class="afm-footer__actions">
            <b-button @click="modalComentarios_Exibir = false" variant="dark" class="mb-0 mr-2">Fechar</b-button>
            <button type="button" @click="enviarComentario();" class="btn btn-success mb-0">
              <i class="fa fa-paper-plane mr-1"></i>Adicionar comentário
            </button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalNovoFornecedor" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal" footer-bg-variant="bodyModal"
      v-model="modalNovoFornecedor_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Fornecedor</span>
          <h3 class="afm-hero__title">Adicionar Fornecedor</h3>
          <p class="afm-hero__description">Selecione um ou mais fornecedores para a cotação.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Fornecedores</label>
            <multiselect v-model="modalNovoFornecedor_FornecedorSelecionado" :multiple="true"
              :options="fornecedoresGlobais" select-label="Selecionar" placeholder="Selecione um fornecedor"
              label="nomeFantasia" track-by="nomeFantasia" :custom-label="descricaoFornecedor"/>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalNovoFornecedor_Exibir = false" variant="dark" class="mb-0 mr-2">
              Fechar
            </b-button>
            <b-button variant="success" class="mb-0" @click="adicionarNovoFornecedor();">
              <i class="fa fa-plus mr-1"></i> Adicionar
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalDataEntregaFornecedor" class="modal-dialog modal-md afm-modal"
      body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalDataEntregaFornecedor_Exibir" size="lg">

      <div class="afm-hero">
        <div>
          <span class="afm-hero__eyebrow">Fornecedor</span>
          <h3 class="afm-hero__title">Adicionar Data de Entrega do Fornecedor</h3>
          <p class="afm-hero__description">Informe a data de entrega prometida pelo fornecedor.</p>
        </div>
      </div>

      <b-container fluid class="afm-sections">
        <section class="afm-section-card">
          <div class="afm-field-group">
            <label class="afm-label">Data de entrega</label>
            <DatePickerMask :append-to-body="true" :not-before="dataMinima" :editable="true" lang="pt-br"
              v-model="modalDataEntregaFornecedor_DataEntrega" format="dd/MM/yyyy" type="date" :open.sync="open"/>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary"></div>
          <div class="afm-footer__actions">
            <b-button @click="modalDataEntregaFornecedor_Exibir = false" variant="dark" class="mb-0 mr-2">
              Fechar
            </b-button>
            <b-button variant="success" class="mb-0"
              @click="definirDataEntregaFornecedor();"
              v-if="solicitacaoCompra && (solicitacaoCompra.idStatusSolicitacaoCompra == 2 || solicitacaoCompra.idStatusSolicitacaoCompra == 7)">
              <i class="fa fa-save mr-1"></i> Atualizar data entrega
            </b-button>
          </div>
        </div>
      </template>
    </b-modal>

    <b-modal :no-close-on-backdrop="true" id="modalHistoricoCompra" class="modal-dialog modal-md afm-modal" body-bg-variant="modal" header-bg-variant="bodyModal"
      footer-bg-variant="bodyModal" v-model="modalHistoricoCompra_Exibir" size="lg" hide-header>

      <div class="afm-hero" style="background: linear-gradient(135deg, rgba(255,214,57,0.10) 0%, rgba(255,255,255,0.03) 100%); border-radius: 10px; padding: 16px 20px; margin-bottom: 4px; display: flex; align-items: center; justify-content: space-between;">
        <div style="display:flex; align-items:center; gap: 14px;">
          <div style="background: rgba(255,214,57,0.15); border: 1px solid rgba(255,214,57,0.35); border-radius: 12px; width: 48px; height: 48px; display:flex; align-items:center; justify-content:center; flex-shrink:0;">
            <i class="fa fa-money" style="font-size: 20px; color: #FFD639;"></i>
          </div>
          <div>
            <span class="afm-hero__eyebrow">Material</span>
            <h3 class="afm-hero__title" style="margin: 2px 0 4px;">Histórico de Preços</h3>
            <p class="afm-hero__description" style="margin:0;">Preços praticados em compras anteriores para este item.</p>
          </div>
        </div>
        <div style="display:flex; flex-direction:column; align-items:center; background: rgba(255,255,255,0.06); border: 1px solid rgba(255,255,255,0.14); border-radius: 10px; padding: 10px 18px; min-width: 80px;">
          <span style="font-size: 26px; font-weight: 700; color: #FFD639; line-height:1;">{{ historicoDeCompra.length }}</span>
          <small style="color: #A9B1C3; font-size: 11px; text-transform: uppercase; letter-spacing: .5px;">registro(s)</small>
        </div>
      </div>

      <b-container fluid class="afm-sections" style="padding-top: 8px;">
        <section class="afm-section-card">

          <div v-if="historicoDeCompra.length > 0">
            <div class="table-card table-card--fluid">
              <table class="estilo-tabela tabela-identidade">
                <thead>
                  <tr>
                    <th class="estilo-cabecalho">Obra</th>
                    <th class="estilo-cabecalho">Cliente</th>
                    <th class="estilo-cabecalho texto-centro">Pedido</th>
                    <th class="estilo-cabecalho">Fornecedor</th>
                    <th class="estilo-cabecalho">Material</th>
                    <th class="estilo-cabecalho texto-centro">Data</th>
                    <th class="estilo-cabecalho texto-centro">Quantidade Pedida</th>
                    <th class="estilo-cabecalho texto-centro">Quantidade Conciliada</th>
                    <th class="estilo-cabecalho texto-centro">Valor</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, index) in historicoDeCompra" :key="'hc-' + (row.id || index)">
                    <td class="estilo-celula">{{ row.obra }}</td>
                    <td class="estilo-celula">{{ row.cliente }}</td>
                    <td class="estilo-celula texto-centro">{{ row.pedido }}</td>
                    <td class="estilo-celula">{{ row.fornecedor }}</td>
                    <td class="estilo-celula">{{ row.material }}</td>
                    <td class="estilo-celula texto-centro">{{ formataData(row.data) }}</td>
                    <td class="estilo-celula texto-centro">{{ row.quantidadePedida }}</td>
                    <td class="estilo-celula texto-centro">{{ row.quantidadeConciliada }}</td>
                    <td class="estilo-celula texto-centro">{{ new Intl.NumberFormat('pt-BR',{ style:'currency', currency:'BRL' }).format(row.valor) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>

          <div v-else class="afm-empty-state">
            <i class="fa fa-inbox" style="font-size: 28px; color: #A9B1C3;"></i>
            <div>
              <strong>Sem registros</strong>
              <p>Este material ainda não possui compras anteriores.</p>
            </div>
          </div>
        </section>
      </b-container>

      <template #modal-footer>
        <div class="afm-footer">
          <div class="afm-footer__summary">
            <span>Total:</span>
            <strong>{{ historicoDeCompra.length }} registro(s)</strong>
          </div>
          <div class="afm-footer__actions">
            <b-button @click="modalHistoricoCompra_Exibir = false" variant="dark" class="mb-0">Fechar</b-button>
          </div>
        </div>
      </template>
    </b-modal>
  </div>
</template>

<script>
import Vue from 'vue'
import moment from 'moment'
import VueSweetalert2 from 'vue-sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'
import ToggleButton from 'vue-js-toggle-button'
import ApiService from '@/services/api.service.js'
import Multiselect from 'vue-multiselect'
import CurrencyInput from '../../../components/CurrencyInput.vue'
import { Money } from 'v-money'
import Loading from 'vue-loading-overlay'
import 'vue-loading-overlay/dist/vue-loading.css'
import DatePickerMask from 'vue2-datepicker-mask'
import 'vue2-datepicker/index.css'
import 'vue2-datepicker/locale/pt-br'
import VueHorizontal from 'vue-horizontal'
Vue.use(VueSweetalert2)
Vue.use(ToggleButton)

export default {
  name: 'Cotacao',
  components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask, VueHorizontal },
  data() {
    return {
      money: { decimal: ',', thousands: '.', prefix: 'R$ ', precision: 2, masked: false },
      number: { decimal: ',', thousands: '.', prefix: '', precision: 2, masked: false },
      decimalInput: { decimal: ',', thousands: '.', prefix: '', precision: 2, masked: false },
      dataMinima: (new Date()).setDate(new Date().getDate() - 1),
      isLoading: false,
      open: false,
      valorTemporario: 0,
      usuarioDTO: null,
      modalHistoricoCompra_IdMaterial: 0,
      modalHistoricoCompra_Exibir: false,
      historicoDeCompra: [],
      fields: [
        { key: 'obra', label: 'Obra' },
        { key: 'cliente', label: 'Cliente' },
        { key: 'pedido', label: 'Pedido' },
        { key: 'fornecedor', label: 'Fornecedor' },
        { key: 'material', label: 'Material' },
        { key: 'dataConciliacao', label: 'Data' },
        { key: 'quantidadePedida', label: 'Quantidade Pedida' },
        { key: 'quantidadeConciliada', label: 'Quantidade Conciliada' },
        { key: 'valor', label: 'Valor' }
      ],
      options: { perPage: 10 },
      solicitacaoCompra: null,
      solicitacaoCompraParaCotacao: null,
      fornecedores: [],
      fornecedoresGlobais: [],
      condicoesDePagamento: [],
      modalNovoFornecedor_Exibir: false,
      modalNovoFornecedor_FornecedorSelecionado: [],
      modalDataEntregaFornecedor_Exibir: false,
      modalDataEntregaFornecedor_DataEntrega: null,
      modalDataEntregaFornecedor_IdFornecedor: 0,
      modalComentarios_Exibir: false,
      modalComentarios_Titulo: 'Comentários',
      modalComentarios_Comentarios: [],
      modalComentarios_Controller: '',
      modalComentarios_IdSolicitacaoOuPedidoCompra: 0,
      modalComentarios_Comentario: '',
      solicitacaoCompra_DataPagamentoServico: null,
      solicitacaoCompra_ValorPagamentoServico: 0.0,
      solicitacaoCompra_PagamentosServico: [],
      colunasTabelaComentarios: ['observacao', 'usuario', 'data'],
      opcoesTabelaComentarios: {
        perPage: 1000,
        headings: { observacao: 'Comentário', usuario: 'Usuário', data: 'Data' },
        clientSorting: true,
        sortable: [],
        pagination: { chunk: 2, dropdown: false },
        filterable: false,
        texts: {
          filterPlaceholder: 'Procurar por',
          count: 'Exibindo {from} de {to} de {count} itens|{count} itens|Um item',
          first: 'Primeiro', last: 'último', filter: '', limit: 'Itens:', page: 'Página:',
          skin: 'table table-striped', noResults: 'Não encontrado'
        }
      },
      fornecedorSelecionadoId: null,
      headerExpanded: true,
    }
  },
  computed: {
    fornecedorSelecionado() {
      const lista = (this.solicitacaoCompraParaCotacao && this.solicitacaoCompraParaCotacao.fornecedores) || []
      return lista.find(f => f.id === this.fornecedorSelecionadoId) || null
    },
    totalManualSelecionado() {
      const arr = (this.fornecedorSelecionado && this.fornecedorSelecionado.pagamentoManual) || []
      return arr.reduce((acc, p) => acc + (Number(p.valor) || 0), 0)
    },
    isManualMismatch() {
      if (!(this.fornecedorSelecionado && this.fornecedorSelecionado.condicaoPagamento && this.fornecedorSelecionado.condicaoPagamento.descricao === 'Manual')) return false
      const total = this.totalFornecedor(this.fornecedorSelecionado.id)
      const sum = this.totalManualSelecionado
      return Math.abs(Number(total) - Number(sum)) > 0.01 // tolerância de 1 centavo
    }
  },
  methods: {
    // ===== Helpers de UI =====
    selecionarFornecedor(id) { this.fornecedorSelecionadoId = id },
    customLabelCondicaoPagamento(option) { return option ? option.descricao : 'Selecione uma condição' },
    formataData(data) { return moment(String(new Date(data))).format('DD/MM/YYYY HH:mm:ss') },
    formataDataSemHora(data) { return moment(String(new Date(data))).format('DD/MM/YYYY') },
    descricaoFornecedor({ nomeFantasia, cnpj, endereco }) { return `${nomeFantasia} - ${cnpj} - ${endereco == null ? '' : endereco}` },

    // ===== Cotações por material =====
    getCotacao(material, idFornecedor) {
      if (!material.cotacoes) Vue.set(material, 'cotacoes', [])
      let c = material.cotacoes.find(x => x.idFornecedor == idFornecedor)
      if (!c) {
        c = { idFornecedor, valorUnitarioCotado: 0, quantidadeCotado: material.quantidade || 0, dataEntrega: null, cotacaoFinal: false, cotacaoMaisBarata: false }
        material.cotacoes.push(c)
      }
      return c
    },
    getCotacaoComDesconto(material, idFornecedor) {
      if (!material.cotacoes) Vue.set(material, 'cotacoes', [])
      let c = material.cotacoes.find(x => x.idFornecedor == idFornecedor)
  
      var valorComDesconto = parseFloat(c.valorUnitarioCotado)  * (1.0 - ((parseFloat(this.fornecedorSelecionado.desconto) / 100.00)));
      c.valorComDesconto = valorComDesconto;

      if (!c) {
        c = { idFornecedor, valorUnitarioCotado: 0, quantidadeCotado: material.quantidade || 0, dataEntrega: null, cotacaoFinal: false, cotacaoMaisBarata: false, valorComDesconto: valorComDesconto }
        material.cotacoes.push(c)
      }
      return c
    },
    subtotalItem(material, idFornecedor) {
      const c = this.getCotacao(material, idFornecedor)
      const q = Number(c.quantidadeCotado || material.quantidade || 0)
      const vu = Number(c.valorUnitarioCotado || 0)
      return q * vu
    },
    somaMateriaisFornecedor(idFornecedor) {
      const mats = (this.solicitacaoCompraParaCotacao && this.solicitacaoCompraParaCotacao.materiais) || []
      return mats.reduce((acc, m) => acc + this.subtotalItem(m, idFornecedor), 0)
    },
    totalFornecedor(idFornecedor) {
      const soma = this.somaMateriaisFornecedor(idFornecedor)
      const f = ((this.solicitacaoCompraParaCotacao && this.solicitacaoCompraParaCotacao.fornecedores) || []).find(x => x.id == idFornecedor) || { frete: 0, imposto: 0, desconto: 0 }
      const descontoFactor = 1 - (Number(f.desconto || 0) / 100)
      return (soma * descontoFactor) + Number(f.frete || 0) + Number(f.imposto || 0)
    },

    // ===== Cotação Manual (editar parcelas) =====
    adicionaPagamentoManualSelecionado() {
      if (!this.fornecedorSelecionado) return
      if (!this.fornecedorSelecionado.pagamentoManual) Vue.set(this.fornecedorSelecionado, 'pagamentoManual', [])
      this.fornecedorSelecionado.pagamentoManual.push({ data: new Date(), valor: 0.0 })
    },
    removePagamentoManualSelecionado(idx) {
      if (!this.fornecedorSelecionado || !this.fornecedorSelecionado.pagamentoManual) return
      this.fornecedorSelecionado.pagamentoManual.splice(idx, 1)
    },

    excluirMaterial(material, idSolicitacaoCompra)
    {
      this.$swal({ title:'Atenção', text:'Deseja excluir o material ' + material.descricao + ' da cotação?', icon:'warning', showCancelButton:true, confirmButtonColor:'#3085d6', cancelButtonColor:'#d33', confirmButtonText:'Sim, excluir !', cancelButtonText:'Não' }).then((result)=>{
        if (result.isConfirmed) {
          this.isLoading = true
          ApiService.excluiMaterialCotacao(material.idMaterial, idSolicitacaoCompra, (result) => {
            this.isLoading = false
            if (result.status != 200) { this.$swal('', result.data, 'error') } else { this.$swal('Sucesso', 'Material excluido !!!', 'success'); window.location.reload(); }
          })
        }
      })

    },

    // ===== Modais/Fluxos existentes (mantidos) =====
    abrirModalHistoricoDePreco(idMaterial){
      this.isLoading = true
      ApiService.obtemHistoricoDeCompra(idMaterial, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.historicoDeCompra = result.data; this.modalHistoricoCompra_Exibir = true }
      })
    },
    enviarComentario() {
      this.isLoading = true
      ApiService.enviarComentario(this.modalComentarios_Controller, this.modalComentarios_IdSolicitacaoOuPedidoCompra, this.modalComentarios_Comentario, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.data, 'error') } else { this.modalComentarios_Comentario = ''; this.obtemSolicitacao(this.solicitacaoCompra.id); this.$swal('Comentário enviado com sucesso', result.message, 'success') }
      })
    },
    removerItensSelecionados() {
      this.$swal({ title:'Atenção', text:'Qual o tipo de aprovação para a nova solicitação? ', icon:'warning', showCancelButton:true, confirmButtonColor:'#3085d6', cancelButtonColor:'#d33', confirmButtonText:'Automática', cancelButtonText:'Normal' }).then((result)=>{
        this.isLoading = true
        this.solicitacaoCompraParaCotacao.aprovacaoAutomatica = result.isConfirmed
        ApiService.removerItensSelecionados(this.solicitacaoCompraParaCotacao, (result) => {
          this.isLoading = false
          if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$swal('', "Uma nova solicitação de compra com o código '" + result.message.codigo + "' foi criada com os itens selecionados.", 'success').then(()=>{ this.$router.go() }) }
        })
      })
    },
    duplicarItensSelecionados() {
      this.$swal({ title:'Atenção', text:'Qual o tipo de aprovação para a nova solicitação? ', icon:'warning', showCancelButton:true, confirmButtonColor:'#3085d6', cancelButtonColor:'#d33', confirmButtonText:'Automática', cancelButtonText:'Normal' }).then((result)=>{
        this.isLoading = true
        this.solicitacaoCompraParaCotacao.aprovacaoAutomatica = result.isConfirmed
        ApiService.duplicarItensSelecionados(this.solicitacaoCompraParaCotacao, (result) => {
          this.isLoading = false
          if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$swal('', "Uma nova solicitação de compra com o código '" + result.message.codigo + "' foi criada com os itens selecionados.", 'success').then(()=>{ this.$router.go() }) }
        })
      })
    },
    defineCotacaoPrincipalMaterial(idMaterialCotacao) {
      this.isLoading = true
      ApiService.definirCotacaoPrincipal('SolicitacaoCompra/Cotacao/Principal', idMaterialCotacao, (result) => {
        if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$router.go() }
      })
    },
    finalizarCotacao() {
      this.$swal({ title:'Atenção', text:'Deseja finalizar a cotação da solicitação ' + this.solicitacaoCompra.codigo + ' ?', icon:'warning', showCancelButton:true, confirmButtonColor:'#3085d6', cancelButtonColor:'#d33', confirmButtonText:'Sim, finalizar !', cancelButtonText:'Não' }).then((result)=>{
        if (result.isConfirmed) {
          this.isLoading = true
          ApiService.finalizarCotacao('SolicitacaoCompra/Cotacao/Finalizar/', this.solicitacaoCompra.id, (result) => {
            this.isLoading = false
            if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$swal('Sucesso', 'Cotação Aprovada !!!', 'success'); let routeData = this.$router.resolve({ name: 'OrdemCompra' }); window.open(routeData.href, '_self') }
          })
        }
      })
    },
    editarCotacao() {
      this.solicitacaoCompraParaCotacao.observacaoDeAprovacao = ''
      this.isLoading = true
      ApiService.editarCotacao(this.solicitacaoCompraParaCotacao, (result) => {
        if (result.status != 200) { this.isLoading = false; this.$swal('', result.data, 'error') } else { this.$router.go() }
      })
    },
    enviarDiretoria() {
      this.isLoading = true
      var objeto = { idSolicitacaoCompra: this.solicitacaoCompraParaCotacao.id, observacaoDeAprovacao: '' }
      ApiService.enviarDiretoria(objeto, (result) => {
        if (result.status != 200) { this.isLoading = false; this.$swal('', result.data, 'error') } else { let routeData = this.$router.resolve({ name: 'OrdemCompra' }); window.open(routeData.href, '_self') }
      })
    },
    devolverCompras() {
      this.isLoading = true
      var objeto = { idSolicitacaoCompra: this.solicitacaoCompraParaCotacao.id, observacaoDeAprovacao: '' }
      ApiService.devolverCompras(objeto, (result) => {
        if (result.status != 200) { this.isLoading = false; this.$swal('', result.data, 'error') } else { let routeData = this.$router.resolve({ name: 'Dashboard' }); window.open(routeData.href, '_self') }
      })
    },
    refazerOrcamento() {
      this.isLoading = true
      ApiService.enviarParaRevisao(this.solicitacaoCompraParaCotacao.id, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.data, 'error') } else { let routeData = this.$router.resolve({ name: 'Dashboard' }); window.open(routeData.href, '_self') }
      })
    },
    definirDataEntregaFornecedor() {
      this.isLoading = true
      ApiService.definirDataEntregaFornecedor(this.solicitacaoCompraParaCotacao.id, this.modalDataEntregaFornecedor_IdFornecedor, moment(String(new Date(this.modalDataEntregaFornecedor_DataEntrega))).format('MM-DD-YYYY'), (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$router.go() }
      })
    },
    defineFornecedor(idFornecedor) {
      this.editarCotacao()
      this.isLoading = true
      ApiService.definirFornecedorPrincipal(this.solicitacaoCompraParaCotacao.id, idFornecedor, (result) => {
        if (result.status != 200) { this.$swal('', result.data, 'error') } else { this.$router.go() }
      })
    },
    removeFornecedor(idFornecedor) {
      this.$swal({ title:'Atenção', text:'Deseja remover este fornecedor e todas as suas cotações? Esse processo é irreversível.', icon:'warning', showCancelButton:true, confirmButtonColor:'#3085d6', cancelButtonColor:'#d33', confirmButtonText:'Sim, remover !', cancelButtonText:'Não' }).then((result)=>{
        if (result.isConfirmed) {
          this.isLoading = true
          ApiService.removeFornecedor(this.solicitacaoCompraParaCotacao.id, idFornecedor, (result) => {
            if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.$router.go() }
          })
        }
      })
    },
    adicionarNovoFornecedor() {
      if (!this.modalNovoFornecedor_FornecedorSelecionado || this.modalNovoFornecedor_FornecedorSelecionado.length == 0) {
        this.$swal('Atenção', 'Selecione um ou mais fornecedores', 'error'); return
      }
      var objeto = { idFornecedores: this.modalNovoFornecedor_FornecedorSelecionado.map(item => item.id), idSolicitacaoCompra: this.solicitacaoCompra.id }
      this.isLoading = true
      ApiService.cadastraNovoFornecedorCotacao(objeto, (result) => {
        this.isLoading = false;
        if (result.status != 200) { this.$swal('', result.data, 'error') } else { this.$router.go() }
      })
    },
    obtemSolicitacao(idSolicitacaoCompra) {
      this.isLoading = true; this.modalCotacao_Cotacoes = []
      ApiService.get('SolicitacaoCompra', idSolicitacaoCompra, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.message, 'error') } else {
          this.solicitacaoCompra = result.data
          this.modalComentarios_Comentarios = this.solicitacaoCompra.comentarios
          this.fornecedores = []
          this.solicitacaoCompra.materiais.forEach((material) => {
            material.cotacoes.forEach((cotacao) => {
              var fornecedorEncontrado = this.fornecedores.filter(function (x) { return x.id == cotacao.idFornecedor })
              if (fornecedorEncontrado.length == 0) { cotacao.fornecedor.valorTotal = 0; cotacao.fornecedor.principal = false; this.fornecedores.push(cotacao.fornecedor) }
            })
          })
          if (this.solicitacaoCompra.centroCustoObra != null && this.solicitacaoCompra.centroCustoObra.bloqueada == true)
            this.$swal('Atenção', 'Esta obra está bloqueada', 'error')
          if (this.solicitacaoCompra.centroCustoObra.valorCustoGasto != null && this.solicitacaoCompra.centroCustoObra.valorCustoGasto > this.solicitacaoCompra.centroCustoObra.valorCustoAjustado)
            this.$swal('Atenção', 'Esta obra está com ETO negativo', 'error')
        }
      })
    },
    obtemSolicitacaoParaCotacao(idSolicitacaoCompra) {
      ApiService.getSolicitacaoCompraParaCotacao(idSolicitacaoCompra, (result) => {
        if (result.status != 200) { this.$swal('', result.message, 'error') } else {
          this.solicitacaoCompraParaCotacao = result.data
          const fs = (this.solicitacaoCompraParaCotacao && this.solicitacaoCompraParaCotacao.fornecedores) || []
          this.fornecedorSelecionadoId = fs.length ? fs[0].id : null
        }
      })
    },
    downloadExcelCotacoes(id, codigo) {
      this.isLoading = true
      ApiService.downloadExcelCotacoes(id, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', 'Erro ao baixar excel', 'error') } else {
          const blob = new Blob([result.data], { type: result.contentType })
          const fileURL = URL.createObjectURL(blob)
          window.open(fileURL, '_blank')
        }
      })
    },
    listaFornecedores() {
      this.isLoading = true; this.fornecedoresGlobais = []
      ApiService.getAll('Fornecedor', true, (result) => {
        this.isLoading = false
        if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.fornecedoresGlobais = result.data }
      })
    },
    listaCondicoesDePagamento() {
      this.condicoesDePagamento = []
      ApiService.getAll('CondicaoPagamento', true, (result) => {
        if (result.status != 200) { this.$swal('', result.message, 'error') } else { this.condicoesDePagamento = result.data }
      })
    },
    adicionaPagamentoServico(index) {
      this.solicitacaoCompraParaCotacao.fornecedores[index].pagamentoManual.push({ data: new Date(), valor: 0.0 })
    },
    removePagamentoSolicitacaoServico(index, index2) {
      this.solicitacaoCompraParaCotacao.fornecedores[index].pagamentoManual.splice(index2, 1)
    }
  },
  mounted() {
    this.listaFornecedores()
    this.listaCondicoesDePagamento()
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO'))
  },
  created() {
    this.obtemSolicitacao(this.$route.params.id)
    this.obtemSolicitacaoParaCotacao(this.$route.params.id)
  },
  updated() {
    for (var i = 0; i < this.fornecedores.length; i++) { this.fornecedores[i].valorTotal = 0 }
  }
}
</script>

<style src="./Cotacao.scss" lang="scss" />

<style scoped>
/* ====== Tema para fundo radial #17193B ====== */
.cotacao-page { margin: 16px; color: #E6EAF2; }
.page-title { margin-bottom: 8px; color: #FFD639; }

.header-card { position: relative; border: 1px solid rgba(255,255,255,0.14); border-radius: 12px; padding: 12px; margin-bottom: 12px; background: rgba(255,255,255,0.06); }
.top-summary{display:flex;align-items:flex-start;justify-content:space-between;gap:12px;margin-bottom:8px;position:relative}
.summary-grid{display:grid;grid-template-columns:repeat(5,minmax(120px,1fr));gap:8px;flex:1}
.kv{display:flex;flex-direction:column;gap:2px;padding:8px;border:1px solid rgba(255,255,255,0.14);border-radius:10px;background:rgba(255,255,255,0.06);}
.kv .k{font-size:12px;color:#A9B1C3}
.kv .v{font-weight:700;}
.summary-actions{display:flex;align-items:center}
.details-toggle{display:inline-flex;align-items:center;gap:6px;font-size:12px;color:#A9B1C3;background:rgba(255,255,255,0.06);border:1px solid rgba(255,255,255,0.14);border-radius:999px;padding:6px 10px;cursor:pointer}
.details-toggle:hover{color:#E6EAF2;background:rgba(255,255,255,0.10)}
.details-toggle i{font-size:12px}

.details-grid{display:grid;grid-template-columns:repeat(3,minmax(260px,1fr));gap:12px;margin-top:8px}
.mini-card{border:1px solid rgba(255,255,255,0.14);border-radius:10px;background:rgba(255,255,255,0.06);padding:10px}
.mini-card__header{font-weight:700;margin-bottom:6px;color:#E6EAF2}
.muted{color:#A9B1C3}
::v-deep textarea { width: 100%; max-width: 100%; color: #FFFFFF; }
::v-deep .afm-section-card textarea { width: 100%; max-width: 100%; color: #FFFFFF; }

/* Layout principal */
.layout { display: grid; grid-template-columns: 300px 1fr; gap: 12px; min-height: 480px; }
.suppliers { border: 1px solid rgba(255,255,255,0.14); border-radius: 12px; padding: 10px; background: rgba(255,255,255,0.06); }
.suppliers-header { display:flex; justify-content:space-between; align-items:center; margin-bottom: 8px; }
.suppliers .title { font-weight: 700; }
.supplier-list { list-style: none; padding: 0; margin: 0; }
.supplier-list li { padding: 8px; border: 1px solid rgba(255,255,255,0.14); border-radius: 10px; margin-bottom: 8px; cursor: pointer; background: rgba(255,255,255,0.02); }
.supplier-list li.active { border-color: #FFD639; background: rgba(255,214,57,0.08); }
.supplier-list .name { font-weight: 600; }
.supplier-list .cnpj { color: #A9B1C3; font-size: 12px; }
.supplier-list .cond-pag { color:#E6EAF2; font-size: 12px; margin-top:2px; }
.supplier-list .total { text-align: right; font-weight: 700; margin-top:4px; }

.materials { border: 1px solid rgba(255,255,255,0.14); border-radius: 12px; padding: 10px; background: rgba(255,255,255,0.06); }
.materials-header { display:flex; align-items:center; justify-content:space-between; margin-bottom: 8px; }
.materials-header h3 { margin: 0; }
.materials-header small { color: #A9B1C3; }

/* GRID interno do painel do fornecedor */
.materials-grid{display:grid;grid-template-columns: 1fr; gap: 12px; align-items:start}
.items-table{min-width:0}
.side-panel{min-width:0; width:50%; justify-self:start;}

/* Tabela compacta */
.table { width: 100%; border-collapse: collapse; border-top: 1px solid rgba(255,255,255,0.14); }
.table th, .table td { border: 1px solid rgba(255,255,255,0.14); padding: 6px; font-size: 13px; }
.table th { background: rgba(255,255,255,0.08); text-align: left; }
.table .num { text-align: right; }
.table .cen { text-align: center; }
.table.compact th, .table.compact td{ padding:6px }
.input-compact{ max-width: 110px !important; background: #111726; border: 1px solid rgba(255,255,255,0.12); border-radius: 6px; height: 30px; padding: 2px 6px; }
.date-compact{ max-width: 120px; }

/* Caixa de pagamento/totais compacta */
.payment-box { border: 1px solid rgba(255,255,255,0.14); border-radius: 12px; padding: 10px; background: rgba(255,255,255,0.06); }
.row-2 { display: grid; grid-template-columns: repeat(2, minmax(120px, 1fr)); gap: 10px; }
.totais { margin-top: 10px; }
.total-line { display:flex; justify-content: space-between; margin-top: 4px; }
.total-line.emph strong { color: #FFD639; }
.total-line.manual-total strong{ color:#60A5FA }
.mismatch strong, .text-error { color:#F87171 !important; }
.text-ok { color:#10B981 }

/* Cotação manual */
.manual-box { margin-top: 12px; border: 1px dashed rgba(255,255,255,0.20); border-radius: 10px; padding: 10px; background: rgba(255,255,255,0.04); }
.manual-header { display:flex; justify-content:space-between; align-items:center; margin-bottom:8px; }
.manual-header .title { font-weight:700; }
.manual-table { width:100%; border-collapse: collapse; }
.manual-table th, .manual-table td { border: 1px solid rgba(255,255,255,0.14); padding: 6px; font-size: 13px; }
.manual-table th { background: rgba(255,255,255,0.08); }
.manual-table .num { text-align: right; }
.manual-table .cen { text-align: center; }

/* Utilitários de botão */
.btn { color: #E6EAF2; border: 1px solid rgba(255,255,255,0.14); border-radius: 8px; background: rgba(255,255,255,0.06); transition: all .15s ease; }
.btn:hover { background: rgba(255,255,255,0.10); }
.btn.btn-sm { padding: 4px 8px; }
.bg-success { background: rgba(16,185,129,0.18); border-color: rgba(16,185,129,0.35); }
.bg-warning { background: rgba(245,158,11,0.18); border-color: rgba(245,158,11,0.35); }
.bg-danger { background: rgba(239,68,68,0.18); border-color: rgba(239,68,68,0.35); }
.btn-outline-danger { background: transparent; color: #F87171; border-color: #F87171; }
.btn-outline-danger:hover { background: rgba(248,113,113,0.12); }

/* Animations */
.fade-enter-active,.fade-leave-active{transition:opacity .15s}
.fade-enter,.fade-leave-to{opacity:0}

/* Badge de comentários */
.comentarios-btn-wrapper {
  position: relative;
  display: inline-block;
}
.comentarios-badge {
  position: absolute;
  top: -4px;
  right: -4px;
  background: #e53e3e;
  color: #fff;
  font-size: 11px;
  font-weight: 700;
  min-width: 18px;
  height: 18px;
  border-radius: 999px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 4px;
  pointer-events: none;
  line-height: 1;
}

/* Responsividade */
@media (max-width:1200px){
  .summary-grid{grid-template-columns:repeat(3,minmax(120px,1fr))}
  .details-grid{grid-template-columns:repeat(2,minmax(240px,1fr))}
  .materials-grid{grid-template-columns:1fr}
}
@media (max-width:768px){
  .summary-grid{grid-template-columns:repeat(2,minmax(120px,1fr))}
  .details-grid{grid-template-columns:1fr}
}
</style>
