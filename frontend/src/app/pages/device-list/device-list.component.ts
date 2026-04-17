import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgFor, NgIf } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { DeviceListItem } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [MatListModule, MatCardModule, MatProgressSpinnerModule, NgFor, NgIf],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css',
})
export class DeviceListComponent implements OnInit {
  devices: DeviceListItem[] = [];
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
}
