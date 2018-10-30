export class CartItem {
    /***
     * Продукт
     * @param id Идентификатор
     * @param name Название
     * @param price Цена
     * @param count Количество
     */
    constructor(id, name, price, count) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.count = count;
    }

    /***
     * Парсит и возвращает объект корзины
     * @param item Объект парсинга
     * @returns {CartItem} Объект корзины
     */
    static parse(item) {
        return new CartItem(item.id, item.name, item.price, item.count);
    }

    /***
     * Отрисовывает продукт
     * @param container HTML element, в котором будет произведена отрисовка
     */
    render(container) {
        container.innerHTML += '' +
            '<div class="cart__item" data-id="' + this.id + '" data-name="' + this.name + '" data-price="' + this.price + '" data-count="' + this.count + '">' +
                '<div class="cart__item-name">' + this.name + '</div>' +
                '<div class="cart__item-price">' + this.price + '</div>' +
            '</div>';
    }
}