import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FinanceiroAvancadoApiService } from '../../../core/services/financeiro-avancado-api.service';

@Component({selector:'app-eto',standalone:true,imports:[CommonModule],templateUrl:'./eto.component.html',styleUrl:'./eto.component.scss'})
export class EtoComponent implements OnInit {
  private api = inject(FinanceiroAvancadoApiService);
  isLoading=false; error=''; itens:any[]=[]; extra='';
  ngOnInit(){ this.isLoading=true; this.api.obtemEto().pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar ETO.'}); }
}
