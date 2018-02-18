using System.Data.Linq;
using System.Data.Linq.Mapping;
namespace AspNetOrganizerWithAngular.Models
{

    partial class TaskTableDataContext:DataContext
    {
        public Table<ToDoTask> ToDoTask;
    }
}