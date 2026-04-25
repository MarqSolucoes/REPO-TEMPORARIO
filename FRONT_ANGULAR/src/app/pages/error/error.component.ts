import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  standalone: false,
  template: `
    <div class="text-center mt-5">
      <h1>404</h1>
      <h2>Página não encontrada</h2>
      <a routerLink="/app/dashboard" class="btn btn-primary mt-3">Voltar ao início</a>
    </div>
  `
})
export class ErrorComponent {}
