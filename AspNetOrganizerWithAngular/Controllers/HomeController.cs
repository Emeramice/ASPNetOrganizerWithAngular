using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.IO;
using System.Data.SQLite;
using System.Web.Mvc;
using System.Web.Http;
using AspNetOrganizerWithAngular.Models;
using Newtonsoft.Json;

namespace AspNetOrganizerWithAngular.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                DataContext taskDbDataContext = new DataContext(taskDbConnection);
            }
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddNewTask([FromBody] ToDoTask result)
        {
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                ToDoTaskForDatabase currentItem = new ToDoTaskForDatabase();
                currentItem.TaskId = null;
                currentItem.TaskName = result.TaskName;
                currentItem.Priority = result.Priority;
                currentItem.DueDateTimeTicks = result.DueDateTime.Ticks;
                currentItem.Comment = result.Comment;
                currentItem.IsCompleted = 0;
                taskDbDataContext.ToDoTask.InsertOnSubmit(currentItem);
                taskDbDataContext.SubmitChanges();
            }
            return View("Index");
        }

        [System.Web.Http.HttpGet]
        public void GetTaskList()
        {
            string jsonData = "";
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                Table<ToDoTaskForDatabase> ToDoTaskDataSet = taskDbDataContext.GetTable<ToDoTaskForDatabase>();
                var TasksSet = from SetItem in ToDoTaskDataSet select new ToDoTask { TaskId = (int)SetItem.TaskId, TaskName = SetItem.TaskName, DueDateTime = new DateTime(SetItem.DueDateTimeTicks), IsCompleted = SetItem.IsCompleted == 1 ? true : false };
                jsonData = JsonConvert.SerializeObject(TasksSet);
                Response.Clear();
                Response.Write(jsonData);
                Response.End();
            }
        }

        [System.Web.Http.HttpPut]
        public ActionResult ChangeItem([FromBody] ToDoTask result)
        {
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                ToDoTaskForDatabase currentItem = taskDbDataContext.ToDoTask.Single(item => item.TaskId == result.TaskId);
                currentItem.IsCompleted = result.IsCompleted ? 1 : 0;
                taskDbDataContext.SubmitChanges();
            }
            return View("Index");
        }
    }
}