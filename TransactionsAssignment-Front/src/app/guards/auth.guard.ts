import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('token');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1])); // Decode token payload
      const expiry = payload.exp; // Extract expiration time
      if (expiry && expiry > Date.now() / 1000) {
        return true;
      }
    }
  
    // Redirect to login if not authenticated or token expired
    this.router.navigate(['/']);
    return false;
  }
  
}
