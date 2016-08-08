module CustomerSetting {
    "use strict";

    export class TogglableSettingBlockSharedStates {
        public toggleStates: boolean[];
        public numberofCollapsedBlocks: number;

        constructor(numberofBlocks: number) {
            if (numberofBlocks > 0) {
                this.toggleStates = [];
                this.numberofCollapsedBlocks = numberofBlocks - 1;

                for (var index = 0; index < numberofBlocks; index++) {
                    this.toggleStates[index] = index === 0 ? true : false;
                }
            }
        }

        public isExpanded(blockIndex: number): boolean {
            return this.toggleStates[blockIndex];
        }

        public isExpandedAll(): boolean {
            return this.numberofCollapsedBlocks === this.toggleStates.length;
        }

        public collapseBlock(blockIndex: number): void {
            this.toggleStates[blockIndex] = false;
            this.numberofCollapsedBlocks--;
        }

        public expandBlock(blockIndex: number): void {
            this.toggleStates[blockIndex] = true;
            this.numberofCollapsedBlocks++;
        }

        public collapseAllBlocks(): void {
            for (var index = 0; index < this.toggleStates.length; index++) {
                this.toggleStates[index] = false;
            }

            this.numberofCollapsedBlocks = 0;
        }

        public expandAllBlocks(): void {
            for (var index = 0; index < this.toggleStates.length; index++) {
                this.toggleStates[index] = true;
            }

            this.numberofCollapsedBlocks = this.toggleStates.length;
        }
    }
}