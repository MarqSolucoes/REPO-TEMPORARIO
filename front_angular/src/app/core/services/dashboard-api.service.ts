import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NotaFiscalVencimento, RascunhoSolicitacao } from '../models/dashboard.models';

interface SalvarRascunhoPayload {
  id?: number;
  titulo: string;
  objetoSerializado: string;
}

@Injectable({ providedIn: 'root' })
export class DashboardApiService {
  constructor(private readonly http: HttpClient) {}

  obtemRascunhos(): Observable<RascunhoSolicitacao[]> {
    return this.http.get<RascunhoSolicitacao[]>('/SolicitacaoCompraRascunho');
  }

  excluiRascunho(id: number): Observable<unknown> {
    return this.http.delete(`/SolicitacaoCompraRascunho/${id}`);
  }

  salvaRascunho(payload: SalvarRascunhoPayload): Observable<unknown> {
    return this.http.post('/SolicitacaoCompraRascunho', payload);
  }

  atualizaRascunho(payload: SalvarRascunhoPayload): Observable<unknown> {
    return this.http.put('/SolicitacaoCompraRascunho', payload);
  }

  editarDataVencimentoNF(notas: NotaFiscalVencimento[]): Observable<unknown> {
    return this.http.post('/NotaFiscal/EditarDataVencimento', notas);
  }
}
