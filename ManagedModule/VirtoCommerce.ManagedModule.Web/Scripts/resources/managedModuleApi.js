angular.module('platformWebApp.managedModule')
.factory('managedModuleApi', ['$resource', function ($resource) {
    return $resource('api/managedModule');
}]);
