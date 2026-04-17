import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { NgIf } from '@angular/common';
import { DeviceService } from '../../services/device.service';
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
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
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

  goBack(): void {
    this.router.navigate(['/devices']);
  }
}
