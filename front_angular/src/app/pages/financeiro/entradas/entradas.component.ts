import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroApiService } from '../../../core/services/financeiro-api.service';
import { EntradaFinanceira } from '../../../core/models/financeiro.models';

@Component({selector:'app-entradas',standalone:true,imports:[CommonModule],templateUrl:'./entradas.component.html',styleUrl:'./entradas.component.scss'})
export class EntradasComponent implements OnInit {
  private api = inject(FinanceiroApiService);
  isLoading=false; errorMessage=''; entradas:EntradaFinanceira[]=[];
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true; this.api.obtemEntradas().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.entradas=r,error:()=>this.errorMessage='Erro ao carregar entradas.'});}
}
