//Call this to register our module to main application
var moduleName = "virtoCommerce.samples.unmanaged";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName );
}

angular.module(moduleName, [
    'unmanagedModule.blades.blade1'
])
.config(
	['$stateProvider',
		function ($stateProvider) {
			$stateProvider
				.state('workspace.unmanagedModuleTemplate', {
					url: '/unmanagedModule',
					templateUrl: 'Scripts/common/templates/home.tpl.html',
					controller: [
						'$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
							var blade = {
								id: 'blade1',
								// controller name must be unique in Application. Use prefix like 'um-'.
								controller: 'virtoCommerce.samples.unmanaged.bladeController',
								template: 'Modules/$(Sample.Unmanaged)/Scripts/blades/blade1.tpl.html',
								isClosingDisabled: true
							};
							bladeNavigationService.showBlade(blade);
						}
					]
				});
		}
	]
)
.run(
	['$rootScope', 'platformWebApp.mainMenuService', '$state', function ($rootScope, mainMenuService, $state) {
		//Register module in main menu
		var menuItem = {
			path: 'browse/unmanaged module',
			icon: 'fa fa-cube',
			title: 'Unmanaged Module',
			priority: 110,
			action: function () { $state.go('workspace.unmanagedModuleTemplate') },
			permission: 'UnmanagedModulePermission'
		};
		mainMenuService.addMenuItem(menuItem);
	}]);
