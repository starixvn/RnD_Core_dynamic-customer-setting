"use strict";
/*jshint -W101,-W106,-W044,-W098*/

(function () {
    var resources = {};
    
    resources["label"]["en-us"] = {
        "AgentMaxPT": "Agent Max PT",
        "AgentMinPT": "Agent Min PT",
        "AgentPT": "Agent PT",
        "Agents": "Agents",
        "BetSetting": "Bet Setting",
        "Commission": "Commission",
        "CommissionForAgent": "Agent Comm",
        "CommissionForMaster": "Master Comm",
        "CommissionForMember": "Member Comm",
        "CopyTooltip": "Click to duplicate the values",
        "CustomerSetting": "Customer Setting",
        "Enable": "Enable",
        "EnableDownline": "Enable Downline",
        "Fax": "Fax",
        "FirstName": "First Name",
        "GeneralInformation": "General Information",
        "IsAuto": "Is Auto",
        "LastName": "Last Name",
        "Live": "Live",
        "MasterMaxPT": "Master Max PT",
        "MasterMinPT": "Master Min PT",
        "MasterPT": "Master PT",
        "Masters": "Masters",
        "Max": "Max",
        "MaxBet": "Max Bet",
        "MaxLive": "Max Live",
        "MaxPerMatch": "Max Per Match",
        "Members": "Members",
        "Min": "Min",
        "MinBet": "Min Bet",
        "MinLive": "Min Live",
        "MobilePhone": "Mobile Phone",
        "Nickname": "Nickname",
        "Phone": "Phone",
        "PositionTaking": "Position Taking",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "Super Max PT",
        "SuperMinPT": "Super Min PT",
        "SuperPT": "Super PT",
        "Update": "Update",
        "UserName": "UserName",
        "UserNames": "UserNames"
    };
    
    resources["message"]["en-us"] = {
        "AccessDeniedMsg": "You do not have permission to access this page.",
        "AccountWasDisabled": "Account {0} was disbaled.",
        "DataIsNotValid": "Data is not valid.",
        "DuplicateValuesAlertMsg": "Out of range for highlighted values.",
        "MaxBetLessThanOrEqualMaxPerMatch": "Max Bet must be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "Max Per Match must be greater than or equal to Max Bet",
        "MinBetLessThanOrEqualMaxBet": "Min Bet must be less than or equal to Max Bet",
        "MinBetMoreThanOrEqualTo": "Min Bet must be greater than or equal to {0}",
        "Msg_90001": "Update process is unsuccessful !",
        "NoChangeToUpdate": "There is no change to update!",
        "SportTypeOrBetTypeIsInvalid": "Sport Type or Bet Type is invalid!",
        "SubAccountIdShouldBeZeroWhenSystemIsNotAgency": "Sub Account Id Should Be Zero When System Is Not Agency",
        "SystemError": "Oops... you are here because of a system error, please notify your administrator.",
        "UpdateBetSettingSuccessfully": "Updated bet setting successfully.",
        "UpdateCommissionSuccessfully": "Updated commission setting successfully.",
        "UpdateMemberPtInfo": "You have changed your position taking",
        "UpdateMemberPtQuestion": "Do you want to update all member&apos;s position taking to:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT",
        "UpdatePositionTakingSuccessfully": "Updated position taking successfully.",
        "UpdateProductStatusSuccessfully": "Updated product status successfully.",
        "UplineLevelCannotBeNullIncaseOfEdittingMember": "Upline Level cannot be null incase of editting member",
        "UplineLevelIsNotValid": "Upline Level is not valid",
        "UplineLevelShouldBeNull": "Upline Level should be null"
    };

    angular.module("customerSetting").value("customerSettingResource", resources);
})();
