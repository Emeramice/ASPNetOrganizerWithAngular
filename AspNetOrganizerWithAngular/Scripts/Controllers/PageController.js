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
        var promise = $http.get('/Home/GetTaskList');
        promise.then(function (response) {
            $scope.taskList = response.data;
        },
        function (response) {
            alert('Epic fail!');
        });
    };

    $scope.addNewTask = function () {
        $http.post('/Home/AddNewTask', $scope.task);
        $scope.NewTaskClicked = false;
        $scope.taskList.push($scope.task);
    };
    $scope.newTaskClick = function () {
        $scope.NewTaskClicked = true;
    };
    $scope.saveTaskClick = function (currentTask) {
        $http.put('/Home/ChangeItem', currentTask);
    };
    $scope.isOverdue = function (currentItem) {
        return (Date.parse(currentItem.DueDateTime) - Date.now() < 0) && (!currentItem.IsCompleted);
    };
    LoadTaskList();
}

PageController.$inject = ['$scope', '$http'];