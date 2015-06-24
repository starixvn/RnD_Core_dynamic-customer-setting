var CustomerLevel;
(function (CustomerLevel) {
    CustomerLevel[CustomerLevel["Member"] = 1] = "Member";
    CustomerLevel[CustomerLevel["Agent"] = 2] = "Agent";
    CustomerLevel[CustomerLevel["Master"] = 3] = "Master";
    CustomerLevel[CustomerLevel["Super"] = 4] = "Super";
    CustomerLevel[CustomerLevel["Company"] = 5] = "Company";
})(CustomerLevel || (CustomerLevel = {}));
var SettingLevel;
(function (SettingLevel) {
    SettingLevel[SettingLevel["Product"] = 1] = "Product";
    SettingLevel[SettingLevel["SportType"] = 2] = "SportType";
    SettingLevel[SettingLevel["BetType"] = 3] = "BetType";
    SettingLevel[SettingLevel["SportTypeBetType"] = 4] = "SportTypeBetType";
})(SettingLevel || (SettingLevel = {}));
var ErrorType;
(function (ErrorType) {
    ErrorType[ErrorType["Success"] = 0] = "Success";
    ErrorType[ErrorType["FormInvalid"] = 1] = "FormInvalid";
    ErrorType[ErrorType["NoChange"] = 2] = "NoChange";
})(ErrorType || (ErrorType = {}));
var MessageType;
(function (MessageType) {
    MessageType[MessageType["Success"] = 0] = "Success";
    MessageType[MessageType["Warning"] = 1] = "Warning";
    MessageType[MessageType["Error"] = 2] = "Error";
})(MessageType || (MessageType = {}));
//# sourceMappingURL=common-enums.js.map