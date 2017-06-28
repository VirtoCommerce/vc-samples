angular.module('$safeprojectname$')
.factory('$safeprojectname$Api', ['$resource', function ($resource) {
    return $resource('api/$safeprojectname$');
}]);
