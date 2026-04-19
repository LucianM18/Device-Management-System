import { Routes } from '@angular/router';
import { DeviceListComponent } from './pages/device-list/device-list.component';
import { DeviceDetailComponent } from './pages/device-detail/device-detail.component';
import { DeviceFormComponent } from './pages/device-form/device-form.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'devices', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'devices', component: DeviceListComponent, canActivate: [authGuard] },
  { path: 'devices/new', component: DeviceFormComponent, canActivate: [authGuard] },
  { path: 'devices/:id', component: DeviceDetailComponent, canActivate: [authGuard] },
  { path: 'devices/:id/edit', component: DeviceFormComponent, canActivate: [authGuard] },
];
