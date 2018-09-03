import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './authenticate-xhr.backend';
import { StarRatingModule } from 'angular-star-rating';

import { routing } from './app.routing';

/* App Root */
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';

/* Account Imports */
import { AccountModule }  from './account/account.module';
/* Dashboard Imports */
import { DashboardModule }  from './dashboard/dashboard.module';

import { ConfigService } from './shared/utils/config.service';
import { RecpieComponent } from './recpie/recpie.component';
import { RecpieService } from './recpie/recpie.service';
import { HomeService } from './home/home.service';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    RecpieComponent,    
  ],
  imports: [
    AccountModule,
    DashboardModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    StarRatingModule.forRoot()
  ],
  providers: [ConfigService, RecpieService, HomeService, { 
    provide: XHRBackend, 
    useClass: AuthenticateXHRBackend,
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }