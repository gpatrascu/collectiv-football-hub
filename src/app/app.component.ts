import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div>Hello {{value}}</div>
    <button (click)="loginWithFacebook()">Login with Facebook</button>
    <!-- Added: optional redirect input for post-login -->
    <div style="margin-top:8px">
      <input [(ngModel)]="redirectPath" placeholder="/path-or-empty-for-current" />
      <button (click)="loginWithGoogle()">Login with Google</button>
    </div>
  `,
})
export class AppComponent {
  value = 'World';

  // Include post_login_redirect_url so the app returns to the current page and preserves state
  loginWithFacebook() {
    const redirect = window.location.origin + window.location.pathname + window.location.search;
    window.location.href = `/.auth/login/facebook?post_login_redirect_url=${encodeURIComponent(redirect)}`;
  }

  // Updated: use redirectPath (if empty -> current location). Accepts absolute or path.
  redirectPath = window.location.pathname + window.location.search;

  loginWithGoogle() {
    let redirect = this.redirectPath || window.location.pathname + window.location.search;
    // If user provided a path (not a full URL), convert to absolute using origin
    if (!/^https?:\/\//i.test(redirect)) {
      // ensure it starts with '/'
      if (!redirect.startsWith('/')) {
        redirect = '/' + redirect;
      }
      redirect = window.location.origin + redirect;
    }
    window.location.href = `/.auth/login/google?post_login_redirect_url=${encodeURIComponent(redirect)}`;
  }
}