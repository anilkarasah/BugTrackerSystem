import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import Bug from '../models/bug.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BugService {
  constructor(private httpClient: HttpClient) {}

  getAllBugs(): Observable<Bug[]> {
    return this.httpClient.get<Bug[]>(`${environment.API_URL}/bugs`);
  }
}
