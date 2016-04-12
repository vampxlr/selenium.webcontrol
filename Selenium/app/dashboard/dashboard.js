(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', 'datacontext', '$http', '$timeout', dashboard]);

    function dashboard(common, datacontext, $http, $timeout) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
       
        var vm = this;
        vm.RegisterDisable = false;
        vm.title = 'Dashboard';
        vm.LoggedInAs = "Not Logged In";
        activate();
        vm.LoginDisable = false;
        vm.LogOutDisable = true;
        vm.placeOrderDisable = true;
        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Dashboard View'); });
        }
        vm.PlaceOrderStockId = 1;
       
        
   
        vm.Login = function Login() {
           
            data = {
                Email: vm.email,
                Password:vm.password

            };

            console.log("inside login function");
            console.log(data);
            vm.LoginDisable = true;
            $http({ method: 'POST', url: '/api/webapi/ManualLogin', data: data })
                      .success(function (data, status, headers, config) {
                          log('Successfully Logged In ' + data);
                          vm.LoggedInAs = data;
                          console.log(data);
                      
                          vm.LogOutDisable = false;
                          vm.placeOrderDisable = false;
                      });

        }
        vm.LogOut = function LogOut() {
            vm.LogOutDisable = false;
            vm.LoginDisable = true;
            $http({ method: 'GET', url: '/api/webapi/LogOut' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Logged Out');
                          vm.LoggedInAs = "Not Logged In";
                          console.log(data);
                          vm.LogOutDisable = true;
                          vm.LoginDisable = false;
                          vm.placeOrderDisable = true;
                      });

        }
      
        vm.ManualRegister = function (data) {

            data = {
                Email: vm.email,
                Password: vm.password

            };

            $http({ method: 'POST', url: '/api/webapi/ManualRegister',data:data })
                     .success(function (data, status, headers, config) {
                         log('Successfully  Logged In ' + data);
                   
                     });

        }





        function DelayLoad() {

            $timeout(function () { vm.GetAllStock(); log('Data Updated'); }, 600);


        }

        vm.buttondisable = "false";
   
        
    }
})();