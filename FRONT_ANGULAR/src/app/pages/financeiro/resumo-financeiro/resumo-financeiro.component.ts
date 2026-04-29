import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-resumo-financeiro',
  standalone: false,
  template: `
    <div class="resumoFinanceiro-page">
      <app-loader [active]="isLoading"></app-loader>
      <h1 class="page-title">Resumo Financeiro</h1>
      <app-widget>
        <p *ngIf="!isLoading">PDF aberto em nova aba. <button class="btn btn-outline-success btn-sm" (click)="downloadPDF()">Baixar novamente</button></p>
      </app-widget>
    </div>`
})
export class ResumoFinanceiroComponent implements OnInit {
  isLoading = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.downloadPDF();
  }

  downloadPDF(): void {
    this.isLoading = true;
    this.api.downloadPdfRelatorioFinanceiro((result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao baixar relatório financeiro', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }
}
