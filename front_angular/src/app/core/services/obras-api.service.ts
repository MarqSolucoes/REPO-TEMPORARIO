import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ObraResumo } from '../models/obra.models';

export interface ObraPayload {
  id?: number;
  descricao: string;
  codigoProposta?: string;
  numeroPedidoCliente?: string;
  enderecoDeEntrega?: string;
}

@Injectable({ providedIn: 'root' })
export class ObrasApiService {
  constructor(private readonly http: HttpClient) {}

  listarObras(apenasAtivos = true): Observable<ObraResumo[]> {
    return this.http.get<ObraResumo[]>('/Obra?apenasAtivos=' + apenasAtivos);
  }

  criarObra(payload: ObraPayload): Observable<unknown> {
    return this.http.post('/Obra', payload);
  }

  editarObra(payload: ObraPayload): Observable<unknown> {
    return this.http.put('/Obra', payload);
  }
}
