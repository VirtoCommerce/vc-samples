//Call this to register our module to main application
var moduleTemplateName = "$safeprojectname$";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [])
.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('workspace.$safeprojectname$', {
                url: '/$safeprojectname$',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                        var newBlade = {
                            id: 'blade1',
                            controller: '$safeprojectname$.blade1Controller',
                            template: 'Modules/$($safeprojectname$)/Scripts/blades/blade1.tpl.html',
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
            path: 'browse/$safeprojectname$',
            icon: 'fa fa-cube',
            title: '$safeprojectname$',
            priority: 100,
            action: function () { $state.go('workspace.$safeprojectname$') },
            permission: '$safeprojectname$Permission'
        };
        mainMenuService.addMenuItem(menuItem);
    }
]);

