import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cotacao',
  standalone: false,
  templateUrl: './cotacao.component.html',
  styleUrls: ['./cotacao.component.scss']
})
export class CotacaoComponent implements OnInit {
  isLoading = false;
  cotacao: any = null;
  id = 0;

  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.params['id'];
    this.loadCotacao();
  }

  loadCotacao(): void {
    this.isLoading = true;
    this.api.get('SolicitacaoCompra', this.id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.cotacao = result.data;
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/app/compras/ordemCompra/EmCotacao']);
  }
}
