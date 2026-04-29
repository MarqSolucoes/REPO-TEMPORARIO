import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { DashboardApiService } from '../../core/services/dashboard-api.service';
import { NotaFiscalVencimento, RascunhoSolicitacao } from '../../core/models/dashboard.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  private readonly dashboardApi = inject(DashboardApiService);
  private readonly formBuilder = inject(FormBuilder);

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  modalEdicaoDataNotaFiscalExibir = false;
  modalNovoPedidoCompraTipoPedidoExibir = false;

  modalEdicaoDataNotaFiscalNotas: NotaFiscalVencimento[] = [];
  rascunhosSolicitacoes: RascunhoSolicitacao[] = [];

  novaSolicitacaoForm = this.formBuilder.nonNullable.group({
    tipo: this.formBuilder.nonNullable.control<'Material' | 'Servico'>('Material'),
    titulo: ['', [Validators.required, Validators.minLength(3)]],
    dataEntrega: ['', Validators.required],
    valorTotal: [0, [Validators.required, Validators.min(0)]],
    observacao: [''],
  });

  ngOnInit(): void {
    this.obtemRascunhos();
  }

  obtemRascunhos(): void {
    this.errorMessage = '';
    this.isLoading = true;
    this.dashboardApi
      .obtemRascunhos()
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (rascunhos) => {
          this.rascunhosSolicitacoes = rascunhos;
        },
        error: () => {
          this.errorMessage = 'Erro ao carregar rascunhos.';
        },
      });
  }

  abrirModalEdicaoDataNotaFiscal(): void {
    this.modalEdicaoDataNotaFiscalExibir = true;
  }

  fecharModalEdicaoDataNotaFiscal(): void {
    this.modalEdicaoDataNotaFiscalExibir = false;
  }

  abrirModalTipoSolicitacao(): void {
    this.successMessage = '';
    this.modalNovoPedidoCompraTipoPedidoExibir = true;
  }

  fecharModalTipoSolicitacao(): void {
    this.modalNovoPedidoCompraTipoPedidoExibir = false;
  }

  editarDataVencimentoNF(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;
    this.dashboardApi
      .editarDataVencimentoNF(this.modalEdicaoDataNotaFiscalNotas)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.successMessage = 'Datas de vencimento atualizadas com sucesso.';
          this.fecharModalEdicaoDataNotaFiscal();
        },
        error: () => {
          this.errorMessage = 'Erro ao salvar edição de vencimento.';
        },
      });
  }

  abreModalNovaSolicitacao(isServico: boolean): void {
    this.novaSolicitacaoForm.reset({
      tipo: isServico ? 'Servico' : 'Material',
      titulo: '',
      dataEntrega: '',
      valorTotal: 0,
      observacao: '',
    });
  }

  salvarNovaSolicitacaoRascunho(): void {
    this.successMessage = '';

    if (this.novaSolicitacaoForm.invalid) {
      this.novaSolicitacaoForm.markAllAsTouched();
      return;
    }

    const formValue = this.novaSolicitacaoForm.getRawValue();
    this.successMessage = `Rascunho ${formValue.tipo.toLowerCase()} pronto para integração de API.`;
  }

  continuaRascunho(row: RascunhoSolicitacao): void {
    this.successMessage = `Fluxo de edição do rascunho \"${row.titulo}\" será migrado no próximo passo.`;
  }

  excluiRascunho(id: number): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;
    this.dashboardApi
      .excluiRascunho(id)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.successMessage = 'Rascunho excluído com sucesso.';
          this.obtemRascunhos();
        },
        error: () => {
          this.errorMessage = 'Erro ao excluir rascunho.';
        },
      });
  }

  formataData(value: string): string {
    return new Date(value).toLocaleDateString('pt-BR');
  }
}
