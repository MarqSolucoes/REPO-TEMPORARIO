import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-fornecedor',
  standalone: false,
  templateUrl: './fornecedor.component.html',
  styleUrls: ['./fornecedor.component.scss']
})
export class FornecedorComponent implements OnInit {
  isLoading = false;
  fornecedores: any[] = [];
  cidades: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Fornecedor';
  modal_ExibirBotaoCadastrar = true;
  fornecedor_Id = 0;
  fornecedor_Nome = '';
  fornecedor_CNPJ = '';
  fornecedor_Email = '';
  fornecedor_Telefone = '';
  fornecedor_CidadeId = 0;
  fornecedor_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
    this.loadCidades();
  }

  trackByIndex(i: number): number { return i; }

  loadCidades(): void {
    this.api.getAll('Cidade', true, (result) => {
      if (result.status === 200) this.cidades = result.data;
    });
  }

  getAll(): void {
    this.fornecedores = [];
    this.api.getAll('Fornecedor', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.fornecedores = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Fornecedor';
    this.fornecedor_Id = 0;
    this.fornecedor_Nome = '';
    this.fornecedor_CNPJ = '';
    this.fornecedor_Email = '';
    this.fornecedor_Telefone = '';
    this.fornecedor_CidadeId = 0;
    this.fornecedor_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Fornecedor';
    this.fornecedor_Id = row.id;
    this.fornecedor_Nome = row.nome;
    this.fornecedor_CNPJ = row.cnpj;
    this.fornecedor_Email = row.email;
    this.fornecedor_Telefone = row.telefone;
    this.fornecedor_CidadeId = row.cidadeId;
    this.fornecedor_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.fornecedor_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: this.fornecedor_Id, Nome: this.fornecedor_Nome, CNPJ: this.fornecedor_CNPJ, Email: this.fornecedor_Email, Telefone: this.fornecedor_Telefone, CidadeId: this.fornecedor_CidadeId, Ativo: this.fornecedor_Ativo };
    if (this.modal_ExibirBotaoCadastrar) {
      this.api.post('Fornecedor', obj, (r: any) => this.handleSave(r));
    } else {
      this.api.put('Fornecedor', obj, (r: any) => this.handleSave(r));
    }
  }

  handleSave(result: any): void {
    this.controle_Salvando = false;
    if (result.status !== 201) {
      Swal.fire({ title: 'Erro ao salvar fornecedor', text: result.message, icon: 'error' });
    } else {
      this.modal_Exibir = false;
      this.getAll();
    }
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('Fornecedor', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar fornecedor', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
