angular.module('extCatalogModule')
.factory('extCatalogModule.extcatalogs', ['$resource', function ($resource) {
    return $resource('api/catalogs/external', {},
    {
        discover: { method: 'GET', isArray: true }      
    });
}]);

