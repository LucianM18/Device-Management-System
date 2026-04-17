import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DeviceDetail, DeviceListItem } from '../models/device.model';

@Injectable({ providedIn: 'root' })
export class DeviceService {
  private readonly baseUrl = 'http://localhost:5199/api';

  constructor(private http: HttpClient) {}

  getAll(): Observable<DeviceListItem[]> {
    return this.http.get<DeviceListItem[]>(`${this.baseUrl}/devices/with-current-user`);
  }

  getById(id: number): Observable<DeviceDetail> {
    return this.http.get<DeviceDetail>(`${this.baseUrl}/devices/${id}`);
  }
}
