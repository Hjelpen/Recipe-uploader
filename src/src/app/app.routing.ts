import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RecpieComponent } from './recpie/recpie.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'recpie', component: RecpieComponent },
  { path: 'recpie/:id', component: RecpieComponent }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
