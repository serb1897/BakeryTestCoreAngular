export interface IOrder {
    customerName: string,
    customerPhone: string,
    customerDeliveryAddress: string,
    statusName: string,
    id: number,
    orderDate: Date,
    totalCost: number,
    customerId: number,
    statusId: number,
    orderItems: IOrderItem[]
}

export interface IOrderItem {
    id: number,
    productId: number,
    productName: string,
    productCost?: number,
    quantity: number,
    itemCost: number,
    orderId: number
}

export interface ICategories {
    id: number,
    name: string,
}