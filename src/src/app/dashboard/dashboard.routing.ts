import { ModuleWithProviders } from '@angular/core';
import { RouterModule }        from '@angular/router';

import { RootComponent }    from './root/root.component';
import { HomeComponent }    from './home/home.component'; 
import { SettingsComponent }    from './settings/settings.component'; 

import { AuthGuard } from '../auth.guard';
import { NewrecepieComponent } from './newrecepie/newrecepie.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
      path: 'dashboard',
      component: RootComponent, canActivate: [AuthGuard],

      children: [      
       { path: '', component: HomeComponent },
       { path: 'home',  component: HomeComponent },
        { path: 'settings', component: SettingsComponent },
        { path: 'newrecepie', component: NewrecepieComponent },

      ]       
    }  
]);

