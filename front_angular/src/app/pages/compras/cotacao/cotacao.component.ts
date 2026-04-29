import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { CotacaoResumo } from '../../../core/models/compras.models';
import { ComprasApiService } from '../../../core/services/compras-api.service';

@Component({
  selector: 'app-cotacao',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cotacao.component.html',
  styleUrl: './cotacao.component.scss',
})
export class CotacaoComponent implements OnInit {
  private readonly api = inject(ComprasApiService);
  isLoading = false;
  errorMessage = '';
  cotacoes: CotacaoResumo[] = [];

  ngOnInit(): void { this.carregar(); }

  carregar(): void {
    this.isLoading = true;
    this.api.listarCotacoes(1).pipe(finalize(() => (this.isLoading = false))).subscribe({
      next: (cotacoes) => (this.cotacoes = cotacoes),
      error: () => (this.errorMessage = 'Erro ao carregar cotações.'),
    });
  }
}
