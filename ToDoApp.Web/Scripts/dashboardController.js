

(function () {
    var dashboardFactory = function ($http) {

        var getCompanyAgentList = function () {
            return $http.get('/CompanyAgent/GetCompanyAgentList',
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getAttendance = function (companyAgentId, date) {
            return $http.get('/Attendance/GetCompanyAgentAttendanceFeed?companyAgentId=' + companyAgentId + "&date=" + date,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getOfflineAttendance = function (companyAgentId, date) {
            return $http.get('/Attendance/GetCompanyAgentAttendanceOfflineFeed?companyAgentId=' + companyAgentId + "&date=" + date,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getEmployeeMovementDetails = function (userId) {
            return $http.get('/Attendance/GetMovementDetails?userId=' + userId,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getLocationBarData = function (userId,date) {
            return $http.get('/Attendance/GetLocationBarData?userId=' + userId+'&date='+date,
                {})
                .then(function (response) {
                    return response;
                });
        };

        return {
            GetCompanyAgentList: getCompanyAgentList,
            GetAttendance: getAttendance,
            GetEmployeeMovementDetails: getEmployeeMovementDetails,
            GetLocationBarData: getLocationBarData,
            GetOfflineAttendance: getOfflineAttendance

        };
    };

    trackerApp.factory('dashboardFactory', dashboardFactory);

})();

trackerApp.controller("dashboardController", ['$scope', '$uibModal', 'dashboardFactory', 'languageFactory', 'RESOURCE_URL', function ($scope, $uibModal, dashboardFactory, languageFactory, RESOURCE_URL) {

    $scope.resourceUrl = RESOURCE_URL;

    $scope.activeTab = 1;

    $scope.GetCompanyAgents = function () {
        dashboardFactory.GetCompanyAgentList().then(function (response) {
            $scope.AgentList = response.data;
        });
    }
    dateObj = new Date();
    $scope.selectedDate = dateObj.getDate() + '/' + (dateObj.getMonth() + 1) + '/' + dateObj.getFullYear();
    $scope.agentId = "";
    $scope.GetAttendance = function (d, a) {
        dashboardFactory.GetAttendance(a, d).then(function (response) {
            $scope.AttendanceList = response.data.EmployeeList;
            $scope.StatusCount = response.data.StatusCount;
        });
        if ($scope.activeTab===2) {
            $scope.GetOfflineAttendance(d, a);
        }
    }
    $scope.offlineAttendance = [];
    $scope.GetOfflineAttendance = function (d, a) {
        dashboardFactory.GetOfflineAttendance(a, d).then(function (response) {
            $scope.offlineAttendance = response.data.EmployeeList;
        });
    }
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();

    $scope.OpenMapLocation = function (ob) {
        $scope.userOb = ob;

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Home/DisplayMapLocation',
            backdrop: 'static',
            scope: $scope,
            controller: "mapLocationController",
            size: 'xl'
        });

    };

    $scope.closeModal = function () {
       
        $scope.modalInstance.close();
    };
    $scope.OpenDetailsMapLocation = function (ob) {
        $scope.userOb = ob;

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Home/DetailsBarMapLocation',
            backdrop: 'static',
            scope: $scope,
            controller: "detailsBarMapLocationController",
            size: 'xl'
        });

    };
    //$scope.LocationList = [
    //    { IsGapBarPin: false, Width: 150, Title: "in at 8:30AM", IsLastOut: false, LastTitle: null },
    //    { IsGapBarPin: true, Width: 100, Title: "out at 10:30AM", IsLastOut: false, LastTitle: null },
    //    { IsGapBarPin: false, Width: 100, Title: "in at 11:30AM", IsLastOut: false, LastTitle: null },
    //    { IsGapBarPin: true, Width: 50, Title: "out at 12:30PM", IsLastOut: false, LastTitle: null },
    //    { IsGapBarPin: false, Width: 320, Title: "in at 1:30AM", IsLastOut: true, LastTitle: "out at 5:30AM" }
    //];
    $scope.showDetailsBar = function (ob, ele) {
        $scope.LocationList = []; //get from server using api and assing to LocationList.
        dashboardFactory.GetLocationBarData(ob.UserId, ob.AttendanceDateVw).then(function (response) {
            $scope.LocationList = response.data;
        });
        // $scope.LocationList = [
        //{ IsGapBarPin: false, Width: 150, Title: "in at 8:30AM", IsLastOut: false, LastTitle: null },
        //{ IsGapBarPin: true, Width: 100, Title: "out at 10:30AM", IsLastOut: false, LastTitle: null },
        //{ IsGapBarPin: false, Width: 100, Title: "in at 11:30AM", IsLastOut: false, LastTitle: null },
        //{ IsGapBarPin: true, Width: 50, Title: "out at 12:30PM", IsLastOut: false, LastTitle: null },
        //{ IsGapBarPin: false, Width: 320, Title: "in at 1:30AM", IsLastOut: true, LastTitle: "out at 5:30AM" }
    //];
        var element = angular.element(ele.target);
        $("#divDetails").css({
            'display': 'block',
            "position": "absolute",
            "top": element.offset().top+70,
            "left": 300
        });
        element[0].parentElement.parentElement.style.height = "110px";
    }
}]);

trackerApp.controller("mapLocationController", ['$scope', '$uibModal', 'dashboardFactory', 'languageFactory', 'RESOURCE_URL', function ($scope, $uibModal, dashboardFactory, languageFactory, RESOURCE_URL) {

    $scope.userOb = { ...$scope.$parent.userOb };
    $scope.resourceUrl = RESOURCE_URL;
    $scope.trackingUpadte = {};
    function pubNubInit() {

        // Update this block with your publish/subscribe keys
        pubnub = new PubNub({
            publishKey: "pub-c-ceba06fa-6402-4273-9f00-dcff585940d0",
            subscribeKey: "sub-c-71ea2646-16c5-11eb-ae19-92aa6521e721",
            uuid: "web-client"
        })



        pubnub.subscribe({
            channels: ['hello_world'],
            withPresence: true,
            triggerEvents: ['message', 'presence', 'status']
        });
    };
    pubNubInit();
    $scope.markerTitle = "";
    $scope.markerConetnt = "";
    function initialize() {
        var myOptions = {
            zoom: 15,
            center: new google.maps.LatLng(25, 80),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        const geocoder = new google.maps.Geocoder();
        var map = new google.maps.Map(document.getElementById('map'), myOptions);

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        zoom: 15,
                        lat: position.coords.latitude,
                        lng: position.coords.longitude,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }



        var heighLiteMarker = function (pos) {
            var winInfo = new google.maps.InfoWindow();
            var myLatLng = new google.maps.LatLng(pos.lat, pos.long);
            var marker = new google.maps.Marker({
                map: map,
                position: myLatLng,
                title: pos.title,
            });
            $scope.markerTitle = pos.title;
            $scope.markerConetnt = '<div>' + pos.desc + '</div>';
            marker.content = '<div>' + pos.desc + '</div>';

            google.maps.event.addListener(marker, 'click', function () {
                winInfo.setContent('<h3>' + $scope.markerTitle + '</h3>' + marker.content);
                winInfo.open(map, marker);
            });
            const cpos = {
                zoom: 15,
                lat: pos.lat,
                lng: pos.long,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map.setCenter(cpos);
            marker.setMap(map);
            moveMarker(geocoder, winInfo, map, marker, pos);
        }

        dashboardFactory.GetEmployeeMovementDetails($scope.userOb.UserId).then(function (response) {
            $scope.AttendanceList = response.data;
            var ob = response.data[response.data.length-1];
            heighLiteMarker({
                title: ob.LogLocation,
                desc: ob.IsCheckInPoint ? "Checkin:" + ob.LogTimeVw : "CheckOut:" + ob.LogTimeVw,
                lat: ob.Latitude,
                long: ob.Longitude
            })
        });


    }
    function moveMarker(geocoder, winInfo,map, marker, pos) {
        //delayed so you can see it move
        //setInterval(function () {
        //    pos.lat = pos.lat + 1;
        //    pos.long = pos.long + 1;
        //    marker.setPosition(new google.maps.LatLng(pos.lat, pos.long));
        //    //  map.panTo( new google.maps.LatLng( 0, 0 ) );
        //}, 1000);
        pubnub.addListener({
            status: function (statusEvent) {
                if (statusEvent.category === "PNConnectedCategory") {
                    //  publishSampleMessage();
                }
            },
            message: function (msg) {
                console.log(msg.message.title);
                console.log(msg.message.description);
                //$scope.trackingUpadte = msg.message;
                $scope.markerTitle = msg.message.title;
                geocodeLatLng(geocoder,winInfo,map,marker,msg.message.latitude, msg.message.longitude);
                marker.setPosition(new google.maps.LatLng(msg.message.latitude, msg.message.longitude));
            },
            presence: function (presenceEvent) {
                // This is where you handle presence. Not important for now :)
            }
        })
    };

    $scope.loadMap = function () {
        initialize();
    }
    $scope.AttendanceList = [];
    $scope.GetEmployeeMovementDetails = function () {
        dashboardFactory.GetEmployeeMovementDetails($scope.userOb.UserId).then(function (response) {
            $scope.AttendanceList = response.data;
            initialize().heighLiteMarker({
                city: 'India',
                desc: 'The Indian economy is the worlds seventh-largest by nominal GDP and third-largest by purchasing power parity (PPP).',
                lat: $scope.AttendanceList[0].Latitude,
                long: $scope.AttendanceList[0].Longitude
            })
        });
    }
    function geocodeLatLng(geocoder, winInfo,map,marker,latting, logint) {
        const latlng = {
            lat: latting,
            lng: logint,
        };
        geocoder.geocode({ location: latlng }, (results, status) => {
            if (status === "OK") {
                if (results[0]) {

                    winInfo.setContent(results[0].formatted_address);
                    winInfo.setContent('<h1>' + results[0].formatted_address + '</h1>' +  $scope.markerConetnt);
                    winInfo.open(map, marker);
                } else {
                    window.alert("No results found");
                }
            } else {
                window.alert("Geocoder failed due to: " + status);
            }
        });
    }
    //$scope.GetEmployeeMovementDetails();
    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };
}]);
trackerApp.controller("detailsBarMapLocationController", ['$scope', '$uibModal', 'dashboardFactory', 'languageFactory', 'RESOURCE_URL', function ($scope, $uibModal, dashboardFactory, languageFactory, RESOURCE_URL) {

    $scope.userOb = { ...$scope.$parent.userOb };
    $scope.resourceUrl = RESOURCE_URL;
  
    $scope.markerTitle = "";
    $scope.markerConetnt = "";
    function initialize() {
        var myOptions = {
            zoom: 15,
            center: new google.maps.LatLng(25, 80),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        const geocoder = new google.maps.Geocoder();
        var map = new google.maps.Map(document.getElementById('map'), myOptions);

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const pos = {
                        zoom: 15,
                        lat: $scope.userOb.Lattitude,
                        lng: $scope.userOb.Longitude,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                    map.setCenter(pos);
                },
                () => {
                    handleLocationError(true, infoWindow, map.getCenter());
                }
            );
        } else {
            // Browser doesn't support Geolocation
            handleLocationError(false, infoWindow, map.getCenter());
        }



        var heighLiteMarker = function () {
            var winInfo = new google.maps.InfoWindow();
            var myLatLng = new google.maps.LatLng($scope.userOb.Lattitude, $scope.userOb.Longitude);
            var marker = new google.maps.Marker({
                map: map,
                position: myLatLng,
                title: $scope.userOb.LogLocation,
            });
            $scope.markerTitle = $scope.userOb.LogLocation;
            marker.content = '<div>' + $scope.userOb.Title + '</div>';

            google.maps.event.addListener(marker, 'click', function () {
                winInfo.setContent('<h3>' + $scope.markerTitle + '</h3>' + marker.content);
                winInfo.open(map, marker);
            });
            marker.setMap(map);
        }

        heighLiteMarker();


    }
  

    $scope.loadMap = function () {
        initialize();
    }
    $scope.AttendanceList = [];
    
    function geocodeLatLng(geocoder, winInfo, map, marker, latting, logint) {
        const latlng = {
            lat: latting,
            lng: logint,
        };
        geocoder.geocode({ location: latlng }, (results, status) => {
            if (status === "OK") {
                if (results[0]) {

                    winInfo.setContent(results[0].formatted_address);
                    winInfo.setContent('<h1>' + results[0].formatted_address + '</h1>' + $scope.markerConetnt);
                    winInfo.open(map, marker);
                } else {
                    window.alert("No results found");
                }
            } else {
                window.alert("Geocoder failed due to: " + status);
            }
        });
    }
    //$scope.GetEmployeeMovementDetails();
    $scope.closeModal = function () {
        var container = $("#divDetails");
        // if the target of the click isn't the container nor a descendant of the container
            container.show();
            console.log(container);
        $scope.modalInstance.close();
    };
}]);

