import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { RecpieComponent } from './recpie/recpie.component';
import { UserComponent } from './user/user.component';
import { FollowerListComponent } from './follower-list/follower-list.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'recpie', component: RecpieComponent },
  { path: 'recpie/:id', component: RecpieComponent },
  { path: 'user/:username', component: UserComponent },
  { path: 'followlist', component: FollowerListComponent }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
