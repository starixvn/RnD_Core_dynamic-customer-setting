/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-block-ui.htm',
    "<div class=loading-container data-ng-show=hasLoading><div class=block-overlay></div><div class=block-loading></div></div>"
  );


  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<div class=breadcrumb-container><ol><li data-ng-repeat=\"line in lines\" data-ng-switch=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-click=selectUserName(line.Name)>{{line.Name}} ({{line.Id}})</a> <span ng-switch-when=true>{{line.Name}} ({{line.Id}})</span></li></ol></div>"
  );


  $templateCache.put('templates/fanex-commission-matrix.htm',
    "<div class=group data-ng-repeat=\"table in tables\"><div data-ng-show=table.Name class=group-name>{{table.Name}}</div><div class=table><table><colgroup><col width=\"80\"><col width=50 data-ng-repeat=\"column in table.TotalOfColumn\"></colgroup><thead><tr><th></th><th data-ng-repeat=\"column in table.Columns\">{{column.Name}}</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.Name}}</td><td data-ng-repeat=\"column in row.Columns\">{{column.Value}}</td></tr></tbody></table></div></div>"
  );


  $templateCache.put('templates/fanex-form.htm',
    "<div class=form-container><div class=controls><label for=product>Product</label><select name=product data-ng-model=product data-ng-options=\"product.Name for product in products\"></select></div><div class=controls><label for=userName>Username</label><input name=userName data-ng-model=\"userName\"></div><div class=controls><button data-ng-click=submit()>Submit</button></div></div>"
  );


  $templateCache.put('templates/fanex-header.htm',
    "<div class=header-container><ul class=navigation><li data-ng-repeat=\"navigationItem in navigationItems\" data-ng-click=activeNavigationItem(navigationItem) data-ng-class=\"{'active': selectedNavigationItem==navigationItem}\"><a href=#>{{navigationItem.Text}}</a></li></ul></div>"
  );


  $templateCache.put('templates/fanex-page.htm',
    "<div data-fanex-header></div><div data-fanex-form></div><div class=message-container data-ng-show=message data-ng-bind=message></div><div data-ng-show=customer data-fanex-breadcrumb></div><div class=result-container data-ng-show=settings data-ng-switch=selectedNavigationItem.Value><div data-ng-switch-when=0 data-fanex-position-taking-matrix></div><div data-ng-switch-when=1 data-fanex-commission-matrix></div></div>"
  );


  $templateCache.put('templates/fanex-position-taking-matrix.htm',
    "<div class=group data-ng-repeat=\"group in groups\"><div class=group-name>{{group.Name}}</div><div class=table data-ng-repeat=\"table in group.Tables\"><div class=table-name>{{table.Name}}</div><table><colgroup><col width=\"80\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"></colgroup><thead><tr><th></th><th>Super</th><th>Master</th><th>Agent</th><th>US</th><th>IS</th><th>UM</th><th>IM</th><th>N</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.RoleId | fanexRoleFormat}}</td><td>{{row.SuperPT | fanexPtFormat}}</td><td>{{row.MasterPT | fanexPtFormat}}</td><td>{{row.AgentPT | fanexPtFormat}}</td><td>{{row.AutoSuperPT | fanexPtFormat}}</td><td data-ng-switch=\"row.IsAutoSuperPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoSuperPT disabled></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\"><div data-ng-switch-when=true><div class=icon-auto-pt></div></div><div data-ng-switch-when=false>{{row.AutoMasterPT | fanexPtFormat}}</div></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\" data-ng-class=\"{'auto-pt': row.RoleId==3 && !disableAutoMPT}\"><div data-ng-switch-when=false><div data-ng-switch=\"row.IsAutoMasterPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoMasterPT disabled></div></div></td><td>{{row.MinPT | fanexPtFormat}}</td></tr></tbody></table></div></div>"
  );

} ]);

/*jshint -W101*/