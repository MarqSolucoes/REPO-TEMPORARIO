import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-material-categoria',
  standalone: false,
  templateUrl: './material-categoria.component.html',
  styleUrls: ['./material-categoria.component.scss']
})
export class MaterialCategoriaComponent implements OnInit {
  isLoading = false;
  categorias: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Categoria';
  modal_ExibirBotaoCadastrar = true;
  categoria_Id = 0;
  categoria_Descricao = '';
  categoria_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.categorias = [];
    this.api.getAll('CategoriaMaterial', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.categorias = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Categoria';
    this.categoria_Id = 0;
    this.categoria_Descricao = '';
    this.categoria_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Categoria';
    this.categoria_Id = row.id;
    this.categoria_Descricao = row.descricao;
    this.categoria_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.categoria_Descricao.trim()) {
      Swal.fire({ title: 'Descrição inválida', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: this.categoria_Id, Descricao: this.categoria_Descricao, Ativo: this.categoria_Ativo };
    if (this.modal_ExibirBotaoCadastrar) {
      this.api.post('CategoriaMaterial', obj, (r: any) => this.handleSave(r));
    } else {
      this.api.put('CategoriaMaterial', obj, (r: any) => this.handleSave(r));
    }
  }

  handleSave(result: any): void {
    this.controle_Salvando = false;
    if (result.status !== 201) {
      Swal.fire({ title: 'Erro ao salvar categoria', text: result.message, icon: 'error' });
    } else {
      this.modal_Exibir = false;
      this.getAll();
    }
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('CategoriaMaterial', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar categoria', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
