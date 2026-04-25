import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-conciliacao',
  standalone: false,
  templateUrl: './conciliacao.component.html',
  styleUrls: ['./conciliacao.component.scss']
})
export class ConciliacaoComponent implements OnInit {
  isLoading = false;
  itens: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.isLoading = true;
    this.itens = [];
    this.api.getAll('Conciliacao', false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.itens = result.data;
      }
    });
  }
}
