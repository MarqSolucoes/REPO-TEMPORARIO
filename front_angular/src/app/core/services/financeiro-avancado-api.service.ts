import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DefResumo, EtoResumo, FluxoCaixaConsolidadoItem, PedidoInternoResumo } from '../models/financeiro-avancado.models';

@Injectable({ providedIn: 'root' })
export class FinanceiroAvancadoApiService {
  constructor(private readonly http: HttpClient) {}

  obtemDefs(): Observable<DefResumo[]> { return this.http.get<DefResumo[]>('/Def?apenasAtivos=true'); }
  obtemEto(): Observable<EtoResumo[]> { return this.http.get<EtoResumo[]>('/RelatorioControleETO/false'); }
  obtemFluxoConsolidado(data: string): Observable<FluxoCaixaConsolidadoItem[]> { return this.http.post<FluxoCaixaConsolidadoItem[]>('/FluxoCaixa/ObtemFluxoCaixaConsolidado', { data }); }
  obtemPedidosInternosParaAprovacao(idUsuario = 0): Observable<PedidoInternoResumo[]> { return this.http.get<PedidoInternoResumo[]>('/PedidoInterno/ObtemPedidosInternosParaAprovacao/' + idUsuario); }
  obtemProximoCodigoPedidoInterno(): Observable<string> { return this.http.get('/PedidoInterno/ProximoCodigo', { responseType: 'text' }); }
}
