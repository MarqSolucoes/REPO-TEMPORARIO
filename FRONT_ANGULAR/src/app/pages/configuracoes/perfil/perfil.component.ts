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
  perfil_Descricao = '';
  perfil_Ativo = true;
  // Permissões
  perfil_MenuCadastros = true;
  perfil_Cargo = true;
  perfil_CargoCadastrar = true;
  perfil_CargoAtivarDesativar = true;
  perfil_Cliente = true;
  perfil_ClienteCadastrar = true;
  perfil_ClienteEditar = true;
  perfil_ClienteAtivarDesativar = true;
  perfil_Fornecedor = true;
  perfil_FornecedorCadastrar = true;
  perfil_FornecedorEditar = true;
  perfil_FornecedorAtivarDesativar = true;
  perfil_Material = true;
  perfil_MaterialCadastrar = true;
  perfil_MaterialEditar = true;
  perfil_MaterialAtivarDesativar = true;
  perfil_CategoriaMaterial = true;
  perfil_CategoriaMaterialCadastrar = true;
  perfil_CategoriaMaterialAtivarDesativar = true;
  perfil_Perfil = true;
  perfil_PerfilCadastrar = true;
  perfil_PerfilEditar = true;
  perfil_PerfilAtivarDesativar = true;
  perfil_Usuario = true;
  perfil_UsuarioCadastrar = true;
  perfil_UsuarioEditar = true;
  perfil_UsuarioTrocarSenha = true;
  perfil_UsuarioAtivarDesativar = true;
  perfil_UsuarioHabilitarDesabilitarLogin = true;
  perfil_Obra = true;
  perfil_ObraCadastrar = true;
  perfil_ObraEditar = true;
  perfil_ObraVisualizar = true;
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

  private resetPermissions(value: boolean): void {
    this.perfil_MenuCadastros = value;
    this.perfil_Cargo = value;
    this.perfil_CargoCadastrar = value;
    this.perfil_CargoAtivarDesativar = value;
    this.perfil_Cliente = value;
    this.perfil_ClienteCadastrar = value;
    this.perfil_ClienteEditar = value;
    this.perfil_ClienteAtivarDesativar = value;
    this.perfil_Fornecedor = value;
    this.perfil_FornecedorCadastrar = value;
    this.perfil_FornecedorEditar = value;
    this.perfil_FornecedorAtivarDesativar = value;
    this.perfil_Material = value;
    this.perfil_MaterialCadastrar = value;
    this.perfil_MaterialEditar = value;
    this.perfil_MaterialAtivarDesativar = value;
    this.perfil_CategoriaMaterial = value;
    this.perfil_CategoriaMaterialCadastrar = value;
    this.perfil_CategoriaMaterialAtivarDesativar = value;
    this.perfil_Perfil = value;
    this.perfil_PerfilCadastrar = value;
    this.perfil_PerfilEditar = value;
    this.perfil_PerfilAtivarDesativar = value;
    this.perfil_Usuario = value;
    this.perfil_UsuarioCadastrar = value;
    this.perfil_UsuarioEditar = value;
    this.perfil_UsuarioTrocarSenha = value;
    this.perfil_UsuarioAtivarDesativar = value;
    this.perfil_UsuarioHabilitarDesabilitarLogin = value;
    this.perfil_Obra = value;
    this.perfil_ObraCadastrar = value;
    this.perfil_ObraEditar = value;
    this.perfil_ObraVisualizar = value;
  }

  openNovo(): void {
    this.modal_Titulo = 'Cadastro de Perfil';
    this.perfil_Id = 0;
    this.perfil_Descricao = '';
    this.perfil_Ativo = true;
    this.resetPermissions(true);
    this.modal_ExibirBotaoCadastrar = true;
    this.modal_Exibir = true;
  }

  openEdit(row: any): void {
    this.modal_Titulo = 'Edição de Perfil';
    this.perfil_Id = row.id;
    this.perfil_Descricao = row.descricao;
    this.perfil_Ativo = row.ativo;
    this.perfil_MenuCadastros = row.menuCadastros;
    this.perfil_Cargo = row.cargo;
    this.perfil_CargoCadastrar = row.cargoCadastrar;
    this.perfil_CargoAtivarDesativar = row.cargoAtivarDesativar;
    this.perfil_Cliente = row.cliente;
    this.perfil_ClienteCadastrar = row.clienteCadastrar;
    this.perfil_ClienteEditar = row.clienteEditar;
    this.perfil_ClienteAtivarDesativar = row.clienteAtivarDesativar;
    this.perfil_Fornecedor = row.fornecedor;
    this.perfil_FornecedorCadastrar = row.fornecedorCadastrar;
    this.perfil_FornecedorEditar = row.fornecedorEditar;
    this.perfil_FornecedorAtivarDesativar = row.fornecedorAtivarDesativar;
    this.perfil_Material = row.material;
    this.perfil_MaterialCadastrar = row.materialCadastrar;
    this.perfil_MaterialEditar = row.materialEditar;
    this.perfil_MaterialAtivarDesativar = row.materialAtivarDesativar;
    this.perfil_CategoriaMaterial = row.materialCategoria;
    this.perfil_CategoriaMaterialCadastrar = row.materialCategoriaCadastrar;
    this.perfil_CategoriaMaterialAtivarDesativar = row.materialCategoriaAtivarDesativar;
    this.perfil_Perfil = row.perfis;
    this.perfil_PerfilCadastrar = row.perfilCadastrar;
    this.perfil_PerfilEditar = row.perfilEditar;
    this.perfil_PerfilAtivarDesativar = row.perfilAtivarDesativar;
    this.perfil_Usuario = row.usuario;
    this.perfil_UsuarioCadastrar = row.usuarioCadastrar;
    this.perfil_UsuarioEditar = row.usuarioEditar;
    this.perfil_UsuarioTrocarSenha = row.usuarioTrocarSenha;
    this.perfil_UsuarioAtivarDesativar = row.usuarioAtivarDesativar;
    this.perfil_UsuarioHabilitarDesabilitarLogin = row.usuarioHabilitarDesabilitarLogin;
    this.perfil_Obra = row.obra;
    this.perfil_ObraCadastrar = row.obraCadastrar;
    this.perfil_ObraEditar = row.obraEditar;
    this.perfil_ObraVisualizar = row.obraVisualizar;
    this.modal_ExibirBotaoCadastrar = false;
    this.modal_Exibir = true;
  }

  private buildObj(): any {
    return {
      Id: this.perfil_Id,
      Descricao: this.perfil_Descricao,
      Ativo: this.perfil_Ativo,
      MenuCadastros: this.perfil_MenuCadastros,
      Cargo: this.perfil_Cargo,
      CargoCadastrar: this.perfil_CargoCadastrar,
      CargoAtivarDesativar: this.perfil_CargoAtivarDesativar,
      Cliente: this.perfil_Cliente,
      ClienteCadastrar: this.perfil_ClienteCadastrar,
      ClienteEditar: this.perfil_ClienteEditar,
      ClienteAtivarDesativar: this.perfil_ClienteAtivarDesativar,
      Fornecedor: this.perfil_Fornecedor,
      FornecedorCadastrar: this.perfil_FornecedorCadastrar,
      FornecedorEditar: this.perfil_FornecedorEditar,
      FornecedorAtivarDesativar: this.perfil_FornecedorAtivarDesativar,
      Material: this.perfil_Material,
      MaterialCadastrar: this.perfil_MaterialCadastrar,
      MaterialEditar: this.perfil_MaterialEditar,
      MaterialAtivarDesativar: this.perfil_MaterialAtivarDesativar,
      MaterialCategoria: this.perfil_CategoriaMaterial,
      MaterialCategoriaCadastrar: this.perfil_CategoriaMaterialCadastrar,
      MaterialCategoriaAtivarDesativar: this.perfil_CategoriaMaterialAtivarDesativar,
      Perfis: this.perfil_Perfil,
      PerfilCadastrar: this.perfil_PerfilCadastrar,
      PerfilEditar: this.perfil_PerfilEditar,
      PerfilAtivarDesativar: this.perfil_PerfilAtivarDesativar,
      Usuario: this.perfil_Usuario,
      UsuarioCadastrar: this.perfil_UsuarioCadastrar,
      UsuarioEditar: this.perfil_UsuarioEditar,
      UsuarioTrocarSenha: this.perfil_UsuarioTrocarSenha,
      UsuarioAtivarDesativar: this.perfil_UsuarioAtivarDesativar,
      UsuarioHabilitarDesabilitarLogin: this.perfil_UsuarioHabilitarDesabilitarLogin,
      Obra: this.perfil_Obra,
      ObraCadastrar: this.perfil_ObraCadastrar,
      ObraEditar: this.perfil_ObraEditar,
      ObraVisualizar: this.perfil_ObraVisualizar
    };
  }

  salvar(): void {
    if (!this.perfil_Descricao.trim()) {
      Swal.fire({ title: 'Descrição inválida', text: '', icon: 'error' });
      return;
    }
    this.controle_Salvando = true;
    const obj = this.buildObj();
    if (this.modal_ExibirBotaoCadastrar) {
      this.api.post('Perfil', obj, (result) => {
        this.controle_Salvando = false;
        if (result.status !== 201) {
          Swal.fire({ title: 'Erro ao salvar perfil', text: result.message, icon: 'error' });
        } else {
          this.modal_Exibir = false;
          this.getAll();
        }
      });
    } else {
      this.api.put('Perfil', obj, (result) => {
        this.controle_Salvando = false;
        if (result.status !== 201) {
          Swal.fire({ title: 'Erro ao salvar perfil', text: result.message, icon: 'error' });
        } else {
          this.modal_Exibir = false;
          this.getAll();
        }
      });
    }
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
