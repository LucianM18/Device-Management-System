import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  DeviceDetail,
  DeviceListItem,
  DeviceUpsertPayload,
} from '../models/device.model';

@Injectable({ providedIn: 'root' })
export class DeviceService {
  private readonly baseUrl = 'http://localhost:5199/api';

  constructor(private http: HttpClient) {}

  getAll(): Observable<DeviceListItem[]> {
    return this.http.get<DeviceListItem[]>(
      `${this.baseUrl}/devices/with-current-user`,
    );
  }

  getById(id: number): Observable<DeviceDetail> {
    return this.http.get<DeviceDetail>(`${this.baseUrl}/devices/${id}`);
  }

  create(payload: DeviceUpsertPayload): Observable<DeviceDetail> {
    return this.http.post<DeviceDetail>(`${this.baseUrl}/devices`, payload);
  }

  update(id: number, payload: DeviceUpsertPayload): Observable<DeviceDetail> {
    return this.http.put<DeviceDetail>(
      `${this.baseUrl}/devices/${id}`,
      payload,
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/devices/${id}`);
  }

  assignDevice(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/devices/${id}/assign`, {});
  }

  unassignDevice(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/devices/${id}/unassign`, {});
  }

  generateDescription(specs: {
    name: string;
    manufacturer: string;
    type: string;
    operatingSystem: string;
    osVersion: string;
    processor: string;
    ramAmount: number;
  }): Observable<{ description: string }> {
    return this.http.post<{ description: string }>(
      `${this.baseUrl}/devices/generate-description`,
      specs,
    );
  }
}
