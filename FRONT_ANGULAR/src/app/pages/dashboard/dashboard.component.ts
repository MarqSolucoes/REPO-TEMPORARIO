import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  usuarioDTO: any = null;

  modalNovoPedidoCompraTipoPedido_Exibir = false;
  modalMinhasSolicitacoes_Exibir = false;
  modalNovoPedidoInterno_Exibir = false;
  modalMeusPedidosInternos_Exibir = false;
  modalAprovacaoNF_Exibir = false;
  modalAprovacaoPI_Exibir = false;
  modalAprovacaoSC_Exibir = false;

  modalMinhasSolicitacoes_Solicitacoes: any[] = [];
  modalMinhasSolicitacoes_Count = 0;
  modalMinhasSolicitacoes_Skip = 0;
  modalMinhasSolicitacoes_Take = 10;
  modalMinhasSolicitacoes_PaginaAtual = 1;
  modalMinhasSolicitacoes_FiltroObrasSelecionadas: any[] = [];
  modalMinhasSolicitacoes_FiltroCodigo = '';
  modalMinhasSolicitacoes_FiltroTitulo = '';
  modalMinhasSolicitacoes_FiltroDataInicio: any = null;
  modalMinhasSolicitacoes_FiltroDataFim: any = null;
  modalMinhasSolicitacoes_FiltroStatusSelecionados: any[] = [];
  modalMinhasSolicitacoes_OpcoesStatus: any[] = [];

  modalMeusPedidosInternos_Pedidos: any[] = [];

  modalAprovacaoNF_NotasFiscais: any[] = [];
  pedidosInternosParaAprovacao: any[] = [];
  solicitacoesCompraParaAprovacao: any[] = [];
  modalAprovacaoCotacao_Cotacoes = 0;

  obras: any[] = [];

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    try {
      this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || 'null');
    } catch { this.usuarioDTO = null; }

    this.listaObras();
    this.obtemCotacoesParaAprovacao();
    this.obtemSolicitacoesCompraParaValidacao();
    this.listaMeusPedidosInternos();

    if (this.usuarioDTO) {
      if (this.usuarioDTO.globalSolicitacaoCompra) {
        this.listaMinhasSolicitacoes();
      }
      if (this.usuarioDTO.globalAprovaCotacao) {
        this.obtemNotasFiscaisParaAprovacao();
        this.obtemPedidosInternosParaAprovacao();
      }
    }
  }

  formataData(data: any): string {
    if (!data) return '';
    const d = new Date(data);
    return d.toLocaleDateString('pt-BR') + ' ' + d.toLocaleTimeString('pt-BR');
  }

  formataDataSemHora(data: any): string {
    if (!data) return '';
    const d = new Date(data);
    return d.toLocaleDateString('pt-BR');
  }

  formataMoeda(valor: number): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor || 0);
  }

  listaObras(): void {
    this.api.getAll('Obra', true, (result) => {
      if (result.status === 200) {
        this.obras = result.data.filter((x: any) => x.cancelada === false);
      }
    });
  }

  listaMinhasSolicitacoes(): void {
    this.modalMinhasSolicitacoes_PaginaAtual = 1;
    this._carregarSolicitacoesComEstadoAtual();
  }

  _buildPayloadMinhasSolicitacoes(skip: number): any {
    return {
      idsObra: (this.modalMinhasSolicitacoes_FiltroObrasSelecionadas || []).map((o: any) => o.id),
      idsStatus: (this.modalMinhasSolicitacoes_FiltroStatusSelecionados || []).map((s: any) => s.id),
      codigo: this.modalMinhasSolicitacoes_FiltroCodigo?.trim() || null,
      titulo: this.modalMinhasSolicitacoes_FiltroTitulo?.trim() || null,
      dataSolicitacaoInicial: this.modalMinhasSolicitacoes_FiltroDataInicio || null,
      dataSolicitacaoFinal: this.modalMinhasSolicitacoes_FiltroDataFim || null,
      skip,
      take: this.modalMinhasSolicitacoes_Take
    };
  }

  _carregarSolicitacoesComEstadoAtual(): void {
    const skip = (this.modalMinhasSolicitacoes_PaginaAtual - 1) * this.modalMinhasSolicitacoes_Take;
    this.modalMinhasSolicitacoes_Skip = skip;
    const filtros = this._buildPayloadMinhasSolicitacoes(skip);
    this.isLoading = true;
    this.api.getSolicitacoesCompraUsuario(filtros, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire('', result.message, 'error');
      } else {
        this.modalMinhasSolicitacoes_Solicitacoes = result.data.solicitacoes || [];
        this.modalMinhasSolicitacoes_Count = result.data.count || 0;
      }
    });
  }

  filtrarMinhasSolicitacoes(): void {
    this.modalMinhasSolicitacoes_PaginaAtual = 1;
    this._carregarSolicitacoesComEstadoAtual();
  }

  mudarPaginaMinhasSolicitacoes(pagina: number): void {
    this.modalMinhasSolicitacoes_PaginaAtual = pagina;
    this._carregarSolicitacoesComEstadoAtual();
  }

  limparFiltrosMinhasSolicitacoes(): void {
    this.modalMinhasSolicitacoes_FiltroObrasSelecionadas = [];
    this.modalMinhasSolicitacoes_FiltroCodigo = '';
    this.modalMinhasSolicitacoes_FiltroTitulo = '';
    this.modalMinhasSolicitacoes_FiltroDataInicio = null;
    this.modalMinhasSolicitacoes_FiltroDataFim = null;
    this.modalMinhasSolicitacoes_FiltroStatusSelecionados = [];
    this.listaMinhasSolicitacoes();
  }

  abrirModalMinhasSolicitacoes(): void {
    this.modalMinhasSolicitacoes_Exibir = true;
    this.listaMinhasSolicitacoes();
  }

  listaMeusPedidosInternos(): void {
    this.api.getPedidosInternosUsuario((result) => {
      if (result.status === 200) {
        this.modalMeusPedidosInternos_Pedidos = result.data;
      }
    });
  }

  obtemNotasFiscaisParaAprovacao(): void {
    this.api.obtemNotasFiscaisParaAprovacao((result) => {
      if (result.status === 200) {
        this.modalAprovacaoNF_NotasFiscais = result.data;
      }
    });
  }

  obtemCotacoesParaAprovacao(): void {
    this.api.obtemCotacoesParaAprovacao((result) => {
      if (result.status === 200) {
        this.modalAprovacaoCotacao_Cotacoes = result.data;
      }
    });
  }

  obtemPedidosInternosParaAprovacao(): void {
    if (!this.usuarioDTO) return;
    this.api.obtemPedidosInternosParaAprovacao(this.usuarioDTO.id, (result) => {
      if (result.status === 200) {
        this.pedidosInternosParaAprovacao = result.data;
      }
    });
  }

  obtemSolicitacoesCompraParaValidacao(): void {
    this.api.getSolicitacoesCompraStatus(1, (result) => {
      if (result.status === 200) {
        this.solicitacoesCompraParaAprovacao = result.data;
      }
    });
  }

  redirecionaAprovarCotacoes(): void {
    this.router.navigate(['/app/compras/ordemCompra/ParaAprovacao']);
  }

  aprovarReprovarPI(idPedidoInterno: number, valor: boolean): void {
    this.api.aprovarReprovarPedidoInterno(idPedidoInterno, valor, (result) => {
      if (result.status !== 200) {
        Swal.fire('', result.message, 'error');
      } else {
        this.obtemPedidosInternosParaAprovacao();
      }
    });
  }

  aprovarReprovarNotaFiscal(id: number, aprovada: boolean): void {
    Swal.fire({
      title: aprovada ? 'Aprovar nota fiscal?' : 'Reprovar nota fiscal?',
      text: 'Confirme para continuar.',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: aprovada ? 'Sim, aprovar' : 'Sim, reprovar',
      cancelButtonText: 'Cancelar',
    }).then((res) => {
      if (!res.isConfirmed) return;
      this.isLoading = true;
      this.api.aprovarReprovarNotaFiscal(id, aprovada, (result) => {
        this.isLoading = false;
        if (!result || result.status !== 200) {
          Swal.fire('', result?.message || 'Erro ao processar.', 'error');
        } else {
          Swal.fire('', 'Operação concluída com sucesso.', 'success');
          this.obtemNotasFiscaisParaAprovacao();
        }
      });
    });
  }

  get minhasSolicitacoes_InicioPagina(): number {
    return (this.modalMinhasSolicitacoes_PaginaAtual - 1) * this.modalMinhasSolicitacoes_Take + 1;
  }

  get minhasSolicitacoes_FimPagina(): number {
    return Math.min(
      this.modalMinhasSolicitacoes_PaginaAtual * this.modalMinhasSolicitacoes_Take,
      this.modalMinhasSolicitacoes_Count
    );
  }

  trackByIndex(index: number): number {
    return index;
  }
}
