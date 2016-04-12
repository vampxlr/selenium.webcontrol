(function () {
    'use strict';
    var controllerId = 'dblogin';
    angular.module('app').controller(controllerId, ['common', 'datacontext', '$http', '$timeout', dblogin]);

    function dblogin(common, datacontext, $http, $timeout) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
       
        var vm = this;
        vm.RegisterDisable = false;
        vm.title = 'DB-login';
        vm.LoggedInAs = "Not Logged In";
        activate();
        vm.LoginDisable = false;
        vm.LogOutDisable = true;
        vm.placeOrderDisable = true;
        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated dblogin View'); });
        }
        vm.PlaceOrderStockId = 1;
        vm.getFirefox = function getFirefox() {
            $http({ method: 'GET', url: '/api/webapi/GetFirefox'});
        }
        vm.RegisterAndLogin = function RegisterAndLogin() {
            vm.RegisterDisable = true;
            vm.LoginDisable = true;
            $http({ method: 'GET', url: '/api/webapi/RegisterAndLogin' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Created User And Logged In ' + data);
                          vm.LoggedInAs = data;
                          console.log(data);
                          vm.RegisterDisable = false;
                          vm.LogOutDisable = false;
                          vm.placeOrderDisable = false;
                          vm.GetALlEmailList();
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
        vm.placeOrderRandomFallHoliday = function placeOrderRandomFallHoliday() {
          
            $http({ method: 'GET', url: '/api/webapi/placeOrderRandomFallHoliday' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Placed Order');
                         
                      });


        }
        vm.placeOrderRandomCommunion = function placeOrderRandomCommunion() {

            $http({ method: 'GET', url: '/api/webapi/placeOrderRandomCommunion' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Placed Order');

                      });


        }
        vm.placeOrderRandomFlowerGirl = function placeOrderRandomFlowerGirl() {

            $http({ method: 'GET', url: '/api/webapi/placeOrderRandomFlowerGirl' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Placed Order');

                      });


        }
        vm.placeOrderRandomSpringHoliday = function placeOrderRandomSpringHoliday() {

            $http({ method: 'GET', url: '/api/webapi/placeOrderRandomSpringHoliday' })
                      .success(function (data, status, headers, config) {
                          log('Successfully Placed Order');

                      });


        }

        vm.placeOrderRandomAll = function () {

            $http({ method: 'GET', url: '/api/webapi/placeOrderRandomAll' })
                 .success(function (data, status, headers, config) {
                     log('Successfully Logged Stock List');
                     vm.GetAllStock();
                 });
        }
    
        vm.GetAllStock = function () {

            $http({ method: 'GET', url: '/api/webapi/GetAllStock' })
                 .success(function (data, status, headers, config) {
                     log('Stocks Loaded');
                     vm.stocks = data;
                     console.log(data);
                 });
        }
        vm.GetAllStock();
        

        vm.placeOrderByStockId = function (id) {
            vm.PlaceOrderStockId = id;
            $http({ method: 'GET', url: '/api/webapi/placeOrderByStockId/' + vm.PlaceOrderStockId })
                 .success(function (data, status, headers, config) {
                     console.log(data);
                     if (data=="true") {
                         vm.openModalCCdetails();
                         log('Successfully Placed Order To Cart');
                     }
                     if (data=="false") {
                         log('Order is Out Of Stock');
                         log('Updating data');
                         DelayLoad();
                     };
                 });
        }

        function DelayLoad() {

            $timeout(function () { vm.GetAllStock(); log('Data Updated'); }, 600);


        }

        vm.openModalCCdetails = function () {
            log('Please Enter Billing Information');
            $('#myModal').modal('show');
        }
        vm.buttondisable = "false";
        vm.enterBillingInformation = function () {
           var postData = {
                firstName: vm.firstName,
                lastName: vm.lastName,
                BillingAddress1: vm.BillingAddress1,
                BillingAddress2: vm.BillingAddress2,
                city: vm.city,
                phone: vm.phone,
                state: vm.state,
                expirationMonth: vm.expirationMonth,
                expirationYear: vm.expirationYear,
                ccType: vm.ccType,
                ccNumber: vm.ccNumber,
                cvv: vm.cvv,
                shipping: vm.shipping,
                zip:vm.zip
            };
            $http({ method: 'POST', url: '/api/webapi/enterBillingInformation', data: postData })
                 .success(function (data, status, headers, config) {
                  
                 });
            
        }

        vm.enterBillingInformationAsGuest = function () {
            var postData = {
                firstName: vm.firstName,
                lastName: vm.lastName,
                BillingAddress1: vm.BillingAddress1,
                BillingAddress2: vm.BillingAddress2,
                city: vm.city,
                phone: vm.phone,
                state: vm.state,
                expirationMonth: vm.expirationMonth,
                expirationYear: vm.expirationYear,
                ccType: vm.ccType,
                ccNumber: vm.ccNumber,
                cvv: vm.cvv,
                shipping: vm.shipping,
                zip: vm.zip
            };
            $http({ method: 'POST', url: '/api/webapi/enterBillingInformationAsGuest', data: postData })
                 .success(function (data, status, headers, config) {

                 });

        }
        
    }
})();