import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-material',
  standalone: false,
  templateUrl: './material.component.html',
  styleUrls: ['./material.component.scss']
})
export class MaterialComponent implements OnInit {
  isLoading = false;
  materiais: any[] = [];
  categorias: any[] = [];
  unidades: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Material';
  modal_ExibirBotaoCadastrar = true;
  material_Id = 0;
  material_Descricao = '';
  material_IdCategoriaMaterial = 0;
  material_IdUnidadeMaterial = 0;
  material_Servico = false;
  material_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
    this.loadCategorias();
    this.loadUnidades();
  }

  trackByIndex(i: number): number { return i; }

  loadCategorias(): void {
    this.api.getAll('CategoriaMaterial', true, (result) => {
      if (result.status === 200) this.categorias = result.data;
    });
  }

  loadUnidades(): void {
    this.api.getUnidades((result) => {
      if (result.status === 200) this.unidades = result.data;
    });
  }

  getAll(): void {
    this.materiais = [];
    this.api.obtemMateriaisPaginados({ skip: 0, take: 99999, descricao: '' }, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.materiais = result.data?.data?.items ?? result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Material';
    this.material_Id = 0;
    this.material_Descricao = '';
    this.material_IdCategoriaMaterial = 0;
    this.material_IdUnidadeMaterial = 0;
    this.material_Servico = false;
    this.material_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Material';
    this.material_Id = row.id;
    this.material_Descricao = row.descricao;
    this.material_IdCategoriaMaterial = row.idCategoriaMaterial;
    this.material_IdUnidadeMaterial = row.idUnidadeMaterial;
    this.material_Servico = row.servico;
    this.material_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.material_Descricao.trim()) {
      Swal.fire({ title: 'Descrição inválida', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = {
      Id: this.material_Id,
      IdCategoriaMaterial: this.material_IdCategoriaMaterial,
      IdUnidadeMaterial: this.material_IdUnidadeMaterial,
      Descricao: this.material_Descricao,
      Servico: this.material_Servico,
      Ativo: this.material_Ativo
    };
    if (this.modal_ExibirBotaoCadastrar) {
      this.api.post('Material', obj, (r: any) => this.handleSave(r));
    } else {
      this.api.put('Material', obj, (r: any) => this.handleSave(r));
    }
  }

  handleSave(result: any): void {
    this.controle_Salvando = false;
    if (result.status !== 201) {
      Swal.fire({ title: 'Erro ao salvar material', text: result.message, icon: 'error' });
    } else {
      this.modal_Exibir = false;
      this.getAll();
    }
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('Material', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar material', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
