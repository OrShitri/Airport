import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LogsComponent } from './components/logs/logs.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { router } from './routing';

@NgModule({
  declarations: [
    AppComponent,
    LogsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    router
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
