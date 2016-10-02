using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.IO;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNetOrganizerWithAngular.Models;
using AspNetOrganizerWithAngular.Controllers;

namespace OrganizerUnitTests
{
    [TestClass]
    public class OrganizerTests
    {
        [TestMethod]
        public void AddNewTaskTest()
        {
            ToDoTask NewTask = new ToDoTask();
            NewTask.TaskName = "Task1";
            NewTask.Priority = 1;
            NewTask.IsCompleted = false;
            NewTask.Comment = "Task1";
            HomeController NewContr = new HomeController();
            NewContr.AddNewTask(NewTask);
            string taskDbConnectionString = "Data source =" + HttpContext.Current.Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                Table<ToDoTaskForDatabase> ToDoTaskDataSet = taskDbDataContext.GetTable<ToDoTaskForDatabase>();
                var TasksSet = (from SetItem in ToDoTaskDataSet select new ToDoTask { TaskId = (int)SetItem.TaskId, TaskName = SetItem.TaskName, DueDateTime = new DateTime(SetItem.DueDateTimeTicks), IsCompleted = SetItem.IsCompleted == 1 ? true : false }).Last();
                Assert.AreEqual("Task1", TasksSet.TaskName);
            }
        }
    }
}
