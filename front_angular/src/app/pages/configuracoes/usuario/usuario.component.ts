import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Usuario } from '../../../core/models/usuario.models';
import { UsuarioApiService } from '../../../core/services/usuario-api.service';

@Component({selector:'app-config-usuario',standalone:true,imports:[CommonModule,ReactiveFormsModule],templateUrl:'./usuario.component.html',styleUrl:'./usuario.component.scss'})
export class UsuarioComponent implements OnInit {
  private api=inject(UsuarioApiService); private fb=inject(FormBuilder);
  itens:Usuario[]=[]; isLoading=false; error=''; editId:number|null=null;
  form=this.fb.nonNullable.group({nome:['',[Validators.required]],email:['',[Validators.required,Validators.email]]});
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true;this.api.listar(true).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar usuários.'});}
  editar(u:Usuario){this.editId=u.id; this.form.patchValue({nome:u.nome||'',email:u.email||''});}
  novo(){this.editId=null; this.form.reset({nome:'',email:''});}
  salvar(){if(this.form.invalid){this.form.markAllAsTouched();return;} const p={id:this.editId??undefined,...this.form.getRawValue()}; this.isLoading=true; this.api.salvar(p).pipe(finalize(()=>this.isLoading=false)).subscribe({next:()=>{this.novo();this.carregar();},error:()=>this.error='Erro ao salvar usuário.'});}
}
