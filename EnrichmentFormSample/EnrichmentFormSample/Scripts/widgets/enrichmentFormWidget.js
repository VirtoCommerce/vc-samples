angular.module('enrichmentFormSample')
    .controller('enrichmentFormSample.widgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        var blade = $scope.blade;
        var propertyNames = ['City', 'Country', 'Position', 'State', 'StreetAddress', 'Zip'];
        $scope.enabled = false;

        $scope.$watch('blade.currentEntity', function (product) {
            if (product) {
                let propertiesCount = 0;
                _.each(product.properties,
                    function(property) {
                        if (propertyNames.indexOf(property.name) > -1) {
                            propertiesCount++;
                        }
                    });
                $scope.enabled = propertiesCount === propertyNames.length;
            }
        });

        $scope.openAddressEditorBlade = function () {
            if (!$scope.enabled) {
                return;
            }
            var newBlade = {
                id: "address Editor Sample",
                entityType: "product",
                currentEntity: blade.currentEntity,
                controller: 'enrichmentFormSample.editAddressController',
                template: 'Modules/$(EnrichmentFormSample)/Scripts/blades/edit-address.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);
