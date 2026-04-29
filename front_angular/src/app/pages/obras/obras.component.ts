import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { ObraResumo } from '../../core/models/obra.models';
import { ObrasApiService } from '../../core/services/obras-api.service';

@Component({
  selector: 'app-obras',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './obras.component.html',
  styleUrl: './obras.component.scss',
})
export class ObrasComponent implements OnInit {
  private readonly obrasApi = inject(ObrasApiService);

  isLoading = false;
  errorMessage = '';
  obras: ObraResumo[] = [];

  ngOnInit(): void {
    this.carregarObras();
  }

  carregarObras(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.obrasApi
      .listarObras(true)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (obras) => (this.obras = obras),
        error: () => (this.errorMessage = 'Erro ao carregar obras.'),
      });
  }
}
