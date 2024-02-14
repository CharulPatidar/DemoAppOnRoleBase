var myApp = angular.module('myApp');

myApp.controller('RoleController', function ($scope, $http, $uibModalInstance, BASE_URL) {

    $scope.roleName;
    var role = $scope.roleName;

    $scope.ok = function () {

        $http.post(BASE_URL + 'Admin/InsertRole',
            {
                role
            })
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data);

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });

        $uibModalInstance.close('done');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };



});//RoleController




myApp.controller('PermissionController', function ($scope, $http, BASE_URL, $uibModal, $uibModalInstance) {

    $scope.permissionName;

    var permission = $scope.permissionName;


    $scope.ok = function () {

        $http.post(BASE_URL + 'Admin/InsertPermission',
            {
                permission
            })
            .then(function (response) {
                // Success callback
                console.log('successful:', response.data);

            })
            .catch(function (error) {
                // Error callback
                console.error(' failed:', error);
            });

        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});//PermissionController




myApp.controller('AllocateRoleToUserController', function ($scope, $http, BASE_URL) {

});//AllocateRoleToUserController


myApp.controller('AllocatePermissionToRoleController', function ($scope, $http, BASE_URL) {

});//AllocatePermissionToRoleController



myApp.controller('DeAllocatePermissionToRoleController', function ($scope, $http, BASE_URL) {

});//DeAllocatePermissionToRoleController




myApp.controller('DeAllocateRoleToUserController', function ($scope, $http, BASE_URL) {

});//DeAllocateRoleToUserController


