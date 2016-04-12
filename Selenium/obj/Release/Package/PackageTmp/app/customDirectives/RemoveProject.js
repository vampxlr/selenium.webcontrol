angular.module('removeproject', [])
    .directive('removeproject',
    function () {
        return {
            restrict: 'E',
            scope: {
                id: '=projectid',
                projects:'=projects'
            },
            templateUrl: "app/templates/directives/removeProject.html",
            controller: function ($scope, $http, $q, $route, datacontext, $timeout) {
                $scope.removeproject = [];
                var id = $scope.id;

                

                $scope.deleteProject = function deleteProjectByProjectId() {
                    var id = $scope.id;
                    console.log('fire');
                    
                    
                    return $http({ method: 'DELETE', url: '/api/loginapi/removeproject/' + id }).then(DelayLoad());
                    
             }



             function DelayLoad() {
                
                 $timeout(function () { $scope.getProjects() }, 600);
             }

             $scope.getProjects = function getProjects() {
                 return datacontext.getProjects().then(function (data) {

                     return $scope.projects = data;
                 });
             }
            }
        }
    }
);