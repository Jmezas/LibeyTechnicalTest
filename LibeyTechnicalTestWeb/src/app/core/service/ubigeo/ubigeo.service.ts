import { Province } from './../../../entities/province';
import { Region } from './../../../entities/region';
import { Ubigeo } from '../../../entities/ubigeo';
import { Observable } from 'rxjs';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Result } from 'src/app/entities/result';

@Injectable({
  providedIn: 'root',
})
export class UbigeoService {
  url!: string;
  constructor(private http: HttpClient) {
    this.url = environment.pathLibeyTechnicalTest + 'Ubigeo/';
  }

  getRegions(): Observable<Result> {
    return this.http.get<Result>(this.url + 'regions');
  }

  getProvinces(regionId: string): Observable<Result> {
    return this.http.get<Result>(this.url + 'provinces/' + regionId);
  }

  getDistricts(regionId: string, provinceId: string): Observable<Result> {
    return this.http.get<Result>(
      this.url + 'districts/' + regionId + '/' + provinceId
    );
  }
}
