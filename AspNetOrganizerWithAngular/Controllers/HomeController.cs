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

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Route("tasks")]
        public void AddNewTask([FromBody] ToDoTask result)
        {
            int lastItem = 0;
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            ToDoTask currentItem = new ToDoTask();
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                currentItem.TaskId = null;
                currentItem.TaskName = result.TaskName;
                currentItem.Priority = result.Priority;
                currentItem.DueDateTime = result.DueDateTime;
                currentItem.Comment = result.Comment;
                currentItem.IsCompleted = 0;
                taskDbDataContext.ToDoTask.InsertOnSubmit(currentItem);
                taskDbDataContext.SubmitChanges();
                Table<ToDoTask> ToDoTaskDataSet = taskDbDataContext.GetTable<ToDoTask>();
                using (SQLiteCommand lastItemCmd = taskDbConnection.CreateCommand())
                {
                    taskDbConnection.Open();
                    lastItemCmd.CommandText = "select seq from sqlite_sequence where name='ToDoTasks'";
                    SQLiteDataReader lastItemRdr = lastItemCmd.ExecuteReader();
                    lastItemRdr.Read();
                    lastItem = lastItemRdr.GetInt32(0);
                }
            }
            Response.Clear();
            Response.Write(lastItem);
            Response.End();
        }

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("tasks")]
        public void GetTaskList()
        {
            string jsonData = "";
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                Table<ToDoTask> ToDoTaskDataSet = taskDbDataContext.GetTable<ToDoTask>();
                var TasksSet = from SetItem in ToDoTaskDataSet select new { TaskId = (int)SetItem.TaskId, TaskName = SetItem.TaskName, DueDateTime = SetItem.DueDateTime, IsCompleted = SetItem.IsCompleted };
                jsonData = JsonConvert.SerializeObject(TasksSet);
                Response.Clear();
                Response.Write(jsonData);
                Response.End();
            }
        }

        [System.Web.Mvc.HttpPut]
        [System.Web.Mvc.Route("tasks/{taskId}")]
        public ActionResult ChangeItem(int taskId, [FromBody] ToDoTask result)
        {
            string taskDbConnectionString = "Data source =" + Server.MapPath("~/") + "TaskBase.db";
            using (SQLiteConnection taskDbConnection = new SQLiteConnection(taskDbConnectionString))
            {
                TaskTableDataContext taskDbDataContext = new TaskTableDataContext(taskDbConnection);
                ToDoTask currentItem = taskDbDataContext.ToDoTask.Single(item => item.TaskId == result.TaskId);
                currentItem.IsCompleted = result.IsCompleted;
                taskDbDataContext.SubmitChanges();
            }
            return View("Index");
        }
    }
}