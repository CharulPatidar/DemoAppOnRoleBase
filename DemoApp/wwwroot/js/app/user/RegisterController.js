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
        debugger;
        console.log(user, dd);

        $http.post(BASE_URL + "User/Register", {

            "userName": user.uname,
            "userEmail": user.email,
            "userPassword": user.password
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
        });
    }
});
