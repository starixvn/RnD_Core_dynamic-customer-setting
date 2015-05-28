(function () {
    'use strict';
    var injectParams = ['$compile', '$templateCache', '$filter', 'fanexAppSettings'];
    var fanexDirectiveBlock = function ($compile, $templateCache, $filter, appSettings) {
        return function (templateUrl, extendScope) {
            return {
                restrict: 'AE',
                scope: {
                    collection: '=',
                    settingLevel: '='
                },
                link: function ($scope, $element) {
                    var hasChangeCollection = false;
                    var hasChangeSettingType = false;

                    $scope.$watch('collection', function (newCollection, oldCollection) {
                        if (newCollection !== oldCollection) {
                            hasChangeCollection = true;
                            checkToUpdate();
                        }
                    });

                    $scope.$watch('settingLevel', function (newSettingLevel, oldSettingLevel) {
                        if (newSettingLevel !== oldSettingLevel) {
                            hasChangeSettingType = true;
                            checkToUpdate();
                        }
                    });

                    function checkToUpdate() {
                        if (hasChangeCollection && hasChangeSettingType) {
                            update();
                        }
                    }

                    function update() {
                        if ($scope.settingLevel === appSettings.settingLevel.SportTypeBetType) {
                            var groupCollection = $filter('fanexDataGroup')($scope.collection);
                            angular.forEach(groupCollection, function (dataGroup) {
                                var sportTypeName = dataGroup.SportTypeName;
                                var groupedTemplate = '<div class="group-name" title="' + sportTypeName + '">' + sportTypeName + '</div>';
                                $element.append(groupedTemplate);
                                appendGroup(dataGroup.Items);
                            });
                        } else {
                            appendGroup($scope.collection);
                        }
                    }

                    var template = $templateCache.get(templateUrl);
                    var groupIndex = 0;
                    function appendGroup(items) {
                        var $template = angular.element(template);
                        $element.append($template);
                        var link = $compile($template);
                        var childScope = $scope.$new();
                        childScope.items = items;
                        childScope.settingLevel = $scope.settingLevel;
                        childScope.groupIndex = groupIndex;
                        if (groupIndex === 0 && $scope.collection.length >= 2) {
                            childScope.hasCopy = true;
                        }
                        if (angular.isFunction(extendScope)) {
                            extendScope(childScope);
                        }
                        link(childScope);
                        groupIndex++;
                    }
                }
            };
        };
    };

    fanexDirectiveBlock.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveBlock', fanexDirectiveBlock);
})();