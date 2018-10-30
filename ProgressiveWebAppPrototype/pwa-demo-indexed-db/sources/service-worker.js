/***
 * Файлы которые будут закешированы и использоваться для работы в offline-режиме
 * @type {string[]}
 */
const filesToCache = [
    '/',
    'images/favicon.ico',
    'images/macbook.png',
    'images/macbook-air.png',
    'images/macbook-pro.png',
    'images/macbook-pro-touchbar.png',
    'bundle.js',
    'service-worker.js',
    'styles/vendor/bootstrap.min.css',
    'styles/main.css',
    'index.html'
];

/***
 * Название кэша в Cache Storage браузера
 * @type {string}
 */
const staticCacheName = 'PWA demo cache';

/***
 *  Добавляем слушатель установки Service Worker
 *  Он вызовается тогда, когда Service Worker будет загружен впервые
 *  или будет загружена впервые более новая версия Service Worker с сервера,
 *  при этом изменение кода Service Worker хотя бы на 1 байт, вызовет повторно это событие
 */
self.addEventListener('install', event => {
    event.waitUntil(
        // Открываем кэш или создаем, если его нет
        caches.open(staticCacheName)
            .then(cache => {
                // Добавляем файлы в кэш
                return cache.addAll(filesToCache);
            })
    );
});

/***
 * Добавляем слушатель активации Service Worker
 * Он вызовается тогда, когда Service Worker будет загружен впервые
 * или будет загружена впервые более новая версия Service Worker с сервера,
 * а также не будет открытых вкладок, использующих старую версию Service Worker
 * До тех пор пока будет использоваться старая версия Service Worker, он будет находится
 * в состоянии "waiting"
 */
self.addEventListener('activate', event => {
    event.waitUntil(
        // Получаем массив имен кэшей
        caches.keys()
            .then(cacheNames => {
                return Promise.all(
                    cacheNames.map(cacheName => {
                        if ([staticCacheName].indexOf(cacheName) === -1) {
                            // Удаляем файлы кэша, не найденные в массиве кэширования (устаревшие)
                            // Данная операция обновляет
                            return caches.delete(cacheName);
                        }
                    })
                )
            })
    );
});

/***
 * Добавляем слушатель исходящих запросов
 * Он вызовется при любом исходящем запросе со страницы
 */
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    // URL найден в кэше, берем файл из кэша
                    return response;
                }

                // URL не найден в кэше, скачиваем файл с сервера
                return fetch(event.request)
                    .then(response => {
                        if (response.status === 404) {
                            console.error("URL не найден");
                        }

                        return caches.open(staticCacheName)
                            .then(cache => {
                                // Кешируем полученный файл
                                cache.put(event.request.url, response.clone());
                                return response;
                            });
                    }).catch(error => {
                        console.log('Ошибка: ', error);
                    });
            }).catch(error => {
            console.log('Ошишбка: ', error);
        })
    );
});

