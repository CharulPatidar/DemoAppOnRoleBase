
var myApp = angular.module('myApp');

myApp.controller('UserProfileController', function ($scope, $http, $state, $window, UserService, BASE_URL) {
    console.log("UserProfileController");

     var userData = UserService.getUserData();
 
    $scope.userName = userData.userName;
    $scope.userEmail = userData.userEmail;
    $scope.userRole = userData.roles


    $scope.notes;

    $scope.getNotes = function () {
        
        $http.get(BASE_URL + 'User/GetNotesByUserId')
            .then(function (response) {
                $scope.notes = response.data.$values;
                console.log("GetNotes successful:", $scope.notes);

                if ($scope.notes.length < 1) {
                    $scope.notes = [{ id: 'NA', topic: 'NA', description: 'NA' }];
                }
            })
            .catch(function (error) {
                console.error("GetNotes failed:", error);
            });

    };

    $scope.getNotes();

});