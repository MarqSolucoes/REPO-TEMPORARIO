import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrdemCompraResumo } from '../models/ordem-compra.models';

@Injectable({ providedIn: 'root' })
export class OrdemCompraApiService {
  constructor(private readonly http: HttpClient) {}

  listarOrdens(apenasAtivos = true): Observable<OrdemCompraResumo[]> {
    return this.http.get<OrdemCompraResumo[]>('/PedidoCompra?apenasAtivos=' + apenasAtivos);
  }
}
