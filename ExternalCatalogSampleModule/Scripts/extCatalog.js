//Call this to register our module to main application
var extCatalogsModuleName = "extCatalogModule";

if (AppDependencies != undefined) {
    AppDependencies.push(extCatalogsModuleName);
}

angular.module(extCatalogsModuleName, [
])
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService',
	function (toolbarService, bladeNavigationService) {
	    toolbarService.register({
	        name: "Add external catalog", icon: 'fa fa-cloud',
	        executeMethod: function (blade) {
	            var newBlade = {
	                id: 'ext-catalog-add',
	                controller: 'extCatalogModule.extCatalogAddController',
	                template: 'Modules/$(External.CatalogModule)/Scripts/blades/catalog-add.tpl.html'
	            };
	            bladeNavigationService.showBlade(newBlade, blade);
	        },
	        canExecuteMethod: function () { return true; },
	        index: 2
	    }, 'virtoCommerce.catalogModule.catalogsListController');
	 
	}]);
