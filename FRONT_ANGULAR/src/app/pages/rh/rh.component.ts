import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rh',
  standalone: false,
  templateUrl: './rh.component.html',
  styleUrls: ['./rh.component.scss']
})
export class RhComponent implements OnInit {
  isLoading = false;
  funcionarios: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.isLoading = true;
    this.funcionarios = [];
    this.api.getAll('RH', false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.funcionarios = result.data;
      }
    });
  }
}
