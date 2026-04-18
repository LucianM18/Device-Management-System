import { Routes } from '@angular/router';
import { DeviceListComponent } from './pages/device-list/device-list.component';
import { DeviceDetailComponent } from './pages/device-detail/device-detail.component';
import { DeviceFormComponent } from './pages/device-form/device-form.component';

export const routes: Routes = [
  { path: '', redirectTo: 'devices', pathMatch: 'full' },
  { path: 'devices', component: DeviceListComponent },
  { path: 'devices/new', component: DeviceFormComponent },
  { path: 'devices/:id', component: DeviceDetailComponent },
  { path: 'devices/:id/edit', component: DeviceFormComponent },
];
