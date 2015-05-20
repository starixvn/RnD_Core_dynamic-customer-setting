/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<ol><li data-ng-repeat=\"line in lines\" data-ng-switch on=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-bind=line.Name data-ng-click=show(line.Name)></a> <span ng-switch-when=true data-ng-bind=line.Name></span></li></ol>"
  );

} ]);

/*jshint -W101*/