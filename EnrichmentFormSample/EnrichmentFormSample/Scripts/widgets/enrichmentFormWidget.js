angular.module('EnrichmentFormSample')
    .controller('EnrichmentFormSample.WidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        var blade = $scope.blade;
        var propertyNames = ['City', 'Country', 'Position', 'State', 'StreetAddress', 'Zip', 'geolocation'];
        $scope.enabled = false;

        $scope.$watch('blade.item', function (product) {
            if (product) {
                let propertiesCount = 0;
                for (var i = 0; i < product.properties.length; i++) {
                    var property = product.properties[i];
                    if (propertyNames.indexOf(property.name) > -1) {
                        propertiesCount++;
                    }
                }
                $scope.enabled = propertiesCount === 7;
            }
        });

        $scope.openItemPropertyBlade = function () {
            if (!$scope.enabled) {
                return;
            }
            var newBlade = {
                id: "itemProperty",
                productId: blade.currentEntity.id,
                entityType: "product",
                currentEntity: blade.currentEntity,
                controller: 'EnrichmentFormSample.EditAddressController',
                template: 'Modules/$(EnrichmentFormSample)/Scripts/blades/edit-address.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);
