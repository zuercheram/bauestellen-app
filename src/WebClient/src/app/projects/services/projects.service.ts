import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../models/project.interface';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {
  private apiBase: string = 'projects-api';

  constructor(private http: HttpClient) {}

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiBase}/api/projects`);
  }
}
