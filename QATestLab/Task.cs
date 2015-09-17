using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QATestLab
{
    // Класс описывающий обьект "Задание(Распоряжение)"
    class Task
    {
        int ID_Pvt; // Уникальный номер (ID) задания
        int PostID_Pvt; // Уникальный номер (ID) должности, которая нужна для выполнения задания
        int EmployeeID_Pvt; // Уникальный номер (ID) сотрудника, который взялся за выполнение задания
        int Hours_Pvt; // Часы (время), которые потребуются для выполнения задания
        bool FreeLance_Pvt; // Флаг фрилансера, то есть, если задания взяли фрилансер - он будет имет значение "true"

        // Конструктор для задания, которое отдают сотруднику
        public Task(int ID, int PostID, int EmployeeID, int Hours)
        {
            ID_Pvt = ID;
            PostID_Pvt = PostID;
            EmployeeID_Pvt = EmployeeID;
            Hours_Pvt = Hours;
            FreeLance_Pvt = false;
        }

        // Конструктор для задание, которое отдают флирансеру
        public Task(int ID)
        {
            ID_Pvt = ID;
            PostID_Pvt = -1;
            EmployeeID_Pvt = -1;
            Hours_Pvt = -1;
            FreeLance_Pvt = true;
        }

        public int ID
        {
            get
            {
                return ID_Pvt;
            }
        }

        public int PostID
        {
            get
            {
                return PostID_Pvt;
            }
        }

        public int EmployeeID
        {
            get
            {
                return EmployeeID_Pvt;
            }
        }

        public int Hours
        {
            get
            {
                return Hours_Pvt;
            }
            set
            {
                if (value < 0) Hours_Pvt = 0;
                else Hours_Pvt = value;
            }
        }

        public bool FreeLance
        {
            get
            {
                return FreeLance_Pvt;
            }
        }
    }
}
