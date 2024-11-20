import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './guards/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { TransactionComponent } from './payment/transaction/transaction.component';

export const routes: Routes = [

    { path: '', component: LoginComponent },
    { path: 'transaction', component: TransactionComponent, canActivate: [AuthGuard] }
];
