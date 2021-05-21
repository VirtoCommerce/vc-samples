// Call this to register your module to main application
var moduleName = "productVideoModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.productVideoModuleState', {
                    url: '/productVideo',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService', function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'productVideoModule.videoListController',
                                template: 'Modules/$(ProductVideo)/Scripts/blades/video-list.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])

    .run(['platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
        'platformWebApp.metaFormsService',
        function (mainMenuService, widgetService, $state, metaFormsService) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/productVideoModule',
                icon: 'fa fa-cube',
                title: 'ProductVideoModule',
                priority: 100,
                action: function () { $state.go('workspace.productVideoModuleState'); },
                permission: 'productVideoModule:access'
            };
            mainMenuService.addMenuItem(menuItem);

            metaFormsService.registerMetaFields('videoDetail', [
                {
                    name: 'productId',
                    title: 'Product Id',
                    placeholder: 'Enter product identifier',
                    colSpan: 2,
                    isRequired: true,
                    valueType: 'ShortText'
                },
                {
                    name: 'url',
                    title: 'Url',
                    placeholder: 'Enter target video url',
                    colSpan: 2,
                    isRequired: true,
                    valueType: 'ShortText'
                },
                //{
                //    name: 'id',
                //    title: 'Identifier',
                //    colSpan: 2,
                //    isRequired: true,
                //    valueType: 'ShortText'
                //},
            ]);
        }
    ]);
