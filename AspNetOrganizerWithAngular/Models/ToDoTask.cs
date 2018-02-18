using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace AspNetOrganizerWithAngular.Models
{
    public enum TaskPriority
    {
        Low,
        Normal,
        High
    }

    [Table(Name = "ToDoTasks")]
    public class ToDoTask
    {
        [Column(Name = "task_id", IsPrimaryKey = true, CanBeNull = false)]
        public int? TaskId { get; set; }
        [Column(Name = "task_name", CanBeNull = false)]
        public string TaskName { get; set; }
        [Column(Name = "priority", CanBeNull = false)]
        public int Priority { get; set; }
        [Column(Name = "date_time_ticks", CanBeNull = false)]
        public long DueDateTime { get; set; }
        [Column(Name = "comment", CanBeNull = true)]
        public string Comment { get; set; }
        [Column(Name = "is_completed", CanBeNull = false)]
        public int IsCompleted { get; set; }
    }
}