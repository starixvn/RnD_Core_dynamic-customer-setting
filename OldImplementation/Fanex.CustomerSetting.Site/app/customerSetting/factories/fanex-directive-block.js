(function () {
    'use strict';
    var injectParams = ['$compile', '$templateCache', '$filter', 'fanexAppSettings'];
    var fanexDirectiveBlock = function ($compile, $templateCache, $filter, appSettings) {
        return function (templateUrl, extendScope) {
            return {
                restrict: 'AE',
                scope: {
                    model: '='
                },
                link: function ($scope, $element) {
                    $scope.$watch('model', function () {
                        if (angular.isUndefined($scope.model)) {
                            return;
                        }

                        var template = $templateCache.get(templateUrl);
                        var groupIndex = 0;
                        var collection = $scope.model.Collection;
                        var settingLevel = $scope.model.SettingLevel;
                        var formType = $scope.model.FormType;

                        function appendGroup(items) {
                            var $template = angular.element(template);
                            $element.append($template);
                            var link = $compile($template);
                            var childScope = $scope.$new();
                            childScope.items = items;
                            childScope.settingLevel = settingLevel;
                            childScope.formType = formType;
                            childScope.groupIndex = groupIndex;
                            if (groupIndex === 0 && collection.length >= 2) {
                                childScope.hasCopy = true;
                            }
                            if (angular.isFunction(extendScope)) {
                                extendScope(childScope);
                            }
                            link(childScope);
                            groupIndex++;
                        }

                        if (settingLevel === appSettings.settingLevel.SportTypeBetType) {
                            var groupCollection = $filter('fanexDataGroup')(collection);
                            angular.forEach(groupCollection, function (dataGroup) {
                                var sportTypeName = dataGroup.SportTypeName;
                                var groupedTemplate = '<div class="group-name" title="' + sportTypeName + '">' + sportTypeName + '</div>';
                                $element.append(groupedTemplate);
                                appendGroup(dataGroup.Items);
                            });
                        } else {
                            appendGroup(collection);
                        }
                    });
                }
            };
        };
    };

    fanexDirectiveBlock.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveBlock', fanexDirectiveBlock);
})();