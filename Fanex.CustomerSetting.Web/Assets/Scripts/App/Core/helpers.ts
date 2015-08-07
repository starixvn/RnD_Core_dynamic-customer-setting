module CustomerSetting {
    "use strict";

    export class StringFormater {
        public static format(originalString: string, ...replacements: any[]): string {
            return originalString.replace(/{(\d+)}/g, (match: string, index: number) => {
                return replacements[index] === (undefined || null) ? match : replacements[index];
            });
        }
    }
}