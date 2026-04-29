import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NotaFiscalVencimento, RascunhoSolicitacao } from '../models/dashboard.models';

@Injectable({ providedIn: 'root' })
export class DashboardApiService {
  constructor(private readonly http: HttpClient) {}

  obtemRascunhos(): Observable<RascunhoSolicitacao[]> {
    return this.http.get<RascunhoSolicitacao[]>('/SolicitacaoCompraRascunho');
  }

  excluiRascunho(id: number): Observable<unknown> {
    return this.http.delete(`/SolicitacaoCompraRascunho/${id}`);
  }

  editarDataVencimentoNF(notas: NotaFiscalVencimento[]): Observable<unknown> {
    return this.http.post('/NotaFiscal/EditarDataVencimento', notas);
  }
}
