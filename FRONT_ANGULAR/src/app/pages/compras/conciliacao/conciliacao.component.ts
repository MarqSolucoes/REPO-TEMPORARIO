import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-conciliacao',
  standalone: false,
  templateUrl: './conciliacao.component.html',
  styleUrls: ['./conciliacao.component.scss']
})
export class ConciliacaoComponent implements OnInit {
  isLoading = false;
  activeTab = 0;

  // Tab 1: Pedidos Em Compra (GET /PedidoCompra/ParaConciliacao)
  pedidosEmCompra: any[] = [];

  // Tab 3: Notas reprovadas Diretoria (GET /PedidoCompra/Conciliacao/ReprovadasDirecao)
  notasReprovadasDiretoria: any[] = [];

  // Tab 4: Notas reprovadas Financeiro (GET /PedidoCompra/Conciliacao/ReprovadasFinanceiro)
  notasReprovadasFinanceiro: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.obtemPedidosEmCompra();
    this.obtemNotasReprovadasDiretoria();
    this.obtemNotasReprovadasFinanceiro();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  formataMoeda(v: number): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
  }

  centroCusto(row: any): string {
    if (row.idCentroCustoDEF != null) {
      return row.centroCustoDEF ? row.centroCustoDEF.descricao : '';
    }
    const cod = row.centroCustoObra ? row.centroCustoObra.codigo : '';
    const cli = row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '';
    return `${cod} - ${cli}`;
  }

  obtemPedidosEmCompra(): void {
    this.activeTab = 0;
    this.isLoading = true;
    this.pedidosEmCompra = [];
    this.api.getPedidosParaConciliacao((result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosEmCompra = result.data;
      }
    });
  }

  obtemNotasReprovadasDiretoria(): void {
    this.activeTab = 1;
    this.isLoading = true;
    this.notasReprovadasDiretoria = [];
    this.api.obtemNotasFiscaisReprovadasDiretoria((result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.notasReprovadasDiretoria = result.data;
      }
    });
  }

  obtemNotasReprovadasFinanceiro(): void {
    this.activeTab = 2;
    this.isLoading = true;
    this.notasReprovadasFinanceiro = [];
    this.api.obtemNotasFiscaisReprovadasFinanceiro((result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.notasReprovadasFinanceiro = result.data;
      }
    });
  }
}

