import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { ConciliacaoResumo } from '../../../core/models/compras.models';
import { ComprasApiService } from '../../../core/services/compras-api.service';

@Component({
  selector: 'app-conciliacao',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './conciliacao.component.html',
  styleUrl: './conciliacao.component.scss',
})
export class ConciliacaoComponent implements OnInit {
  private readonly api = inject(ComprasApiService);
  private readonly fb = inject(FormBuilder);

  isLoading = false;
  errorMessage = '';
  conciliacoes: ConciliacaoResumo[] = [];

  filtroForm = this.fb.group({ termo: [''] });

  ngOnInit(): void { this.buscar(); }

  buscar(): void {
    this.isLoading = true;
    const filtro = { termo: this.filtroForm.getRawValue().termo || '' };
    this.api.listarConciliacoes(filtro).pipe(finalize(() => (this.isLoading = false))).subscribe({
      next: (conciliacoes) => (this.conciliacoes = conciliacoes),
      error: () => (this.errorMessage = 'Erro ao carregar conciliações.'),
    });
  }
}
