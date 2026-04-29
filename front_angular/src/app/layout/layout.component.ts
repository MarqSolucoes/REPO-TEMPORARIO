import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {
  links = [
    { label: 'Dashboard', path: '/dashboard' },
    { label: 'Login', path: '/login' },
    { label: 'Obras', path: '/obras' },
    { label: 'Ordem Compra', path: '/compras/ordem-compra' },
    { label: 'Cotação', path: '/compras/cotacao' },
    { label: 'Conciliação', path: '/compras/conciliacao' },
    { label: 'Entradas', path: '/financeiro/entradas' },
    { label: 'Resumo Financeiro', path: '/financeiro/resumo-financeiro' },
    { label: 'Configurações', path: '/configuracoes' },
    { label: 'RH', path: '/rh' },
    { label: 'Error', path: '/error' },
  ];
}
