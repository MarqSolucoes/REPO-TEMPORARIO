import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-ordem-compra',
  standalone: false,
  templateUrl: './ordem-compra.component.html',
  styleUrls: ['./ordem-compra.component.scss']
})
export class OrdemCompraComponent implements OnInit {
  isLoading = false;
  usuarioDTO: any = {};
  quantidades: any = null;

  // Tab data arrays (mirroring Vue data properties)
  solicitacoesParaValidacao: any[] = [];
  solicitacoesEmCotacao: any[] = [];
  pedidosParaAprovacao: any[] = [];
  pedidosEmCompra: any[] = [];
  solicitacoesCanceladas: any[] = [];
  pedidosCancelados: any[] = [];
  solicitacoesDevolvidasDiretoria: any[] = [];

  activeTab = 0;

  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.obtemQuantidades();
    this.obtemSolicitacoesCompraEmCotacao();

    const tag = this.route.snapshot.params['tag'] || '';
    if (tag === 'ParaAprovacao') {
      this.obtemSolicitacoesCompraParaAprovacao();
    }
  }

  trackByIndex(i: number): number { return i; }

  formataMoeda(v: number): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
  }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  centroCusto(row: any): string {
    if (row.idCentroCustoDEF != null) {
      return row.centroCustoDEF ? row.centroCustoDEF.descricao : '';
    }
    const cod = row.centroCustoObra ? row.centroCustoObra.codigo : '';
    const cli = row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '';
    return `${cod} - ${cli}`;
  }

  obtemQuantidades(): void {
    this.api.obtemQuantidades((result) => {
      if (result.status === 200) this.quantidades = result.data;
    });
  }

  // ===== Tab 1: Para Validação — SolicitacaoCompra/Status/1 =====
  obtemSolicitacoesParaValidacao(): void {
    this.activeTab = 0;
    this.isLoading = true;
    this.solicitacoesParaValidacao = [];
    this.obtemQuantidades();
    this.api.getSolicitacoesCompraStatus(1, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.solicitacoesParaValidacao = result.data;
      }
    });
  }

  // ===== Tab 2: Em Cotação — SolicitacaoCompra/Status/2 =====
  obtemSolicitacoesCompraEmCotacao(): void {
    this.activeTab = 1;
    this.isLoading = true;
    this.solicitacoesEmCotacao = [];
    this.obtemQuantidades();
    this.api.getSolicitacoesCompraStatus(2, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.solicitacoesEmCotacao = result.data;
      }
    });
  }

  // ===== Tab 3: Para Aprovação — SolicitacaoCompra/Status/3 =====
  obtemSolicitacoesCompraParaAprovacao(): void {
    this.activeTab = 2;
    this.isLoading = true;
    this.pedidosParaAprovacao = [];
    this.obtemQuantidades();
    this.api.getSolicitacoesCompraStatus(3, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosParaAprovacao = result.data;
      }
    });
  }

  // ===== Tab 4: Em Compra — PedidoCompra/Status/1 =====
  obtemPedidosEmCompra(): void {
    this.activeTab = 3;
    this.isLoading = true;
    this.pedidosEmCompra = [];
    this.obtemQuantidades();
    this.api.getPedidosCompraStatus(1, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosEmCompra = result.data;
      }
    });
  }

  // ===== Tab 5: Finalizados/Cancelados Pedidos — PedidoCompra/Status/2 + 3 =====
  obtemPedidosFinalizadosCancelados(): void {
    this.activeTab = 4;
    this.isLoading = true;
    this.pedidosCancelados = [];
    this.obtemQuantidades();
    this.api.getPedidosCompraStatus(2, (resultA) => {
      if (resultA.status !== 200) {
        this.isLoading = false;
        Swal.fire({ title: '', text: resultA.message, icon: 'error' });
      } else {
        this.api.getPedidosCompraStatus(3, (resultB) => {
          this.isLoading = false;
          if (resultB.status !== 200) {
            Swal.fire({ title: '', text: resultB.message, icon: 'error' });
          } else {
            this.pedidosCancelados = [...resultA.data, ...resultB.data];
          }
        });
      }
    });
  }

  // ===== Tab 6: Finalizados/Cancelados Solicitações — SolicitacaoCompra/Status/4 + 5 =====
  obtemSolicitacoesFinalizadasCanceladas(): void {
    this.activeTab = 5;
    this.isLoading = true;
    this.solicitacoesCanceladas = [];
    this.obtemQuantidades();
    this.api.getSolicitacoesCompraStatus(4, (resultA) => {
      if (resultA.status !== 200) {
        this.isLoading = false;
        Swal.fire({ title: '', text: resultA.message, icon: 'error' });
      } else {
        this.api.getSolicitacoesCompraStatus(5, (resultB) => {
          this.isLoading = false;
          if (resultB.status !== 200) {
            Swal.fire({ title: '', text: resultB.message, icon: 'error' });
          } else {
            this.solicitacoesCanceladas = [...resultA.data, ...resultB.data];
          }
        });
      }
    });
  }

  // ===== Tab 7: Devolvidas Diretoria — SolicitacaoCompra/Status/7 =====
  obtemSolicitacoesDevolvidasDiretoria(): void {
    this.activeTab = 6;
    this.isLoading = true;
    this.solicitacoesDevolvidasDiretoria = [];
    this.obtemQuantidades();
    this.api.getSolicitacoesCompraStatus(7, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.solicitacoesDevolvidasDiretoria = result.data;
      }
    });
  }

  verCotacao(id: any): void {
    this.router.navigate(['/app/compras/cotacao', id]);
  }
}

