angular.module('productVideoModule')
    .controller('productVideoModule.videoListController', [
        '$scope', 'productVideoModule.webApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService',
        'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', '$timeout', 'platformWebApp.ui-grid.extension', 'platformWebApp.dialogService',
        function ($scope, api, bladeNavigationService, authService, bladeUtils, uiGridHelper, $timeout, gridOptionExtension, dialogService) {
            $scope.uiGridConstants = uiGridHelper.uiGridConstants;
            var blade = $scope.blade;
            blade.title = 'Product Video';

            function getSearchCriteria() {
                return {
                    productIds: ['productIdvvv', 'productIdF', 'productId1', 'productId', 'EventedProductId4',
                        'EventedProductId3', 'EventedProductId2', 'EventedProductId1', 'EventedProductId'],
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                };
            }

            blade.refresh = function () {
                var searchCriteria = getSearchCriteria();
                api.search(searchCriteria, function (data) {
                    blade.isLoading = false;
                    $scope.pageSettings.totalItems = data.totalCount;
                    $scope.listEntries = data.results ? data.results : [];
                });
            };

            function isItemsChecked() {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            }

            function deleteList(list) {
                var dialog = {
                    id: "confirmDeleteItem",
                    videoCount: list.length,
                    callback: function (remove) {
                        if (remove) {
                            blade.isLoading = true;

                            api.delete({ ids: list.map(x => x.id) }, function (data) {
                                blade.refresh();
                            });
                        }
                    }
                }
                dialogService.showDialog(dialog, 'Modules/$(ProductVideo)/Scripts/dialogs/deleteProductVideos-dialog.tpl.html', 'platformWebApp.confirmDialogController');
            }
            blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh",
                    icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fas fa-trash-alt',
                    executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
                    canExecuteMethod: isItemsChecked,
                    permission: 'catalog:delete'
                },
            ];

            if (authService.checkPermission('productvideo:create')) {
                blade.toolbarCommands.splice(1,
                    0,
                    {
                        name: "platform.commands.add",
                        icon: 'fas fa-plus',
                        executeMethod: function (data) {
                            selectedNode = undefined;
                            $scope.selectedNodeId = undefined;

                            var newBlade = {
                                id: 'listItemChild',
                                isNew: true,
                                currentEntity: {},
                                controller: 'productVideoModule.videoAddController',
                                template: 'Modules/$(ProductVideo)/Scripts/blades/product-video-add.tpl.html'
                            };

                            bladeNavigationService.showBlade(newBlade, blade);
                        },
                        canExecuteMethod: function () {
                            return true;
                        }
                    });
            }

            blade.setSelectedNode = function (listItem) {
                $scope.selectedNodeId = listItem.type;
            };

            blade.openItemsBlade = function (item) {
                var newBlade = {
                    id: 'itemsList1',
                    currentEntity: item,
                    isNew: false,
                    securityScopes: item.securityScopes,
                    controller: 'productVideoModule.videoAddController',
                    template: 'Modules/$(ProductVideo)/Scripts/blades/product-video-add.tpl.html',
                };

                bladeNavigationService.showBlade(newBlade, blade);
            }

            $scope.selectNode = function (type) {
                blade.setSelectedNode(type);
                blade.selectedType = type;
                blade.openItemsBlade(type);
            };

            // ui-grid
            $scope.setGridOptions = function (gridId, gridOptions) {
                $scope.gridOptions = gridOptions;
                gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);

                gridOptions.onRegisterApi = function (gridApi) {
                    $scope.gridApi = gridApi;
                };

                bladeUtils.initializePagination($scope);
            };

            blade.headIcon = 'fa fa-film';
            //blade.refresh();
        }]);
