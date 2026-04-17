import { Routes } from '@angular/router';
import { DeviceListComponent } from './pages/device-list/device-list.component';
import { DeviceDetailComponent } from './pages/device-detail/device-detail.component';

export const routes: Routes = [
  { path: '', redirectTo: 'devices', pathMatch: 'full' },
  { path: 'devices', component: DeviceListComponent },
  { path: 'devices/:id', component: DeviceDetailComponent },
];
