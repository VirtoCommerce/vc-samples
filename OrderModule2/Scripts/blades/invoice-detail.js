angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.invoiceDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings',
    function ($scope, bladeNavigationService, dialogService, settings) {
        var blade = $scope.blade;

        if (blade.isNew) {
            blade.title = 'New invoice';

            var foundField = _.findWhere(blade.metaFields, { name: 'createdDate' });
            if (foundField) {
                foundField.isReadonly = false;
            }
        } else {
            blade.title = 'invoice details';
            blade.subtitle = 'sample';
        }

        blade.currentStore = _.findWhere(blade.parentBlade.stores, { id: blade.customerOrder.storeId });
        blade.realOperationsCollection = blade.customerOrder.invoices;

        blade.statuses = settings.getValues({ id: 'Invoice.Status' });
        blade.openStatusSettingManagement = function () {
            var newBlade = new DictionarySettingDetailBlade('Invoice.Status');
            newBlade.parentRefresh = function (data) {
                blade.statuses = data;
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);