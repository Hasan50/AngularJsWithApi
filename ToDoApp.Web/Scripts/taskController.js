

(function () {
    var taskFactory = function ($http) {
        var taskSaveNotification = function (model) {
            return $http.post('/PushNotification/TaskNotification',
                { AssignedToId:model  })
                .then(function (response) {
                    return response;
                });
        };
        var saveData = function (model) {
            return $http.post('/Employee/Save',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var updateData = function (model) {
            return $http.post('/Employee/Update',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var getStatusList = function () {
            return $http.get('/Task/GetTaskStatusList',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getPriorityList = function () {
            return $http.get('/Task/GetPriorityList',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getData = function (id) {
            return $http.get('/Task/GetTaskDetails?id='+id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getTaskAttachment = function (id) {
            return $http.get('/Task/GetTaskAttachments?taskId=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var deleteData = function (id) {
            return $http.get('/Task/DeleteTask?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAll = function (date, dueDate) {
            return $http.get('/Task/GetTasksWithFilter?date=' + date + "&dueDate=" + dueDate,
                {})
                .then(function (response) {
                    return response;
                });
        };
        return {
            GetAll: getAll,
            SaveData: saveData,
            UpdateData: updateData,
            GetData: getData,
            GetStatusList: getStatusList,
            GetPriorityList: getPriorityList,
            DeleteData: deleteData,
            GetTaskAttachment: getTaskAttachment,
            TaskSaveNotification: taskSaveNotification

        };
    };

    trackerApp.factory('taskFactory', taskFactory);

})();

trackerApp.controller("taskController", ['$scope', '$uibModal', 'taskFactory', 'employeeFactory', 'RESOURCE_URL', 'languageFactory', function ($scope, $uibModal, taskFactory, employeeFactory, RESOURCE_URL, languageFactory) {

    $scope.resourceUrl = RESOURCE_URL;
    $scope.ErrorList = [];
    var validateForm = function (group) {
        $scope.ErrorList = [];
        $("." + group).removeClass("invalid-field");
        var notFound = true;
        $("." + group).each(function () {
            if ($(this).hasClass("ng-invalid")) {
                $(this).addClass("invalid-field");
                var message = $(this).parent().prev("label").clone().children().remove().end().text();
                $scope.ErrorList.push({ Message: message + " is required." });
                notFound = false;
            }
        });
        return notFound;
    };
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.DataList = [];
    $scope.TaskAttachmentList = [];
    $scope.pageSize = 10;
    $scope.GetDataList = function () {
        taskFactory.GetAll($scope.filterOb.taskDate, $scope.filterOb.dueDate).then(function (response) {
            $scope.DataList = response.data;
        });
    }
    $scope.employeeList = [];
    $scope.GetEmployeeList = function () {
        employeeFactory.GetAll().then(function (response) {
            $scope.employeeList = response.data;
        });
    }
    $scope.GetEmployeeList();
    $scope.AddOrEdit = function (id) {
        var newDate = new Date().getDate();
        var newMonth = new Date().getMonth()+1;
        var newYear = new Date().getFullYear();
        $scope.BusinessModel = {
            StatusId: $scope.statusList[0].Id.toString(),
            PriorityId: $scope.priorityList[0].Id.toString(),
            DueDateVw: "" + newDate + "/" + newMonth + "/" + newYear+""
        };
        
        if (id !=null) {
            taskFactory.GetData(id).then(function (response) {
                $scope.BusinessModel = response.data;
                $scope.BusinessModel.StatusId = response.data.StatusId.toString();
                $scope.BusinessModel.PriorityId = response.data.PriorityId.toString();
            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Task/TaskCreate',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };
    $scope.Details = function (ob) {
        
        taskFactory.GetData(ob.Id).then(function (response) {
            $scope.BusinessModel = response.data;
            $scope.BusinessModel.DueDate = response.data.DueDateVw;
            $scope.BusinessModel.StatusId = response.data.StatusId.toString();
            $scope.BusinessModel.PriorityId = response.data.PriorityId.toString();

        });
        taskFactory.GetTaskAttachment(ob.Id).then(function (response) {
            $scope.TaskAttachmentList = response.data;
        });
        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Task/TaskDetails',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };
    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };
    $scope.reloadGrid = function () {
        reloadList();
    }
    $scope.resetForm = function () {
        $scope.filterOb = {
            taskDate: null,
            dueDate: null,
        }
        $scope.GetDataList();
    }
    $scope.filterOb = {
        taskDate: null,
        dueDate: null,
    }
    //var reloadList = function () {
    //    var url = "/Task/GetTasksWithFilter?date=" + $scope.filterOb.taskDate + "&dueDate=" + $scope.filterOb.dueDate;
    //    $("#gridList").jqGrid('setGridParam', { url: url });
    //    $("#gridList").trigger("reloadGrid");
    //};

    $scope.ExportToExcel = function () {
        window.location = '/Task/ExportToExcel?date=' + $scope.filterOb.taskDate + "&dueDate=" + $scope.filterOb.dueDate;
    };
    $scope.download = function (a) {
        let url = $scope.resourceUrl + a.BlobName
        window.open(url, '_blank');
    }
    $scope.deleteData = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        taskFactory.DeleteData(id).then(function (response) {
                            if (response.data.Success) {
                                showMessage(response.data.Message, "success");
                                $scope.GetDataList();
                                $scope.$apply();
                            } else {
                                showMessage(response.data.Message, "error");
                            }
                        });
                    }
                },
                cancel: function () {
                    // nothing to do
                }
            }
        });
    };

    $scope.statusList = [];
    $scope.getStatusList = function () {
        taskFactory.GetStatusList().then(function (result) {
            $scope.statusList = result.data;
        });
    }
    $scope.getStatusList();
    $scope.priorityList = [];
    $scope.getPriorityList = function () {
        taskFactory.GetPriorityList().then(function (result) {
            $scope.priorityList = result.data;
        });
    }
    $scope.getPriorityList();
    $scope.IsLoading = false;
    $scope.Save = function () {
        if (validateForm("validator")) {
            $scope.IsLoading = true;
            var data = new FormData();
            if ($scope.selectedFile != null) {
                for (var i = 0; i < $scope.selectedFile.length; i++) {
                    data.append($scope.selectedFile[i].name, $scope.selectedFile[i]);
                }
            }
            var saveData = { ...$scope.BusinessModel };

            data.append('postOb', JSON.stringify(saveData));
            $.ajax({
                type: "POST",
                url: "/Task/SaveTask",
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $scope.IsLoading = false;
                    if (result.Success != null) {
                        $scope.sendTaskPushNotification(saveData.AssignedToId);
                        showMessage(result.Message, "success");
                        $scope.closeModal();
                        $scope.BusinessModel = {};
                        $scope.tempFilePath = "Choose file";
                        $scope.selectedFile = null;
                        $scope.GetDataList();
                        $scope.$apply();
                    } else {
                        showMessage(result.Message, "error");
                    }
                },
                error: function () {
                    $scope.IsLoading = false;
                    showMessage(result.Message, "error");
                }
            });
        }




        //taskFactory.SaveData($scope.BusinessModel).then(function (response) {
        //    if (response.data.Success) {
        //        showMessage(response.data.Message, "success");
        //        $scope.closeModal();
        //        $scope.BusinessModel = {};
        //        reloadList();
        //    }
        //    else {
        //        showMessage(response.data.Message, "error");
        //    }
        //});
    };
    $scope.sendTaskPushNotification=function(assingId){
        taskFactory.TaskSaveNotification(assingId).then(function (response) {
            if (response.data.Success) {
                console.log(response);
            }
        });
    }
    $scope.Update = function () {
        var updateObject = { ...$scope.BusinessModel };
        updateObject.MaximumOfficeHours = updateObject.MaximumOfficeHours1st + ":" + updateObject.MaximumOfficeHours2nd;
        updateObject.OfficeOutTime = updateObject.OfficeOutTime1st + ":" + updateObject.OfficeOutTime2nd;
        taskFactory.UpdateData(updateObject).then(function (response) {
            if (response.data.Success) {
                showMessage(response.data.Message, "success");
                $scope.closeModal();
                $scope.BusinessModel = {};
                $scope.GetDataList();
                $scope.$apply();
            }
            else {
                showMessage(response.data.Message, "error");
            }
        });
    };
    $scope.tempFilePath = "Choose file";
    $scope.selectedFile = [];
    $scope.uploadImage = function () {
        var fileUpload = $("#imageFiles").get(0);
        $scope.tempFilePath = fileUpload.value;
        var files = fileUpload.files;
        $scope.selectedFile = files;
        //var data = new FormData();
        //for (var i = 0; i < files.length; i++) {
        //    data.append(files[i].name, files[i]);
        //}
        //$.ajax({
        //    type: "POST",
        //    url: "/Doctor/UploadProfileImage",
        //    contentType: false,
        //    processData: false,
        //    data: data,
        //    success: function (imgSrc) {
        //        if (imgSrc != null) {
        //            $(".profileImage").attr('src', "");
        //            $(".profileImage").attr('src', imgSrc);
        //            //$scope.GetPerson();
        //        }
        //    },
        //    error: function () {
        //        //alert("There was error uploading files!");
        //    }
        //});
    };
}]);

