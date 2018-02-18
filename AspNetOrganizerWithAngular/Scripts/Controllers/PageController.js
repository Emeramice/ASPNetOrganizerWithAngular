var PageController = function ($scope, $http) {
    $scope.task = {
        Priority:1,
        DueDateTime:new Date()
    };

    $scope.taskPriorities =
    [{
        priorId: 0,
        priorTitle: 'Low'
    },
    {
        priorId: 1,
        priorTitle: 'Normal'
    },
    {
        priorId: 2,
        priorTitle: 'High'
    }
    ];

    $scope.addNewTask = function () {
        $scope.task.DueDateTime = $scope.task.DueDateTime.getTime();
        var promise = $http.post('/tasks', $scope.task);
        promise.then(function (response) {
            $scope.NewTaskClicked = false;
            $scope.task.DueDateTime = new Date($scope.task.DueDateTime);
            $scope.task.TaskId = response.data;
            $scope.taskList.push($scope.task);
            $scope.task = {
                Priority: 1,
                DueDateTime: new Date()
            };
        },
        function (response) {
            alert('Epic fail!');
        });
    };

    $scope.newTaskClick = function () {
        $scope.NewTaskClicked = true;
    };

    $scope.saveTaskClick = function (currentTask) {
        currentTask.DueDateTime = currentTask.DueDateTime.getTime();
        $http.put('/tasks/' + currentTask.TaskId, currentTask);
    };

    $scope.isOverdue = function (currentItem) {
        return ((new Date(currentItem.DueDateTime)) - Date.now() < 0) && (!currentItem.IsCompleted);
    };

    LoadTaskList = function () {
        var promise = $http.get('/tasks');
        promise.then(function (response) {
            response.data.forEach(ConvertDate)
            $scope.taskList = response.data;
        },
        function (response) {
            alert('Epic fail!');
        });
    };

    ConvertDate = function (element, index, array) {
        element.DueDateTime=new Date(element.DueDateTime)
    };

    $scope.taskList = [];
    $scope.NewTaskClicked = false;
    LoadTaskList();
}

PageController.$inject = ['$scope', '$http'];