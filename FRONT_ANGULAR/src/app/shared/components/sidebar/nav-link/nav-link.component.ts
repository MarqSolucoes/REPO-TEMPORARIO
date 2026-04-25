import { Component, Input } from '@angular/core';
import { LayoutService } from '../../../../core/services/layout.service';

@Component({
  selector: 'app-nav-link',
  standalone: false,
  templateUrl: './nav-link.component.html'
})
export class NavLinkComponent {
  @Input() header = '';
  @Input() link = '';
  @Input() iconName = '';
  @Input() isHeader = false;
  @Input() childrenLinks: any[] | null = null;
  @Input() activeItem: string | null = '';
  @Input() index = '';
  @Input() externalLink = false;

  expanded = false;

  constructor(private layout: LayoutService) {}

  get isActive(): boolean {
    return !!(this.activeItem && this.index && this.activeItem.includes(this.index));
  }

  get fullIconName(): string {
    return `fi ${this.iconName}`;
  }

  toggle(): void {
    this.expanded = !this.expanded;
    this.layout.changeSidebarActive(this.isActive ? null : this.link);
  }
}
