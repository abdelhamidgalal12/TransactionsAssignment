import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../services/api.service';
import { EncryptionUtil } from '../../utils/encryption.util';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transaction',
  standalone: true,
  imports: [MatButtonModule, MatInputModule, MatFormFieldModule,MatProgressSpinner, MatCardModule,MatFormField,MatLabel,MatInputModule, CommonModule,FormsModule],
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css'],
})
export class TransactionComponent {
  transaction = {
    processingCode: '',
    systemTraceNr: '',
    functionCode: '',
    cardNo: '',
    cardHolder: '',
    amountTrxn: 0,
    currencyCode: 0,
  };
  response: { approvalCode?: string; message?: string } = {};
  isLoading = false; 
  errorMessage = ''; 
  formErrors: any = {}; 

  constructor(private apiService: ApiService, private snackBar: MatSnackBar,private router: Router) {}

  validateForm(): boolean {
    this.formErrors = {};
    let isValid = true;

    if (!this.transaction.processingCode) {
      this.formErrors['processingCode'] = 'Processing Code is required';
      isValid = false;
    }
    if (!this.transaction.systemTraceNr) {
      this.formErrors['systemTraceNr'] = 'System Trace Number is required';
      isValid = false;
    }
    if (!this.transaction.functionCode) {
      this.formErrors['functionCode'] = 'function Code is required';
      isValid = false;
    }
    if (!this.transaction.cardNo || this.transaction.cardNo.length < 13) {
      this.formErrors['cardNo'] = 'Card Number is required and must be at least 13 digits';
      isValid = false;
    }
    if (!this.transaction.cardHolder) {
      this.formErrors['cardHolder'] = 'Card Holder is required';
      isValid = false;
    }
    if (!this.transaction.amountTrxn || this.transaction.amountTrxn <= 0) {
      this.formErrors['amountTrxn'] = 'Amount should be a positive number';
      isValid = false;
    }
    if (!this.transaction.currencyCode || this.transaction.currencyCode < 1) {
      this.formErrors['currencyCode'] = 'Currency Code is required';
      isValid = false;
    }

    return isValid;
  }
  showNotification(message: string, isError: boolean) {
    const action = isError ? 'Retry' : 'Close';
    this.snackBar.open(message, action, {
      duration: 5000,
      panelClass: isError ? ['error-snackbar'] : ['success-snackbar'],
    });
  }

  onSubmit() {

    this.errorMessage = '';

    if (!this.validateForm()) {
      return; 
    }

    const token = localStorage.getItem('token');
    if (!token) {
      this.showNotification('Unauthorized access. Please log in.', true);
      return;
    }

    this.isLoading = true;
    this.apiService.generateEncryptionKey().subscribe({
      next: (keyResponse) => {
        const encryptionKey = keyResponse.key;
        if (!encryptionKey) {
          this.showNotification('Failed to retrieve a valid encryption key.', true);
          this.isLoading = false;
          return;
        }
        try {
          const encryptedData = EncryptionUtil.encrypt(this.transaction, encryptionKey);
          console.log(encryptedData);

          this.apiService.processTransaction(encryptedData, encryptionKey, token).subscribe({
            next: (response) => {
              console.log('API Response:', response);

              if (response?.result?.approvalCode && response?.result?.message) {
                this.response = {
                  approvalCode: response.result.approvalCode,
                  message: response.result.message
                };
                this.showNotification('Transaction processed successfully.', false);
                this.isLoading = false;

              } else {
                this.showNotification('Invalid response received from the backend.', true);
              this.isLoading = false;

              }
            },
            error: (err) => {
              if (err.status === 401) 
                {
                  localStorage.removeItem('token');
                  this.router.navigate(['/']); 
              }
              else{
              console.error('Transaction API Error:', err);
              this.showNotification('Transaction failed. Please try again.', true);
              this.isLoading = false;
              }
            },
            complete: () => {
              this.isLoading = false;
            }
          });
        }
         catch (error) {
          console.error('Encryption Error:', error);
          this.showNotification('Encryption failed. Please try again.', true);
          this.isLoading = false;
        }
      },
      error: (err) => {
        console.error('Encryption Key Generation Error:', err);
        this.showNotification('Failed to generate encryption key. Please try again.', true);
        this.isLoading = false;
      }
    });
  }
}

