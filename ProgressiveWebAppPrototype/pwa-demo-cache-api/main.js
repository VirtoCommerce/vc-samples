'use strict';

(function() {
    const SERVICE_WORKER_API_NODE = 'serviceWorker';
    let isAlreadyRegistered = false;

    // Если Service Worker API доступно в браузере
    if (SERVICE_WORKER_API_NODE in navigator) {

        navigator.serviceWorker.getRegistrations().then(function(registrations) {
            registrations.forEach(function(worker) {
                console.log(worker);

                if (worker.scope === 'http://localhost:8080/') {
                    isAlreadyRegistered = true;
                }
            });
        });

        // Регистрируем новый Service Worker

        window.addEventListener('load', () => {
            if (!isAlreadyRegistered) {
                navigator.serviceWorker.register('service-worker.js')
                    .then(swReg => {
                        console.log('Service Worker успешно зарегистрировался', swReg);
                    })
                    .catch(err => {
                        console.error('Не получилось зарегистрировать Service Worker', err);
                    });
            }
        });
    }
})();