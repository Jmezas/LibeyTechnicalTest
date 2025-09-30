import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LibeyUser } from 'src/app/entities/libeyuser';
import { Result } from 'src/app/entities/result';
@Injectable({
  providedIn: 'root',
})
export class LibeyUserService {
  constructor(private http: HttpClient) {}
  Find(documentNumber: string): Observable<Result> {
    const uri = `${environment.pathLibeyTechnicalTest}LibeyUser/${documentNumber}`;
    return this.http.get<Result>(uri);
  }

  Create(user: LibeyUser): Observable<Result> {
    const uri = `${environment.pathLibeyTechnicalTest}LibeyUser`;
    return this.http.post<Result>(uri, user);
  }

  Update(user: LibeyUser): Observable<Result> {
    const uri = `${environment.pathLibeyTechnicalTest}LibeyUser`;
    return this.http.put<Result>(uri, user);
  }

  Delete(documentNumber: string): Observable<Result> {
    const uri = `${environment.pathLibeyTechnicalTest}LibeyUser/${documentNumber}`;
    return this.http.delete<Result>(uri);
  }

  List(): Observable<Result> {
    const uri = `${environment.pathLibeyTechnicalTest}LibeyUser/GetAllUsers`;
    return this.http.get<Result>(uri);
  }
}
