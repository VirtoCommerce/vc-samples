angular.module('productVideoModule')
    .controller('productVideoModule.videoAddController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'productVideoModule.webApi',
            'platformWebApp.metaFormsService',
            function ($scope, bladeNavigationService, api, metaFormsService) {
                var blade = $scope.blade;
                blade.title = 'Product Video';
                blade.updatePermission = 'catalog:update';

                blade.metaFields = metaFormsService.getMetaFields("videoDetail");

                blade.refresh = function (parentRefresh) {
                    initializeBlade(blade.currentEntity);
                    if (parentRefresh) {
                        blade.parentBlade.refresh();
                    }
                }

                function initializeBlade(data) {
                    if (!blade.isNew) {
                        blade.title = data.name;
                    }

                    blade.currentEntity = angular.copy(data);
                    blade.origEntity = data;
                    blade.isLoading = false;
                    blade.securityScopes = data.securityScopes;
                };

                function isDirty() {
                    return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
                }

                function canSave() {
                    return isDirty() && formScope && formScope.$valid;
                }

                var formScope;
                $scope.setForm = function (form) { formScope = form; }

                $scope.cancelChanges = function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                    $scope.bladeClose();
                };
                $scope.saveChanges = function () {
                    blade.isLoading = true;

                    if (blade.isNew) {
                        api.createOrUpdate({}, [blade.currentEntity], function (data) {
                            blade.isNew = false;
                            initializeBlade(blade.currentEntity);
                            initializeToolbar();
                            blade.refresh(true);
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                        });
                    }
                    else {
                        api.createOrUpdate({}, [blade.currentEntity], function () {
                            blade.refresh(true);
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                        });
                    }
                };

                blade.onClose = function (closeCallback) {
                    bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "catalog.dialogs.catalog-save.title", "catalog.dialogs.catalog-save.message");
                };

                function initializeToolbar() {
                    if (!blade.isNew) {
                        blade.toolbarCommands = [
                            {
                                name: "platform.commands.save", icon: 'fas fa-save',
                                executeMethod: function () {
                                    $scope.saveChanges();
                                },
                                canExecuteMethod: canSave,
                                permission: blade.updatePermission
                            },
                            {
                                name: "platform.commands.reset", icon: 'fa fa-undo',
                                executeMethod: function () {
                                    angular.copy(blade.origEntity, blade.currentEntity);
                                },
                                canExecuteMethod: isDirty,
                                permission: blade.updatePermission
                            }
                        ];
                    }
                }

                $scope.gridsterOpts = { columns: 3 };

                initializeToolbar();
                blade.refresh(false);
            }]);
