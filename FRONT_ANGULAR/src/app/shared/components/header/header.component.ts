import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { LayoutService } from '../../../core/services/layout.service';

@Component({
  selector: 'app-header',
  standalone: false,
  template: `
    <nav class="app-header d-print-none">
      <div class="ml-auto d-flex align-items-center">
        <span (click)="logoutUser()" style="cursor:pointer; padding: 0 16px;">
          {{ user?.nome }}
          <i class="la la-power-off px-2"></i>
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
