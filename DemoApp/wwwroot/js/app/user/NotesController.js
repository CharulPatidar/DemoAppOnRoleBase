

var myApp = angular.module('myApp');



myApp.controller('NotesController', function ($scope, $http, $uibModal, $state, $stateParams, $window, BASE_URL, $location, UserService, signalRService, ngNotify) {
    
    $scope.notes = null;
    $scope.isInsert = false
    $scope.isEdit = false;
    $scope.isDelete = false;

    $scope.userData = UserService.getUserData();
    $scope.isInsert = !$scope.userData.roles.some(role => ['Intern'].includes(role));
    $scope.isEdit = !$scope.userData.roles.some(role => ['Intern', 'Developer'].includes(role));
    $scope.isDelete = !$scope.userData.roles.some(role => ['Intern', 'Developer','TeamLead'].includes(role));

    var queryString = $location.search();
    var notesValue = queryString.notes;

    if (notesValue != null) {
        alert("Notes--> " + notesValue);

    }



  

    // Fetch notes using an IIFE
    (function getNotes() {
        $http.get(BASE_URL + 'User/GetAllNotes')
            .then(function (response) {
                $scope.notes = response.data.$values;
                console.log("GetNotes successful:", $scope.notes);
            })
            .catch(function (error) {
                console.error("GetNotes failed:", error);
            });
    })();




    // Normal function
    $scope.getNotes = function () {



        $http.get(BASE_URL + 'User/GetAllNotes')
            .then(function (response) {
                $scope.notes = response.data.$values;
                console.log("GetNotes successful:", $scope.notes);

                var notesData = $scope.notes
               // openNewTab(notesData);
            })
            .catch(function (error) {
                console.error("GetNotes failed:", error);
            });

    };

    signalRService.connection.on("ReceiveMsg", function (n) {

        var noteObject = JSON.parse(n);
        console.log("Received Note: " + n);

        $scope.getNotes();
        ngNotify.set(' New Note Added' + noteObject.topic, {
            type: 'success'
        });
    });

    // Function to open new tab with notes data
    function openNewTab(notesData) {


        // Generate the URL with the parameters
        var url = $state.href($state.current.name, { notes: JSON.stringify(notesData) });
        
        // Open the new tab with the generated URL
        var newTab = $window.open(url, '_blank');
    }


    $scope.openEditModal = function (note) {
        var modalInstance = $uibModal.open({
            animation: true,
            size: '100%',
            //template: '<h1>Error loading template!</h1>', // Custom template in case templateUrl fails
            templateUrl: '/html/edit.html', // URL to your modal template
            controller: 'EditController',
            resolve: {
                note: function () {
                    return note;                 }
            }
        });

        modalInstance.result.then(function (result) {
            // Do something with the result if needed

            console.log("modalInstance Result -- > ", result);



            // Find the index of the edited note in the $scope.notes array
            var index = $scope.notes.findIndex(function (note) {
                return note.id === result.id; // Assuming result contains the editedNote
            });

            // If the edited note is found in the array
            if (index !== -1) {
                // Update the note in the array with the edited note
                $scope.notes[index] = result;
                ngNotify.set('Note Edited' + note.topic, {
                    type: 'info'
                });
            }

          


        })
            .catch(function (error) {

                console.log("modalInstance Error --> ",error);

        });
    }; // openEditModal

    $scope.openAddNoteModel = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            size: '100%',
            templateUrl: '/html/addNote.html', 
            controller: 'AddController',
        });

        modalInstance.result.then(function (result) { 

            console.log("modalInstance Result -- > ", result);

            $scope.notes.push(result);
            ngNotify.set('Note Added' + note.topic, {
                type: 'info'
            });
           
        })
        .catch(function (error) {

            console.log("modalInstance Error --> ", error);

        });

    }; // openAddNoteModel


    $scope.openDeleteNoteModel = function (note) {
        var modalInstance = $uibModal.open({
            animation: true,
            size: '100%',
            templateUrl: '/html/deleteNote.html',
            controller: 'DeleteController',
            resolve: {
                note: function () {
                    return note;
                }
            }
        });

        modalInstance.result.then(function (result) {

            console.log("modalInstance Result -- > ", result);

            var index = $scope.notes.indexOf(result);
            if (index !== -1) {
                $scope.notes.splice(index, 1);
                ngNotify.set('Note Deleted' + note.topic, {
                    type: 'error'
                });
            }

        })
            .catch(function (error) {

                console.log("modalInstance Error --> ", error);

            });

    }; // openDeleteNoteModel


});//NotesController