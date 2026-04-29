import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ObraResumo } from '../../core/models/obra.models';
import { ObraPayload, ObrasApiService } from '../../core/services/obras-api.service';

@Component({
  selector: 'app-obras',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './obras.component.html',
  styleUrl: './obras.component.scss',
})
export class ObrasComponent implements OnInit {
  private readonly obrasApi = inject(ObrasApiService);
  private readonly fb = inject(FormBuilder);

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  obras: ObraResumo[] = [];
  obraEmEdicaoId: number | null = null;

  obraForm = this.fb.nonNullable.group({
    descricao: ['', [Validators.required, Validators.minLength(3)]],
    codigoProposta: [''],
    numeroPedidoCliente: [''],
    enderecoDeEntrega: [''],
  });

  ngOnInit(): void {
    this.carregarObras();
  }

  carregarObras(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.obrasApi
      .listarObras(true)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (obras) => (this.obras = obras),
        error: () => (this.errorMessage = 'Erro ao carregar obras.'),
      });
  }

  prepararNovaObra(): void {
    this.obraEmEdicaoId = null;
    this.obraForm.reset({
      descricao: '',
      codigoProposta: '',
      numeroPedidoCliente: '',
      enderecoDeEntrega: '',
    });
  }

  editarObra(obra: ObraResumo): void {
    this.obraEmEdicaoId = obra.id;
    this.obraForm.patchValue({
      descricao: obra.descricao ?? '',
      codigoProposta: obra.codigo ?? '',
      numeroPedidoCliente: '',
      enderecoDeEntrega: '',
    });
  }

  salvarObra(): void {
    this.successMessage = '';
    this.errorMessage = '';

    if (this.obraForm.invalid) {
      this.obraForm.markAllAsTouched();
      return;
    }

    const payload: ObraPayload = {
      id: this.obraEmEdicaoId ?? undefined,
      ...this.obraForm.getRawValue(),
    };

    this.isLoading = true;
    const request$ = this.obraEmEdicaoId ? this.obrasApi.editarObra(payload) : this.obrasApi.criarObra(payload);
    request$
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.successMessage = this.obraEmEdicaoId ? 'Obra atualizada com sucesso.' : 'Obra cadastrada com sucesso.';
          this.prepararNovaObra();
          this.carregarObras();
        },
        error: () => {
          this.errorMessage = 'Erro ao salvar obra.';
        },
      });
  }
}
