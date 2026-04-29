import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroAvancadoApiService } from '../../../core/services/financeiro-avancado-api.service';

@Component({selector:'app-pedido-interno-recorrente',standalone:true,imports:[CommonModule],templateUrl:'./pedido-interno-recorrente.component.html',styleUrl:'./pedido-interno-recorrente.component.scss'})
export class PedidoInternoRecorrenteComponent implements OnInit {
  private api = inject(FinanceiroAvancadoApiService);
  isLoading=false; error=''; itens:any[]=[]; extra='';
  ngOnInit(){ this.isLoading=true; this.api.obtemProximoCodigoPedidoInterno().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.extra='Próximo código: '+r,error:()=>this.error='Erro ao carregar próximo código de pedido interno.'}); }
}
