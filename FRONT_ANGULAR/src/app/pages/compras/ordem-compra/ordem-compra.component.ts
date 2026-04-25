import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-ordem-compra',
  standalone: false,
  templateUrl: './ordem-compra.component.html',
  styleUrls: ['./ordem-compra.component.scss']
})
export class OrdemCompraComponent implements OnInit {
  isLoading = false;
  ordens: any[] = [];
  tag = '';
  usuarioDTO: any = {};

  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.tag = this.route.snapshot.params['tag'] || 'EmCotacao';
    this.usuarioDTO = JSON.parse(localStorage.getItem('usuarioDTO') || '{}');
    this.getAll();
  }

  trackByIndex(i: number): number { return i; }

  getAll(): void {
    this.isLoading = true;
    this.ordens = [];
    this.api.getAll(`OrdemCompra/${this.tag}`, false, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.ordens = result.data;
      }
    });
  }

  verCotacao(id: any): void {
    this.router.navigate(['/app/compras/cotacao', id]);
  }
}
