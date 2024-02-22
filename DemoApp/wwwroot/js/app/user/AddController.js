
var myApp = angular.module('myApp');

myApp.controller('AddController', function ($scope, $uibModal, $uibModalInstance, $http, $state, $window, BASE_URL) {

    console.log("AddController -- > Notes ");

    $scope.note;


    $scope.ok = function (note) {

        console.log(note);
        var token = localStorage.getItem('token'); // Assuming token is stored in localStorage


        console.log(token)

        $http.post(BASE_URL + 'Note/AddNotes',
            {
                "NoteTopic": note.topic,
                "NoteDescription": note.description
            })
            .then(function (response) {
                // Success callback
                console.log('POST request successful:', response.data);
               
            })
            .catch(function (error) {
                // Error callback
                console.error('POST request failed:', error);
            });






        $uibModalInstance.close(note);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});
