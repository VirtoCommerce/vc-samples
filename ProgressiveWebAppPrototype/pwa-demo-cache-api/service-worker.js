const filesToCache = [
    'image1.png',
    'image2.png',
    'index.html',
    'service-worker.js',
    'main.css',
    'main.js',
    'some-other-page.html',
];

const staticCacheName = 'PWA demo cache';

self.addEventListener('install', event => {
    console.log('Устанавливаем PWA (заполняем БД)...');
    event.waitUntil(
        caches.open(staticCacheName)
            .then(cache => {
                return cache.addAll(filesToCache);
            })
    );
});

self.addEventListener('activate', event => {
    console.log('Активируем Service Worker...');

    const cacheWhitelist = [staticCacheName];

    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheWhitelist.indexOf(cacheName) === -1) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

self.addEventListener('fetch', event => {
    console.log('На Service Worker пришел запрос: ', event.request.url);
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    console.log('URL ', event.request.url, ' найден в Offline кэше');
                    return response;
                }

                console.log('Запрос к серверу по Internet: ', event.request.url);

                return fetch(event.request)
                    .then(response => {
                        if (response.status === 404) {
                            console.log("Страница не найдена.");
                        }
                        return caches.open(staticCacheName)
                            .then(cache => {
                                cache.put(event.request.url, response.clone());
                                return response;
                            });
                    }).catch(error => {
                        console.log('Запрос выполнен с ошибкой: ', error);
                    });
            }).catch(error => {
            console.log('Запрос выполнен с ошибкой: ', error);
        })
    );
});