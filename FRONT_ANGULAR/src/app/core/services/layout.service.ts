import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LayoutService {
  sidebarClose$ = new BehaviorSubject<boolean>(false);
  sidebarActiveElement$ = new BehaviorSubject<string | null>(null);

  switchSidebar(value?: boolean): void {
    this.sidebarClose$.next(value !== undefined ? value : !this.sidebarClose$.value);
  }

  changeSidebarActive(index: string | null): void {
    this.sidebarActiveElement$.next(index);
  }
}
