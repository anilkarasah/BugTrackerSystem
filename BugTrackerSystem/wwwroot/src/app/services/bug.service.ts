import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Bug, UpsertBug } from '../models/bug.model';
import { environment } from 'src/environments/environment';

const apiBugsUrl = `${environment.API_URL}/bugs`;

@Injectable({
  providedIn: 'root',
})
export class BugService {
  constructor(private httpClient: HttpClient) {}

  getAllBugs(): Observable<Bug[]> {
    return this.httpClient.get<Bug[]>(`${apiBugsUrl}`);
  }

  getBugById(id: Number): Observable<Bug> {
    return this.httpClient.get<Bug>(`${apiBugsUrl}/${id}`);
  }

  reportBug(bug: any): Observable<Bug> {
    return this.httpClient.post<Bug>(`${apiBugsUrl}`, {
      title: bug.title,
      description: bug.description,
      projectId: bug.projectId,
    });
  }

  upsertBug(bug: UpsertBug, id: Number): Observable<Bug> {
    return this.httpClient.patch<Bug>(`${apiBugsUrl}/${id}`, {
      title: bug.title,
      description: bug.description,
      status: bug.status,
    });
  }

  deleteBug(bug: Bug): Observable<Bug> {
    return this.httpClient.delete<Bug>(`${apiBugsUrl}/${bug.id}`);
  }

  getListOfProjects(): Observable<any> {
    return this.httpClient.get<any>(`${environment.API_URL}/projects/bugs`);
  }
}
