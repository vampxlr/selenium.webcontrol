(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', 'datacontext', '$http', dashboard]);

    function dashboard(common, datacontext,$http) {
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
      
        vm.getFirefox = function getFirefox() {
           
            $http({ method: 'GET', url: '/api/webapi/GetFirefox'});
        }
        vm.RegisterAndLogin = function RegisterAndLogin() {
            vm.RegisterDisable = true;
         
            $http({ method: 'GET', url: '/api/webapi/RegisterAndLogin' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Created User And Logged In ' + data);
                          vm.LoggedInAs = data;
                          console.log(data);
                          vm.RegisterDisable = false;
                          vm.LogOutDisable = false;
                          vm.placeOrderDisable = false;
                      });

        }
        vm.GetALlEmailList = function GetALlEmailList() {
            vm.loading = "Loading...";
            $http({ method: 'GET', url: '/api/webapi/getEmailList' }).then(function (data) {
                console.log(data.data);
                vm.loading = "";
                vm.Emailist = data.data;
                
            });

        }

        vm.GetALlEmailList();
        vm.Login = function Login(data) {
            console.log("inside login function");
            console.log(data);
            vm.LoginDisable = true;
            $http({ method: 'POST', url: '/api/webapi/LoginUsingEmail', data: data })
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
        vm.placeOrderRandom = function placeOrderRandom() {
          
            $http({ method: 'GET', url: '/api/webapi/placeOrderRandom' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Placed Order');
                         
                      });


        }
    }
})();