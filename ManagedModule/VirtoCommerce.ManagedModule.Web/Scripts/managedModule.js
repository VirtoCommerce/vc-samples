﻿//Call this to register our module to main application
var moduleTemplateName = "virtoCommerce.samples.managed";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [])
.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('workspace.managedModule', {
                url: '/managedModule',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                        var newBlade = {
                            id: 'blade1',
                            controller: 'virtoCommerce.samples.managed.blade1Controller',
                            template: 'Modules/$(virtoCommerce.samples.managed)/Scripts/blades/blade1.tpl.html',
                            isClosingDisabled: true
                        };
                        bladeNavigationService.showBlade(newBlade);
                    }
                ]
            });
    }
])
.run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
    function ($rootScope, mainMenuService, widgetService, $state) {
        //Register module in main menu
        var menuItem = {
            path: 'browse/managedModule',
            icon: 'fa fa-cube',
            title: 'Managed Module',
            priority: 100,
            action: function () { $state.go('workspace.managedModule') },
            permission: 'managedModulePermission'
        };
        mainMenuService.addMenuItem(menuItem);
    }
]);

