/*jshint -W101*/

angular.module('customerSettingApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-bet-limit-readonly.htm',
    "<div class=bet-limit-item><div class=name title={{name}}>{{name}}</div><div class=\"min-bet read-only\" data-ng-bind=minBet></div><div class=\"max-bet read-only\" data-ng-bind=maxBet></div><div class=\"max-per-match read-only\" data-ng-bind=maxPerMatch></div></div>"
  );


  $templateCache.put('templates/fanex-bet-limit.htm',
    "<div class=bet-limit-item><div class=name title={{name}}>{{name}}<div data-fanex-item-copy=hasCopy data-setting-type=settingType></div></div><div class=min-bet><input data-ng-model=minBet.value data-fanex-bet-limit-input data-error-message=minBet.errorMessage() data-ng-class=\"{'full-width': !hasMinBetLimit, 'invalid': !minBet.isValid()}\"> <span data-ng-show=hasMinBetLimit>&gt;&#61;{{minBet.limit | number}}</span></div><div class=max-bet><input data-ng-model=maxBet.value data-fanex-bet-limit-input data-error-message=maxBet.errorMessage() data-ng-class=\"{'full-width': !hasMaxBetLimit, 'invalid': !maxBet.isValid()}\"> <span data-ng-show=hasMaxBetLimit>&lt;&#61;{{maxBet.limit | number}}</span></div><div class=max-per-match><input data-ng-model=maxPerMatch.value data-fanex-bet-limit-input data-error-message=maxPerMatch.errorMessage() data-ng-class=\"{'full-width': !hasMaxPerMatchLimit, 'invalid': !maxPerMatch.isValid()}\"> <span data-ng-show=hasMaxPerMatchLimit>&lt;&#61;{{maxPerMatch.limit | number}}</span></div></div>"
  );


  $templateCache.put('templates/fanex-block-ui.htm',
    "<div class=loading-container data-ng-show=hasLoading><div class=block-overlay></div><div class=block-loading></div></div>"
  );


  $templateCache.put('templates/fanex-customer-information.htm',
    "<div class=information-container><div class=row><div class=col-1>{{'UserName' | fanexTranslate}}</div><div class=col-2 data-ng-bind=userName></div><div class=col-3>{{'Nickname' | fanexTranslate}}</div><div class=col-4 data-ng-bind=nickName></div></div><div class=row><div class=col-1>{{'FirstName' | fanexTranslate}}</div><div class=col-2 data-ng-bind=firstName></div><div class=col-3>{{'Phone' | fanexTranslate}}</div><div class=col-4 data-ng-bind=phone></div></div><div class=row><div class=col-1>{{'LastName' | fanexTranslate}}</div><div class=col-2 data-ng-bind=lastName></div><div class=col-3>{{'MobilePhone' | fanexTranslate}}</div><div class=col-4 data-ng-bind=mobilePhone></div></div><div class=row><div class=col-1>{{'Fax' | fanexTranslate}}</div><div class=col-2 data-ng-bind=fax></div><div class=col-3></div><div class=col-4></div></div></div>"
  );


  $templateCache.put('templates/fanex-default-page.htm',
    "<div data-fanex-message></div><div class=product-container><div class=container data-ng-show=custNames><div class=header>{{custNames}}</div></div><div class=container data-ng-show=information><div class=header>{{'GeneralInformation' | fanexTranslate}}</div><div class=body data-fanex-customer-information=information></div></div><div class=container data-ng-show=statuses><div class=header>{{'ProductStatus' | fanexTranslate}} <span title=\"{{'Update' | fanexTranslate}}\" class=update data-ng-click=updateStatuses()><i class=icon-update></i></span></div><div class=body data-ng-include=statusTemplate></div></div><div class=container data-ng-show=discounts><div class=header>{{'Commission' | fanexTranslate}} <span data-ng-show=hasUpdateDiscount title=\"{{'Update' | fanexTranslate}}\" class=update data-ng-click=updateDiscounts()><i class=icon-update></i></span></div><div class=body data-ng-include=discountTemplate></div></div><div class=container data-ng-show=betLimits><div class=header>{{'BetSetting' | fanexTranslate}} <span data-ng-show=hasUpdateBetLimit title=\"{{'Update' | fanexTranslate}}\" class=update data-ng-click=updateBetLimits()><i class=icon-update></i></span></div><div class=body data-ng-include=betLimitTemplate></div></div><div class=container data-ng-show=positionTakings><div class=header>{{'PositionTaking' | fanexTranslate}} <span title=\"{{'Update' | fanexTranslate}}\" class=update data-ng-click=updatePositionTakings()><i class=icon-update></i></span></div><div class=body data-ng-include=positionTakingTemplate></div></div><button class=update data-ng-show=hasUpdateAll data-ng-click=updateAll()>{{'Update' | fanexTranslate}}</button></div><div data-fanex-tooltip></div><div data-fanex-pt-update-member-dialog></div>"
  );


  $templateCache.put('templates/fanex-demo-page.htm',
    "Your product is not supported."
  );


  $templateCache.put('templates/fanex-discount-master-edit-member.htm',
    "<div class=discount-item><div class=name title={{name}}>{{name}}</div><div class=agent-discount data-ng-bind=agentDiscount></div><div class=member-discount data-ng-bind=memberDiscount></div></div>"
  );


  $templateCache.put('templates/fanex-discount-super-edit-member.htm',
    "<div class=discount-item><div class=name title={{name}}>{{name}}</div><div class=master-discount data-ng-bind=masterDiscount></div><div class=agent-discount data-ng-bind=agentDiscount></div><div class=member-discount data-ng-bind=memberDiscount></div></div>"
  );


  $templateCache.put('templates/fanex-discount.htm',
    "<div class=discount-item><div class=name title={{name}}>{{name}}</div><div class=value><select data-ng-model=value data-ng-options=\"option for option in options\"></select></div></div>"
  );


  $templateCache.put('templates/fanex-error-page.htm',
    "<div data-fanex-message></div>"
  );


  $templateCache.put('templates/fanex-item-copy.htm',
    "<span class=copy title=\"{{'CopyTooltip' | fanexTranslate}}\" data-ng-show=hasCopy data-ng-click=copy()><i class=icon-copy></i></span>"
  );


  $templateCache.put('templates/fanex-message.htm',
    "<div class=message-container data-ng-show=users><div data-ng-repeat=\"user in users\"><div class=name data-ng-show=user.name>{{user.name}}</div><div data-ng-repeat=\"message in user.messages\" class=message data-ng-class=message.class><div class=icon></div><div class=content data-ng-bind-html=message.content></div></div></div></div>"
  );


  $templateCache.put('templates/fanex-pt-agent-edit-member.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=agent-pt data-fanex-pt-select=agentPT></div></div>"
  );


  $templateCache.put('templates/fanex-pt-company-edit-super.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=super-max-pt data-fanex-pt-select=superMaxPT></div><div class=master-max-pt data-fanex-pt-select=masterMaxPT></div><div class=agent-max-pt data-fanex-pt-select=agentMaxPT></div><div class=super-min-pt data-fanex-pt-select=superMinPT></div></div>"
  );


  $templateCache.put('templates/fanex-pt-copy.htm',
    "<span class=copy title=\"{{'CopyTooltip' | fanexTranslate}}\" data-ng-show=hasCopy data-ng-click=copy()><i class=icon-copy></i></span>"
  );


  $templateCache.put('templates/fanex-pt-master-edit-agent.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=master-pt data-fanex-pt-select=masterPT></div><div class=master-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"masterIsAuto\"></label></div><div class=agent-max-pt data-fanex-pt-select=agentMaxPT></div><div class=agent-min-pt data-fanex-pt-select=agentMinPT></div></div>"
  );


  $templateCache.put('templates/fanex-pt-master-edit-member.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=master-pt data-fanex-pt-select=masterPT></div><div class=master-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"masterIsAuto\"></label></div><div class=agent-pt data-ng-bind=\"agentPT | fanexPtFormat\"></div></div>"
  );


  $templateCache.put('templates/fanex-pt-select.htm',
    "<select data-ng-model=data.value data-ng-disabled=data.disabled data-ng-class=\"{'invalid': !data.valid}\" data-fanex-pt-select-options=data.options></select>"
  );


  $templateCache.put('templates/fanex-pt-super-edit-master.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=super-pt data-fanex-pt-select=superPT></div><div class=super-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"superIsAuto\"></label></div><div class=master-max-pt data-fanex-pt-select=masterMaxPT></div><div class=master-min-pt data-fanex-pt-select=masterMinPT></div></div>"
  );


  $templateCache.put('templates/fanex-pt-super-edit-member.htm',
    "<div class=position-taking-item><div class=name title={{name}}>{{name}}<div data-fanex-pt-copy=hasCopy></div></div><div class=super-pt data-fanex-pt-select=superPT></div><div class=super-is-auto><label class=checkbox><input type=checkbox data-ng-model=\"superIsAuto\"></label></div><div class=master-pt data-ng-bind=\"masterPT | fanexPtFormat\"></div><div class=agent-pt data-ng-bind=\"agentPT | fanexPtFormat\"></div></div>"
  );


  $templateCache.put('templates/fanex-pt-update-member-dialog.htm',
    "<div class=dialog-container data-ng-show=isShow><div class=dialog-scrollable><div class=dialog-content><div class=dialog-header><div class=dialog-close data-ng-click=clickClose()><i class=icon-close></i></div><div class=dialog-title>{{'UpdateMemberPtTitle' | fanexTranslate}}</div></div><div class=dialog-body><span>{{'UpdateMemberPtInfo' | fanexTranslate}}</span><br><span>{{'UpdateMemberPtQuestion' | fanexTranslate}}</span><table class=position-taking-group><tbody><tr data-ng-repeat=\"pt in pts\"><td>{{pt.name}}</td><td><select data-ng-model=pt.value data-fanex-pt-select-options=pt.options></select></td></tr></tbody></table></div><div class=dialog-footer><button data-ng-click=clickYes()>{{'Yes' | fanexTranslate}}</button> <button data-ng-click=clickNo()>{{'No' | fanexTranslate}}</button></div></div></div><div class=dialog-overlay></div></div>"
  );


  $templateCache.put('templates/fanex-status.htm',
    "<div class=status-item><div class=name title={{name}}>{{name}}</div><div class=value><label class=checkbox><input type=checkbox data-ng-model=enable data-ng-disabled=\"enableDisabled\"> {{'Enable' | fanexTranslate}}</label><label class=\"checkbox downline\" data-ng-show=showDownline><input type=checkbox data-ng-model=downline data-ng-disabled=\"downlineDisabled\"> {{'EnableDownline' | fanexTranslate}}</label></div></div>"
  );


  $templateCache.put('templates/fanex-tooltip.htm',
    "<div class=tooltip-container data-ng-class=align><div class=tooltip-arrow></div><div class=tooltip-inner data-ng-bind=message></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-bet-limit-container.htm',
    "<div class=bet-limit-container><div class=title><div class=name></div><div class=min-bet>{{'MinBet' | fanexTranslate}}</div><div class=max-bet>{{'MaxBet' | fanexTranslate}}</div><div class=max-per-match>{{'MaxPerMatch' | fanexTranslate}}</div></div><div data-ng-repeat=\"betLimit in betLimits\" data-fanex-bet-limit=betLimit data-form-type=formType data-setting-level=betLimitLevel data-has-copy=\"betLimit==betLimits[0] && betLimits.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-discount-container.htm',
    "<div class=discount-container><div data-ng-repeat=\"discount in discounts\" data-fanex-discount=discount data-setting-level=discountLevel data-form-type=formType data-ng-class=\"{'full-width': $last && $even}\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-discount-master-edit-member-container.htm',
    "<div class=\"discount-container master-edit-member\"><div class=title><div class=name></div><div class=agent-discount>{{'CommissionForAgent' | fanexTranslate}}</div><div class=member-discount>{{'CommissionForMember' | fanexTranslate}}</div></div><div data-ng-repeat=\"discount in discounts\" data-fanex-discount-master-edit-member=discount data-setting-level=discountLevel></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-discount-super-edit-member-container.htm',
    "<div class=\"discount-container super-edit-member\"><div class=title><div class=name></div><div class=master-discount>{{'CommissionForMaster' | fanexTranslate}}</div><div class=agent-discount>{{'CommissionForAgent' | fanexTranslate}}</div><div class=member-discount>{{'CommissionForMember' | fanexTranslate}}</div></div><div data-ng-repeat=\"discount in discounts\" data-fanex-discount-super-edit-member=discount data-setting-level=discountLevel></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-agent-edit-member-container.htm',
    "<div class=\"position-taking-container agent-edit-member\"><div class=title><div class=name></div><div class=agent-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-agent-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-company-edit-super-container.htm',
    "<div class=\"position-taking-container company-edit-super\"><div class=title><div class=name></div><div class=super-max-pt>{{'SuperMaxPT' | fanexTranslate}}</div><div class=master-max-pt>{{'MasterMaxPT' | fanexTranslate}}</div><div class=agent-max-pt>{{'AgentMaxPT' | fanexTranslate}}</div><div class=super-min-pt>{{'SuperMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-company-edit-super=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-master-edit-agent-container.htm',
    "<div class=\"position-taking-container master-edit-agent\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\"><div class=title><div class=name></div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=agent-max-pt>{{'AgentMaxPT' | fanexTranslate}}</div><div class=agent-min-pt>{{'AgentMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-master-edit-agent=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-master-edit-member-container.htm',
    "<div class=\"position-taking-container master-edit-member\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\"><div class=title><div class=name></div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=agent-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-master-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-super-edit-master-container.htm',
    "<div class=\"position-taking-container super-edit-master\"><div class=title><div class=name></div><div class=super-pt>{{'SuperPT' | fanexTranslate}}</div><div class=super-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=master-max-pt>{{'MasterMaxPT' | fanexTranslate}}</div><div class=master-min-pt>{{'MasterMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-super-edit-master=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-pt-super-edit-member-container.htm',
    "<div class=\"position-taking-container super-edit-member\"><div class=title><div class=name></div><div class=super-pt>{{'SuperPT' | fanexTranslate}}</div><div class=super-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakings\" data-fanex-pt-super-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div>"
  );


  $templateCache.put('templates/no-group/fanex-status-container.htm',
    "<div class=status-container><div data-ng-repeat=\"status in statuses\" class=status-item data-fanex-status=status data-setting-level=statusLevel data-form-type=formType data-ng-class=\"{'full-width': $last && $even}\"></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-bet-limit-container.htm',
    "<div data-ng-init=\"betLimitGroups=(betLimits|fanexDataGroup)\" class=bet-limit-container><div data-ng-repeat=\"betLimitGroup in betLimitGroups\" class=sport-group><div class=sport-name title={{betLimitGroup.SportTypeName}}>{{betLimitGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=min-bet>{{'MinBet' | fanexTranslate}}</div><div class=max-bet>{{'MaxBet' | fanexTranslate}}</div><div class=max-per-match>{{'MaxPerMatch' | fanexTranslate}}</div></div><div data-ng-repeat=\"betLimit in betLimitGroup.Items\" data-fanex-bet-limit=betLimit data-form-type=formType data-setting-level=betLimitLevel data-has-copy=\"betLimit==betLimits[0] && betLimits.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-discount-container.htm',
    "<div class=discount-container data-ng-init=\"discountGroups=(discounts|fanexDataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div data-ng-repeat=\"discount in discountGroup.Items\" data-fanex-discount=discount data-form-type=formType data-setting-level=discountLevel data-ng-class=\"{'full-width': $last && $even}\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-discount-master-edit-member-container.htm',
    "<div class=\"discount-container master-edit-member\" data-ng-init=\"discountGroups=(discounts|fanexDataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=agent-discount>{{'CommissionForAgent' | fanexTranslate}}</div><div class=member-discount>{{'CommissionForMember' | fanexTranslate}}</div></div><div data-ng-repeat=\"discount in discountGroup.Items\" data-fanex-discount-master-edit-member=discount data-setting-level=discountLevel></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-discount-super-edit-member-container.htm',
    "<div class=\"discount-container super-edit-member\" data-ng-init=\"discountGroups=(discounts|fanexDataGroup)\"><div data-ng-repeat=\"discountGroup in discountGroups\" class=sport-group><div class=sport-name title={{discountGroup.SportTypeName}}>{{discountGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-discount>{{'CommissionForMaster' | fanexTranslate}}</div><div class=agent-discount>{{'CommissionForAgent' | fanexTranslate}}</div><div class=member-discount>{{'CommissionForMember' | fanexTranslate}}</div></div><div data-ng-repeat=\"discount in discountGroup.Items\" data-fanex-discount-super-edit-member=discount data-setting-level=discountLevel></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-agent-edit-member-container.htm',
    "<div class=\"position-taking-container agent-edit-member\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=agent-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-agent-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-company-edit-super-container.htm',
    "<div class=\"position-taking-container company-edit-super\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-max-pt>{{'SuperMaxPT' | fanexTranslate}}</div><div class=master-max-pt>{{'MasterMaxPT' | fanexTranslate}}</div><div class=agent-max-pt>{{'AgentMaxPT' | fanexTranslate}}</div><div class=super-min-pt>{{'SuperMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-company-edit-super=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-master-edit-agent-container.htm',
    "<div class=\"position-taking-container master-edit-agent\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=agent-max-pt>{{'AgentMaxPT' | fanexTranslate}}</div><div class=agent-min-pt>{{'AgentMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-master-edit-agent=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-master-edit-member-container.htm',
    "<div class=\"position-taking-container master-edit-member\" data-ng-class=\"{ 'disable-auto-mpt' : disableAutoMPT }\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=agent-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-master-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-super-edit-master-container.htm',
    "<div class=\"position-taking-container super-edit-master\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-pt>{{'SuperPT' | fanexTranslate}}</div><div class=super-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=master-max-pt>{{'MasterMaxPT' | fanexTranslate}}</div><div class=master-min-pt>{{'MasterMinPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-super-edit-master=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-pt-super-edit-member-container.htm',
    "<div class=\"position-taking-container super-edit-member\" data-ng-init=\"positionTakingGroups=(positionTakings|fanexDataGroup)\"><div data-ng-repeat=\"positionTakingGroup in positionTakingGroups\" class=sport-group><div class=sport-name title={{positionTakingGroup.SportTypeName}}>{{positionTakingGroup.SportTypeName}}</div><div class=title><div class=name></div><div class=super-pt>{{'SuperPT' | fanexTranslate}}</div><div class=super-is-auto>{{'IsAuto' | fanexTranslate}}</div><div class=master-pt>{{'MasterPT' | fanexTranslate}}</div><div class=master-pt>{{'AgentPT' | fanexTranslate}}</div></div><div data-ng-repeat=\"positionTaking in positionTakingGroup.Items\" data-fanex-pt-super-edit-member=positionTaking data-setting-level=positionTakingLevel data-has-copy=\"positionTaking==positionTakings[0] && positionTakings.length>=2\"></div></div></div>"
  );


  $templateCache.put('templates/sport-group/fanex-status-container.htm',
    "<div class=status-container data-ng-init=\"statusGroups=(statuses|fanexDataGroup)\"><div data-ng-repeat=\"statusGroup in statusGroups\" class=sport-group><div class=sport-name title={{statusGroup.SportTypeName}}>{{statusGroup.SportTypeName}}</div><div data-ng-repeat=\"status in statusGroup.Items\" class=status-item data-fanex-status=status data-setting-level=statusLevel data-form-type=formType data-ng-class=\"{'full-width': $last && $even}\"></div></div></div>"
  );

} ]);

/*jshint -W101*/