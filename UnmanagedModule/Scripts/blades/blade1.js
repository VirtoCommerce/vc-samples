angular.module('unmanagedModule.blades.blade1', [])
.controller('um-blade1Controller', ['$scope', function ($scope) {
    var blade = $scope.blade;

    $scope.data = "UnmanagedModule content";
    blade.title = "UnmanagedModule title";
    blade.isLoading = false;
}]);
