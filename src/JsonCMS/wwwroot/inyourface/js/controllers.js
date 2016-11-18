     
var galleryApp = angular.module('galleryApp', []) // [] creates module and will contain any dependencies

.directive('sbLoad', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            var fn = $parse(attrs.sbLoad);
            elem.on('load', function (event) {
                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
            });
        }
    };
}]);

galleryApp.controller('GalleryCtrl',
    function ($scope, $http, $timeout, $window, api) {

        $scope.reloadingState = null; // sets 'truthy' to false
        $scope.comment = '';
        $scope.address = ''; // dummy for spam
        $scope.surname = '';

    var gotSections = function (data) {
        $scope.folders = data;
        var rnd = Math.floor((Math.random() * data.length) + 1);
        $scope.getSection(data[rnd - 1].value, data[rnd - 1].key);
    }

    var errorCallback = function (reason) {
        $window.alert('Error retrieving data');
    }
    
    $http.get('/api/GalleryApi/index').success(gotSections).error(errorCallback);

    $scope.getSection = function (sectionName, sectionNo) {
        $scope.reloadingState = null;
        $scope.sectionName = sectionName;
        $scope.sectionNo = sectionNo;
        $scope.imageIndex = 0;

        api.getSectionImages(sectionName).success(function (data) {
            $scope.images = data;
            $scope.setPagers();

            $timeout(function () {
                $scope.reloadingState = 'show';
            }, 800);
          
        });
    };

    $scope.getNext = function () {
        
        if ($scope.imageIndex + 1 < $scope.images.length) {
            $scope.imageIndex++;
            $scope.reloadingState = null;
        }
        $scope.setPagers();
    }

    $scope.getPrev = function () {
        if ($scope.imageIndex + 1 > 1) {
            $scope.imageIndex--;
            $scope.reloadingState = null;
        }
        $scope.setPagers();
    }

    $scope.setPagers = function () {
        $scope.onFirst = $scope.imageIndex + 1 == 1;
        $scope.onLast = $scope.imageIndex + 1 == $scope.images.length;
    }

    $scope.onImgLoad = function (event) {
        $scope.reloadingState = 'show';
    }

});

/*

see http://www.pluralsight.com/courses/angularjs-get-started

Directives and View :   'ng-model' onwards for writing into model from form
Routing module :        describes js routing using # part of url and how to split code 
                        down into different controllers for different parts of a SPA

*/