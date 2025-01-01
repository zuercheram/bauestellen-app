import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  MatProgressBarModule,
  ProgressBarMode,
} from '@angular/material/progress-bar';
import { ProgressBarService } from './progress-bar.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-progress-bar',
  imports: [MatProgressBarModule],
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.scss',
})
export class ProgressBarComponent implements OnInit, OnDestroy {
  progressbarMode: ProgressBarMode = 'indeterminate';

  showProgressBar = false;

  private serviceSubscription!: Subscription;

  constructor(private readonly progressBarService: ProgressBarService) {}

  ngOnInit(): void {
    this.serviceSubscription =
      this.progressBarService.showProgressBar$.subscribe((mode) => {
        this.showProgressBar = mode;
      });
  }

  ngOnDestroy(): void {
    this.serviceSubscription.unsubscribe();
  }
}
