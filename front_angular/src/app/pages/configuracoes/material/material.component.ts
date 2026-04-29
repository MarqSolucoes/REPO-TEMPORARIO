import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Material } from '../../../core/models/material.models';
import { MaterialApiService } from '../../../core/services/material-api.service';

@Component({selector:'app-config-material',standalone:true,imports:[CommonModule,ReactiveFormsModule],templateUrl:'./material.component.html',styleUrl:'./material.component.scss'})
export class MaterialComponent implements OnInit {
  private api=inject(MaterialApiService); private fb=inject(FormBuilder);
  itens:Material[]=[]; isLoading=false; error=''; editId:number|null=null;
  form=this.fb.nonNullable.group({nome:['',[Validators.required,Validators.minLength(2)]]});
  ngOnInit(){this.carregar();}
  carregar(){this.isLoading=true;this.api.listar(true).pipe(finalize(()=>this.isLoading=false)).subscribe({next:r=>this.itens=r,error:()=>this.error='Erro ao carregar material.'});}
  editar(x:Material){this.editId=x.id; this.form.patchValue({nome:x.nome||''});}
  novo(){this.editId=null; this.form.reset({nome:''});}
  salvar(){if(this.form.invalid){this.form.markAllAsTouched();return;} const p={id:this.editId??undefined,nome:this.form.getRawValue().nome}; this.isLoading=true; this.api.salvar(p).pipe(finalize(()=>this.isLoading=false)).subscribe({next:()=>{this.novo();this.carregar();},error:()=>this.error='Erro ao salvar material.'});}
}
