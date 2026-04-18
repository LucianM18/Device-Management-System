import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgFor, NgIf } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { DeviceListItem } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [MatListModule, MatCardModule, MatButtonModule, MatProgressSpinnerModule, NgFor, NgIf],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css',
})
export class DeviceListComponent implements OnInit {
  devices: DeviceListItem[] = [];
  deletingDeviceIds = new Set<number>();
  loading = true;
  error: string | null = null;

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load devices.';
        this.loading = false;
      },
    });
  }

  navigateToDetail(id: number): void {
    this.router.navigate(['/devices', id]);
  }

  navigateToCreate(): void {
    this.router.navigate(['/devices/new']);
  }

  deleteDevice(event: MouseEvent, device: DeviceListItem): void {
    event.stopPropagation();
    this.error = null;

    const confirmed = window.confirm(`Delete device "${device.name}"?`);
    if (!confirmed) {
      return;
    }

    this.deletingDeviceIds.add(device.id);

    this.deviceService.delete(device.id).subscribe({
      next: () => {
        this.devices = this.devices.filter((d) => d.id !== device.id);
        this.deletingDeviceIds.delete(device.id);
      },
      error: () => {
        this.error = 'Failed to delete device.';
        this.deletingDeviceIds.delete(device.id);
      },
    });
  }
}
