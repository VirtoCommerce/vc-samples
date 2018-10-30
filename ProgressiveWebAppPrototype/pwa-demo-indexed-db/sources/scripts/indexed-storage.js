export class IndexedStorage {
    /***
     * Класс для работы с IndexedDB
     */
    constructor() {
        this._db = null;
    }

    /***
     * Открывает базу данных
     * @param name Имя
     * @param version Версия
     */
    open(name, version) {
        return new Promise(function (resolve, reject) {
            let request = window.indexedDB.open(name, version);

            request.onupgradeneeded = (event) => {
                this._db = event.target.result;

                if (this._db.objectStoreNames.contains('Cart')) {
                    this._db.deleteObjectStore('Cart');
                }

                this._db.createObjectStore('Cart', { keyPath: "id" });
            };

            request.onsuccess = (event) => {
                this._db = event.target.result;
                resolve(this)
            };
            request.onerror = (event) => {
                reject(event.target.error)
            };
        }.bind(this))
    }

    /***
     * Добавляет новую запись в текущую базу данных
     * @param storeName Имя хранилища
     * @param item Объект
     * @returns {Promise}
     */
    addItem(storeName, item) {
        return new Promise(function(resolve, reject) {
            let transaction = this._db.transaction(storeName, 'readwrite');
            let addRequest = transaction.objectStore(storeName).add(item);

            addRequest.onsuccess = (event) => {
                resolve(event.target.result);
            };

            addRequest.onerror = (event) => {
                reject(event.target.error);
            };
        }.bind(this));
    }

    /***
     * Обновляет существующюю запись в базе данных
     * @param storeName Имя хранилища
     * @param item Объект
     * @returns {Promise}
     */
    updateItem(storeName, item) {
        return new Promise(function(resolve, reject) {
            let transaction = this._db.transaction(storeName, 'readwrite');
            let addRequest = transaction.objectStore(storeName).put(item);

            addRequest.onsuccess = (event) => {
                resolve(event.target.result);
            };

            addRequest.onerror = (event) => {
                reject(event.target.error);
            };
        }.bind(this));
    }

    /***
     * Возвращает запись из базы данных
     * @param storeName Имя хранилища
     * @param itemId Идентификатор записи
     * @returns {Promise}
     */
    getItem(storeName, itemId) {
        return new Promise(function(resolve, reject) {
            let transaction = this._db.transaction(storeName, 'readwrite');
            let getRequest = transaction.objectStore(storeName).get(itemId);

            getRequest.onsuccess = (event) => {
                resolve(event.target.result);
            };
            getRequest.onerror = (event) => {
                reject(event.target.error);
            };
        }.bind(this));
    }

    /***
     * Возвращает все записи из хранилища
     * @param storeName Имя хранилища
     * @returns {Promise}
     */
    getAll(storeName) {
        return new Promise(function(resolve, reject) {
            let transaction = this._db.transaction(storeName, 'readwrite');
            let getRequest = transaction.objectStore(storeName).getAll();

            getRequest.onsuccess = (event) => {
                resolve(event.target.result);
            };
            getRequest.onerror = (event) => {
                reject(event.target.error);
            };
        }.bind(this));
    }

    /***
     * Удаляет запись из базы данных
     * @param storeName Имя хранилища
     * @param itemId Идентификатор записи
     * @returns {Promise}
     */
    deleteItem(storeName, itemId) {
        return new Promise(function(resolve, reject) {
            let transaction = this._db.transaction(storeName, 'readwrite');
            let getRequest = transaction.objectStore(storeName).delete(itemId);

            getRequest.onsuccess = (event) => {
                resolve(event.target.result);
            };
            getRequest.onerror = (event) => {
                reject(event.target.error);
            };
        }.bind(this));
    }
}