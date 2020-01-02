using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test005_Blazor.Data
{
    public class TodoItem
    {
        public int Id { get; set; } // идентификатор задачи
        public string Name { get; set; } // название задачи
        public string Description { get; set; } // описание задачи
        public bool IsComplete { get; set; } // признак выполнена задача или нет
        public string UserId { get; set; } // идентификатор пользователя
        public DateTime CreationDate { get; set; } // дата создания задачи
    }
}
