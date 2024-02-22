var myApp = angular.module('myApp');

myApp.controller('RegisterController', function ($scope, $http, $state, BASE_URL, ngNotify) {

    $scope.userData = {
        uname: "",
        email: "",
        password: "",
        confirmPassword: "",
    }

   

    $scope.register = function (dd) {


        var user = $scope.userData;

        console.log(user, dd);

        if (dd.password !== dd.confirmPassword) {

            ngNotify.set('Both password should be same! ', {
                type: 'error'
            });
            return;
        }


        $http.post(BASE_URL + "User/Register", {

            "UserName": user.uname,
            "UserEmail": user.email,
            "UserPassword": user.password
        })
        .then(function (response) {
            console.log("Register successful:", response.data);
            ngNotify.set('Register successful ! ', {
                type: 'success'
            });

            $state.go('home');
        })
        .catch(function (error) {
            console.error("Registration failed:", error);
            ngNotify.set('Register failed ! ' + error.data, {
                type: 'error'
            });
        });
    }
});
