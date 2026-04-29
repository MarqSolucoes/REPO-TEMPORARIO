import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loader',
  standalone: false,
  template: `
    <div class="loader-overlay" *ngIf="active">
      <div class="spinner-border text-warning" role="status">
        <span class="sr-only">Carregando...</span>
      </div>
    </div>
  `
})
export class LoaderComponent {
  @Input() active = false;
}
