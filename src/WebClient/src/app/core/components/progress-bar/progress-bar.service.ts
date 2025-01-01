import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProgressBarService {
  public showProgressBar$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);

  showProgressBar() {
    this.showProgressBar$.next(true);
  }

  hideProgressBar() {
    this.showProgressBar$.next(false);
  }
}
