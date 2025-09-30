import { LibeyUserResponse } from './../../../entities/libeyuser';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LibeyUserService } from '../../../core/service/libeyuser/libeyuser.service';
import swal from 'sweetalert2';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  listUsers: LibeyUserResponse[] = [];
  filteredUsers: LibeyUserResponse[] = [];
  searchTerm: string = '';
  isLoading: boolean = false;

  // Paginación
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  Math: any;

  constructor(
    private router: Router,
    private libeyUserService: LibeyUserService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.libeyUserService.List().subscribe({
      next: (response) => {
        this.isLoading = false;
        this.listUsers = response.data;
        this.filteredUsers = [...this.listUsers];
        this.totalItems = this.listUsers.length;
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error al cargar usuarios:', error);
      },
    });
  }

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      this.filteredUsers = [...this.listUsers];
    } else {
      const term = this.searchTerm.toLowerCase().trim();
      this.filteredUsers = this.listUsers.filter(
        (user) =>
          user.documentNumber.toLowerCase().includes(term) ||
          user.name.toLowerCase().includes(term) ||
          user.fathersLastName.toLowerCase().includes(term) ||
          user.mothersLastName.toLowerCase().includes(term) ||
          user.email.toLowerCase().includes(term) ||
          user.phone.includes(term) ||
          user.region.regionDescription.toLowerCase().includes(term) ||
          user.province.provinceDescription.toLowerCase().includes(term) ||
          user.ubigeo.ubigeoDescription.toLowerCase().includes(term) ||
          user.documentType.documentTypeDescription.toLowerCase().includes(term)
      );
    }
    this.totalItems = this.filteredUsers.length;
    this.currentPage = 1;
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.filteredUsers = [...this.listUsers];
    this.totalItems = this.listUsers.length;
    this.currentPage = 1;
  }

  onNew(): void {
    this.router.navigate(['/user/maintenance']);
  }

  onEdit(user: LibeyUserResponse): void {
    this.router.navigate(['/user/maintenance', user.documentNumber]);
  }

  onDelete(user: LibeyUserResponse): void {
    swal
      .fire({
        title: '¿Está seguro?',
        text: `¿Desea eliminar al usuario ${user.name} ${user.fathersLastName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#38E8C6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
      })
      .then((result) => {
        if (result.isConfirmed) {
          this.deleteUser(user.documentNumber);
        }
      });
  }

  deleteUser(userId: string): void {
    this.libeyUserService.Delete(userId).subscribe({
      next: (response) => {
        if (response.success) {
          swal.fire('Eliminado', 'El usuario ha sido eliminado.', 'success');
          this.loadUsers();
        } else {
          swal.fire('Error', response.data.message || 'No se pudo eliminar el usuario', 'error');
        }
      },
      error: (error) => {
        console.error('Error al eliminar usuario:', error);
        swal.fire('Error', 'Ocurrió un error al eliminar el usuario', 'error');
      }
    });
  }

  // Métodos de paginación
  get paginatedUsers(): LibeyUserResponse[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredUsers.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
}
