import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserInfo } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = 'http://localhost:5199/api/auth';

  private currentUserSubject = new BehaviorSubject<UserInfo | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  get currentUser(): UserInfo | null {
    return this.currentUserSubject.value;
  }

  get isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }

  login(email: string, password: string): Observable<UserInfo> {
    return this.http
      .post<UserInfo>(`${this.baseUrl}/login`, { email, password })
      .pipe(tap((user) => this.currentUserSubject.next(user)));
  }

  register(email: string, password: string, name: string): Observable<UserInfo> {
    return this.http.post<UserInfo>(`${this.baseUrl}/register`, { email, password, name });
  }

  logout(): Observable<void> {
    return this.http
      .post<void>(`${this.baseUrl}/logout`, {})
      .pipe(tap(() => this.currentUserSubject.next(null)));
  }

  fetchCurrentUser(): Observable<UserInfo> {
    return this.http
      .get<UserInfo>(`${this.baseUrl}/me`)
      .pipe(tap((user) => this.currentUserSubject.next(user)));
  }
}
