'use strict';

(function () {
    var injectParams = ['$compile', '$templateCache', '$filter', 'fanexAppSettings'];
    var fanexDirectiveContainer = function ($compile, $templateCache, $filter, appSettings) {
        return function (templateUrl, extendOptions) {
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
                                var groupedTemplate = '<div class="sport-name" title="' + sportTypeName + '">' + sportTypeName + '</div>';
                                $element.append(groupedTemplate);
                                appendGroup(dataGroup.Items);
                            });
                        } else {
                            appendGroup($scope.collection);
                        }
                    }

                    var template = $templateCache.get(templateUrl);
                    function appendGroup(items) {
                        var $template = angular.element(template);
                        $element.append($template);
                        var link = $compile($template);
                        var childScope = $scope.$new();
                        childScope.items = items;
                        childScope.settingLevel = $scope.settingLevel;
                        angular.extend(childScope, extendOptions);
                        link(childScope);
                    }
                }
            };
        };
    };

    fanexDirectiveContainer.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveContainer', fanexDirectiveContainer);
})();