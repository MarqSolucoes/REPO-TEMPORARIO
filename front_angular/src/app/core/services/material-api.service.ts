import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Material } from '../models/material.models';

@Injectable({ providedIn: 'root' })
export class MaterialApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Material[]> { return this.http.get<Material[]>('/Material?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Material>): Observable<unknown> { return payload.id ? this.http.put('/Material', payload) : this.http.post('/Material', payload); }
}
