import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { OrdemCompraResumo } from '../../../core/models/ordem-compra.models';
import { OrdemCompraApiService } from '../../../core/services/ordem-compra-api.service';

@Component({
  selector: 'app-ordem-compra',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ordem-compra.component.html',
  styleUrl: './ordem-compra.component.scss',
})
export class OrdemCompraComponent implements OnInit {
  private readonly api = inject(OrdemCompraApiService);

  isLoading = false;
  errorMessage = '';
  ordens: OrdemCompraResumo[] = [];

  ngOnInit(): void {
    this.carregarOrdens();
  }

  carregarOrdens(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.api
      .listarOrdens(true)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (ordens) => (this.ordens = ordens),
        error: () => (this.errorMessage = 'Erro ao carregar ordens de compra.'),
      });
  }
}
