var myApp = angular.module('myApp');

myApp.controller('DashboardController', function ($scope, $http, BASE_URL, $uibModal, ngNotify) {
    console.log("DashboardController");
    $scope.users;
    $scope.roles = [];
    $scope.permissions;
   
    
     var BASEURL = BASE_URL;

    // IIFE for fetching all users
    (function getAllUsers() {
        $http.get(BASEURL + 'User/GetAllUsers')
            .then(function (response) {
                console.log("All Users successful:", response.data.$values);
                $scope.users = response.data.$values;
                     // Assign users data to $scope.users
            })
            .catch(function (error) {
                console.error("All Users failed:", error);
            });
    })();

    // IIFE for fetching all roles
     $scope.getAllRoles = (function getAllRoles() {
        $http.get(BASEURL + 'Role/GetAllRoles')
            .then(function (response) {
                console.log("All Roles successful:", response.data.$values);
                $scope.roles = response.data.$values; // Assign roles data to $scope.roles
            })
            .catch(function (error) {
                console.error("All Roles failed:", error);
            });
    })();

    // IIFE for fetching all permissions
    $scope.getAllPermission =  (function getAllPermissions() {
        $http.get(BASEURL + 'Permission/GetAllPermission')
            .then(function (response) {
                console.log("All Permission successful:", response.data.$values);
                $scope.permissions = response.data.$values;; // Assign permissions data to $scope.permissions
            })
            .catch(function (error) {
                console.error("All Permission failed:", error);
            });
    })();


    $scope.selectedUser = {
        id: "",
        userName: "",
        userEmail: ""
    };

    $scope.selectUser = function (user) {

        $scope.selectedUser = {
            id: user.id,
            userName: user.userName,
            userEmail: user.userEmail
        };
    };




    $scope.selectedRole = {
        id: "",
        roleName: ""
    };

    $scope.selectRole = function (role) {
        $scope.selectedRole = {
            id: role.id,
            roleName: role.roleName
        };
    };




    $scope.selectedPermission = {
        id: "",
        permissionName: ""
    };

    $scope.selectPermission = function (permission) {
        $scope.selectedPermission = {
            id: permission.id,
            permissionName: permission.permissionName
        };
    };

    

    // Allocation ******************************************************************************************
    $scope.isCheckedRU = false;

    $scope.AllocatedRU = {
        RoleId: '',
        UserId: '',
        RoleName: '',
        UserName:''
    };
    $scope.AllocateRoleToUser = function () {

        $http.post(BASE_URL + 'Role/AllocateRoleToUser',
            {
                "UserId": $scope.AllocatedRU.UserId,
                "RoleId": $scope.AllocatedRU.RoleId
            })
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data);

                ngNotify.set('successful ' + response.data  , {
                    type: 'success'
                });

                

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
                ngNotify.set('failed ' + error.data, {
                    type: 'error'
                });
            });

    }



    $scope.AllocatedPR = {
        RoleId: '',
        PermissionId: '',
        RoleName: '',
        PermissionName: ''
    };
    $scope.AllocatePermissionToRole = function () {

        $http.post(BASE_URL + 'Permission/AllocatePermissionToRole',
            {
                "PermissionId": $scope.AllocatedPR.PermissionId,
                "RoleId": $scope.AllocatedPR.RoleId
            })
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data);
                ngNotify.set('successful ' + response.data, {
                    type: 'success'
                });

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });

    }

    //*********************************************************************************************************

    
    // Add Role And Permission To DB ----------------------------------------------------
    $scope.openAddRoleModel = function () {


        var modalInstance = $uibModal.open({
            animation: true,
            size: '100%',
            template: `
               <div>
                    <h2>Add Role</h2>
                    <div class="form-group">
                        <label for="roleName">Role Name:</label>
                        <input type="text" class="form-control" id="roleName" ng-model="roleName">
                    </div>
                    <button class="btn btn-primary" ng-click="ok()">OK</button>
                    <button class="btn btn-secondary" ng-click="cancel()">Cancel</button>
                </div>

            `,
            controller: function ($scope, $uibModalInstance) {

                $scope.$uibModalInstance = $uibModalInstance; // Injecting $uibModalInstance into RoleController

                // Define your RoleController functions here
                $scope.roleName;
                $scope.ok = function () {
                    var roleName = JSON.stringify($scope.roleName)
                    $http.post(BASE_URL + 'Role/InsertRole', roleName)
                        .then(function (response) {
                            // Success callback
                            console.log(' successful:', response.data);
                            var data = response.data;
                           // $scope.getAllRoles();
                            //debugger;
                            ngNotify.set('successfully addded role :  ' + roleName, {
                                type: 'success'
                            });

                        })
                        .catch(function (error) {
                            // Error callback
                            console.error('failed:', error);
                        });

                    $uibModalInstance.close('done');
                };

                $scope.cancel = function () {
                    // Your implementation
                    $uibModalInstance.dismiss('cancel');
                };
            }
            
        });

        modalInstance.result.then(function (result) {
            // Do something with the result if needed

            console.log("modalInstance Result -- > ", result);
        })
        .catch(function (error) {

            console.log("modalInstance Error --> ", error);

        });


    }

    $scope.openAddPermissionModel = function () {



        var modalInstance = $uibModal.open({
            animation: true,
            size: '100%',
            template: `
               <div>
                    <h2>Add Permission</h2>
                    <div class="form-group">
                        <label for="permissionName">Permission Name:</label>
                        <input type="text" class="form-control" id="permissionName" ng-model="permissionName">
                    </div>
                    <button class="btn btn-primary" ng-click="ok()">OK</button>
                    <button class="btn btn-secondary" ng-click="cancel()">Cancel</button>
                </div>

            `,
            controller: function ($scope, $uibModalInstance) {

                $scope.$uibModalInstance = $uibModalInstance; // Injecting $uibModalInstance into RoleController

                // Define your RoleController functions here
                $scope.permissionName;
                $scope.ok = function () {
                    var permissionName = JSON.stringify($scope.permissionName)
                    $http.post(BASE_URL + 'Permission/InsertPermission', permissionName)
                        .then(function (response) {
                            // Success callback
                            console.log(' successful:', response.data);
                            var data = response.data;
                           // $scope.getAllPermission();
                           // debugger;
                            ngNotify.set('successfully addded Permission :  ' + permissionName, {
                                type: 'success'
                            });

                        })
                        .catch(function (error) {
                            // Error callback
                            console.error('failed:', error);
                        });

                    $uibModalInstance.close('done');
                };

                $scope.cancel = function () {
                    // Your implementation
                    $uibModalInstance.dismiss('cancel');
                };
            }

        });

        modalInstance.result.then(function (result) {
            // Do something with the result if needed

            console.log("modalInstance Result -- > ", result);
        })
            .catch(function (error) {

                console.log("modalInstance Error --> ", error);

            });



    }

    //---------------------------------------------------------------------------------------


    // DeAllocation Role from Users ===================================================================================
    $scope.getRoleForUser = {
        roleName: '',
        roleId: ''
    };
    $scope.AllUsersByRole;
    $scope.GetAllUserByRoleId = function (role) {

        $scope.getRoleForUser.roleName = role.roleName
        $scope.getRoleForUser.roleId = role.id;


        $http.get(BASE_URL + 'User/GetAllUserByRoleId?roleId=' + role.id)
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data.$values);
                var data = response.data.$values;

                if (data && data.length < 1) {
                    // data array is not empty
                    data = [{ userName: 'NA', userId: 'NA' }];
                }
                console.log(data);
                $scope.AllUsersByRole = data;

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });

    } 


    $scope.DeAllocateRoleToUser = function (userId) {

        var roleId = $scope.getRoleForUser.roleId;
        console.log("de",roleId)

        $http.post(BASE_URL + 'Role/DeAllocateRoleToUser', {
            
            "RoleId": roleId,
            "UserId": userId
            
        })
        .then(function (response) {
            // Success callback
            console.log(' successful:', response.data);
            ngNotify.set('successful ' + response.data, {
                type: 'success'
            });
        })
        .catch(function (error) {
            // Error callback
            console.error('failed:', error);
        });


    }




    // =============================================================================================================

    
    // DeAllocation Permission from  Roles

    $scope.getRoleForPermission = {
        roleName : '',
        roleId : ''
    };
    $scope.AllPermissionByRole;
    $scope.GetAllPermissionByRoleId = function (role) {

        $scope.getRoleForPermission.roleName = role.roleName;
        $scope.getRoleForPermission.roleId = role.id;


        $http.get(BASE_URL + 'Permission/GetAllPermissionByRoleId?roleId=' + role.id)
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data.$values);
                var data = response.data.$values;
                if (data && data.length < 1) {
                    // data array is not empty
                    data = [{ userName: 'NA', userId: 'NA' }];

                }
                $scope.AllPermissionByRole = data;
            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });


    }

    $scope.DeAllocatePermissionFromRole = function (p) {

        var roleId = $scope.getRoleForPermission.roleId;
        console.log("de", p);

        $http.post(BASE_URL + 'Permission/DeAllocatePermissionToRole', {

            "RoleId": roleId,
            "PermissionId": p.permissionId

        })
            .then(function (response) {
                // Success callback 
                debugger;
                console.log(' successful:', response.data);
                ngNotify.set('successful ' + response.data, {
                    type: 'success'
                });
            })
            .catch(function (error) {
                // Error callback
                debugger;
                ngNotify.set('failed:  ' + response.data, {
                    type: 'error'
                });
                console.error('failed:', error);
            });


    }



    $scope.DeleteUser = function (user) {
        var userId = user.id;
        debugger;
        $http({
            method: 'DELETE',
            url: BASE_URL + 'User/DeleteUser?userId=' + userId,
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data);
                ngNotify.set('successful ' + response.data, {
                    type: 'success'
                });

                var index = $scope.users.findIndex(function (u) {
                    return u.id === userId;
                });
                // If the user is found, remove it from the array
                if (index !== -1) {
                    $scope.users.splice(index, 1);
                }

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });
    }

    
    $scope.DeleteRole = function (role) {
        var roleId = role.id;
        $http({
            method: 'DELETE',
            url: BASE_URL + 'Role/DeleteRole?RoleId=' + roleId,
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                // Success callback
                console.log(' successful:', response.data);
                ngNotify.set('successful ' + response.data, {
                    type: 'success'
                });

                var index = $scope.roles.findIndex(function (r) {
                    return r.id === roleId;
                })
                // If the user is found, remove it from the array
                if (index !== -1) {
                    $scope.roles.splice(index, 1);
                }

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });
    }

    $scope.DeletePermission = function (permission) {
        var permissionId = permission.id;
        debugger;
        $http({
            method: 'DELETE',
            url: BASE_URL + 'Permission/DeletePermission?PermissionId=' + permissionId,
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(function (response) {
                // Success callback
                console.log(' successfully:', response.data);
                ngNotify.set('successfully deleted Permission ' + response.data, {
                    type: 'success'
                });

                var index = $scope.permissions.findIndex(function (p) {
                    return p.id === permissionId;
                })
                // If the user is found, remove it from the array
                if (index !== -1) {
                    $scope.permissions.splice(index, 1);
                }

            })
            .catch(function (error) {
                // Error callback
                console.error('failed:', error);
            });
    }

});//DashboardController


