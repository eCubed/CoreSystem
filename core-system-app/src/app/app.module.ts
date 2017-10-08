import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AuthGuard } from './guards/auth.guard';

import { ArraysService } from './services/arrays.service';
import { AuthService } from './services/auth.service';
import { ApiService } from './services/api.service';
import { LocalStorageService } from './services/local-storage.service';
import { ResponsiveService } from './services/responsive.service';

import { AppComponent } from './app.component';
import { HomeComponent } from './routes/home/home.component';
import { AboutComponent } from './routes/about/about.component';
import { NotFoundComponent } from './routes/not-found/not-found.component';
import { LoginComponent } from './routes/login/login.component';
import { DashboardComponent } from './routes/dashboard/dashboard.component';
import { UnauthorizedComponent } from './routes/unauthorized/unauthorized.component';

const appRoutes: Routes = [
	{
		path: '',
		component: HomeComponent
	},  
  {
    path: 'about',
    component: AboutComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
	{
		path: '**',
		component: NotFoundComponent
	}
];

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    NotFoundComponent,
    LoginComponent,
    DashboardComponent,
    UnauthorizedComponent
  ],
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    ArraysService, 
    ApiService,
    AuthGuard,
    AuthService,
    LocalStorageService,
    ResponsiveService],
  bootstrap: [AppComponent]
})
export class AppModule { }
