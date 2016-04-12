angular.module('removeuser', [])
    .directive('removeuser',
    function () {
        return {
            restrict: 'E',
            scope: {
                id: '=userid',
                users:'=users'
            },
            templateUrl: "app/templates/directives/removeuser.html",
            controller: function ($scope, $http, $q, $route, datacontext, $timeout) {
                $scope.removeuser = [];
                var id = $scope.id;

                

             $scope.deleteUser= function ProjectsByUserId() {
                    var id = $scope.id;
                    console.log('fire');
                    return $http({ method: 'DELETE', url: '/api/loginapi/removeuser/' + id }).then(DelayLoad());
                    
             }



             function DelayLoad() {
                
                 $timeout(function () { $scope.getUsers() }, 600);
             }

             $scope.getUsers=   function getUsers() {
               return datacontext.getUsers().then(function (data) {



                     return $scope.users = data;
                 });
             }
            }
        }
    }
);