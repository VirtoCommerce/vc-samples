//Call this to register our module to main application
var moduleName = "external.priceModule.externalPrice";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.ui-grid.extension', function (gridOptionExtension) {
        gridOptionExtension.registerExtension('pricelist-grid', function (gridOptions) {
            gridOptions.columnDefs.push({
                name: 'basePrice',
                displayName: 'external-pricing.blades.prices-list.labels.base-price',
                editableCellTemplate: 'sale-cellTextEditor',
                validators: { saleValidator: true },
                cellTemplate: 'priceCellTitleValidator',
                enableCellEdit: true
            });
        });
    }]);
