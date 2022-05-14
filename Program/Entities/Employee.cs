using System;

namespace Entities
{
    public class Employee
    {
        // Идентификационный номер.
        public int Id { get; set; }

        // Фамилия сотрудника.
        public string Surname { get; set; }

        // Имя сотрудника.
        public string Name { get; set; }

        // Отчество сотрудника.
        public string Patronymic { get; set; }

        // Время начала работы.
        public DateTime StartedWorking { get; set; }

        // Время окончания работы.
        public DateTime EndedWorking { get; set; }
    }
}