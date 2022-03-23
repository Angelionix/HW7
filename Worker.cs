using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    public struct Worker
    {
        private int id;
        private int age;
        private int height;
        private DateTime timeOfCreation;
        private string fio;
        private DateTime birthdayDate;
        private string birthdayAddress;


        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public DateTime TimeOfCreation
        {
            get
            {
                return timeOfCreation;
            }
            set
            {
                timeOfCreation = value;
            }
        }
        public string FIO
        {
            get
            {
                return fio;
            }
            set
            {
                fio = value;
            }
        }
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public DateTime BirthdayDate
        {
            get
            {
                return birthdayDate;
            }
            set
            {
                birthdayDate = value;
            }
        }

        public string BirthdayAddres
        {
            get
            {
                return birthdayAddress;
            }
            set
            {
                birthdayAddress = value;
            }
        }

        public int Height 
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        // Сделать подсчет возраста автоматический
        public Worker(int id, string fio, int age, int height, DateTime birthdayDate, string birthdayAddres, DateTime dataOfCreation)
        {
            this.id = id;
            this.timeOfCreation = dataOfCreation;
            this.fio = fio;
            this.birthdayAddress = birthdayAddres;
            this.birthdayDate = birthdayDate;
            this.age = age;
            this.height = height;
        }

    }
}
