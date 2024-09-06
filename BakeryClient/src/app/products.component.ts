import { Component, OnInit } from '@angular/core';
import { ApiDataService, IApiResultObject } from './services/api-data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IProducts, ICart } from './dto/product-models';
import { ISnackBarModel } from './uicomponents/snackbar.component'

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.css'],
    providers: [ApiDataService]
})
export class ProductsComponent implements OnInit {

    clientForm: FormGroup;

    private init: IApiResultObject = null;
    products: IProducts[] = [];
    cart: ICart[] = [];
    snackBarModel: ISnackBarModel = {} as ISnackBarModel;

    totalCost: number = 0;
    showClientForm: boolean = false;

    constructor(private apiDataService: ApiDataService, private fb: FormBuilder) {
        this.init = {} as IApiResultObject;
        this.init.data = [];
    }

    ngOnInit(): void {
        this.GetProduct();
        this.InitClientForm();
    }

    InitClientForm(): void {
        this.clientForm = this.fb.group({
            name: ['', [Validators.required, Validators.minLength(2)]],
            phone: ['', [Validators.required, Validators.minLength(10)]],
            deliveryAddress: ['', Validators.required]
        });
    }

    onSubmit() {
        if (this.clientForm.valid) {
            const formData = this.clientForm.value;
            this.CreateCustomer(formData);
        } else {
            console.log('Form is invalid');
        }
    }

    GetProduct(): void {
        this.apiDataService.Get("https://localhost:7215/api/Product")
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.products = this.init.data;
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }

    AddToCart(product: IProducts): void {
        if (product) {
            let thisProductInCart = this.cart.find(item => item.productId === product.id);

            if (thisProductInCart) {
                thisProductInCart.quantity += 1;
            } else {
                this.cart.push({
                    itemCost: product.price,
                    productId: product.id,
                    productName: product.name,
                    quantity: 1
                });
            }
            this.GetTotalCost();
        }
    }

    ChangeCartItemQuality(cartItem: ICart, isPlus: boolean) {
        if (isPlus)
            cartItem.quantity += 1;
        else {
            cartItem.quantity -= 1;
            if (cartItem.quantity < 1) {
                this.cart = this.cart.filter(item => item.productId !== cartItem.productId);
            }
        }
        if (this.cart.length == 0)
            this.showClientForm = false;
        this.GetTotalCost();
    }

    DeleteCartItem(productId: number) {
        this.cart = this.cart.filter(item => item.productId !== productId);
        if (this.cart.length == 0)
            this.showClientForm = false;
        this.GetTotalCost();
    }

    GetTotalCost(): void {
        this.totalCost = 0;
        this.cart.forEach(item => {
            this.totalCost += (item.itemCost * item.quantity);
        });
    }

    CreateCustomer(formData: any): void {
        this.apiDataService.Post("https://localhost:7215/api/Order/create-customer", formData)
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.CreateOrder(this.init.data);
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }

    CreateOrder(customerId: number): void {

        let orderItems = this.cart.map(m => ({
            productId: m.productId,
            productName: m.productName,
            quantity: m.quantity,
            itemCost: m.itemCost
        }));

        let data = {
            customerId: customerId,
            orderItems: orderItems
        };

        this.apiDataService.Post("https://localhost:7215/api/Order/create-order", data)
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.cart = [];
                    this.showClientForm = false;
                    this.clientForm.reset();
                    this.snackBarModel = {
                        isError: false,
                        message: "Замовлення створене"
                    }
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }
}