import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-obras',
  standalone: false,
  templateUrl: './obras.component.html',
  styleUrls: ['./obras.component.scss']
})
export class ObrasComponent implements OnInit {
  isLoading = false;
  obras: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Nova Obra';
  obra_Nome = '';
  obra_Descricao = '';
  obra_ClienteId = 0;
  clientes: any[] = [];
  controle_Salvando = false;

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
    this.loadClientes();
  }

  trackByIndex(i: number): number { return i; }

  loadClientes(): void {
    this.api.getAll('Cliente', true, (result) => {
      if (result.status === 200) this.clientes = result.data;
    });
  }

  getAll(): void {
    this.isLoading = true;
    this.obras = [];
    this.api.getAll('Obra', false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.obras = result.data;
      }
    });
  }

  openNovo(): void {
    this.modal_Titulo = 'Nova Obra';
    this.obra_Nome = '';
    this.obra_Descricao = '';
    this.obra_ClienteId = 0;
    this.modal_Exibir = true;
  }

  salvar(): void {
    if (!this.obra_Nome.trim()) {
      Swal.fire({ title: 'Nome inválido', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = { Id: 0, IdCliente: this.obra_ClienteId, Nome: this.obra_Nome, Descricao: this.obra_Descricao };
    this.api.post('Obra', obj, (result) => {
      this.controle_Salvando = false;
      if (result.status !== 201) {
        Swal.fire({ title: 'Erro ao cadastrar obra', text: result.message, icon: 'error' });
      } else {
        this.modal_Exibir = false;
        this.getAll();
      }
    });
  }

  verDetalhe(id: any): void {
    this.router.navigate(['/app/obraDetalhe', id]);
  }
}
