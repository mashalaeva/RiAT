using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Services
{
    public class FeedbackService
    {
        private readonly FeedbackDBContext _db;

        public FeedbackService(FeedbackDBContext db)
        {
            _db = db;
        }

        //--------------------------Create------------------------------------

        public void AddFeedback(int employeeId, int mark, string comment)
        {
            var employee = (from emp in _db.Employee
                where emp.Id == employeeId
                select emp).FirstOrDefault();
            if (employee == null || mark < 1 || mark > 5)
                return;

            int maxId = _db.Feedback.Max(x => x.Id);
            var fb = new Feedback()
            {
                Id = maxId + 1,
                Employee = employee,
                DateAndTime = DateTime.Now,
                Mark = mark,
                Comment = comment
            };
            _db.Feedback.Add(fb);
            _db.SaveChanges();
        }

        public void AddEmployee(Employee employee)
        {
            _db.Employee.Add(employee);
            _db.SaveChanges();
        }

        //--------------------------Read--------------------------------------

        public Feedback FindFeedbackById(int feedbackId)
            => (from fb in _db.Feedback
                where fb.Id == feedbackId
                select fb).FirstOrDefault();

        public List<Feedback> FindFeedbacksBySeveralDates(DateTime date1, DateTime date2)
            => (from fb in _db.Feedback
                where fb.DateAndTime.Date >= date1.Date && 
                      fb.DateAndTime.Date <= date2.Date
                orderby fb.DateAndTime
                select new Feedback
                {
                    Id = fb.Id,
                    DateAndTime = fb.DateAndTime,
                    Employee = fb.Employee,
                    Comment = fb.Comment,
                    Mark = fb.Mark
                }).ToList();
        
        public List<Feedback> FindFeedbacksByDate(DateTime date)
            => (from fb in _db.Feedback
                where fb.DateAndTime.Date == date.Date
                orderby fb.DateAndTime
                select new Feedback
                {
                    Id = fb.Id,
                    DateAndTime = fb.DateAndTime,
                    Employee = fb.Employee,
                    Comment = fb.Comment,
                    Mark = fb.Mark
                }).ToList();

        public List<Feedback> FindFeedbacksByEmployeeId(int id)
            => (from fb in _db.Feedback
                where fb.Employee.Id == id
                orderby fb.DateAndTime
                select new Feedback
                {
                    Id = fb.Id,
                    DateAndTime = fb.DateAndTime,
                    Employee = fb.Employee,
                    Comment = fb.Comment,
                    Mark = fb.Mark
                }).ToList();

        public List<Feedback> FindAllFeedbacks()
            => (from fb in _db.Feedback
                orderby fb.DateAndTime
                select new Feedback
                {
                    Id = fb.Id,
                    DateAndTime = fb.DateAndTime,
                    Employee = fb.Employee,
                    Comment = fb.Comment,
                    Mark = fb.Mark
                }).ToList();

        public Employee FindEmployeeById(int id)
            => (from emp in _db.Employee
                where emp.Id == id
                select emp).FirstOrDefault();

        public List<Employee> FindWorkingTodayEmployees()
            => (from emp in _db.Employee
                where emp.StartedWorking.Date == DateTime.Today
                select emp).ToList();

        public List<Employee> FindAllEmployees()
            => _db.Employee.ToList();

        //--------------------------Update------------------------------------

        public void UpdateEmployee(Employee employee)
        {
            var findEmployee = FindEmployeeById(employee.Id);
            if (findEmployee == null)
            {
                AddEmployee(employee);
            }
            else
            {
                findEmployee.Name = employee.Name;
                findEmployee.Surname = employee.Surname;
                findEmployee.Patronymic = employee.Patronymic;
                findEmployee.EndedWorking = employee.EndedWorking;
                findEmployee.StartedWorking = employee.StartedWorking;
            }

            _db.SaveChanges();
        }

        public void UpdateEmployeeStartWorkingDate(Employee employee, DateTime dateTime)
        {
            var findEmployee = FindEmployeeById(employee.Id);
            if (findEmployee == null)
            {
                employee.StartedWorking = dateTime;
                AddEmployee(employee);
            }
            else
            {
                findEmployee.StartedWorking = dateTime;
            }

            _db.SaveChanges();
        }

        public void UpdateEmployeeEndWorkingDate(Employee employee, DateTime dateTime)
        {
            var findEmployee = FindEmployeeById(employee.Id);
            if (findEmployee == null)
            {
                employee.EndedWorking = dateTime;
                AddEmployee(employee);
            }
            else
            {
                findEmployee.EndedWorking = dateTime;
            }

            _db.SaveChanges();
        }

        //--------------------------Delete------------------------------------

        private void DeleteEmployee(Employee employee)
        {
            try
            {
                DeleteFeedbackByEmployee(employee.Id);
                _db.Employee.Remove(employee);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteFeedback(Feedback feedback)
        {
            try
            {
                _db.Feedback.Remove(feedback);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteFeedbackById(int feedbackId)
        {
            DeleteFeedback(FindFeedbackById(feedbackId));
        }

        public void DeleteFeedbackByEmployee(int employeeId)
        {
            var employee = (from emp in _db.Employee
                where emp.Id == employeeId
                select emp).First();
            if (employee == null)
                return;

            var feedbacks = FindFeedbacksByEmployeeId(employeeId);
            foreach (var fb in feedbacks)
                DeleteFeedback(fb);
        }

        public void DeleteFeedbackByDate(DateTime date)
        {
            var feedbacks = FindFeedbacksByDate(date);
            foreach (var fb in feedbacks)
                DeleteFeedback(fb);
        }
    }
}