angular.module('external.customerReviewsModule')
    .factory('external.customerReviewsModule.webApi', ['$resource', function ($resource) {
        return $resource('api/external.customerReviewsModule', {}, {
            search: { method: 'POST', url: 'api/customerReviews/search' },
            update: { method: 'PUT' }
        });
}]);
