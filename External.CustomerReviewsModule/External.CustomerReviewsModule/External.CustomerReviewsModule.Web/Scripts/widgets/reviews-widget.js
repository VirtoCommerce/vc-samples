angular.module('external.customerReviewsModule')
    .controller('external.customerReviewsModule.reviewsWidgetController', ['$scope', 'external.customerReviewsModule.webApi', 'platformWebApp.bladeNavigationService',
        function ($scope, api, bladeNavigationService) {
            var blade = $scope.blade;
            var filter = { take: 0 };

            function refresh() {
                $scope.loading = true;
                api.search(filter, function (data) {
                    $scope.loading = false;
                    $scope.totalCount = data.totalCount;
                });
            }

            $scope.openBlade = function () {
                if ($scope.loading || !$scope.totalCount)
                    return;

                var newBlade = {
                    id: "reviewsList",
                    filter: filter,
                    title: 'Customer reviews for "' + blade.title + '"',
                    controller: 'external.customerReviewsModule.reviewsController',
                    template: 'Modules/$(external.customerReviewsModule)/Scripts/blades/reviews.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.$watch("blade.itemId", function (id) {
                filter.productIds = [id];

                if (id)
                    refresh();
            });
    }]);
