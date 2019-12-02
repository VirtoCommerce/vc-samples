angular.module('CustomerReviews')
    .factory('CustomerReviews.api', ['$resource', function ($resource) {
        return $resource('api/customerReviews', {}, {
            search: { method: 'POST', url: 'api/customerReviews/search' },
            update: { method: 'PUT' }
        });
    }]);
