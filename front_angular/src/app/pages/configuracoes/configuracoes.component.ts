import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-configuracoes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './configuracoes.component.html',
  styleUrl: './configuracoes.component.scss',
})
export class ConfiguracoesComponent {
  paginas = [
    'Cliente',
    'Cidade',
    'Usuário',
    'Fornecedor',
    'Cargo',
    'Material',
    'MaterialCategoria',
    'Perfil',
  ];
}
