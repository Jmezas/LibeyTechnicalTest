import { DocumentTypeService } from './../../../core/service/documentType/document-type.service';
import swal from 'sweetalert2';
import { Component, OnInit } from '@angular/core';
import { DocumentDto } from '../../../entities/documentDto';
import { UbigeoService } from '../../../core/service/ubigeo/ubigeo.service';
import { Region } from '../../../entities/region';
import { Province } from '../../../entities/province';
import { Ubigeo } from '../../../entities/ubigeo';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LibeyUserService } from '../../../core/service/libeyuser/libeyuser.service';
import { LibeyUser } from 'src/app/entities/libeyuser';
import { Result } from 'src/app/entities/result';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-usermaintenance',
  templateUrl: './usermaintenance.component.html',
  styleUrls: ['./usermaintenance.component.css'],
})
export class UsermaintenanceComponent implements OnInit {
  userForm!: FormGroup;
  listDocumentTypes: DocumentDto[] = [];
  listRegions: Region[] = [];
  listProvinces: Province[] = [];
  listDistricts: Ubigeo[] = [];

  isEditMode: boolean = false;
  userId: string = '';
  isLoading: boolean = false;

  constructor(
    private documentTypeService: DocumentTypeService,
    private ubigeoService: UbigeoService,
    private fb: FormBuilder,
    private libeyUserService: LibeyUserService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.getDocumentTypes();
    this.getRegions();
    this.setupFormSubscriptions();
    this.checkEditMode();
  }

  checkEditMode(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.userId = params['id'];
        this.loadUserData(this.userId);
      }
    });
  }

  loadUserData(userId: string): void {
    this.isLoading = true;
    this.libeyUserService.Find(userId).subscribe({
      next: (response: Result) => {
        if (response.success && response.data) {
          const user = response.data ;

          // Cargar provincias y distritos según la región seleccionada
          if (user.ubigeo) {
            this.getProvinces(user.ubigeo.provinceCode);

            if (user.ubigeo.ubigeoCode) {
              this.getDistricts(user.ubigeo.regionCode, user.ubigeo.provinceCode);
            }
          }

          // Poblar el formulario con los datos del usuario
          this.userForm.patchValue({
            documentTypeId: user.documentTypeId,
            documentNumber: user.documentNumber,
            firstName: user.name,
            paternalSurname: user.fathersLastName,
            maternalSurname: user.mothersLastName,
            address: user.address,
            regionCode: user.ubigeo.regionCode,
            provinceCode: user.ubigeo.provinceCode,
            districtCode: user.ubigeo.ubigeoCode,
            phone: user.phone,
            email: user.email,
            password: '' // No cargar la contraseña por seguridad
          });

          this.isLoading = false;
        }
      },
      error: (error) => {
        console.error('Error al cargar usuario:', error);
        swal.fire('Error', 'No se pudo cargar los datos del usuario', 'error');
        this.isLoading = false;
        this.router.navigate(['/users']);
      }
    });
  }

  Submit(): void {
    if (this.userForm.valid) {
      const formData = this.userForm.value;

      const user: LibeyUser = {
        documentTypeId: formData.documentTypeId,
        documentNumber: formData.documentNumber,
        name: formData.firstName,
        fathersLastName: formData.paternalSurname,
        mothersLastName: formData.maternalSurname,
        address: formData.address,
        regionCode: formData.regionCode,
        provinceCode: formData.provinceCode,
        ubigeoCode: formData.districtCode,
        phone: formData.phone,
        email: formData.email,
        password: formData.password,
        active: true,
      };

      if (this.isEditMode && this.userId) {
        // Actualizar usuario existente
        this.updateUser(user);
      } else {
        // Crear nuevo usuario
        this.createUser(user);
      }
    } else {
      this.markFormGroupTouched(this.userForm);
      swal.fire(
        'Error',
        'Por favor complete todos los campos requeridos',
        'error'
      );
    }
  }

  createUser(user: LibeyUser): void {
    this.libeyUserService.Create(user).subscribe({
      next: (response: Result) => {
        if (response.success) {
          swal.fire({
            title: 'Éxito',
            text: 'Usuario registrado correctamente',
            icon: 'success',
            confirmButtonColor: '#38E8C6'
          }).then(() => {
            this.router.navigate(['/user/list']);
          });
        } else {
          swal.fire('Error', response.data.message || 'No se pudo crear el usuario', 'error');
        }
      },
      error: (error) => {
        console.error('Error al crear usuario:',  error.data.message);
        swal.fire('Error', 'Ocurrió un error al crear el usuario', 'error');
      }
    });
  }

  updateUser( user: LibeyUser): void {
    this.libeyUserService.Update(user).subscribe({
      next: (response: Result) => {
        if (response.success) {
          swal.fire({
            title: 'Éxito',
            text: 'Usuario actualizado correctamente',
            icon: 'success',
            confirmButtonColor: '#38E8C6'
          }).then(() => {
            this.router.navigate(['/user/list']);
          });
        } else {
          swal.fire('Error', response.data.message || 'No se pudo actualizar el usuario', 'error');
        }
      },
      error: (error) => {
        console.error('Error al actualizar usuario:', error.data.message);
        swal.fire('Error', 'Ocurrió un error al actualizar el usuario', 'error');
      }
    });
  }

  createForm(): FormGroup {
    return (this.userForm = this.fb.group({
      documentTypeId: [null, Validators.required],
      documentNumber: [
        '',
        [Validators.required, Validators.pattern(/^[0-9]{8}$/)],
      ],
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      paternalSurname: ['', [Validators.required, Validators.minLength(2)]],
      maternalSurname: ['', Validators.required],
      address: ['', Validators.required],
      regionCode: [null, Validators.required],
      provinceCode: [null, Validators.required],
      districtCode: [null, Validators.required],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{9}$/)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    }));
  }

  getDocumentTypes(): void {
    this.documentTypeService.getDocumentTypes().subscribe((data: Result) => {
      this.listDocumentTypes = data.data;
    });
  }

  getRegions(): void {
    this.ubigeoService.getRegions().subscribe((data: Result) => {
      this.listRegions = data.data;
    });
  }

  getProvinces(regionId: string): void {
    this.ubigeoService.getProvinces(regionId).subscribe((data: Result) => {
      this.listProvinces = data.data;
    });
  }

  getDistricts(regionId: string, provinceId: string): void {
    this.ubigeoService.getDistricts(regionId, provinceId).subscribe((data: Result) => {
      this.listDistricts = data.data;
    });
  }

  setupFormSubscriptions(): void {
    this.userForm.get('regionCode')?.valueChanges.subscribe((regionCode) => {
      if (regionCode) {
        this.getProvinces(regionCode);
        if (!this.isLoading) {
          this.userForm.patchValue({
            provinceCode: null,
            districtCode: null,
          });
          this.listDistricts = [];
        }
      }
    });

    this.userForm.get('provinceCode')?.valueChanges.subscribe((provinceCode) => {
      if (provinceCode) {
        const regionCode = this.userForm.get('regionCode')?.value;
        this.getDistricts(regionCode, provinceCode);
        if (!this.isLoading) {
          this.userForm.patchValue({
            districtCode: null,
          });
        }
      }
    });
  }

  onClear(): void {
    this.userForm.reset();
    this.listProvinces = [];
    this.listDistricts = [];
  }

  onBack(): void {
    window.history.back();
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach((key) => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.userForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getErrorMessage(fieldName: string): string {
    const field = this.userForm.get(fieldName);
    if (field?.hasError('required')) {
      return 'Este campo es requerido';
    }
    if (field?.hasError('email')) {
      return 'Ingrese un email válido';
    }
    if (field?.hasError('minlength')) {
      const minLength = field.errors?.['minlength'].requiredLength;
      return `Mínimo ${minLength} caracteres`;
    }
    if (field?.hasError('pattern')) {
      if (fieldName === 'documentNumber') {
        return 'debe tener 8 dígitos';
      }
      if (fieldName === 'phone') {
        return 'Debe tener 9 dígitos';
      }
    }
    return '';
  }

  // Getter para usar en el template
  get formTitle(): string {
    return this.isEditMode ? 'Editar Usuario' : 'Nuevo Usuario';
  }

  get submitButtonText(): string {
    return this.isEditMode ? 'Actualizar' : 'Guardar';
  }
}
