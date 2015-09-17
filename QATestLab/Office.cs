using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QATestLab
{
    class Office
    {
        Random MyRandom; // Генератор случайных чисел
        List<Post> Posts; // Список должностей
        List<Task> Tasks; // Список заданий
        List<int> ImportantPostIDs; // Список ID-ишниками "важных" должностей
        List<Employee> FreeEmployees; // Список свободных сотрудников
        List<Employee> BusyEmployees; // Список занятых сотрудников
        int PostAmount; // Количество должностей 
        int EmployeeAmount; // Количество сотрудников
        StreamWriter Logs; // Поток для записи в файл с логами
        StreamWriter Salary; // Поток для записи в файл с логами
        StreamWriter Report; // Поток для записи в файл с отчетом

        public Office()
        {
            MyRandom = MyRandom = new Random(DateTime.Now.Millisecond);
            EmployeeAmount = MyRandom.Next(Constant.MIN_EMPLOYEE, Constant.MAX_EMPLOYEE);
            Posts = InitPosts();
            PostAmount = Posts.Count;
            ImportantPostIDs = InitImportantPostIDs();
            FreeEmployees = InitEmployees();
            BusyEmployees = new List<Employee>();
            Tasks = new List<Task>();
            Logs = new StreamWriter(Constant.FILE_LOGS);
            Salary = new StreamWriter(Constant.FILE_SALARY);
            Report = new StreamWriter(Constant.FILE_REPORT);
        }

        // Метод возвращающий список должностей считанных с файла
        List<Post> InitPosts()
        {
            string SepareteLine = "------------------";
            List<Post> Posts = new List<Post>();
            StreamReader File = new StreamReader(Constant.FILE_POSTS);
            string Line = null;
            int PropertyAmount = 6;
            int ID = 0;
            string Title = null;
            string Duty = null;
            int Salary = 0;
            int EaringsPerHour = 0;
            bool Importance = false;

            while ((Line = File.ReadLine()) != null)
            {
                if (Line.Contains(SepareteLine))
                {
                    for (int Property = 0; Property < PropertyAmount; Property++)
                    {
                        Line = File.ReadLine();
                        switch (Property)
                        {
                            case 0:
                                ID = Convert.ToInt32(Line.Replace("ID: ", ""));
                                break;
                            case 1:
                                Title = Convert.ToString(Line.Replace("Title: ", ""));
                                break;
                            case 2:
                                Duty = Convert.ToString(Line.Replace("Duty: ", ""));
                                break;
                            case 3:
                                Salary = Convert.ToInt32(Line.Replace("Salary: ", ""));
                                break;
                            case 4:
                                EaringsPerHour = Convert.ToInt32(Line.Replace("EaringsPerHour: ", ""));
                                break;
                            case 5:
                                Importance = Convert.ToBoolean(Line.Replace("Importance: ", ""));
                                break;
                        }
                    }

                    Posts.Add(new Post(ID, Title, Duty, Salary, EaringsPerHour, Importance));
                }
            }

            File.Close();

            return Posts;
        }

        // Метод возвращающий список сотрудников, созданый на основе сущесвтующих должностей и количества сотрудников
        List<Employee> InitEmployees()
        {
            List<Employee> Employees = new List<Employee>();

            // Добавляем по одному сотруднику на кажду важную должность
            for (int i = 1; i <= ImportantPostIDs.Count; i++)
                Employees.Add(new Employee(i, "Employye" + i, new List<int>(new int[] {ImportantPostIDs[i - 1]})));

            // Добавляем остальных сотрудников
            for (int i = ImportantPostIDs.Count + 1; i <= EmployeeAmount; i++)
                Employees.Add(new Employee(i, "Employye" + i, RandomOccupiedPosts()));
            
            return Employees;
        }

        // Метод возвращающий список всех ID-ишников "важных" должностей
        List<int> InitImportantPostIDs()
        {
            List<int> ImportantPostIDs = new List<int>();

            // Заполняем список ID-ишниками "важных" должностей
            foreach (Post ImportantPost in Posts)
                if (ImportantPost.Importance) ImportantPostIDs.Add(ImportantPost.ID);

            return ImportantPostIDs;
        }

        // Метод возвращающий список из ID-ишников должностей которых занимает сотрудник
        // Список имеет случайно заданое количество элементов
        // Список заполнен случайными неповторяющимися ID-ишниками должностей
        // Список может иметь максимум один ID "важной" должности
        List<int> RandomOccupiedPosts()
        {
            int OccupiedPostAmount = MyRandom.Next(1, PostAmount - ImportantPostIDs.Count + 1); // Случайно количество занимаемых должностей сотрудником
            List<int> OccupiedPosts = new List<int>(); // Список занимаемых сотрудником должностей
            bool WithImportantPost = false; // Флаг присутствия в списке "важной" должности
            int RandomPost; // Случайный номер должности в списке

            // Заполняем список заполненых сотрудником должностей
            for (int i = 0; i < OccupiedPostAmount; i++)
            {
                RandomPost = MyRandom.Next(0, PostAmount);
                if (OccupiedPosts.Contains(Posts[RandomPost].ID)) i--;
                else
                {
                    if (ImportantPostIDs.Contains(Posts[RandomPost].ID))
                    {
                        if (WithImportantPost) i--;
                        else
                        {
                            OccupiedPosts.Add(Posts[RandomPost].ID);
                            Posts[RandomPost].AddEmployee();
                            WithImportantPost = true;
                        }
                    }
                    else
                    {
                        OccupiedPosts.Add(Posts[RandomPost].ID);
                        Posts[RandomPost].AddEmployee();
                    }
                }
            }

            return OccupiedPosts;
        }

        // Метод возвращающий свободного сотрудника по заданой должности
        Employee GetFreeEmployee(int PostID)
        {
            return FreeEmployees.Find(Employee => Employee.OccupiedPosts.Contains(PostID));
        }
        
        // Метод возвращающий случайную должность
        Post GetRandomPost()
        {
            int PostNum = 0;
            
            for (int i = 0; i < 1; i++)
            {
                PostNum = MyRandom.Next(0, PostAmount);
                if (Posts[PostNum].ID == Constant.DIRECTOR_POST_ID) i--;
            }

            return Posts[PostNum];
        }

        // Метод возвращающий сумму отработанных часов, по всем должностям, которые занимает сотрудник, за неделю
        int WeekHourSum(List<int> WeekHours)
        {
            int S = 0;

            foreach (int A in WeekHours)
                S += A;

            return S;
        }

        // Метод выполняющий поставленные задачи директорами
        void DoTasks()
        {
            int OccupiedPost;
            Employee BusyEmployee;

            foreach (Task CurrentTask in Tasks)
            {
                // Если распоряжение принял сотрудник
                if (!CurrentTask.FreeLance && CurrentTask.Hours > 0)
                {
                    BusyEmployee = BusyEmployees.Find(Employee => Employee.ID == CurrentTask.EmployeeID);
                    OccupiedPost = BusyEmployee.OccupiedPosts.IndexOf(CurrentTask.PostID);

                    CurrentTask.Hours--;
                    BusyEmployee.WeekHours[OccupiedPost]++;

                    // Если распоряжение выполнено
                    if (CurrentTask.Hours == 0)
                    {
                        Logs.WriteLine("   Сотрудник " + BusyEmployee.Name + " закончил выполнять распоряжение №" + CurrentTask.ID);

                        if (WeekHourSum(BusyEmployee.WeekHours) < Constant.MAX_WEEK_HOURS)
                        {
                            BusyEmployees.RemoveAt(BusyEmployees.IndexOf(BusyEmployees.Find(x => x.ID == BusyEmployee.ID)));
                            FreeEmployees.Add(BusyEmployee);
                        }
                    }
                }
            }
        }

        // Метод составляющий детальный отчет по выплатам каждому сотруднику
        void ComposeDetailSalaryReport()
        {
            Post CurrentOccupiedPost;

            foreach (Employee CurrentEmployee in FreeEmployees)
            {
                Salary.WriteLine("Сотрудник " + CurrentEmployee.Name);
                Salary.Write(" Должности: ");

                // Записываем в файл должности занимаемые сотрудником
                for (int i = 0; i < CurrentEmployee.OccupiedPosts.Count; i++)
                {
                    CurrentOccupiedPost = Posts.Find(Post => Post.ID == CurrentEmployee.OccupiedPosts[i]);
                    Salary.Write("\"" + CurrentOccupiedPost.Title + "\"");
                    if (i != CurrentEmployee.OccupiedPosts.Count - 1)
                        Salary.Write(", ");
                    else
                        Salary.WriteLine("");
                }

                // Записываем в файл время отработаное сотрудником по каждой из занимаемой им должности
                for (int i = 0; i < CurrentEmployee.OccupiedPosts.Count; i++)
                {
                    CurrentOccupiedPost = Posts.Find(Post => Post.ID == CurrentEmployee.OccupiedPosts[i]);
                    if (!ImportantPostIDs.Contains(CurrentEmployee.OccupiedPosts[i]))
                        Salary.WriteLine(" Отработано часов по должности \"" + CurrentOccupiedPost.Title + "\": " + CurrentEmployee.WorkedHours[i]);
                }

                //  Записываем в файл зарплату начисленную сотруднику по каждой из занимаемой им должности
                for (int i = 0; i < CurrentEmployee.OccupiedPosts.Count; i++)
                {
                    CurrentOccupiedPost = Posts.Find(Post => Post.ID == CurrentEmployee.OccupiedPosts[i]);
                    if (ImportantPostIDs.Contains(CurrentEmployee.OccupiedPosts[i]))
                        Salary.WriteLine(" Зарплата по должности \"" + CurrentOccupiedPost.Title + "\": " + CurrentOccupiedPost.Salary);
                    else
                        Salary.WriteLine(" Зарплата по должности \"" + CurrentOccupiedPost.Title + "\": " + CurrentEmployee.WorkedHours[i] * CurrentOccupiedPost.EaringsPerHour);
                }

                Salary.WriteLine(" Заработано всего: " + CurrentEmployee.EarnedMoney);
            }
        }

        // Метод составляющий сумммарный отчет о выполненой работе и выплатам сотрудникам
        void ComposeRTotaleport()
        {
            int WastedTime; // Потраченное время на выполнение распоряжений по определенной должности
            int TotalWastedTime = 0; // Общее потраченное время на выполнение распоряжений по всем должностям
            int ExpensesByPost; // Выплати по определенной должности
            int TotalExpenses = 0; // Общие выплаты по всем должности
            List<int> WastedTimes = new List<int>(); // Список из потраченного времемени на выполнение распоряжений по каждой должности
            
            // Выводим в файл количество выполненных распоряжений по определенной должности
            foreach (Post CurrentPost in Posts)
            {
                if (CurrentPost.ID != Constant.DIRECTOR_POST_ID)
                    Report.WriteLine("Выполнено распоряжений по должности \"" + CurrentPost.Title + "\": " + Tasks.Count(Task => Task.PostID == CurrentPost.ID));
            }

            Report.WriteLine("Всего выполнено распоряжений: " + Tasks.Count(Task => !Task.FreeLance));
            Report.WriteLine("Всего распоряжений переданых фрилансерам: " + Tasks.Count(Task => Task.FreeLance));
            Report.WriteLine("Всего отдано распоряжений: " + Tasks.Count);

            // Выводим в файл общее количество времени потраченного на выполнение распоряженний по определенной должности
            foreach (Post CurrentPost in Posts)
            {
                WastedTime = 0;
                if (CurrentPost.ID != Constant.DIRECTOR_POST_ID)
                {
                    foreach (Employee CurrentEmployee in FreeEmployees.FindAll(Employee => Employee.OccupiedPosts.Contains(CurrentPost.ID)))
                        WastedTime += CurrentEmployee.WorkedHours[CurrentEmployee.OccupiedPosts.IndexOf(CurrentPost.ID)];

                    Report.WriteLine("Потрачено времени сотрудниками на выполнение распоряжений по должности \"" + CurrentPost.Title + "\": " + WastedTime);
                }
                TotalWastedTime += WastedTime;
                WastedTimes.Add(WastedTime);
            }

            Report.WriteLine("Всего потрачено времени на выполнение распоряжений: " + TotalWastedTime);

            // Выводим в файл суммарные выплаты сотрудникам по определенной должности
            foreach (Post CurrentPost in Posts)
            {
                if (CurrentPost.Importance)
                    ExpensesByPost = FreeEmployees.Count(Employee => Employee.OccupiedPosts.Contains(CurrentPost.ID)) * CurrentPost.Salary;
                else
                    ExpensesByPost = WastedTimes[Posts.IndexOf(CurrentPost)] * CurrentPost.EaringsPerHour;

                TotalExpenses += ExpensesByPost;
                Report.WriteLine("Выплачено денежных единиц сотрудникам по должности \"" + CurrentPost.Title + "\": " + ExpensesByPost);
            }

            Report.WriteLine("Всего выплачено денежных единиц сотрудникам: " + TotalExpenses);
        }

        // Метод моделирования работы офиса
        public void Model()
        {
            int DoneTime; // Время для завершения задания
            int TaskAmount; // Количество заданий, которое выдает директор; задается случайным образом 
            int DirectorAmount = Posts.Find(Post => Post.ID == Constant.DIRECTOR_POST_ID).EmployeeAmount; // Количиство директоров
            int TaskID; // Уникальный номер задания
            Post RandomPost; // Случайная должность
            Employee FreeEmployee; // Свободный сотрудник
            Post CurrentOccupiedPost;  // Текущая занимаемая должность
            int WeekSalary; // Общая зарплата за неделю
            int WeekSalaryByPost; // Зарплата за неделю по должности

            // Подробнее об офисе
            Console.WriteLine("Состав офиса:\n Сотрудников: " + EmployeeAmount + "\n Из них: ");
            foreach (Post CurrentPost in Posts)
                Console.WriteLine("  " + CurrentPost.Title + ": " + CurrentPost.EmployeeAmount);

            // Процесс моделирования работы офиса
            for (int Week = 1; Week <= Constant.WORK_WEEKS; Week++)
            {
                Logs.WriteLine("Неделя №" + Week);

                for (int Day = 1; Day <= Constant.WORK_DAYS; Day++)
                {
                    Logs.WriteLine(" День №" + Day);

                    for (int Hour = 1; Hour <= Constant.DAY_WORK_HOURS; Hour++)
                    {
                        Logs.WriteLine("  " + Hour + " час рабочого дня");

                        // В начале каждого часа выполняем задания
                        DoTasks();

                        for (int Director = 0; Director < DirectorAmount; Director++)
                        {
                            TaskAmount = MyRandom.Next(1, PostAmount - 1);

                            // Раздаем задания i-ым директором
                            for (int Task = 0; Task < TaskAmount; Task++)
                            {
                                RandomPost = GetRandomPost();
                                FreeEmployee = GetFreeEmployee(RandomPost.ID);
                                TaskID = (Tasks.Count != 0 ? Tasks[Tasks.Count - 1].ID  : 1)+ 1;

                                Logs.WriteLine("   Директор " + (Director + 1) + " отдал распоряжение №" + TaskID + ": \"" + RandomPost.Duty + "\"");

                                if (FreeEmployee != null) // Если есть свободный сотрудник, который может выполнить поставленное задание, то отдаем задание сотруднику
                                {
                                    DoneTime = MyRandom.Next(Constant.MIN_TASK_HOURS, Constant.MAX_TASK_HOURS + 1);
                                    if (WeekHourSum(FreeEmployee.WeekHours) + 1 == Constant.MAX_WEEK_HOURS && DoneTime == 2) DoneTime = 1;

                                    Tasks.Add(new Task(TaskID, RandomPost.ID, FreeEmployee.ID, DoneTime));
                                    FreeEmployees.RemoveAt(FreeEmployees.IndexOf(FreeEmployees.Find(x => x.ID == FreeEmployee.ID)));
                                    BusyEmployees.Add(FreeEmployee);

                                    Logs.WriteLine("   Cотрудник " + FreeEmployee.Name + " принял распоряжение №" + TaskID + ", сказав, что выполнит его за " + DoneTime + " час(а)");
                                }
                                else // Если в офисе не нашолся сотрудник, который сможет выполнить поставленное задание, то отдаем задание фрилансер
                                {
                                    Tasks.Add(new Task(TaskID));
                                    Logs.WriteLine("   На момент выдачи распоряжение №" + TaskID + ", в офисе не нашлись сотрудники, которые смогут выполнить его. Задание передано фрилансерам.");
                                }
                            } // Task

                        } // Director
                    } // Hour

                    // Время для того, чтобы зокончить все задание, которые были не закончены к концу рабочого дня
                    for (int Hour = Constant.DAY_WORK_HOURS + 1; Hour <= Constant.DAY_WORK_HOURS + Constant.MAX_TASK_HOURS; Hour++) 
                    {
                        Logs.WriteLine("  " + Hour + " час рабочого дня");
                        DoTasks();
                    } // Hour
                } // Day

                // Переносим всех сотрудников(те сотрудники, у которых сумма наработаных часов за неделю равна 40) со списка занятых
                FreeEmployees = (FreeEmployees.Concat(BusyEmployees)).ToList();
                BusyEmployees.Clear();

                // В конце каждой недели, начисляем зарплату сотрудникам, которые занимают должности на почасовой ставке
                foreach (Employee CurrentEmployee in FreeEmployees)
                {
                    WeekSalary = 0;

                    for (int i = 0; i < CurrentEmployee.OccupiedPosts.Count; i++)
                    {
                        CurrentEmployee.WorkedHours[i] += CurrentEmployee.WeekHours[i];
                        CurrentOccupiedPost = Posts.Find(Post => Post.ID == CurrentEmployee.OccupiedPosts[i]);

                        // Если должность, которую занимает сотрудник, на почасовой ставке, то начисляем зарплату по отработаным, за текущую неделю, часам
                        if (CurrentOccupiedPost.EaringsPerHour > 0)
                        {
                            WeekSalaryByPost = CurrentEmployee.WeekHours[i] * CurrentOccupiedPost.EaringsPerHour;
                            WeekSalary += WeekSalaryByPost;
                            CurrentEmployee.EarnedMoney += WeekSalaryByPost;

                            Logs.WriteLine("   Сотрудник " + CurrentEmployee.Name + " отработал " + CurrentEmployee.WeekHours[i] + " час(а/ов) по должности \"" + CurrentOccupiedPost.Title + "\" за текущую неделю, заработав " + WeekSalaryByPost + " денежн(ую/ых) единиц(у/ы)");
                        }

                        // Обнуляем часы отработанные сотрудником по каждой должности за неделю
                        CurrentEmployee.WeekHours[i] = 0;
                    }

                    if (WeekSalary > 0) 
                        Logs.WriteLine("   Сотрудник " + CurrentEmployee.Name + " получил " + WeekSalary + " денежн(ую/ых) единиц(у/ы), по всем должностям, за текущую неделю");
                }
            } // Week

            // В конце месяца, начисляем зарплату сотрудникам, которые занимают должность на фиксированой ставке  
            foreach (Employee CurrentEmployee in FreeEmployees)
            {
                for (int i = 0; i < CurrentEmployee.OccupiedPosts.Count; i++)
                {
                    CurrentOccupiedPost = Posts.Find(Post => Post.ID == CurrentEmployee.OccupiedPosts[i]);

                    // Если должность, которую занимает сотрудник, на фиксированой ставке, то начисляем зарплату по фиксированой ставке, которая указана в должности
                    if (CurrentOccupiedPost.Salary > 0)
                    {
                        CurrentEmployee.EarnedMoney += CurrentOccupiedPost.Salary;
                        Logs.WriteLine("   Сотрудник " + CurrentEmployee.Name + " отработал месяц по должности \"" + CurrentOccupiedPost.Title + "\" на фиксированой ставке, заработав " + CurrentOccupiedPost.Salary + " денежн(ую/ых) единиц(у/ы)");
                    }
                }
            }

            // Составляем детальный отчет по выплатам каждому сотруднику
            ComposeDetailSalaryReport();

            // Составляем отчет о выполненой работе и выплатам сотрудникам
            ComposeRTotaleport();

            Logs.Close();
            Salary.Close();
            Report.Close();
            Console.WriteLine("Подробные сведения о работе офиса сохранены в файл " + Constant.FILE_LOGS + "...");
            Console.WriteLine("Подробные сведения о зарплате по каждому сотруднику сохранены в файл " + Constant.FILE_SALARY + "...");
            Console.WriteLine("Отчет о выполненной работе и выданной зарплате сохранен в файл " + Constant.FILE_REPORT + "...");
        }
    }
}
