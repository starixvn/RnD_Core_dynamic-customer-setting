module CustomerSetting {
    export class StringFormater {
        public static format(originalString: string, ...replacements: any[]): string {
            return originalString.replace(/{(\d+)}/g, (match, index) => {
                return replacements[index] === (undefined || null) ? match : replacements[index];
            })
        }
    }
}