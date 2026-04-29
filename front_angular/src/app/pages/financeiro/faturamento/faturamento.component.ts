import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroExtraApiService } from '../../../core/services/financeiro-extra-api.service';
import { FaturamentoResumo } from '../../../core/models/financeiro-extra.models';

@Component({selector:'app-faturamento',standalone:true,imports:[CommonModule],templateUrl:'./faturamento.component.html',styleUrl:'./faturamento.component.scss'})
export class FaturamentoComponent implements OnInit { private api=inject(FinanceiroExtraApiService); isLoading=false; error=''; itens:FaturamentoResumo[]=[]; ngOnInit(){this.api.obtemFaturamentos().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar faturamentos.'}); this.isLoading=true; }}
