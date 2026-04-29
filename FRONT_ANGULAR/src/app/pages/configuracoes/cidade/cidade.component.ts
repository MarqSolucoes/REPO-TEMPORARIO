import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cidade',
  standalone: false,
  templateUrl: './cidade.component.html',
  styleUrls: ['./cidade.component.scss']
})
export class CidadeComponent implements OnInit {
  isLoading = false;
  cidades: any[] = [];
  usuarioDTO: any = {};
  modal_Exibir = false;
  modal_Titulo = 'Edição de Cidade';
  cidade_Id = 0;
  cidade_UF = '';
  cidade_Nome = '';
  cidade_AliquotaImpostoISS: any = 0;
  cidade_Ativo = true;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.listaCidades();
  }

  trackByIndex(i: number): number { return i; }

  listaCidades(): void {
    this.isLoading = true;
    this.cidades = [];
    this.api.getAll('Cidade', false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.cidades = result.data;
      }
    });
  }

  editarCidade(): void {
    if (!this.cidade_AliquotaImpostoISS && this.cidade_AliquotaImpostoISS !== 0) {
      Swal.fire({ title: 'Informe uma alíquota válida para ISS', text: '', icon: 'error' });
      return;
    }
    this.isLoading = true;
    const obj = { Id: this.cidade_Id, UF: this.cidade_UF, Nome: this.cidade_Nome, AliquotaImpostoISS: this.cidade_AliquotaImpostoISS, Ativo: this.cidade_Ativo };
    this.api.put('Cidade', obj, (result) => {
      this.isLoading = false;
      if (result.status !== 201) {
        Swal.fire({ title: 'Erro ao editar cidade', text: result.message, icon: 'error' });
      } else {
        this.modal_Exibir = false;
        this.listaCidades();
      }
    });
  }

  ativarDesativar(id: any, ativo: any): void {
    this.api.ativarDesativar('Cidade', id, !ativo, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro ao ativar/desativar cidade', text: result.message, icon: 'error' });
      } else {
        this.listaCidades();
      }
    });
  }

  openEdit(row: any): void {
    this.cidade_Id = row.id;
    this.cidade_UF = row.uf;
    this.cidade_Nome = row.nome;
    this.cidade_AliquotaImpostoISS = row.aliquotaImpostoISS;
    this.cidade_Ativo = row.ativo;
    this.modal_Exibir = true;
  }
}
