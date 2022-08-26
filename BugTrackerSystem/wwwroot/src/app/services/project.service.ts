import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/project.model';
import { Observable } from 'rxjs';

const apiProjectUrl = `${environment.API_URL}/projects`;

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  constructor(private http: HttpClient) {}

  getAllProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(apiProjectUrl);
  }

  getProjectById(id: string): Observable<Project> {
    return this.http.get<Project>(`${apiProjectUrl}/${id}`);
  }

  addContributor(contributorId: string, projectId: string): Observable<any> {
    return this.http.post<any>(
      `${apiProjectUrl}/${projectId}/contributor/${contributorId}`,
      null
    );
  }

  removeContributor(contributorId: string, projectId: string): Observable<any> {
    return this.http.delete<any>(
      `${apiProjectUrl}/${projectId}/contributor/${contributorId}`
    );
  }
}
