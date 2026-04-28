import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-faturamento',
  standalone: false,
  templateUrl: './faturamento.component.html',
  styleUrls: ['./faturamento.component.scss']
})
export class FaturamentoComponent implements OnInit {
  isLoading = false;
  activeTab = 0;

  // Filtros
  filtro_Obras: any[] = [];
  filtro_Clientes: any[] = [];
  filtro_ObraSelecionada: any = null;
  filtro_ClienteSelecionado: any = null;
  filtro_NumeroNF = '';
  filtro_Observacao = '';
  filtro_DataFaturamentoInicial: string | null = null;
  filtro_DataFaturamentoFinal: string | null = null;
  filtro_DataRecebimentoInicial: string | null = null;
  filtro_DataRecebimentoFinal: string | null = null;
  filtro_StatusFaturamento = [
    { id: 0, descricao: 'Todos' },
    { id: -1, descricao: 'A Faturar' },
    { id: 1, descricao: 'Faturado Recebido' },
    { id: 2, descricao: 'Faturado a Receber' },
    { id: 3, descricao: 'Cancelado' }
  ];
  filtro_StatusFaturamentoSelecionado: any = { id: 0, descricao: 'Todos' };

  faturamentos: any[] = [];
  medicoes: any[] = [];

  // Modal data faturamento
  modalDataFaturamento_Exibir = false;
  modalDataFaturamento_Faturamento: any = null;
  modalDataFaturamento_DataFaturamento: string | null = null;
  modalDataFaturamento_ValorRecebido = 0;
  modalDataFaturamento_DataProximoRecebimento: string | null = null;
  modalDataFaturamento_TipoTotal = true;

  // Modal arquivos
  modalArquivos_Exibir = false;
  modalArquivos_Titulo = 'Arquivos';
  modalArquivos_Arquivos: any[] = [];
  modalArquivos_IdFaturamento: any = null;

  get totalValorBruto(): number {
    return (this.medicoes || []).reduce((s, i) => s + (Number(i.valor) || 0), 0);
  }
  get totalValorLiquido(): number {
    return (this.medicoes || []).reduce((s, i) => s + (Number(i.valorLiquido) || 0), 0);
  }

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.listaObras();
    this.listaClientes();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  formataDataSemHora(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  listaObras(): void {
    this.filtro_Obras = [];
    this.api.getAll('Obra', false, (result) => {
      if (result.status === 200) {
        this.filtro_Obras = (result.data || []).filter((x: any) => !x.cancelada);
      }
    });
  }

  listaClientes(): void {
    this.filtro_Clientes = [];
    this.api.getAll('Cliente', false, (result) => {
      if (result.status === 200) this.filtro_Clientes = result.data;
    });
  }

  obtemFaturamentos(): void {
    this.isLoading = true;
    const obj: any = {
      idObra: this.filtro_ObraSelecionada ? this.filtro_ObraSelecionada.id : null,
      idCliente: this.filtro_ClienteSelecionado ? this.filtro_ClienteSelecionado.id : null,
      idStatusFaturamento: this.filtro_StatusFaturamentoSelecionado ? this.filtro_StatusFaturamentoSelecionado.id : 0,
      numeroNotaFiscal: this.filtro_NumeroNF,
      observacao: this.filtro_Observacao,
      dataFaturamentoInicial: this.filtro_DataFaturamentoInicial,
      dataFaturamentoFinal: this.filtro_DataFaturamentoFinal,
      dataRecebimentoInicial: this.filtro_DataRecebimentoInicial,
      dataRecebimentoFinal: this.filtro_DataRecebimentoFinal
    };
    this.api.obtemFaturamentos(obj, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.faturamentos = result.data.faturamentosDTO || result.data;
      }
    });
    this.obtemValoresAFaturar();
  }

  obtemValoresAFaturar(): void {
    const obj: any = {
      idObra: this.filtro_ObraSelecionada ? this.filtro_ObraSelecionada.id : null,
      idCliente: this.filtro_ClienteSelecionado ? this.filtro_ClienteSelecionado.id : null,
      numeroNotaFiscal: this.filtro_NumeroNF,
      dataFaturamentoInicial: this.filtro_DataFaturamentoInicial,
      dataFaturamentoFinal: this.filtro_DataFaturamentoFinal
    };
    this.api.obtemValoresAFaturar(obj, (result) => {
      if (result.status === 200) this.medicoes = result.data;
    });
  }

  limparFiltros(): void {
    this.filtro_ObraSelecionada = null;
    this.filtro_ClienteSelecionado = null;
    this.filtro_NumeroNF = '';
    this.filtro_Observacao = '';
    this.filtro_DataFaturamentoInicial = null;
    this.filtro_DataFaturamentoFinal = null;
    this.filtro_DataRecebimentoInicial = null;
    this.filtro_DataRecebimentoFinal = null;
    this.filtro_StatusFaturamentoSelecionado = this.filtro_StatusFaturamento[0];
  }

  abrirModalDataFaturamento(fat: any): void {
    this.modalDataFaturamento_Faturamento = fat;
    this.modalDataFaturamento_DataFaturamento = null;
    this.modalDataFaturamento_ValorRecebido = 0;
    this.modalDataFaturamento_DataProximoRecebimento = null;
    this.modalDataFaturamento_TipoTotal = true;
    this.modalDataFaturamento_Exibir = true;
  }

  salvarDataFaturamento(): void {
    const obj: any = {
      id: this.modalDataFaturamento_Faturamento.id,
      dataFaturamento: this.modalDataFaturamento_DataFaturamento,
      valorRecebido: this.modalDataFaturamento_TipoTotal
        ? (this.modalDataFaturamento_Faturamento?.valorLiquido || 0)
        : this.modalDataFaturamento_ValorRecebido,
      dataProximoRecebimento: this.modalDataFaturamento_DataProximoRecebimento,
      tipoTotal: this.modalDataFaturamento_TipoTotal
    };
    this.isLoading = true;
    this.api.alteraValorFaturamento(obj, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.modalDataFaturamento_Exibir = false;
        this.obtemFaturamentos();
      }
    });
  }

  abrirModalArquivos(fat: any): void {
    this.modalArquivos_IdFaturamento = fat.id;
    this.modalArquivos_Arquivos = fat.arquivos || [];
    this.modalArquivos_Titulo = `Arquivos - NF ${fat.numeroNF || ''}`;
    this.modalArquivos_Exibir = true;
  }

  downloadArquivo(id: any): void {
    this.isLoading = true;
    this.api.downloadFile('Faturamento', id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }

  downloadRelatorio(): void {
    const obj: any = {
      idObra: this.filtro_ObraSelecionada ? this.filtro_ObraSelecionada.id : null,
      idCliente: this.filtro_ClienteSelecionado ? this.filtro_ClienteSelecionado.id : null,
      idStatusFaturamento: this.filtro_StatusFaturamentoSelecionado?.id || 0,
      numeroNotaFiscal: this.filtro_NumeroNF,
      dataFaturamentoInicial: this.filtro_DataFaturamentoInicial,
      dataFaturamentoFinal: this.filtro_DataFaturamentoFinal,
      dataRecebimentoInicial: this.filtro_DataRecebimentoInicial,
      dataRecebimentoFinal: this.filtro_DataRecebimentoFinal
    };
    this.api.downloadRelatorioFaturamento(obj, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao gerar relatório', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }
}
