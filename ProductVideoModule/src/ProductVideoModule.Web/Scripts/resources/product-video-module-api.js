angular.module('productVideoModule')
    .factory('productVideoModule.webApi', ['$resource', function ($resource) {
        return $resource('api/productVideo/:id', { id: '@Id' }, {
            get: { method: 'GET' },
            getById: { method: 'GET', url: 'api/productVideo/', params: { id: '@id' } },
            search: { method: 'POST', url: '/api/productVideo/search' },
            createOrUpdate: { method: 'POST' },
            delete: { method: 'DELETE' }
        });
    }]);
