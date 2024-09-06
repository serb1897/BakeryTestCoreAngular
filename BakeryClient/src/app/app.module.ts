import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ProductsComponent } from './products.component';
import { OrdersComponent } from './orders.component';
import { SnackBarComponent } from './uicomponents/snackbar.component'

@NgModule({
    declarations: [
        AppComponent,
        ProductsComponent,
        OrdersComponent,
        SnackBarComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        ReactiveFormsModule,
        AppRoutingModule,
        NgxMaskDirective,
        NgxMaskPipe,
        FormsModule
    ],
    providers: [provideNgxMask()],
    bootstrap: [AppComponent]
})
export class AppModule { }