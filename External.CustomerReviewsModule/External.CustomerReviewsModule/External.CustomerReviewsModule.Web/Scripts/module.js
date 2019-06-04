//Call this to register our module to main application
var moduleName = "external.customerReviewsModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('workspace.externalCustomerReviewsModuleState', {
                    url: '/external.customerReviewsModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var newBlade = {
                                id: 'reviewsList',
                                controller: 'external.customerReviewsModule.reviewsController',
                                template: 'Modules/$(external.customerReviewsModule)/Scripts/blades/reviews.html',
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
            // Register module in main menu
            var menuItem = {
                path: 'browse/external.customerReviewsModule',
                icon: 'fa fa-cube',
                title: 'Customer Reviews',
                priority: 100,
                action: function () { $state.go('workspace.externalCustomerReviewsModuleState'); },
                permission: 'customerReview:read'
            };
            mainMenuService.addMenuItem(menuItem);

            // Register reviews widget inside product blade
            var itemReviewsWidget = {
                controller: 'external.customerReviewsModule.reviewsWidgetController',
                template: 'Modules/$(external.customerReviewsModule)/Scripts/widgets/reviews-widget.html'
            };
            widgetService.registerWidget(itemReviewsWidget, 'itemDetail');
        }
    ]);
