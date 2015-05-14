/*! fanex-customer-setting v0.0.0.2 | (c) 2015 Nexcel Solutions Vietnam */

'use strict';

(function () {
    var customerSettingApp = angular.module('customerSettingApp', []);
    customerSettingApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push(['$q', '$window', '$rootScope', '$templateCache', 'fanexMessageService', 'fanexBlockUiService',
            function ($q, $window, $rootScope, $templateCache, messageService, blockUiService) {
                function startBlockUi(config) {
                    config.noBlock = (config.method === 'GET' && angular.isDefined($templateCache.get(config.url)));
                    if (!config.noBlock) {
                        blockUiService.show();
                    }
                }

                function stopBlockUi(config) {
                    if (!config.noBlock) {
                        blockUiService.hide();
                    }
                }

                return {
                    request: function (config) {
                        startBlockUi(config);
                        return config;
                    },
                    response: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        return response;
                    },
                    responseError: function (response) {
                        if (response) {
                            stopBlockUi(response.config);
                        }

                        messageService.showError('Sorry, an error occurred while processing your request.');
                        return $q.reject(response);
                    }
                };
            }
        ]);
    } ]);
} ());

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

'use strict';

(function () {
    var injectParams = ['$rootScope', '$scope', 'fanexAppSettings', 'fanexModelService', 'fanexUpdateMemberPtService',
            'fanexTemplateService', 'fanexRequestService', 'fanexMessageService', 'fanexResourceService', 'fanexSettingTypeHandler'];
    var fanexDefaultController = function ($rootScope, $scope, appSettings, modelService, updateMemberPtService,
            templateService, requestService, messageService, resourceService, SettingTypeHandler) {
        var queryModel = modelService.getQueryModel();
        requestService.query(queryModel, function (queryData) {
            var requestContext = queryData.RequestContext;
            var customerSetting = queryData.CustomerSetting;
            var settingContext = queryData.SettingContext;
            resourceService.setCurrentProductName(queryData.ProductName);
            resourceService.setLanguage(queryData.Language);

            var multipleInfo = queryModel.getMultipleInfo();
            if (multipleInfo.IsMultiple) {
                $scope.custNames = multipleInfo.CustNames.join(',');
            } else {
                $scope.information = customerSetting.Information;
            }

            $scope.formType = queryModel.FormType;

            // status
            $scope.statuses = customerSetting.Statuses;
            $scope.statusLevel = settingContext.StatusLevel;
            $scope.statusTemplate = templateService.resolveStatus();

            // discount
            $scope.discounts = customerSetting.Discounts;
            $scope.discountLevel = settingContext.DiscountLevel;
            $scope.discountTemplate = templateService.resolveDiscount();
            $scope.hasUpdateDiscount = $scope.formType !== appSettings.formType.SuperEditMember
                && $scope.formType !== appSettings.formType.MasterEditMember;

            // bet limit
            $scope.betLimits = customerSetting.BetLimits;
            $scope.betLimitLevel = settingContext.BetLimitLevel;
            $scope.betLimitTemplate = templateService.resolveBetLimit();
            $scope.hasUpdateBetLimit = $scope.hasUpdateDiscount;

            // position taking
            $scope.positionTakings = customerSetting.PositionTakings;
            $scope.positionTakingLevel = settingContext.PositionTakingLevel;
            $scope.positionTakingTemplate = templateService.resolvePositionTaking();
            if (requestContext.FormType === appSettings.formType.MasterEditAgent) {
                $scope.disableAutoMPT = settingContext.DisableAutoMPT;
            }

            $scope.hasUpdateAll = queryModel;
        });

        var commandType;
        $scope.updateStatuses = function () {
            commandType = appSettings.commandType.Status;
            update();
        };

        $scope.updateDiscounts = function () {
            commandType = appSettings.commandType.Discount;
            update();
        };

        $scope.updateBetLimits = function () {
            commandType = appSettings.commandType.BetLimit;
            update();
        };

        $scope.updatePositionTakings = function () {
            commandType = appSettings.commandType.PositionTaking;
            update();
        };

        $scope.updateAll = function () {
            commandType = appSettings.commandType.All;
            update();
        };

        $rootScope.$on('update-member-position-taking', function (event, updateMemberPT) {
            update(updateMemberPT);
        });

        function getUpdateData(settingType) {
            var settingTypeHandler = new SettingTypeHandler(settingType);
            var scopes = settingTypeHandler.getScopes();
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
        }

        function update(updateMemberPT) {
            var queryData = requestService.getQueryData();
            var requestContext = queryData.RequestContext;
            var settingContext = queryData.SettingContext;

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
                            var name = resourceService.getDisplayName($scope.positionTakingLevel, pt);
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
                        updateMemberPtService.show(memberPTs);
                        return true;
                    }
                }

                return false;
            }

            var positionTakings;
            var betLimits;
            var discounts;
            var statuses;
            switch (commandType) {
                case appSettings.commandType.All:
                    statuses = getUpdateData(appSettings.settingType.Status);
                    discounts = getUpdateData(appSettings.settingType.Discount);
                    betLimits = getUpdateData(appSettings.settingType.BetLimit);
                    positionTakings = getUpdateData(appSettings.settingType.PositionTaking);
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

                case appSettings.commandType.PositionTaking:
                    positionTakings = getUpdateData(appSettings.settingType.PositionTaking);
                    if (!hasUpdate(positionTakings, true)) {
                        return;
                    }

                    if (angular.isUndefined(updateMemberPT) && hasUncheckAutoPT(positionTakings)) {
                        return;
                    }
                    break;

                case appSettings.commandType.BetLimit:
                    betLimits = getUpdateData(appSettings.settingType.BetLimit);
                    if (!hasUpdate(betLimits, true)) {
                        return;
                    }
                    break;

                case appSettings.commandType.Discount:
                    discounts = getUpdateData(appSettings.settingType.Discount);
                    if (!hasUpdate(discounts, true)) {
                        return;
                    }
                    break;

                case appSettings.commandType.Status:
                    statuses = getUpdateData(appSettings.settingType.Status);
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

            var multipleInfo = queryModel.getMultipleInfo();
            var commandModel = angular.extend({
                CommandType: commandType,
                StatusLevel: settingContext.StatusLevel,
                StatusString: JSON.stringify(statuses),
                DiscountLevel: settingContext.DiscountLevel,
                DiscountString: JSON.stringify(discounts),
                BetLimitLevel: settingContext.BetLimitLevel,
                BetLimitString: JSON.stringify(betLimits),
                PositionTakingLevel: settingContext.PositionTakingLevel,
                PositionTakingString: JSON.stringify(positionTakings)
            }, queryModel);
            if (multipleInfo.IsMultiple) {
                commandModel = angular.extend({
                    TargetCustIds: JSON.stringify(multipleInfo.CustIds)
                }, commandModel);
            }

            requestService.command(commandModel, function (commandData) {
                if (multipleInfo.IsMultiple) {
                    var custNames = multipleInfo.CustNames;
                    var commandResults = commandData;
                    messageService.showMultiple(custNames, commandResults);
                    return;
                }

                var commandResult = commandData;
                messageService.showSingle(commandResult);
                angular.forEach(commandResult.SettingResults, function (settingResult) {
                    if (settingResult.IsSuccess) {
                        var settingType = settingResult.SettingType;
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

    fanexDefaultController.$inject = injectParams;
    angular.module('customerSettingApp').controller('FanexDefaultController', fanexDefaultController);
} ());

'use strict';

(function () {
    var injectParams = ['$timeout', 'fanexMessageService'];
    var fanexErrorController = function ($timeout, messageService) {
        messageService.showError('Please check your URL');
    };

    fanexErrorController.$inject = injectParams;
    angular.module('customerSettingApp').controller('FanexErrorController', fanexErrorController);
} ());

'use strict';

(function () {
    var injectParams = ['$filter', 'fanexAppSettings', 'fanexCommonService', 'fanexTooltipService'];
    var fanexBetLimitInputDirective = function ($filter, appSettings, commonService, tooltipService) {
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

    fanexBetLimitInputDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimitInput', fanexBetLimitInputDirective);
})();

'use strict';

(function () {
    var injectParams = ['$filter', 'fanexAppSettings', 'fanexBetLimitService', 'fanexResourceService'];
    var errorType = {
        Limit: 1,
        MinBet: 2,
        MaxBet: 3,
        MaxPerMatch: 4
    };
    var fanexBetLimitDirective = function ($filter, appSettings, betLimitService, resourceService) {
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
                betLimit: '=fanexBetLimit',
                formType: '=',
                settingLevel: '=',
                hasCopy: '='
            },
            template: '<div data-ng-include="templateUrl"></div>',
            controller: ['$scope', function ($scope) {
                betLimitService.linkScope($scope);

                $scope.settingType = appSettings.settingType.BetLimit;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.betLimit);
                var betLimit = $scope.betLimit;
                $scope.isReadonly = $scope.formType === appSettings.formType.SuperEditMember
                    || $scope.formType === appSettings.formType.MasterEditMember;
                $scope.templateUrl = $scope.isReadonly
                    ? 'templates/fanex-bet-limit-readonly.htm'
                    : 'templates/fanex-bet-limit.htm';

                var data = parse(betLimit, $scope.formType);
                $scope.minBet = data.minBet;
                $scope.hasMinBetLimit = $scope.minBet.limit > 0;
                $scope.maxBet = data.maxBet;
                $scope.hasMaxBetLimit = $scope.formType !== appSettings.formType.CompanyEditSuper;
                $scope.maxPerMatch = data.maxPerMatch;
                $scope.hasMaxPerMatchLimit = $scope.hasMaxBetLimit;
                $scope.appSettings = appSettings;

                if ($scope.isReadonly) {
                    return;
                }

                $scope.copy = function (copyScope) {
                    $scope.minBet.value = copyScope.minBet.value;
                    $scope.maxBet.value = copyScope.maxBet.value;
                    $scope.maxPerMatch.value = copyScope.maxPerMatch.value;
                };

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

    fanexBetLimitDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBetLimit', fanexBetLimitDirective);
})();

'use strict';

(function () {
    var injectParams = ['$compile', '$templateCache', 'fanexBlockUiService'];
    var fanexBlockUiDirective = function ($compile, $templateCache, blockUiService) {
        return {
            restrict: 'A',
            compile: function () {
                return function (scope, element) {
                    var template = $templateCache.get('templates/fanex-block-ui.htm');
                    var clone = angular.element(template);
                    element.append(clone);
                    var newScope = scope.$new();
                    var link = $compile(clone);
                    link(newScope);
                    blockUiService.setup(newScope);
                };
            }
        };
    };

    fanexBlockUiDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexBlockUi', fanexBlockUiDirective);
})();

'use strict';

(function () {
    var injectParams = [];
    var fanexCustomerInformationDirective = function () {
        return {
            restrict: 'AE',
            scope: {
                information: '=fanexCustomerInformation'
            },
            templateUrl: 'templates/fanex-customer-information.htm',
            controller: ['$scope', function ($scope) {
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

    fanexCustomerInformationDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexCustomerInformation', fanexCustomerInformationDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountMasterEditMemberDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountMasterEditMember',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-master-edit-member.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexDiscountMasterEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMember', fanexDiscountMasterEditMemberDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexSuperEditMemberDiscountDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountSuperEditMember',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-super-edit-member.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexSuperEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexSuperEditMemberDiscount', fanexSuperEditMemberDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountDirective = function (appSettings, discountService, resourceService) {
        function parse(data, formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return discountService.createSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);

                case appSettings.formType.SuperEditMaster:
                    return discountService.createSelect(0, data.SuperDiscount, data.MasterDiscount);

                case appSettings.formType.MasterEditAgent:
                    return discountService.createSelect(0, data.MasterDiscount, data.AgentDiscount);

                case appSettings.formType.AgentEditMember:
                    return discountService.createSelect(0, data.AgentDiscount, data.PlayerDiscount);
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
                discount: '=fanexDiscount',
                formType: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                var data = parse(discount, $scope.formType);
                $scope.min = data.min;
                $scope.value = data.value;
                $scope.options = data.options;

                $scope.validate = function () {
                    var errCode = $scope.value !== getValue(discount, $scope.formType)
                    ? appSettings.errorType.Success
                    : appSettings.errorType.NoChange;

                    var data = {
                        SportType: discount.SportType,
                        BetType: discount.BetType
                    };
                    data[getName($scope.formType)] = $scope.value;
                    return {
                        errCode: errCode,
                        data: data
                    };
                };
            } ]
        };
    };

    fanexDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscount', fanexDiscountDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexSettingTypeHandler'];
    var fanexItemCopyDirective = function (SettingTypeHandler) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                settingType: '=settingType',
                hasCopy: '=fanexItemCopy'
            },
            templateUrl: 'templates/fanex-item-copy.htm',
            link: function (scope) {
                if (!scope.hasCopy) {
                    return;
                }

                scope.copy = function () {
                    var settingTypeHandler = new SettingTypeHandler(scope.settingType);
                    var scopes = settingTypeHandler.getScopes();
                    var source = scopes[0];
                    for (var i = 1; i < scopes.length; i++) {
                        scopes[i].copy(source);
                    }
                };
            }
        };
    };

    fanexItemCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexItemCopy', fanexItemCopyDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexMessageService'];
    var fanexMessageDirective = function (fanexMessageService) {
        return {
            restrict: 'A',
            replace: true,
            templateUrl: 'templates/fanex-message.htm',
            controller: ['$scope', function ($scope) {
                fanexMessageService.setup($scope);
            } ]
        };
    };

    fanexMessageDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexMessage', fanexMessageDirective);
})();

'use strict';

(function () {
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache',
            'fanexProductSettingService', 'fanexModelService'];
    var fanexProductViewDirective = function ($sce, $compile, $controller, $templateCache,
            fanexProductSettingService, modelService) {
        return {
            transclude: 'element',
            compile: function (element, attr, linker) {
                return function (scope, $element) {
                    var currentElement;

                    // update view
                    function update() {
                        var productViewSetting = fanexProductSettingService.errorViewSetting;
                        var queryModel = modelService.getQueryModel();
                        if (angular.isDefined(queryModel)) {
                            var productId = queryModel.ProductId;
                            productViewSetting = fanexProductSettingService.getViewSetting(productId);
                        }

                        var templateUrl = productViewSetting.templateUrl;
                        if (!angular.isDefined(templateUrl)) {
                            return;
                        }

                        templateUrl = $sce.getTrustedResourceUrl(templateUrl);
                        var template = $templateCache.get(templateUrl);
                        var newScope = scope.$new();
                        linker(newScope, function (clone) {
                            clone.html(template);
                            $element.after(clone);

                            if (currentElement) {
                                currentElement.remove();
                            }

                            var link = $compile(clone.contents());
                            currentElement = clone;
                            var controller = productViewSetting.controller;
                            if (angular.isDefined(controller)) {
                                controller = $controller(productViewSetting.controller, {
                                    $scope: newScope
                                });
                                clone.data('$ngControllerController', controller);
                                clone.children().data('$ngControllerController', controller);
                            }

                            link(newScope);
                        });

                        scope.$emit('product-view-loaded');
                    }

                    scope.$on('fanex-model-changed', update);
                };
            }
        };
    };

    fanexProductViewDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexProductView', fanexProductViewDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtAgentEditMemberDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtAgentEditMember',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-agent-edit-member.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);
                var positionTaking = $scope.positionTaking;
                var isInitialized = false;

                // Agent PT
                function generateAgentPT() {
                    var min = positionTaking.N2;
                    var max = positionTaking.A2;
                    var value = !isInitialized
                        ? positionTaking.A1
                        : ptService.correctValue(min, max, $scope.agentPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentPT = generateAgentPT();
                $scope.$watch('agentPT.value', function (newValue, oldValue) {
                    if ($scope.agentPT !== null && $scope.agentPT.valid === false) {
                        return;
                    }

                    if (newValue !== oldValue) {
                        $scope.agentPT = generateAgentPT();
                    }
                });

                // Copy
                $scope.copy = function (source) {
                    return ptService.copyValue(source, $scope, 'agentPT', generateAgentPT);
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

    fanexPtAgentEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtAgentEditMember', fanexPtAgentEditMemberDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtCompanyEditSuperDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtCompanyEditSuper',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-company-edit-super.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

                // Super Max PT
                function generateSuperMaxPT() {
                    var min = 0;
                    var max = positionTaking.DefaultMaxPT;
                    var value = !isInitialized
                        ? positionTaking.S4
                        : ptService.correctValue(min, max, $scope.superMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superMaxPT = generateSuperMaxPT();
                $scope.$watch('superMaxPT.value', function (newValue, oldValue) {
                    if ($scope.superMaxPT !== null && $scope.superMaxPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.masterMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if ($scope.agentMaxPT !== null && $scope.agentMaxPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.agentMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();

                // Super Min PT
                function generateSuperMinPT() {
                    var min = 0;
                    var max = $scope.superMaxPT.value;
                    var value = !isInitialized
                        ? positionTaking.N4
                        : ptService.correctValue(min, max, $scope.superMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superMinPT = generateSuperMinPT();

                // Copy
                $scope.copy = function (source) {
                    return ptService.copyValue(source, $scope, 'superMaxPT', generateSuperMaxPT)
                        && ptService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && ptService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && ptService.copyValue(source, $scope, 'superMinPT', generateSuperMinPT);
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

    fanexPtCompanyEditSuperDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCompanyEditSuper', fanexPtCompanyEditSuperDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexPtService', 'fanexResourceService', 'fanexTooltipService'];
    var fanexPtCopyDirective = function (ptService, resourceService, tooltipService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                hasCopy: '=fanexPtCopy'
            },
            templateUrl: 'templates/fanex-pt-copy.htm',
            link: function (scope, element) {
                if (!scope.hasCopy) {
                    return;
                }

                scope.copy = function () {
                    var count = 0;
                    var scopes = ptService.getScopes();
                    var source = scopes[0];
                    for (var i = 1; i < scopes.length; i++) {
                        var scope = scopes[i];
                        var result = scope.copy(source);
                        if (!result) {
                            count++;
                        }
                    }

                    if (count > 0) {
                        tooltipService.show(element, resourceService.getString('DuplicateValuesAlertMsg'), 'left');
                        element.on('mouseleave', function () {
                            tooltipService.hide();
                            element.off('mouseleave');
                        });
                    } else {
                        tooltipService.hide();
                    }
                };
            }
        };
    };

    fanexPtCopyDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtCopy', fanexPtCopyDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtMasterEditAgentDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtMasterEditAgent',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-master-edit-agent.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

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
                        : ptService.correctValue(min, max, $scope.masterPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterPT = generateMasterPT();
                $scope.$watch('masterPT.value', function (newValue, oldValue) {
                    if ($scope.agentMaxPT !== null && $scope.agentMaxPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.agentMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMaxPT = generateAgentMaxPT();
                $scope.$watch('agentMaxPT.value', function (newValue, oldValue) {
                    if ($scope.agentMinPT !== null && $scope.agentMinPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.agentMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.agentMinPT = generateAgentMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    return ptService.copyValue(source, $scope, 'masterPT', generateMasterPT)
                        && ptService.copyValue(source, $scope, 'agentMaxPT', generateAgentMaxPT)
                        && ptService.copyValue(source, $scope, 'agentMinPT', generateAgentMinPT);
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

    fanexPtMasterEditAgentDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditAgent', fanexPtMasterEditAgentDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtMasterEditMemberDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtMasterEditMember',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-master-edit-member.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

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
                            : ptService.correctValue(min, max, $scope.masterPT.value);
                    var select = ptService.createSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.masterPT = generateMasterPT();

                // Agent PT
                $scope.agentPT = positionTaking.A1;

                // Copy
                $scope.copy = function (source) {
                    $scope.masterIsAuto = source.masterIsAuto;
                    if (!$scope.masterPT.disabled || source.masterIsAuto) {
                        return ptService.copyValue(source, $scope, 'masterPT', generateMasterPT);
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

    fanexPtMasterEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtMasterEditMember', fanexPtMasterEditMemberDirective);
})();

'use strict';

(function () {
    var injectParams = ['$parse', 'fanexAppSettings'];
    var fanexPtSelectOptionsDirective = function ($parse, appSettings) {
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

                scope.$watchCollection(attrs.fanexPtSelectOptions, function (newValues, oldValues) {
                    if (newValues !== oldValues) {
                        render();
                    }
                });

                var valueFn = $parse(attrs.fanexPtSelectOptions);
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

    fanexPtSelectOptionsDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSelectOptions', fanexPtSelectOptionsDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var fanexPtSelectDirective = function ($rootScope) {
        return {
            restrict: 'AE',
            scope: {
                data: '=fanexPtSelect'
            },
            templateUrl: 'templates/fanex-pt-select.htm',
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

    fanexPtSelectDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSelect', fanexPtSelectDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtSuperEditMasterDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtSuperEditMaster',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-super-edit-master.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

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
                        : ptService.correctValue(min, max, $scope.superPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.superPT = generateSuperPT();
                $scope.$watch('superPT.value', function (newValue, oldValue) {
                    if ($scope.masterMaxPT !== null && $scope.masterMaxPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.masterMaxPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMaxPT = generateMasterMaxPT();
                $scope.$watch('masterMaxPT.value', function (newValue, oldValue) {
                    if ($scope.masterMinPT !== null && $scope.masterMinPT.valid === false) {
                        return;
                    }

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
                        : ptService.correctValue(min, max, $scope.masterMinPT.value);
                    return ptService.createSelect(min, max, value);
                }
                $scope.masterMinPT = generateMasterMinPT();

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    return ptService.copyValue(source, $scope, 'superPT', generateSuperPT)
                        && ptService.copyValue(source, $scope, 'masterMaxPT', generateMasterMaxPT)
                        && ptService.copyValue(source, $scope, 'masterMinPT', generateMasterMinPT);
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

    fanexPtSuperEditMasterDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMaster', fanexPtSuperEditMasterDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexPtService', 'fanexResourceService'];
    var fanexPtSuperEditMemberDirective = function (appSettings, ptService, resourceService) {
        return {
            restrict: 'AE',
            scope: {
                positionTaking: '=fanexPtSuperEditMember',
                hasCopy: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-pt-super-edit-member.htm',
            controller: ['$scope', function ($scope) {
                ptService.linkScope($scope);

                var positionTaking = $scope.positionTaking;
                var isInitialized = false;
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.positionTaking);

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
                            : ptService.correctValue(min, max, $scope.superPT.value);
                    var select = ptService.createSelect(min, max, value);
                    select.disabled = disabled;
                    return select;
                }
                $scope.superPT = generateSuperPT();

                // Master PT
                $scope.masterPT = positionTaking.M1;

                // Agent PT
                $scope.agentPT = positionTaking.A1;

                // Copy
                $scope.copy = function (source) {
                    $scope.superIsAuto = source.superIsAuto;
                    if (!$scope.superPT.disabled || source.superIsAuto) {
                        return ptService.copyValue(source, $scope, 'superPT', generateSuperPT);
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

    fanexPtSuperEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtSuperEditMember', fanexPtSuperEditMemberDirective);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'fanexAppSettings', 'fanexUpdateMemberPtService'];
    var fanexPtUpdateMemberDialogDirective = function ($rootScope, appSettings, updateMemberPtService) {
        return {
            restrict: 'AE',
            replace: true,
            templateUrl: 'templates/fanex-pt-update-member-dialog.htm',
            controller: ['$scope', function ($scope) {
                updateMemberPtService.setup($scope);
                $scope.settingType = appSettings.settingType.PositionTaking;
                $scope.isShow = false;
                $scope.clickClose = function () {
                    $scope.isShow = false;
                };
                $scope.clickYes = function () {
                    $scope.isShow = false;
                    update(true);
                };
                $scope.clickNo = function () {
                    $scope.isShow = false;
                    update(false);
                };

                function update(isUpdateAllMembers) {
                    var memberPTs = [];
                    angular.forEach($scope.pts, function (pt) {
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
            } ]
        };
    };

    fanexPtUpdateMemberDialogDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexPtUpdateMemberDialog', fanexPtUpdateMemberDialogDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexStatusService', 'fanexResourceService'];
    var fanexStatusDirective = function (appSettings, statusService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                status: '=fanexStatus',
                formType: '=',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-status.htm',
            controller: ['$scope', function ($scope) {
                statusService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.status);
                $scope.enable = !$scope.status.IsDisabled;
                $scope.downline = !$scope.status.IsDisabledDownline;
                $scope.showDownline = $scope.formType !== appSettings.formType.AgentEditMember;
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

    fanexStatusDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexStatus', fanexStatusDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexTooltipService'];
    var fanexTooltipDirective = function (tooltipService) {
        return {
            restrict: 'AE',
            priority: 100,
            replace: true,
            templateUrl: 'templates/fanex-tooltip.htm',
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

    fanexTooltipDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexTooltip', fanexTooltipDirective);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexModelService'];
    var fanexCommandReader = function (appSettings, modelService) {
        function SingleCommandReader() {
            this.read = function (response) {
                var commandResult = response;
                return commandResult;
            };
        }

        function MultipleCommandReader() {
            this.read = function (response) {
                var commandResults = response;
                return commandResults;
            };
        }

        function CommandReader() {
            this.read = function (response) {
                var queryModel = modelService.getQueryModel();
                var multipleInfo = queryModel.getMultipleInfo();
                var reader;
                if (multipleInfo.IsMultiple) {
                    reader = new MultipleCommandReader();
                } else {
                    reader = new SingleCommandReader();
                }

                return reader.read(response);
            };
        }

        return CommandReader;
    };

    fanexCommandReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('FanexCommandReader', fanexCommandReader);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var fanexDirectiveLinker = function ($rootScope) {
        function DirectiveLinker() {
            var scopes = [];

            function linkScope(scope) {
                scopes.push(scope);
            }

            function getScopes() {
                return scopes;
            }

            $rootScope.$on('product-view-loaded', function () {
                scopes = [];
            });

            return {
                linkScope: linkScope,
                getScopes: getScopes
            };
        }

        return DirectiveLinker;
    };

    fanexDirectiveLinker.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveLinker', fanexDirectiveLinker);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexFormTypeHandler = function (appSettings) {
        var CompanyEditSuperHandler = function () {
            this.formType = appSettings.formType.CompanyEditSuper;
            this.pattern = '/editSuper/:superId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Company,
                    TargetCustId: params.superId,
                    TargetRoleId: appSettings.roleType.Super
                };
            };
        };

        var SuperEditMasterHandler = function () {
            this.formType = appSettings.formType.SuperEditMaster;
            this.pattern = '/editMaster/:masterId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.masterId,
                    TargetRoleId: appSettings.roleType.Master
                };
            };
        };

        var MasterEditAgentHandler = function () {
            this.formType = appSettings.formType.MasterEditAgent;
            this.pattern = '/editAgent/:agentId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.agentId,
                    TargetRoleId: appSettings.roleType.Agent
                };
            };
        };

        var AgentEditMemberHandler = function () {
            this.formType = appSettings.formType.AgentEditMember;
            this.pattern = '/editMember/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Agent,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var SuperEditMemberHandler = function () {
            this.formType = appSettings.formType.SuperEditMember;
            this.pattern = '/superEditMember/:superId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: params.superId,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var MasterEditMemberHandler = function () {
            this.formType = appSettings.formType.MasterEditMember;
            this.pattern = '/masterEditMember/:masterId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: params.masterId,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        function FormTypeHandler(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return new CompanyEditSuperHandler();

                case appSettings.formType.SuperEditMaster:
                    return new SuperEditMasterHandler();

                case appSettings.formType.MasterEditAgent:
                    return new MasterEditAgentHandler();

                case appSettings.formType.SuperEditMember:
                    return new SuperEditMemberHandler();

                case appSettings.formType.MasterEditMember:
                    return new MasterEditMemberHandler();

                case appSettings.formType.AgentEditMember:
                    return new AgentEditMemberHandler();
            }
        }

        return FormTypeHandler;
    };

    fanexFormTypeHandler.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexFormTypeHandler', fanexFormTypeHandler);
})();

'use strict';

(function () {
    var injectParams = ['$location', 'fanexAppSettings', 'fanexCommonService', 'fanexFormTypeHandler'];
    var fanexQueryModel = function ($location, appSettings, commonService, FormTypeHandler) {
        var isValid = false;
        var viewParams = {};
        var isMultiple = false;
        var custIds = [];
        var custNames = [];
        var formTypeHandler;

        function readSingleInformation() {
            var path = $location.path();
            var formTypes = commonService.getPropertyValues(appSettings.formType);
            angular.forEach(formTypes, function (formType) {
                if (isValid) {
                    return;
                }

                formTypeHandler = new FormTypeHandler(formType);
                var pattern = formTypeHandler.pattern;
                var matches = pattern.match(/\/:\w+/g);
                var queryParams = [];
                angular.forEach(matches, function (match) {
                    queryParams.push(match.replace('/:', ''));
                });

                var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
                matches = regExp.exec(path);
                if (matches !== null && matches.length - 1 === queryParams.length) {
                    isValid = true;
                    angular.forEach(queryParams, function (queryParam, index) {
                        viewParams[queryParam] = parseInt(matches[index + 1]);
                    });
                }
            });
        }

        function readMultipleInformation() {
            isMultiple = false;
            var queryString = $location.search();
            var custIdsQueryString = queryString.custIds;
            var custNamesQueryString = queryString.custNames;
            if (angular.isDefined(custIdsQueryString) && angular.isDefined(custNamesQueryString)) {
                isMultiple = true;
                custIds = custIdsQueryString.split(',').map(Number);
                custNames = custNamesQueryString.split(',');
                if (custIds.length !== custNames.length || custIds.length <= 1) {
                    isValid = false;
                }
            }
        }

        function QueryModel() {
            isValid = false;
            readSingleInformation();
            if (isValid) {
                readMultipleInformation();
            }

            if (!isValid) {
                return undefined;
            }

            var model = formTypeHandler.createModel(viewParams);
            model.getMultipleInfo = function () {
                return {
                    IsMultiple: isMultiple,
                    CustIds: custIds,
                    CustNames: custNames
                };
            };

            return model;
        }

        return QueryModel;
    };

    fanexQueryModel.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexQueryModel', fanexQueryModel);
})();

'use strict';

(function () {
    var injectParams = [];
    var fanexQueryReader = function () {
        function QueryData(response) {
            var requestContext = response.RequestContext;
            var customerSetting = response.CustomerSetting;
            var settingContext = customerSetting.SettingContext;
            var productName = requestContext.ProductName;
            var language = requestContext.Language;

            return {
                RequestContext: requestContext,
                CustomerSetting: customerSetting,
                SettingContext: settingContext,
                ProductName: productName,
                Language: language
            };
        }

        function QueryReader() {
            this.read = function (response) {
                return new QueryData(response);
            };
        }

        return QueryReader;
    };

    fanexQueryReader.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexQueryReader', fanexQueryReader);
})();

'use strict';

(function () {
    var injectParams = [];
    var fanexSelectBuilder = function () {
        function OptionsBuilder(step, factor) {
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
        }

        function SelectBuilder(step, factor) {
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
        }

        return SelectBuilder;
    };

    fanexSelectBuilder.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexSelectBuilder', fanexSelectBuilder);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexStatusService', 'fanexDiscountService',
            'fanexBetLimitService', 'fanexPtService'];
    var fanexSettingTypeHandler = function (appSettings, statusService, discountService,
            betLimitService, ptService) {
        var StatusHandler = function () {
            this.settingType = appSettings.settingType.Status;
            this.getScopes = statusService.getScopes;
        };

        var DiscountHandler = function () {
            this.settingType = appSettings.settingType.Discount;
            this.getScopes = discountService.getScopes;
        };

        var BetLimitHandler = function () {
            this.settingType = appSettings.settingType.BetLimit;
            this.getScopes = betLimitService.getScopes;
        };

        var PositionTakingHandler = function () {
            this.settingType = appSettings.settingType.PositionTaking;
            this.getScopes = ptService.getScopes;
        };

        function SettingTypeHandler(settingType) {
            switch (settingType) {
                case appSettings.settingType.Status:
                    return new StatusHandler();

                case appSettings.settingType.Discount:
                    return new DiscountHandler();

                case appSettings.settingType.BetLimit:
                    return new BetLimitHandler();

                case appSettings.settingType.PositionTaking:
                    return new PositionTakingHandler();
            }
        }

        return SettingTypeHandler;
    };

    fanexSettingTypeHandler.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexSettingTypeHandler', fanexSettingTypeHandler);
})();

'use strict';

(function () {
    var injectParams = [];
    var fanexDataGroup = function () {
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
    };

    fanexDataGroup.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexDataGroup', fanexDataGroup);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexPtFormat = function (appSettings) {
        return function (value) {
            return value / appSettings.positionTakingDivider;
        };
    };

    fanexPtFormat.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexPtFormat', fanexPtFormat);
})();

'use strict';

(function () {
    var injectParams = ['fanexResourceService'];
    var fanexTranslate = function (resourceService) {
        return function (key) {
            return resourceService.getString(key);
        };
    };

    fanexTranslate.$inject = injectParams;
    angular.module('customerSettingApp').filter('fanexTranslate', fanexTranslate);
})();

'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker'];
    var fanexBetLimitService = function (DirectiveLinker) {
        var service = new DirectiveLinker();
        return service;
    };

    fanexBetLimitService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexBetLimitService', fanexBetLimitService);
})();

'use strict';

(function () {
    var injectParams = [];
    var fanexBlockUiService = function () {
        var blockUiScope;

        function setup(scope) {
            if (angular.isDefined(blockUiScope)) {
                throw 'Only one block ui component in document';
            }

            blockUiScope = scope;
            blockUiScope.hasLoading = false;
        }

        function show() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = true;
        }

        function hide() {
            if (angular.isUndefined(blockUiScope)) {
                throw 'setup is required';
            }

            blockUiScope.hasLoading = false;
        }

        return {
            setup: setup,
            show: show,
            hide: hide
        };
    };

    fanexBlockUiService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexBlockUiService', fanexBlockUiService);
})();

'use strict';

(function () {
    var injectParams = ['$window'];
    var fanexCommonService = function ($window) {
        // scroll to top
        function scrollToTop() {
            $window.scrollTo(0, 0);
        }

        // check if value is in array, return index of value or -1 if not exist
        function inArray(value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        }

        // check if object is array and it's not empty.
        function isArrayAndNotEmpty(object) {
            return angular.isArray(object) && object.length > 0;
        }

        // return array of property values of object.
        function getPropertyValues(object) {
            var propertyValues = [];
            for (var propertyName in object) {
                propertyValues.push(object[propertyName]);
            }

            return propertyValues;
        }

        return {
            scrollToTop: scrollToTop,
            inArray: inArray,
            isArrayAndNotEmpty: isArrayAndNotEmpty,
            getPropertyValues: getPropertyValues
        };
    };

    fanexCommonService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexCommonService', fanexCommonService);
})();

'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker', 'fanexSelectBuilder'];
    var fanexDiscountService = function (DirectiveLinker, SelectBuilder) {
        var service = new DirectiveLinker();

        service.createSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(0.0005, 10000);
            return selectBuilder.build(min, max, value);
        };

        return service;
    };

    fanexDiscountService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexDiscountService', fanexDiscountService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', '$timeout', '$sce', 'fanexCommonService', 'fanexAppSettings', 'fanexResourceService'];
    var fanexMessageService = function ($rootScope, $timeout, $sce, commonService, appSettings, resourceService) {
        var messageScope;
        var messageOptions = {
            RemoveNoChange: true
        };
        var messageClasses = {};
        messageClasses[appSettings.messageType.Success] = 'success';
        messageClasses[appSettings.messageType.Warning] = 'warning';
        messageClasses[appSettings.messageType.Error] = 'error';

        var Message = function (type, content) {
            return {
                'class': messageClasses[type],
                content: $sce.trustAsHtml(content)
            };
        };

        Message.fromResult = function (result) {
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
        };

        Message.fromResults = function (results) {
            var messages = [];
            angular.forEach(results, function (result) {
                var message = Message.fromResult(result);
                messages.push(message);
            });

            return messages;
        };

        var User = function (name, messages) {
            return {
                name: name,
                messages: messages
            };
        };

        function setup(scope) {
            messageScope = scope;
        }

        function config(options) {
            messageOptions = angular.extend(options, messageOptions);
        }

        function showSuccess(messageContent) {
            var message = new Message(appSettings.messageType.Success, messageContent || 'Undefined success message');
            showMessage(message);
        }

        function showWarning(messageContent) {
            var message = new Message(appSettings.messageType.Warning, messageContent || 'Undefined warning message');
            showMessage(message);
        }

        function showNoChange() {
            showWarning(resourceService.getString('NoChangeToUpdate'));
        }

        function showError(messageContent) {
            var message = new Message(appSettings.messageType.Error, messageContent || 'Undefined error message');
            showMessage(message);
        }

        function showMessage(message) {
            var messages = [message];
            showMessages(messages);
        }

        function showMessages(messages) {
            showUserMessages(undefined, messages);
        }

        function showUserMessages(userName, userMessages) {
            var user = new User(userName, userMessages);
            showUser(user);
        }

        function showUser(user) {
            var users = [user];
            showUsers(users);
        }

        function showUsers(users) {
            commonService.scrollToTop();

            $timeout(function () {
                messageScope.users = users;
            });
        }

        function filter(settingResults) {
            settingResults = commonService.getPropertyValues(settingResults);
            if (messageOptions.RemoveNoChange) {
                return filterRemoveNoChange(settingResults);
            }

            return settingResults;
        }

        // if having any successful or error messages -> remove any no-change results
        function filterRemoveNoChange(settingResults) {
            var filteredSettingResults = [];
            angular.forEach(settingResults, function (settingResult) {
                if (settingResult.ErrCode !== appSettings.errorCode.NoChange) {
                    filteredSettingResults.push(settingResult);
                }
            });

            if (filteredSettingResults.length === 0) {
                filteredSettingResults = settingResults[0];
            }

            return filteredSettingResults;
        }

        // user has list of results: position taking result, bet limit result, discount result, status result.
        function showSingle(commandResult) {
            var settingResults = commandResult.SettingResults;
            settingResults = filter(settingResults);
            var messages = Message.fromResults(settingResults);
            showMessages(messages);
        }

        function showMultiple(userNames, commandResults) {
            var users = [];
            angular.forEach(userNames, function (userName, index) {
                var commandResult = commandResults[index];
                var userMessages;
                if (commandResult.IsSuccess) {
                    var settingResults = commandResult.SettingResults;
                    settingResults = filter(settingResults);
                    userMessages = Message.fromResults(settingResults);
                } else {
                    var message = Message.fromResult(commandResult);
                    userMessages = [message];
                }
                var user = new User(userName, userMessages);
                users.push(user);
            });

            showUsers(users);
        }

        function hide() {
            if (angular.isDefined(messageScope)) {
                messageScope.users = undefined;
            }
        }

        $rootScope.$on('product-view-loaded', function () {
            hide();
        });

        return {
            setup: setup,
            config: config,
            hide: hide,
            showSuccess: showSuccess,
            showWarning: showWarning,
            showNoChange: showNoChange,
            showError: showError,
            showSingle: showSingle,
            showMultiple: showMultiple
        };
    };

    fanexMessageService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexMessageService', fanexMessageService);
})();

'use strict';

(function () {
    var injectParams = ['$rootScope', 'fanexQueryModel'];
    var fanexModelService = function ($rootScope, QueryModel) {
        var queryModel = new QueryModel();
        $rootScope.$on('$locationChangeStart', function () {
            queryModel = new QueryModel();
            $rootScope.$emit('fanex-model-changed', queryModel);
        });

        return {
            getQueryModel: function () {
                return queryModel;
            }
        };
    };

    fanexModelService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexModelService', fanexModelService);
})();

'use strict';

(function () {
    var injectParams = ['fanexProductSettings'];
    var fanexProductSettingService = function (productSettings) {
        function getViewSetting(productId) {
            var viewSettings = productSettings.viewSettings;
            var viewSetting = viewSettings['default'];
            angular.forEach(viewSettings.products, function (productViewSetting) {
                if (productViewSetting.id === productId) {
                    viewSetting = productViewSetting;
                }
            });

            return viewSetting;
        }

        return {
            getViewSetting: getViewSetting,
            errorViewSetting: productSettings.viewSettings.error
        };
    };

    fanexProductSettingService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexProductSettingService', fanexProductSettingService);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexDirectiveLinker', 'fanexSelectBuilder'];
    var fanexPtService = function (appSettings, DirectiveLinker, SelectBuilder) {
        var service = new DirectiveLinker();

        service.copyValue = function (source, destination, field, generator) {
            destination[field].value = source[field].value;
            destination[field] = generator();
            var isCopied = source[field].value === destination[field].value;
            destination[field].valid = isCopied;
            return isCopied;
        };

        service.correctValue = function (min, max, value) {
            return Math.max(min, Math.min(max, value));
        };

        service.createSelect = function (min, max, value) {
            var selectBuilder = new SelectBuilder(1, 1);
            return selectBuilder.build(min, max, value);
        };

        return service;
    };

    fanexPtService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexPtService', fanexPtService);
})();

'use strict';

(function () {
    var injectParams = ['$injector', '$http', '$rootScope', 'fanexAppSettings', 'fanexProductSettingService', 'fanexMessageService'];
    var fanexRequestService = function ($injector, $http, $rootScope, appSettings, productSettingService, messageService) {
        var queryData;
        var commandData;

        function isSuccessResponse(response) {
            if (angular.isDefined(response.IsSuccess) && response.IsSuccess === false) {
                messageService.showError(response.ErrMsg);
                return false;
            }

            return true;
        }

        function query(queryModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Query';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    var productId = queryModel.ProductId;
                    var productViewSetting = productSettingService.getViewSetting(productId);
                    var QueryReader = $injector.get(productViewSetting.queryReader);
                    var queryReader = new QueryReader();
                    queryData = queryReader.read(response);
                    successCallback(queryData);
                }
            });

            return request;
        }

        function getQueryData() {
            return queryData;
        }

        function command(commandModel, successCallback) {
            var url = appSettings.rootUrl + '/CustomerSetting/Command';
            var request = $http.post(url, commandModel);
            request.success(function (response) {
                if (!isSuccessResponse(response)) {
                    return;
                }

                if (angular.isFunction(successCallback)) {
                    var productId = commandModel.ProductId;
                    var productViewSetting = productSettingService.getViewSetting(productId);
                    var CommandReader = $injector.get(productViewSetting.commandReader);
                    var commandReader = new CommandReader();
                    commandData = commandReader.read(response);
                    successCallback(commandData);
                }
            });

            return request;
        }

        function getCommandData() {
            return commandData;
        }

        return {
            query: query,
            getQueryData: getQueryData,
            command: command,
            getCommandData: getCommandData
        };
    };

    fanexRequestService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexRequestService', fanexRequestService);
})();

'use strict';

(function () {
    var injectParams = ['fanexResources', 'fanexAppSettings'];
    var fanexResourceService = function (resources, appSettings) {
        var resource = {};
        var currentProductName;

        function setCurrentProductName(productName) {
            currentProductName = productName;
        }

        function setLanguage(language) {
            var selectResource = resources[language.toLowerCase()];
            if (angular.isUndefined(selectResource)) {
                selectResource = resources[appSettings.defaultLanguage.toLowerCase()];
            }
            resource = selectResource;
        }

        function getString(key) {
            var value = resource[key];
            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                value = key;
            }
            return value;
        }

        function getFormatString(key) {
            var input = getString(key);
            var args = Array.prototype.slice.call(arguments, 1);
            return input.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        }

        function getDisplayName(settingLevel, data) {
            var liveName = data.IsLive === true ? ' (' + getString('Live') + ')' : '';
            switch (settingLevel) {
                case appSettings.settingLevel.Product:
                    return currentProductName;
                case appSettings.settingLevel.SportType:
                    return data.SportTypeName;
                case appSettings.settingLevel.BetType:
                    return data.BetTypeName + liveName;
                case appSettings.settingLevel.SportTypeBetType:
                    return data.BetTypeName + liveName;
            }
        }

        setLanguage(appSettings.defaultLanguage);
        return {
            setCurrentProductName: setCurrentProductName,
            setLanguage: setLanguage,
            getString: getString,
            getFormatString: getFormatString,
            getDisplayName: getDisplayName
        };
    };

    fanexResourceService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexResourceService', fanexResourceService);
})();

'use strict';

(function () {
    var injectParams = ['fanexDirectiveLinker'];
    var fanexStatusService = function (DirectiveLinker) {
        var service = new DirectiveLinker();
        return service;
    };

    fanexStatusService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexStatusService', fanexStatusService);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings', 'fanexRequestService'];
    var fanexTemplateService = function (appSettings, requestService) {
        var levelFolders = {};
        levelFolders[appSettings.settingLevel.Product] = 'no-group';
        levelFolders[appSettings.settingLevel.SportType] = 'no-group';
        levelFolders[appSettings.settingLevel.BetType] = 'no-group';
        levelFolders[appSettings.settingLevel.SportTypeBetType] = 'sport-group';

        var positionTakingFiles = {};
        positionTakingFiles[appSettings.formType.CompanyEditSuper] = 'fanex-pt-company-edit-super-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMaster] = 'fanex-pt-super-edit-master-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditAgent] = 'fanex-pt-master-edit-agent-container.htm';
        positionTakingFiles[appSettings.formType.SuperEditMember] = 'fanex-pt-super-edit-member-container.htm';
        positionTakingFiles[appSettings.formType.MasterEditMember] = 'fanex-pt-master-edit-member-container.htm';
        positionTakingFiles[appSettings.formType.AgentEditMember] = 'fanex-pt-agent-edit-member-container.htm';

        var discountFiles = {};
        discountFiles[appSettings.formType.CompanyEditSuper] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMaster] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.MasterEditAgent] = 'fanex-discount-container.htm';
        discountFiles[appSettings.formType.SuperEditMember] = 'fanex-discount-super-edit-member-container.htm';
        discountFiles[appSettings.formType.MasterEditMember] = 'fanex-discount-master-edit-member-container.htm';
        discountFiles[appSettings.formType.AgentEditMember] = 'fanex-discount-container.htm';

        this.resolveStatus = function () {
            var queryData = requestService.getQueryData();
            var settingContext = queryData.SettingContext;
            var levelFolder = levelFolders[settingContext.StatusLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/fanex-status-container.htm'
            : undefined;
        };

        this.resolveDiscount = function () {
            var queryData = requestService.getQueryData();
            var settingContext = queryData.SettingContext;
            var requestContext = queryData.RequestContext;
            var levelFolder = levelFolders[settingContext.DiscountLevel];
            var fileName = discountFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };

        this.resolveBetLimit = function () {
            var queryData = requestService.getQueryData();
            var settingContext = queryData.SettingContext;
            var levelFolder = levelFolders[settingContext.BetLimitLevel];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/fanex-bet-limit-container.htm'
            : undefined;
        };

        this.resolvePositionTaking = function () {
            var queryData = requestService.getQueryData();
            var settingContext = queryData.SettingContext;
            var requestContext = queryData.RequestContext;
            var levelFolder = levelFolders[settingContext.PositionTakingLevel];
            var fileName = positionTakingFiles[requestContext.FormType];
            return angular.isDefined(levelFolder)
            ? 'templates/' + levelFolder + '/' + fileName
            : undefined;
        };
    };

    fanexTemplateService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexTemplateService', fanexTemplateService);
})();

'use strict';

(function () {
    var injectParams = ['$timeout'];
    var fanexTooltipService = function ($timeout) {
        var tooltipScope;
        var tooltipElement;
        var tooltipAlign;
        var boundingElement;

        function setup(scope, element) {
            tooltipScope = scope;
            tooltipElement = element;
            tooltipElement.addClass('ng-hide');
        }

        function calculatePosition() {
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
        }

        function show(element, message, align) {
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
                calculatePosition();
            }
        }

        function hide() {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (!tooltipElement.hasClass('ng-hide')) {
                tooltipElement.addClass('ng-hide');
            }
        }

        return {
            setup: setup,
            calculatePosition: calculatePosition,
            show: show,
            hide: hide
        };
    };

    fanexTooltipService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexTooltipService', fanexTooltipService);
})();

'use strict';

(function () {
    var injectParams = ['fanexPtService'];
    var fanexUpdateMemberPtService = function (ptService) {
        var $scope;

        function setup(scope) {
            $scope = scope;
        }

        function hide() {
            $scope.isShow = false;
        }

        function show(memberPts) {
            var pts = [];
            angular.forEach(memberPts, function (memberPt) {
                var ptSelect = ptService.createSelect(0, memberPt.Pt, memberPt.Pt);
                pts.push({
                    sportType: memberPt.SportType,
                    betType: memberPt.BetType,
                    isLive: memberPt.IsLive,
                    name: memberPt.Name,
                    value: ptSelect.value,
                    options: ptSelect.options
                });
            });
            $scope.isShow = true;
            $scope.pts = pts;
        }

        return {
            setup: setup,
            show: show,
            hide: hide
        };
    };

    fanexUpdateMemberPtService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexUpdateMemberPtService', fanexUpdateMemberPtService);
})();

'use strict';

(function () {
    var body = angular.element(document.body);
    var rootUrl = body.attr('data-root-url');
    angular.module('customerSettingApp').value('fanexAppSettings', {
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
        commandType: {
            All: 0,
            PositionTaking: 1,
            BetLimit: 2,
            Discount: 3,
            Status: 4
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

'use strict';

(function () {
    angular.module('customerSettingApp').value('fanexProductSettings', {
        viewSettings: {
            'default': {
                controller: 'FanexDefaultController',
                templateUrl: 'templates/fanex-default-page.htm',
                queryReader: 'fanexQueryReader',
                commandReader: 'FanexCommandReader'
            },
            error: {
                controller: 'FanexErrorController',
                templateUrl: 'templates/fanex-error-page.htm'
            },
            products: [
                {
                    id: 0,
                    name: 'demo',
                    templateUrl: 'templates/fanex-demo-page.htm'
                }
            ]
        }
    });
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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
        DuplicateValuesAlertMsg: "Out of range for highlighted values.",
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

    angular.module('customerSettingApp').value('fanexResources', resources);
})();
