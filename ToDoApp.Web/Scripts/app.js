


var trackerApp = angular.module("trackerApp", ['ui.bootstrap', 'ngRoute', 'vsGoogleAutocomplete', 'angularUtils.directives.dirPagination',"tagInAuTl"]);
// Constants
//trackerApp.constant('RESOURCE_URL', 'http://192.168.43.36:50753/UploadFiles/');
trackerApp.constant('RESOURCE_URL', 'https://app.headbodybest.com/UploadFiles/');

trackerApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/Dashboard', {
                templateUrl: '/Home/Dashboard',
                controller: 'dashboardController'
            }).when('/my-clients', {
                templateUrl: '/MyClients/Index',
                controller: 'myClientsController'
            }).when('/task-list', {
                templateUrl: '/Task/Index',
                controller: 'taskController'
            }).when('/leave-list', {
                templateUrl: '/Leave/Index',
                controller: 'leaveController'
            }).when('/reports', {
                templateUrl: '/Report/Index',
                controller: 'reportController'
            }).when('/employee-list', {
                templateUrl: '/Employee/AllEmployee',
                controller: 'employeeController'
            }).when('/company-agent-list', {
                templateUrl: '/CompanyAgent/Index',
                controller: 'companyAgentController'
            }).when('/update-profile', {
                templateUrl: '/Account/UpdateProfile',
                controller: 'userController'
            }).when('/change-password', {
                templateUrl: '/Account/ChangePassword',
                controller: 'userController'
            }).otherwise({
                templateUrl: '/Home/Dashboard',
                controller: 'dashboardController'
            });
    }]);


trackerApp.directive('datepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        compile: function () {
            return {
                pre: function (scope, element, attrs, ngModelCtrl) {
                    var format, dateObj;
                    format = (!attrs.dpformat) ? 'd/m/yyyy' : attrs.dpformat;
                    if (!attrs.initDate && !attrs.dpformat) {
                        // If there is no initDate attribute than we will get todays date as the default
                        dateObj = new Date();
                        scope[attrs.ngModel] = dateObj.getDate() + '/' +  (dateObj.getMonth() + 1) + '/' +dateObj.getFullYear();
                    } else if (!attrs.initDate) {
                        // Otherwise set as the init date
                        scope[attrs.ngModel] = attrs.initDate;
                    } else {
                        // I could put some complex logic that changes the order of the date string I
                        // create from the dateObj based on the format, but I'll leave that for now
                        // Or I could switch case and limit the types of formats...
                    }
                    // Initialize the date-picker
                    $(element).datepicker({
                        format: format,
                    }).on('changeDate', function (ev) {
                        // To me this looks cleaner than adding $apply(); after everything.
                        scope.$apply(function () {
                            ngModelCtrl.$setViewValue(ev.format(format));
                        });
                    });
                }
            }
        }
    }
});
trackerApp.directive('ckEditor', function () {
    return {
        require: '?ngModel',
        link: function (scope, elm, attr, ngModel) {
            var ck = CKEDITOR.replace(elm[0]);
            if (!ngModel) return;
            ck.on('instanceReady', function () {
                ck.setData(ngModel.$viewValue);
            });
            function updateModel() {
                scope.$apply(function () {
                    ngModel.$setViewValue(ck.getData());
                });
            }
            ck.on('change', updateModel);
            ck.on('key', updateModel);
            ck.on('dataReady', updateModel);

            ngModel.$render = function (value) {
                ck.setData(ngModel.$viewValue);
            };
        }
    };
});
trackerApp.directive('compile', compile)
compile.$inject = ['$compile'];
function compile($compile) {
    return function (scope, element, attrs) {
        scope.$watch(
            function (scope) {
                return scope.$eval(attrs.compile);
            },
            function (value) {
                element.html(value);
                $compile(element.contents())(scope);
            }
        );
    };
}
trackerApp.filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
});

trackerApp.filter('ctime', function () {
    return function (jsonDate) {
        if (typeof jsonDate !== 'undefined' && jsonDate !== null) {
            var date = new Date(parseInt(jsonDate.substr(6)));
            return date;
        }
        return "";
    };
});

trackerApp.directive("angdatepicker", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, ngModelCtrl) {
            var updateModel = function (dateText) {
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(dateText);
                });
            };
            var options = {
                showOn: "both",
                buttonImage: "/Content/images/Calendar.gif",
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy",
                changeYear: true,
                yearRange: "-100:+100",
                onSelect: function (dateText) {
                    updateModel(dateText);
                }
            };
            elem.datepicker(options);
        }
    };
});

trackerApp.directive('validNumber', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^0-9\.]/g, '');
                var decimalCheck = clean.split('.');

                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 2);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});

trackerApp.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter, { 'event': event });
                });

                event.preventDefault();
            }
        });
    };
});

trackerApp.directive('myTooltip', function () {
    return {
        link: function (scope, element, attrs) {
            var changeTooltipPosition = function (event) {
                var tooltipX = event.pageX - 5 - $('div.mytooltip').width();
                var tooltipY = event.pageY + 5;
                $('div.mytooltip').css({ top: tooltipY, left: tooltipX });
            };

            var showTooltip = function (event) {
                $('div.mytooltip').remove();
                $("<div class='mytooltip'>" + attrs.myTooltip + "</div>").appendTo('body');
                changeTooltipPosition(event);
            };

            var hideTooltip = function () {
                $('div.mytooltip').remove();
            };

            element.bind({
                mousemove: changeTooltipPosition,
                mouseenter: showTooltip,
                mouseleave: hideTooltip
            });

        }
    };
});


trackerApp.directive('focusNext', function () {
    return {
        restrict: 'A',
        link: function ($scope, elem, attrs) {
            elem.bind("keydown keypress", function (e) {
                var code = e.keyCode || e.which;
                if (code === 13) {
                    e.preventDefault();
                    var fields = $('#autoFocusable').find('input, textarea, select');
                    var index = fields.index(this);
                    if (index > -1 && (index + 1) < fields.length)
                        fields.eq(index + 1).focus();
                }
            });
        }
    };
});

var getQueryStringValue = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results === null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
};

var getToday = function () {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    today = dd + '/' + mm + '/' + yyyy;
    return today;
};

var initializeDateRangePicker = function () {
    var dateFormat = "dd/mm/yy",
        from = $("#startDate")
            .datepicker({
                //defaultDate: "+1w",
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                numberOfMonths: 1
            })
            .on("change", function () {
                to.datepicker("option", "minDate", getDate(this));
            }),
        to = $("#endDate").datepicker({
            defaultDate: "+1w",
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            numberOfMonths: 1
        })
            .on("change", function () {
                from.datepicker("option", "maxDate", getDate(this));
            });

    function getDate(element) {
        var date;
        try {
            date = $.datepicker.parseDate(dateFormat, element.value);
        } catch (error) {
            date = null;
        }

        return date;
    }
};

trackerApp.directive("anglettercapital", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, ngModelCtrl) {
            elem.keyup(function () {
                var str = jQuery(this).val();
                var spart = str.split(" ");
                for (var i = 0; i < spart.length; i++) {
                    var j = spart[i].charAt(0).toUpperCase();
                    spart[i] = j + spart[i].substr(1);
                }
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(spart.join(" "));
                    ngModelCtrl.$render();
                });
            });
        }
    };
});

trackerApp.directive("rowsortable", function () {
    return {
        restrict: "A",
        scope: {
            callbackFn: '&'
        },
        link: function (scope, elem, attr, ctrl) {
            elem.sortable({
                items: 'tr:has(td)',
                cursor: "move",
                opacity: 0.75,
                stop: function (event, ui) {
                    scope.callbackFn({ arg1: ui });
                }
            });
        }
    };
});


var showMessage = function (message, type) {
    $.toast({
        text: message,
        icon: type,
        showHideTransition: 'slide',
        position: 'top-center'
    });
};