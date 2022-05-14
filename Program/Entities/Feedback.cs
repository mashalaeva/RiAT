using System;

namespace Entities
{
    public class Feedback
    {
        // Идентификационный номер.
        public int Id { get; set; }

        // Сотрудник, которому поставили оценку.
        public Employee Employee { get; set; }

        // Оценка, выставленная сотруднику.
        public int Mark { get; set; }

        // Комментарий.
        public string Comment { get; set; }
        
        // Дата и время выставления оценки.
        public DateTime DateAndTime { get; set; }
    }
}