export interface DeviceListItem {
  id: number;
  name: string;
  manufacturer: string;
  type: string;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  description?: string | null;
  currentUserName?: string | null;
  currentUserRole?: string | null;
}

export interface DeviceDetail {
  id: number;
  name: string;
  manufacturer: string;
  type: string;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  description?: string | null;
}
