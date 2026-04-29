import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { LayoutService } from '../../../core/services/layout.service';

@Component({
  selector: 'app-header',
  standalone: false,
  template: `
    <nav class="app-header d-print-none">
      <a routerLink="/app/dashboard" class="navbar-brand">WH <strong>Engenharia</strong></a>
      <div class="ml-auto d-flex align-items-center">
        <span (click)="logoutUser()" style="cursor:pointer; padding: 0 16px;">
          {{ user?.nome }}
          <i class="fa fa-power-off px-2"></i>
        </span>
      </div>
    </nav>
  `
})
export class HeaderComponent implements OnInit {
  user: any = {};

  constructor(private auth: AuthService, private layout: LayoutService) {}

  ngOnInit(): void {
    try { this.user = JSON.parse(localStorage.getItem('user') || '{}'); } catch {}
  }

  logoutUser(): void {
    this.auth.logoutUser();
  }
}
