import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ObraResumo } from '../models/obra.models';

@Injectable({ providedIn: 'root' })
export class ObrasApiService {
  constructor(private readonly http: HttpClient) {}

  listarObras(apenasAtivos = true): Observable<ObraResumo[]> {
    return this.http.get<ObraResumo[]>('/Obra?apenasAtivos=' + apenasAtivos);
  }
}
