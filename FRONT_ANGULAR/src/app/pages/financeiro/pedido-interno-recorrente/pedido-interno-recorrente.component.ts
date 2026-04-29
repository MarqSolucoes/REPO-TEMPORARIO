import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-pedido-interno-recorrente',
  standalone: false,
  templateUrl: './pedido-interno-recorrente.component.html',
  styleUrls: ['./pedido-interno-recorrente.component.scss']
})
export class PedidoInternoRecorrenteComponent implements OnInit {
  isLoading = false;
  usuarioDTO: any = null;
  pedidosInternosRecorrentes: any[] = [];
  defs: any[] = [];
  dias: number[] = Array.from({ length: 28 }, (_, i) => i + 1);

  modalPIR_Exibir = false;
  modalPIR_Titulo = '';
  modalPIR_Id = 0;
  modalPIR_Descricao = '';
  modalPIR_DefSelecionado: any = null;
  modalPIR_DiaSelecionado: number | null = 1;
  modalPIR_DataLimiteGeracao: string | null = null;
  modalPIR_Valor = 0;
  modalPIR_NecessitaConfirmacao = false;
  modalPIR_Ativo = true;
  modalPIR_MostrarBotaoCadastrar = true;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || 'null');
    this.getAll();
    this.listaDefs();
  }

  trackByIndex(i: number): number { return i; }

  formataDataSemHora(d: any): string {
    if (!d) return '';
    return new Date(d).toLocaleDateString('pt-BR');
  }

  getAll(): void {
    this.pedidosInternosRecorrentes = [];
    this.api.getAllPedidosInternosRecorrentes('PedidoInternoRecorrente', (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.pedidosInternosRecorrentes = result.data;
      }
    });
  }

  listaDefs(): void {
    this.defs = [];
    this.api.getAll('Def', true, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.defs = result.data;
      }
    });
  }

  abrirNovo(): void {
    this.modalPIR_Titulo = 'Cadastro de Pedido Interno Recorrente';
    this.modalPIR_Id = 0;
    this.modalPIR_Descricao = '';
    this.modalPIR_DefSelecionado = null;
    this.modalPIR_DiaSelecionado = 1;
    this.modalPIR_DataLimiteGeracao = null;
    this.modalPIR_Valor = 0;
    this.modalPIR_NecessitaConfirmacao = false;
    this.modalPIR_Ativo = true;
    this.modalPIR_MostrarBotaoCadastrar = true;
    this.modalPIR_Exibir = true;
  }

  abrirEdicao(row: any): void {
    this.modalPIR_Titulo = 'Editar Pedido Interno Recorrente';
    this.modalPIR_Id = row.id;
    this.modalPIR_Descricao = row.descricao;
    this.modalPIR_DefSelecionado = row.def || null;
    this.modalPIR_DiaSelecionado = row.diaGeracao;
    this.modalPIR_DataLimiteGeracao = row.dataLimiteGeracao ? new Date(row.dataLimiteGeracao).toISOString().slice(0, 10) : null;
    this.modalPIR_Valor = row.valor;
    this.modalPIR_NecessitaConfirmacao = row.necessitaConfirmacao;
    this.modalPIR_Ativo = row.ativo;
    this.modalPIR_MostrarBotaoCadastrar = false;
    this.modalPIR_Exibir = true;
  }

  cadastrar(): void {
    const obj: any = {
      descricao: this.modalPIR_Descricao,
      idDef: this.modalPIR_DefSelecionado ? this.modalPIR_DefSelecionado.id : null,
      diaGeracao: this.modalPIR_DiaSelecionado,
      dataLimiteGeracao: this.modalPIR_DataLimiteGeracao,
      valor: this.modalPIR_Valor,
      necessitaConfirmacao: this.modalPIR_NecessitaConfirmacao,
      ativo: true
    };
    this.api.post('PedidoInternoRecorrente', obj, (result) => {
      if (result.status !== 201) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.getAll();
        this.modalPIR_Exibir = false;
      }
    });
  }

  editar(): void {
    const obj: any = {
      id: this.modalPIR_Id,
      descricao: this.modalPIR_Descricao,
      idDef: this.modalPIR_DefSelecionado ? this.modalPIR_DefSelecionado.id : null,
      diaGeracao: this.modalPIR_DiaSelecionado,
      dataLimiteGeracao: this.modalPIR_DataLimiteGeracao,
      valor: this.modalPIR_Valor,
      necessitaConfirmacao: this.modalPIR_NecessitaConfirmacao,
      ativo: this.modalPIR_Ativo
    };
    this.api.put('PedidoInternoRecorrente', obj, (result) => {
      if (result.status !== 201) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.getAll();
        this.modalPIR_Exibir = false;
      }
    });
  }

  ativarDesativar(row: any): void {
    const novoValor = !row.ativo;
    this.api.ativarDesativar('PedidoInternoRecorrente', row.id, novoValor, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
