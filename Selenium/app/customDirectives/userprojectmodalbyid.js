angular.module('userprojectmodalbyid', [])
    .directive('userprojectmodalbyid',
    function () {
        return {
            restrict: 'E',
            scope: {
                p: '=user'
            },
            templateUrl: "app/templates/directives/userprojectmodalbyid.html",
            controller: function ($scope, $http, $q, $timeout) {
                $scope.assignedprojects = [];
                $scope.unassignedprojects = [];
                var id = $scope.p.userId;

               

               function AssignedProjectsByUserId() {

                    return $http({ method: 'GET', url: '/api/loginapi/AssignedProjectsByID/' + $scope.p.userId });
                            
                }
            
               $scope.AssignedProjectsByUserId = function () {
                   AssignedProjectsByUserId().then(function (result) {
                       console.log("assignedprojets");
                       console.log(result.data);
                       return $scope.assignedprojects = result.data;
                   });
               }
               $scope.AssignedProjectsByUserId();


               function unAssignedProjectsByUserId() {
                   
                   return $http({ method: 'GET', url: '/api/loginapi/unAssignedProjectsByID/' + $scope.p.userId });

                }

               $scope.unAssignedProjectsByUserId = function () {
                   unAssignedProjectsByUserId().then(function (result) {
                      
                        $scope.unassignedprojects = result.data;
                        selectionProcess();
                   });
               }
               $scope.unAssignedProjectsByUserId();


               $scope.UserId = $scope.p.userId;
               $scope.disable = false;

                $scope.AddUserToProject = function AddUserToProject() {
                    $scope.disable = true;
                    console.log("Fire AddUserToProject");
                    
                    $scope.postData = { userId: $scope.UserId, projectId: $scope.selectedOptionUnAssignedProjects.Id };
                    return $http({ method: 'POST', url: '/api/loginapi/AddUserToProject', data: $scope.postData })
                        .then(DelayLoad());

                }
                $scope.RemoveUserFromProject = function RemoveUserFromProject(Project_id) {

                    console.log("Fire RemoveUserFromProject");
                    console.log(Project_id);
                    $scope.DeleteData = { userId: $scope.UserId, projectId: Project_id };
                    return $http({ method: 'POST', url: '/api/loginapi/RemoveUserFromProject', data: $scope.DeleteData })
                        .then(DelayLoad());

                }
             
               function selectionProcess() {
                    $scope.selectedOptionUnAssignedProjects = $scope.unassignedprojects[0];
                    console.log("Selection Process");
                    console.log($scope.selectedOptionUnAssignedProjects);
                }
               

                function DelayLoad() {

                    $timeout(function () { $scope.unAssignedProjectsByUserId(); $scope.disable = false; }, 600);

                    $timeout(function () { $scope.AssignedProjectsByUserId(); $scope.disable = false; }, 600);
                    
                }
              





            }
           

        }



    }
);