import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-obra-detalhe',
  standalone: false,
  templateUrl: './obra-detalhe.component.html',
  styleUrls: ['./obra-detalhe.component.scss']
})
export class ObraDetalheComponent implements OnInit {
  isLoading = false;
  obra: any = null;
  id = 0;

  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.params['id'];
    this.loadObra();
  }

  loadObra(): void {
    this.isLoading = true;
    this.api.get('Obra', this.id, (result) => {
      this.isLoading = false;
      if (result.status !== 200) {
        Swal.fire({ title: '', text: result.message, icon: 'error' });
      } else {
        this.obra = result.data;
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/app/obras']);
  }
}
