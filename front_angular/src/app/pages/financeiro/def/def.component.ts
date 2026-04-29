import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroAvancadoApiService } from '../../../core/services/financeiro-avancado-api.service';

@Component({selector:'app-def',standalone:true,imports:[CommonModule],templateUrl:'./def.component.html',styleUrl:'./def.component.scss'})
export class DefComponent implements OnInit {
  private api = inject(FinanceiroAvancadoApiService);
  isLoading=false; error=''; itens:any[]=[]; extra='';
  ngOnInit(){ this.isLoading=true; this.api.obtemDefs().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar DEF.'}); }
}
