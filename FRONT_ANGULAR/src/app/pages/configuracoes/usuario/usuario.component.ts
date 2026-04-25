import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-usuario',
  standalone: false,
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.scss']
})
export class UsuarioComponent implements OnInit {
  isLoading = false;
  usuarios: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Cadastro de Usuário';
  modal_ExibirBotaoCadastrar = true;
  usuario_Id = 0;
  usuario_Nome = '';
  usuario_Login = '';
  usuario_Senha = '';
  usuario_Ativo = true;
  controle_Salvando = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.usuarios = [];
    this.api.getAll('Usuario', false, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.usuarios = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Usuário';
    this.usuario_Id = 0;
    this.usuario_Nome = '';
    this.usuario_Login = '';
    this.usuario_Senha = '';
    this.usuario_Ativo = true;
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.usuario_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: this.usuario_Id, Nome: this.usuario_Nome, Login: this.usuario_Login, Senha: this.usuario_Senha, Ativo: this.usuario_Ativo };
    this.api.post('Usuario', obj, (result) => {
      this.controle_Salvando = false;
      if (result.status !== 201) {
        Swal.fire({ title: 'Erro ao salvar usuário', text: result.message, icon: 'error' });
      } else {
        this.modal_Exibir = false;
        this.getAll();
      }
    });
  }

  ativarDesativar(id: any, ativo: boolean): void {
    this.api.ativarDesativar('Usuario', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar usuário', text: result.message, icon: 'error' });
      } else {
        this.getAll();
      }
    });
  }
}
