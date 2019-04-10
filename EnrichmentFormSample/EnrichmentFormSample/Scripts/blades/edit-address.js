angular.module('enrichmentFormSample')
    .controller('enrichmentFormSample.editAddressController', ['$scope', 'platformWebApp.settings',
        function ($scope, settings) {
            var blade = $scope.blade;
            blade.title = blade.currentEntity.name;
            blade.subtitle = 'edit-address.edit-address';

            var defaultMapCenter = { lat: -33.8688, lng: 151.2195 };
            var autocomplete;
            var marker;
            var map;
            var geocoder;
            var isInit = false;
            var addressFields = $scope.addressFields = {};

            var componentForm = {
                street_number: 'short_name',
                route: 'long_name',
                locality: 'long_name',
                administrative_area_level_1: 'short_name',
                country: 'long_name',
                postal_code: 'short_name'
            };

            if (!window.google || !window.google.maps) {
                settings.getValues({ id: 'EnrichmentFormSample.General.GoogleMapApiKey' }, function (data) {
                    var key = data[0];
                    if (!key) {
                        alert('You should set google api map key in General settings!');
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

                isInit = true;
                blade.isLoading = false;
                _.each(blade.currentEntity.properties, function (property) {
                    var value = _.any(property.values) ? property.values[0].value : '';
                    switch (property.name) {
                        case 'StreetAddress':
                            addressFields.streetAddress = value;
                            break;
                        case 'State':
                            addressFields.state = value;
                            break;
                        case 'Country':
                            addressFields.country = value;
                            break;
                        case 'City':
                            addressFields.city = value;
                            break;
                        case 'Position':
                            if (value) {
                                var pos = value.split(',');
                                addressFields.position = { lat: Number(pos[0]), lng: Number(pos[1]) };
                            } else {
                                addressFields.position = null;
                            }
                            break;
                        case 'Zip':
                            addressFields.zip = value;
                            break;
                    }
                });

                initAutocomplete();
                var mapCenter = addressFields.position || defaultMapCenter;
                map = new google.maps.Map(document.getElementById('address-map'),
                    {
                        center: mapCenter,
                        zoom: 16
                    });
                if (addressFields.position) {
                    setMarker(addressFields.position);
                }
                geocoder = new google.maps.Geocoder();
            }

            $scope.init = function () {
                if (window.google && window.google.maps) {
                    init();
                }
            };

            $scope.saveChanges = function () {
                var properties = angular.copy(blade.currentEntity.properties);
                _.each(properties, function (property) {
                    var value = '';
                    switch (property.name) {
                        case 'StreetAddress':
                            value = addressFields.streetAddress;
                            break;
                        case 'State':
                            value = addressFields.state;
                            break;
                        case 'Country':
                            value = addressFields.country;
                            break;
                        case 'City':
                            value = addressFields.city;
                            break;
                        case 'Position':
                            value = addressFields.position ? addressFields.position.lat.toString() + ',' + addressFields.position.lng.toString() : addressFields.position;
                            break;
                        case 'Zip':
                            value = addressFields.zip;
                            break;
                        default:
                            return;
                    }
                    if (property.values.length) {
                        property.values[0].value = value;
                    } else {
                        property.values.push({ value: value, isInherited: false });
                    }
                });

                blade.currentEntity.properties = properties;
                $scope.bladeClose();
            };

            $scope.cancelChanges = function () {
                $scope.bladeClose();
            };

            $scope.isValid = function () {
                return $scope.isChanged;
            };

            $scope.setChanged = function () {
                $scope.isChanged = true;
            };

            $scope.isChanged = false;

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
                    if (status === google.maps.GeocoderStatus.OK && results.length) {
                        fillInAddress(results[0]);
                        document.getElementById('address_autocomplete').value = results[0].formatted_address;
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
                $scope.addressFields = addressFields = {};

                var streetAddress = '';
                _.each(place.address_components, function (addressComponent) {
                    var addressType = addressComponent.types[0];
                    var val = addressComponent[componentForm[addressType]];
                    switch (addressType) {
                        case 'route':
                            streetAddress = val + streetAddress;
                            break;
                        case 'street_number':
                            streetAddress = streetAddress + ' ' + val;
                            break;
                        case 'country':
                            addressFields.country = val;
                            break;
                        case 'locality':
                            addressFields.city = val;
                            break;
                        case 'administrative_area_level_1':
                            addressFields.state = val;
                            break;
                        case 'postal_code':
                            addressFields.zip = val;
                            break;
                    }
                });
                addressFields.streetAddress = streetAddress;

                var position = {
                    lat: place.geometry.location.lat(),
                    lng: place.geometry.location.lng()
                };
                addressFields.position = position;
                $scope.isChanged = true;
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

        }
    ]);
