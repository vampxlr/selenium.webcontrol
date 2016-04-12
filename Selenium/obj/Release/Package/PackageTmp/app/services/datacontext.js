(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId, ['common', 'entityManagerFactory', datacontext]);

    function datacontext(common, emFactory) {
        var EntityQuery = breeze.EntityQuery;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId);
        var logError = getLogFn(serviceId, 'error');
        var logSuccess = getLogFn(serviceId, 'success');
        var manager = emFactory.newManager();


        var $q = common.$q;
        var service = {
            getPeople: getPeople,
            getMessageCount: getMessageCount,
            getProjects: getProjects,
            getUserProjects: getUserProjects,
            getUsers: getUsers,
            postandGetUserProjects: postandGetUserProjects,
            getUnassignedProjectsByUserId: getUnassignedProjectsByUserId
        };

        return service;

        function getMessageCount() { return $q.when(72); }

        function getPeople() {
            $http({ method: 'GET', url: '/api/loginapi/project' })
                      .success(function (data, status, headers, config) {
                         
                          return $q.when(data);
                      });
        }
        vm.people = [];

        function getProjects() {
            var projects=[];

            return EntityQuery.from('projects')
            .select('ProjectName, ProjectId,RosterWork,RosterBreak,Location,ProjectNumber,Duration')
            .using(manager).execute()
            .to$q(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                projects = data.results;
              
                log('Retrived [Projects Partials] from remote data source ', projects.length, true);
                return projects;
            }


        }

        function getUsers() {
            var orderBy = 'username , userrole';
            var users;

            return EntityQuery.from('users').expand('UserProjects')
            .using(manager).execute()
            .to$q(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                users = data.results;
                
                log('Retrived [Users Partials] from remote data source ', users.length, true);
                return users;
            }
        }

        function getUserProjects() {
            var orderBy = 'username , userrole';
            var UserProjects;



            return EntityQuery.from('UserProjects').expand('user,project')
            .using(manager).execute()
            .to$q(querySucceeded, _queryFailed);

            function querySucceeded(data) {
                UserProjects = data.results;
               
                log('Retrived [UserProjects Partials] from remote data source ', UserProjects.length, true);
                return UserProjects;
            }
        }

        function _queryFailed(error) {
            var msg = config.appErrorPrefix + 'Error retreiving data.' + error.message;
            logError(msg, error);
            throw error;
        }

        function postandGetUserProjects() {
            $http({ method: 'GET', url: '/api/loginapi/UserProjects/'})
                      .success(function (data, status, headers, config) {
                         
                          return $q.when(data);
                      });
        }

        function getUnassignedProjectsByUserId() {

            $http({ method: 'GET', url: '/api/loginapi/UserProjects/' })
                    .success(function (data, status, headers, config) {
                        console.log(data);
                        return $q.when(data);
                    });
        }

    }
})();