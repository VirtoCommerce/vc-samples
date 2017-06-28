angular.module('$safeprojectname$')
.controller('$safeprojectname$.blade1Controller', ['$scope', '$safeprojectname$Api', function ($scope, api) {
    var blade = $scope.blade;
    blade.title = '$safeprojectname$';

    blade.refresh = function () {
        managedModuleApi.get(function (data) {
            blade.data = data.result;
            blade.isLoading = false;
        });
    }

    blade.refresh();
}]);
