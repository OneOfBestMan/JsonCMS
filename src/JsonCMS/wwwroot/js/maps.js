var geocoder = null;
var map = null;
var _markerPath;
var c = 0;
var _domain = null;

function showMarker(thisLocation, lat, longt) {
    // shows current location without link
    var latLng = new google.maps.LatLng(lat, longt);

    var marker = new google.maps.Marker({
        position: latLng,
        map: map,
        title: thisLocation
    });

    marker.setIcon(_markerPath);
}

function showMarkerByCnt(thislocation,locations, latitudes, longitudes) {
    // used to show other locations with links

    if (c != thislocation) {

        var location = locations[c];

        var lat = latitudes[c];
        var long = longitudes[c];

        var latLng = new google.maps.LatLng(lat, long);

        var marker = new google.maps.Marker({
            position: latLng,
            map: map,
            title: location

        });

        marker.setIcon(_markerPath);

        google.maps.event.addListener(marker, 'click', function () {
            window.location = "/" + location + "?d=" + _domain;
        });
    }
    c++;
}

function getMapData(markerPath, mapId, domain, thisLocation /*string*/) {

    this._domain = domain;

    $.getJSON("/api/MapApi" + "?d=" + domain + "&thislocation=" + thisLocation, function (data) {

        var locations = [];
        var latitudes = [];
        var longitudes = [];
        var urls = [];
        var currentLocation = data.thisLocation; /* int */
        var mapzoom = data.mapzoom;

        for (var i = 0; i < data.markers.length; i++) {
            locations.push(data.markers[i].location);
            latitudes.push(data.markers[i].latitude);
            longitudes.push(data.markers[i].longitude);
            urls.push(data.markers[i].url);
        }

        initializeMap(locations, latitudes, longitudes, currentLocation, mapzoom, markerPath,
        mapId);

    });
}

function initializeMap(locations, latitudes, longitudes, currentlocation, mapzoom, markerPath, 
        mapId) {

    var _markerPath = markerPath;

    var currentlat = latitudes[currentlocation];
    var currentlong = longitudes[currentlocation];
    var thislocation = locations[currentlocation];
    var z = parseInt(0 + mapzoom);

    var latlng = new google.maps.LatLng(currentlat, currentlong);
    var myOptions = {
        zoom: z,
        center: latlng,
        panControl: false,
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL,
            position: google.maps.ControlPosition.TOP_RIGHT
        },

        scaleControl: true,
        streetViewControl: false,
        overviewMapControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById(mapId),
        myOptions);
    geocoder = new google.maps.Geocoder();

    showMarker(thislocation, currentlat, currentlong); // makes sure current marker is plotted first

    for (var i = 0; i < latitudes.length; i++) {
        setTimeout(function () {
            showMarkerByCnt(thislocation, locations, latitudes, longitudes);
        }, i * 10);
    }

}