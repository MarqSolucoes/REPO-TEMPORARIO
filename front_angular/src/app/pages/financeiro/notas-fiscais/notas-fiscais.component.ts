import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroApiService } from '../../../core/services/financeiro-api.service';
import { NotaFiscalFinanceira } from '../../../core/models/financeiro.models';

@Component({selector:'app-notas-fiscais',standalone:true,imports:[CommonModule],templateUrl:'./notas-fiscais.component.html',styleUrl:'./notas-fiscais.component.scss'})
export class NotasFiscaisComponent implements OnInit {
  private api = inject(FinanceiroApiService);
  isLoading=false; errorMessage=''; notas:NotaFiscalFinanceira[]=[];
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true; this.api.obtemNotasFiscaisFinanceiro().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.notas=r,error:()=>this.errorMessage='Erro ao carregar notas fiscais.'});}
}
