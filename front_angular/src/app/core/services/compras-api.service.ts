import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CotacaoResumo, ConciliacaoResumo } from '../models/compras.models';

@Injectable({ providedIn: 'root' })
export class ComprasApiService {
  constructor(private readonly http: HttpClient) {}

  listarCotacoes(idStatus = 1): Observable<CotacaoResumo[]> {
    return this.http.get<CotacaoResumo[]>('/SolicitacaoCompra/ParaCotacao/' + idStatus);
  }

  listarConciliacoes(filtro: Record<string, unknown>): Observable<ConciliacaoResumo[]> {
    return this.http.post<ConciliacaoResumo[]>('/PedidoCompra/ObtemNotasConciliadas', filtro);
  }
}
