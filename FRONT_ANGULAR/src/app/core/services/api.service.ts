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
    this.http.post(`${this.base}/${controller}/HabilitarDesabilitar?id=${id}&habilitar=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== RASCUNHO =====
  obtemRascunhos(cb: (r: any) => void): void {
    this.http.get(`${this.base}/Rascunho`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  salvaRascunho(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Rascunho`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  atualizaRascunho(objeto: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/Rascunho`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  excluiRascunho(id: any, cb: (r: any) => void): void {
    this.http.delete(`${this.base}/Rascunho/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== MATERIAIS =====
  buscaMaterialPorTexto(material: string, cb: (r: any) => void): void {
    this.http.get(`${this.base}/Material/BuscaPorTexto?material=${material}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== SOLICITAÇÕES DE COMPRA =====
  getSolicitacoesCompraStatus(idStatus: number, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra?idStatus=${idStatus}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getSolicitacoesCompraUsuario(filtros: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/SolicitacoesUsuario`, filtros).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  cancelarSolicitacaoCompra(id: any, motivo: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Cancelar/${id}?motivo=${motivo}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  validarSolicitacaoCompra(id: any, idComprador: number, cb: (r: any) => void): void {
    this.http.post(`${this.base}/SolicitacaoCompra/Validar/${id}?idComprador=${idComprador}`, {}).subscribe({
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
    this.http.get(`${this.base}/PedidoInterno/PedidosUsuario`).subscribe({
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
    this.http.get(`${this.base}/PedidoInterno/ParaAprovacao/${idUsuario}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  aprovarReprovarPedidoInterno(id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoInterno/AprovarReprovar/${id}?valor=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== NOTAS FISCAIS =====
  obtemNotasFiscaisParaAprovacao(cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/NotaFiscal/ParaAprovacao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemCotacoesParaAprovacao(cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/QuantidadeParaAprovacao`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  aprovarReprovarNotaFiscal(id: any, aprovada: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/AprovarReprovar/${id}?aprovada=${aprovada}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  editarDataVencimentoNF(objeto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/PedidoCompra/NotaFiscal/EditarDataVencimento`, objeto).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  obtemNotasFiscaisAlteracaoData(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoCompra/NotaFiscal/AlteracaoData/${id}`).subscribe({
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
    this.http.get(`${this.base}/PedidoCompra/PDF/${id}?exibirDataPagamento=${exibirDataPagamento}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfArquivoSolicitacaoCompra(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/SolicitacaoCompra/PDF/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  downloadPdfPedidoInterno(id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/PedidoInterno/PDF/${id}`,
      { responseType: 'blob', observe: 'response' }).subscribe({
      next: (resp: any) => cb({ status: 200, message: '', data: resp.body, contentType: resp.headers.get('content-type') }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== COMENTARIOS =====
  obtemComentarios(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/Comentarios/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  enviarComentario(controller: string, id: any, texto: string, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/Comentarios/${id}`, { texto }).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  // ===== OUTROS =====
  listaFornecedoresFiltrados(filtro: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Fornecedor/Filtrados`, filtro).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  habilitarDesabilitarLogin(idUsuario: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/Usuario/HabilitarDesabilitarLogin?id=${idUsuario}&habilitarDesabilitar=${valor}`, {}).subscribe({
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
    this.http.post(`${this.base}/PedidoCompra/Cancelar/${id}?motivo=${motivo}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }
}
