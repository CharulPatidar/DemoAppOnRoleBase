

var myApp = angular.module('myApp', ['ui.router', 'ui.bootstrap', 'ngNotify' ]);


// Define AngularJS controller
myApp.controller('myController', function ($scope ,$state) {
    // Message for AngularJS binding

    $state.go('home');

    $scope.message = 'This is an AngularJS message.';
});



myApp.constant('BASE_URL', 'https://localhost:7128/api/'); // Define the base URL here





myApp.factory('AuthInterceptor', function ($rootScope, $state) {
    var authInterceptor = {};
    authInterceptor.request = function (config) {
        var token = localStorage.getItem('token');

        
        if (config.url === '/html/register.html') {
            return config;
        }

        if (token) {
            config.headers.Authorization = 'Bearer ' + token;
        } else {
            // Redirect to login page if token is not found
            $state.go('login');
        }
        return config;
    };

    authInterceptor.logout = function () {

        // Clear Authorization header
        delete $http.defaults.headers.common['Authorization'];
    };

    return authInterceptor;
});

myApp.factory('AuthService', function ($rootScope, AuthInterceptor) {

    var authService = {};

    authService.logout = function () {
        // Remove token from localStorage
        localStorage.removeItem('token');

        // Call logout function in AuthInterceptor
        AuthInterceptor.logout();

        // Broadcast logout event
        $rootScope.$broadcast('logoutEvent');


        // we can catch it by .... in any controller
        //$scope.$on('logoutEvent', function () {
        //    // Your code to handle the logout event
        //});

       
    };

    return authService;
});


myApp.service('UserService', function ($rootScope) {
     // Initialize an empty object to store user data

    var userData = {
        userName:'',
        userId: '',
        roles: [],
        userEmail: ''
    };

    return {
        setUserData: function (data) {
            userData = data; // Set user data
            // Broadcast an event when user data changes
            $rootScope.$broadcast('userDataChanged', userData);
        },
        getUserData: function () {
            return userData; // Get user data
        }
    };
});

myApp.factory('signalRService', function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/NotesHub").build();

    connection.start().then(() => {
        console.log("SignalR connection established");
    }).catch(err => console.error(err));

    // Return the connection object and the onReceiveMsg method
    return {
        connection: connection,
        onReceiveMsg: function (callback) {
            // Ensure that connection object is defined before trying to subscribe to events
            if (connection) {
                connection.on("ReceiveMsg", callback);
            } else {
                console.error("SignalR connection is not initialized.");
            }
        }
    };
});

