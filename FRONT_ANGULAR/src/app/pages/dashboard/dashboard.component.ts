import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  template: `
    <div>
      <h1 class="page-title">Dashboard</h1>
      <app-widget>
        <p>Bem-vindo ao sistema WH Engenharia.</p>
      </app-widget>
    </div>
  `
})
export class DashboardComponent {}
