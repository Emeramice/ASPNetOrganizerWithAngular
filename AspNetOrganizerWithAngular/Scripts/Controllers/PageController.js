var PageController = function ($scope, $http) {
    $scope.task = {
        TaskName: '',
        Priority: 0,
        DueDateTime: undefined,
        Comment: '',
        IsCompleted: false
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
    $scope.taskList = [];
    $scope.NewTaskClicked = false;
    LoadTaskList = function () {
        var promise = $http.get('/tasks');
        promise.then(function (response) {
            $scope.taskList = response.data;
        },
        function (response) {
            alert('Epic fail!');
        });
    };

    $scope.addNewTask = function () {
        var promise = $http.post('/tasks', $scope.task);
        promise.then(function (response) {
            $scope.NewTaskClicked = false;
            var time = $scope.task.DueDateTime;
            var dateString = time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate();
            $scope.task.DueDateTime = dateString;
            $scope.task.TaskId = response.data;
            $scope.taskList.push($scope.task);
            $scope.task = {};
        },
        function (response) {
            alert('Epic fail!');
        });
    };
    $scope.newTaskClick = function () {
        $scope.NewTaskClicked = true;
    };
    $scope.saveTaskClick = function (currentTask) {
        $http.put('/tasks/' + currentTask.TaskId, currentTask);
    };
    $scope.isOverdue = function (currentItem) {
        return (Date.parse(currentItem.DueDateTime) - Date.now() < 0) && (!currentItem.IsCompleted);
    };
    LoadTaskList();
}

PageController.$inject = ['$scope', '$http'];