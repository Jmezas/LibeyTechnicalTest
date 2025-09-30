import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentDto } from 'src/app/entities/documentDto';
import { Result } from 'src/app/entities/result';

@Injectable({
  providedIn: 'root',
})
export class DocumentTypeService {
  url!: string;
  constructor(private http: HttpClient) {
    this.url = environment.pathLibeyTechnicalTest + 'DocumentType/';
  }
  getDocumentTypes(): Observable<Result> {
    return this.http.get<Result>(this.url + 'GetAllDocumentTypes');
  }
}
