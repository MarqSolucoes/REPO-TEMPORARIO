<template>
    <div class="conciliacao-page">
        <loading :active.sync="isLoading" :can-cancel="false" :is-full-page="true" color="#ffd639"></loading>

        <!-- Input único e compartilhado para troca de arquivo de NF (fora de qualquer v-for) -->
        <input ref="novoArquivoPdfNF" type="file" accept="application/pdf"
            @change="alteraArquivoNF" style="display:none;" />

        <!-- ===== Modal: Cancelamento de Saldo (MELHORADO) ===== -->
        <b-modal :no-close-on-backdrop="true" id="modalCancelamentoSaldo" class="modal-dialog modal-md"
            v-bind:title="modalCancelamentoSaldo_Titulo" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" v-model="modalCancelamentoSaldo_Exibir">
            <b-container>
                <!-- Resumo do pedido / saldos -->
                <section class="header-card mb-3">
                    <div class="summary-grid">
                        <div class="kv">
                            <span class="k">Pedido</span>
                            <span class="v">{{ pedidoParaConciliacaoNF ? pedidoParaConciliacaoNF.codigo : '—' }}</span>
                        </div>
                        <div class="kv">
                            <span class="k">Saldo disponível</span>
                            <span class="v">{{
                                new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
                                    .format(modalCancelamentoSaldo_SaldoDisponivel)
                            }}</span>
                        </div>
                        <div class="kv">
                            <span class="k">Saldo já cancelado</span>
                            <span class="v">{{
                                new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
                                    .format(pedidoParaConciliacaoNF ? (pedidoParaConciliacaoNF.saldoCancelado || 0) : 0)
                            }}</span>
                        </div>
                    </div>
                </section>

                <!-- Formulário -->
                <div class="modal-form">
                    <b-row>
                        <b-col>
                            <label class="lbl">Motivo <span class="req">*</span></label>
                            <multiselect v-model="modalCancelamentoSaldo_MotivoSelecionado" :multiple="false"
                                :options="motivosCancelamentoSaldo" select-label="Selecionar"
                                placeholder="Selecione um motivo" label="descricao" track-by="descricao"
                                @keydown.enter.prevent="onEnterCancelamento" />
                            <small v-if="!modalCancelamentoSaldo_MotivoSelecionado" class="helper error">
                                Selecione um motivo para continuar.
                            </small>
                        </b-col>
                    </b-row>

                    <br />

                    <b-row>
                        <b-col>
                            <label class="lbl">Observação</label>
                            <textarea-autosize id="textarea" v-model="modalCancelamentoSaldo_Observacao"
                                class="form-control" :min-height="110" style="color: white"
                                placeholder="Descreva, se necessário, o motivo do cancelamento..."
                                @keydown.enter.exact.prevent="onEnterCancelamento" />
                            <small class="helper">Opcional – ajude futuros aprovadores com um contexto curto.</small>
                        </b-col>
                    </b-row>

                    <br />

                    <b-row>
                        <b-col md="6">
                            <label class="lbl">Valor a cancelar <span class="req">*</span></label>
                            <Money class="kv-input" v-model="modalCancelamentoSaldo_Valor" v-bind="money" @input="noop"
                                @keydown.enter.prevent="onEnterCancelamento" />
                            <small v-if="modalCancelamentoSaldo_Valor <= 0" class="helper error">
                                Informe um valor superior a R$ 0,00.
                            </small>
                            <small v-else-if="modalCancelamentoSaldo_SaldoRestante < 0" class="helper error">
                                O valor informado supera o saldo disponível.
                            </small>
                        </b-col>
                        <b-col md="6">
                            <label class="lbl">Saldo restante após cancelamento</label>
                            <div class="tile-readonly"
                                :class="{ 'negativo': modalCancelamentoSaldo_SaldoRestante < 0 }">
                                {{
                                    new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
                                        .format(Math.max(0, modalCancelamentoSaldo_SaldoRestante))
                                }}
                            </div>
                            <small v-if="modalCancelamentoSaldo_SaldoRestante < 0" class="helper error">
                                Revise o valor a cancelar.
                            </small>
                        </b-col>
                    </b-row>
                </div>
            </b-container>

            <template #modal-footer>
                <b-row>
                    <b-col cols="auto" style="padding: 0">
                        <b-button @click="modalCancelamentoSaldo_Exibir = false" variant="dark"
                            class="width-100 mb-3 mr-3">
                            Fechar
                        </b-button>
                        <b-button variant="danger" class="width-230 mb-3 mr-3"
                            :disabled="!modalCancelamentoSaldo_FormValido" @click="cancelarSaldo()">
                            <span>Cancelar Saldo</span>
                        </b-button>
                    </b-col>
                </b-row>
            </template>
        </b-modal>

        <!-- ===== Modal: Conciliação ===== -->
        <b-modal :no-close-on-backdrop="true" id="modalConciliacao" class="modal-dialog modal-md"
            v-bind:title="modalConciliacao_Titulo" body-bg-variant="modal" header-bg-variant="bodyModal"
            footer-bg-variant="bodyModal" size="xl" v-model="modalConciliacao_Exibir">
            <b-container>
                <!-- === HEADER TILES (Info iniciais) === -->
                <section class="header-card conc-header mb-3">
                    <div class="top-summary">
                        <div class="summary-grid">
                            <div class="kv">
                                <span class="k">Pedido Compra</span>
                                <span class="v">{{ modalConciliacao_PedidoCompra ? modalConciliacao_PedidoCompra.codigo
                                    : '—'
                                    }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">Data do Pedido</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.dataCadastro)
                                    ? formataDataSemHora(modalConciliacao_PedidoCompra.dataCadastro) : '—' }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">Data de Entrega</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.dataEntrega)
                                    ? formataDataSemHora(modalConciliacao_PedidoCompra.dataEntrega) : '—' }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">Centro de Custo</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.centroCustoObra) ?
                                    modalConciliacao_PedidoCompra.centroCustoObra.codigo : '—' }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">Fornecedor</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.fornecedor) ?
                                    modalConciliacao_PedidoCompra.fornecedor.nomeFantasia : '—' }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">Razão Social</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.fornecedor &&
                                    modalConciliacao_PedidoCompra.fornecedor.razaoSocial) ?
                                    modalConciliacao_PedidoCompra.fornecedor.razaoSocial : '—' }}</span>
                            </div>
                            <div class="kv">
                                <span class="k">CNPJ</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.fornecedor &&
                                    modalConciliacao_PedidoCompra.fornecedor.cnpj) ?
                                    modalConciliacao_PedidoCompra.fornecedor.cnpj : '—' }}</span>
                            </div>
                            <div class="kv kv--span2">
                                <span class="k">Endereço</span>
                                <span class="v">{{ (modalConciliacao_PedidoCompra &&
                                    modalConciliacao_PedidoCompra.enderecoEntrega) ?
                                    modalConciliacao_PedidoCompra.enderecoEntrega : '—' }}</span>
                            </div>
                        </div>
                    </div>
                </section>

                <!-- Seletor de fornecedor -->
                <b-row>
                    <b-col md="12">
                        <label>Fornecedor: </label>
                        <multiselect v-model="modalConciliacao_FornecedorSelecionado" :multiple="false"
                            :options="modalConciliacao_Filiais" select-label="Selecionar"
                            placeholder="Selecione um Fornecedor" :custom-label="descricaoFornecedor"
                            track-by="nomeFantasia">
                        </multiselect>
                    </b-col>
                </b-row>

                <br />

                <div>
                    <b-row>
                        <b-col>
                            <toggle-button v-model="modalConciliacao_TipoConciliacao"
                                :color="{ checked: '#2D8515', unchecked: '#4348F0', disabled: '#000000' }"
                                :labels="{ checked: 'Conciliação total', unchecked: 'Conciliação parcial' }"
                                :width="160" :height="25" :font-size="14" />
                        </b-col>
                    </b-row>

                    <br />

                    <b-row>
                        <b-col>
                            <label>Descrição: </label>
                            <textarea-autosize id="textarea" v-model="modalConciliacao_Descricao" class="form-control"
                                :min-height="135" style="color: white" />
                        </b-col>
                    </b-row>

                    <br />

                    <!-- ========== TABELA DE MATERIAIS ========== -->
                    <b-row>
                        <b-col>
                            <div class="table-card table-card--fluid table-card--fit">
                                <table class="estilo-tabela tabela-conciliacao" v-if="pedidoParaConciliacaoNF">
                                    <thead>
                                        <tr>
                                            <th class="estilo-cabecalho texto-centro col-sm">Item</th>
                                            <th class="estilo-cabecalho col-desc">Descrição</th>
                                            <th class="estilo-cabecalho texto-centro col-sm">Qtd</th>
                                            <th class="estilo-cabecalho texto-centro col-sm">Qtd Faltante</th>
                                            <th class="estilo-cabecalho texto-centro col-md">Qtd Conciliação</th>
                                            <th class="estilo-cabecalho texto-centro col-md">Valor Un. Comprado</th>
                                            <th class="estilo-cabecalho texto-centro col-md">Valor Un.</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="(material, index) in pedidoParaConciliacaoNF.materiais"
                                            :key="material.id">
                                            <td class="estilo-celula texto-centro">{{ index + 1 }}</td>
                                            <td class="estilo-celula wrap">{{ material.material }}</td>
                                            <td class="estilo-celula texto-centro">{{ material.quantidade }}</td>
                                            <td class="estilo-celula texto-centro">{{ (material.quantidade -
                                                material.quantidadeConciliada) }}</td>
                                            <td class="estilo-celula texto-centro">
                                                <input class="input-table"
                                                    v-model="pedidoParaConciliacaoNF.materiais[index].quantidadeParaConciliar"
                                                     />
                                            </td>
                                            <td class="estilo-celula texto-centro">
                                                {{ new Intl.NumberFormat('pt-BR', {
                                                    style: 'currency', currency: 'BRL'
                                                }).format(material.valorComprado) }}
                                            </td>
                                            <td class="estilo-celula texto-centro">
                                                <input class="input-table"
                                                    v-model="pedidoParaConciliacaoNF.materiais[index].valorConciliado"
                                                     />
                                            </td>
                                        </tr>
                                        <tr
                                            v-if="!pedidoParaConciliacaoNF || (pedidoParaConciliacaoNF && (!pedidoParaConciliacaoNF.materiais || pedidoParaConciliacaoNF.materiais.length === 0))">
                                            <td class="estilo-celula texto-centro" colspan="7">Nenhum registro
                                                encontrado</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </b-col>
                    </b-row>

                    <br />

                    <!-- ========== CAMPOS EDITÁVEIS EM TILES (NF / IMPOSTOS / FRETE) ========== -->
                    <b-row v-if="pedidoParaConciliacaoNF">
                        <b-col md="12">
                            <section class="header-card conc-edit mb-2">
                                <div class="summary-grid">
                                    <div class="kv">
                                        <span class="k">Número da nota fiscal</span>
                                        <span class="v">
                                            <b-form-input class="kv-input"
                                                v-model="pedidoParaConciliacaoNF.numeroNotaFiscal" />
                                        </span>
                                    </div>

                                    <div class="kv">
                                        <span class="k">Valor dos impostos</span>
                                        <span class="v">
                                            <Money class="kv-input" v-model="pedidoParaConciliacaoNF.valorImposto"
                                                v-bind="money"  />
                                        </span>
                                    </div>

                                    <div class="kv">
                                        <span class="k">Valor do frete</span>
                                        <span class="v">
                                            <Money class="kv-input" v-model="pedidoParaConciliacaoNF.valorFrete"
                                                v-bind="money"  />
                                        </span>
                                    </div>
                                </div>
                            </section>
                        </b-col>
                    </b-row>

                </div>

                <br />

                <!-- ===== Widget: Datas de Pagamento ===== -->
                <b-row>
                    <b-col md="12">
                        <Widget customHeader class="estiloWidget">
                            <section class="header-card conc-metrics mb-3">
                                <div class="summary-grid">
                                    <div class="kv">
                                        <span class="k">Condição de pagamento</span>
                                        <span class="v">{{ (pedidoParaConciliacaoNF != null ?
                                            pedidoParaConciliacaoNF.condicaoPagamento.descricao : '') }}</span>
                                    </div>
                                    <div class="kv">
                                        <span class="k">Valor da Nota</span>
                                        <span class="v">{{ Intl.NumberFormat('pt-BR', {
                                            style: 'currency', currency: 'BRL'
                                        }).format(calculaValorNota()) }}</span>
                                    </div>
                                    <div class="kv">
                                        <span class="k">Valor das parcelas</span>
                                        <span class="v">{{
                                            Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
                                                .format(somaValoresDatasPagamento())
                                        }}</span>
                                    </div>
                                </div>
                            </section>

                            <div class="pagto-wrapper">
                                <div class="table-card table-card--fluid">
                                    <table class="estilo-tabela tabela-identidade">
                                        <thead>
                                            <tr>
                                                <th class="estilo-cabecalho texto-centro">Data</th>
                                                <th class="estilo-cabecalho texto-centro">Valor</th>
                                                <th class="estilo-cabecalho texto-centro">Ações</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(row, index) in modalConciliacao_DatasDePagamento" :key="'dp-' + (row.id || index)">
                                                <td class="estilo-celula texto-centro">
                                                    <div class="col-cell">
                                                        <DatePickerMask :editable="true" lang="pt-br" :value="row.data"
                                                            @input="atualizaDataPagamento(row.id, $event)" :not-before="dataMinima"
                                                            format="dd/MM/yyyy" type="date" :append-to-body="true"
                                                            popup-class="conciliacao-datepicker-popup" :open.sync="open">
                                                        </DatePickerMask>
                                                    </div>
                                                </td>
                                                <td class="estilo-celula texto-centro">
                                                    <div class="col-cell">
                                                        <Money class="kv-input" :value="row.valor" v-bind="money"
                                                            @input="atualizaValorDataPagamento(row.id, $event)" />
                                                    </div>
                                                </td>
                                                <td class="estilo-celula texto-centro">
                                                    <div class="col-cell acoes-cell">
                                                        <button @click="excluiDataPagamento(row.id)" type="button"
                                                            class="btn btn-danger">
                                                            <i class="fa fa-close" title="Excluir data"></i>
                                                        </button>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr v-if="!modalConciliacao_DatasDePagamento || modalConciliacao_DatasDePagamento.length === 0">
                                                <td class="estilo-celula texto-centro" colspan="3">Nenhum registro encontrado</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="pagto-actions">
                                <span class="label-inline">Inserir nova data:</span>
                                <DatePickerMask :editable="true" lang="pt-br"
                                    v-model="modalConciliacao_NovaDataPagamento" :not-before="dataMinima"
                                    format="dd/MM/yyyy" type="date" :append-to-body="true"
                                    popup-class="conciliacao-datepicker-popup" :open.sync="open">
                                </DatePickerMask>
                                <b-button variant="success" class="width-230 mb-3 mr-3"
                                    @click="inserirNovaDataPagamento()">
                                    <span>Inserir</span>
                                </b-button>
                                <b-button variant="success" class="width-230 mb-3 mr-3" @click="calculaValorParcelas()">
                                    <span>Atualizar Valores</span>
                                </b-button>
                            </div>
                        </Widget>
                    </b-col>
                </b-row>

                <!-- ===== Widget: Saldos (VERTICAL) ===== -->
                <b-row>
                    <b-col md="12">
                        <Widget customHeader class="estiloWidget">
                            <h4>Saldos</h4>
                            <br />
                            <div class="saldos-vertical">
                                <div class="kv">
                                    <span class="k">Valor do Pedido</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.valorTotalPedido)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>

                                <div class="kv">
                                    <span class="k">Saldo Disponível</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.saldoDisponivel)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>

                                <div class="kv">
                                    <span class="k">NFs para Aprovação</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.valorPendenteAprovacao)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>

                                <div class="kv">
                                    <span class="k">NFs Aprovadas</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.valorGasto)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>

                                <div class="kv">
                                    <span class="k">NFs Aprovadas e não pagas</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.valorPrevistoGasto)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>

                                <div class="kv">
                                    <span class="k">Saldo Cancelado</span>
                                    <span class="v">
                                        {{ pedidoParaConciliacaoNF ? new
                                            Intl.NumberFormat('pt-BR', {
                                                style: 'currency', currency: 'BRL'
                                            }).format(pedidoParaConciliacaoNF.saldoCancelado)
                                            : 'R$ 0,00' }}
                                    </span>
                                </div>
                            </div>
                        </Widget>
                    </b-col>
                </b-row>

                <!-- ===== Widget: Notas fiscais ===== -->
                <b-row>
                    <b-col md="12">
                        <Widget customHeader class="estiloWidget">
                            <h4>Notas fiscais</h4>
                            <br />

                            <div class="table-card table-card--fluid">
                                <table class="estilo-tabela tabela-identidade">
                                    <thead>
                                        <tr>
                                            <th class="texto-centro">NF</th>
                                            <th>Fornecedor</th>
                                            <th class="texto-centro">Data vencimento</th>
                                            <th class="texto-centro">Valor</th>
                                            <th class="texto-centro">Status</th>
                                            <th class="texto-centro">Ações</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="nf in modalConciliacao_NotasFiscais"
                                            :key="'nf-' + (nf.id || nf.numero || Math.random())">
                                            <td class="texto-centro">{{ nf.numeroNotaFiscal || nf.numero || '—' }}</td>
                                            <td>{{ (nf.fornecedor) ?
                                                nf.fornecedor.nomeFantasia : '—' }}
                                            </td>
                                            <td class="texto-centro">{{ nf.dataVencimento ?
                                                formataDataSemHora(nf.dataVencimento) : '—' }}</td>
                                            <td class="texto-centro">{{ new
                                                Intl.NumberFormat('pt-BR', {
                                                    style: 'currency', currency: 'BRL'
                                                }).format(nf.valor
                                                    ||
                                                    0) }}</td>
                                            <td class="texto-centro">{{ nf.aprovada == null ? 'Pendente' : (nf.aprovada
                                                ?
                                                'Aprovada' : 'Reprovada') }}</td>
                                            <td class="texto-centro">
                                                <button
                                                    type="button"
                                                    class="btn btn-info"
                                                    @click="abrirModalAcoesNotaFiscal(nf)">
                                                    Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                                </button>
                                            </td>
                                        </tr>
                                        <tr v-if="modalConciliacao_NotasFiscais.length === 0">
                                            <td class="texto-centro" colspan="6">Nenhum registro encontrado</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </Widget>
                    </b-col>
                </b-row>

                <!-- ===== Widget: Cancelamentos de saldo ===== -->
                <b-row>
                    <b-col md="12">
                        <Widget customHeader class="estiloWidget">
                            <h4>Cancelamentos de saldo</h4>
                            <br />

                            <div class="table-card table-card--fluid">
                                <table class="estilo-tabela tabela-identidade">
                                    <thead>
                                        <tr>
                                            <th class="texto-centro">Data</th>
                                            <th>Motivo</th>
                                            <th>Observação</th>
                                            <th class="texto-centro">Valor</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="c in modalConciliacao_CancelamentosSaldo"
                                            :key="'c-' + (c.id || Math.random())">
                                            <td class="texto-centro">{{
                                                (c.dataCadastro ?
                                                    formataDataSemHora(c.dataCadastro) : '—') }}</td>
                                            <td>{{ (c.motivoDevolucao && c.motivoDevolucao.descricao ?
                                                c.motivoDevolucao.descricao : '—') }}</td>
                                            <td>{{ c.observacao || '—' }}</td>
                                            <td class="texto-centro">{{ new
                                                Intl.NumberFormat('pt-BR', {
                                                    style: 'currency', currency: 'BRL'
                                                }).format(c.valor
                                                    ||
                                                    0) }}</td>
                                        </tr>
                                        <tr v-if="modalConciliacao_CancelamentosSaldo.length === 0">
                                            <td class="texto-centro" colspan="4">Nenhum registro encontrado</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <br />
                            <button
                                @click="modalCancelamentoSaldo_IdPedidoCompra = pedidoParaConciliacaoNF.id; modalCancelamentoSaldo_Titulo = `Cancelamento de Saldo ${pedidoParaConciliacaoNF.codigo}`; modalCancelamentoSaldo_Exibir = true"
                                type="button" class="btn mb-3 mr-3 btn-danger" v-if="permissao_AprovarPedido">
                                Cancelar Saldo
                            </button>
                        </Widget>
                    </b-col>
                </b-row>

                <!-- ===== Upload anexo ===== -->
                <div>
                    <b-row>
                        <b-col md="12">
                            <Widget customHeader class="estiloWidget">
                                <b-row>
                                    <b-col>
                                        <input type="file" name="file" id="fileInput" style="display: none"
                                            class="hidden-input" @change="onChange" ref="file" multiple="false" />
                                        <br />
                                        Arquivo anexado: {{ upload_Files.length }} arquivo
                                        <br />
                                        <ul id="example-2">
                                            <li v-for="item in upload_Files" :key="item.name">
                                                {{ item.name }}
                                            </li>
                                        </ul>
                                        <label type="button" class="btn width-75 mb-3 mr-3 bg-success" for="fileInput">
                                            Escolher Arquivo
                                        </label>

                                        <b-button v-on:click="removerArquivos()" v-if="upload_Files.length > 0"
                                            variant="danger" class="width-75 mb-3 mr-3">
                                            <span>Remover Arquivos</span>
                                        </b-button>
                                    </b-col>
                                </b-row>
                            </Widget>
                        </b-col>
                    </b-row>
                </div>

            </b-container>

            <template #modal-footer>
                <b-row>
                    <b-col cols="auto" style="padding: 0">
                        <b-button @click="modalConciliacao_Exibir = false" variant="dark"
                            class="width-100 mb-3 mr-3">Fechar
                        </b-button>
                        <b-button variant="danger" class="width-230 mb-3 mr-3" @click="conciliarPedidoCompra()">
                            <span>Conciliar NF</span>
                        </b-button>
                    </b-col>
                </b-row>
            </template>
        </b-modal>

        <!-- ===== Página ===== -->
        <h1 class="page-title">Conciliação de notas fiscais</h1>
        <b-row>
            <b-col lg="12">
                <b-row>
                    <b-col md="12" xs="12">
                        <b-tabs class="mb-lg">

                            <b-tab @click="obtemPedidosEmCompra"
                                :title="'PARA CONCILIAÇÃO [' + (quantidades == null ? 0 : quantidades) + ']'"
                                class="estiloWidget"
                                :title-link-class="tab1Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
                                :active="tab1Ativa">
                                <b-button variant="success" class="width-100 mb-3 mr-3" @click="exibirModalFiltros()">
                                    FILTROS
                                </b-button>

                                <div class="table-card">
                                    <table class="estilo-tabela tabela-identidade">
                                        <thead>
                                            <tr>
                                                <th class="texto-centro">Código</th>
                                                <th class="texto-centro">Código Solicitação</th>
                                                <th class="texto-centro">Data Pedido</th>
                                                <th class="texto-centro">Data Entrega</th>
                                                <th class="texto-centro">Data Vencimento</th>
                                                <th>Centro de Custo</th>
                                                <th>Fornecedor</th>
                                                <th class="texto-centro">Valor Total</th>
                                                <th class="texto-centro">Ações</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(row, index) in pedidosEmCompra" :key="'p-' + (row.id || index)">
                                                <td class="texto-centro">{{ row.codigo }}</td>
                                                <td class="texto-centro">{{ row.solicitacaoCompra.codigo }}</td>
                                                <td class="texto-centro">{{ formataData(row.dataCadastro) }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(row.dataEntrega) }}</td>
                                                <td class="texto-centro">
                                                    {{row.faturas.map(f => formataDataSemHora(f.dataFatura)).join(' - ')
                                                    }}
                                                </td>
                                                <td>
                                                    {{
                                                        row.idCentroCustoDEF != null
                                                            ? row.centroCustoDEF.descricao
                                                            : row.centroCustoObra.codigo + ' - ' +
                                                            row.centroCustoObra.cliente.nomeFantasia
                                                    }}
                                                </td>
                                                <td>{{ row.fornecedor.nomeFantasia }}</td>
                                                <td class="texto-centro">
                                                    {{ new Intl.NumberFormat('pt-BR', {
                                                        style: 'currency', currency:
                                                            'BRL'
                                                    }).format(row.valorTotal) }}
                                                </td>
                                                <td class="texto-centro">
                                                    <button @click="obtemPedidoParaConciliacaoNF(row)" type="button"
                                                        class="btn mb-2 mr-2 btn-success"
                                                        v-if="permissao_AprovarPedido">
                                                        NF
                                                    </button>
                                                    <button @click="devolverPedidoParaCotacao(row)" type="button"
                                                        class="btn mb-2 mr-2 btn-warning"
                                                        v-if="row.notasFiscais.length == 0">
                                                        <i class="fa fa-refresh" title="Devolver para cotação"></i>
                                                    </button>
                                                </td>
                                            </tr>
                                            <tr v-if="!pedidosEmCompra || pedidosEmCompra.length === 0">
                                                <td class="texto-centro" colspan="9">Nenhum registro encontrado</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </b-tab>

                            <b-tab @click="obtemNotasConciliadas"
                                :title="'NOTAS CONCILIADAS [' + (pedidosConciliados != null ? pedidosConciliados.length : 0) + ']'"
                                class="estiloWidget"
                                :title-link-class="tab2Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
                                :active="tab2Ativa">
                                <!-- ====== FILTROS ====== -->
                                <section class="header-card mb-3">
                                    <h4 style="margin-bottom: 15px;">Filtros</h4>

                                    <b-row>
                                        <b-col md="4">
                                            <label class="lbl">Fornecedor</label>
                                            <multiselect v-model="filtro_FornecedoresSelecionados" :multiple="true"
                                                :options="filtro_Fornecedor" select-label="Selecionar"
                                                placeholder="Selecione um ou mais Fornecedores" label="nomeFantasia"
                                                track-by="nomeFantasia" />
                                        </b-col>

                                        <b-col md="4">
                                            <label class="lbl">Número Pedido</label>
                                            <b-form-input v-model="filtro_NumeroPedido" class="input-filter"
                                                placeholder="Informe o número do pedido" />
                                        </b-col>

                                        <b-col md="4">
                                            <label class="lbl">Valor</label>
                                            <Money class="input-filter" v-model="filtro_Valor" v-bind="money"
                                                placeholder="Valor da nota" />
                                        </b-col>
                                    </b-row>

                                    <br />

                                    <b-row>
                                        <b-col md="4">
                                            <label class="lbl">Número NF</label>
                                            <b-form-input v-model="filtro_NumeroNF" class="input-filter"
                                                placeholder="Informe o número da nota fiscal" />
                                        </b-col>

                                        <b-col md="4">
                                            <label class="lbl">Data Inicial</label>
                                            <div class="date-wrapper">
                                                <DatePickerMask :append-to-body="true" class="input-filter date-picker"
                                                    :not-before="dataMinima" :editable="true" lang="pt-br"
                                                    v-model="filtro_DataInicial" format="dd/MM/yyyy" :clearable="false"
                                                    type="date" />
                                                <i v-if="filtro_DataInicial" class="fa fa-times clear-icon"
                                                    @click="filtro_DataInicial = null" title="Limpar data"></i>
                                            </div>
                                        </b-col>

                                        <b-col md="4">
                                            <label class="lbl">Data Final</label>
                                            <div class="date-wrapper">
                                                <DatePickerMask :append-to-body="true" class="input-filter date-picker"
                                                    :not-before="dataMinima" :editable="true" lang="pt-br"
                                                    v-model="filtro_DataFinal" format="dd/MM/yyyy" :clearable="false"
                                                    type="date" />
                                                <i v-if="filtro_DataFinal" class="fa fa-times clear-icon"
                                                    @click="filtro_DataFinal = null" title="Limpar data"></i>
                                            </div>
                                        </b-col>
                                    </b-row>

                                    <br />

                                    <b-row>
                                        <b-col md="12" class="text-right">
                                            <b-button @click="limpaFiltros()" variant="danger"
                                                class="width-120 mb-3 mr-3">
                                                Limpar Filtros
                                            </b-button>
                                            <b-button @click="obtemNotasConciliadas()" variant="success"
                                                class="width-120 mb-3 mr-3">
                                                Pesquisar
                                            </b-button>
                                        </b-col>
                                    </b-row>
                                </section>

                                <!-- ====== TABELA ====== -->
                                <div class="table-card">
                                    <table class="estilo-tabela tabela-identidade">
                                        <thead>
                                            <tr class="text-muted">
                                                <th class="texto-centro">Pedido</th>
                                                <th>Fornecedor</th>
                                                <th class="texto-centro">Nota Fiscal</th>
                                                <th>Nome</th>
                                                <th class="texto-centro">Data Vencimento</th>
                                                <th class="texto-centro">Valor</th>
                                                <th class="texto-centro">Status</th>
                                                <th class="texto-centro">Ações</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(row, index) in pedidosConciliados"
                                                :key="'conc-' + (row.id || index)">
                                                <td class="texto-centro">{{ row.pedidoCompra.codigo }}</td>
                                                <td>{{ row.pedidoCompra.fornecedor.nomeFantasia }}</td>
                                                <td class="texto-centro">{{ row.numeroNotaFiscal }}</td>
                                                <td>{{ row.nome }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(row.dataVencimento) }}
                                                </td>
                                                <td class="texto-centro">
                                                    {{ new Intl.NumberFormat('pt-BR', {
                                                        style: 'currency', currency:
                                                            'BRL'
                                                    }).format(row.valor) }}
                                                </td>
                                                <td class="texto-centro">
                                                    {{ row.aprovada == null ? 'Pendente' : row.aprovada ? 'Aprovada' :
                                                        'Reprovada' }}
                                                </td>
                                                <td class="texto-centro">
                                                    <button
                                                        type="button"
                                                        class="btn btn-info"
                                                        @click="abrirModalAcoesNotaFiscal(row)">
                                                        Ações <i class="fa fa-ellipsis-h ml-1" aria-hidden="true"></i>
                                                    </button>
                                                </td>
                                            </tr>

                                            <tr v-if="!pedidosConciliados || pedidosConciliados.length === 0">
                                                <td class="texto-centro" colspan="8">Nenhum registro encontrado</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </b-tab>

                            <b-tab @click="obtemNotasReprovadasDiretoria"
                                :title="'NOTAS RECUSADAS DIRETORIA [' + (notasReprovadasDiretoria != null ? notasReprovadasDiretoria.length : 0) + ']'"
                                class="estiloWidget"
                                :title-link-class="tab3Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
                                :active="tab3Ativa">

                                <div class="table-card">
                                    <table class="estilo-tabela tabela-identidade">
                                        <thead>
                                            <tr class="text-muted">
                                                <th class="texto-centro">NF</th>
                                                <th>Descrição</th>
                                                <th class="texto-centro">Número NF</th>
                                                <th class="texto-centro">Pedido</th>
                                                <th class="texto-centro">Data Pedido</th>
                                                <th class="texto-centro">Obra</th>
                                                <th>Endereço da Obra</th>
                                                <th>Cliente</th>
                                                <th class="texto-centro">CNPJ</th>
                                                <th class="texto-centro">Aprovador</th>
                                                <th class="texto-centro">Data Aprovação</th>
                                                <th class="texto-centro">Ações</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="n in notasReprovadasDiretoria" :key="n.idNotaFiscal">
                                                <td class="texto-centro">{{ n.notaFiscal }}</td>
                                                <td>{{ n.descricaoNotaFiscal }}</td>
                                                <td class="texto-centro">{{ n.numeroNotaFiscal }}</td>
                                                <td class="texto-centro">{{ n.pedido }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(n.dataPedido) }}</td>
                                                <td class="texto-centro">{{ n.obra }}</td>
                                                <td>{{ n.enderecoEntregaObra }}</td>
                                                <td>{{ n.cliente }}</td>
                                                <td class="texto-centro">{{ n.cnpjCliente }}</td>
                                                <td class="texto-centro">{{ n.usuarioAprovador }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(n.dataAprovacao) }}</td>

                                                <td class="texto-centro acoes-cell">
                                                    <div class="acoes-inline">
                                                        <b-button
                                                            size="sm"
                                                            variant="success"
                                                            class="acao-btn"
                                                            @click="downloadFile(n.idArquivo, n.nomeArquivo)">
                                                            NF
                                                        </b-button>

                                                         <b-button
                                                            size="sm"
                                                            variant="success"
                                                            class="acao-btn"
                                                            @click="reenviarNotaFiscalParaDiretor(n)">
                                                            Reenviar
                                                        </b-button>

                                                        <b-button
                                                            size="sm"
                                                            variant="warning"
                                                            class="acao-btn text-white"
                                                            @click="editarNotaFiscalReprovada(n)">
                                                            Editar
                                                        </b-button>

                                                        <b-button
                                                            size="sm"
                                                            variant="danger"
                                                            class="acao-btn"
                                                            @click="marcarVisualizacaoNotaFiscal(n)">
                                                            Cancelar
                                                        </b-button>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr
                                                v-if="!notasReprovadasDiretoria || notasReprovadasDiretoria.length === 0">
                                                <td class="texto-centro" colspan="11">Nenhum registro encontrado</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </b-tab>

                            <b-tab @click="obtemNotasReprovadasFinanceiro"
                                :title="'NOTAS RECUSADAS FINANCEIRO [' + (notasReprovadasFinanceiro != null ? notasReprovadasFinanceiro.length : 0) + ']'"
                                class="estiloWidget"
                                :title-link-class="tab4Ativa ? 'tab-title-class-active' : 'tab-title-class-inactive'"
                                :active="tab4Ativa">

                                <div class="table-card">
                                    <table class="estilo-tabela tabela-identidade">
                                        <thead>
                                            <tr class="text-muted">
                                                <th class="texto-centro">NF</th>
                                                <th>Descrição</th>
                                                <th class="texto-centro">Número NF</th>
                                                <th class="texto-centro">Pedido</th>
                                                <th class="texto-centro">Data Pedido</th>
                                                <th class="texto-centro">Obra</th>
                                                <th>Endereço da Obra</th>
                                                <th>Cliente</th>
                                                <th class="texto-centro">CNPJ</th>
                                                <th class="texto-centro">Aprovador</th>
                                                <th class="texto-centro">Data Aprovação</th>
                                                <th class="texto-centro">Ações</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="n in notasReprovadasFinanceiro" :key="n.idNotaFiscal">
                                                <td class="texto-centro">{{ n.notaFiscal }}</td>
                                                <td>{{ n.descricaoNotaFiscal }}</td>
                                                <td class="texto-centro">{{ n.numeroNotaFiscal }}</td>
                                                <td class="texto-centro">{{ n.pedido }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(n.dataPedido) }}</td>
                                                <td class="texto-centro">{{ n.obra }}</td>
                                                <td>{{ n.enderecoEntregaObra }}</td>
                                                <td>{{ n.cliente }}</td>
                                                <td class="texto-centro">{{ n.cnpjCliente }}</td>
                                                <td class="texto-centro">{{ n.usuarioAprovador }}</td>
                                                <td class="texto-centro">{{ formataDataSemHora(n.dataAprovacao) }}</td>
                                                <td class="texto-centro">
                                                    <button type="button" @click="reenviarNotaFiscalParaFinanceiro(n)"
                                                        class="btn mb-2 mr-2 bg-success">
                                                        Reenviar para Financeiro
                                                    </button>
                                                </td>
                                            </tr>
                                            <tr
                                                v-if="!notasReprovadasFinanceiro || notasReprovadasFinanceiro.length === 0">
                                                <td class="texto-centro" colspan="11">Nenhum registro encontrado</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </b-tab>
                        </b-tabs>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>

        <!-- Modal de ações das tabelas (padrão do sistema) -->
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
import ModalAcoes from "@/components/ModalAcoes/ModalAcoes.vue";

Vue.use(VueSweetalert2);
Vue.use(ToggleButton);

export default {
    name: 'Conciliacao',
    components: { Multiselect, CurrencyInput, Money, Loading, DatePickerMask, ModalAcoes },
    data() {
        return {
            money: { decimal: ",", thousands: ".", prefix: "R$ ", precision: 2, masked: false },
            decimalInput: { decimal: ",", thousands: ".", prefix: "", precision: 2, masked: false },
            isLoading: false,
            dataMinima: (new Date()).setDate(new Date().getDate() - 1),
            open: false,
            quantidades: 0,

            pedidosEmCompra: [],
            notasReprovadasDiretoria: [],
            notasReprovadasFinanceiro: [],
            pedidosConciliados: [],
            notasRecusadasDiretoria: [],
            notasRecusadasFinanceiro: [],
            upload_Files: [],

            tab1Ativa: true,
            tab2Ativa: false,
            tab3Ativa: false,
            tab4Ativa: false,

            pedidoParaConciliacaoNF: null,

            idPedidoCompraNotaFiscalNovoArquivo: 0,

            // Estado do componente compartilhado ModalAcoes (uma ação por linha)
            modalAcoes_Exibir: false,
            modalAcoes_Titulo: "",
            modalAcoes_Itens: [],

            modalConciliacao_DatasDePagamento: [],

            modalConciliacao_Exibir: false,
            modalConciliacao_Titulo: 'Conciliação de NF',
            modalConciliacao_TipoConciliacao: false,
            modalConciliacao_Descricao: '',
            modalConciliacao_FornecedorCNPJ: '',
            modalConciliacao_CnpjValidado: false,
            modalConciliacao_NovaDataPagamento: null,
            modalConciliacao_Filiais: [],
            modalConciliacao_FornecedorSelecionado: null,
            modalConciliacao_PedidoCompra: null,

            // Tabelas internas do modal de conciliação
            modalConciliacao_NotasFiscais: [],
            modalConciliacao_CancelamentosSaldo: [],

            // Cancelamento de saldo (modal)
            modalCancelamentoSaldo_Exibir: false,
            modalCancelamentoSaldo_Titulo: 'Cancelamento de Saldo',
            modalCancelamentoSaldo_IdPedidoCompra: 0,
            modalCancelamentoSaldo_MotivoSelecionado: null,
            modalCancelamentoSaldo_Observacao: '',
            modalCancelamentoSaldo_Valor: 0,

            // Filtros
            filtro_Fornecedor: [],
            filtro_FornecedoresSelecionados: [],
            filtro_NumeroPedido: '',
            filtro_Valor: 0.0,
            filtro_NumeroNF: '',
            filtro_DataInicial: null,
            filtro_DataFinal: null,

            permissao_VisualizarPedido: true,
            permissao_AprovarPedido: true,
            permissao_CancelarPedido: true,

            motivosCancelamentoSaldo: [],

            colunasEmCompra: [
                "codigo", "codigoSolicitacao", "dataCadastro", "dataVencimento", "dataEntrega", "centroCusto", "fornecedor", "valorTotalCotado", "acoes",
            ],
            opcoesEmCompra: {
                perPage: 1000,
                headings: {
                    codigo: "Código",
                    codigoSolicitacao: "Código Solicitação",
                    dataCadastro: "Data do Pedido",
                    dataEntrega: "Data de entrega",
                    dataVencimento: "Data Vencimento",
                    centroCusto: "Centro de Custo",
                    fornecedor: "Fornecedor",
                    valorTotalCotado: "Valor total cotado",
                    acoes: "Ações",
                },
                clientSorting: true,
                filterable: false,
                sortable: ["codigo", "codigoSolicitacao", "dataEntrega", "dataCadastro", "centroCusto", "fornecedor", "valorTotalCotado"],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count: "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
                    first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado",
                },
            },

            colunasDatasDePagamento: ["data", "valor", "acoes"],
            opcoesDatasDePagamento: {
                perPage: 1000,
                headings: { data: "Data", valor: "Valor", acoes: "Ações" },
                clientSorting: false, filterable: false, sortable: [],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count: "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
                    first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado",
                },
            },

            colunasConciliados: ["pedidoCompra", "fornecedor", "numeroNotaFiscal", "nome", "dataVencimento", "valor", "status", "acoes"],
            opcoesConciliados: {
                perPage: 1000,
                headings: {
                    nome: "Nome", numeroNotaFiscal: "Nota Fiscal", pedidoCompra: "Pedido",
                    fornecedor: "Fornecedor", dataVencimento: "Data Vencimento",
                    valor: "Valor", status: "Status", acoes: "Ações",
                },
                clientSorting: true, filterable: false,
                sortable: ["codigo", "codigoSolicitacao", "dataEntrega", "dataCadastro", "centroCusto", "fornecedor", "valorTotalCotado"],
                pagination: { chunk: 2, dropdown: true },
                texts: {
                    filterPlaceholder: "Procurar por",
                    count: "Exibindo {from} de {to} --- total de {count} itens|{count} itens|Um item",
                    first: "Primeiro", last: "último", filter: "", limit: "Itens:", page: "Página:", noResults: "Não encontrado",
                },
            },
        };
    },
    watch: {
  'pedidoParaConciliacaoNF.valorFrete': 'calculaValorParcelas',
  'pedidoParaConciliacaoNF.valorImposto': 'calculaValorParcelas',
  'quantidadeParcelas': 'calculaValorParcelas',
  pedidoParaConciliacaoNF: {
    handler() {
      this.calculaValorParcelas()
    },
    deep: true
  }
},
    computed: {
        modalCancelamentoSaldo_SaldoDisponivel() {
            return this.pedidoParaConciliacaoNF ? (this.pedidoParaConciliacaoNF.saldoDisponivel || 0) : 0;
        },
        modalCancelamentoSaldo_SaldoRestante() {
            const valor = Number(this.modalCancelamentoSaldo_Valor || 0);
            return this.modalCancelamentoSaldo_SaldoDisponivel - valor;
        },
        modalCancelamentoSaldo_FormValido() {
            return (
                !!this.modalCancelamentoSaldo_MotivoSelecionado &&
                Number(this.modalCancelamentoSaldo_Valor) > 0 &&
                this.modalCancelamentoSaldo_SaldoRestante >= 0
            );
        },
    },
    methods: {
        noop() { },

        onEnterCancelamento() {
            if (this.modalCancelamentoSaldo_FormValido) {
                this.cancelarSaldo();
            }
        },

        descricaoFornecedor: function ({ nomeFantasia, cnpj }) {
            return `${cnpj} - ${nomeFantasia}`;
        },

        normalizaValorMonetario(valor) {
            if (valor === null || valor === undefined || valor === '') return 0;
            if (typeof valor === 'number') return valor;

            const valorString = valor.toString().trim();

            if (valorString === '') return 0;

            return parseFloat(
                valorString
                    .replace(/\s/g, '')
                    .replace('R$', '')
                    .replace(/\./g, '')
                    .replace(',', '.')
            ) || 0;
        },

        somaValoresDatasPagamento() {
            return this.modalConciliacao_DatasDePagamento.reduce((total, item) => {
                return total + this.normalizaValorMonetario(item.valor);
            }, 0);
        },

        obterIndiceDataPagamentoPorId(id) {
            return this.modalConciliacao_DatasDePagamento.findIndex(x => x.id === id);
        },

        atualizaValorDataPagamento(id, valor) {
            const index = this.obterIndiceDataPagamentoPorId(id);

            if (index !== -1) {
                this.$set(
                    this.modalConciliacao_DatasDePagamento[index],
                    'valor',
                    this.normalizaValorMonetario(valor)
                );
            }
        },

        atualizaDataPagamento(id, data) {
            const index = this.obterIndiceDataPagamentoPorId(id);

            if (index !== -1) {
                this.$set(this.modalConciliacao_DatasDePagamento[index], 'data', data);
            }
        },

        obterDatasPagamentoNormalizadas() {
            return this.modalConciliacao_DatasDePagamento.map((item, index) => {
                return {
                    id: index + 1,
                    data: item.data,
                    valor: this.normalizaValorMonetario(item.valor)
                };
            });
        },

        calculaValorNota() {
            if (this.pedidoParaConciliacaoNF == null) return 0;

            const valorImposto = this.pedidoParaConciliacaoNF.valorImposto === '' ? 0 : parseFloat(this.pedidoParaConciliacaoNF.valorImposto.toString().replace(',', '.'));
            const valorFrete = this.pedidoParaConciliacaoNF.valorFrete === '' ? 0 : parseFloat(this.pedidoParaConciliacaoNF.valorFrete.toString().replace(',', '.'));

            let valorNota = this.pedidoParaConciliacaoNF.materiais
                .reduce((sum, item) => sum + ((item.valorConciliado == '' ? 0 : parseFloat(item.valorConciliado.toString().replace(',', '.'))) * item.quantidadeParaConciliar), 0);

            valorNota += valorImposto;
            valorNota += valorFrete;

            return valorNota;
        },

        calculaValorParcelas: function () {
            if (!this.modalConciliacao_DatasDePagamento || this.modalConciliacao_DatasDePagamento.length === 0) return;

            const n = this.modalConciliacao_DatasDePagamento.length;
            // Trabalha em centavos inteiros para eliminar erros de ponto flutuante
            const totalCentavos = Math.round(this.calculaValorNota() * 100);
            const baseCentavos  = Math.floor(totalCentavos / n);
            let   restoCentavos = totalCentavos - baseCentavos * n; // 0..n-1

            this.modalConciliacao_DatasDePagamento.forEach((x, index) => {
                const centavos = baseCentavos + (restoCentavos > 0 ? 1 : 0);
                if (restoCentavos > 0) restoCentavos -= 1;
                this.$set(this.modalConciliacao_DatasDePagamento[index], 'valor', centavos / 100);
            });
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

        removerArquivos() {
            this.upload_Files = [];
        },

        exibeAlerta(mensagem) {
            this.$swal("", mensagem, "info");
        },

        // ============================================================
        // Ações das linhas de nota fiscal (usadas pelo <ModalAcoes>).
        // ============================================================
        montaAcoesNotaFiscal(row) {
            // Normaliza para suportar tanto a linha da tabela externa
            // (row.arquivo) quanto a linha da tabela dentro do modal
            // de conciliação (row.arquivo também existe).
            const idArquivo = row && row.arquivo ? row.arquivo.id : null;
            const nomeArquivo = row && row.arquivo ? row.arquivo.nome : null;

            return [
                {
                    label: "Download",
                    descricao: "Baixar o PDF da nota fiscal",
                    icone: "download",
                    variante: "warning",
                    onClick: () => this.downloadFile(idArquivo, nomeArquivo),
                    visible: idArquivo != null,
                },
                {
                    label: "Alterar arquivo",
                    descricao: "Enviar um novo PDF para esta nota fiscal",
                    icone: "upload",
                    variante: "success",
                    onClick: () => this.abrirSeletorNovoArquivoNF(row.id),
                    visible: row && row.id != null,
                },
            ];
        },

        abrirModalAcoesNotaFiscal(row) {
            this.modalAcoes_Itens = this.montaAcoesNotaFiscal(row);
            this.modalAcoes_Titulo = "Ações da nota fiscal";
            this.modalAcoes_Exibir = true;
        },

        abrirSeletorNovoArquivoNF(idNotaFiscal) {
            this.idPedidoCompraNotaFiscalNovoArquivo = idNotaFiscal;
            this.$nextTick(() => {
                const input = this.$refs.novoArquivoPdfNF;
                if (!input) return;
                // Limpa o value para permitir re-selecionar o mesmo arquivo, se necessário
                input.value = "";
                input.click();
            });
        },

        alteraArquivoNF() {
            this.isLoading = true;
            [...this.$refs.novoArquivoPdfNF.files].forEach((file) => {
                const formData = new FormData();
                formData.append("arquivo", file);
                ApiService.uploadNovoPdfNotaFiscal(formData, this.idPedidoCompraNotaFiscalNovoArquivo, (result) => {
                    this.isLoading = false;
                    if (result.status != 200) {
                        this.$swal("", result.data, "error");
                    }
                    else {
                        this.obtemNotasConciliadas();
                    }
                });
            });
        },

        downloadFile(id, nomeArquivo) {
            this.isLoading = true;
            ApiService.downloadFile('PedidoCompra', id, (result) => {
                this.isLoading = false;
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                }
                else {
                    const blob = new Blob([result.data], { type: result.contentType });
                    const fileURL = URL.createObjectURL(blob);
                    window.open(fileURL, '_blank');
                }
            });
        },

        validaCNPJ() {
            if (this.pedidoParaConciliacaoNF.fornecedor.cnpj == this.modalConciliacao_FornecedorCNPJ) {
                this.modalConciliacao_CnpjValidado = true;
            } else {
                this.modalConciliacao_CnpjValidado = false;
                if (this.pedidoParaConciliacaoNF.notasFiscais && this.pedidoParaConciliacaoNF.notasFiscais.length == 0) {
                    this.$swal("", "", "error");
                    this.$swal({
                        title: "Atenção",
                        text: "CNPJ não confere. Deseja trocar o fornecedor?",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#3085d6",
                        cancelButtonColor: "#d33",
                        confirmButtonText: "Sim!",
                        cancelButtonText: "Não",
                    }).then((result) => {
                        if (result.isConfirmed) {
                            this.isLoading = true;
                            var objetoTrocaFornecedor = { IdPedidoCompra: this.pedidoParaConciliacaoNF.id, CNPJNovoFornecedor: this.modalConciliacao_FornecedorCNPJ };
                            ApiService.trocarFornecedor(objetoTrocaFornecedor, (result2) => {
                                this.isLoading = false;
                                if (result2.status != 200) {
                                    this.$swal("", result2.message, "error");
                                }
                                else {
                                    this.$swal("Sucesso", "Fornecedor alterado para '" + result2.message.nomeFantasia + "'", "success");
                                    window.location.reload();
                                }
                            });
                        }
                    });
                } else {
                    this.$swal({
                        title: "Atenção",
                        text: "CNPJ não confere e já existe uma conciliação anterior. Deseja abrir um novo pedido de compra para o fornecedor informado?",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#3085d6",
                        cancelButtonColor: "#d33",
                        confirmButtonText: "Sim!",
                        cancelButtonText: "Não",
                    }).then((result) => {
                        if (result.isConfirmed) {
                            this.isLoading = true;
                            var objetoCancelamentoSaldo = { IdPedidoCompra: this.pedidoParaConciliacaoNF.id, CNPJNovoFornecedor: this.modalConciliacao_FornecedorCNPJ };
                            ApiService.cancelarSaldoAbrirPedido(objetoCancelamentoSaldo, (result2) => {
                                this.isLoading = false;
                                if (result2.status != 200) {
                                    this.$swal("", result2.message, "error");
                                }
                                else {
                                    this.$swal("Sucesso", "Pedido '" + result2.message.codigo + "' criado com sucesso.", "success");
                                    window.location.reload();
                                }
                            });
                        }
                    });
                }
            }
        },

        excluiDataPagamento(id) {
            var listaAuxiliar = [];
            this.modalConciliacao_DatasDePagamento.forEach(data => {
                if (data.id != id) {
                    var objetoData = {
                        id: listaAuxiliar.length + 1,
                        data: data.data,
                        valor: this.normalizaValorMonetario(data.valor)
                    };
                    listaAuxiliar.push(objetoData);
                }
            });
            this.modalConciliacao_DatasDePagamento = listaAuxiliar;
        },

        inserirNovaDataPagamento() {
            if (this.modalConciliacao_NovaDataPagamento != null) {
                var objetoData = {
                    id: this.modalConciliacao_DatasDePagamento.length + 1,
                    data: this.modalConciliacao_NovaDataPagamento,
                    valor: 0
                };
                this.modalConciliacao_DatasDePagamento.push(objetoData);
                this.modalConciliacao_NovaDataPagamento = null;
            }
            this.calculaValorParcelas();
        },

        cancelarSaldo: function () {
            this.isLoading = true;

            if (this.modalCancelamentoSaldo_MotivoSelecionado == null) {
                this.isLoading = false;
                this.$swal("", "Escolha um motivo de cancelamento de saldo", "error");
                return;
            }

            if (this.modalCancelamentoSaldo_Valor <= 0) {
                this.isLoading = false;
                this.$swal("", "Informe um valor superior a R$ 0,00", "error");
                return;
            }

            if (this.modalCancelamentoSaldo_SaldoRestante < 0) {
                this.isLoading = false;
                this.$swal("", "O valor informado supera o saldo disponível", "error");
                return;
            }

            var objCancelamentoSaldo = {
                id: 0,
                idPedidoCompra: this.modalCancelamentoSaldo_IdPedidoCompra,
                idMotivoDevolucaoSaldo: this.modalCancelamentoSaldo_MotivoSelecionado.id,
                valor: this.modalCancelamentoSaldo_Valor,
                observacao: this.modalCancelamentoSaldo_Observacao
            };

            ApiService.cancelarSaldo(objCancelamentoSaldo, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.data, "error");
                } else {
                    this.modalCancelamentoSaldo_IdPedidoCompra = 0;
                    this.modalCancelamentoSaldo_MotivoSelecionado = null;
                    this.modalCancelamentoSaldo_Observacao = '';
                    this.modalCancelamentoSaldo_Titulo = 'Cancelamento de Saldo';
                    this.modalCancelamentoSaldo_Valor = 0;
                    this.modalCancelamentoSaldo_Exibir = false;

                    this.modalConciliacao_Exibir = false;

                    this.obtemPedidosEmCompra();

                    this.$swal("", "Cancelamento de saldo efetuado com sucesso", "success");
                }
            });
        },

        editarNotaFiscalReprovada(notaFiscal) {
     
            this.pedidoParaConciliacaoNF = null;
            this.isLoading = true;

            ApiService.get('PedidoCompra', notaFiscal.idPedidoCompra, (result1) => {

                if (result1.status != 200) {
                    this.$swal("", result1.message, "error");
                } else {
                    this.modalConciliacao_PedidoCompra = result1.data;
                    
                    ApiService.getPedidoCompraParaConciliacaoNF(notaFiscal.idPedidoCompra, (result) => {
                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.message, "error");
                        } else {
                            this.pedidoParaConciliacaoNF = result.data;

                            this.modalConciliacao_DatasDePagamento = [];
                            if (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.faturas) {
                                this.pedidoParaConciliacaoNF.faturas.forEach(fatura => {
                                    var objetoData = {
                                        id: this.modalConciliacao_DatasDePagamento.length + 1,
                                        data: fatura.dataFatura,
                                        valor: 0
                                    };
                                    this.modalConciliacao_DatasDePagamento.push(objetoData);
                                });
                            }

                            this.modalConciliacao_NotasFiscais =
                                (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.notasFiscais)
                                    ? this.pedidoParaConciliacaoNF.notasFiscais : [];

                            this.modalConciliacao_CancelamentosSaldo =
                                (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.cancelamentosSaldo)
                                    ? this.pedidoParaConciliacaoNF.cancelamentosSaldo : [];

                            ApiService.obtemFiliais(this.pedidoParaConciliacaoNF.fornecedor.id, (result2) => {
                                if (result2.status != 200) {
                                    this.$swal("", result2.message, "error");
                                } else {
                                    this.modalConciliacao_Filiais = result2.data;
                                    this.modalConciliacao_FornecedorSelecionado =
                                        this.modalConciliacao_Filiais.filter(filial => {
                                            return filial.id == this.pedidoParaConciliacaoNF.fornecedor.id;
                                        })[0];

                                    this.modalConciliacao_Exibir = true;
                                }
                            });

                            ApiService.marcarVisualizacaoNotaFiscalReprovada(notaFiscal.idNotaFiscal, (result) => {
                                this.isLoading = false;

                                if (result.status != 200) {
                                } else {
                                    this.obtemNotasReprovadasDiretoria();
                                }
                            });
                        }
                    });
                }
            });
        },

        marcarVisualizacaoNotaFiscal(notaFiscal){
            this.$swal({
                title: "Atenção",
                text: "Deseja cancelar a nota fiscal '" + notaFiscal.nomeArquivo + "'?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sim!",
                cancelButtonText: "Não",
            }).then((result) => {
                if (result.isConfirmed) {
                    this.isLoading = true;

                    ApiService.marcarVisualizacaoNotaFiscalReprovada(notaFiscal.idNotaFiscal, (result) => {
                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.data, "error");
                        } else {
                            this.$swal("", "Nota fiscal cancelada.", "success");
                            this.obtemNotasReprovadasDiretoria();
                        }
                    });
                }
            });
        },

        reenviarNotaFiscalParaDiretor(notaFiscal) {
            this.$swal({
                title: "Atenção",
                text: "Deseja reenviar a nota fiscal '" + notaFiscal.numeroNotaFiscal + "' para aprovação da diretoria?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sim!",
                cancelButtonText: "Não",
            }).then((result) => {
                if (result.isConfirmed) {
                    this.isLoading = true;

                    ApiService.reenviarNotaFiscalDiretoria(notaFiscal.idNotaFiscal, (result) => {
                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.data, "error");
                        } else {
                            this.$swal("", "Nota fiscal reenviada para a diretoria", "success");
                            this.obtemNotasReprovadasDiretoria();
                        }
                    });
                }
            });
        },

        reenviarNotaFiscalParaFinanceiro(notaFiscal) {
            this.$swal({
                title: "Atenção",
                text: "Deseja reenviar a nota fiscal '" + notaFiscal.numeroNotaFiscal + "' para aprovação do financeiro?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sim!",
                cancelButtonText: "Não",
            }).then((result) => {
                if (result.isConfirmed) {
                    this.isLoading = true;

                    ApiService.reenviarNotaFiscalFinanceiro(notaFiscal.idNotaFiscal, (result) => {
                        this.isLoading = false;

                        if (result.status != 200) {
                            this.$swal("", result.data, "error");
                        } else {
                            this.$swal("", "Nota fiscal reenviada para o financeiro", "success");
                            this.obtemNotasReprovadasFinanceiro();
                        }
                    });
                }
            });
        },

        conciliarPedidoCompra: function () {
            let valorNota = this.pedidoParaConciliacaoNF.materiais.reduce((sum, item) => {
                return sum + ((item.valorConciliado == '' ? 0 : parseFloat(item.valorConciliado.toString().replace(',', '.'))) * item.quantidadeParaConciliar);
            }, 0);

            valorNota = valorNota + parseFloat(this.pedidoParaConciliacaoNF.valorFrete.toString().replace(',', '.'));
            valorNota = valorNota + parseFloat(this.pedidoParaConciliacaoNF.valorImposto.toString().replace(',', '.'));

            this.pedidoParaConciliacaoNF.valorFrete = parseFloat(this.pedidoParaConciliacaoNF.valorFrete.toString().replace(',', '.'));
            this.pedidoParaConciliacaoNF.valorImposto = parseFloat(this.pedidoParaConciliacaoNF.valorImposto.toString().replace(',', '.'));

            const datasPagamentoNormalizadas = this.obterDatasPagamentoNormalizadas();
            let valorPagamentoNasDatas = 0.0;

            datasPagamentoNormalizadas.forEach(x => {
                valorPagamentoNasDatas = parseFloat(valorPagamentoNasDatas) + this.normalizaValorMonetario(x.valor);
            });

            if (valorNota <= 0) {
                this.$swal("", "Nota fiscal com valor inválido", "error");
                return;
            }

            valorPagamentoNasDatas = parseFloat(Number(valorPagamentoNasDatas).toFixed(12));

            if (valorNota != valorPagamentoNasDatas) {
                this.$swal("", "Nota fiscal com valor diferente da somatória dos valores nas datas informadas", "error");
                return;
            }

            if (this.modalConciliacao_FornecedorSelecionado == null || this.modalConciliacao_FornecedorSelecionado.length == 0) {
                this.$swal("", "Informe um fornecedor válido", "error");
                return;
            }

            if (this.pedidoParaConciliacaoNF.numeroNotaFiscal == null) {
                this.$swal("", "Informe um número válido para nota fiscal", "error");
                return;
            }

            if (datasPagamentoNormalizadas.length == 0) {
                this.$swal("", "Informe ao menos uma data de pagamento", "error");
                return;
            }

            if (this.upload_Files.length == 0) {
                this.$swal("", "Escolha um arquivo PDF para realizar a conciliação da nota fiscal", "error");
            } else {
                this.isLoading = true;

                const formData = new FormData();
                this.upload_Files.forEach((file) => {
                    formData.append("arquivo", file);
                });

                ApiService.uploadFile('PedidoCompra/Conciliar', formData, this.pedidoParaConciliacaoNF.id, (result) => {
                    if (result.status != 200) {
                        this.isLoading = false;
                        this.$swal("", result.data, "error");
                    } else {
                        if (this.modalConciliacao_TipoConciliacao) {
                            this.pedidoParaConciliacaoNF.materiais.forEach((material) => {
                                material.quantidadeParaConciliar = material.quantidade - material.quantidadeConciliada;
                            });
                        }

                        this.modalConciliacao_DatasDePagamento = datasPagamentoNormalizadas;
                        this.pedidoParaConciliacaoNF.descricao = this.modalConciliacao_Descricao;
                        this.pedidoParaConciliacaoNF.dataValorPagamento = datasPagamentoNormalizadas;
                        this.pedidoParaConciliacaoNF.idFornecedorEscolhido = this.modalConciliacao_FornecedorSelecionado.id;

                        this.pedidoParaConciliacaoNF.materiais.forEach((material) => {
                            material.quantidadeParaConciliar = parseFloat(material.quantidadeParaConciliar.toString().replace(',', '.'));
                            material.valorConciliado = parseFloat(material.valorConciliado.toString().replace(',', '.'));
                        });

                        ApiService.conciliar(this.pedidoParaConciliacaoNF, result.data.id, (result2) => {
                            this.isLoading = false;

                            if (result2.status != 200) {
                                this.$swal("", result2.data, "error");
                            } else {
                                this.modalConciliacao_Descricao = '';
                                this.modalConciliacao_Exibir = false;
                                this.upload_Files = [];

                                this.$swal("", "Conciliação de nota fiscal efetuada com sucesso", "success");

                                window.location.reload();
                            }
                        });
                    }
                });
            }
        },

        limpaFiltros: function () {
            this.filtro_DataFinal = null;
            this.filtro_DataInicial = null;
            this.filtro_FornecedoresSelecionados = [];
            this.filtro_NumeroNF = '';
            this.filtro_NumeroPedido = '';
            this.filtro_Valor = 0.0;
        },

        obtemNotasConciliadas: function () {
            this.tab1Ativa = false;
            this.tab2Ativa = true;
            this.tab3Ativa = false;
            this.tab4Ativa = false;

            var objetoPesquisa = {
                idsFornecedores: [],
                numeroPedido: this.filtro_NumeroPedido,
                numeroNotaFiscal: this.filtro_NumeroNF,
                valor: this.filtro_Valor,
                dataInicial: this.filtro_DataInicial,
                dataFinal: this.filtro_DataFinal
            };

            if (this.filtro_FornecedoresSelecionados != null) {
                this.filtro_FornecedoresSelecionados.forEach(x => {
                    objetoPesquisa.idsFornecedores.push(x.id);
                });
            }

            this.pedidosConciliados = [];
            this.isLoading = true;

            ApiService.obtemNotasConciliadas(objetoPesquisa, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidosConciliados = result.data;
                }
            });
        },

        obtemPedidoParaConciliacaoNF: function (pedido) {
            this.modalConciliacao_PedidoCompra = pedido;
            this.pedidoParaConciliacaoNF = null;
            this.isLoading = true;

            ApiService.getPedidoCompraParaConciliacaoNF(pedido.id, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidoParaConciliacaoNF = result.data;

                    this.modalConciliacao_DatasDePagamento = [];
                    if (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.faturas) {
                        this.pedidoParaConciliacaoNF.faturas.forEach(fatura => {
                            var objetoData = {
                                id: this.modalConciliacao_DatasDePagamento.length + 1,
                                data: fatura.dataFatura,
                                valor: 0
                            };
                            this.modalConciliacao_DatasDePagamento.push(objetoData);
                        });
                    }

                    this.modalConciliacao_NotasFiscais =
                        (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.notasFiscais)
                            ? this.pedidoParaConciliacaoNF.notasFiscais : [];

                    this.modalConciliacao_CancelamentosSaldo =
                        (this.pedidoParaConciliacaoNF && this.pedidoParaConciliacaoNF.cancelamentosSaldo)
                            ? this.pedidoParaConciliacaoNF.cancelamentosSaldo : [];

                    ApiService.obtemFiliais(this.pedidoParaConciliacaoNF.fornecedor.id, (result2) => {
                        if (result2.status != 200) {
                            this.$swal("", result2.message, "error");
                        } else {
                            this.modalConciliacao_Filiais = result2.data;
                            this.modalConciliacao_FornecedorSelecionado =
                                this.modalConciliacao_Filiais.filter(filial => {
                                    return filial.id == this.pedidoParaConciliacaoNF.fornecedor.id;
                                })[0];

                            this.modalConciliacao_Exibir = true;
                        }
                    });
                }
            });
        },

        obtemPedidosEmCompra: function () {
            this.tab1Ativa = true;
            this.tab2Ativa = false;
            this.tab3Ativa = false;
            this.tab4Ativa = false;

            this.isLoading = true;
            this.pedidosEmCompra = [];

            ApiService.getPedidosParaConciliacao((result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.pedidosEmCompra = result.data;
                    this.quantidades = result.data.length;
                }
            });
        },

        obtemNotasReprovadasDiretoria: function () {
            this.tab1Ativa = false;
            this.tab2Ativa = false;
            this.tab3Ativa = true;
            this.tab4Ativa = false;

            this.isLoading = true;
            this.notasReprovadasDiretoria = [];

            ApiService.obtemNotasFiscaisReprovadasDiretoria((result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.notasReprovadasDiretoria = result.data;
                }
            });
        },

        obtemNotasReprovadasFinanceiro: function () {
            this.tab1Ativa = false;
            this.tab2Ativa = false;
            this.tab3Ativa = false;
            this.tab4Ativa = true;

            this.isLoading = true;
            this.notasReprovadasFinanceiro = [];

            ApiService.obtemNotasFiscaisReprovadasFinanceiro((result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.notasReprovadasFinanceiro = result.data;
                }
            });
        },

        obtemMotivosCancelamentoSaldo: function () {
            this.isLoading = true;
            this.motivosCancelamentoSaldo = [];

            ApiService.getAll('MotivoDevolucao', true, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.motivosCancelamentoSaldo = result.data;
                }
            });
        },

        listaFornecedores: function () {
            this.filtro_Fornecedor = [];

            ApiService.getAll("Fornecedor", false, (result) => {
                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.filtro_Fornecedor = result.data;
                }
            });
        },

        devolverPedidoParaCotacao(pedidoCompra) {
            this.isLoading = true;

            ApiService.devolverPedidoParaCotacao(pedidoCompra.id, (result) => {
                this.isLoading = false;

                if (result.status != 200) {
                    this.$swal("", result.message, "error");
                } else {
                    this.$swal("Sucesso", "O pedido foi devidamente cancelado e a solicitação de compra originária do pedido está disponível no status 'Devolvidas Diretoria' ", "success");
                    this.obtemPedidosEmCompra();
                }
            });
        },
    },
    mounted() {
        this.obtemPedidosEmCompra();
        this.obtemNotasReprovadasDiretoria();
        this.obtemNotasReprovadasFinanceiro();
        this.obtemMotivosCancelamentoSaldo();
        this.listaFornecedores();

        // As chamadas acima alimentam os contadores das abas. Garantimos que
        // a aba 1 permaneça ativa ao carregar a página.
        this.tab1Ativa = true;
        this.tab2Ativa = false;
        this.tab3Ativa = false;
        this.tab4Ativa = false;
    },
};
</script>

<style src="./Conciliacao.scss" lang="scss" />

<style scoped>
/* Cards de resumo dentro do modal (key-value grid) */
.header-card {
    background: rgba(255, 255, 255, 0.03);
    border: 1px solid rgba(255, 255, 255, 0.10);
    border-radius: 12px;
    padding: 14px;
    box-shadow: 0 4px 14px rgba(0, 0, 0, 0.08);
}

.summary-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
    gap: 10px;
}

.kv .k {
    color: #ffd639;
    font-weight: 700;
    font-size: 12px;
    letter-spacing: .02em;
    text-transform: uppercase;
    opacity: .95;
}

.kv .v {
    display: block;
    font-weight: 600;
    margin-top: 2px;
}

.kv.kv--span2 {
    grid-column: span 2;
}

.kv-input {
    background: rgba(0, 0, 0, .35) !important;
    border: 1px solid rgba(255, 255, 255, .15) !important;
    border-radius: 10px;
    color: #fff !important;
}

.modal-form .lbl {
    font-weight: 700;
    margin-bottom: 6px;
    display: block;
}

.modal-form .req {
    color: #ff7b7b
}

.helper {
    display: block;
    margin-top: 4px;
    opacity: .85
}

.helper.error {
    color: #ff7b7b
}

.tile-readonly {
    background: rgba(255, 255, 255, .06);
    border: 1px solid rgba(255, 255, 255, .12);
    border-radius: 10px;
    padding: 8px 12px;
    font-weight: 700;
}

.tile-readonly.negativo {
    border-color: #ff7b7b;
    color: #ffbaba;
}

/* Saldos verticais no modal */
.saldos-vertical {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.saldos-vertical .kv {
    background: rgba(255, 255, 255, 0.02);
    border: 1px solid rgba(255, 255, 255, 0.12);
    border-radius: 12px;
    padding: 12px 14px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.saldos-vertical .kv .k {
    color: #ffd639;
}

.saldos-vertical .kv .v {
    font-weight: 700;
}

/* Botão radius uniforme nesse modal */
.btn {
    border-radius: 10px !important;
}

/* ===================================================================
   AJUSTES DE CAMPOS E LAYOUT — padronização dark
   =================================================================== */

/* Inputs "brancos" que aparecem dentro de tabelas (qtd/valor conciliação) */
.input-table {
    background: rgba(0, 0, 0, .35);
    border: 1px solid rgba(255, 255, 255, .15);
    border-radius: 8px;
    color: #fff;
    padding: 6px 10px;
    width: 100%;
    max-width: 120px;
    text-align: center;
    outline: none;
    transition: border-color .15s ease, background .15s ease;
}

.input-table:focus {
    border-color: #ffd639;
    background: rgba(0, 0, 0, .55);
    box-shadow: 0 0 0 2px rgba(255, 214, 57, .15);
}

/* b-form-input genérico dentro do modal de conciliação */
.modal-dialog ::v-deep .form-control,
.modal-dialog ::v-deep input.form-control {
    background-color: rgba(0, 0, 0, .35) !important;
    border: 1px solid rgba(255, 255, 255, .15) !important;
    border-radius: 10px !important;
    color: #fff !important;
}

.modal-dialog ::v-deep .form-control:focus {
    border-color: #ffd639 !important;
    background-color: rgba(0, 0, 0, .55) !important;
    box-shadow: 0 0 0 2px rgba(255, 214, 57, .15) !important;
}

/* v-money (Money) — o componente renderiza um input direto */
.modal-dialog ::v-deep .kv-input input,
.modal-dialog ::v-deep input.kv-input,
.modal-dialog ::v-deep .v-money,
.modal-dialog ::v-deep input.v-money {
    background-color: rgba(0, 0, 0, .35) !important;
    border: 1px solid rgba(255, 255, 255, .15) !important;
    border-radius: 10px !important;
    color: #fff !important;
    padding: 6px 10px !important;
}

/* DatePickerMask — precisa alcançar os inputs internos dele */
.modal-dialog ::v-deep .mx-datepicker,
.modal-dialog ::v-deep .mx-datepicker .mx-input-wrapper,
.modal-dialog ::v-deep .mx-input {
    background-color: rgba(0, 0, 0, .35) !important;
    border: 1px solid rgba(255, 255, 255, .15) !important;
    border-radius: 10px !important;
    color: #fff !important;
    box-shadow: none !important;
}

.modal-dialog ::v-deep .mx-datepicker {
    width: 100%;
}

.modal-dialog ::v-deep .mx-input:focus {
    border-color: #ffd639 !important;
    background-color: rgba(0, 0, 0, .55) !important;
    box-shadow: 0 0 0 2px rgba(255, 214, 57, .15) !important;
}

.modal-dialog ::v-deep .mx-icon-calendar,
.modal-dialog ::v-deep .mx-icon-clear {
    color: rgba(255, 255, 255, .75) !important;
}

/* Textarea autosize da descrição — alinhar com o tema */
.modal-dialog ::v-deep textarea.form-control {
    background-color: rgba(0, 0, 0, .35) !important;
    border: 1px solid rgba(255, 255, 255, .15) !important;
    border-radius: 10px !important;
    color: #fff !important;
    min-height: 90px !important;
    resize: vertical;
}

/* Descrição: label visual */
.modal-dialog label {
    color: #ffd639;
    font-weight: 600;
    font-size: 12px;
    letter-spacing: .02em;
    text-transform: uppercase;
    margin-bottom: 6px;
    display: block;
}

/* Layout do bloco "Inserir nova data" — alinhar tudo na mesma linha */
.pagto-actions {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 10px;
    margin-top: 14px;
    padding: 12px 14px;
    background: rgba(255, 255, 255, .02);
    border: 1px solid rgba(255, 255, 255, .08);
    border-radius: 12px;
}

.pagto-actions .label-inline {
    color: #ffd639;
    font-weight: 600;
    font-size: 12px;
    letter-spacing: .02em;
    text-transform: uppercase;
    margin: 0;
}

.pagto-actions ::v-deep .mx-datepicker {
    width: auto;
    min-width: 180px;
    flex: 0 0 auto;
}

.pagto-actions .btn {
    margin: 0 !important;
    width: auto !important;
    min-width: 160px;
}

/* Células da tabela de parcelas — inputs preenchem a célula sem ficarem brancos */
.col-cell {
    display: flex;
    align-items: center;
    justify-content: center;
    min-height: 38px;
}

.col-cell.acoes-cell {
    justify-content: center;
}

.tabela-identidade ::v-deep .mx-datepicker,
.tabela-identidade ::v-deep .mx-input {
    max-width: 160px;
}

/* Garante que o toggle "Conciliação parcial" não fica solto */
.modal-dialog ::v-deep .vue-js-switch {
    vertical-align: middle;
}

/* Botões de ação em tabela (lixeira etc) — consistência com padrão afm */
.acoes-cell .btn {
    padding: 6px 10px !important;
    min-width: 36px;
    line-height: 1;
}
</style>