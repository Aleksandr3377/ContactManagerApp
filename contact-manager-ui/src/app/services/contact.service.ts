import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Contact {
  id: number;
  name: string;
  dateOfBirth: string;
  married: boolean;
  phone: string;
  salary: number;
}

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl = 'https://localhost:44389/api/contacts';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.apiUrl);
  }

  update(contact: Contact): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${contact.id}`, contact);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
