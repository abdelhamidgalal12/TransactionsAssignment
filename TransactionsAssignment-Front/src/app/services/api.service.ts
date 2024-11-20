import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7014/api';
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth`, { username, password });
  }
  generateEncryptionKey(): Observable<any> {
    return this.http.get(`${this.apiUrl}/transaction/generatekey`);
  }
  processTransaction(encryptedData: string, encryptionKey: string, token: string): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post(`${this.apiUrl}/transaction/process`, {
      EncryptedTransactionData: encryptedData,
      EncryptionKey: encryptionKey
    }, { headers });
  }
}