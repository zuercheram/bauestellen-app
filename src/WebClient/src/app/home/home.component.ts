import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../core/services/auth.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-home',
  imports: [CommonModule, MatButtonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  constructor(public readonly authService: AuthService) {}
}
