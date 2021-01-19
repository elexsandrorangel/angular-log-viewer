import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Log} from '../models/log';
import {environment} from '../../environments/environment';

const baseURL = `${environment.apiUrl}/logs`;
@Injectable({
  providedIn: 'root'
})
export class LogsService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Log[]> {
    return this.httpClient.get<Log[]>(baseURL);
  }

  getById(id: string): Observable<any>{
    return this.httpClient.get(`${baseURL}/${id}`);
  }

  create(data): Observable<any> {
    return this.httpClient.post(baseURL, data);
  }

  update(id, data): Observable<any> {
    return this.httpClient.put(`${baseURL}/${id}`, data);
  }

  delete(id): Observable<any> {
    return this.httpClient.delete(`${baseURL}/${id}`);
  }

  importFile(data): Observable<any> {
    return this.httpClient.post(`${baseURL}/import`, data);
  }
}
