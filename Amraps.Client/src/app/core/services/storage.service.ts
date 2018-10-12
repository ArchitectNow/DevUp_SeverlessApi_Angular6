import {Injectable} from '@angular/core';

@Injectable()
export class StorageService {
  storage: any;
  useSession = true;
  isEnabled = false;

  constructor() {
    if (this.useSession && !window.sessionStorage) {
      this.isEnabled = false;
      console.error('Current browser does not support Session Storage');
      return;
    }
    if (!this.useSession && !window.localStorage) {
      this.isEnabled = false;
      console.error('Current browser does not support Local Storage');
      return;
    }

    this.isEnabled = true;
    this.storage = this.useSession ? window.sessionStorage : window.localStorage;
  }

  public set(key: string, value: string): void {
    if (this.isEnabled) {
      this.storage[key] = value;
    }
  }

  public get(key: string): string {
    if (!this.isEnabled) {
      return '';
    }

    return this.storage[key] || false;
  }

  public setObject(key: string, value: any): void {
    if (!this.isEnabled) {
      return;
    }
    this.storage[key] = JSON.stringify(value);
  }

  public getObject(key: string): any {
    if (!this.isEnabled) {
      return null;
    }
    return JSON.parse(this.storage[key] || '{}');
  }

  public getValue<TType>(key: string): TType {
    if (!this.isEnabled) {
      return null;
    }
    const obj = JSON.parse(this.storage[key] || null);
    return <TType>obj || null;
  }

  public remove(key: string): any {
    if (!this.isEnabled) {
      return null;
    }
    this.storage.removeItem(key);
  }

  public clear() {
    this.storage.clear();
  }
}

export const LOCAL_STORAGE_PROVIDERS: any[] = [
  StorageService
];
