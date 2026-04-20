import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { DeviceService } from '../../services/device.service';
import { DeviceListItem } from '../../models/device.model';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    MatListModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatPaginatorModule,
    FormsModule,
    NgFor,
    NgIf,
  ],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css',
})
export class DeviceListComponent implements OnInit, OnDestroy {
  devices: DeviceListItem[] = [];
  deletingDeviceIds = new Set<number>();
  loading = true;
  error: string | null = null;

  searchQuery = '';
  totalCount = 0;
  pageIndex = 0;
  pageSize = 5;

  private readonly searchSubject = new Subject<string>();
  private readonly destroy$ = new Subject<void>();

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe((query) => {
        this.pageIndex = 0;
        this.loadDevices(query);
      });

    this.loadDevices('');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchChange(query: string): void {
    this.searchSubject.next(query);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    // Ignore event.pageSize, always use 5
    this.loadDevices(this.searchQuery);
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
        this.totalCount = Math.max(0, this.totalCount - 1);
        this.deletingDeviceIds.delete(device.id);
      },
      error: () => {
        this.error = 'Failed to delete device.';
        this.deletingDeviceIds.delete(device.id);
      },
    });
  }

  private loadDevices(query: string): void {
    this.loading = true;
    this.error = null;

    this.deviceService
      .search(query, this.pageIndex + 1, this.pageSize)
      .subscribe({
        next: (result) => {
          this.devices = result.items;
          this.totalCount = result.totalCount;
          this.loading = false;
        },
        error: () => {
          this.error = 'Failed to load devices.';
          this.loading = false;
        },
      });
  }
}
