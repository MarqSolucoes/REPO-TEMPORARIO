import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroExtraApiService } from '../../../core/services/financeiro-extra-api.service';
import { CustoFixoResumo } from '../../../core/models/financeiro-extra.models';

@Component({selector:'app-custo-fixo',standalone:true,imports:[CommonModule],templateUrl:'./custo-fixo.component.html',styleUrl:'./custo-fixo.component.scss'})
export class CustoFixoComponent implements OnInit { private api=inject(FinanceiroExtraApiService); isLoading=false; error=''; itens:CustoFixoResumo[]=[]; ngOnInit(){this.isLoading=true; this.api.obtemCustosFixos().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar custos fixos.'}); }}
