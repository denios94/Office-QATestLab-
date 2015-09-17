using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QATestLab
{
    // Класс описывающий обьект "Должность"
    class Post
    {
        int ID_Pvt; // Уникальный номер (ID) должности
        string Title_Pvt; // Название должности
        string Duty_Pvt; // Должностная обязанность 
        int Salary_Pvt; // Зарплата (для фиксированой ставки равна определенному числу, для почасовой - равна -1)
        int EaringsPerHour_Pvt; // Заработок в час (для почасовой ставки равна определенному числу, для фиксированой - равна -1)
        int EmployeeAmount_Pvt; // Количество сотрудников занимающих эту должность
        bool Importance_Pvt; // Флаг важности должности (как минимум один сотрудник должен быть на этой должности)
        // Примичение: для директора, менеджера и прочих подобных должностей, флаг Importance_Pvt будет иметь значение "true"

        public Post(int ID, string Title, string Duty, int Salary, int EaringsPerHour, bool Importance)
        {
            ID_Pvt = ID;
            Title_Pvt = Title;
            Duty_Pvt = Duty;
            Salary_Pvt = Salary;
            EaringsPerHour_Pvt = EaringsPerHour;
            Importance_Pvt = Importance;
            EmployeeAmount_Pvt = 0;
        }

        public int ID
        {
            get
            {
                return ID_Pvt;
            }
        }

        public string Title
        {
            get
            {
                return Title_Pvt;
            }
        }

        public string Duty
        {
            get
            {
                return Duty_Pvt;
            }
        }

        public int Salary
        {
            get
            {
                return Salary_Pvt;
            }
        }

        public int EaringsPerHour
        {
            get
            {
                return EaringsPerHour_Pvt;
            }
        }

        public bool Importance
        {
            get
            {
                return Importance_Pvt;
            }
        }

        public int EmployeeAmount
        {
            get
            {
                return EmployeeAmount_Pvt;
            }
        }

        public void AddEmployee()
        {
            EmployeeAmount_Pvt++;
        }
    }
}
