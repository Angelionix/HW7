using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    public class NoteBook
    {
        private Worker[] workers;                                                            // массив наших записей
        private string[] titles = new string[7] { "ID", "Дата создания",                     // Массив с заголовками полей
                                                "Фамилия Имя Отчество", "Возраст", "Рост",   // записей
                                                    "Дата рождения", "Место рождения" };
        private DateTime creationDate;                                                       // время создания базы
        private int index = 0;                                                               // индекс последней не пустой записи

        //свойство для доступка к заголовкам
        public string[] Title
        {
            get 
            {
                return titles;
            }
        }                                               
        //свойство для доступа к индексу
        public int Index
        {
            get 
            {
                return index;
            }
        }
        //сойства для доступа к параметру длинны массива записей
        public int WorkerCounts
        {
            get
            {
                return workers.Length;
            }
        }
        //Конструктор ежеднеевника
        public NoteBook()
        {
            creationDate = DateTime.Now;
            workers = new Worker[1];
            workers[0] = new Worker(0, "Фамилия/Имя/Отчество", 0, 250, DateTime.Now, "Prostokvashio", creationDate);
        }

        /// <summary>
        /// Метод Resize удваивает емость массива в случае если индекс добавляемого элемента больше длины текущего массива
        /// </summary>
        /// <param name="flag"> Параметр который показывает нужно ли увеличить обьем массива </param>
        private void Resize(bool flag, float i)
        {
            if (flag)
            {
                Array.Resize(ref this.workers, (int)(this.workers.Length * i));
            }
        }

        /// <summary>
        /// Метод для добалвения записи в ноутбук
        /// </summary>
        /// <param name="newWorker"> Обьект типа воркер который мы добавляем в базу </param>
        /// <param name="index"> Индекс куда добавлять воркера</param>
        public void AddWorker(Worker newWorker, int index)
        {
            Resize(index >= workers.Length, 2);
            workers[index] = newWorker;
            this.index++;
        }
        /// <summary>
        /// метод для удалениязаписи из базы по ИД
        /// </summary>
        /// <param name="index"> ИД работника</param>
        public void DeleteWorker(int index)
        {
            bool canResize = true;
            int i = index - 1;
            Array.Clear(workers, i, 1);
            for ( i = index - 1; i < workers.Length - 1; index++)
            {
                workers[i] = workers[i + 1];
                workers[i].ID = i + 1;
                if (workers[i].ID == 0 && canResize)
                {
                    Resize(((float)workers.Length / i > 2f), 0.5f);
                    canResize = false;
                }
                i++;
            }
        }

        /// <summary>
        /// Метод редактировния инфы
        /// </summary>
        /// <param name="id"> ИД записи</param>
        /// <param name="choise">Параметр отвечающий за то что имеено будем редактировать</param>
        public void EditWorker(int id, string fio, int height, DateTime birthDate, string birthAddres)
        {
            workers[id - 1].FIO = fio;
            workers[id - 1].Height = height;
            workers[id - 1].BirthdayDate = birthDate;
            workers[id - 1].BirthdayAddres = birthAddres;
        }

        /// <summary>
        /// метод для доступа к записи по ее ИД
        /// </summary>
        /// <param name="id">Ид записи</param>
        /// <returns>возвращает экземпляр класса Worker</returns>
        public Worker SeeByID(int id)
        {
            return workers[id - 1];
        }
        
        /// <summary>
        /// Метод для сортировки спискарабочих по дате создания пузырьковым методом
        /// </summary>
        /// <param name="choise"> параметр отвечает за то прямая сортировка или боратная</param>
        public void WorkerSortingByDate(int choise)
        {
            Worker temp;
            if (choise == 1)
            {
                for (int i = 0; i < workers.Length; i++)
                {
                    for (int j = i + 1; j < workers.Length; j++)
                    {
                        if (workers[i].TimeOfCreation > workers[j].TimeOfCreation)
                        {
                            temp = workers[i];
                            workers[i] = workers[j];
                            workers[j] = temp;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < workers.Length; i++)
                {
                    for (int j = i + 1; j < workers.Length; j++)
                    {
                        if (workers[i].TimeOfCreation < workers[j].TimeOfCreation)
                        {
                            temp = workers[i];
                            workers[i] = workers[j];
                            workers[j] = temp;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод выводит в консоль данные опеределнного работника
        /// </summary>
        /// <param name="worker">Мы передаем в метод экземпляр рабочего</param>
        public void NotePrintingToConsole(Worker worker)
        {
            Console.WriteLine($"{worker.ID,5}| {worker.TimeOfCreation,19}| {worker.FIO,30}| {worker.Age,8}| {worker.Height,5}| {worker.BirthdayDate,19}| {worker.BirthdayAddres,20}");
        }

        /// <summary>
        /// Метод выводит в консоль первой строкой заголовки полей
        /// и затем все записи, исключая пустые, которые имееют ИД=0
        /// </summary>
        public void PrintingAllNotes()
        {
            Console.WriteLine($"{titles[0],5}| {titles[1],19}| {titles[2],30}| {titles[3],8}| {titles[4],5}| {titles[5],19}| {titles[6],20}");
            foreach (Worker worker in workers)
            {
                if(worker.FIO !=null)
                {
                    NotePrintingToConsole(worker);
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Метод осуществляет подсчет возраста работника. Так как базу будут открвать возхможно намного позже ее создания, то и позраст
        /// если его вводить только пи создании будет не актуальным, этот мтеод юбуджет вызываться при загрузке базы в программу и актуализировать
        /// возраст
        /// </summary>
        /// <param name="birthDate"> Передаем дату рождения</param>
        /// <returns> Возвращает возраст в годах </returns>
        public int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int age = today.Year - birthDate.Year;
            if (birthDate.AddYears(age) > today)
            {
                age--;
            }
            return age;
        }
    }
}
