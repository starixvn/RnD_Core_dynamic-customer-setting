module CustomerSetting {
    export class StringFormater {
        public static format(originalString: string, ...replacements: any[]): string {
            return originalString.replace(/{(\d+)}/g, (match, number) => {
                return typeof replacements[number] !== 'undefined' ? replacements[number] : match;
            })
        }
    }
}