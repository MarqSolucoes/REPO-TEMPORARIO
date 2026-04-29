import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cargo } from '../models/cargo.models';

@Injectable({ providedIn: 'root' })
export class CargoApiService {
  constructor(private readonly http: HttpClient) {}
  listar(apenasAtivos = true): Observable<Cargo[]> { return this.http.get<Cargo[]>('/Cargo?apenasAtivos=' + apenasAtivos); }
  salvar(payload: Partial<Cargo>): Observable<unknown> { return payload.id ? this.http.put('/Cargo', payload) : this.http.post('/Cargo', payload); }
}
