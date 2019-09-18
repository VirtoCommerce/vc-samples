// Call this to register your module to main application
var moduleName = "customerReviews.export";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.metaFormsService', 'platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService',
        function (metaFormsService, toolbarService, bladeNavigationService) {
            // registering "Export" tool in CustomerReviews list blade:
            // Only currently selected items are filtered for export
            // --or--
            // advanced filter can be applied later
            toolbarService.register({
                name: "platform.commands.export",
                icon: 'fa fa-upload',
                canExecuteMethod: function () { return true; },
                executeMethod: function (blade) {
                    var selectedRows = blade.$scope.gridApi.selection.getSelectedRows();
                    var exportDataRequest = {
                        exportTypeName: 'CustomerReviews.Export.Data.ExportImport.ExportableCustomerReview',
                        dataQuery: {
                            exportTypeName: 'CustomerReviewExportDataQuery',
                            isAllSelected: !_.any(selectedRows),
                            objectIds: _.pluck(selectedRows, "id")
                        }
                    };

                    angular.extend(exportDataRequest.dataQuery, blade.getSearchCriteria());

                    var newBlade = {
                        id: 'customerReviewExport',
                        title: 'customerReviews.blades.exporter.reviewsTitle',
                        subtitle: 'customerReviews.blades.exporter.reviewsSubtitle',
                        controller: 'virtoCommerce.exportModule.exportSettingsController',
                        template: 'Modules/$(VirtoCommerce.Export)/Scripts/blades/export-settings.tpl.html',
                        exportDataRequest: exportDataRequest,
                        totalItemsCount: exportDataRequest.dataQuery.objectIds.length || blade.$scope.pageSettings.totalItems

                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                index: 1
            }, 'CustomerReviews.Web.reviewsListController');

            // Additional fields for advanced filtering in "Select data to export" blade
            metaFormsService.registerMetaFields('CustomerReviews.Export.Data.ExportImport.ExportableCustomerReview' + 'ExportFilter', [
                {
                    name: 'productSelector',
                    title: "pricing.selectors.titles.products",
                    templateUrl: 'Modules/$(VirtoCommerce.Pricing)/Scripts/selectors/product-selector.tpl.html'
                },
                {
                    name: 'isActive',
                    title: "Active reviews only",
                    valueType: "Boolean"
                }]);

        }
    ]);
