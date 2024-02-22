var myApp = angular.module('myApp');

myApp.config(function ($stateProvider, $urlRouterProvider, $httpProvider) {


    $stateProvider
        .state('home', {
            url: '/home',
            templateUrl: '/html/home.html',
            controller: 'HomeController',
        })
        .state('login', {
            url: '/login',
            templateUrl: '/html/login.html',
            controller: 'LoginController',
        })
        .state('register', {
            url: '/register',
            templateUrl: '/html/register.html',
            controller: 'RegisterController',
        })
        .state('dashboard', {
            url: '/dashboard',
            templateUrl: '/html/dashboard.html',
            controller: 'DashboardController',
        })
        .state('about', {
            url: '/about',
            templateUrl: '/html/about.html',
            controller: 'AboutController',
        })
        .state('notes', {
            url: '/notes?notes',
            templateUrl: '/html/notes.html',
            controller: 'NotesController',
           
        })
        .state('profile', {
            url: '/profile',
            templateUrl: '/html/userProfile.html',
            controller: 'UserProfileController',

        });
            
     //$urlRouterProvider.otherwise('/home'); // Default route

    $httpProvider.interceptors.push('AuthInterceptor');

 });