import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroAvancadoApiService } from '../../../core/services/financeiro-avancado-api.service';

@Component({selector:'app-fluxo-caixa-consolidado',standalone:true,imports:[CommonModule],templateUrl:'./fluxo-caixa-consolidado.component.html',styleUrl:'./fluxo-caixa-consolidado.component.scss'})
export class FluxoCaixaConsolidadoComponent implements OnInit {
  private api = inject(FinanceiroAvancadoApiService);
  isLoading=false; error=''; itens:any[]=[]; extra='';
  ngOnInit(){ this.isLoading=true; this.api.obtemFluxoConsolidado(new Date().toISOString()).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar fluxo consolidado.'}); }
}
