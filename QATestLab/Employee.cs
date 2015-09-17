using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QATestLab
{
    // Класс описывающий обьект "Сотрудник"
    class Employee
    {
        int ID_Pvt; // Уникальный номер (ID) сотрудника
        string Name_Pvt; // Имя сотрудника
        List<int> OccupiedPosts_Pvt; // Список из ID-ишников должностей которых занимает сотрудник
        public List<int> WorkedHours; // Список из общих отработанных часов сотрудником по каждой занятой им доджности
        public List<int> WeekHours; // Список из отработанных часов за неделю сотрудником по каждой занятой им доджности
        public int EarnedMoney; // Заработаные деньги сотрудником по всем его должностям

        public Employee(int ID, string Name, List<int> OccupiedPosts)
        {
            ID_Pvt = ID;
            Name_Pvt = Name;
            OccupiedPosts_Pvt = OccupiedPosts;
            WorkedHours = new List<int>();
            WeekHours = new List<int>();
            for (int i = 0; i < OccupiedPosts.Count; i++)
            {
                WorkedHours.Add(0);
                WeekHours.Add(0);
            }
        }

        public int ID
        {
            get
            {
                return ID_Pvt;
            }
        }

        public string Name
        {
            get
            {
                return Name_Pvt;
            }
        }

        public List<int> OccupiedPosts
        {
            get
            {
                return OccupiedPosts_Pvt;
            }
        }
    }
}
