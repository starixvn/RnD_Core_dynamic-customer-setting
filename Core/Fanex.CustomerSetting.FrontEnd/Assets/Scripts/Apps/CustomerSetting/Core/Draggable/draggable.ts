/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface IDraggableScope extends ng.IScope {
        draggableBannerId: string;
    }

    interface IMouseEventHandler {
        (eventObject: JQueryMouseEventObject, ...args: any[]): any;
    }

    class Draggable implements ng.IDirective {
        public static $inject: string[] = ["$document"];

        public restrict: string = "A";

        public get scope(): any {
            return {
                draggableBannerId: "=draggable"
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: IDraggableScope, element: ng.IAugmentedJQuery) => {
            var draggableBanner: ng.IAugmentedJQuery = angular.element(document.getElementById(scope.draggableBannerId));
            var container = angular.element(element.parent())[0];
            var width: number;
            var height: number;
            var startX: number;
            var startY: number;
            var currentX: number;
            var currentY: number;

            draggableBanner.css({
                cursor: "move"
            });

            draggableBanner.on("mousedown", (event: JQueryMouseEventObject) => {
                startX = event.clientX - element[0].offsetLeft;
                startY = event.clientY - element[0].offsetTop;
                width = element[0].offsetWidth;
                height = element[0].offsetHeight;

                this.$document.on("mousemove", mouseMove);
                this.$document.on("mouseup", mouseUp);
            });

            var mouseMove: IMouseEventHandler = (event: JQueryMouseEventObject) => {
                currentX = event.clientX - startX;
                currentY = event.clientY - startY;
                updatePosition();
            }

            var mouseUp: IMouseEventHandler = (event: JQueryMouseEventObject) => {
                this.$document.off("mousemove", mouseMove);
                this.$document.off("mouseup", mouseUp);
            }

            function updatePosition(): void {
                if (container) {
                    var containerClientRect: ClientRect = container.getBoundingClientRect();

                    if (currentX < containerClientRect.left) {
                        currentX = containerClientRect.left;
                    } else if (currentX > containerClientRect.right - width) {
                        currentX = containerClientRect.right - width;
                    }

                    if (currentY < containerClientRect.top) {
                        currentY = containerClientRect.top;
                    } else if (currentY > containerClientRect.bottom - height) {
                        currentY = containerClientRect.bottom - height;
                    }
                }

                element.css({
                    "margin-top": currentY + "px",
                    "margin-left": currentX + "px"
                });
            }
        };

        private $document: ng.IDocumentService;

        constructor($document: ng.IDocumentService) {
            this.$document = $document;
        }
    }

    ModuleRegister.directive("draggable", Draggable);
}