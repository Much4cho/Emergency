import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PagesComponent} from './pages/pages.component';
import {ClientComponent} from './pages/client/client.component';
import {ReportsComponent} from './pages/reports/reports.component';
import {PersonnelComponent} from './pages/personnel/personnel.component';
import {LoginComponent} from './pages/personnel/login/login.component';
import {HandlingComponent} from './pages/personnel/handling/handling.component';

const routes: Routes = [
  {
    path: 'home',
    component: PagesComponent,
    children: [
      {
        path: 'client',
        component: ClientComponent
      },
      {
        path: 'personnel',
        component: PersonnelComponent,
        children: [
          {
            path: 'login',
            component: LoginComponent
          },
          {
            path: 'handling',
            component: HandlingComponent
          }
        ]
      },
      {
        path: 'reports',
        component: ReportsComponent
      },
    ]
  },
  {
    path: '**', redirectTo: 'home/client', pathMatch: 'full'
  },
  {
    path: '', redirectTo: 'home/client', pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
