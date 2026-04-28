import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-fluxo-caixa-consolidado',
  standalone: false,
  templateUrl: './fluxo-caixa-consolidado.component.html',
  styleUrls: ['./fluxo-caixa-consolidado.component.scss']
})
export class FluxoCaixaConsolidadoComponent implements OnInit {
  isLoading = false;

  mesSelecionado = '';
  mesFinalSelecionado = '';
  defs: any[] = [];
  saldosIniciais: any[] = [];
  fluxoCaixaConsolidado: any[] = [];

  // KPIs
  get kpiDespesasRealizadas(): number {
    return (this.fluxoCaixaConsolidado || [])
      .filter(r => r && r.dia)
      .reduce((s, r) => s + (Number(r.totalDiario) || 0), 0);
  }
  get kpiReceitasRealizadas(): number {
    return (this.fluxoCaixaConsolidado || [])
      .filter(r => r && r.dia)
      .reduce((s, r) => s + (Number(r.aReceberFaturado) || 0) + (Number(r.estornos) || 0), 0);
  }
  get kpiSaldoFinalRealizado(): number {
    const rows = this.fluxoCaixaConsolidado || [];
    if (!rows.length) return 0;
    const last = [...rows].reverse().find(r => r.saldo != null);
    return last ? Number(last.saldo) : 0;
  }

  // Modal detalhe
  modalDetalhe_Exibir = false;
  modalDetalhe_Dia: any = null;
  modalDetalhe_Fluxos: any[] = [];
  modalDetalhe_Credito = '';
  modalDataPagamentoRecebimento_IdFluxoCaixa: any = null;
  modalDataPagamentoRecebimento_NovaData: string | null = null;

  // Modal saldos iniciais
  modalSaldoInicial_Exibir = false;

  // Modal edição saldo inicial
  modalSaldoInicialEdicao_Exibir = false;
  modalSaldoInicialEdicao_SaldoInicial: any = null;
  modalSaldoInicialEdicao_Saldo = 0;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.listaDEFs();
    this.listaSaldosIniciais();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  fmt(v: any, saldo = false): string {
    const n = Number(v || 0);
    if (!saldo && n === 0) return '';
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(n);
  }

  formatBRL(v: any): string {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(Number(v || 0));
  }

  rowClass(row: any): string {
    if (!row.dia) {
      const obs = (row.diaObservacao || '').toLowerCase();
      if (obs.includes('total p') || obs.includes('despesa')) return 'table-TotalP';
      if (obs.includes('total r') || obs.includes('receita')) return 'table-TotalR';
    }
    return '';
  }

  listaDEFs(): void {
    this.api.getAll('DEF', false, (result) => {
      if (result.status === 200) this.defs = result.data;
    });
  }

  listaSaldosIniciais(): void {
    this.saldosIniciais = [];
    this.api.obtemSaldosIniciais('FluxoCaixaSaldoInicial', (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.saldosIniciais = result.data;
      }
    });
  }

  obtemFluxoCaixaConsolidado(): void {
    this.isLoading = true;
    const obj = { mesAno: this.mesSelecionado, mesAnoFinal: this.mesFinalSelecionado };
    this.api.obtemFluxoCaixaConsolidado(obj, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.fluxoCaixaConsolidado = result.data;
      }
    });
  }

  abrirDetalhe(row: any, credito: boolean): void {
    this.modalDetalhe_Credito = credito ? 'Créditos' : 'Débitos';
    this.modalDetalhe_Dia = row;
    this.isLoading = true;
    const obj = { mesAno: this.mesSelecionado, dia: row.dia, credito };
    this.api.obtemFluxoCaixaDia(obj, (result) => {
      this.isLoading = false;
      if (result.status === 200) {
        this.modalDetalhe_Fluxos = result.data;
        this.modalDetalhe_Exibir = true;
      }
    });
  }

  informarPagamentoOuRecebimento(): void {
    this.isLoading = true;
    const obj = {
      id: this.modalDataPagamentoRecebimento_IdFluxoCaixa,
      data: this.modalDataPagamentoRecebimento_NovaData
    };
    this.api.informarPagamentoOuRecebimentoFluxoCaixa(obj, (result) => {
        this.isLoading = false;
        if (result.status !== 200) {
          Swal.fire({ title: '', text: result.message, icon: 'error' });
        } else {
          this.modalDetalhe_Exibir = false;
          this.obtemFluxoCaixaConsolidado();
        }
      }
    );
  }

  abrirEdicaoSaldoInicial(si: any): void {
    this.modalSaldoInicialEdicao_SaldoInicial = si;
    this.modalSaldoInicialEdicao_Saldo = si.saldo;
    this.modalSaldoInicialEdicao_Exibir = true;
  }

  salvarSaldoInicial(): void {
    this.modalSaldoInicialEdicao_SaldoInicial.saldo = this.modalSaldoInicialEdicao_Saldo;
    this.api.put('FluxoCaixaSaldoInicial', this.modalSaldoInicialEdicao_SaldoInicial, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.modalSaldoInicialEdicao_Exibir = false;
        this.modalSaldoInicial_Exibir = false;
        this.listaSaldosIniciais();
      }
    });
  }

  downloadExcel(): void {
    const obj = { mesAno: this.mesSelecionado, mesAnoFinal: this.mesFinalSelecionado };
    this.api.downloadExcelFluxoCaixa(obj, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao baixar Excel', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }

  downloadPDF(): void {
    const obj = { mesAno: this.mesSelecionado, mesAnoFinal: this.mesFinalSelecionado };
    this.api.downloadFluxoCaixaPDF(obj, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao baixar PDF', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }
}
