import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PagesComponent} from './pages/pages.component';
import {ClientComponent} from './pages/client/client.component';
import {ReportsComponent} from './pages/reports/reports.component';
import {PersonnelComponent} from './pages/personnel/personnel.component';
import {LoginComponent} from './pages/personnel/login/login.component';
import { TeamsComponent } from './pages/teams/teams.component';
import { KafkaComponent } from './pages/kafka/kafka.component';

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
      },
      {
        path: 'reports',
        component: ReportsComponent
      },
      {
        path: 'teams',
        component: TeamsComponent
      },
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'kafka',
        component: KafkaComponent
      }
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
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
