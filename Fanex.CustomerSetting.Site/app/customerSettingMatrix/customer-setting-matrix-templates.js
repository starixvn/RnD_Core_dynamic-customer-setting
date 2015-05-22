/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-block-ui.htm',
    "<div class=loading-container data-ng-show=hasLoading><div class=block-overlay></div><div class=block-loading></div></div>"
  );


  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<ol><li data-ng-repeat=\"line in lines\" data-ng-switch=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-click=show(line.Name)>{{line.Name}} ({{line.Id}})</a> <span ng-switch-when=true>{{line.Name}} ({{line.Id}})</span></li></ol>"
  );


  $templateCache.put('templates/fanex-position-taking-matrix.htm',
    "<div class=group data-ng-repeat=\"group in groups\"><div class=group-name>{{group.Name}}</div><div class=table data-ng-repeat=\"table in group.Tables\"><div class=table-name>{{table.Name}}</div><table><colgroup><col width=\"80\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"><col width=\"50\"></colgroup><thead><tr><th></th><th>Super</th><th>Master</th><th>Agent</th><th>US</th><th>IS</th><th>UM</th><th>IM</th><th>N</th></tr></thead><tbody><tr data-ng-repeat=\"row in table.Rows\"><td>{{row.RoleId | fanexRoleFormat}}</td><td>{{row.SuperPT | fanexPtFormat}}</td><td>{{row.MasterPT | fanexPtFormat}}</td><td>{{row.AgentPT | fanexPtFormat}}</td><td>{{row.AutoSuperPT | fanexPtFormat}}</td><td data-ng-switch=\"row.IsAutoSuperPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoSuperPT disabled></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\"><div data-ng-switch-when=true><div class=icon-auto-pt></div></div><div data-ng-switch-when=false>{{row.AutoMasterPT | fanexPtFormat}}</div></td><td data-ng-switch=\"row.RoleId==3 && !disableAutoMPT\" data-ng-class=\"{'auto-pt': row.RoleId==3 && !disableAutoMPT}\"><div data-ng-switch-when=false><div data-ng-switch=\"row.IsAutoMasterPT !== undefined\"><input data-ng-switch-when=true type=checkbox value=row.IsAutoMasterPT disabled></div></div></td><td>{{row.MinPT | fanexPtFormat}}</td></tr></tbody></table></div></div>"
  );

} ]);

/*jshint -W101*/