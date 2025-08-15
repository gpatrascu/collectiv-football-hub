import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div>Hello {{value}}</div>
    <button (click)="loginWithFacebook()">Login with Facebook</button>
  `,
})
export class AppComponent {
  value = 'World';

  loginWithFacebook() {
    window.location.href = '/.auth/login/facebook';
  }
}