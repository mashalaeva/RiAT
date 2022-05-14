using System;
using System.Collections.Generic;
using System.IO;
using Faker;

namespace Services
{
    public static class DbFaker
    {
        private static readonly string[] FemaleFirstNames =
        {
            "Александра", "Алёна", "Алеся", "Алина",
            "Алиса", "Анастасия", "Ангелина", "Анна",
            "Арина", "Валентина", "Валерия", "Варвара",
            "Вера", "Вероника", "Дарья", "Диана", "Дина",
            "Ева", "Евгения", "Екатерина", "Елена",
            "Елизавета", "Жанна", "Зоя", "Злата", "Ирина",
            "Кристина", "Ксения", "Лариса", "Лилия",
            "Любовь", "Лидия", "Марина", "Мария", "Марьяна",
            "Надежда", "Наталия", "Ника", "Нина", "Оксана",
            "Олеся", "Ольга", "Полина", "Светлана", "Софья",
            "Татьяна", "Ульяна", "Эльвира", "Юлия", "Яна"
        };

        private static readonly string[] FemaleLastNames =
        {
            "Зайцева", "Соловьева", "Борисова", "Яковлева",
            "Григорьева", "Романова", "Воробьева", "Сергеева",
            "Кузьмина", "Фролова", "Александрова", "Дмитриева",
            "Королева", "Гусева", "Киселева", "Ильина",
            "Максимова", "Полякова", "Сорокина", "Виноградова",
            "Ковалева", "Белова", "Медведева", "Антонова",
            "Тарасова"
        };

        private static readonly string[] FemalePatronymics =
        {
            "Артёмовна", "Александровна", "Максимовна",
            "Дмитриевна", "Ивановна", "Кирилловна",
            "Михайловна", "Егоровна", "Андреевна",
            "Ильинишна", "Алексеевна", "Романовна",
            "Сергеевна", "Владиславовна", "Ярославовна",
            "Тимофеевна", "Денисовна", "Владимировна",
            "Павловна", "Константиновна", "Евгеньевна",
            "Николаевна", "Степановна", "Захаровна"
        };

        private static readonly string[] MaleFirstNames =
        {
            "Артём", "Александр", "Максим", "Даниил",
            "Дмитрий", "Иван", "Кирилл", "Никита",
            "Михаил", "Егор", "Матвей", "Андрей",
            "Илья", "Алексей", "Роман", "Сергей",
            "Владислав", "Ярослав", "Тимофей", "Арсений",
            "Денис", "Владимир", "Павел", "Глеб", "Константин",
            "Евгений", "Николай", "Степан", "Захар"
        };

        private static readonly string[] MaleLastNames =
        {
            "Иванов", "Смирнов", "Кузнецов", "Попов",
            "Васильев", "Петров", "Соколов", "Михайлов",
            "Новиков", "Федоров", "Морозов", "Волков",
            "Алексеев", "Лебедев", "Семенов", "Егоров",
            "Павлов", "Козлов", "Степанов", "Николаев",
            "Орлов", "Андреев", "Макаров", "Никитин",
            "Захаров"
        };

        private static readonly string[] MalePatronymics =
        {
            "Артёмович", "Александрович", "Максимович",
            "Дмитриевич", "Иванович", "Кириллович",
            "Михайлович", "Егорович", "Андреевич",
            "Ильич", "Алексеевич", "Романович", "Сергеевич",
            "Владиславович", "Ярославович", "Тимофеевич",
            "Денисович", "Владимирович", "Павлович",
            "Константинович", "Евгеньевич", "Николаевич",
            "Степанович", "Захарович"
        };

        private static string GenerateFemaleFirstName()
            => FemaleFirstNames[Number.RandomNumber(0, FemaleFirstNames.Length)];

        private static string GenerateFemaleLastName()
            => FemaleLastNames[Number.RandomNumber(0, FemaleLastNames.Length)];

        private static string GenerateFemalePatronymic()
            => FemalePatronymics[Number.RandomNumber(0, FemalePatronymics.Length)];

        private static string GenerateMaleFirstName()
            => MaleFirstNames[Number.RandomNumber(0, MaleFirstNames.Length)];

        private static string GenerateMaleLastName()
            => MaleLastNames[Number.RandomNumber(0, MaleLastNames.Length)];

        private static string GenerateMalePatronymic()
            => MalePatronymics[Number.RandomNumber(0, MalePatronymics.Length)];

        // Для генерации дня рождения.
        /*private static DateTime GenerateBirthdate()
            => Date.Between(new DateTime(1970, 1, 1),
                new DateTime(2004, 3, 1));*/

        // Для генерации номера трудовой книжки.
        /*private static readonly string[] BookSeries =
        {
            "ТК ", "ТК-I ", "ТК-II ", "ТК-III ", "ТК-IV ", "ТК-V ", "ТК-VI "
        };*/

        private static void GenerateStartAndEndWorkingDate(out DateTime start, out DateTime end)
        {
            start = Date.Between(new DateTime(2022, 4, 10),
                new DateTime(2022, 5, 10));
            
            while (start.Hour < 7 || start.Hour >= 18)
                start = Date.Between(new DateTime(2022, 4, 10),
                    new DateTime(2022, 5, 10));

            if (start.Date != new DateTime(2022, 5, 10))
            {
                end = start.Add(new TimeSpan(4, Number.RandomNumber(0, 60), 
                    Number.RandomNumber(0, 60)));
            }
            else
            {
                end = Date.Between(new DateTime(2022, 4, 10),
                    new DateTime(2022, 5, 9));
            }
        }

        public static void GenerateEmployees(int num, ref List<string> result)
        {
            for (int i = 1; i <= num; i++)
            {
                GenerateStartAndEndWorkingDate(out var startDateTime, out var endDateTime);
                /*var employmentRecordNumber = string.Concat(
                    BookSeries[Number.RandomNumber(0, BookSeries.Length - 1)],
                    Number.RandomNumber(1000000, 9999999));*/
                if (i <= num / 2)
                    result.Add($"INSERT INTO public.\"Employee\" VALUES({i}, " +
                               $"'{GenerateMaleLastName()}', " +
                               $"'{GenerateMaleFirstName()}', " +
                               $"'{GenerateMalePatronymic()}', " +
                               $"TO_TIMESTAMP('{startDateTime.Date.Day}.{startDateTime.Date.Month}." +
                               $"{startDateTime.Date.Year}.{startDateTime.Hour}.{startDateTime.Minute}." +
                               $"{startDateTime.Second}', 'dd-mm-yyyy-hh24-mi-ss'), " +
                               $"TO_TIMESTAMP('{endDateTime.Date.Day}.{endDateTime.Date.Month}." +
                               $"{endDateTime.Date.Year}.{endDateTime.Hour}.{endDateTime.Minute}." +
                               $"{endDateTime.Second}', 'dd-mm-yyyy-hh24-mi-ss'));");
                else
                    result.Add($"INSERT INTO public.\"Employee\" VALUES({i}, " +
                               $"'{GenerateFemaleLastName()}', " +
                               $"'{GenerateFemaleFirstName()}', " +
                               $"'{GenerateFemalePatronymic()}', " +
                               $"TO_TIMESTAMP('{startDateTime.Date.Day}.{startDateTime.Date.Month}." +
                               $"{startDateTime.Date.Year}.{startDateTime.Hour}.{startDateTime.Minute}." +
                               $"{startDateTime.Second}', 'dd-mm-yyyy-hh24-mi-ss'), " +
                               $"TO_TIMESTAMP('{endDateTime.Date.Day}.{endDateTime.Date.Month}." +
                               $"{endDateTime.Date.Year}.{endDateTime.Hour}.{endDateTime.Minute}." +
                               $"{endDateTime.Second}', 'dd-mm-yyyy-hh24-mi-ss'));");
            }
        }

        public static void GenerateFeedbacks(int feedbacksNum, int employeesNum,
            ref List<string> result)
        {
            for (int i = 1; i <= feedbacksNum; i++)
            {
                DateTime dt = Date.Between(new DateTime(2022, 4, 10, 7, 0, 0),
                    new DateTime(2022, 5, 10, 22, 0, 0));
                while (dt.Hour < 7 || dt.Hour >= 22)
                    dt = Date.Between(new DateTime(2022, 4, 10, 7, 0, 0),
                        new DateTime(2022, 5, 10, 22, 0, 0));
                result.Add($"INSERT INTO public.\"Feedback\" VALUES({i}, " +
                           $"{Number.RandomNumber(1, employeesNum + 1)}, " +
                           $"{Number.RandomNumber(1, 6)}, " +
                           $"'{Lorem.Sentence(Number.RandomNumber(3, 10))}', " +
                           $"TO_TIMESTAMP('{dt.Date.Day}.{dt.Date.Month}.{dt.Date.Year}." +
                           $"{dt.Hour}.{dt.Minute}.{dt.Second}', " +
                           "'dd-mm-yyyy-hh24-mi-ss'));");
            }
        }
    }
}