import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ObraResumo } from '../../core/models/obra.models';
import { ObraPayload, ObrasApiService } from '../../core/services/obras-api.service';

@Component({
  selector: 'app-obra-detalhe',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './obra-detalhe.component.html',
  styleUrl: './obra-detalhe.component.scss',
})
export class ObraDetalheComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly obrasApi = inject(ObrasApiService);
  private readonly fb = inject(FormBuilder);

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  obraId = 0;
  obra: ObraResumo | null = null;

  detalheForm = this.fb.nonNullable.group({
    descricao: ['', [Validators.required, Validators.minLength(3)]],
  });

  ngOnInit(): void {
    this.obraId = Number(this.route.snapshot.paramMap.get('id') || 0);
    this.carregar();
  }

  carregar(): void {
    this.isLoading = true;
    this.obrasApi
      .listarObras(false)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (obras) => {
          this.obra = obras.find((o) => o.id === this.obraId) ?? null;
          if (!this.obra) {
            this.errorMessage = 'Obra não encontrada.';
            return;
          }
          this.detalheForm.patchValue({ descricao: this.obra.descricao });
        },
        error: () => (this.errorMessage = 'Erro ao carregar detalhe da obra.'),
      });
  }

  salvar(): void {
    if (this.detalheForm.invalid || !this.obra) {
      this.detalheForm.markAllAsTouched();
      return;
    }

    const payload: ObraPayload = {
      id: this.obra.id,
      descricao: this.detalheForm.getRawValue().descricao,
      codigoProposta: this.obra.codigo,
    };

    this.isLoading = true;
    this.obrasApi
      .editarObra(payload)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => (this.successMessage = 'Informações da obra atualizadas com sucesso.'),
        error: () => (this.errorMessage = 'Erro ao atualizar obra.'),
      });
  }
}
