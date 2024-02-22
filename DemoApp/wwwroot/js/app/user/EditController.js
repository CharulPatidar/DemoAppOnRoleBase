var myApp = angular.module('myApp');

myApp.controller('EditController', function ($scope, $uibModal, $uibModalInstance, $http, $state, $window, BASE_URL, note) {

    console.log("EditController -- > Notes ", note);

    $scope.note = note;

    console.log($uibModalInstance);
    $scope.note ; // Set note in the scope

    $scope.ok = function (note) {
        console.log(note);
        var token = localStorage.getItem('token'); // Assuming token is stored in localStorage
       

        console.log(token)

        $http.put(BASE_URL + 'Note/UpdateNoteById',
            {
              
                "NoteId": note.id,
                "NoteTopic": note.topic,
                "NoteDescription": note.description
            })
            .then(function (response) {
                // Success callback
                console.log('put request successful:  ', response.data);
                $state.go('notes');

            })
            .catch(function (error) {
                // Error callback
                console.error('put request failed:  ', error);
            });






        $uibModalInstance.close(note);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});
