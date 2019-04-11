angular.module('enrichmentFormSample')
    .controller('enrichmentFormSample.widgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        var blade = $scope.blade;
        var propertyNames = ['City', 'Country', 'Position', 'State', 'StreetAddress', 'Zip'];
        $scope.enabled = false;

        $scope.$watch('blade.currentEntity', function (product) {
            if (product) {
                $scope.enabled = _.every(propertyNames,
                    function(name) {
                        return _.any(product.properties,
                            function(property) {
                                return property.name === name;
                            });
                    });
            }
        });

        $scope.openAddressEditorBlade = function () {
            if (!$scope.enabled) {
                return;
            }
            var newBlade = {
                id: "address Editor Sample",
                currentEntity: blade.currentEntity,
                controller: 'enrichmentFormSample.editAddressController',
                template: 'Modules/$(EnrichmentFormSample)/Scripts/blades/edit-address.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);
