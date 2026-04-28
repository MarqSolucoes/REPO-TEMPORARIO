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
  cliente_RazaoSocial = '';
  cliente_NomeFantasia = '';
  cliente_CNPJ = '';
  cliente_InscricaoEstadual = '';
  cliente_EmailFinanceiro = '';
  cliente_EmailComercial = '';
  cliente_ResponsavelComercial = '';
  cliente_TelefoneCelular = '';
  cliente_TelefoneFixo = '';
  cliente_DiasDePagamento: any = 0;
  cliente_CEP = '';
  cliente_Endereco = '';
  cliente_Bairro = '';
  cliente_Observacao = '';
  cliente_IdCidade = 0;
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
    this.cliente_RazaoSocial = '';
    this.cliente_NomeFantasia = '';
    this.cliente_CNPJ = '';
    this.cliente_InscricaoEstadual = '';
    this.cliente_EmailFinanceiro = '';
    this.cliente_EmailComercial = '';
    this.cliente_ResponsavelComercial = '';
    this.cliente_TelefoneCelular = '';
    this.cliente_TelefoneFixo = '';
    this.cliente_DiasDePagamento = 0;
    this.cliente_CEP = '';
    this.cliente_Endereco = '';
    this.cliente_Bairro = '';
    this.cliente_Observacao = '';
    this.cliente_IdCidade = 0;
    this.cliente_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Cliente';
    this.cliente_Id = row.id;
    this.cliente_RazaoSocial = row.razaoSocial;
    this.cliente_NomeFantasia = row.nomeFantasia;
    this.cliente_CNPJ = row.cnpj;
    this.cliente_InscricaoEstadual = row.inscricaoEstadual;
    this.cliente_EmailFinanceiro = row.emailFinanceiro;
    this.cliente_EmailComercial = row.emailComercial;
    this.cliente_ResponsavelComercial = row.responsavelComercial;
    this.cliente_TelefoneCelular = row.telefoneCelular;
    this.cliente_TelefoneFixo = row.telefoneFixo;
    this.cliente_DiasDePagamento = row.diasDePagamento;
    this.cliente_CEP = row.cep;
    this.cliente_Endereco = row.endereco;
    this.cliente_Bairro = row.bairro;
    this.cliente_Observacao = row.observacao;
    this.cliente_IdCidade = row.cidade?.id ?? row.idCidade;
    this.cliente_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.cliente_RazaoSocial.trim()) {
      Swal.fire({ title: 'Razão Social inválida', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = {
      Id: this.cliente_Id,
      IdCidade: this.cliente_IdCidade,
      RazaoSocial: this.cliente_RazaoSocial,
      NomeFantasia: this.cliente_NomeFantasia,
      CNPJ: this.cliente_CNPJ,
      InscricaoEstadual: this.cliente_InscricaoEstadual,
      EmailFinanceiro: this.cliente_EmailFinanceiro,
      EmailComercial: this.cliente_EmailComercial,
      ResponsavelComercial: this.cliente_ResponsavelComercial,
      TelefoneCelular: this.cliente_TelefoneCelular,
      TelefoneFixo: this.cliente_TelefoneFixo,
      DiasDePagamento: this.cliente_DiasDePagamento,
      CEP: this.cliente_CEP,
      Endereco: this.cliente_Endereco,
      Bairro: this.cliente_Bairro,
      Observacao: this.cliente_Observacao,
      Ativo: this.cliente_Ativo
    };
    if (this.modal_ExibirBotaoCadastrar) {
      this.api.post('Cliente', obj, (r: any) => this.handleSave(r));
    } else {
      this.api.put('Cliente', obj, (r: any) => this.handleSave(r));
    }
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
