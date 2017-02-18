// custom service example - wraps api calls. Would use for all
// then can call from multiple controllers
// have left call for sectionNames to demonstrate direct call to $http

var api = function ($http) {

    var getSectionImages = function (sectionName) {
        return $http.get('/api/GalleryApi/index/' + sectionName + '?d=inyourface').
            success(function (response) {
                return response;
            });
    }

    return {
        getSectionImages: getSectionImages
    };

}

var module = angular.module("galleryApp"); // gets the module without creating
module.factory("api", api);