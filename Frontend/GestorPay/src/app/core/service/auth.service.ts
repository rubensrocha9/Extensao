import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Auth, ConfirmEmail } from '../../shared/models/Auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private api = environment.host;
  private userPayload: any;

  constructor(
    private router: Router,
    private http: HttpClient,
  ) {
    this.userPayload = this.decodedToken();
   }

   signUp(user: Auth): Observable<Auth>{
    return this.http.post<Auth>(`${this.api}/register`, user)
  }

  checkEmail(email: string): Observable<any> {
    return this.http.get(`${this.api}/check-email-availability/${email}`);
  }

  confirmEmail(confirmEmail: ConfirmEmail){
    return this.http.post<ConfirmEmail>(`${this.api}/confirme-email`, confirmEmail);
  }

  signIn(user: any){
    const url = `${this.api}/authenticate`;
    return this.http.post<any>(url, user);
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login']);
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }

  userAuthenticated(): boolean{
    return !!localStorage.getItem('token');
  }

  decodedToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token);
  }

  getFullNameFromToken(){
    if (this.userPayload){
      return this.userPayload.unique_name;
    }
  }

  getRoleFromToken(){
    if (this.userPayload){
      return this.userPayload.role;
    }
  }
}
