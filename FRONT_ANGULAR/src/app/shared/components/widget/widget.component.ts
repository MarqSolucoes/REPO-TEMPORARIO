import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-widget',
  standalone: false,
  template: `
    <section class="widget" [ngClass]="className">
      <ng-content></ng-content>
    </section>
  `
})
export class WidgetComponent {
  @Input() className = '';
  @Input() customHeader = false;
  @Input() title = '';
}
