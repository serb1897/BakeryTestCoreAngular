import { Component, OnInit } from '@angular/core';
import { ApiDataService, IApiResultObject } from './services/api-data.service';
import { IOrder, IOrderItem, ICategories } from './dto/order-models';
import { ISnackBarModel } from './uicomponents/snackbar.component'

@Component({
    selector: 'app-orders',
    templateUrl: './orders.component.html',
    styleUrls: ['./orders.component.css'],
    providers: [ApiDataService]
})
export class OrdersComponent implements OnInit {

    private init: IApiResultObject = null;
    orders: IOrder[] = [];
    orderForDetail: IOrder;
    snackBarModel: ISnackBarModel = {} as ISnackBarModel;

    totalCost: number = 0;
    showDetailOrder: boolean = false;

    constructor(private apiDataService: ApiDataService) {
        this.init = {} as IApiResultObject;
        this.init.data = [];
    }

    ngOnInit(): void {
        this.GetOrders();
    }

    GetOrders(): void {
        this.apiDataService.Get("https://localhost:7215/api/Order")
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.orders = this.init.data;
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }

    GetOrderForDetail(id: number): void {
        this.orderForDetail = this.orders.find(item => item.id == id);
        console.log(this.orderForDetail);
        if (this.orderForDetail) {

            this.orderForDetail.orderItems.forEach(item => {
                item.productCost = item.itemCost / item.quantity;
            });
            this.GetTotalCost();
            this.showDetailOrder = true;
        }
        else
            this.showDetailOrder = false;
    }

    GetTotalCost(): void {
        this.totalCost = 0;
        this.orderForDetail.orderItems.forEach(item => {
            this.totalCost += (item.productCost * item.quantity);
        });
    }

    ChangeCartItemQuality(cartItem: IOrderItem, isPlus: boolean): void {
        if (isPlus)
            cartItem.quantity += 1;
        else {
            cartItem.quantity -= 1;
            if (cartItem.quantity < 1) {
                this.orderForDetail.orderItems = this.orderForDetail.orderItems.filter(item => item.productId !== cartItem.productId);
            }
        }
        this.GetTotalCost();
    }

    DeleteCartItem(productId: number): void {
        this.orderForDetail.orderItems = this.orderForDetail.orderItems.filter(item => item.productId !== productId);
        this.GetTotalCost();
    }

    EditCartItems(orderId: number): void {
        let data = this.orderForDetail.orderItems.map(m => ({
            id: m.id,
            productId: m.productId,
            productName: m.productName,
            quantity: m.quantity,
            itemCost: m.itemCost,
            orderId: m.orderId
        }));

        let request = {
            orderId: orderId,
            orderItems: data
        };

        this.apiDataService.Put("https://localhost:7215/api/Order", request)
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.snackBarModel = {
                        isError: false,
                        message: "Замовлення змінено"
                    }
                    this.showDetailOrder = false;
                    this.GetOrders();
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }

    ChangeOrderStatus(orderId: number): void {
        let data = {
            orderId: orderId,
            statusId: this.orderForDetail.statusId
        };

        this.apiDataService.Patch("https://localhost:7215/api/Order", data)
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.snackBarModel = {
                        isError: false,
                        message: "Статус змінено"
                    }
                    this.showDetailOrder = false;
                    this.GetOrders();
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }

    DeleteOrder(orderId: number): void {
        this.apiDataService.Delete("https://localhost:7215/api/Order?orderId=" + orderId)
            .subscribe(d => {
                this.init = d as IApiResultObject;
                if (this.init.status) {
                    this.snackBarModel = {
                        isError: false,
                        message: "Замовлення видалено"
                    }
                    this.showDetailOrder = false;
                    this.GetOrders();
                }
                else
                    this.snackBarModel = {
                        isError: true,
                        message: this.init.message
                    }
            });
    }
}