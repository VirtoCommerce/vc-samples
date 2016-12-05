angular.module('virtoCommerce.samples.unmanaged', [])
.controller('virtoCommerce.samples.unmanaged.bladeController', ['$scope', function ($scope) {
    var blade = $scope.blade;

    $scope.data = "UnmanagedModule content";
    blade.title = "UnmanagedModule title";
    blade.isLoading = false;
}]);
