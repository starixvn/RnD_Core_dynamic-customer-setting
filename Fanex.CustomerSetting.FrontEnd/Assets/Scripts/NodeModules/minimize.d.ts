/// <reference path="../typings/htmlparser2/htmlparser2.d.ts" />

declare module "minimize" {
    import htmlparser2 = require("htmlparser2");

    class Minimize {
        constructor(options?: IOptions);
        constructor(parser?: htmlparser2.Parser, options?: IOptions);

        parse(content: string, callback: Function): void;

        minifier(error: Error, dom: ITraverableDomElement): void;

        traverse(data: ITraverableDomElement[], html: string, done: Function): string;

        plug(plugins: NodeModule[]): void;

        use(id: string, plugin: NodeModule): void;
    }

    interface ITraverableDomElement {
        type: string;
        name: string;
        attribs: { name: string; value: string }[];
        children: ITraverableDomElement[];
    }

    export = Minimize;
}


interface IOptions {
    // Do not remove empty attributes
    empty?: boolean;

    // Do not strip CDATA from scripts
    cdata?: boolean;

    // Do not remove comments
    comments?: boolean;

    // Do not remove conditional internet explorer comments
    conditionals?: boolean;

    // Do not remove redundant attributes
    spare?: boolean;

    // Do not remove arbitrary quotes
    quotes?: boolean;

    // Preserve one whitespace
    loose?: boolean;
}
