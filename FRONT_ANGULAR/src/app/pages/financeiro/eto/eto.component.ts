import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-eto',
  standalone: false,
  templateUrl: './eto.component.html',
  styleUrls: ['./eto.component.scss']
})
export class EtoComponent implements OnInit {
  isLoading = false;
  ajustes: any[] = [];

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.obtemAjustes();
  }

  trackByIndex(i: number): number { return i; }

  formataData(d: any): string {
    if (!d) return '';
    const dt = new Date(d);
    return `${dt.toLocaleDateString('pt-BR')} ${dt.toLocaleTimeString('pt-BR')}`;
  }

  obtemAjustes(): void {
    this.isLoading = true;
    this.api.obtemAjustes((result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.data || result.message, icon: 'error' });
      } else {
        this.ajustes = result.data;
      }
    });
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;
    this.isLoading = true;
    const formData = new FormData();
    formData.append('arquivo', input.files[0]);
    this.api.uploadAjusteETO(formData, () => {
      this.isLoading = false;
      input.value = '';
      Swal.fire({ title: '', text: 'Processamento concluído', icon: 'success' });
      this.obtemAjustes();
    });
  }

  downloadExcel(): void {
    this.api.downloadExcelAjusteETO((result) => {
      if (result.status !== 200) {
        Swal.fire({ title: '', text: 'Erro ao baixar excel', icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }

  downloadArquivo(id: any, tipo: number): void {
    this.isLoading = true;
    this.api.downloadArquivoAjusteETO(id, tipo, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        const blob = new Blob([result.data], { type: result.contentType });
        window.open(URL.createObjectURL(blob), '_blank');
      }
    });
  }
}
