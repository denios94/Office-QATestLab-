using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QATestLab
{
    // Класс описывающий все констаты, что задействованы в данном проекте 
    static class Constant
    {
        public const int MIN_EMPLOYEE = 10; // Минимальное количество сотрудников
        public const int MAX_EMPLOYEE = 100; // Максимальное количество сотрудников
        public const int MAX_WEEK_HOURS = 40; // Максимальное количество рабочих часов в неделю для сотрудника
        public const string FILE_POSTS = "Posts.txt"; // Путь к файлу с информацией о должностях
        public const string FILE_LOGS = "Logs.txt"; // Путь к файлу с информацией о подробной работе офиса  
        public const string FILE_REPORT = "Report.txt"; // Путь к файлу отчета
        public const string FILE_SALARY = "Salary.txt"; // Путь к файлу с информацией о зарплате по каждому сотруднику  
        public const int WORK_WEEKS = 4; // Рабочие недели, после которых готовится суммарный отчет по выполненой работе и выданой зарплате
        public const int WORK_DAYS = 5; // Количество рабочих дней офиса в неделю
        public const int DAY_WORK_HOURS = 8; // Время работы офиса в день
        public const int DIRECTOR_POST_ID = 6; // ID должности директора
        public const int MIN_TASK_HOURS = 1; // Минимальное количество часов на выполнение задания
        public const int MAX_TASK_HOURS = 2; // Максимальное количество часов на выполнение задания
    }
}
