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