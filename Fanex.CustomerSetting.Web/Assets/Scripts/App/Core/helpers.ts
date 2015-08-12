module CustomerSetting {
    "use strict";

    export class StringFormater {
        public static format(originalString: string, ...replacements: any[]): string {
            return originalString.replace(/{(\d+)}/g, (match: string, index: number) => {
                return replacements[index] === (undefined || null) ? match : replacements[index];
            });
        }
    }

    export class ArrayHelpers {
        public static join2DimensionalArray<T>(source: Array<Array<T> | T>): Array<T> {
            var result: Array<T> = new Array<T>();

            if (source && source.length > 0) {
                source.forEach((item: Array<T> | T) => {
                    if (item instanceof Array) {
                        result.push.apply(result, item);
                    } else {
                        result.push.apply(result, [item]);
                    }
                });
            }

            return result;
        }
    }
}