/*! Customer Setting Application v0.0.1 | (c) 2015-04-17 13:04:55 Nexcel Solutions Vietnam */
'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$window', '$rootScope', 'messageService',
            function ($q, $window, $rootScope, messageService) {
                var blockUI = function () {
                    $rootScope.hasLoading = true;
                };

                var unblockUI = function () {
                    $rootScope.hasLoading = false;
                };

                return {
                    request: function (config) {
                        blockUI();
                        return config;
                    },
                    response: function (response) {
                        unblockUI();
                        return response;
                    },
                    responseError: function (response) {
                        unblockUI();
                        messageService.showError('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            } ]);
    } ]);

    customerSettingApp.run(['$rootScope', 'resourceService', 'appSettings', function ($rootScope, resourceService, appSettings) {
        $rootScope.title = 'Customer Setting Renovation';
        resourceService.setLanguage(appSettings.defaultLanguage);
        $rootScope.resourceService = resourceService;
    } ]);
} ());

/*jshint -W101*/

angular.module('customerSettingApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/agent-edit-member-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=agent-pt data-position-taking-select=agentPT></div></div>"
  );


  $templateCache.put('templates/company-edit-super-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=super-max-pt data-position-taking-select=superMaxPT></div><div class=master-max-pt data-position-taking-select=masterMaxPT></div><div class=agent-max-pt data-position-taking-select=agentMaxPT></div><div class=super-min-pt data-position-taking-select=superMinPT></div></div>"
  );


  $templateCache.put('templates/customer-setting-bet-limit-readonly.htm',
    "<div class=bet-limit-item><div class=name title={{name}}>{{name}}</div><div class=\"min-bet read-only\" data-ng-bind=minBet></div><div class=\"max-bet read-only\" data-ng-bind=maxBet></div><div class=\"max-per-match read-only\" data-ng-bind=maxPerMatch></div></div>"
  );


  $templateCache.put('templates/customer-setting-bet-limit.htm',
    "<div class=bet-limit-item><div class=name title={{name}}>{{name}} <span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-ng-click=copy()><i class=icon-copy></i></span></div><div class=min-bet><input data-ng-model=minBet.value data-bet-limit-input data-error-message=minBet.errorMessage() data-ng-class=\"{'full-width': !hasMinBetLimit, 'invalid': !minBet.isValid()}\"> <span data-ng-show=hasMinBetLimit>&gt;&#61;{{minBet.limit | number}}</span></div><div class=max-bet><input data-ng-model=maxBet.value data-bet-limit-input data-error-message=maxBet.errorMessage() data-ng-class=\"{'full-width': !hasMaxBetLimit, 'invalid': !maxBet.isValid()}\"> <span data-ng-show=hasMaxBetLimit>&lt;&#61;{{maxBet.limit | number}}</span></div><div class=max-per-match><input data-ng-model=maxPerMatch.value data-bet-limit-input data-error-message=maxPerMatch.errorMessage() data-ng-class=\"{'full-width': !hasMaxPerMatchLimit, 'invalid': !maxPerMatch.isValid()}\"> <span data-ng-show=hasMaxPerMatchLimit>&lt;&#61;{{maxPerMatch.limit | number}}</span></div></div>"
  );


  $templateCache.put('templates/customer-setting-dialog.htm',
    "<div class=dialog-container data-ng-show=isShow><div class=dialog-scrollable><div class=dialog-content><div class=dialog-header><div class=dialog-close data-ng-click=clickClose()><i class=icon-close></i></div><div class=dialog-title>{{resourceService.getString('UpdateMemberPtTitle')}}</div></div><div class=dialog-body><span>{{resourceService.getString('UpdateMemberPtInfo')}}</span><br><span>{{resourceService.getString('UpdateMemberPtQuestion')}}</span><table class=position-taking-group><tbody><tr data-ng-repeat=\"pt in pts\"><td>{{pt.name}}</td><td><select data-ng-model=pt.value data-position-taking-select-options=pt.options></select></td></tr></tbody></table></div><div class=dialog-footer><button data-ng-click=clickYes()>{{resourceService.getString('Yes')}}</button> <button data-ng-click=clickNo()>{{resourceService.getString('No')}}</button></div></div></div><div class=dialog-overlay></div></div>"
  );


  $templateCache.put('templates/customer-setting-discount.htm',
    "<div class=discount-item><div class=name title={{name}} data-ng-bind=name></div><div class=value><select data-ng-model=value data-ng-options=\"option for option in options\"></select></div></div>"
  );


  $templateCache.put('templates/customer-setting-information.htm',
    "<div class=information-container><div class=row><div class=col-1>{{resourceService.getString('UserName')}}</div><div class=col-2 data-ng-bind=userName></div><div class=col-3>{{resourceService.getString('Nickname')}}</div><div class=col-4 data-ng-bind=nickName></div></div><div class=row><div class=col-1>{{resourceService.getString('FirstName')}}</div><div class=col-2 data-ng-bind=firstName></div><div class=col-3>{{resourceService.getString('Phone')}}</div><div class=col-4 data-ng-bind=phone></div></div><div class=row><div class=col-1>{{resourceService.getString('LastName')}}</div><div class=col-2 data-ng-bind=lastName></div><div class=col-3>{{resourceService.getString('MobilePhone')}}</div><div class=col-4 data-ng-bind=mobilePhone></div></div><div class=row><div class=col-1>{{resourceService.getString('Fax')}}</div><div class=col-2 data-ng-bind=fax></div><div class=col-3></div><div class=col-4></div></div></div>"
  );


  $templateCache.put('templates/customer-setting-message.htm',
    "<div class=message-container data-ng-show=messages><div data-ng-repeat=\"message in messages\" class=message data-ng-class=message.messageClass><div class=icon></div><div class=content data-ng-bind-html=message.messageContent></div></div></div>"
  );


  $templateCache.put('templates/customer-setting-status.htm',
    "<div class=status-item><div class=name title={{name}} data-ng-bind=name></div><div class=value><label class=checkbox><input type=checkbox data-ng-model=enable data-ng-disabled=\"enableDisabled\"> {{resourceService.getString('Enable')}}</label><label class=\"checkbox downline\" data-ng-show=showDownline><input type=checkbox data-ng-model=downline data-ng-disabled=\"downlineDisabled\"> {{resourceService.getString('EnableDownline')}}</label></div></div>"
  );


  $templateCache.put('templates/customer-setting-tooltip.htm',
    "<div class=tooltip data-ng-class=align><div class=tooltip-arrow></div><div class=tooltip-inner data-ng-bind=message></div></div>"
  );


  $templateCache.put('templates/customer-setting-view.htm',
    "<div class=view data-ng-controller=CustomerSettingController><div class=container data-ng-show=information><div class=header>{{resourceService.getString('GeneralInformation')}}</div><div class=body data-customer-setting-information=information></div></div><div class=container data-ng-show=statuses><div class=header>{{resourceService.getString('ProductStatus')}} <span title=\"{{resourceService.getString('Update')}}\" class=update data-ng-click=updateStatuses()><i class=icon-update></i></span></div><div class=body data-ng-include=statusTemplate></div></div><div class=container data-ng-show=discounts><div class=header>{{resourceService.getString('Commission')}} <span data-ng-show=hasUpdateDiscount title=\"{{resourceService.getString('Update')}}\" class=update data-ng-click=updateDiscounts()><i class=icon-update></i></span></div><div class=body data-ng-include=discountTemplate></div></div><div class=container data-ng-show=betLimits><div class=header>{{resourceService.getString('BetSetting')}} <span data-ng-show=hasUpdateBetLimit title=\"{{resourceService.getString('Update')}}\" class=update data-ng-click=updateBetLimits()><i class=icon-update></i></span></div><div class=body data-ng-include=betLimitTemplate></div></div><div class=container data-ng-show=positionTakings><div class=header>{{resourceService.getString('PositionTaking')}} <span title=\"{{resourceService.getString('Update')}}\" class=update data-ng-click=updatePositionTakings()><i class=icon-update></i></span></div><div class=body data-ng-include=positionTakingTemplate></div></div><button class=update data-ng-show=information data-ng-click=updateAll()>{{resourceService.getString('Update')}}</button></div>"
  );


  $templateCache.put('templates/master-edit-agent-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=master-pt data-position-taking-select=masterPT></div><div class=master-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"masterIsAuto\"></label></div><div class=agent-max-pt data-position-taking-select=agentMaxPT></div><div class=agent-min-pt data-position-taking-select=agentMinPT></div></div>"
  );


  $templateCache.put('templates/master-edit-member-discount.htm',
    "<div class=discount-item><div class=name title={{name}} data-ng-bind=name></div><div class=agent-discount data-ng-bind=agentDiscount></div><div class=member-discount data-ng-bind=memberDiscount></div></div>"
  );


  $templateCache.put('templates/master-edit-member-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=master-pt data-position-taking-select=masterPT></div><div class=master-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"masterIsAuto\"></label></div><div class=agent-pt data-ng-bind=agentPT></div></div>"
  );


  $templateCache.put('templates/no-group/agent-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container agent-edit-member\"><div class=title><div class=name></div><div class=agent-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-agent-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/betlimit-container.htm',
    "<div class=bet-limit-container><div class=title><div class=name></div><div class=min-bet>{{resourceService.getString('MinBet')}}</div><div class=max-bet>{{resourceService.getString('MaxBet')}}</div><div class=max-per-match>{{resourceService.getString('MaxPerMatch')}}</div></div><div data-ng-repeat=\"betLimit in betLimits\" data-customer-setting-bet-limit=betLimit data-has-copy=\"betLimit==betLimits[0] && betLimits.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/company-edit-super-position-taking-container.htm',
    "<div class=\"position-taking-container company-edit-super\"><div class=title><div class=name></div><div class=super-max-pt>{{resourceService.getString('SuperMaxPT')}}</div><div class=master-max-pt>{{resourceService.getString('MasterMaxPT')}}</div><div class=agent-max-pt>{{resourceService.getString('AgentMaxPT')}}</div><div class=super-min-pt>{{resourceService.getString('SuperMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-company-edit-super-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/discount-container.htm',
    "<div class=discount-container><div data-ng-repeat=\"discount in discounts\" data-customer-setting-discount=discount data-ng-class=\"{'full-width': $last && $even}\"></div></div>"
  );


  $templateCache.put('templates/no-group/master-edit-agent-position-taking-container.htm',
    "<div class=\"position-taking-container master-edit-agent\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\"><div class=title><div class=name></div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=agent-max-pt>{{resourceService.getString('AgentMaxPT')}}</div><div class=agent-min-pt>{{resourceService.getString('AgentMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-master-edit-agent-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/master-edit-member-discount-container.htm',
    "<div class=\"discount-container master-edit-member\"><div class=title><div class=name></div><div class=agent-discount>{{resourceService.getString('CommissionForAgent')}}</div><div class=member-discount>{{resourceService.getString('CommissionForMember')}}</div></div><div data-ng-repeat=\"discount in discounts\" data-master-edit-member-discount=discount></div></div>"
  );


  $templateCache.put('templates/no-group/master-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container master-edit-member\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\"><div class=title><div class=name></div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=agent-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-master-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/status-container.htm',
    "<div class=status-container><div data-ng-repeat=\"status in statuses\" data-customer-setting-status=status data-ng-class=\"{'full-width': $last && $even}\"></div></div>"
  );


  $templateCache.put('templates/no-group/super-edit-master-position-taking-container.htm',
    "<div class=\"position-taking-container super-edit-master\"><div class=title><div class=name></div><div class=super-pt>{{resourceService.getString('SuperPT')}}</div><div class=super-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=master-max-pt>{{resourceService.getString('MasterMaxPT')}}</div><div class=master-min-pt>{{resourceService.getString('MasterMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-super-edit-master-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/super-edit-member-discount-container.htm',
    "<div class=\"discount-container super-edit-member\"><div class=title><div class=name></div><div class=master-discount>{{resourceService.getString('CommissionForMaster')}}</div><div class=agent-discount>{{resourceService.getString('CommissionForAgent')}}</div><div class=member-discount>{{resourceService.getString('CommissionForMember')}}</div></div><div data-ng-repeat=\"discount in discounts\" data-super-edit-member-discount=discount></div></div>"
  );


  $templateCache.put('templates/no-group/super-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container super-edit-member\"><div class=title><div class=name></div><div class=super-pt>{{resourceService.getString('SuperPT')}}</div><div class=super-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakings\" data-super-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/position-taking-select.htm',
    "<select data-ng-model=data.value data-ng-disabled=data.disabled data-ng-class=\"{'invalid': !data.valid}\" data-position-taking-select-options=data.options></select>"
  );


  $templateCache.put('templates/sport-group/agent-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container agent-edit-member\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=agent-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-agent-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/betlimit-container.htm',
    "<div data-ng-init=\"betLimitGroups=(betLimits|dataGroup)\" class=bet-limit-container><div data-ng-repeat=\"betLimitGroup in betLimitGroups\" class=sport-group><div class=sport-name title={{betLimitGroup.SportTypeName}}>{{betLimitGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=min-bet>{{resourceService.getString('MinBet')}}</div><div class=max-bet>{{resourceService.getString('MaxBet')}}</div><div class=max-per-match>{{resourceService.getString('MaxPerMatch')}}</div></div><div data-ng-repeat=\"item in betLimitGroup.Items\" data-customer-setting-bet-limit=item data-has-copy=\"item==betLimits[0] && betLimits.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/company-edit-super-position-taking-container.htm',
    "<div class=\"position-taking-container company-edit-super\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-max-pt>{{resourceService.getString('SuperMaxPT')}}</div><div class=master-max-pt>{{resourceService.getString('MasterMaxPT')}}</div><div class=agent-max-pt>{{resourceService.getString('AgentMaxPT')}}</div><div class=super-min-pt>{{resourceService.getString('SuperMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-company-edit-super-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/discount-container.htm',
    "<div class=discount-container data-ng-init=\"discountGroups=(discounts|dataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div data-ng-repeat=\"item in discountGroup.Items\" data-customer-setting-discount=item data-ng-class=\"{'full-width': $last && $even}\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/master-edit-agent-position-taking-container.htm',
    "<div class=\"position-taking-container master-edit-agent\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=agent-max-pt>{{resourceService.getString('AgentMaxPT')}}</div><div class=agent-min-pt>{{resourceService.getString('AgentMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-master-edit-agent-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/master-edit-member-discount-container.htm',
    "<div class=\"discount-container master-edit-member\" data-ng-init=\"discountGroups=(discounts|dataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=agent-discount>{{resourceService.getString('CommissionForAgent')}}</div><div class=member-discount>{{resourceService.getString('CommissionForMember')}}</div></div><div data-ng-repeat=\"item in discountGroup.Items\" data-master-edit-member-discount=item></div></div></div>"
  );


  $templateCache.put('templates/sport-group/master-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container master-edit-member\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=agent-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-master-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/status-container.htm',
    "<div class=status-container data-ng-init=\"statusGroups=(statuses|dataGroup)\"><div data-ng-repeat=\"statusGroup in statusGroups\" class=sport-group><div class=sport-name title={{statusGroup.SportTypeName}}>{{statusGroup.SportTypeName}}</div><div data-ng-repeat=\"item in statusGroup.Items\" data-customer-setting-status=item data-ng-class=\"{'full-width': $last && $even}\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/super-edit-master-position-taking-container.htm',
    "<div class=\"position-taking-container super-edit-master\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-pt>{{resourceService.getString('SuperPT')}}</div><div class=super-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=master-max-pt>{{resourceService.getString('MasterMaxPT')}}</div><div class=master-min-pt>{{resourceService.getString('MasterMinPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-super-edit-master-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/super-edit-member-discount-container.htm',
    "<div class=\"discount-container super-edit-member\" data-ng-init=\"discountGroups=(discounts|dataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-discount>{{resourceService.getString('CommissionForMaster')}}</div><div class=agent-discount>{{resourceService.getString('CommissionForAgent')}}</div><div class=member-discount>{{resourceService.getString('CommissionForMember')}}</div></div><div data-ng-repeat=\"item in discountGroup.Items\" data-super-edit-member-discount=item></div></div></div>"
  );


  $templateCache.put('templates/sport-group/super-edit-member-position-taking-container.htm',
    "<div class=\"position-taking-container super-edit-member\" data-ng-init=\"positionTakingGroups=(positionTakings|dataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-pt>{{resourceService.getString('SuperPT')}}</div><div class=super-is-auto>{{resourceService.getString('IsAuto')}}</div><div class=master-pt>{{resourceService.getString('MasterPT')}}</div><div class=master-pt>{{resourceService.getString('AgentPT')}}</div></div><div data-ng-repeat=\"item in positionTakingGroup.Items\" data-super-edit-member-position-taking=item data-has-copy=\"item==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/super-edit-master-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=super-pt data-position-taking-select=superPT></div><div class=super-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"superIsAuto\"></label></div><div class=master-max-pt data-position-taking-select=masterMaxPT></div><div class=master-min-pt data-position-taking-select=masterMinPT></div></div>"
  );


  $templateCache.put('templates/super-edit-member-discount.htm',
    "<div class=discount-item><div class=name title={{name}} data-ng-bind=name></div><div class=master-discount data-ng-bind=masterDiscount></div><div class=agent-discount data-ng-bind=agentDiscount></div><div class=member-discount data-ng-bind=memberDiscount></div></div>"
  );


  $templateCache.put('templates/super-edit-member-position-taking.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<span class=copy title=\"{{resourceService.getString('CopyTooltip')}}\" data-ng-show=hasCopy data-customer-setting-copy><i class=icon-copy></i></span></div><div class=super-pt data-position-taking-select=superPT></div><div class=super-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"superIsAuto\"></label></div><div class=master-pt data-ng-bind=masterPT></div><div class=agent-pt data-ng-bind=agentPT></div></div>"
  );
} ]);

'use strict';

(function () {
    var injectParams = ['$rootScope', '$scope', 'templateService', 'dataService', 'controllerService',
                        'appSettings', 'modelService', 'messageService', 'resourceService', 'ResponseReader'];
    var customerSettingController = function ($rootScope, $scope, templateService, dataService, controllerService,
                        appSettings, modelService, messageService, resourceService, ResponseReader) {
        var selectModel = modelService.createSelectModel();
        dataService.select(selectModel, function (response) {
            controllerService.updateResponse(response);
            var requestContext = controllerService.getRequestContext();
            var customerSetting = controllerService.getCustomerSetting();
            var settingContext = controllerService.getSettingContext();
            resourceService.setLanguage(requestContext.Language);

            $scope.information = customerSetting.Information;
            $scope.statuses = customerSetting.Statuses;
            $scope.statusTemplate = templateService.resolveStatus();
            $scope.discounts = customerSetting.Discounts;
            $scope.hasUpdateDiscount = requestContext.FormType !== appSettings.formType.SuperEditMember
                && requestContext.FormType !== appSettings.formType.MasterEditMember;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.betLimits = customerSetting.BetLimits;
            $scope.hasUpdateBetLimit = $scope.hasUpdateDiscount;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (requestContext.FormType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = settingContext.DisableAutoMPT;
            }
        });

        var selectSettingType;
        $scope.updateStatuses = function () {
            selectSettingType = appSettings.settingType.Status;
            update();
        };

        $scope.updateDiscounts = function () {
            selectSettingType = appSettings.settingType.Discount;
            update();
        };

        $scope.updateBetLimits = function () {
            selectSettingType = appSettings.settingType.BetLimit;
            update();
        };

        $scope.updatePositionTakings = function () {
            selectSettingType = appSettings.settingType.PositionTaking;
            update();
        };

        $scope.updateAll = function () {
            selectSettingType = appSettings.settingType.All;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function update(updateMemberPT) {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();

            function hasUpdate(data, isShowNoChange) {
                if (angular.isUndefined(data)) {
                    return false; // form invalid
                }

                if (data.length === 0) {
                    if (isShowNoChange) {
                        messageService.showNoChange();
                    }
                    return false;
                }

                return true;
            }

            function hasUncheckAutoPT(data) {
                var isAutoPTName, ptName;
                switch (requestContext.FormType) {
                    case appSettings.formType.SuperEditMaster:
                        isAutoPTName = 'IS3';
                        ptName = 'S3';
                        break;

                    case appSettings.formType.MasterEditAgent:
                        isAutoPTName = 'IM2';
                        ptName = 'M2';
                        break;
                }

                if (angular.isDefined(isAutoPTName) && angular.isDefined(ptName)) {
                    var memberPTs = [];
                    angular.forEach(data, function (pt) {
                        var isAuto = pt[isAutoPTName];
                        if (isAuto === false) {
                            var ptValue = pt[ptName];
                            var name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, pt);
                            memberPTs.push({
                                SportType: pt.SportType,
                                BetType: pt.BetType,
                                IsLive: pt.IsLive,
                                Name: name,
                                Pt: ptValue
                            });
                        }
                    });

                    if (memberPTs.length > 0) {
                        $rootScope.updateModal = {
                            MemberPTs: memberPTs
                        };
                        return true;
                    }
                }

                return false;
            }

            messageService.hide();
            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (selectSettingType) {
                case appSettings.settingType.All:
                    statuses = controllerService.getUpdateData(appSettings.settingType.Status);
                    discounts = controllerService.getUpdateData(appSettings.settingType.Discount);
                    betLimits = controllerService.getUpdateData(appSettings.settingType.BetLimit);
                    positionTakings = controllerService.getUpdateData(appSettings.settingType.PositionTaking);
                    var hasUpdatePT;
                    if (!hasUpdate(statuses, false)
                        && !hasUpdate(discounts, false)
                        && !hasUpdate(betLimits, false)
                        && !(hasUpdatePT = hasUpdate(positionTakings, true))) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUpdatePT && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }

                    break;

                case appSettings.settingType.PositionTaking:
                    positionTakings = controllerService.getUpdateData(appSettings.settingType.PositionTaking);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.settingType.BetLimit:
                    betLimits = controllerService.getUpdateData(appSettings.settingType.BetLimit);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Discount:
                    discounts = controllerService.getUpdateData(appSettings.settingType.Discount);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.settingType.Status:
                    statuses = controllerService.getUpdateData(appSettings.settingType.Status);
                    if (!hasUpdate(statuses, true)) {
                        return;
                    }
                    break;
            }

            if (angular.isDefined(updateMemberPT)) {
                var isUpdateAllMembers = updateMemberPT.IsUpdateAllMembers;
                var memberPTs = updateMemberPT.MemberPTs;
                var newPositionTakings = [];
                angular.forEach(positionTakings, function (positionTaking) {
                    var isFound = false;
                    var newPositionTaking = positionTaking;
                    angular.forEach(memberPTs, function (memberPt) {
                        if (!isFound
                            && newPositionTaking.SportType === memberPt.SportType
                            && newPositionTaking.BetType === memberPt.BetType
                            && newPositionTaking.IsLive === memberPt.IsLive) {
                            newPositionTaking = angular.extend({
                                IsUpdateAllMembers: isUpdateAllMembers,
                                MemberPositionTaking: memberPt.Pt
                            }, newPositionTaking);
                            newPositionTakings.push(newPositionTaking);
                            isFound = true;
                        }
                    });
                });

                positionTakings = newPositionTakings;
            }

            // delete un-used properties
            angular.forEach(positionTakings, function (positionTaking) {
                delete (positionTaking.SportTypeName);
                delete (positionTaking.BetTypeName);
            });

            function reload(oldData, newData) {
                angular.forEach(newData, function (newItem) {
                    angular.forEach(oldData, function (oldItem) {
                        var isFound = false;
                        if (!isFound
                            && newItem.SportType === oldItem.SportType
                            && newItem.BetType === oldItem.BetType
                            && newItem.IsLive === oldItem.IsLive) {
                            angular.copy(newItem, oldItem);
                            isFound = true;
                        }
                    });
                });
            }

            var selectModel = modelService.createSelectModel();
            var updateModel = angular.extend({
                UserName: $scope.information.UserName,
                SettingType: selectSettingType,
                StatusLevel: settingContext.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: settingContext.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: settingContext.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: settingContext.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, selectModel);
            dataService.update(updateModel, function (response) {
                var responseReader = new ResponseReader(updateModel.SettingType);
                var settingResults = responseReader.read(response);
                angular.forEach(settingResults, function (settingResult) {
                    var result = settingResult.result;
                    if (result.IsSuccess) {
                        var settingType = settingResult.settingType;
                        switch (settingType) {
                            case appSettings.settingType.Status:
                                reload($scope.statuses, statuses);
                                break;

                            case appSettings.settingType.Discount:
                                reload($scope.discounts, discounts);
                                break;

                            case appSettings.settingType.BetLimit:
                                reload($scope.betLimits, betLimits);
                                break;

                            case appSettings.settingType.PositionTaking:
                                reload($scope.positionTakings, positionTakings);
                                break;
                        }
                    }
                });
            });
        }
    };

    customerSettingController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingController', customerSettingController);
} ());

'use strict';

(function () {
    var injectParams = ['$scope', '$rootScope', '$timeout', 'modelService', 'templateService', 'messageService'];
    var customerSettingViewController = function ($scope, $rootScope, $timeout, modelService, templateService, messageService) {
        $rootScope.$on('$locationChangeStart', function () {
            $scope.view = undefined;
            messageService.hide();
        });

        $rootScope.$on('$locationChangeSuccess', function () {
            if (!modelService.isValid) {
                messageService.showError('Please check your URL');
            } else {
                $timeout(function () {
                    var modelParams = modelService.params;
                    $scope.view = templateService.resolveView(modelParams.productId);
                });
            }
        });
    };

    customerSettingViewController.$inject = injectParams;
    angular.module('customerSettingApp').controller('CustomerSettingViewController', customerSettingViewController);
} ());

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var agentEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=agentEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/agent-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                var isCopied = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Agent PT
                function generateAgentPT() {
                    var min = positionTaking.N2;
                    var max = positionTaking.A2;
                    var value = !isInitialized
                        ? positionTaking.A1
                        : commonService.correctValue(min, max, $scope.agentPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentPT = generateAgentPT();
                $scope.$watch('agentPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.agentPT = generateAgentPT();
                        isCopied = false;
                    }
                });

                // Copy
                $scope.copy = function (source) {
                    return commonService.copyValue(source, $scope, 'agentPT', generateAgentPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (positionTaking.A1 === $scope.agentPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            BetType: positionTaking.BetType,
                            IsLive: positionTaking.IsLive,
                            A1: $scope.agentPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    agentEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('agentEditMemberPositionTaking', agentEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$filter', 'commonService', 'tooltipService', 'appSettings'];
    var betLimitInputDirective = function ($filter, commonService, tooltipService, appSettings) {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                errorMessage: '='
            },
            link: function (scope, element, attrs, modelCtrl) {
                attrs.$set('maxlength', appSettings.betLimitMaxLength);

                scope.$watch('errorMessage', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        if (angular.isDefined(scope.errorMessage)) {
                            if (element[0] === document.activeElement) {
                                tooltipService.show(element, scope.errorMessage);
                            }
                        } else {
                            if (element[0] === document.activeElement) {
                                tooltipService.hide();
                            }
                        }
                    }
                });

                element.on('focus', function () {
                    if (angular.isDefined(scope.errorMessage)) {
                        tooltipService.show(element, scope.errorMessage);
                    }
                });

                element.on('blur', function () {
                    if (angular.isDefined(scope.errorMessage)) {
                        tooltipService.hide();
                    }
                });

                element.on('keydown', function (event) {
                    if (modelCtrl.$modelValue === 0
                        && commonService.inArray(event.keyCode, [46, 8]) !== -1) {
                        event.preventDefault();
                        return;
                    }

                    if (commonService.inArray(event.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 // Allow: backspace, delete, tab, escape, enter
                        || (event.keyCode === 65 && event.ctrlKey === true) // Allow: Ctrl+A
                        || (event.keyCode >= 35 && event.keyCode <= 40)) { // Allow: home, end, left, right, down, up
                        // let it happen, don't do anything
                        return;
                    }

                    if ((event.shiftKey || (event.keyCode < 48 || event.keyCode > 57)) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                        return;
                    }
                });

                modelCtrl.$formatters.unshift(function () {
                    return $filter('number')(modelCtrl.$modelValue);
                });

                modelCtrl.$parsers.unshift(function (viewValue) {
                    var value = 0;
                    if (viewValue) {
                        var plainNumber = viewValue.replace(/[\,\.]/g, '');
                        value = parseInt(plainNumber);
                    }

                    element.val($filter('number')(value));
                    return value;
                });
            }
        };
    };

    betLimitInputDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('betLimitInput', betLimitInputDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var companyEditSuperPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=companyEditSuperPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/company-edit-super-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Max PT
                function generateSuperMaxPT() {
                    var min = 0;
                    var max = positionTaking.DefaultMaxPT;
                    var value = !isInitialized
                        ? positionTaking.S4
                        : commonService.correctValue(min, max, $scope.superMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superMaxPT = generateSuperMaxPT();
                $scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterMaxPT = generateMasterMaxPT();
                        $scope.superMinPT = generateSuperMinPT();
                    }
                });

                // Master Max PT
                function generateMasterMaxPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.M4
                        : commonService.correctValue(min, max, $scope.masterMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Agent Max PT
                function generateAgentMaxPT() {
                    var min = 0;
                    var max = $scope.masterMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.A4
                        : commonService.correctValue(min, max, $scope.agentMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();

                // Super Min PT
                function generateSuperMinPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N4
                        : commonService.correctValue(min, max, $scope.superMinPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superMinPT = generateSuperMinPT();

                // Copy
                $scope.copy = function (source) {
                    return commonService.copyValue(source, $scope, 'superMaxPT', generateSuperMaxPT)
                        && commonService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && commonService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && commonService.copyValue(source, $scope, 'superMinPT', generateSuperMinPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (positionTaking.S4 === $scope.superMaxPT.value
                        && positionTaking.M4 === $scope.masterMaxPT.value
                        && positionTaking.A4 === $scope.agentMaxPT.value
                        && positionTaking.N4 === $scope.superMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            BetType: positionTaking.BetType,
                            IsLive: positionTaking.IsLive,
                            S4: $scope.superMaxPT.value,
                            M4: $scope.masterMaxPT.value,
                            A4: $scope.agentMaxPT.value,
                            N4: $scope.superMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    companyEditSuperPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('companyEditSuperPositionTaking', companyEditSuperPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$filter', 'resourceService', 'controllerService', 'appSettings'];
    var errorType = {
        Limit: 1,
        MinBet: 2,
        MaxBet: 3,
        MaxPerMatch: 4
    };
    var customerSettingBetLimitDirective = function ($rootScope, $filter, resourceService, controllerService, appSettings) {
        function parse(data, formType) {
            var BetLimitInput = function (value, limit) {
                this.errors = [];
                this.value = value;
                this.limit = limit;

                this.setInvalid = function (errorType, errMsg) {
                    var hasErrType = false;
                    angular.forEach(this.errors, function (error) {
                        if (!hasErrType) {
                            if (error.errorType === errorType) {
                                hasErrType = true;
                            }
                        }
                    });

                    if (!hasErrType) {
                        this.errors.push({
                            errorType: errorType,
                            errMsg: errMsg
                        });
                    }
                };

                this.setValid = function (errorType) {
                    var hasErrType = false;
                    var errorIndex = -1;
                    angular.forEach(this.errors, function (error, index) {
                        if (!hasErrType) {
                            if (error.errorType === errorType) {
                                hasErrType = true;
                                errorIndex = index;
                            }
                        }
                    });

                    if (hasErrType) {
                        this.errors.splice(errorIndex, 1);
                    }
                };

                this.clearErrors = function () {
                    this.errors.length = 0;
                };

                this.isValid = function () {
                    return this.errors.length === 0;
                };

                this.errorMessage = function () {
                    if (this.errors.length === 0) {
                        return undefined;
                    }

                    return this.errors[0].errMsg;
                };
            };

            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, appSettings.defaultMaxBet),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, appSettings.defaultMaxBet)
                    };

                case appSettings.formType.SuperEditMaster:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

                case appSettings.formType.MasterEditAgent:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };

                case appSettings.formType.SuperEditMember:
                    return {
                        minBet: data.MinBet,
                        maxBet: data.MaxBet,
                        maxPerMatch: data.MaxPerMatch
                    };

                case appSettings.formType.MasterEditMember:
                    return {
                        minBet: data.MinBet,
                        maxBet: data.MaxBet,
                        maxPerMatch: data.MaxPerMatch
                    };

                case appSettings.formType.AgentEditMember:
                    return {
                        minBet: new BetLimitInput(data.MinBet, data.MinBetMin),
                        maxBet: new BetLimitInput(data.MaxBet, data.MaxBetMax),
                        maxPerMatch: new BetLimitInput(data.MaxPerMatch, data.MaxPerMatchMax)
                    };
            }
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                betLimit: '=customerSettingBetLimit',
                hasCopy: '='
            },
            template: '<div data-ng-include="templateUrl"></div>',
            controller: ['$scope', function ($scope) {
                controllerService.linkBetLimit($scope);
                var betLimit = $scope.betLimit;
                var requestContext = controllerService.getRequestContext();

                $scope.isReadonly = requestContext.FormType === appSettings.formType.SuperEditMember
                    || requestContext.FormType === appSettings.formType.MasterEditMember;
                $scope.templateUrl = $scope.isReadonly
                    ? 'templates/customer-setting-bet-limit-readonly.htm'
                    : 'templates/customer-setting-bet-limit.htm';

                var data = parse(betLimit, requestContext.FormType);
                $scope.minBet = data.minBet;
                $scope.hasMinBetLimit = $scope.minBet.limit > 0;
                $scope.maxBet = data.maxBet;
                $scope.hasMaxBetLimit = requestContext.FormType !== appSettings.formType.CompanyEditSuper;
                $scope.maxPerMatch = data.maxPerMatch;
                $scope.hasMaxPerMatchLimit = $scope.hasMaxBetLimit;
                $scope.appSettings = appSettings;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.BetLimit, betLimit);
                $scope.resourceService = resourceService;

                if ($scope.isReadonly) {
                    return;
                }

                if ($scope.hasCopy) {
                    $scope.copy = function () {
                        $rootScope.$broadcast('copy-bet-limit', $scope);
                    };
                }

                $scope.$on('copy-bet-limit', function (event, copyScope) {
                    if (copyScope !== $scope) {
                        $scope.minBet.value = copyScope.minBet.value;
                        $scope.maxBet.value = copyScope.maxBet.value;
                        $scope.maxPerMatch.value = copyScope.maxPerMatch.value;
                    }
                });

                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if (!$scope.minBet.isValid() || !$scope.maxBet.isValid() || !$scope.maxPerMatch.isValid()) {
                        errCode = appSettings.errorType.FormInvalid;
                    } else if (betLimit.MinBet === $scope.minBet.value
                        && betLimit.MaxBet === $scope.maxBet.value
                        && betLimit.MaxPerMatch === $scope.maxPerMatch.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: betLimit.SportType,
                            BetType: betLimit.BetType,
                            MinBet: $scope.minBet.value,
                            MaxBet: $scope.maxBet.value,
                            MaxPerMatch: $scope.maxPerMatch.value
                        }
                    };
                };
            } ],
            link: function (scope) {
                if (scope.isReadonly) {
                    return;
                }

                var validateCount = 0;

                function resetValidate() {
                    validateCount = 0;
                }

                function hasValidate() {
                    validateCount++;
                    if (validateCount === 3) {
                        validateCount = 0;
                        return false;
                    }

                    if (scope.minBet.value === scope.maxBet.value
                        && scope.maxBet.value === scope.maxPerMatch.value
                        && scope.maxPerMatch.value === 0) {
                        scope.minBet.clearErrors();
                        scope.maxBet.clearErrors();
                        scope.maxPerMatch.clearErrors();
                        return false;
                    }

                    return true;
                }

                var validateMinBet = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.minBet.value < scope.minBet.limit) {
                        var formattedNumber = $filter('number')(scope.minBet.limit);
                        scope.minBet.setInvalid(errorType.Limit, resourceService.getFormatString('MinBetMoreThanOrEqualTo', formattedNumber));
                    }
                    else {
                        scope.minBet.setValid(errorType.Limit);
                    }

                    scope.maxBet.setValid(errorType.MinBet);
                    if (scope.minBet.value > scope.maxBet.value) {
                        scope.minBet.setInvalid(errorType.MaxBet, resourceService.getString('MinBetLessThanOrEqualMaxBet'));
                    }
                    else {
                        scope.minBet.setValid(errorType.MaxBet);
                    }

                    if (scope.minBet.isValid() && scope.maxPerMatch.isValid()) {
                        validateMaxBet();
                    }
                };

                var validateMaxBet = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.maxBet.value > scope.maxBet.limit) {
                        var formattedNumber = $filter('number')(scope.maxBet.limit);
                        scope.maxBet.setInvalid(errorType.Limit, resourceService.getFormatString('MaxBetLessThanOrEqualTo', formattedNumber));
                    }
                    else {
                        scope.maxBet.setValid(errorType.Limit);
                    }

                    scope.minBet.setValid(errorType.MaxBet);
                    if (scope.maxBet.value < scope.minBet.value) {
                        scope.maxBet.setInvalid(errorType.MinBet, resourceService.getString('MaxBetMoreThanOrEqualMinBet'));
                    } else {
                        scope.maxBet.setValid(errorType.MinBet);
                    }

                    scope.maxPerMatch.setValid(errorType.MaxBet);
                    if (scope.maxBet.value > scope.maxPerMatch.value) {
                        scope.maxBet.setInvalid(errorType.MaxPerMatch, resourceService.getString('MaxBetLessThanOrEqualMaxPerMatch'));
                    }
                    else {
                        scope.maxBet.setValid(errorType.MaxPerMatch);
                    }

                    if (scope.minBet.isValid() && scope.maxBet.isValid()) {
                        validateMaxPerMatch();
                    }
                };

                var validateMaxPerMatch = function () {
                    if (!hasValidate()) {
                        return;
                    }

                    if (scope.maxPerMatch.value > scope.maxPerMatch.limit) {
                        var errMsg = resourceService.getFormatString('MaxPerMatchLessThanOrEqualTo', $filter('number')(scope.maxPerMatch.limit));
                        scope.maxPerMatch.setInvalid(errorType.Limit, errMsg);
                    } else {
                        scope.maxPerMatch.setValid(errorType.Limit);
                    }

                    scope.maxBet.setValid(errorType.MaxPerMatch);
                    if (scope.maxPerMatch.value < scope.maxBet.value) {
                        scope.maxPerMatch.setInvalid(errorType.MaxBet, resourceService.getString('MaxPerMatchMoreThanOrEqualMaxBet'));
                    }
                    else {
                        scope.maxPerMatch.setValid(errorType.MaxBet);
                    }

                    if (scope.maxBet.isValid() && scope.maxPerMatch.isValid()) {
                        validateMinBet();
                    }
                };

                resetValidate();
                validateMinBet();

                scope.$watch('minBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMinBet();
                    }
                });

                scope.$watch('maxBet.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMaxBet();
                    }
                });

                scope.$watch('maxPerMatch.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        resetValidate();
                        validateMaxPerMatch();
                    }
                });
            }
        };
    };

    customerSettingBetLimitDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingBetLimit', customerSettingBetLimitDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'controllerService', 'appSettings', 'tooltipService'];
    var customerSettingCopyDirective = function ($rootScope, controllerService, appSettings, tooltipService) {
        return {
            restrict: 'A',
            link: function (scope, element) {
                element.on('click', function () {
                    var copy = function () {
                        var count = 0;
                        var scopes = controllerService.getScopes(appSettings.settingType.PositionTaking);
                        var source = scopes[0];
                        for (var i = 1; i < scopes.length; i++) {
                            var scope = scopes[i];
                            var result = scope.copy(source);
                            if (!result) {
                                count++;
                            }

                            if (count > 0) {
                                tooltipService.show(element, 'Some bet types can not be copied', 'left');
                            } else {
                                tooltipService.hide();
                            }
                        }
                    };

                    if ($rootScope.$$phase) {
                        scope.$evalAsync(copy);
                    } else {
                        scope.$apply(copy);
                    }
                });
            }
        };
    };

    customerSettingCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingCopy', customerSettingCopyDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService'];
    var customerSettingDialogDirective = function ($rootScope, commonService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                data: '=customerSettingDialog'
            },
            templateUrl: 'templates/customer-setting-dialog.htm',
            link: function (scope) {
                scope.resourceService = resourceService;
                scope.isShow = false;
                function update(isUpdateAllMembers) {
                    var memberPTs = [];
                    angular.forEach(scope.pts, function (pt) {
                        memberPTs.push({
                            SportType: pt.sportType,
                            BetType: pt.betType,
                            IsLive: pt.isLive,
                            Pt: isUpdateAllMembers ? pt.value : 0
                        });
                    });
                    $rootScope.$broadcast('update-member-position-taking', {
                        IsUpdateAllMembers: isUpdateAllMembers,
                        MemberPTs: memberPTs
                    });
                }

                scope.clickClose = function () {
                    scope.isShow = false;
                };
                scope.clickYes = function () {
                    scope.isShow = false;
                    update(true);
                };
                scope.clickNo = function () {
                    scope.isShow = false;
                    update(false);
                };

                scope.$watch('data', function (newValue, oldValue) {
                    if (newValue === oldValue) {
                        return;
                    }

                    if (angular.isUndefined(scope.data)) {
                        scope.isShow = false;
                        return;
                    }

                    var memberPtss = scope.data.MemberPTs;
                    var pts = [];
                    angular.forEach(memberPtss, function (memberPts) {
                        var ptSelect = commonService.createPositionTakingSelect(0, memberPts.Pt, memberPts.Pt);
                        pts.push({
                            sportType: memberPts.SportType,
                            betType: memberPts.BetType,
                            isLive: memberPts.IsLive,
                            name: memberPts.Name,
                            value: ptSelect.value,
                            options: ptSelect.options
                        });
                    });
                    scope.isShow = true;
                    scope.pts = pts;
                });
            }
        };
    };

    customerSettingDialogDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDialog', customerSettingDialogDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var customerSettingDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        function parse(data, formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return commonService.createDiscountSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);

                case appSettings.formType.SuperEditMaster:
                    return commonService.createDiscountSelect(0, data.SuperDiscount, data.MasterDiscount);

                case appSettings.formType.MasterEditAgent:
                    return commonService.createDiscountSelect(0, data.MasterDiscount, data.AgentDiscount);

                case appSettings.formType.AgentEditMember:
                    return commonService.createDiscountSelect(0, data.AgentDiscount, data.PlayerDiscount);
            }
        }

        function getName(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return 'SuperDiscount';

                case appSettings.formType.SuperEditMaster:
                    return 'MasterDiscount';

                case appSettings.formType.MasterEditAgent:
                    return 'AgentDiscount';

                case appSettings.formType.AgentEditMember:
                    return 'PlayerDiscount';
            }
        }

        function getValue(data, formType) {
            var name = getName(formType);
            return data[name];
        }

        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=customerSettingDiscount'
            },
            templateUrl: 'templates/customer-setting-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                var requestContext = controllerService.getRequestContext();
                var data = parse(discount, requestContext.FormType);
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, discount);
                $scope.min = data.min;
                $scope.value = data.value;
                $scope.options = data.options;

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue(discount, requestContext.FormType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: discount.SportType,
                        BetType: discount.BetType
                    };
                    data[getName(requestContext.FormType)] = $scope.value;
                    return {
                        errCode: errCode,
                        data: data
                    };
                };
            } ]
        };
    };

    customerSettingDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingDiscount', customerSettingDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['resourceService'];
    var customerSettingInformationDirective = function (resourceService) {
        return {
            restrict: 'AE',
            scope: {
                information: '=customerSettingInformation'
            },
            templateUrl: 'templates/customer-setting-information.htm',
            controller: ['$scope', function ($scope) {
                $scope.resourceService = resourceService;
                $scope.$watch('information', function () {
                    if (angular.isUndefined($scope.information)) {
                        return;
                    }

                    $scope.userName = $scope.information.UserName;
                    $scope.nickName = $scope.information.UserName2;
                    $scope.firstName = $scope.information.FirstName;
                    $scope.lastName = $scope.information.LastName;
                    $scope.phone = $scope.information.Phone;
                    $scope.mobilePhone = $scope.information.MobilePhone;
                    $scope.fax = $scope.information.Fax;
                });
            } ]
        };
    };

    customerSettingInformationDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingInformation', customerSettingInformationDirective);
})();

'use strict';

(function () {
    var injectParams = [];
    var customerSettingMessageDirective = function () {
        return {
            restrict: 'A',
            replace: true,
            scope: {
                messages: '=customerSettingMessage'
            },
            templateUrl: 'templates/customer-setting-message.htm'
        };
    };

    customerSettingMessageDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingMessage', customerSettingMessageDirective);
})();

'use strict';

(function () {
    var injectParams = ['resourceService', 'controllerService', 'appSettings'];
    var customerSettingStatusDirective = function (resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=customerSettingStatus'
            },
            templateUrl: 'templates/customer-setting-status.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkStatus($scope);
                var requestContext = controllerService.getRequestContext();
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Status, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabledDownline;
                $scope.showDownline = requestContext.FormType !== appSettings.formType.AgentEditMember;

                $scope.enableDisabled = false;
                $scope.downlineDisabled = true;
                if ($scope.status.IsDisabledUpline === true) {
                    $scope.enableDisabled = true;
                    $scope.downlineDisabled = true;
                }

                if ($scope.enableDisabled === false) {
                    var oldEnable = $scope.enable;
                    $scope.$watch('enable', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            $scope.downlineDisabled = !($scope.enable && !oldEnable);
                            if (!$scope.enable) {
                                $scope.downline = false;
                            }
                            else {
                                $scope.downline = !$scope.status.IsDisabled && !$scope.status.IsDisabledDownline;
                            }
                        }
                    });
                }

                $scope.validate = function () {
                    var errCode = $scope.enable === $scope.status.IsDisabled || $scope.downline === $scope.status.IsDisabledDownline
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    return {
                        errCode: errCode,
                        data: {
                            SportType: $scope.status.SportType,
                            BetType: $scope.status.BetType,
                            IsDisabled: !$scope.enable,
                            IsDisabledDownline: !$scope.downline
                        }
                    };
                };
            } ]
        };
    };

    customerSettingStatusDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingStatus', customerSettingStatusDirective);
})();

'use strict';

(function () {
    var injectParams = ['tooltipService'];
    var customerSettingTooltipDirective = function (tooltipService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: 'templates/customer-setting-tooltip.htm',
            scope: {},
            link: function (scope, element) {
                tooltipService.setup(scope, element);
                scope.$watch('message', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        tooltipService.calculatePosition();
                    }
                });
            }
        };
    };

    customerSettingTooltipDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingTooltip', customerSettingTooltipDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditAgentPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditAgentPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/master-edit-agent-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM2;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterPT = generateMasterPT();
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Master PT
                function generateMasterPT() {
                    var min = !$scope.masterIsAuto ? Math.max(0, positionTaking.N3 - positionTaking.A4) : 0;
                    var max = positionTaking.M3;
                    var value = !isInitialized
                        ? !$scope.masterIsAuto ? positionTaking.M2 : positionTaking.UM2
                        : commonService.correctValue(min, max, $scope.masterPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterPT = generateMasterPT();
                $scope.$watch('masterPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.agentMaxPT = generateAgentMaxPT();
                    }
                });

                // Agent Max PT
                function generateAgentMaxPT() {
                    var min = Math.max(0, positionTaking.N3 - $scope.masterPT.value);
                    var max = !$scope.masterIsAuto
                        ? Math.min(positionTaking.M3 - $scope.masterPT.value, positionTaking.A4)
                        : Math.min(positionTaking.M3, positionTaking.A4);
                    var value = !isInitialized
                        ? positionTaking.A2
                        : commonService.correctValue(min, max, $scope.agentMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();
                $scope.$watch('agentMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.agentMinPT = generateAgentMinPT();
                    }
                });

                // Agent Min PT
                function generateAgentMinPT() {
                    var min = 0;
                    var max = $scope.agentMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N2
                        : commonService.correctValue(min, max, $scope.agentMinPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.agentMinPT = generateAgentMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    return commonService.copyValue(source, $scope, 'masterPT', generateMasterPT)
                        && commonService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && commonService.copyValue(source, $scope, 'agentMinPT', generateAgentMinPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.masterIsAuto ? positionTaking.M2 : positionTaking.UM2) === $scope.masterPT.value
                        && positionTaking.IM2 === $scope.masterIsAuto
                        && positionTaking.A2 === $scope.agentMaxPT.value
                        && positionTaking.N2 === $scope.agentMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            SportTypeName: positionTaking.SportTypeName,
                            BetType: positionTaking.BetType,
                            BetTypeName: positionTaking.BetTypeName,
                            IsLive: positionTaking.IsLive,
                            M2: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM2: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM2: $scope.masterIsAuto,
                            A2: $scope.agentMaxPT.value,
                            N2: $scope.agentMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    masterEditAgentPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditAgentPositionTaking', masterEditAgentPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=masterEditMemberDiscount'
            },
            templateUrl: 'templates/master-edit-member-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    masterEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberDiscount', masterEditMemberDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=masterEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/master-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Master Is Auto
                $scope.masterIsAuto = positionTaking.IM1;
                $scope.$watch('masterIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterPT = generateMasterPT();
                    }
                });

                // Master PT
                function generateMasterPT() {
                    var min = Math.max(0, positionTaking.N3 - positionTaking.A1);
                    var max = !$scope.masterIsAuto
                        ? positionTaking.M3 - positionTaking.A1
                        : positionTaking.M3;
                    var disabled = positionTaking.IM2 && !$scope.masterIsAuto;
                    var value = disabled
                        ? positionTaking.M1
                        : !isInitialized
                            ? !$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1
                            : commonService.correctValue(min, max, $scope.masterPT.value);
                    var select = commonService.createPositionTakingSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.masterPT = generateMasterPT();

                // Agent PT
                $scope.agentPT = positionTaking.A1 / appSettings.positionTakingDivider;

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    if (!$scope.masterPT.disabled || source.masterIsAuto) {
                        return commonService.copyValue(source, $scope, 'masterPT', generateMasterPT);
                    }

                    return true;
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.masterIsAuto ? positionTaking.M1 : positionTaking.UM1) === $scope.masterPT.value
                        && positionTaking.IM1 === $scope.masterIsAuto) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            SportTypeName: positionTaking.SportTypeName,
                            BetType: positionTaking.BetType,
                            BetTypeName: positionTaking.BetTypeName,
                            IsLive: positionTaking.IsLive,
                            M1: $scope.masterIsAuto ? 0 : $scope.masterPT.value,
                            UM1: $scope.masterIsAuto ? $scope.masterPT.value : 0,
                            IM1: $scope.masterIsAuto
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    masterEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberPositionTaking', masterEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['$parse', 'appSettings'];
    var positionTakingSelectOptionsDirective = function ($parse, appSettings) {
        var optionTemplate = angular.element(document.createElement('option'));
        return {
            restrict: 'A',
            require: ['select', '?ngModel'],
            link: function (scope, selectElement, attrs, ctrls) {
                //var selectCtrl = ctrls[0];
                var ngModelCtrl = ctrls[1];
                if (!ngModelCtrl) {
                    return;
                }

                if (angular.isDefined(attrs.ngOptions)) {
                    throw "Don't use ng-options in this directive";
                }

                scope.$watchCollection(attrs.positionTakingSelectOptions, function (newValues, oldValues) {
                    if (newValues !== oldValues) {
                        render();
                    }
                });

                var valueFn = $parse(attrs.positionTakingSelectOptions);
                var currentOptions;
                var currentValue;
                function render() {
                    var value = ngModelCtrl.$modelValue;
                    var options = valueFn(scope);
                    var hasChange = false;
                    if (currentValue !== value) {
                        hasChange = true;
                    } else if (angular.isUndefined(currentOptions) || currentOptions.length !== options.length) {
                        hasChange = true;
                    } else {
                        for (var i = 0; i < currentOptions.length; i++) {
                            if (currentOptions[i] !== options[i]) {
                                hasChange = true;
                                break;
                            }
                        }
                    }

                    if (!hasChange) {
                        //console.log('This check is useful');
                        return;
                    }

                    currentOptions = options;
                    currentValue = value;
                    selectElement.children().remove();
                    angular.forEach(currentOptions, function (option) {
                        var value = option;
                        var label = value / appSettings.positionTakingDivider;
                        var selected = currentValue === option;
                        var optionElement = optionTemplate.clone();
                        optionElement.val(value);
                        optionElement.prop('selected', selected);
                        optionElement.attr('selected', selected);
                        optionElement.text(label);
                        selectElement.append(optionElement);
                    });
                }

                ngModelCtrl.$render = render;
                ngModelCtrl.$parsers.unshift(function (viewValue) {
                    var value = parseInt(viewValue);
                    currentValue = value;
                    return value;
                });
            }
        };
    };

    positionTakingSelectOptionsDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelectOptions', positionTakingSelectOptionsDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var positionTakingSelectDirective = function ($rootScope) {
        return {
            restrict: 'AE',
            scope: {
                data: '=positionTakingSelect'
            },
            templateUrl: 'templates/position-taking-select.htm',
            link: function (scope, element) {
                element.on('click', function () {
                    var click = function () {
                        scope.data.valid = true;
                    };

                    if ($rootScope.$$phase) {
                        scope.$evalAsync(click);
                    } else {
                        scope.$apply(click);
                    }
                });
            }
        };
    };

    positionTakingSelectDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('positionTakingSelect', positionTakingSelectDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMasterPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMasterPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/super-edit-master-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Is Auto
                $scope.superIsAuto = positionTaking.IS3;
                $scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.superPT = generateSuperPT();
                        $scope.masterMaxPT = generateMasterMaxPT();
                    }
                });

                // Super PT
                function generateSuperPT() {
                    var min = !$scope.superIsAuto ? Math.max(0, positionTaking.N4 - positionTaking.M4) : 0;
                    var max = positionTaking.S4;
                    var value = !isInitialized
                        ? !$scope.superIsAuto ? positionTaking.S3 : positionTaking.US3
                        : commonService.correctValue(min, max, $scope.superPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.superPT = generateSuperPT();
                $scope.$watch('superPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterMaxPT = generateMasterMaxPT();
                    }
                });

                // Master Max PT
                function generateMasterMaxPT() {
                    var min = Math.max(0, positionTaking.N4 - $scope.superPT.value);
                    var max = !$scope.superIsAuto
                        ? Math.min(positionTaking.S4 - $scope.superPT.value, positionTaking.M4)
                        : Math.min(positionTaking.S4, positionTaking.M4);
                    var value = !isInitialized
                        ? positionTaking.M3
                        : commonService.correctValue(min, max, $scope.masterMaxPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.masterMinPT = generateMasterMinPT();
                    }
                });

                // Master Min PT
                function generateMasterMinPT() {
                    var min = 0;
                    var max = $scope.masterMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N3
                        : commonService.correctValue(min, max, $scope.masterMinPT.value);
                    return commonService.createPositionTakingSelect(min, max, value);
                }
                $scope.masterMinPT = generateMasterMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    return commonService.copyValue(source, $scope, 'superPT', generateSuperPT)
                        && commonService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && commonService.copyValue(source, $scope, 'masterMinPT', generateMasterMinPT);
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.superIsAuto ? positionTaking.S3 : positionTaking.US3) === $scope.superPT.value
                        && positionTaking.IS3 === $scope.superIsAuto
                        && positionTaking.M3 === $scope.masterMaxPT.value
                        && positionTaking.N3 === $scope.masterMinPT.value) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            SportTypeName: positionTaking.SportTypeName,
                            BetType: positionTaking.BetType,
                            BetTypeName: positionTaking.BetTypeName,
                            IsLive: positionTaking.IsLive,
                            S3: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US3: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS3: $scope.superIsAuto,
                            M3: $scope.masterMaxPT.value,
                            N3: $scope.masterMinPT.value
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    superEditMasterPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMasterPositionTaking', superEditMasterPositionTakingDirective);
})();

'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=superEditMemberDiscount'
            },
            templateUrl: 'templates/super-edit-member-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    superEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberDiscount', superEditMemberDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'commonService', 'resourceService', 'controllerService', 'appSettings'];
    var superEditMemberPositionTakingDirective = function ($rootScope, commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=superEditMemberPositionTaking',
                hasCopy: '='
            },
            templateUrl: 'templates/super-edit-member-position-taking.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkPositionTaking($scope);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.resourceService = resourceService;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.PositionTaking, positionTaking);

                // Super Is Auto
                $scope.superIsAuto = positionTaking.IS1;
                $scope.$watch('superIsAuto', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $scope.superPT = generateSuperPT();
                    }
                });

                // Super PT
                function generateSuperPT() {
                    var min = Math.max(0, positionTaking.N4 - positionTaking.M1 - positionTaking.A1);
                    var max = !$scope.superIsAuto
                        ? Math.max(positionTaking.N4 - positionTaking.M1 - positionTaking.A1, positionTaking.S4 - positionTaking.M3)
                        : positionTaking.S4;
                    var disabled = positionTaking.IS3 && !$scope.superIsAuto;
                    var value = disabled
                        ? positionTaking.S1
                        : !isInitialized
                            ? !$scope.superIsAuto ? positionTaking.S1 : positionTaking.US1
                            : commonService.correctValue(min, max, $scope.superPT.value);
                    var select = commonService.createPositionTakingSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.superPT = generateSuperPT();

                // Master PT
                $scope.masterPT = positionTaking.M1 / appSettings.positionTakingDivider;

                // Agent PT
                $scope.agentPT = positionTaking.A1 / appSettings.positionTakingDivider;

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    if (!$scope.superPT.disabled || source.superIsAuto) {
                        return commonService.copyValue(source, $scope, 'superPT', generateSuperPT);
                    }

                    return true;
                };

                // Validate
                $scope.validate = function () {
                    var errCode = appSettings.errorType.Success;
                    if ((!$scope.superIsAuto ? positionTaking.S1 : positionTaking.US1) === $scope.superPT.value
                        && positionTaking.IS1 === $scope.superIsAuto) {
                        errCode = appSettings.errorType.NoChange;
                    }

                    return {
                        errCode: errCode,
                        data: {
                            SportType: positionTaking.SportType,
                            SportTypeName: positionTaking.SportTypeName,
                            BetType: positionTaking.BetType,
                            BetTypeName: positionTaking.BetTypeName,
                            IsLive: positionTaking.IsLive,
                            S1: $scope.superIsAuto ? 0 : $scope.superPT.value,
                            US1: $scope.superIsAuto ? $scope.superPT.value : 0,
                            IS1: $scope.superIsAuto
                        }
                    };
                };

                isInitialized = true;
            } ]
        };
    };

    superEditMemberPositionTakingDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('superEditMemberPositionTaking', superEditMemberPositionTakingDirective);
})();

'use strict';

(function () {
    angular.module('customerSettingApp').filter('dataGroup', function () {
        function DataGroup(sportType, sportTypeName) {
            this.SportType = sportType;
            this.SportTypeName = sportTypeName;
            this.Items = [];
            this.addItem = function (item) {
                this.Items.push(item);
            };
        }

        return function (items) {
            var dataGroups = [];
            var currentSportType = -1;
            var dataGroup = null;
            angular.forEach(items, function (item) {
                var sportType = item.SportType;
                var sportTypeName = item.SportTypeName;
                if (dataGroup === null || sportType !== currentSportType) {
                    currentSportType = sportType;
                    dataGroup = new DataGroup(sportType, sportTypeName);
                    dataGroups.push(dataGroup);
                }
                dataGroup.addItem(item);
            });

            return dataGroups;
        };
    });
})();

'use strict';

(function () {
    var injectParams = ['$window'];
    var commonService = function ($window) {
        this.scrollToTop = function () {
            $window.scrollTo(0, 0);
        };

        this.inArray = function (value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        };

        var OptionsBuilder = function (step, factor) {
            this.generateOptions = function (min, max, value) {
                var options = [];
                min = Math.max(0, min);
                max = Math.max(0, max);
                while (max >= min) {
                    options.push(max);
                    max -= step;
                    max = Math.round(max * factor) / factor;
                }

                if (angular.isDefined(value)) {
                    var selectIndex = -1;
                    angular.forEach(options, function (option, index) {
                        if (selectIndex === -1 && option === value) {
                            selectIndex = index;
                        }
                    });

                    if (selectIndex === -1) {
                        if (value < min) {
                            options.push(value);
                            selectIndex = options.length - 1;
                        } else if (value > max) {
                            options.unshift(value);
                            selectIndex = 0;
                        }
                    }
                }

                return options;
            };
        };

        var SelectBuilder = function (step, factor) {
            var optionsBuilder = new OptionsBuilder(step, factor);
            this.build = function (min, max, value) {
                var options = optionsBuilder.generateOptions(min, max, value);
                return {
                    valid: true,
                    disabled: false,
                    value: value,
                    options: options
                };
            };
        };

        this.correctValue = function (min, max, value) {
            return Math.max(min, Math.min(max, value));
        };

        this.copyValue = function (source, destination, field, generator) {
            destination[field].value = source[field].value;
            destination[field] = generator();
            var isCopied = source[field].value === destination[field].value;
            destination[field].valid = isCopied;
            return isCopied;
        };

        this.createPositionTakingSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(1, 1);
            return selectBuilder.build(min, max, value);
        };

        this.createDiscountSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(0.0005, 10000);
            return selectBuilder.build(min, max, value);
        };
    };

    commonService.$inject = injectParams;
    angular.module('customerSettingApp').service('commonService', commonService);
})();

'use strict';

(function () {
    var injectParams = ['appSettings', 'resourceService'];
    var controllerService = function (appSettings, resourceService) {
        var statusScopes = [];
        var discountScopes = [];
        var betLimitScopes = [];
        var positionTakingScopes = [];
        this.linkStatus = function (scope) {
            statusScopes.push(scope);
        };
        this.linkDiscount = function (scope) {
            discountScopes.push(scope);
        };
        this.linkBetLimit = function (scope) {
            betLimitScopes.push(scope);
        };
        this.linkPositionTaking = function (scope) {
            positionTakingScopes.push(scope);
        };

        this.getScopes = function (settingType) {
            switch (settingType) {
                case appSettings.settingType.Status:
                    return statusScopes;

                case appSettings.settingType.Discount:
                    return discountScopes;

                case appSettings.settingType.BetLimit:
                    return betLimitScopes;

                case appSettings.settingType.PositionTaking:
                    return positionTakingScopes;
            }

            return undefined;
        };

        this.getUpdateData = function (settingType) {
            var scopes = this.getScopes(settingType);
            var data = [];
            var isFormInvalid = false;
            angular.forEach(scopes, function (scope) {
                if (!isFormInvalid) {
                    if (angular.isFunction(scope.validate)) {
                        var result = scope.validate();
                        if (result.errCode === appSettings.errorType.Success) {
                            data.push(result.data);
                        } else if (result.errCode === appSettings.errorType.FormInvalid) {
                            isFormInvalid = true;
                        }
                    }
                }
            });

            return isFormInvalid
                ? undefined
                : data;
        };

        this.getDisplayName = function (settingType, data) {
            var liveName = data.IsLive === true ? ' (' + resourceService.getString('Live') + ')' : '';
            var settingLevel;
            switch (settingType) {
                case appSettings.settingType.PositionTaking:
                    settingLevel = settingContext.PositionTakingLevel;
                    break;

                case appSettings.settingType.BetLimit:
                    settingLevel = settingContext.BetLimitLevel;
                    break;

                case appSettings.settingType.Discount:
                    settingLevel = settingContext.DiscountLevel;
                    break;

                case appSettings.settingType.Status:
                    settingLevel = settingContext.StatusLevel;
                    break;
            }

            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return requestContext.ProductName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }

            return undefined;
        };

        function RequestContext() {
            this.TargetCustId = 0;
            this.FormType = undefined;
            this.ProductId = 0;
            this.ProductName = undefined;
            this.Language = undefined;
        }
        function SettingContext() {
            this.PositionTakingLevel = undefined;
            this.BetLimitLevel = undefined;
            this.DiscountLevel = undefined;
            this.StatusLevel = undefined;
            this.DisableAutoMPT = undefined;
        }
        function CustomerSetting() {
            this.Information = undefined;
            this.Statuses = undefined;
            this.Discounts = undefined;
            this.BetLimits = undefined;
            this.PositionTakings = undefined;
        }

        var requestContext = new RequestContext();
        var settingContext = new SettingContext();
        var customerSetting = new CustomerSetting();
        this.updateResponse = function (response) {
            angular.copy(response.RequestContext, requestContext);
            angular.copy(response.CustomerSetting, customerSetting);
            angular.copy(response.CustomerSetting.SettingContext, settingContext);
        };

        this.getRequestContext = function () {
            return requestContext;
        };
        this.getSettingContext = function () {
            return settingContext;
        };
        this.getCustomerSetting = function () {
            return customerSetting;
        };
    };

    controllerService.$inject = injectParams;
    angular.module('customerSettingApp').service('controllerService', controllerService);
})();

'use strict';

(function () {
    var injectParams = ['$http', '$rootScope', 'messageService', 'appSettings', 'ResponseReader'];
    var dataService = function ($http, $rootScope, messageService, appSettings, ResponseReader) {
        function isSuccessResponse(response) {
            if (!response.IsSuccess) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        this.select = function (selectModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Select';
            var request = $http.post(url, selectModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };

        this.update = function (updateModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Update';
            var request = $http.post(url, updateModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                var responseReader = new ResponseReader(updateModel.SettingType);
                var settingResults = responseReader.read(response);
                var results = [];
                angular.forEach(settingResults, function (settingResult) {
                    results.push(settingResult.result);
                });
                messageService.showResults(results);

                if (angular.isFunction(successCallback)) {
                    successCallback(response);
                }
            });

            return request;
        };
    };

    dataService.$inject = injectParams;
    angular.module('customerSettingApp').service('dataService', dataService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$timeout', 'commonService', '$sce', 'appSettings', 'resourceService'];
    var messageService = function ($rootScope, $timeout, commonService, $sce, appSettings, resourceService) {
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var Message = function (messageType, messageContent) {
            this.MessageType = messageType;
            this.MessageContent = messageContent;
        };

        function generateMesage(result) {
            var messageType = appSettings.messageType.Error;
            switch (result.ErrCode) {
                case appSettings.errorCode.Success:
                    messageType = appSettings.messageType.Success;
                    break;

                case appSettings.errorCode.NoChange:
                    messageType = appSettings.messageType.Warning;
                    break;
            }

            var messageContent = result.ErrMsg || 'Undefined messsage';
            return new Message(messageType, messageContent);
        }

        function showMessage(message) {
            var htmlMessages = [];
            htmlMessages.push({
                messageClass: messageClasses[message.MessageType],
                messageContent: $sce.trustAsHtml(message.MessageContent)
            });
            commonService.scrollToTop();

            $timeout(function () {
                $rootScope.messages = htmlMessages;
            });
        }

        function showMessages(messages) {
            var htmlMessages = [];
            angular.forEach(messages, function (message) {
                htmlMessages.push({
                    messageClass: messageClasses[message.MessageType],
                    messageContent: $sce.trustAsHtml(message.MessageContent)
                });
            });
            commonService.scrollToTop();

            $timeout(function () {
                $rootScope.messages = htmlMessages;
            });
        }

        var self = this;
        this.showMessage = function (messageType, messageContent) {
            var message = new Message(messageType, messageContent);
            showMessage(message);
        };

        this.showResult = function (result) {
            var message = generateMesage(result);
            showMessage(message);
        };

        this.showResults = function (results) {
            // if having any successful or error messages -> remove any no-change messages
            var messages = [];
            angular.forEach(results, function (result) {
                if (result.ErrCode !== appSettings.errorCode.NoChange) {
                    var message = generateMesage(result);
                    messages.push(message);
                }
            });

            if (messages.length === 0) {
                self.showNoChange();
            } else {
                showMessages(messages);
            }
        };

        this.showSuccess = function (messageContent) {
            this.showMessage(appSettings.messageType.Success, messageContent || 'Undefined success message');
        };

        this.showWarning = function (messageContent) {
            this.showMessage(appSettings.messageType.Warning, messageContent || 'Undefined warning message');
        };

        this.showNoChange = function () {
            this.showWarning(resourceService.getString('NoChangeToUpdate'));
        };

        this.showError = function (messageContent) {
            this.showMessage(appSettings.messageType.Error, messageContent || 'Undefined error message');
        };

        this.hide = function () {
            $rootScope.messages = undefined;
        };
    };

    messageService.$inject = injectParams;
    angular.module('customerSettingApp').service('messageService', messageService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$location', 'appSettings'];
    var modelService = function ($rootScope, $location, appSettings) {
        var formSettings = [
            {
                formType: appSettings.formType.CompanyEditSuper,
                pattern: '/editSuper/:superId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMaster,
                pattern: '/editMaster/:masterId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditAgent,
                pattern: '/editAgent/:agentId/:productId'
            },
            {
                formType: appSettings.formType.AgentEditMember,
                pattern: '/editMember/:memberId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMember,
                pattern: '/superEditMember/:superId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditMember,
                pattern: '/masterEditMember/:masterId/:memberId/:productId'
            }
        ];

        var self = this;
        function loadParams() {
            var isValidModel = false;
            var modelParams = {};
            var path = $location.path();
            angular.forEach(formSettings, function (formSetting) {
                if (isValidModel) {
                    return;
                }

                var formType = formSetting.formType;
                var pattern = formSetting.pattern;
                var matches = pattern.match(/\/:\w+/g);
                var params = [];
                angular.forEach(matches, function (match) {
                    params.push(match.replace('/:', ''));
                });

                var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
                matches = regExp.exec(path);
                if (matches !== null && matches.length - 1 === params.length) {
                    isValidModel = true;
                    angular.forEach(params, function (param, index) {
                        modelParams[param] = parseInt(matches[index + 1]);
                    });
                    modelParams.formType = formType;
                }
            });

            self.isValid = isValidModel;
            self.params = modelParams;
        }

        this.createSelectModel = function () {
            var isValid = this.isValid;
            var params = this.params;
            if (!isValid) {
                return undefined;
            }

            var custId;
            var roleId;
            var targetCustId;
            var targetRoleId;
            switch (params.formType) {
                case appSettings.formType.CompanyEditSuper:
                    targetCustId = params.superId;
                    targetRoleId = appSettings.roleType.Super;
                    break;

                case appSettings.formType.SuperEditMaster:
                    targetCustId = params.masterId;
                    targetRoleId = appSettings.roleType.Master;
                    break;

                case appSettings.formType.MasterEditAgent:
                    targetCustId = params.agentId;
                    targetRoleId = appSettings.roleType.Agent;
                    break;

                case appSettings.formType.SuperEditMember:
                    custId = params.superId;
                    roleId = appSettings.roleType.Super;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.MasterEditMember:
                    custId = params.masterId;
                    roleId = appSettings.roleType.Master;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.AgentEditMember:
                    custId = params.agentId;
                    roleId = appSettings.roleType.Agent;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;
            }

            return {
                CustId: custId,
                RoleId: roleId,
                TargetCustId: targetCustId,
                TargetRoleId: targetRoleId,
                FormType: params.formType,
                ProductId: params.productId
            };
        };

        $rootScope.$on('$locationChangeStart', function () {
            loadParams();
        });
        
        loadParams();
    };

    modelService.$inject = injectParams;
    angular.module('customerSettingApp').service('modelService', modelService);
})();

'use strict';

(function () {
    var injectParams = ['resources', 'appSettings'];
    var resourceService = function (resources, appSettings) {
        var resource = {};

        this.setLanguage = function (language) {
            if (angular.isUndefined(language)) {
                language = appSettings.defaultLanguage.toLowerCase();
            } else {
                language = language.toLowerCase();
            }

            var selectResource = resources[language];
            if (angular.isUndefined(selectResource)) {
                selectResource = resources[appSettings.defaultLanguage.toLowerCase()];
            }
            resource = selectResource;
        };

        this.getString = function (key) {
            var value = resource[key];
            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                value = key;
            }
            return value;
        };

        this.getFormatString = function (key) {
            var input = this.getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        };
    };

    resourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('resourceService', resourceService);
})();

'use strict';

(function () {
    var injectParams = ['appSettings'];
    var responseReaderFactory = function (appSettings) {
        var SettingResult = function (settingType, result) {
            this.settingType = settingType;
            this.result = result;
        };

        var ResponseReader = function (settingType) {
            this.settingType = settingType;
            this.isSuccess = appSettings.errorCode.Success;
            this.settingResults = [];
        };

        ResponseReader.prototype.read = function (response) {
            this.isSuccess = response.IsSuccess;
            var settingResults = [];
            switch (this.settingType) {
                case appSettings.settingType.All:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, response.StatusResult));
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, response.DiscountResult));
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, response.BetLimitResult));
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, response.PositionTakingResult));
                    break;

                case appSettings.settingType.Status:
                    settingResults.push(new SettingResult(appSettings.settingType.Status, response.StatusResult));
                    break;

                case appSettings.settingType.Discount:
                    settingResults.push(new SettingResult(appSettings.settingType.Discount, response.DiscountResult));
                    break;

                case appSettings.settingType.BetLimit:
                    settingResults.push(new SettingResult(appSettings.settingType.BetLimit, response.BetLimitResult));
                    break;

                case appSettings.settingType.PositionTaking:
                    settingResults.push(new SettingResult(appSettings.settingType.PositionTaking, response.PositionTakingResult));
                    break;
            }

            return settingResults;
        };

        return ResponseReader;
    };

    responseReaderFactory.$inject = injectParams;
    angular.module('customerSettingApp').factory('ResponseReader', responseReaderFactory);
})();

'use strict';

(function () {
    var injectParams = ['appSettings', 'controllerService'];
    var templateService = function (appSettings, controllerService) {
        var levelFolders = {};
        levelFolders[appSettings.settingLevel.Product] = 'no-group';
        levelFolders[appSettings.settingLevel.SportType] = 'no-group';
        levelFolders[appSettings.settingLevel.BetType] = 'no-group';
        levelFolders[appSettings.settingLevel.SportTypeBetType] = 'sport-group';

        var positionTakingFiles = {};
        positionTakingFiles[appSettings.formType.CompanyEditSuper] = 'company-edit-super-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMaster] = 'super-edit-master-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditAgent] = 'master-edit-agent-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-position-taking-container.htm';
        positionTakingFiles[appSettings.formType.AgentEditMember] = 'agent-edit-member-position-taking-container.htm';

        var discountFiles = {};
        discountFiles[appSettings.formType.CompanyEditSuper] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMaster] = 'discount-container.htm';
        discountFiles[appSettings.formType.MasterEditAgent] = 'discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMember] = 'super-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.MasterEditMember] = 'master-edit-member-discount-container.htm';
        discountFiles[appSettings.formType.AgentEditMember] = 'discount-container.htm';

        this.resolveView = function () {
            return 'templates/customer-setting-view.htm';
        };

        this.resolveStatus = function () {
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.StatusLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function () {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.DiscountLevel];
            var fileName = discountFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };

        this.resolveBetLimit = function () {
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.BetLimitLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/betlimit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function () {
            var requestContext = controllerService.getRequestContext();
            var settingContext = controllerService.getSettingContext();
            var levelFolder = levelFolders[settingContext.PositionTakingLevel];
            var fileName = positionTakingFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    templateService.$inject = injectParams;
    angular.module('customerSettingApp').service('templateService', templateService);
})();

'use strict';

(function () {
    var injectParams = ['$timeout'];
    var tooltipService = function ($timeout) {
        var tooltipScope;
        var tooltipElement;
        var tooltipAlign;
        var boundingElement;

        this.calculatePosition = function () {
            var rect = boundingElement.getBoundingClientRect();
            var tooltipRect = tooltipElement[0].getBoundingClientRect();
            var scrollX = window.pageXOffset || document.body.scrollLeft;
            var scrollY = window.pageYOffset || document.body.scrollTop;
            var left = 0;
            if (tooltipAlign === 'middle') {
                left = Math.max(0, rect.left + ((rect.right - rect.left) - (tooltipRect.right - tooltipRect.left)) / 2 + scrollX);
            } else {
                left = Math.max(0, rect.left + scrollX);
            }

            var top = Math.max(0, rect.top - (tooltipRect.bottom - tooltipRect.top) + scrollY);
            tooltipElement.css({
                'top': top + 'px',
                'left': left + 'px'
            });
        };

        this.setup = function (scope, element) {
            if (angular.isDefined(tooltipScope)) {
                throw 'Only one tooltip component in document';
            }

            tooltipScope = scope;
            tooltipElement = element;
            tooltipElement.addClass('ng-hide');
        };

        this.show = function (element, message, align) {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (angular.isUndefined(align)) {
                align = 'middle';
            }

            tooltipAlign = align;
            tooltipScope.align = tooltipAlign;
            boundingElement = element[0];
            if (tooltipScope.message !== message) {
                $timeout(function () {
                    tooltipScope.message = message;
                });
            }

            if (tooltipElement.hasClass('ng-hide')) {
                tooltipElement.removeClass('ng-hide');
                this.calculatePosition();
            }
        };

        this.hide = function () {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (!tooltipElement.hasClass('ng-hide')) {
                tooltipElement.addClass('ng-hide');
            }
        };
    };

    tooltipService.$inject = injectParams;
    angular.module('customerSettingApp').service('tooltipService', tooltipService);
})();

'use strict';
/*jshint -W101,-W106,-W044,-W098*/

(function () {
    var resources = {};
    
    resources['en-us'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "Agent Max PT",
        AgentMinPT: "Agent Min PT",
        AgentPT: "Agent PT",
        Agents: "Agents",
        BetSetting: "Bet Setting",
        Commission: "Commission",
        CommissionForAgent: "Agent Comm",
        CommissionForMaster: "Master Comm",
        CommissionForMember: "Member Comm",
        CopyTooltip: "Click to duplicate the values",
        Enable: "Enable",
        EnableDownline: "Enable Downline",
        Fax: "Fax",
        FirstName: "First Name",
        GeneralInformation: "General Information",
        IsAuto: "Is Auto",
        LastName: "Last Name",
        Live: "Live",
        MasterMaxPT: "Master Max PT",
        MasterMinPT: "Master Min PT",
        MasterPT: "Master PT",
        Masters: "Masters",
        Max: "Max",
        MaxBet: "Max Bet",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "Max Per Match",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "Members",
        Min: "Min",
        MinBet: "Min Bet",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "Mobile Phone",
        Nickname: "Nickname",
        NoChangeToUpdate: "There is no change to update!",
        Phone: "Phone",
        PositionTaking: "Position Taking",
        ProductStatus: "Product Status",
        SuperMaxPT: "Super Max PT",
        SuperMinPT: "Super Min PT",
        SuperPT: "Super PT",
        Update: "Update",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "You have changed your position taking",
        UpdateMemberPtQuestion: "Do you want to update all member&apos;s position taking to:",
        UpdateMemberPtTitle: "Confirm Update Members&apos;s PT",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "UserName"
    };
    
    resources['zh-cn'] = {
        AccessDeniedMsg: "抱歉!您没有此页面的权限",
        AgentMaxPT: "代理最高占成",
        AgentMinPT: "代理最低占成数",
        AgentPT: "代理占成数",
        Agents: "代理",
        BetSetting: "投注设定",
        Commission: "佣金",
        CommissionForAgent: "代理佣金",
        CommissionForMaster: "总代理佣金",
        CommissionForMember: "会员佣金",
        CopyTooltip: "Click to duplicate the values",
        Enable: "启用",
        EnableDownline: "启用下线",
        Fax: "传真",
        FirstName: "名",
        GeneralInformation: "基本信息",
        IsAuto: "是自动",
        LastName: "姓",
        Live: "现场",
        MasterMaxPT: "总代理最高占成",
        MasterMinPT: "总代理最低占成数",
        MasterPT: "总代理占成数",
        Masters: "会员",
        Max: "最高",
        MaxBet: "最高投注",
        MaxBetLessThanOrEqualMaxPerMatch: "最大投注额必须小于或等于最大单场比赛投注额",
        MaxBetLessThanOrEqualTo: "最大投注额必须小于或等于 {0}",
        MaxBetMoreThanOrEqualMinBet: "最大投注额必须大于或等于最小投注额",
        MaxLive: "Max Live",
        MaxPerMatch: "单场限额",
        MaxPerMatchLessThanOrEqualTo: "最大单场比赛投注额必须小于或等于 {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "最大单场比赛投注额必须大于或等于最大投注额",
        Members: "会员",
        Min: "最低",
        MinBet: "最低投注",
        MinBetLessThanOrEqualMaxBet: "最小投注额必须小于或等于最大投注额",
        MinBetMoreThanOrEqualTo: "最大投注额必须大于或等于 {0}",
        MinLive: "Min Live",
        MobilePhone: "手机",
        Nickname: "昵称",
        NoChangeToUpdate: "没有更新！",
        Phone: "电话",
        PositionTaking: "占成数",
        ProductStatus: "Product Status",
        SuperMaxPT: "超级总代理最高占成",
        SuperMinPT: "超级总代理最低占成",
        SuperPT: "超级总代理占成数",
        Update: "更改",
        UpdateBetSettingSuccessfully: "成功更新投注设置。",
        UpdateCommissionSuccessfully: "成功更新佣金设置",
        UpdateMemberPtInfo: "您已更改您的占成数",
        UpdateMemberPtQuestion: "您是否要把所有会员的占成数更改为:",
        UpdateMemberPtTitle: "确认更新会员的占成数",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "用户名"
    };
    
    resources['zh-tw'] = {
        AccessDeniedMsg: "抱歉!您沒有此頁面的權限",
        AgentMaxPT: "代理最高佔成",
        AgentMinPT: "代理最低占成数",
        AgentPT: "代理占成数",
        Agents: "代理",
        BetSetting: "投注設定",
        Commission: "佣金",
        CommissionForAgent: "代理佣金",
        CommissionForMaster: "總代理佣金",
        CommissionForMember: "會員佣金",
        CopyTooltip: "Click to duplicate the values",
        Enable: "啟用",
        EnableDownline: "啟用下線",
        Fax: "傳真",
        FirstName: "名",
        GeneralInformation: "基本信息",
        IsAuto: "是自動",
        LastName: "姓",
        Live: "現場",
        MasterMaxPT: "總代理最高佔成",
        MasterMinPT: "总代理最低占成数",
        MasterPT: "总代理占成数",
        Masters: "會員",
        Max: "最高",
        MaxBet: "最高投注",
        MaxBetLessThanOrEqualMaxPerMatch: "最大投注額必須小於或等於最大單場比賽投注額",
        MaxBetLessThanOrEqualTo: "最大投注額必須小於或等於 {0}",
        MaxBetMoreThanOrEqualMinBet: "最大投注額必須大於或等於最小投注額",
        MaxLive: "Max Live",
        MaxPerMatch: "單場限額",
        MaxPerMatchLessThanOrEqualTo: "最大單場比賽投注額必須小於或等於 {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "最大單場比賽投注額必須大於或等於最大投注額",
        Members: "會員",
        Min: "最低",
        MinBet: "最低投注",
        MinBetLessThanOrEqualMaxBet: "最小投注額必須小於或等於最大投注額",
        MinBetMoreThanOrEqualTo: "最大投注額必須大於或等於 {0}",
        MinLive: "Min Live",
        MobilePhone: "手機",
        Nickname: "暱稱",
        NoChangeToUpdate: "沒有更新!",
        Phone: "電話",
        PositionTaking: "佔成數",
        ProductStatus: "Product Status",
        SuperMaxPT: "超級總代理最高佔成",
        SuperMinPT: "超級總代理最低佔成",
        SuperPT: "超级总代理占成数",
        Update: "更改",
        UpdateBetSettingSuccessfully: "成功更新投注設置。",
        UpdateCommissionSuccessfully: "成功更新傭金設置",
        UpdateMemberPtInfo: "您已更改您的占成数",
        UpdateMemberPtQuestion: "您是否要把所有會員佔成數更改為:",
        UpdateMemberPtTitle: "確認更新會員的佔成數",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "用戶名"
    };
    
    resources['ko-kr'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "에이전트 최대 PT",
        AgentMinPT: "에이전트 최소 PT",
        AgentPT: "에이전트 PT",
        Agents: "에이전트",
        BetSetting: "배팅 설정",
        Commission: "수수료",
        CommissionForAgent: "에이전트 수수료",
        CommissionForMaster: "마스터 수수료",
        CommissionForMember: "회원 수수료",
        CopyTooltip: "Click to duplicate the values",
        Enable: "활성화",
        EnableDownline: "다운라인 활성화하기",
        Fax: "팩스",
        FirstName: "이름",
        GeneralInformation: "일반 정보",
        IsAuto: "자동",
        LastName: "성",
        Live: "라이브",
        MasterMaxPT: "마스터 최대 PT",
        MasterMinPT: "마스터 최소PT",
        MasterPT: "마스터 PT",
        Masters: "마스터",
        Max: "최대",
        MaxBet: "최대 배팅",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "경기당 최대 값",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "회원",
        Min: "최소",
        MinBet: "최소 배팅",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "핸드폰",
        Nickname: "닉네임",
        NoChangeToUpdate: "There is no change to update!",
        Phone: "전화",
        PositionTaking: "거래 조건",
        ProductStatus: "Product Status",
        SuperMaxPT: "슈퍼 최대 PT",
        SuperMinPT: "슈퍼 최소PT",
        SuperPT: "슈퍼 PT",
        Update: "업데이트",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "포지션테이킹(PT)이 변경되었습니다",
        UpdateMemberPtQuestion: "모든회원의 거래조건을 바꾸기를 원하십니까:",
        UpdateMemberPtTitle: "Confirm Update Members&apos;s PT",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "사용자 이름"
    };
    
    resources['ja-jp'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "Agent Max PT",
        AgentMinPT: "Agent Min PT",
        AgentPT: "Agent PT",
        Agents: "Agents",
        BetSetting: "Bet Setting",
        Commission: "Commission",
        CommissionForAgent: "Agent Comm",
        CommissionForMaster: "Master Comm",
        CommissionForMember: "Member Comm",
        CopyTooltip: "Click to duplicate the values",
        Enable: "Enable",
        EnableDownline: "Enable Downline",
        Fax: "ファックス",
        FirstName: "名",
        GeneralInformation: "General Information",
        IsAuto: "Is Auto",
        LastName: "氏",
        Live: "ライブ",
        MasterMaxPT: "Master Max PT",
        MasterMinPT: "Master Min PT",
        MasterPT: "Master PT",
        Masters: "Masters",
        Max: "Max",
        MaxBet: "最大ベット",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "1 試合毎の最大",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "Members",
        Min: "Min",
        MinBet: "Min Bet",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "携帯電話番号",
        Nickname: "ニックネーム",
        NoChangeToUpdate: "There is no change to update!",
        Phone: "電話番号",
        PositionTaking: "Position Taking",
        ProductStatus: "Product Status",
        SuperMaxPT: "Super Max PT",
        SuperMinPT: "Super Min PT",
        SuperPT: "Super PT",
        Update: "更新",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "You have changed your position taking",
        UpdateMemberPtQuestion: "Do you want to update all member&apos;s position taking to:",
        UpdateMemberPtTitle: "Confirm Update Members&apos;s PT",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "ユーザー名"
    };
    
    resources['th-th'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "แบ่งสู้สูงสุดของเอ",
        AgentMinPT: "ค่าแบ่งสู้ต่ำสุดของ",
        AgentPT: "ค่าแบ่งสู้ของเอเย่น",
        Agents: "ตัวแทน",
        BetSetting: "การกำหนดการวางเดิมพัน",
        Commission: "คอมมิชชั่น",
        CommissionForAgent: "คอมมิชชั่นของ Agent",
        CommissionForMaster: "คอมมิชชั่นของ Master",
        CommissionForMember: "คอมมิชชั่นของสมาชิก",
        CopyTooltip: "Click to duplicate the values",
        Enable: "เปิดใช้งาน",
        EnableDownline: "เปิดใช้งาน ดาวไลน์",
        Fax: "แฟกซ์",
        FirstName: "ชื่อ",
        GeneralInformation: "ข้อมูลทั่วไป",
        IsAuto: "คืออัตโนมัติ",
        LastName: "นามสกุล",
        Live: "สด",
        MasterMaxPT: "แบ่งสู้สูงสุดของ",
        MasterMinPT: "แบ่งสู้ขั้นต่ำของมาสเตอร์",
        MasterPT: "ค่าแบ่งสู้ของมาสเตอร์",
        Masters: "ตัวแทนหลัก",
        Max: "สูงสุด",
        MaxBet: "ค่าเดิมพันสูงสุด",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "ค่าเดิมพันสูงสุดต่อคู่การแข่งขัน",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "สมาชิก",
        Min: "ต่ำสุด",
        MinBet: "ค่าเดิมพันต้ำสุด",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "โทรศัพท์มือถือ",
        Nickname: "ชื่อเล่น",
        NoChangeToUpdate: "There is no change to update!",
        Phone: "โทรศัพท์",
        PositionTaking: "ค่าความเสี่ยง",
        ProductStatus: "Product Status",
        SuperMaxPT: "แบ่งสู้สูงสุดของซุปเปรอ์",
        SuperMinPT: "แบ่งสู้ต่ำสุดของซุปเปรอ์",
        SuperPT: "ค่าแบ่งสู้ของซุปเปอร์",
        Update: "ล่าสุด",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "คุณมีการเปลี่ยนค่าแบ่งสู้ของคุณ",
        UpdateMemberPtQuestion: "คุณต้องการจะอัพเดทแบ่งสู้บนสมาชิกทั้งหมกหรือไม่:",
        UpdateMemberPtTitle: "ยืนยันการอัพเดทการตั้งค่าความเสี่ยง",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "ชื่อผู้ใช้ระบบ"
    };
    
    resources['vi-vn'] = {
        AccessDeniedMsg: "You do not have permission to access this page.",
        AgentMaxPT: "PT lớn nhất của Agent",
        AgentMinPT: "PT nhỏ nhất của Agent",
        AgentPT: "PT của Agent",
        Agents: "Agents",
        BetSetting: "Tiền Cược",
        Commission: "Hoa hồng",
        CommissionForAgent: "Agent Comm",
        CommissionForMaster: "Master Comm",
        CommissionForMember: "Member Comm",
        CopyTooltip: "Click to duplicate the values",
        Enable: "Cho phép",
        EnableDownline: "Enable Downline",
        Fax: "Fax",
        FirstName: "Tên",
        GeneralInformation: "Thông tin Chung",
        IsAuto: "Auto PT",
        LastName: "Họ",
        Live: "Trực Tuyến",
        MasterMaxPT: "PT lớn nhất của Master",
        MasterMinPT: "PT nhỏ nhất của Master",
        MasterPT: "PT của Master",
        Masters: "Masters",
        Max: "Max",
        MaxBet: "Cược lớn nhất",
        MaxBetLessThanOrEqualMaxPerMatch: "Max Bet must be less than or equal to Max Per Match",
        MaxBetLessThanOrEqualTo: "Max Bet must be less than or equal to {0}",
        MaxBetMoreThanOrEqualMinBet: "Max Bet must be greater than or equal to Min Bet",
        MaxLive: "Max Live",
        MaxPerMatch: "Cược tối đa cho 1 trận",
        MaxPerMatchLessThanOrEqualTo: "Max Per Match must be less than or equal to {0}",
        MaxPerMatchMoreThanOrEqualMaxBet: "Max Per Match must be greater than or equal to Max Bet",
        Members: "Members",
        Min: "Nhỏ nhất",
        MinBet: "Cược nhỏ nhất",
        MinBetLessThanOrEqualMaxBet: "Min Bet must be less than or equal to Max Bet",
        MinBetMoreThanOrEqualTo: "Min Bet must be greater than or equal to {0}",
        MinLive: "Min Live",
        MobilePhone: "Điện thoại di động",
        Nickname: "Biệt danh",
        NoChangeToUpdate: "Không có gi thay đổi để cập nhật!",
        Phone: "Điện thoại",
        PositionTaking: "PT",
        ProductStatus: "Product Status",
        SuperMaxPT: "PT lớn nhất của Super",
        SuperMinPT: "PT nhỏ nhất của Super",
        SuperPT: "PT của Super",
        Update: "Cập nhật",
        UpdateBetSettingSuccessfully: "Updated bet setting successfully.",
        UpdateCommissionSuccessfully: "Updated commission setting successfully.",
        UpdateMemberPtInfo: "Bạn đã  thay đổi giá trị PT của mình",
        UpdateMemberPtQuestion: "Bạn có muốn cập nhật giá trị PTcủa tất cả các thành viên:",
        UpdateMemberPtTitle: "Confirm Update Members&apos;s PT",
        UpdatePositionTakingSuccessfully: "Updated position taking successfully.",
        UpdateProductStatusSuccessfully: "Updated product status successfully.",
        UserName: "Tài khoản"
    };

    angular.module('customerSettingApp').value('resources', resources);
})();


'use strict';

(function () {
    var body = angular.element(document.body);
    var rootUrl = body.attr("data-root-url");
    angular.module('customerSettingApp').value('appSettings', {
        rootUrl: rootUrl,
        defaultLanguage: 'en-US',
        defaultMaxBet: 99999999999,
        betLimitMaxLength: 14,
        positionTakingDivider: 100,
        formType: {
            CompanyEditSuper: 1,
            SuperEditMaster: 2,
            MasterEditAgent: 3,
            AgentEditMember: 4,
            SuperEditMember: 5,
            MasterEditMember: 6
        },
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4,
            Company: 5
        },
        settingType: {
            All: 0,
            PositionTaking: 1,
            BetLimit: 2,
            Discount: 3,
            Status: 4
        },
        settingLevel: {
            Product: 1,
            SportType: 2,
            BetType: 3,
            SportTypeBetType: 4
        },
        errorType: {
            Success: 0,
            FormInvalid: 1,
            NoChange: 2
        },
        errorCode: {
            Success: 0,
            ErrorValue: -1,
            NoChange: -2,
            AccessDenied: -3
        },
        messageType: {
            Success: 0,
            Warning: 1,
            Error: 2
        }
    });
})();