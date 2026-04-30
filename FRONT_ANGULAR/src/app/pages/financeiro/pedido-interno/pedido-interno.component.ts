import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-pedido-interno',
  standalone: false,
  templateUrl: './pedido-interno.component.html',
  styleUrls: ['./pedido-interno.component.scss']
})
export class PedidoInternoComponent implements OnInit {
  isLoading = false;
  usuarioLogado: any = null;
  pedidosInternos: any[] = [];

  modalPedidoInterno_Exibir = false;
  modalPedidoInterno_Titulo = 'Pedido Interno';
  modalPedidoInterno_Parcelas: any[] = [];

  modalPedidoInternoEdicao_Exibir = false;
  modalPedidoInternoEdicao_DataPagamento: string | null = null;
  modalPedidoInternoEdicao_IdParcela = 0;
  modalPedidoInternoEdicao_Valor = 0;

  modalArquivos_Exibir = false;
  modalArquivos_Arquivos: any[] = [];
  modalArquivos_IdPedidoInterno = 0;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioLogado = JSON.parse(localStorage.getItem('usuarioDTO') || 'null');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleString('pt-BR');
  }

  formataDataSemHora(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  getAll(): void {
    this.isLoading = true;
    this.pedidosInternos = [];
    this.api.getAllPedidosInternos('PedidoInterno', (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosInternos = result.data;
      }
    });
  }

  abrirModalParcelas(pi: any): void {
    this.modalPedidoInterno_Titulo = `Pedido Interno ${pi.codigoFormatado}`;
    this.modalPedidoInterno_Parcelas = pi.parcelas || [];
    this.modalPedidoInterno_Exibir = true;
  }

  definirParcelaComoPaga(idParcela: number): void {
    this.isLoading = true;
    this.api.definirParcelaComoPaga(idParcela, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.data || result.message, icon: 'error' });
      } else {
        this.getAll();
        this.modalPedidoInterno_Exibir = false;
      }
    });
  }

  abrirEdicaoParcela(parcela: any): void {
    this.modalPedidoInternoEdicao_DataPagamento = parcela.dataPagamento ? new Date(parcela.dataPagamento).toISOString().slice(0, 10) : null;
    this.modalPedidoInternoEdicao_IdParcela = parcela.id;
    this.modalPedidoInternoEdicao_Valor = parcela.valor;
    this.modalPedidoInternoEdicao_Exibir = true;
    this.modalPedidoInterno_Exibir = false;
  }

  alterarParcela(): void {
    const obj = {
      dataPagamento: this.modalPedidoInternoEdicao_DataPagamento,
      valor: this.modalPedidoInternoEdicao_Valor,
      idParcela: this.modalPedidoInternoEdicao_IdParcela
    };
    this.isLoading = true;
    this.api.alterarParcela(obj, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.data || result.message, icon: 'error' });
      } else {
        this.getAll();
        this.modalPedidoInternoEdicao_Exibir = false;
      }
    });
  }

  abrirModalArquivos(pi: any): void {
    this.modalArquivos_IdPedidoInterno = pi.id;
    this.modalArquivos_Arquivos = pi.arquivos || [];
    this.modalArquivos_Exibir = true;
  }

  downloadArquivo(id: any): void {
    this.isLoading = true;
    this.api.downloadFile('PedidoInterno', id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }

  excluirArquivo(id: any): void {
    this.isLoading = true;
    this.api.deleteArquivo('PedidoInterno', id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.api.getArquivos('PedidoInterno', this.modalArquivos_IdPedidoInterno, (r) => {
          if (r.status === 200) this.modalArquivos_Arquivos = r.data;
        });
        Swal.fire({ title: 'Arquivo excluído com sucesso', text: result.message, icon: 'success' });
      }
    });
  }
}
