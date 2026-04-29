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
  rascunhoEmEdicaoId: number | null = null;

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
    const payload = {
      id: this.rascunhoEmEdicaoId ?? undefined,
      titulo: formValue.titulo,
      objetoSerializado: JSON.stringify({
        servico: formValue.tipo === 'Servico',
        dataEntrega: formValue.dataEntrega,
        titulo: formValue.titulo,
        valorTotal: formValue.valorTotal,
        descricao: formValue.observacao,
      }),
    };

    this.isLoading = true;
    const requisicao$ = this.rascunhoEmEdicaoId ? this.dashboardApi.atualizaRascunho(payload) : this.dashboardApi.salvaRascunho(payload);
    requisicao$
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.successMessage = this.rascunhoEmEdicaoId ? 'Rascunho atualizado com sucesso.' : 'Rascunho criado com sucesso.';
          this.rascunhoEmEdicaoId = null;
          this.obtemRascunhos();
        },
        error: () => {
          this.errorMessage = 'Erro ao salvar rascunho.';
        },
      });
  }

  continuaRascunho(row: RascunhoSolicitacao): void {
    try {
      const objeto = row.objetoSerializado ? JSON.parse(row.objetoSerializado) : null;
      this.rascunhoEmEdicaoId = row.id;
      this.modalNovoPedidoCompraTipoPedidoExibir = true;
      this.novaSolicitacaoForm.patchValue({
        tipo: objeto?.servico ? 'Servico' : 'Material',
        titulo: objeto?.titulo ?? row.titulo ?? '',
        dataEntrega: objeto?.dataEntrega ?? '',
        valorTotal: objeto?.valorTotal ?? 0,
        observacao: objeto?.descricao ?? '',
      });
      this.successMessage = `Rascunho "${row.titulo}" carregado para edição.`;
    } catch {
      this.errorMessage = 'Não foi possível carregar o rascunho selecionado.';
    }
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
