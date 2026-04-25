import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-def',
  standalone: false,
  templateUrl: './def.component.html',
  styleUrls: ['./def.component.scss']
})
export class DefComponent implements OnInit {
  isLoading = false;
  defs: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.listaDefs();
  }

  trackByIndex(i: number): number { return i; }

  listaDefs(): void {
    this.isLoading = true;
    this.defs = [];
    this.api.getAll('DEF', false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.defs = result.data;
      }
    });
  }

  habilitarDesabilitarPedidoInterno(id: any, valor: boolean): void {
    this.api.habilitarDesabilitarPedidoInterno('DEF', id, valor, (result) => {
      if (result.status !== 200) {
        Swal.fire({ title: 'Erro', text: result.message, icon: 'error' });
      } else {
        this.listaDefs();
      }
    });
  }
}
