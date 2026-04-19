import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { NgIf } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { AuthService } from '../../services/auth.service';
import { DeviceDetail } from '../../models/device.model';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatProgressSpinnerModule, MatDividerModule, NgIf],
  templateUrl: './device-detail.component.html',
  styleUrl: './device-detail.component.css',
})
export class DeviceDetailComponent implements OnInit {
  device: DeviceDetail | null = null;
  deviceId: number | null = null;
  loading = true;
  error: string | null = null;
  assignError: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.deviceId = id;
    this.loadDevice(id);
  }

  private loadDevice(id: number): void {
    this.deviceService.getById(id).subscribe({
      next: (data) => {
        this.device = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Device not found.';
        this.loading = false;
      },
    });
  }

  get isAssignedToCurrentUser(): boolean {
    return (
      this.device?.currentUserId != null &&
      this.device.currentUserId === this.authService.currentUser?.id
    );
  }

  get isUnassigned(): boolean {
    return this.device?.currentUserId == null;
  }

  assign(): void {
    if (this.deviceId === null) return;
    this.assignError = null;
    this.deviceService.assignDevice(this.deviceId).subscribe({
      next: () => this.loadDevice(this.deviceId!),
      error: (err) => {
        this.assignError = err.error?.message ?? 'Could not assign device.';
      },
    });
  }

  unassign(): void {
    if (this.deviceId === null) return;
    this.assignError = null;
    this.deviceService.unassignDevice(this.deviceId).subscribe({
      next: () => this.loadDevice(this.deviceId!),
      error: (err) => {
        this.assignError = err.error?.message ?? 'Could not unassign device.';
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/devices']);
  }

  navigateToUpdate(): void {
    if (this.deviceId !== null) {
      this.router.navigate(['/devices', this.deviceId, 'edit']);
    }
  }
}
