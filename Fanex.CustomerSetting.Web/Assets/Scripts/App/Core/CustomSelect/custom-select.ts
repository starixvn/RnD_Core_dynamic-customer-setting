/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../common.ts" />
/// <reference path="../helper-services.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    export interface RangedValueSelectScope extends ng.IScope {
        value: number;
        min: number;
        max: number;
        step: number;
        divisor: number;
        range: KeyValuePair<number, number>[];
    }

    export class RangedValueSelect {
        public replace: boolean = true;
        public restrict: string = "A";

        public get scope(): any {
            return {
                value: "=",
                min: "=",
                max: "=",
                step: "=",
                divisor: "="
            };
        }

        public controller: any = ["$scope", "valueRangeGenerator", ($scope: RangedValueSelectScope, valueRangeGenerator: ValueRangeGenerator) => {
            $scope.$watchGroup(["min", "max"], () => {
                var newRange: KeyValuePair<number, number>[] = valueRangeGenerator.generateRanges(
                    $scope.value,
                    $scope.min,
                    $scope.max,
                    $scope.step,
                    $scope.divisor);

                $scope.range = newRange;
            });
        }];
    }

    ModuleRegister.directive("rangedValueSelect", RangedValueSelect);

    class ValueRangeGenerator {
        public generateRanges(value: number, min: number, max: number, step: number, divider: number): Array<KeyValuePair<number, number>> {
            var ranges: Array<KeyValuePair<number, number>> = new Array<KeyValuePair<number, number>>();

            min = Math.max(0, min);
            max = Math.max(0, max);

            if (value > max) {
                ranges.push(new KeyValuePair<number, number>(value, value));
            }

            while (max >= min) {
                ranges.push(new KeyValuePair<number, number>(max, max));
                max -= step;
                max = Math.round(max * divider) / divider;
            }

            if (value < min) {
                ranges.push(new KeyValuePair<number, number>(value, value));
            }

            return ranges;
        }
    }

    ModuleRegister.service("valueRangeGenerator", ValueRangeGenerator);
}