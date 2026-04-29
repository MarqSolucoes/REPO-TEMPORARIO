import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroExtraApiService } from '../../../core/services/financeiro-extra-api.service';
import { FolhaPagamentoResumo } from '../../../core/models/financeiro-extra.models';

@Component({selector:'app-folha-pagamento',standalone:true,imports:[CommonModule],templateUrl:'./folha-pagamento.component.html',styleUrl:'./folha-pagamento.component.scss'})
export class FolhaPagamentoComponent implements OnInit { private api=inject(FinanceiroExtraApiService); isLoading=false; error=''; itens:FolhaPagamentoResumo[]=[]; ngOnInit(){this.isLoading=true; this.api.obtemFolhaPagamento().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar folha de pagamento.'}); }}
