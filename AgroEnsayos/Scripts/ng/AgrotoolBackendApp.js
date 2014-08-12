
//Creates APP in global scope
var APP = angular.module('APP', ['ngResource', 'ui.bootstrap', 'angucomplete']);

APP.factory('globalErrorInterceptor', function ($q)
{
    return {
        'responseError': function (r)
        {
            window.openAngularException(r);
            return $q.reject(r)
        }
    }
})

APP.config(function ($httpProvider)
{
    $httpProvider.interceptors.push('globalErrorInterceptor')
})