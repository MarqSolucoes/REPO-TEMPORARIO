import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Cliente } from '../../../core/models/cliente.models';
import { ClienteApiService } from '../../../core/services/cliente-api.service';

@Component({selector:'app-config-cliente',standalone:true,imports:[CommonModule,ReactiveFormsModule],templateUrl:'./cliente.component.html',styleUrl:'./cliente.component.scss'})
export class ClienteComponent implements OnInit {
  private api=inject(ClienteApiService); private fb=inject(FormBuilder);
  itens:Cliente[]=[]; isLoading=false; error=''; editId:number|null=null;
  form=this.fb.nonNullable.group({nome:['',[Validators.required,Validators.minLength(2)]]});
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true;this.api.listar(true).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar clientes.'});}
  editar(c:Cliente){this.editId=c.id; this.form.patchValue({nome:c.nome||''});}
  novo(){this.editId=null; this.form.reset({nome:''});}
  salvar(){if(this.form.invalid){this.form.markAllAsTouched();return;} const p={id:this.editId??undefined,nome:this.form.getRawValue().nome}; this.isLoading=true; this.api.salvar(p).pipe(finalize(()=>this.isLoading=false)).subscribe({next:()=>{this.novo();this.carregar();},error:()=>this.error='Erro ao salvar cliente.'});}
}
