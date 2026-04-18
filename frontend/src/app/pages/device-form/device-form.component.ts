import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DeviceService } from '../../services/device.service';
import { DeviceUpsertPayload } from '../../models/device.model';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './device-form.component.html',
  styleUrl: './device-form.component.css',
})
export class DeviceFormComponent implements OnInit {
  readonly form;

  mode: 'create' | 'edit' = 'create';
  deviceId: number | null = null;
  loading = false;
  submitting = false;
  error: string | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly deviceService: DeviceService
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      manufacturer: ['', [Validators.required, Validators.maxLength(100)]],
      type: ['', [Validators.required, Validators.maxLength(20)]],
      operatingSystem: ['', [Validators.required, Validators.maxLength(50)]],
      osVersion: ['', [Validators.required, Validators.maxLength(50)]],
      processor: ['', [Validators.required, Validators.maxLength(100)]],
      ramAmount: [8, [Validators.required, Validators.min(1), Validators.max(128)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
    });
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.mode = 'edit';
      this.deviceId = Number(idParam);
      this.loadDevice(this.deviceId);
    }
  }

  get title(): string {
    return this.mode === 'create' ? 'Add Device' : 'Update Device';
  }

  get submitText(): string {
    return this.mode === 'create' ? 'Create Device' : 'Save Changes';
  }

  onSubmit(): void {
    this.error = null;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.buildPayload();
    this.submitting = true;

    if (this.mode === 'create') {
      this.deviceService.create(payload).subscribe({
        next: (created) => {
          this.submitting = false;
          this.router.navigate(['/devices', created.id]);
        },
        error: () => {
          this.submitting = false;
          this.error = 'Failed to create device.';
        },
      });
      return;
    }

    if (this.deviceId === null) {
      this.submitting = false;
      this.error = 'Invalid device id.';
      return;
    }

    this.deviceService.update(this.deviceId, payload).subscribe({
      next: (updated) => {
        this.submitting = false;
        this.router.navigate(['/devices', updated.id]);
      },
      error: () => {
        this.submitting = false;
        this.error = 'Failed to update device.';
      },
    });
  }

  cancel(): void {
    if (this.mode === 'edit' && this.deviceId !== null) {
      this.router.navigate(['/devices', this.deviceId]);
      return;
    }

    this.router.navigate(['/devices']);
  }

  private loadDevice(id: number): void {
    this.loading = true;
    this.deviceService.getById(id).subscribe({
      next: (device) => {
        this.form.patchValue({
          name: device.name,
          manufacturer: device.manufacturer,
          type: device.type,
          operatingSystem: device.operatingSystem,
          osVersion: device.osVersion,
          processor: device.processor,
          ramAmount: device.ramAmount,
          description: device.description ?? '',
        });
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.error = 'Failed to load device for editing.';
      },
    });
  }

  private buildPayload(): DeviceUpsertPayload {
    const value = this.form.getRawValue();

    return {
      name: (value.name ?? '').trim(),
      manufacturer: (value.manufacturer ?? '').trim(),
      type: (value.type ?? '').trim(),
      operatingSystem: (value.operatingSystem ?? '').trim(),
      osVersion: (value.osVersion ?? '').trim(),
      processor: (value.processor ?? '').trim(),
      ramAmount: Number(value.ramAmount),
      description: (value.description ?? '').trim(),
    };
  }
}
