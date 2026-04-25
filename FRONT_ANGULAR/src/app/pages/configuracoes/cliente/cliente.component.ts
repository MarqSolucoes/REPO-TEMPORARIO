import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cliente',
  standalone: false,
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.scss']
})
export class ClienteComponent implements OnInit {
  isLoading = false;
  clientes: any[] = [];
  cidades: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Cliente';
  modal_ExibirBotaoCadastrar = true;
  cliente_Id = 0;
  cliente_Nome = '';
  cliente_CNPJ = '';
  cliente_InscricaoEstadual = '';
  cliente_Email = '';
  cliente_Telefone = '';
  cliente_CidadeId = 0;
  cliente_Ativo = true;
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
    this.clientes = [];
    this.api.getAll('Cliente', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.clientes = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Cliente';
    this.cliente_Id = 0;
    this.cliente_Nome = '';
    this.cliente_CNPJ = '';
    this.cliente_InscricaoEstadual = '';
    this.cliente_Email = '';
    this.cliente_Telefone = '';
    this.cliente_CidadeId = 0;
    this.cliente_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Cliente';
    this.cliente_Id = row.id;
    this.cliente_Nome = row.nome;
    this.cliente_CNPJ = row.cnpj;
    this.cliente_InscricaoEstadual = row.inscricaoEstadual;
    this.cliente_Email = row.email;
    this.cliente_Telefone = row.telefone;
    this.cliente_CidadeId = row.cidadeId;
    this.cliente_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.cliente_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: this.cliente_Id, Nome: this.cliente_Nome, CNPJ: this.cliente_CNPJ, InscricaoEstadual: this.cliente_InscricaoEstadual, Email: this.cliente_Email, Telefone: this.cliente_Telefone, CidadeId: this.cliente_CidadeId, Ativo: this.cliente_Ativo };
    const action = this.modal_ExibirBotaoCadastrar
      ? this.api.post('Cliente', obj, (r: any) => this.handleSave(r))
      : this.api.put('Cliente', obj, (r: any) => this.handleSave(r));
  }

  handleSave(result: any): void {
    this.controle_Salvando = false;
    if (result.status !== 201) {
      Swal.fire({ title: 'Erro ao salvar cliente', text: result.message, icon: 'error' });
    } else {
      this.modal_Exibir = false;
      this.getAll();
    }
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('Cliente', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar cliente', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
