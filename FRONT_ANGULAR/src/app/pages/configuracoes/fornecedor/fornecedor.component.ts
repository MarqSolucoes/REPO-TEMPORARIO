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
  condicoesPagamento: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Fornecedor';
  modal_ExibirBotaoCadastrar = true;
  fornecedor_Id = 0;
  // 1 = Pessoa Física, 2 = Pessoa Jurídica
  fornecedor_IdTipoFornecedor = 2;
  // Pessoa Jurídica
  fornecedor_RazaoSocial = '';
  fornecedor_NomeFantasia = '';
  fornecedor_CNPJ = '';
  fornecedor_InscricaoEstadual = '';
  // Pessoa Física
  fornecedor_Nome = '';
  fornecedor_CPF = '';
  // Comum
  fornecedor_NumeroCadastral = '';
  fornecedor_Email = '';
  fornecedor_NomeVendedor = '';
  fornecedor_TelefoneCelular = '';
  fornecedor_TelefoneFixo = '';
  fornecedor_CEP = '';
  fornecedor_Bairro = '';
  fornecedor_Endereco = '';
  fornecedor_Observacao = '';
  fornecedor_IdCidade = 0;
  fornecedor_IdCondicaoPagamento = 0;
  fornecedor_Banco = '';
  fornecedor_Agencia = '';
  fornecedor_Conta = '';
  fornecedor_TipoConta = '';
  fornecedor_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
    this.loadCidades();
    this.loadCondicoesPagamento();
  }

  trackByIndex(i: number): number { return i; }

  loadCidades(): void {
    this.api.getAll('Cidade', true, (result) => {
      if (result.status === 200) this.cidades = result.data;
    });
  }

  loadCondicoesPagamento(): void {
    this.api.getAll('CondicaoPagamento', true, (result) => {
      if (result.status === 200) this.condicoesPagamento = result.data;
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
    this.fornecedor_IdTipoFornecedor = 2;
    this.fornecedor_RazaoSocial = '';
    this.fornecedor_NomeFantasia = '';
    this.fornecedor_CNPJ = '';
    this.fornecedor_InscricaoEstadual = '';
    this.fornecedor_Nome = '';
    this.fornecedor_CPF = '';
    this.fornecedor_NumeroCadastral = '';
    this.fornecedor_Email = '';
    this.fornecedor_NomeVendedor = '';
    this.fornecedor_TelefoneCelular = '';
    this.fornecedor_TelefoneFixo = '';
    this.fornecedor_CEP = '';
    this.fornecedor_Bairro = '';
    this.fornecedor_Endereco = '';
    this.fornecedor_Observacao = '';
    this.fornecedor_IdCidade = 0;
    this.fornecedor_IdCondicaoPagamento = 0;
    this.fornecedor_Banco = '';
    this.fornecedor_Agencia = '';
    this.fornecedor_Conta = '';
    this.fornecedor_TipoConta = '';
    this.fornecedor_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Fornecedor';
    this.fornecedor_Id = row.id;
    this.fornecedor_IdTipoFornecedor = row.idTipoFornecedor;
    this.fornecedor_RazaoSocial = row.razaoSocial;
    this.fornecedor_NomeFantasia = row.nomeFantasia;
    this.fornecedor_CNPJ = row.cnpj;
    this.fornecedor_InscricaoEstadual = row.inscricaoEstadual;
    this.fornecedor_Nome = row.nome;
    this.fornecedor_CPF = row.cpf;
    this.fornecedor_NumeroCadastral = row.numeroCadastral;
    this.fornecedor_Email = row.email;
    this.fornecedor_NomeVendedor = row.nomeVendedor;
    this.fornecedor_TelefoneCelular = row.telefoneCelular;
    this.fornecedor_TelefoneFixo = row.teleFoneFixo;
    this.fornecedor_CEP = row.cep;
    this.fornecedor_Bairro = row.bairro;
    this.fornecedor_Endereco = row.endereco;
    this.fornecedor_Observacao = row.observacao;
    this.fornecedor_IdCidade = row.idCidade;
    this.fornecedor_IdCondicaoPagamento = row.idCondicaoPagamento;
    this.fornecedor_Banco = row.banco;
    this.fornecedor_Agencia = row.agencia;
    this.fornecedor_Conta = row.conta;
    this.fornecedor_TipoConta = row.tipoConta;
    this.fornecedor_Ativo = row.ativo;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  salvar(): void {
    const isPJ = this.fornecedor_IdTipoFornecedor === 2;
    if (isPJ && !this.fornecedor_RazaoSocial.trim()) {
      Swal.fire({ title: 'Razão Social inválida', text: '', icon: 'error' });
      return;
    }
    if (!isPJ && !this.fornecedor_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = {
      Id: this.fornecedor_Id,
      IdCidade: this.fornecedor_IdCidade,
      IdTipoFornecedor: this.fornecedor_IdTipoFornecedor,
      RazaoSocial: isPJ ? this.fornecedor_RazaoSocial : null,
      NomeFantasia: isPJ ? this.fornecedor_NomeFantasia : null,
      CNPJ: isPJ ? this.fornecedor_CNPJ : null,
      InscricaoEstadual: isPJ ? this.fornecedor_InscricaoEstadual : null,
      Nome: !isPJ ? this.fornecedor_Nome : null,
      CPF: !isPJ ? this.fornecedor_CPF : null,
      NumeroCadastral: this.fornecedor_NumeroCadastral,
      Email: this.fornecedor_Email,
      NomeVendedor: this.fornecedor_NomeVendedor,
      TelefoneCelular: this.fornecedor_TelefoneCelular,
      TelefoneFixo: this.fornecedor_TelefoneFixo,
      CEP: this.fornecedor_CEP,
      Bairro: this.fornecedor_Bairro,
      Endereco: this.fornecedor_Endereco,
      Observacao: this.fornecedor_Observacao,
      Ativo: this.fornecedor_Ativo,
      IdCondicaoPagamento: this.fornecedor_IdCondicaoPagamento,
      Banco: this.fornecedor_Banco,
      Agencia: this.fornecedor_Agencia,
      Conta: this.fornecedor_Conta,
      TipoConta: this.fornecedor_TipoConta
    };
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
