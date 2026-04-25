import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = environment.baseURLApi;

  constructor(private http: HttpClient) {}

  get(controller: string, id: any, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  getAll(controller: string, apenasAtivos: boolean, cb: (r: any) => void): void {
    this.http.get(`${this.base}/${controller}?apenasAtivos=${apenasAtivos}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  post(controller: string, dto: any, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}`, dto).subscribe({
      next: (data: any) => cb({ status: 201, message: data, data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  put(controller: string, dto: any, cb: (r: any) => void): void {
    this.http.put(`${this.base}/${controller}`, dto).subscribe({
      next: (data: any) => cb({ status: 201, message: data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  delete(controller: string, id: any, cb: (r: any) => void): void {
    this.http.delete(`${this.base}/${controller}/${id}`).subscribe({
      next: (data: any) => cb({ status: 200, message: '', data }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  ativarDesativar(controller: string, id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/AtivarDesativar?id=${id}&ativarDesativar=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }

  habilitarDesabilitarPedidoInterno(controller: string, id: any, valor: boolean, cb: (r: any) => void): void {
    this.http.post(`${this.base}/${controller}/HabilitarDesabilitar?id=${id}&habilitar=${valor}`, {}).subscribe({
      next: (data: any) => cb({ status: 200, message: '' }),
      error: (err) => cb({ status: err.status || 500, message: err.error || 'Erro de comunicação com o servidor' })
    });
  }
}
