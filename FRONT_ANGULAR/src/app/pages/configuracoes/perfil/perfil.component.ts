import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-perfil',
  standalone: false,
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  isLoading = false;
  perfis: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Perfil';
  modal_ExibirBotaoCadastrar = true;
  perfil_Id = 0;
  perfil_Nome = '';
  perfil_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.perfis = [];
    this.api.getAll('Perfil', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.perfis = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Perfil';
    this.perfil_Id = 0;
    this.perfil_Nome = '';
    this.perfil_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.perfil_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: this.perfil_Id, Nome: this.perfil_Nome, Ativo: this.perfil_Ativo };
    this.api.post('Perfil', obj, (result) => {
      this.controle_Salvando = false;
      if (result.status !== 201) {
        Swal.fire({ title: 'Erro ao salvar perfil', text: result.message, icon: 'error' });
      } else {
        this.modal_Exibir = false;
        this.getAll();
      }
    });
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('Perfil', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar perfil', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
