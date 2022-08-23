import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Bug, UpsertBug } from '../models/bug.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BugService {
  constructor(private httpClient: HttpClient) {}

  getAllBugs(): Observable<Bug[]> {
    return this.httpClient.get<Bug[]>(`${environment.API_URL}/bugs`);
  }

  getBugById(id: Number): Observable<Bug> {
    return this.httpClient.get<Bug>(`${environment.API_URL}/bugs/${id}`);
  }

  upsertBug(bug: UpsertBug, id: Number): Observable<Bug> {
    return this.httpClient.patch<Bug>(
      `${environment.API_URL}/bugs/${id}`,
      {
        title: bug.title,
        description: bug.description,
        status: bug.status,
      },
      {
        withCredentials: true,
      }
    );
  }

  deleteBug(bug: Bug): Observable<any> {
    return this.httpClient.delete<Bug>(
      `${environment.API_URL}/bugs/${bug.id}`,
      {
        withCredentials: true,
      }
    );
  }
}
