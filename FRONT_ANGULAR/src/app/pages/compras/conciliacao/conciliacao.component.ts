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

  // Tab 0: Pedidos Em Compra (GET /PedidoCompra/ParaConciliacao)
  pedidosEmCompra: any[] = [];

  // Tab 1: Notas Conciliadas — com filtros
  pedidosConciliados: any[] = [];
  filtro_Fornecedores: any[] = [];
  filtro_FornecedoresSelecionados: any[] = [];
  filtro_NumeroPedido = '';
  filtro_Valor = 0;
  filtro_NumeroNF = '';
  filtro_DataInicial: string | null = null;
  filtro_DataFinal: string | null = null;

  // Tab 2: Notas reprovadas Diretoria
  notasReprovadasDiretoria: any[] = [];

  // Tab 3: Notas reprovadas Financeiro
  notasReprovadasFinanceiro: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.obtemPedidosEmCompra();
    this.obtemNotasReprovadasDiretoria();
    this.obtemNotasReprovadasFinanceiro();
    this.listaFornecedores();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  formataMoeda(v: number): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v || 0);
  }

  statusNF(aprovada: boolean | null): string {
    if (aprovada == null) return 'Pendente';
    return aprovada ? 'Aprovada' : 'Reprovada';
  }

  centroCusto(row: any): string {
    if (row.idCentroCustoDEF != null) {
      return row.centroCustoDEF ? row.centroCustoDEF.descricao : '';
    }
    const cod = row.centroCustoObra ? row.centroCustoObra.codigo : '';
    const cli = row.centroCustoObra && row.centroCustoObra.cliente ? row.centroCustoObra.cliente.nomeFantasia : '';
    return `${cod} - ${cli}`;
  }

  listaFornecedores(): void {
    this.api.getAll('Fornecedor', false, (result) => {
      if (result.status === 200) this.filtro_Fornecedores = result.data;
    });
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

  // ===== Tab 1: Notas Conciliadas =====
  obtemNotasConciliadas(): void {
    this.activeTab = 1;
    this.isLoading = true;
    this.pedidosConciliados = [];

    const objetoPesquisa: any = {
      idsFornecedores: (this.filtro_FornecedoresSelecionados || []).map((f: any) => f.id),
      numeroPedido: this.filtro_NumeroPedido,
      numeroNotaFiscal: this.filtro_NumeroNF,
      valor: this.filtro_Valor,
      dataInicial: this.filtro_DataInicial,
      dataFinal: this.filtro_DataFinal
    };

    this.api.obtemNotasConciliadas(objetoPesquisa, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosConciliados = result.data;
      }
    });
  }

  limpaFiltrosConciliadas(): void {
    this.filtro_FornecedoresSelecionados = [];
    this.filtro_NumeroPedido = '';
    this.filtro_Valor = 0;
    this.filtro_NumeroNF = '';
    this.filtro_DataInicial = null;
    this.filtro_DataFinal = null;
  }

  obtemNotasReprovadasDiretoria(): void {
    this.activeTab = 2;
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
    this.activeTab = 3;
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


