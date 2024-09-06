export interface IProducts {
    id: number,
    name: string,
    description: string,
    price: number,
    categoryId: number,
    categoryName: string,
    isAvailable: boolean
}

export interface ICart {
    productId: number,
    productName: string,
    quantity: number,
    itemCost: number
}