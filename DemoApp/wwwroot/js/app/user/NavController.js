
var myApp = angular.module('myApp');



myApp.controller('NavController', function ($scope, $http, $state, AuthService, UserService, ngNotify) {

    $scope.userData = {};
    $scope.isAdmin = false;
    $scope.isLogin = false;
    $scope.userName = false;
   
    // Listen for userDataChanged event
    $scope.$on('userDataChanged', function (event, data) {
        // Update userData and isAdmin when the event is received

        $scope.userData = data;
        $scope.isAdmin = $scope.userData.roles.includes('Admin');
        $scope.isLogin =  ( data.userName != '' ) ? true : false ;
        $scope.userName = (data.userName !== '') ? $scope.userData.userName : false;

    });


    $scope.logout = function () {
        

      

        var userData = {
            userName: '',
            userId: '',
            roles: [],
            userEmail: ''
        };

        UserService.setUserData(userData);

        $state.go('login');


        AuthService.logout();

        ngNotify.set('logged OUT! ', {
            type: 'success'
        });

        // Redirect to the login page or any other desired page
       

    }


});