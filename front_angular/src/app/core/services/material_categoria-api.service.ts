import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MaterialCategoria } from '../models/material_categoria.models';

@Injectable({ providedIn: 'root' })
export class MaterialCategoriaApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<MaterialCategoria[]> { return this.http.get<MaterialCategoria[]>('/MaterialCategoria?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<MaterialCategoria>): Observable<unknown> { return payload.id ? this.http.put('/MaterialCategoria', payload) : this.http.post('/MaterialCategoria', payload); }
}
