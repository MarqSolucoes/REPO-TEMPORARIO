import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Cidade } from '../../../core/models/cidade.models';
import { CidadeApiService } from '../../../core/services/cidade-api.service';

@Component({selector:'app-config-cidade',standalone:true,imports:[CommonModule,ReactiveFormsModule],templateUrl:'./cidade.component.html',styleUrl:'./cidade.component.scss'})
export class CidadeComponent implements OnInit {
  private api=inject(CidadeApiService); private fb=inject(FormBuilder);
  itens:Cidade[]=[]; isLoading=false; error=''; editId:number|null=null;
  form=this.fb.nonNullable.group({nome:['',[Validators.required]],uf:['',[Validators.required,Validators.minLength(2),Validators.maxLength(2)]]});
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true;this.api.listar(true).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar cidades.'});}
  editar(c:Cidade){this.editId=c.id; this.form.patchValue({nome:c.nome||'',uf:c.uf||''});}
  novo(){this.editId=null; this.form.reset({nome:'',uf:''});}
  salvar(){if(this.form.invalid){this.form.markAllAsTouched();return;} const p={id:this.editId??undefined,...this.form.getRawValue()}; this.isLoading=true; this.api.salvar(p).pipe(finalize(()=>this.isLoading=false)).subscribe({next:()=>{this.novo();this.carregar();},error:()=>this.error='Erro ao salvar cidade.'});}
}
