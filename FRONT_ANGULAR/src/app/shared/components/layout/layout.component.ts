import { Component, OnInit } from '@angular/core';
import { LayoutService } from '../../../core/services/layout.service';

@Component({
  selector: 'app-layout',
  standalone: false,
  template: `
    <div class="root" [class.sidebarClose]="sidebarClose">
      <app-header></app-header>
      <app-sidebar></app-sidebar>
      <div class="content">
        <router-outlet></router-outlet>
      </div>
      <footer class="contentFooter">WH Engenharia - Desenvolvido por Solinski Corp</footer>
    </div>
  `
})
export class LayoutComponent implements OnInit {
  sidebarClose = true;

  constructor(private layout: LayoutService) {}

  ngOnInit(): void {
    this.layout.sidebarClose$.subscribe(v => this.sidebarClose = v);
  }
}
