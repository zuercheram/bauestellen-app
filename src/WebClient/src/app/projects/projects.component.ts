import { Component, OnInit } from '@angular/core';
import { ProjectsService } from './services/projects.service';
import { Observable } from 'rxjs';
import { Project } from './models/project.interface';
import { CommonModule } from '@angular/common';
import { ProjectListItemComponent } from './project-list-item/project-list-item.component';

@Component({
  selector: 'app-projects',
  imports: [CommonModule, ProjectListItemComponent],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss',
})
export class ProjectsComponent {
  projects$: Observable<Project[]>;

  constructor(private readonly projectsService: ProjectsService) {
    this.projects$ = projectsService.getProjects();
  }
}
