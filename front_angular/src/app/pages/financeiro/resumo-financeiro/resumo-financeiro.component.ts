import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FinanceiroApiService } from '../../../core/services/financeiro-api.service';

@Component({selector:'app-resumo-financeiro',standalone:true,imports:[CommonModule],templateUrl:'./resumo-financeiro.component.html',styleUrl:'./resumo-financeiro.component.scss'})
export class ResumoFinanceiroComponent {
  private api = inject(FinanceiroApiService);
  errorMessage='';
  baixarRelatorio(): void {
    this.api.downloadRelatorioPdf().subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        window.open(url, '_blank');
      },
      error: () => this.errorMessage = 'Erro ao baixar relatório financeiro.',
    });
  }
}
