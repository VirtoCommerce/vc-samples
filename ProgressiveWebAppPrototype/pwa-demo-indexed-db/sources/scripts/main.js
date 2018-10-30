import {App} from "./app";

/***
 * Точка входа
 */
(function() {
    // Узел доступа к Service Worker API
    const SERVICE_WORKER_API_NODE = 'serviceWorker';

    // Узел доступа к IndexedDB API
    const INDEXED_DB_API_NODE = 'indexedDB';

    // Кнопки добавления товара в корзину
    let addToCartButtons = document.querySelectorAll('.js-add-to-cart-button');

    // Контейнер для товаров в корзине
    let cartItemsContainer = document.getElementById('js-cart');

    // Кнопка очистки корзины
    let clearCartButton = document.getElementById('js-clear-cart-button');

    // Экземпляр приложения
    let app = new App(addToCartButtons, cartItemsContainer, clearCartButton);

    // Проверяем поддержку технологий браузерами
    if (!SERVICE_WORKER_API_NODE in navigator) {
        console.error('Текущий браузер не поддерживает технологию Cache API');
        return;
    }

    if (!INDEXED_DB_API_NODE in window) {
        console.error('Текущий браузер не поддерживает технологию IndexedDB');
        return;
    }

    // Запускаем приложение по завершению загрузки страницы
    window.addEventListener('load', () => app.run());
})();