import through = require("through2");

class AnnotationPattern {
    constructor(
        public pattern: RegExp,
        public type: string,
        public firstLowerCase?: boolean,
        public removePattern?: boolean) {
        this.firstLowerCase = this.firstLowerCase || true;
        this.removePattern = this.removePattern || true;
    }
}

class AngularAnnotationOption {
    public defaultPatterns: AnnotationPattern[];
    public firstLowerCase: boolean;
    public moduleName: string;
}

class AngularAnnotation {
    constructor(private opt: AngularAnnotationOption) {
    }

    private static _defaultOptions: AngularAnnotationOption;

    private static get defaultOptions(): AngularAnnotationOption {
        if (AngularAnnotation._defaultOptions === undefined) {
            AngularAnnotation._defaultOptions = new AngularAnnotationOption();
            AngularAnnotation._defaultOptions.defaultPatterns = new Array<AnnotationPattern>(
                new AnnotationPattern(/Controller$/, "controller"),
                new AnnotationPattern(/Ctrl$/, "controller"),
                new AnnotationPattern(/Service$/, "service"),
                new AnnotationPattern(/Manager$/, "service"),
                new AnnotationPattern(/ServiceDelegate$/, "service"),
                new AnnotationPattern(/Provider$/, "provider"),
                new AnnotationPattern(/Directive$/, "directive"),
                new AnnotationPattern(/Filter$/, "filter"));

            AngularAnnotation._defaultOptions.firstLowerCase = true;
            AngularAnnotation._defaultOptions.moduleName = "angularModule";
        }

        return AngularAnnotation._defaultOptions;
    }

    public options: AngularAnnotationOption;

    private transform(content: string, opt: AngularAnnotationOption): string {
    }

    private addAngularModule(moduleName: string, pattern: AnnotationPattern): void {
        
    }
}

export = AngularAnnotation;