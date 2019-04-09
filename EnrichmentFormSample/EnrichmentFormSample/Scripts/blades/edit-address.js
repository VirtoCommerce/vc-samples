angular.module('enrichmentFormSample')
    .controller('enrichmentFormSample.editAddressController', ['$scope', 'platformWebApp.settings',
        function ($scope, settings) {

            var blade = $scope.blade;
            var autocomplete;

            var marker;
            var map;
            var geocoder;
            var isInit = false;

            var data = {};

            $scope.data = data;

            var componentForm = {
                street_number: 'short_name',
                route: 'long_name',
                locality: 'long_name',
                administrative_area_level_1: 'short_name',
                country: 'long_name',
                postal_code: 'short_name'
            };

            if (!window.google || !window.google.maps) {
                settings.getValues({ id: 'EnrichmentFormSample.General.GoogleMapApiKey' }).$promise.then(
                    function (data) {
                        var key = data[0];
                        if (!key) {
                            alert('You should set google api map key in General settings !');
                            $scope.bladeClose();
                            return;
                        }
                        $.getScript("https://maps.googleapis.com/maps/api/js?key=" + key + "&libraries=places&callback=MapApiLoaded",
                            function () { });
                        window.MapApiLoaded = function () {
                            init();
                        };
                    });

            }

            function init() {
                if (isInit) {
                    return;
                }
                blade.title = blade.currentEntity.name;
                blade.subtitle = 'edit-address.edit-address';

                for (var i = 0; i < blade.currentEntity.properties.length; i++) {
                    var property = blade.currentEntity.properties[i];
                    var value = property.values.length ? property.values[0].value : '';
                    switch (property.name) {
                        case 'StreetAddress':
                            data.streetAddress = value;
                            break;
                        case 'State':
                            data.state = value;
                            break;
                        case 'Country':
                            data.country = value;
                            break;
                        case 'City':
                            data.city = value;
                            break;
                        case 'Position':
                            if (value) {
                                var pos = value.split(',');
                                data.position = { lat: Number(pos[0]), lng: Number(pos[1]) };
                            } else {
                                data.position = null;
                            }
                            break;
                        case 'Zip':
                            data.zip = value;
                            break;
                    }
                }

                $scope.blade.isLoading = false;
                initAutocomplete();
                var mapCenter = data.position ? data.position : { lat: -33.8688, lng: 151.2195 };
                map = new google.maps.Map(document.getElementById('address-map'),
                    {
                        center: mapCenter,
                        zoom: 16
                    });
                if (data.position) {
                    setMarker(data.position);
                }
                geocoder = new google.maps.Geocoder();
                isInit = true;
            }

            $scope.init = function () {
                if (!window.google || !window.google.maps) {
                    return;
                }
                init();
            };

            $scope.saveChanges = function () {
                var properties = angular.copy(blade.currentEntity.properties);
                for (var i = 0; i < properties.length; i++) {
                    var property = properties[i];
                    var value = '';
                    switch (property.name) {
                        case 'StreetAddress':
                            value = data.streetAddress;
                            break;
                        case 'State':
                            value = data.state;
                            break;
                        case 'Country':
                            value = data.country;
                            break;
                        case 'City':
                            value = data.city;
                            break;
                        case 'Position':
                            value = data.position ? data.position.lat.toString() + ',' + data.position.lng.toString() : data.position;
                            break;
                        case 'Zip':
                            value = data.zip;
                            break;
                        default:
                            continue;
                    }
                    if (property.values.length) {
                        property.values[0].value = value;
                    } else {
                        property.values.push({ value: value, isInherited: false });
                    }
                }
                blade.currentEntity.properties = properties;
                $scope.bladeClose();
            };

            $scope.isValid = false;

            function initAutocomplete() {
                // Create the autocomplete object, restricting the search predictions to
                // geographical location types.
                // 
                autocomplete = new google.maps.places.Autocomplete(
                    document.getElementById('address_autocomplete'));

                // Avoid paying for data that you don't need by restricting the set of
                // place fields that are returned to just the address components.
                autocomplete.setFields(['address_component', 'geometry']);

                // When the user selects an address from the drop-down, populate the
                // address fields in the form.
                autocomplete.addListener('place_changed', function () {
                    fillInAddress(autocomplete.getPlace());
                });
            }

            function onMarkerDropped(event) {
                geocoder.geocode({ 'latLng': marker.getPosition() }, function (results, status) {
                    if (status === google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            fillInAddress(results[0]);
                            document.getElementById('address_autocomplete').value = results[0].formatted_address;
                        }
                    }
                });
            }

            function setMarker(position) {
                if (!marker) {
                    marker = new google.maps.Marker({
                        position: position,
                        map: map,
                        draggable: true
                    });
                    marker.addListener('dragend', onMarkerDropped);
                } else {
                    marker.setPosition(position);
                }
            }

            function fillInAddress(place) {
                // clear data
                data.city = '';
                data.country = '';
                data.state = '';
                data.street = '';
                data.streetAddress = '';

                var streetAddress = '';
                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];
                    var val = place.address_components[i][componentForm[addressType]];
                    switch (addressType) {
                        case 'route':
                            streetAddress = val + streetAddress;
                            break;
                        case 'street_number':
                            streetAddress = streetAddress + ' ' + val;
                            break;
                        case 'country':
                            data.country = val;
                            break;
                        case 'locality':
                            data.city = val;
                            break;
                        case 'administrative_area_level_1':
                            data.state = val;
                            break;
                        case 'postal_code':
                            data.zip = val;
                            break;
                    }
                }
                data.streetAddress = streetAddress;
                var lat = place.geometry.location.lat();
                var lng = place.geometry.location.lng();
                var position = { lat: lat, lng: lng };
                data.position = position;
                $scope.isValid = true;
                map.setCenter(position);
                setMarker(position);
                $scope.$apply();
            }

            // Bias the autocomplete object to the user's geographical location,
            // as supplied by the browser's 'navigator.geolocation' object.
            $scope.geolocate = function () {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        var geolocation = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };
                        var circle = new google.maps.Circle(
                            { center: geolocation, radius: position.coords.accuracy });
                        autocomplete.setBounds(circle.getBounds());
                    });
                }
            };


        }]);
