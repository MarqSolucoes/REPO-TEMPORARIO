import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroAvancadoApiService } from '../../../core/services/financeiro-avancado-api.service';

@Component({selector:'app-pedido-interno',standalone:true,imports:[CommonModule],templateUrl:'./pedido-interno.component.html',styleUrl:'./pedido-interno.component.scss'})
export class PedidoInternoComponent implements OnInit {
  private api = inject(FinanceiroAvancadoApiService);
  isLoading=false; error=''; itens:any[]=[]; extra='';
  ngOnInit(){ this.isLoading=true; this.api.obtemPedidosInternosParaAprovacao(0).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar pedidos internos.'}); }
}
