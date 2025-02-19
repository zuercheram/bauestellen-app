import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProjectsComponent } from './projects/projects.component';
import { authGuard } from './core/guards/auth/auth.guard';
import { EditProjectComponent } from './projects/edit-project/edit-project.component';
import { PageNotFoundComponent } from './core/components/page-not-found/page-not-found.component';
import { AboutComponent } from './about/about.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'projects',
    canActivate: [authGuard],
    children: [
      { path: 'list', component: ProjectsComponent },
      { path: 'create', component: EditProjectComponent },
      { path: 'edit/:id', component: EditProjectComponent },
    ],
  },
  { path: 'about', component: AboutComponent },
  { path: '**', component: PageNotFoundComponent },
];
