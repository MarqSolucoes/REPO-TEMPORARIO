import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Fornecedor } from '../../../core/models/fornecedor.models';
import { FornecedorApiService } from '../../../core/services/fornecedor-api.service';

@Component({selector:'app-config-fornecedor',standalone:true,imports:[CommonModule,ReactiveFormsModule],templateUrl:'./fornecedor.component.html',styleUrl:'./fornecedor.component.scss'})
export class FornecedorComponent implements OnInit {
  private api=inject(FornecedorApiService); private fb=inject(FormBuilder);
  itens:Fornecedor[]=[]; isLoading=false; error=''; editId:number|null=null;
  form=this.fb.nonNullable.group({nome:['',[Validators.required,Validators.minLength(2)]]});
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true;this.api.listar(true).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar fornecedor.'});}
  editar(x:Fornecedor){this.editId=x.id; this.form.patchValue({nome:x.nome||''});}
  novo(){this.editId=null; this.form.reset({nome:''});}
  salvar(){if(this.form.invalid){this.form.markAllAsTouched();return;} const p={id:this.editId??undefined,nome:this.form.getRawValue().nome}; this.isLoading=true; this.api.salvar(p).pipe(finalize(()=>this.isLoading=false)).subscribe({next:()=>{this.novo();this.carregar();},error:()=>this.error='Erro ao salvar fornecedor.'});}
}
