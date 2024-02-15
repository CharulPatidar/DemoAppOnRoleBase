var myApp = angular.module('myApp');

myApp.controller('LoginController', function ($scope, $http, $state, $window, BASE_URL, UserService, ngNotify ) {
   

    
    $scope.user = {
        email: "",
        password: "",
    };


    function setUserData() {

        $http.get(BASE_URL + "User/GetUserData")
            .then(function (response) {

                console.log("GetUserData successful:", response.data);

                var userData = {
                    userName: response.data.userName,
                    userId: response.data.userId,
                    roles: response.data.roles,
                    userEmail: response.data.userEmail
                };

                ngNotify.set('You have successfully logged in!  ' + userData.userName, {
                    type: 'success'
                });

                UserService.setUserData(userData);
               

            })
            .catch(function (error) {

                console.error("Login failed:", error);
            });


    }

    setUserData(); 
   
    $scope.login = function () {
        var user = $scope.user;

        $http.post(BASE_URL + "User/Login",
            {
                "userEmail": user.email,
                "userPassword": user.password
            })
            .then(function (response) {
               
                console.log("Login successful:", response.data);

               
                localStorage.setItem('token', response.data.token);

               
                $http.defaults.headers.common['Authorization'] = 'Bearer ' + response.data.token;

                setUserData();

               
                
                $state.go('home');
            })
            .catch(function (error) {
               
                console.error("Login failed :", error);
                ngNotify.set('logged in failed', 'error');

            });
    };


    $scope.register = function () {
        $state.go('register');
    }
});


