import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cargo',
  standalone: false,
  templateUrl: './cargo.component.html',
  styleUrls: ['./cargo.component.scss']
})
export class CargoComponent implements OnInit {
  isLoading = false;
  cargos: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Cargo';
  modal_ExibirBotaoCadastrar = true;
  cargo_Id = 0;
  cargo_Descricao = '';
  cargo_Ativo = true;
  controle_CargoCadastrando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.cargos = [];
    this.api.getAll('Cargo', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.cargos = result.data.map((c: any) => ({ ...c, ativoLabel: c.ativo ? 'Ativo' : 'Inativo' }));
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Cargo';
    this.cargo_Id = 0;
    this.cargo_Descricao = '';
    this.cargo_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  post(): void {
    if (!this.cargo_Descricao.trim()) {
      Swal.fire({ title: 'Descrição inválida', text: '', icon: 'error' });
      return;
    }
    this.controle_CargoCadastrando = true;
    const obj = { Id: this.cargo_Id, Descricao: this.cargo_Descricao, Ativo: this.cargo_Ativo };
    this.api.post('Cargo', obj, (result) => {
      this.controle_CargoCadastrando = false;
      if (result.status !== 201) {
        Swal.fire({ title: 'Erro ao cadastrar cargo', text: result.message, icon: 'error' });
      } else {
        this.modal_Exibir = false;
        this.getAll();
      }
    });
  }

  ativarDesativar(id: any, ativo: string): void {
    this.api.ativarDesativar('Cargo', id, ativo === 'Ativo' ? false : true, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar cargo', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
