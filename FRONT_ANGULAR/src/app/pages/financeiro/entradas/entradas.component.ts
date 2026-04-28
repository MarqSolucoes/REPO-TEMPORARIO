import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-entradas',
  standalone: false,
  templateUrl: './entradas.component.html',
  styleUrls: ['./entradas.component.scss']
})
export class EntradasComponent implements OnInit {
  isLoading = false;
  activeTab = 0;

  entradas: any[] = [];
  entradasRecusadas: any[] = [];

  modalDetalhePI_Exibir = false;
  modalDetalhePI_Titulo = '';
  modalDetalhePI_PI: any = null;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.obtemEntradas();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleString('pt-BR');
  }

  obtemEntradas(): void {
    this.activeTab = 0;
    this.api.obtemEntradasFinanceiro((result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao obter entradas', text: result.message, icon: 'error' });
      } else {
        this.entradas = result.data;
      }
    });
  }

  obtemEntradasRecusadas(): void {
    this.activeTab = 1;
    this.api.obtemEntradasRecusadasFinanceiro((result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao obter entradas', text: result.message, icon: 'error' });
      } else {
        this.entradasRecusadas = result.data;
      }
    });
  }

  autorizaEntrada(entrada: any): void {
    this.isLoading = true;
    this.api.autorizaEntradaFinanceiro(entrada, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao autorizar entrada', text: result.message, icon: 'error' });
      } else {
        this.obtemEntradas();
        this.obtemEntradasRecusadas();
      }
    });
  }

  cancelaEntrada(entrada: any): void {
    if (entrada.idPedidoCompraNotaFiscal != null) {
      Swal.fire({
        title: 'Atenção',
        text: `Por qual motivo a nota fiscal '${entrada.nomeNF}' está sendo recusada?`,
        icon: 'warning',
        showCancelButton: true,
        showDenyButton: true,
        confirmButtonColor: '#d33',
        denyButtonColor: '#d33',
        cancelButtonColor: '#aaa',
        confirmButtonText: 'Nota fiscal divergente',
        denyButtonText: 'Fornecedor divergente',
        cancelButtonText: 'Cancelar'
      }).then((res: any) => {
        if (!res.isConfirmed && !res.isDenied) return;
        entrada.motivoRecusa = res.isConfirmed ? 'Nota fiscal divergente' : 'Fornecedor divergente';
        this.isLoading = true;
        this.api.cancelaEntradaFinanceiro(entrada, (result) => {
          this.isLoading = false;
          if (result.status !== 200) {
            Swal.fire({ title: 'Erro ao cancelar entrada', text: result.message, icon: 'error' });
          } else {
            this.obtemEntradas();
          }
        });
      });
    } else {
      this.isLoading = true;
      this.api.cancelaEntradaFinanceiro(entrada, (result) => {
        this.isLoading = false;
        if (result.status !== 200) {
          Swal.fire({ title: 'Erro ao cancelar entrada', text: result.message, icon: 'error' });
        } else {
          this.obtemEntradas();
        }
      });
    }
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

  downloadOC(id: any): void {
    this.isLoading = true;
    this.api.downloadPdfPedidoCompra(id, true, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao baixar pedido de compra', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }

  exibeDetalhePI(id: any): void {
    this.api.get('PedidoInterno', id, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao obter PI', text: result.message, icon: 'error' });
      } else {
        this.modalDetalhePI_PI = result.data;
        this.modalDetalhePI_Titulo = `Detalhe PI - ${result.data.codigoFormatado} - ${result.data.descricao}`;
        this.modalDetalhePI_Exibir = true;
      }
    });
  }
}
