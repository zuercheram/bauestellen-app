import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Project } from '../models/project.interface';

@Component({
  selector: 'app-project-list-item',
  imports: [CommonModule],
  templateUrl: './project-list-item.component.html',
  styleUrls: ['./project-list-item.component.scss'],
})
export class ProjectListItemComponent {
  @Input() project: Project | undefined = undefined;
}
