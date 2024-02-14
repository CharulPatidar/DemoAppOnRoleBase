
var myApp = angular.module('myApp');

myApp.controller('DeleteController', function ($scope, $uibModal, $uibModalInstance, $http, $state, $window, BASE_URL, note) {

    console.log("DeleteController -- > Notes ", note);

    $scope.note = note;

    $scope.ok = function (note) {
        console.log(note);
        var token = localStorage.getItem('token'); // Assuming token is stored in localStorage

        $http.delete(BASE_URL + 'Manager/DeleteNoteById', {
            headers: {
                'Content-Type': 'application/json' // Set Content-Type header
            },
            data: {
                "UserId": token,
                "NoteId": note.id
            }
        })
            .then(function (response) {
                // Success callback
                console.log('delete request successful:', response.data);

            })
            .catch(function (error) {
                // Error callback
                console.error('delete request failed:', error);
            });



        $uibModalInstance.close(note);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});
