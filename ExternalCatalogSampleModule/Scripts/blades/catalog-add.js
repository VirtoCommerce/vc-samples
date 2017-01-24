angular.module('extCatalogModule')
.controller('extCatalogModule.extCatalogAddController', ['$scope', 'platformWebApp.bladeNavigationService', 'extCatalogModule.extcatalogs', 'virtoCommerce.catalogModule.catalogs', function ($scope, bladeNavigationService, extcatalogs, catalogs) {
    var blade = $scope.blade;
    blade.apiUrl = "http://demo.virtocommerce.com/admin";
    blade.isLoading = false;
    $scope.discover = function (apiUrl) {
        blade.isLoading = true;
        extcatalogs.discover({ apiUrl: apiUrl }, function (data) {
            blade.extCatalogs = _.filter(data, function(x) { return !x.isVirtual });
            blade.isLoading = false;
        });
    };

    $scope.saveChanges = function () {
        blade.extCatalog.name = blade.apiUrl.replace('http://', '') + '/' + blade.extCatalog.name;
        blade.extCatalog.properties = [{
            name: 'apiUrl',
            valueType : 'ShortText',
            values: [ { value : blade.apiUrl, valueType: 'ShortText' }]
        }];
        catalogs.save({}, blade.extCatalog, function (data) {
            $scope.bladeClose();
            blade.parentBlade.refresh();
        });
       
    };
   
    $scope.discover(blade.apiUrl)
}]);
