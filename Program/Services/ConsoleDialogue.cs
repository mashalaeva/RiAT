using System;
using System.Collections.Generic;
using Entities;

namespace Services
{
    public class ConsoleDialogue
    {
        private FeedbackService _service;

        public void StartDialog()
        {
            do
            {
                _service = new FeedbackService(new FeedbackDBContext());

                Console.WriteLine("Список команд:\n" +
                                  "Esc \t | Выйти из приложения\n" +
                                  "Q \t | Вывести содержимое всех таблиц\n" +
                                  "W \t | Вывести все отзывы по дате\n" +
                                  "E \t | Вывести отзывы по одному сотруднику\n" +
                                  "R \t | Добавить отзыв\n" +
                                  "T \t | Удалить сотрудника и отзывы на него по ID сотрудника\n" +
                                  "Y \t | Открыть смену сотрудника (сменить время начала работы на текущее)\n" +
                                  "U \t | Закрыть смену сотрудника (сменить время окончания работы на текущее)\n" +
                                  "I \t | Создать фейковые данные\n");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q:
                        PrintAllTables();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.W:
                        PrintFeedbacksByDate();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.E:
                        PrintFeedbacksByEmployee();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.R:
                        AddFeedback();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.T:
                        DeleteEmployeeById();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.Y:
                        ChangeStartedWorkingTime();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.U:
                        ChangeEndedWorkingTime();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.I:
                        CreateFakeData();
                        Console.WriteLine("Нажмите любую кнопку для продолжения или Escape для выхода");
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private void PrintAllTables()
        {
            List<Employee> employees = _service.FindAllEmployees();
            Console.WriteLine("Сотрудники:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID сотрудника: {employee.Id}, ФИО: {employee.Surname} " +
                                  $"{employee.Name} {employee.Patronymic}, " +
                                  "дата и время начала смены: " +
                                  $"{employee.StartedWorking.Date.Day}." +
                                  $"{employee.StartedWorking.Date.Month}." +
                                  $"{employee.StartedWorking.Date.Year} " +
                                  $"{employee.StartedWorking.Hour}:" +
                                  $"{employee.StartedWorking.Minute}:" +
                                  $"{employee.StartedWorking.Second}, " +
                                  "дата и время окончания смены: " +
                                  $"{employee.EndedWorking.Date.Day}." +
                                  $"{employee.EndedWorking.Date.Month}." +
                                  $"{employee.EndedWorking.Date.Year} " +
                                  $"{employee.EndedWorking.Hour}:" +
                                  $"{employee.EndedWorking.Minute}:" +
                                  $"{employee.EndedWorking.Second};");
            }

            Console.WriteLine("\nОтзывы:");
            List<Feedback> feedbacks = _service.FindAllFeedbacks();
            foreach (var feedback in feedbacks)
            {
                Console.WriteLine($"ID отзыва: {feedback.Id}, ID сотрудника: {feedback.Employee.Id}, " +
                                  $"оценка: {feedback.Mark}, комментарий: \"{feedback.Comment}\", " +
                                  $"дата и время: {feedback.DateAndTime.Date.Day}." +
                                  $"{feedback.DateAndTime.Date.Month}.{feedback.DateAndTime.Date.Year} " +
                                  $"{feedback.DateAndTime.Hour}:{feedback.DateAndTime.Minute}:" +
                                  $"{feedback.DateAndTime.Second};");
            }
        }

        private void PrintFeedbacksByDate()
        {
            Console.WriteLine("Введите дату в формате ДД.ММ.ГГГГ в диапазоне от 10.04.2022 до " +
                              $"{DateTime.Today.Day}.{DateTime.Today.Month}.{DateTime.Today.Year}:");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date) ||
                   date.Date < new DateTime(2022, 4, 10) ||
                   date.Date > DateTime.Today)
                Console.WriteLine(
                    "Необходимо ввести дату в формате ДД.ММ.ГГГГ в диапазоне от 10.04.2022 до " +
                    $"{DateTime.Today.Day}.{DateTime.Today.Month}.{DateTime.Today.Year}!");
            try
            {
                var list = _service.FindFeedbacksByDate(date);
                foreach (var feedback in list)
                {
                    Console.WriteLine($"ID отзыва: {feedback.Id}, индекс сотрудника: {feedback.Employee.Id}, " +
                                      $"оценка: {feedback.Mark}, дата и время: {feedback.DateAndTime.Date.Day}." +
                                      $"{feedback.DateAndTime.Date.Month}.{feedback.DateAndTime.Date.Year} " +
                                      $"{feedback.DateAndTime.Hour}:{feedback.DateAndTime.Minute}:" +
                                      $"{feedback.DateAndTime.Second}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PrintFeedbacksByEmployee()
        {
            var employees = _service.FindAllEmployees();
            Console.WriteLine("Введите идентификационный номер (ID) сотрудника из следующего списка:");
            foreach (var emp in employees)
                Console.Write(emp.Id + " ");

            Console.WriteLine();

            int id;
            while (!int.TryParse(Console.ReadLine(), out id) ||
                   _service.FindEmployeeById(id) == null)
                Console.WriteLine("Необходимо ввести идентификационный номер (ID) сотрудника из списка выше!");
            try
            {
                var list = _service.FindFeedbacksByEmployeeId(id);
                foreach (var feedback in list)
                {
                    Console.WriteLine($"ID отзыва: {feedback.Id}, ID сотрудника: {feedback.Employee.Id}, " +
                                      $"оценка: {feedback.Mark}, комментарий: \"{feedback.Comment}\", " +
                                      $"дата и время: {feedback.DateAndTime.Date.Day}." +
                                      $"{feedback.DateAndTime.Date.Month}.{feedback.DateAndTime.Date.Year} " +
                                      $"{feedback.DateAndTime.Hour}:{feedback.DateAndTime.Minute}:" +
                                      $"{feedback.DateAndTime.Second};");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AddFeedback()
        {
            Console.WriteLine("Чтобы добавить отзыв, введите идентификационный номер (ID) сотрудника, " +
                              "оценку от 1 до 5 и комментарий через «; »,\n" +
                              "после чего будет выведен список всех " +
                              "отзывов данного сотрудника.");
            string[] str = Console.ReadLine()!.Trim().Split("; ");
            while (str.Length != 3)
            {
                Console.WriteLine("Вы должны ввести ровно 3 значения через «; »");
                str = Console.ReadLine()!.Trim().Split(" ");
            }

            try
            {
                _service.AddFeedback(int.Parse(str[0]), int.Parse(str[1]), str[2]);
                var feedbacks = _service.FindFeedbacksByEmployeeId(int.Parse(str[0]));
                foreach (var feedback in feedbacks)
                {
                    Console.WriteLine($"ID отзыва: {feedback.Id}, ID сотрудника: {feedback.Employee.Id}, " +
                                      $"оценка: {feedback.Mark}, комментарий: \"{feedback.Comment}\", " +
                                      $"дата и время: {feedback.DateAndTime.Date.Day}." +
                                      $"{feedback.DateAndTime.Date.Month}.{feedback.DateAndTime.Date.Year} " +
                                      $"{feedback.DateAndTime.Hour}:{feedback.DateAndTime.Minute}:" +
                                      $"{feedback.DateAndTime.Second};");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Не удалось добавить отзыв, поскольку во введенной " +
                                  "строке содержались недопустимые значения." +
                                  "\nПопробуйте снова");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось добавить отзыв из-за ошибки " +
                                  $"\"{ex.Message}\"\n" +
                                  "Попробуйте снова");
            }
        }
        
        private void DeleteEmployeeById()
        {
            Console.WriteLine("Чтобы удалить данные о сотруднике, введите его идентификационный номер (ID).\n" +
                              "Список ID существуюющих в базе сотрудников:");

            var employees = _service.FindAllEmployees();
            foreach (var emp in employees)
                Console.Write(emp.Id + " ");

            Console.WriteLine("\nПосле удаления будет выведен список текущих сотрудников и отзывов.");

            int id;
            while (!int.TryParse(Console.ReadLine(), out id) ||
                   _service.FindEmployeeById(id) == null)
                Console.WriteLine("Необходимо ввести идентификационный номер (ID) сотрудника из списка выше!");

            try
            {
                _service.DeleteFeedbackById(id);
                Console.WriteLine("Успех! Текущий список сотрудников выглядит следующим образом:");
                employees = _service.FindAllEmployees();
                foreach (var employee in employees)
                {
                    Console.WriteLine($"ID сотрудника: {employee.Id}, ФИО: {employee.Surname} " +
                                      $"{employee.Name} {employee.Patronymic}, " +
                                      "дата и время начала смены: " +
                                      $"{employee.StartedWorking.Date.Day}." +
                                      $"{employee.StartedWorking.Date.Month}." +
                                      $"{employee.StartedWorking.Date.Year} " +
                                      $"{employee.StartedWorking.Hour}:" +
                                      $"{employee.StartedWorking.Minute}:" +
                                      $"{employee.StartedWorking.Second}, " +
                                      "дата и время окончания смены: " +
                                      $"{employee.EndedWorking.Date.Day}." +
                                      $"{employee.EndedWorking.Date.Month}." +
                                      $"{employee.EndedWorking.Date.Year} " +
                                      $"{employee.EndedWorking.Hour}:" +
                                      $"{employee.EndedWorking.Minute}:" +
                                      $"{employee.EndedWorking.Second};");
                }
                Console.WriteLine("Текущий список отзывов выглядит следующим образом:");
                var feedbacks = _service.FindAllFeedbacks();
                foreach (var feedback in feedbacks)
                {
                    Console.WriteLine($"ID отзыва: {feedback.Id}, ID сотрудника: {feedback.Employee.Id}, " +
                                      $"оценка: {feedback.Mark}, комментарий: \"{feedback.Comment}\", " +
                                      $"дата и время: {feedback.DateAndTime.Date.Day}." +
                                      $"{feedback.DateAndTime.Date.Month}.{feedback.DateAndTime.Date.Year} " +
                                      $"{feedback.DateAndTime.Hour}:{feedback.DateAndTime.Minute}:" +
                                      $"{feedback.DateAndTime.Second};");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeStartedWorkingTime()
        {
            Console.WriteLine("Чтобы изменить время прихода сотрудника на текущее," +
                              " введите идентификационный номер (ID) сотрудника" +
                              " из следующего списка:");

            var employees = _service.FindAllEmployees();
            foreach (var emp in employees)
                Console.Write(emp.Id + " ");

            Console.WriteLine("\nПосле изменения времени будут выведены данные о сотруднике.");

            int id;
            while (!int.TryParse(Console.ReadLine(), out id) ||
                   _service.FindEmployeeById(id) == null)
                Console.WriteLine("Необходимо ввести идентификационный номер (ID) сотрудника из списка выше!");

            try
            {
                _service.UpdateEmployeeStartWorkingDate(_service.FindEmployeeById(id), DateTime.Now);
                Console.WriteLine("Успех! Данные о сотруднике выглядят следующим образом:");
                var employee = _service.FindEmployeeById(id);
                Console.WriteLine($"ID сотрудника: {employee.Id}, ФИО: {employee.Surname} " +
                                  $"{employee.Name} {employee.Patronymic}, " +
                                  "дата и время начала смены: " +
                                  $"{employee.StartedWorking.Date.Day}." +
                                  $"{employee.StartedWorking.Date.Month}." +
                                  $"{employee.StartedWorking.Date.Year} " +
                                  $"{employee.StartedWorking.Hour}:" +
                                  $"{employee.StartedWorking.Minute}:" +
                                  $"{employee.StartedWorking.Second}, " +
                                  "дата и время окончания смены: " +
                                  $"{employee.EndedWorking.Date.Day}." +
                                  $"{employee.EndedWorking.Date.Month}." +
                                  $"{employee.EndedWorking.Date.Year} " +
                                  $"{employee.EndedWorking.Hour}:" +
                                  $"{employee.EndedWorking.Minute}:" +
                                  $"{employee.EndedWorking.Second};");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeEndedWorkingTime()
        {
            Console.WriteLine("Чтобы изменить время закрытия смены сотрудником на текущее," +
                              " введите идентификационный номер (ID) сотрудника" +
                              " из следующего списка:");

            var employees = _service.FindAllEmployees();
            foreach (var emp in employees)
                Console.Write(emp.Id + " ");

            Console.WriteLine("\nПосле изменения времени будут выведены данные о сотруднике.");

            int id;
            while (!int.TryParse(Console.ReadLine(), out id) ||
                   _service.FindEmployeeById(id) == null)
                Console.WriteLine("Необходимо ввести идентификационный номер (ID) сотрудника из списка выше!");

            try
            {
                _service.UpdateEmployeeEndWorkingDate(_service.FindEmployeeById(id), DateTime.Now);
                Console.WriteLine("Успех! Данные о сотруднике выглядят следующим образом:");
                var employee = _service.FindEmployeeById(id);
                Console.WriteLine($"ID сотрудника: {employee.Id}, ФИО: {employee.Surname} " +
                                  $"{employee.Name} {employee.Patronymic}, " +
                                  "дата и время начала смены: " +
                                  $"{employee.StartedWorking.Date.Day}." +
                                  $"{employee.StartedWorking.Date.Month}." +
                                  $"{employee.StartedWorking.Date.Year} " +
                                  $"{employee.StartedWorking.Hour}:" +
                                  $"{employee.StartedWorking.Minute}:" +
                                  $"{employee.StartedWorking.Second}, " +
                                  "дата и время окончания смены: " +
                                  $"{employee.EndedWorking.Date.Day}." +
                                  $"{employee.EndedWorking.Date.Month}." +
                                  $"{employee.EndedWorking.Date.Year} " +
                                  $"{employee.EndedWorking.Hour}:" +
                                  $"{employee.EndedWorking.Minute}:" +
                                  $"{employee.EndedWorking.Second};");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateFakeData()
        {
            Console.WriteLine("Введите количество сотрудников и отзывов через пробел.");
            string[] args = Console.ReadLine()!.Trim().Split(" ");
            while (args.Length != 2)
            {
                Console.WriteLine("Вы должны ввести ровно 2 значений через пробел");
                args = Console.ReadLine()!.Trim().Split(" ");
            }

            try
            {
                List<string> employees = new List<string>();
                DbFaker.GenerateEmployees(int.Parse(args[0]), ref employees);

                Console.WriteLine("Employees:");
                foreach (var emp in employees)
                    Console.WriteLine(emp);

                List<string> feedbacks = new List<string>();
                DbFaker.GenerateFeedbacks(int.Parse(args[1]), int.Parse(args[0]), ref feedbacks);

                Console.WriteLine("\nFeedbacks:");
                foreach (var fb in feedbacks)
                    Console.WriteLine(fb);
            }
            catch (FormatException)
            {
                Console.WriteLine("Не удалось сгенерировать данные, поскольку во введенной " +
                                  "строке содержались недопустимые значения." +
                                  "\nПопробуйте снова");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось сгенерировать данные из-за ошибки " +
                                  $"\"{ex.Message}\"\n" +
                                  "Попробуйте снова");
            }
        }
    }
}