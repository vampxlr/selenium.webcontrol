(function () {
    'use strict';
    
    var app = angular.module('app', [
        // Angular modules 
        'ngAnimate',        // animations
        'ngRoute',          // routing
        'ngSanitize',       // sanitizes html bindings (ex: sidebar.js)

        // Custom modules 
        'common',           // common functions, logger, spinner
        'common.bootstrap', // bootstrap dialog wrapper functions

        // 3rd Party Modules
        'ui.bootstrap',      // ui-bootstrap (ex: carousel, pagination, dialog)
        //custom filters
        'unique',
        'removeuser',
        'removeproject',
        'userprojectmodalbyid',
        'uiDate'
    ]);
    
    // Handle routing errors and success events
    app.run(['$route', '$rootScope', '$q',
         function ($route, $rootScope, $q) {
             // Include $route to kick start the router.
             breeze.core.extendQ($rootScope, $q);
         }]);
    
    app.directive('rocky', function () {
        var directive = {};
       
        directive.restrict = 'E'; /* restrict this directive to elements */

        directive.template = "My first directive: {{ello}}";

        return directive;
    });


})();