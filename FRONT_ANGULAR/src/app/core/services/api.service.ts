import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = environment.baseURLApi;

  constructor(private http: HttpClient) {}

  get(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getAll(controller: string, apenasAtivos: boolean, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}?apenasAtivos=${apenasAtivos}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  post(controller: string, dto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}`, dto).subscribe({
      next: (data: any) => cb({ status: 201, message: data, data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  put(controller: string, dto: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/${controller}`, dto).subscribe({
      next: (data: any) => cb({ status: 201, message: data, data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  delete(controller: string, id: any, cb: (r: any) => void): void {
    this.http.delete(`${this.base}/${controller}/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  ativarDesativar(controller: string, id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/AtivarDesativar?id=${id}&ativarDesativar=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  habilitarDesabilitarPedidoInterno(controller: string, id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/HabilitarDesabilitarPI?id=${id}&habilitarDesabilitar=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== RASCUNHO =====
  obtemRascunhos(cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompraRascunho`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  salvaRascunho(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompraRascunho`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  atualizaRascunho(objeto: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/SolicitacaoCompraRascunho`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  excluiRascunho(id: any, cb: (r: any) => void): void {
    this.http.delete(`${this.base}/SolicitacaoCompraRascunho/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== MATERIAIS =====
  buscaMaterialPorTexto(material: string, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Material/BuscaMaterialPorTexto/${material}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== SOLICITAÇÕES DE COMPRA =====
  getSolicitacoesCompraStatus(idStatus: number, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/Status/${idStatus}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getSolicitacoesCompraUsuario(filtros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Usuario/`, filtros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarSolicitacaoCompra(id: any, motivo: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cancelar?idSolicitacaoCompra=${id}&motivoCancelamento=${motivo}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  validarSolicitacaoCompra(id: any, idComprador: number, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Validar/${id}/${idComprador}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  reprovarSolicitacaoCompra(id: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Reprovar/${id}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  reabrirSolicitacaoCompra(obj: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Reabrir`, obj).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarParaAprovacao(id: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/EnviarParaAprovacao/${id}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarParaRevisao(id: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/EnviarParaRevisao/${id}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== PEDIDOS INTERNOS =====
  getPedidosInternosUsuario(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoInterno/Usuario/`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemProximoCodigoPedidoInterno(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoInterno/ProximoCodigo`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemPedidosInternosParaAprovacao(idUsuario: number, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoInterno/ObtemPedidosInternosParaAprovacao/${idUsuario}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  aprovarReprovarPedidoInterno(id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoInterno/AprovarReprovarPedidoInterno/${id}/${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== NOTAS FISCAIS =====
  obtemNotasFiscaisParaAprovacao(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/NotaFiscal/ObtemParaAprovacao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemCotacoesParaAprovacao(cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/Cotacoes/ObtemParaAprovacao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  aprovarReprovarNotaFiscal(id: any, aprovada: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/AprovarReprovar/${id}/${aprovada}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  editarDataVencimentoNF(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/NotaFiscal/EditarDataVencimento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasFiscaisAlteracaoData(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/NotaFiscal/ObtemNotasFiscaisAlteracaoData/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== ARQUIVOS =====
  uploadFile(controller: string, formData: FormData, id: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/Upload/${id}`, formData).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadFile(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/Download/${id}`, { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getArquivos(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/Arquivos/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfPedidoCompra(id: any, exibirDataPagamento: boolean = true, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/Pdf/Download/${id}/${exibirDataPagamento}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfArquivoSolicitacaoCompra(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/Pdf/Download/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfPedidoInterno(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoInterno/Pdf/Download/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== COMENTARIOS =====
  obtemComentarios(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/Comentario/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarComentario(controller: string, id: any, texto: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/Comentario?id=${id}&comentario=${texto}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== OUTROS =====
  listaFornecedoresFiltrados(filtro: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Fornecedor/GetFiltrado`, filtro).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  habilitarDesabilitarLogin(idUsuario: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Usuario/HabilitaDesabilitaLogin?id=${idUsuario}&habilitaDesabilitaLogin=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarSenha(idUsuario: any, senha: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Usuario/AlterarSenha?id=${idUsuario}&senha=${senha}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarPedidoCompra(id: any, motivo: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Cancelar?idPedidoCompra=${id}&motivoCancelamento=${motivo}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== USUÁRIOS =====
  getEngenheiros(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Usuario/Engenheiros`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getDiretores(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Usuario/Diretores`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== SOLICITAÇÕES DE COMPRA (extras) =====
  getSolicitacaoCompraParaCotacao(idStatus: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/ParaCotacao/${idStatus}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemQuantidades(cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/ObtemQuantidades`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarSolicitacaoParaAprovacao(idSolicitacaoCompra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/EnviarSolicitacaoParaAprovacao/${idSolicitacaoCompra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  removerItensSelecionados(solicitacaoCompraParaCotacao: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/RemoverItensSelecionados`, solicitacaoCompraParaCotacao).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  duplicarItensSelecionados(solicitacaoCompraParaCotacao: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/DuplicarItensSelecionados`, solicitacaoCompraParaCotacao).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  excluiMaterialCotacao(idMaterial: any, idSolicitacaoCompra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/Material/${idMaterial}/${idSolicitacaoCompra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfCotacoes(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/Pdf/Cotacoes/Download/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelCotacoes(id: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Excel/Cotacoes/Download/${id}`, null,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== COTAÇÕES =====
  editarCotacao(objSolicitacaoCompra: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/SolicitacaoCompra/Cotacao/Editar`, objSolicitacaoCompra).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  definirFornecedorPrincipal(idSolicitacaoCompra: any, idFornecedor: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/DefinirFornecedorPrincipal?idSolicitacaoCompra=${idSolicitacaoCompra}&idFornecedor=${idFornecedor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  removeFornecedor(idSolicitacaoCompra: any, idFornecedor: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/ExcluirFornecedor?idSolicitacaoCompra=${idSolicitacaoCompra}&idFornecedor=${idFornecedor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  definirDataEntregaFornecedor(idSolicitacaoCompra: any, idFornecedor: any, dataEntrega: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/DefinirDataEntregaFornecedor?idSolicitacaoCompra=${idSolicitacaoCompra}&idFornecedor=${idFornecedor}&dataEntrega=${dataEntrega}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cadastrarCotacao(objetoCotacao: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao`, objetoCotacao).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cadastraNovoFornecedorCotacao(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/Fornecedor`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemCotacoes(idSolicitacaoCompraMaterial: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/Cotacoes?idSolicitacaoCompraMaterial=${idSolicitacaoCompraMaterial}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  definirCotacaoPrincipal(controller: string, idObjeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/${idObjeto}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarDiretoria(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/EnviarDiretoria`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  devolverCompras(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/DevolverCompras`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  finalizarCotacao(controller: string, idObjeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}${idObjeto}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  reprovarCotacao(idObjeto: any, comentario: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cotacao/Reprovar/${idObjeto}?comentario=${comentario}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== PEDIDOS DE COMPRA (extras) =====
  getPedidoCompraParaConciliacaoNF(idPedidoCompra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/ParaConciliacao/${idPedidoCompra}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  conciliar(idPedidoCompraNotaFiscal: any, objPedidoCompraConciliacao: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/PedidoCompra/Conciliar/${idPedidoCompraNotaFiscal}`, objPedidoCompraConciliacao).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getPedidosCompraStatus(idStatus: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/Status/${idStatus}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  finalizarPedidoCompra(idPedidoCompra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Finalizar?idPedidoCompra=${idPedidoCompra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarSaldo(objCancelamentoSaldo: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/CancelarSaldo`, objCancelamentoSaldo).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getPedidosParaConciliacao(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/ParaConciliacao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemPedidosFinalizadosFiltrados(objParametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/ObtemFinalizadosFiltrados`, objParametros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarSaldoAbrirPedido(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Conciliar/CancelarSaldoAbrirPedido`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  trocarFornecedor(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Conciliar/TrocarFornecedor`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  devolverPedidoParaCotacao(idPedidoCompra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/DevolverParaCotacao/${idPedidoCompra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== NOTAS FISCAIS (extras) =====
  getItemFiltro(item: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/NotaFiscal/ItemFiltro?item=${item}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasFiltradas(objFiltro: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/NotaFiscal/ObtemNotasFiltradas`, objFiltro).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasFiscaisReprovadasDiretoria(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/Conciliacao/ReprovadasDirecao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  reenviarNotaFiscalDiretoria(idPedidoCompraNotaFiscal: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Conciliacao/NotaFiscal/ReenviarDiretoria/${idPedidoCompraNotaFiscal}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  reenviarNotaFiscalFinanceiro(idPedidoCompraNotaFiscal: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/Conciliacao/NotaFiscal/ReenviarFinanceiro/${idPedidoCompraNotaFiscal}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasFiscaisReprovadasFinanceiro(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/Conciliacao/ReprovadasFinanceiro`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasConciliadas(objetoPesquisa: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/ObtemNotasConciliadas`, objetoPesquisa).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarNotaFiscal(idPedidoCompraNotaFiscal: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/Cancelar/${idPedidoCompraNotaFiscal}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarObraNF(idPedidoCompraNotaFiscal: any, idObra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/AlterarObra/${idPedidoCompraNotaFiscal}/${idObra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  marcarVisualizacaoNotaFiscalReprovada(idPedidoCompraNotaFiscal: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/MarcarVisualizacaoNotaFiscalReprovada/${idPedidoCompraNotaFiscal}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  postNotaFiscalPagamento(objPagamento: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/PostNotaFiscalPagamento`, objPagamento).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getNotaFiscalPagamento(idPedidoCompraNotaFiscal: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/NotaFiscal/ObtemNotaFiscalPagamento/${idPedidoCompraNotaFiscal}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alteraArquivoNotaFiscal(idPedidoCompraArquivo: any, file: FormData, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/Alterar/Upload/${idPedidoCompraArquivo}`, file).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  uploadNovoPdfNotaFiscal(idPedidoCompraNotaFiscal: any, file: FormData, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/UploadNotaFiscal/${idPedidoCompraNotaFiscal}`, file).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== ARQUIVOS (extras) =====
  deleteArquivo(controller: string, idArquivo: any, cb: (r: any) => void): void {
    this.http.delete(`${this.base}/${controller}/Arquivos/${idArquivo}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== PEDIDOS INTERNOS (extras) =====
  getAllPedidosInternos(controller: string, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getAllPedidosInternosRecorrentes(controller: string, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  definirParcelaComoPaga(idParcela: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoInterno/DefinirParcelaPaga/${idParcela}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarParcela(objParcela: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoInterno/AlterarParcela/`, objParcela).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== FLUXO DE CAIXA =====
  obtemFluxoCaixaConsolidado(data: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/ObtemFluxoCaixaConsolidado`, data).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemFluxoCaixaDia(data: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/ObtemFluxosDoDia`, data).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  informarPagamentoOuRecebimentoFluxoCaixa(objDataPagamento: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/InformarPagamentoOuRecebimento`, objDataPagamento).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cadastraNovosValoresOCPI(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/CadastraNovosValoresOCPI`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelFluxoCaixa(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/Excel/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadFluxoCaixaPDF(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/FluxoCaixa/PDF/Download`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== RELATÓRIO FINANCEIRO =====
  downloadPdfRelatorioFinanceiro(cb: (r: any) => void): void {
    this.http.get(`${this.base}/RelatorioFinanceiro/Pdf/Download`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== FATURAMENTO =====
  obtemFaturamentos(objParametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/GetFiltrado`, objParametros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemFaturamentoObra(idObra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Faturamento/GetObra/${idObra}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemDetalheClienteFaturamento(idCliente: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Faturamento/GetClienteDetalhe/${idCliente}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadRelatorioFaturamento(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/Excel/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alteraValorFaturamento(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/AlteraValor`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarDataFaturamento(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/AlteraData`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemValoresAFaturar(objParametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/ObtemValoresAFaturar`, objParametros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemValoresAFaturarDaObra(idObra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Faturamento/ObtemValoresAFaturar/${idObra}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  informaFaturamento(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/InformarFaturamento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  salvarAjusteManualAFaturar(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/AjustaFaturamento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarFaturamento(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Faturamento/CancelarFaturamento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== OBRAS =====
  bloquearDesbloquearObra(idObjeto: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/BloquearDesbloquear/${idObjeto}/${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarObra(idObra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/Cancelar/${idObra}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  habilitarAprovacaoAutomatica(idObjeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/HabilitarAprovacaoAutomatica/${idObjeto}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadContaCorrenteExcel(idObra: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/ContaCorrenteExcel/Download/${idObra}`, null,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadResumoETOExcel(cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/ResumoETOExcel/Download/`, null,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadResumoETOPDF(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/ResumoETOPdf/Download`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadContaCorrentePDF(idObra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/ContaCorrentePDF/Download/${idObra}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadEtoObraPdf(idObra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/EtoObraPdf/Download/${idObra}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelAjusteETO(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/ExcelAjusteETO/Download/`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  uploadAjusteETO(file: FormData, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Obra/Upload/AjusteETO`, file).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemAjustes(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/ObtemAjustes`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadArquivoAjusteETO(id: any, tipo: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Obra/AjusteETO/Download/${id}/${tipo}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== RELATÓRIO CONTROLE ETO =====
  obtemRelatorioControleETO(obraBloqueada: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/RelatorioControleETO/${obraBloqueada}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadRelatorioETOExcelComAjuste(cb: (r: any) => void): void {
    this.http.post(`${this.base}/RelatorioControleETO/ExcelComAjuste/Download/`, null,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadRelatorioETOExcel(cb: (r: any) => void): void {
    this.http.post(`${this.base}/RelatorioControleETO/Excel/Download/`, null,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadRelatorioETOPDF(cb: (r: any) => void): void {
    this.http.get(`${this.base}/RelatorioControleETO/Pdf/Download`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemProximasDatasAjuste(idObra: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/RelatorioControleETO/GetProximasDatasAjuste/${idObra}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  informaAjustes(objAjuste: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/RelatorioControleETO/Ajuste`, objAjuste).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  visualizaComentarioAjuste(idAjuste: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/RelatorioControleETO/VisualizarComentario/${idAjuste}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== AGENDA =====
  obtemAgenda(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/ObtemAgenda`, parametros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemAgendaFaturamento(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/ObtemAgendaFaturamento`, parametros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alteraDefAgenda(id: any, idDef: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/AlteraDEF/${id}/${idDef}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alteraValorAgenda(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/AlteraValor`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  informarPagamentoRecebimentoEmLote(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/InformarPagamentoRecebimentoLote`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarDefEmLote(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/AlteraDefLote`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  alterarDataAgendaEmLote(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/AlteraDataPagamentoLote`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarPagamentoRecebimento(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/CancelarPagamentoRecebimento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  salvarAjusteManual(idFluxoCaixa: any, objetoDatas: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/AjusteManual/${idFluxoCaixa}`, objetoDatas).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPDFAgenda(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/PDF/Download`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPDFAgendaFaturamento(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/PDF/Faturamento/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelAgenda(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/Excel/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelCarimbo(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/ExcelCarimbos/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadExcelAgendaFaturamento(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/Excel/Faturamento/Download/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadNotaFiscalComCarimbo(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Agenda/PDF/Carimbo/Download/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadZipCarimbo(parametros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Agenda/Carimbo/Download/Zip/`, parametros,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== CONTA CORRENTE / SALDOS =====
  obtemSaldosIniciais(controller: string, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== FINANCEIRO =====
  obtemEntradasFinanceiro(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Financeiro/ObtemEntradas`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemEntradasRecusadasFinanceiro(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Financeiro/ObtemEntradasRecusadas`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelaEntradaFinanceiro(entrada: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Financeiro/CancelaEntrada`, entrada).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  autorizaEntradaFinanceiro(entrada: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Financeiro/AutorizaEntrada`, entrada).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== MATERIAIS (extras) =====
  getUnidades(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Material/Unidades`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemMateriaisPaginados(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Material/ObtemMateriaisPaginados`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemHistoricoDeCompra(idMaterial: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Material/ObtemHistoricoDeCompra/${idMaterial}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== FORNECEDORES (extras) =====
  obtemFiliais(idFornecedor: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Fornecedor/ObtemFiliais/${idFornecedor}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== HISTÓRICO =====
  obtemHistorico(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Historico`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemCodigosHistorico(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Historico/RetornaCodigos/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }
}
