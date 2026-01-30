import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor() {
    this.createToastContainer();
  }
  private createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toast-container';
    container.className = 'toast toast-bottom toast-end';
    document.body.appendChild(container);
    return container;
  }
  private createToastElement(message: string, alertClass: string, duration = 5000) {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) return;

    const toast = document.createElement('div');
    toast.className = `alert ${alertClass} shadow-lg mb-4`;
    toast.innerHTML = `
      <span>${message}</span>
      <button class="btn btn-sm btn-ghost ml-4">✕</button>1
    `;

    toast.querySelector('button')?.addEventListener('click', () => {
      toastContainer.removeChild(toast);
    });

    toastContainer.appendChild(toast);

    setTimeout(() => {
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    }, duration);
  }
  success(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }
  error(message: string, duration?: number, status?: string) {
    this.createToastElement(message, 'alert-error', duration);
  }
  warning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }
  info(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
}
