import {IndexedStorage} from "./indexed-storage";
import {CartItem} from "./cart-item";

export class App {
    /***
     * Приложение
     * @param addToCartButtons Кнопки добавления товара в корзину
     * @param cartItemsContainer Контейнер для товаров в корзине
     * @param clearCartButton Кпопка очистки корзины
     */
    constructor(addToCartButtons, cartItemsContainer, clearCartButton) {
        this.ui = {
            addToCartButtons: addToCartButtons,
            cartItemsContainer: cartItemsContainer,
            clearCartButton: clearCartButton
        };
    }

    /***
     * Запускает приложение
     */
    run() {
        let app = this;

        // Регистрируем Service Worker
        navigator.serviceWorker.register('service-worker.js')
             .catch(err => {
                console.error('Не получилось зарегистрировать Service Worker', err);
             });

        // Создаем хранилище данных
        let storage = new IndexedStorage();

        // Открываем базу данных или создаем новую, если ее нет
        storage.open('PWADemoDb', 1)
            .then(function(storage) {
                 // Получаем все записи из хранилища
                return storage.getAll('Cart')
            })
            .then(function(items) {
                // Выставляем видимость кнопки очистки корзины
                if (items.length > 0) {
                    app.ui.clearCartButton.style.display = 'Block';
                }
                else {
                    app.ui.clearCartButton.style.display = 'None';
                }

                // Рендерим все обнаруженные товары в корзине
                items.forEach(function(item) {
                    let cartItem = CartItem.parse(item);

                    for (let i = 0; i < cartItem.count; i++) {
                        cartItem.render(app.ui.cartItemsContainer);
                    }
                });
            })
            .catch(function(error) {
                console.error(error);
            });

        // Добавляем продукт в корзину при клике на кнопку "Добавить в корзину"
        app.ui.addToCartButtons.forEach(function(button) {
            button.addEventListener('click', function() {
                let cartItem = new CartItem(button.dataset.id, button.dataset.name, button.dataset.price, 1);
                app.addItemToCart(storage, cartItem);
            });
        });

        // Очищаем корзину при клике на кнопку "Очистить корзину"
        app.ui.clearCartButton.addEventListener('click', function() {
            app.clearCart(storage);
        });

        // Выводим сообщение о возможности отправки информации о товарах в корзине на сервер
        // при переходе из Offline в Online
        window.addEventListener('online', function() {
            if (navigator.onLine) {
                storage.getAll('Cart')
                    .then(function(items) {
                        let itemsString = '';

                        items.forEach(function(item) {
                            itemsString += item.name + ' (x' + item.count + ')' + ', ';
                        });

                        alert('Network is available now and we can send data to server: ' + itemsString);
                    });
            }
        });
    }

    /***
     * Добавляет товар в корзину
     * @param storage Хранилище данных
     * @param cartItem Товар
     */
    addItemToCart(storage, cartItem) {
        let app = this;

        storage.getItem('Cart', cartItem.id)
            .then(function(item) {
                if (item) {
                    item.count++;
                    storage.updateItem('Cart', item)
                        .then(function() {
                            cartItem.count = item.count;
                            cartItem.render(app.ui.cartItemsContainer);
                            app.ui.clearCartButton.style.display = 'Block';
                        }, function(error) {
                            // Транзацкия завершилась с ошибкой
                            console.error(error);
                        });
                }
                else {
                    storage.addItem('Cart', cartItem)
                        .then(function() {
                            cartItem.render(app.ui.cartItemsContainer);
                            app.ui.clearCartButton.style.display = 'Block';
                        }, function(error) {
                            // Транзацкия завершилась с ошибкой
                            console.error(error);
                        });
                }
            }, function(error) {
                // Транзацкия завершилась с ошибкой
                console.error(error);
            })
    }

    /***
     * Очищает корзину, удаляя все товары
     * @param storage Хранилище данных
     */
    clearCart(storage) {
        let app = this;

        app.ui.cartItemsContainer.childNodes.forEach(function(cartItem) {
            storage.deleteItem('Cart', cartItem.dataset.id)
                .then(function() {
                    cartItem.parentNode.removeChild(cartItem);
                    app.ui.clearCartButton.style.display = 'None';
                }, function(error) {
                    // Транзацкия завершилась с ошибкой
                    console.error(error);
                });
        });
    }
}