import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../models/project.interface';
import { ApiBaseService } from '../../core/services/apiBase.service';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService extends ApiBaseService {
  private apiBase: string = 'projects-api';

  public projects = signal<Project[]>([]);

  fetchProjects() {
    const url = `${this.apiBase}/api/projects`;
    this.get<Project[]>(url).then((res) => {
      res.subscribe((projects) => {
        this.projects.set(projects || []);
      });
    });
  }
}
