import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-notas-fiscais',
  standalone: false,
  templateUrl: './notas-fiscais.component.html',
  styleUrls: ['./notas-fiscais.component.scss']
})
export class NotasFiscaisComponent implements OnInit {
  isLoading = false;

  // Filter data
  obras: any[] = [];
  pedidos: any[] = [];
  fornecedores: any[] = [];

  obraSelecionada: any = null;
  pedidoSelecionado: any = null;
  fornecedorSelecionado: any = null;
  statusSelecionado: any = null;
  pagamentoSelecionado: any = null;
  dataPagamentoInicial: string | null = null;
  dataPagamentoFinal: string | null = null;
  numeroNota = '';

  statusOpcoes = [
    { id: 1, descricao: 'Pendente' },
    { id: 2, descricao: 'Aprovada' },
    { id: 3, descricao: 'Reprovada' }
  ];
  pagamentoOpcoes = [
    { id: 1, descricao: 'Pago' },
    { id: 2, descricao: 'A Pagar' }
  ];

  notasFiscais: any[] = [];

  // Modal pagamento
  modalPagamentoNF_Exibir = false;
  modalPagamentoNF_titulo = '';
  modalPagamentoNF_NotaFiscal: any = null;
  modalPagamentoNF_ValorBruto = 0;
  modalPagamentoNF_MaterialAbatido = 0;
  modalPagamentoNF_BaseCalculo = 0;
  modalPagamentoNF_IR = 0;
  modalPagamentoNF_Art30 = 0;
  modalPagamentoNF_INSS = 0;
  modalPagamentoNF_ISS = 0;
  modalPagamentoNF_ValorLiquido = 0;
  modalPagamentoNF_IRDataPagamento: string | null = null;
  modalPagamentoNF_Art30DataPagamento: string | null = null;
  modalPagamentoNF_INSSDataPagamento: string | null = null;
  modalPagamentoNF_ISSDataPagamento: string | null = null;

  impostoIR = [{ id: 1, descricao: '1.0%', valor: 1.0 }, { id: 2, descricao: '1.5%', valor: 1.5 }];
  impostoArt30 = [{ id: 1, descricao: '4,65%', valor: 4.65 }];
  impostoINSS = [{ id: 1, descricao: '3,5%', valor: 3.5 }, { id: 2, descricao: '11%', valor: 11 }];
  basePagamento = [{ id: 1, descricao: 'Bruto' }, { id: 2, descricao: 'Base Cálculo' }];
  basePagamentoArt30 = [{ id: 1, descricao: 'Bruto' }];

  modalPagamentoNF_IRSelecionado: any = null;
  modalPagamentoNF_IRBaseSelecionada: any = null;
  modalPagamentoNF_Art30Selecionado: any = null;
  modalPagamentoNF_Art30BaseSelecionada: any = null;
  modalPagamentoNF_INSSSelecionado: any = null;
  modalPagamentoNF_INSSBaseSelecionada: any = null;
  modalPagamentoNF_ISSBaseSelecionada: any = null;
  impostoISS = 1.0;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.listaItens('Obra');
    this.listaItens('Pedido');
    this.listaItens('Fornecedor');
    this.dataPagamentoInicial = new Date().toISOString().slice(0, 10);
    this.obtemNotasFiltradas(false);
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  listaItens(item: string): void {
    this.api.getItemFiltro(item, (result) => {
      if (result.status === 200) {
        if (item === 'Obra') this.obras = result.data;
        else if (item === 'Pedido') this.pedidos = result.data;
        else if (item === 'Fornecedor') this.fornecedores = result.data;
      }
    });
  }

  obtemNotasFiltradas(limpaFiltro: boolean): void {
    this.isLoading = true;
    let objFiltro: any = {
      idObra: null, idPedido: null, idFornecedor: null,
      idStatus: null, idPagamento: null,
      dataPagamentoInicial: null, dataPagamentoFinal: null,
      numeroNotaFiscal: ''
    };
    if (!limpaFiltro) {
      objFiltro.numeroNotaFiscal = this.numeroNota;
      if (this.obraSelecionada) objFiltro.idObra = this.obraSelecionada.id;
      if (this.pedidoSelecionado) objFiltro.idPedido = this.pedidoSelecionado.id;
      if (this.fornecedorSelecionado) objFiltro.idFornecedor = this.fornecedorSelecionado.id;
      if (this.statusSelecionado) objFiltro.idStatus = this.statusSelecionado.id;
      if (this.pagamentoSelecionado) objFiltro.idPagamento = this.pagamentoSelecionado.id;
      objFiltro.dataPagamentoInicial = this.dataPagamentoInicial;
      objFiltro.dataPagamentoFinal = this.dataPagamentoFinal;
    } else {
      this.obraSelecionada = null;
      this.pedidoSelecionado = null;
      this.statusSelecionado = null;
      this.pagamentoSelecionado = null;
      this.fornecedorSelecionado = null;
      this.dataPagamentoInicial = new Date().toISOString().slice(0, 10);
      this.dataPagamentoFinal = null;
      this.numeroNota = '';
      objFiltro.dataPagamentoInicial = this.dataPagamentoInicial;
    }
    this.api.obtemNotasFiltradas(objFiltro, (result) => {
      this.isLoading = false;
      if (result.status === 200) {
        this.notasFiscais = result.data;
      }
    });
  }

  abrirModalPagamento(nf: any): void {
    this.modalPagamentoNF_titulo = `NF ${nf.numeroNota}`;
    this.modalPagamentoNF_NotaFiscal = nf;
    this.modalPagamentoNF_ValorBruto = 0;
    this.modalPagamentoNF_MaterialAbatido = 0;
    this.modalPagamentoNF_BaseCalculo = 0;
    this.modalPagamentoNF_IR = 0;
    this.modalPagamentoNF_Art30 = 0;
    this.modalPagamentoNF_INSS = 0;
    this.modalPagamentoNF_ISS = 0;
    this.modalPagamentoNF_ValorLiquido = 0;
    this.modalPagamentoNF_IRSelecionado = null;
    this.modalPagamentoNF_IRBaseSelecionada = null;
    this.modalPagamentoNF_Art30Selecionado = null;
    this.modalPagamentoNF_Art30BaseSelecionada = null;
    this.modalPagamentoNF_INSSSelecionado = null;
    this.modalPagamentoNF_INSSBaseSelecionada = null;
    this.modalPagamentoNF_ISSBaseSelecionada = null;
    this.modalPagamentoNF_IRDataPagamento = null;
    this.modalPagamentoNF_Art30DataPagamento = null;
    this.modalPagamentoNF_INSSDataPagamento = null;
    this.modalPagamentoNF_ISSDataPagamento = null;

    this.api.getNotaFiscalPagamento(nf.id, (result) => {
      if (result.status === 200 && result.data) {
        const d = result.data;
        this.modalPagamentoNF_ValorBruto = d.valorBruto || 0;
        this.modalPagamentoNF_MaterialAbatido = d.materialAbatido || 0;
        this.modalPagamentoNF_BaseCalculo = d.baseCalculo || 0;
        this.modalPagamentoNF_IR = d.ir || 0;
        this.modalPagamentoNF_Art30 = d.art30 || 0;
        this.modalPagamentoNF_INSS = d.inss || 0;
        this.modalPagamentoNF_ISS = d.iss || 0;
        this.modalPagamentoNF_ValorLiquido = d.valorLiquido || 0;
      }
    });
    this.modalPagamentoNF_Exibir = true;
  }

  calculaBaseDeCalculo(): void {
    this.modalPagamentoNF_BaseCalculo = this.modalPagamentoNF_ValorBruto - this.modalPagamentoNF_MaterialAbatido;
    this.calculaValorLiquido();
  }

  calculaIR(): void {
    if (this.modalPagamentoNF_IRSelecionado) {
      const base = this.modalPagamentoNF_IRBaseSelecionada?.id === 2
        ? this.modalPagamentoNF_BaseCalculo : this.modalPagamentoNF_ValorBruto;
      this.modalPagamentoNF_IR = base * (this.modalPagamentoNF_IRSelecionado.valor / 100);
    }
    this.calculaValorLiquido();
  }

  calculaValorLiquido(): void {
    this.modalPagamentoNF_ValorLiquido =
      this.modalPagamentoNF_ValorBruto -
      this.modalPagamentoNF_IR -
      this.modalPagamentoNF_Art30 -
      this.modalPagamentoNF_INSS -
      this.modalPagamentoNF_ISS;
  }

  salvarPagamento(): void {
    const obj = {
      idPedidoCompraNotaFiscal: this.modalPagamentoNF_NotaFiscal?.id,
      valorBruto: this.modalPagamentoNF_ValorBruto,
      materialAbatido: this.modalPagamentoNF_MaterialAbatido,
      baseCalculo: this.modalPagamentoNF_BaseCalculo,
      ir: this.modalPagamentoNF_IR,
      art30: this.modalPagamentoNF_Art30,
      inss: this.modalPagamentoNF_INSS,
      iss: this.modalPagamentoNF_ISS,
      valorLiquido: this.modalPagamentoNF_ValorLiquido,
      irDataPagamento: this.modalPagamentoNF_IRDataPagamento,
      art30DataPagamento: this.modalPagamentoNF_Art30DataPagamento,
      inssDataPagamento: this.modalPagamentoNF_INSSDataPagamento,
      issDataPagamento: this.modalPagamentoNF_ISSDataPagamento
    };
    this.isLoading = true;
    this.api.postNotaFiscalPagamento(obj, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.modalPagamentoNF_Exibir = false;
        this.obtemNotasFiltradas(false);
      }
    });
  }

  downloadNF(id: any): void {
    this.isLoading = true;
    this.api.downloadFile('PedidoCompra', id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }
}
