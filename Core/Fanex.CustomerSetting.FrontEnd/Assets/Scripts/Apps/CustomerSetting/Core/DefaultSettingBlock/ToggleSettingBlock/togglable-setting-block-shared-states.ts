module CustomerSetting {
    "use strict";

    export class TogglableSettingBlockSharedStates {
        public toggleStates: boolean[];
        public numberofExpandedBlocks: number;

        constructor(numberofBlocks: number, expandFirstBlock: boolean = true) {
            expandFirstBlock = expandFirstBlock || false;
            if (numberofBlocks > 0) {
                this.toggleStates = [];
                this.numberofExpandedBlocks = expandFirstBlock ? 1 : 0;

                for (var index = 0; index < numberofBlocks; index++) {
                    this.toggleStates[index] = (expandFirstBlock && index === 0) ? true : false;
                }
            }
        }

        public isExpanded(blockIndex: number): boolean {
            return this.toggleStates[blockIndex];
        }

        public isExpandedAll(): boolean {
            return this.numberofExpandedBlocks === this.toggleStates.length;
        }

        public collapseBlock(blockIndex: number): void {
            if (this.toggleStates[blockIndex]) {
                this.toggleStates[blockIndex] = false;
                this.numberofExpandedBlocks--;
            }
        }

        public expandBlock(blockIndex: number): void {
            if (!this.toggleStates[blockIndex]) {
                this.toggleStates[blockIndex] = true;
                this.numberofExpandedBlocks++;
            }
        }

        public collapseAllBlocks(): void {
            for (var index = 0; index < this.toggleStates.length; index++) {
                this.toggleStates[index] = false;
            }

            this.numberofExpandedBlocks = 0;
        }

        public expandAllBlocks(): void {
            for (var index = 0; index < this.toggleStates.length; index++) {
                this.toggleStates[index] = true;
            }

            this.numberofExpandedBlocks = this.toggleStates.length;
        }
    }
}