import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products.component';
import { OrdersComponent } from './orders.component';
import { AppComponent } from './app.component';

const routes: Routes = [
    { path: 'products', component: ProductsComponent },
    { path: 'orders', component: OrdersComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }