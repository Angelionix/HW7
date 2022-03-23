using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    class Program
    {

        static void Main(string[] args)
        {
            bool isCreatigFile = true;
            bool isFileShoising = true;
            bool isWorkWithNote = true;
            bool isEditingNote = true;
            int choise = 0;
            int id = 0;
            string defaultPath = Directory.GetCurrentDirectory() + "\\";    // Дефолтный путь к рабочей папке
            string notebookName = string.Empty;                             // Имя Ноутбука с которым будем работать
            string pathForNoteBook = defaultPath;                           // Путь к файлу ноутбука на диске
            string temp = string.Empty;                                     // Временная текстовая переменная
            NoteBook noteBook = new NoteBook();                             // Создаем контейнер для нашего ноутбука с которым будем работать
            Worker currWorker = new Worker();                               // Создаем контейнер для работы с записью
            FileOperations fileIO = new FileOperations();
            string fIO;
            string birthAddress;
            DateTime birthDate;
            int height;

            Console.WriteLine("Добрый день!\nВас приветствует Ежедневник v.0002");
            Console.WriteLine($"Выберите пожалуйста действие \n 1.Указать рабочий каталог \n 2.Катологе по умолчанию - {defaultPath}");
            choise = SafeInputing(1, 2);
            // Если юзер выбер 1 вариант, то просим указать путь к каталогу и если он не существует создаем его
            // Если пользователь выберет вариант 2, то проверяем есть ли каталог по умолчанию. Если нету создаем его
            #region DirectorySetting
            switch (choise)
            {
                case 1:
                    Console.WriteLine("Пожалуйста введите путь до каталога с Записными книжками");
                    pathForNoteBook = Console.ReadLine();
                    if (!pathForNoteBook[pathForNoteBook.Length - 1].Equals('\\'))      // проверяем что путь указан с символом - \
                    {                                                                   // на конце, если его нет добавляем
                        pathForNoteBook += '\\';
                    }
                    // Если по введеноому пути есть директория, то мы выводим в косоль список файлов в ней
                    if (Directory.Exists(pathForNoteBook))
                    {
                        string[] noteBooksIndir = Directory.GetFiles(pathForNoteBook);
                        Console.WriteLine($"В каталоге {pathForNoteBook} содежится {noteBooksIndir.Length} файлов:");
                        foreach (string file in noteBooksIndir)
                        {
                            Console.WriteLine(file);
                        }
                        temp = pathForNoteBook;
                    }
                    // если директории нету, то создаем директорию
                    else
                    {
                        Console.WriteLine("По указанному пути директория не существует, поэтому мы ее создадим");
                        Directory.CreateDirectory(pathForNoteBook);
                        Console.WriteLine(defaultPath);
                    }
                    break;
                case 2:
                    // Проверяем существует ли дефолтная директория, если нет то создаем е
                    // если существуем выводим в консоль список файлов в ней
                    pathForNoteBook = defaultPath;
                    if (Directory.Exists(pathForNoteBook))
                    {
                        Console.WriteLine("Дефолтный каталог уже существует");
                        if (Directory.GetFiles(pathForNoteBook).Length > 0)
                        {
                            string[] files = Directory.GetFiles(pathForNoteBook);
                            Console.WriteLine($"В каталоге {pathForNoteBook} содежится {files.Length} файлов:");
                            foreach (string file in files)
                            {
                                Console.WriteLine(file);
                            }
                            break;
                        }
                        Directory.CreateDirectory(pathForNoteBook);
                    }
                    break;
            }
            #endregion DirectorySetting

            #region FileChoising
            Console.WriteLine("1.Загрузить данные из дневника");
            Console.WriteLine("2.Создать новый дневник");

            choise = SafeInputing(1, 2);

            // Если пользователь выберет 1 то мы загружаем выбранный дневник
            // Если же выбере вариант 2 то мы создаем новый двневник
            switch (choise)
            {
                case 1:
                    // Просим указать имя файла, если он существует то мы загружаем все заметки из него
                    // если такого файла нет, то предалгаем или создать новый файл с таким именем или попробовать ввести другое имя
                    do
                    {
                        temp = pathForNoteBook;
                        Console.WriteLine("Введите название блокнота включая раcширение .txt");
                        notebookName = Console.ReadLine();
                        temp = temp + notebookName;
                        if (!File.Exists(temp))
                        {
                            Console.WriteLine("Указанный вами файл не существует\n 1. Попробуете другое имя \n 2. Создаем новый файл");
                            choise = SafeInputing(1, 2);
                            switch (choise)
                            {
                                case 1:
                                    break;
                                case 2:
                                    isFileShoising = false;
                                    fileIO.CreatingNewFile(temp);
                                    break;
                            }
                        }
                        else
                        {
                            isFileShoising = false;

                            Console.WriteLine("1.Загрузить всё");
                            Console.WriteLine("2.Загрузить временной диапазон");
                            choise = SafeInputing(1, 2);

                            if (choise == 1)
                            {
                                // Мы тут загружаем все заметки, даты нам впринципе не нужны, но для того чтобы в одном методе совместить
                                // и загурку всего и загрузку из диапазона, добавляем их в параметры
                                fileIO.ImportNotes(noteBook, temp, 1, DateTime.Now, DateTime.Now);
                            }
                            else
                            {

                                DateTime fromDate = GetCorrectDate(new DateTime(1950, 01, 01), DateTime.Now, "Укажите с какой даты необходимо загрузить записи");
                                DateTime toDate = GetCorrectDate(fromDate, DateTime.Now, "Укажите до какой даты необходимо загрузить записи");
                                fileIO.ImportNotes(noteBook, temp, 2, fromDate, toDate);
                            }
                        }
                    } while (isFileShoising);
                    break;
                case 2:
                    do
                    {
                        temp = pathForNoteBook;
                        Console.WriteLine("Пожалуйста введите название дневника включая раcширение .txt");
                        notebookName = Console.ReadLine();
                        temp = pathForNoteBook + notebookName;
                        if (File.Exists(temp))
                        {
                            Console.WriteLine("Файл с указаным именем уже существует\n1. Перезаписать его? \n 2. Ввести другое имя");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                isCreatigFile = false;
                                fileIO.CreatingNewFile(temp);
                            }
                        }
                        fileIO.CreatingNewFile(temp);
                        isCreatigFile = false;
                    } while (isCreatigFile);
                    break;
            }
        #endregion fileChoising

            Console.WriteLine($"Дневник - {notebookName} , содержит {noteBook.Index} записей");
            isWorkWithNote = true;
            do
            {
                noteBook.PrintingAllNotes();
                Console.WriteLine("1. Добавить запись");
                Console.WriteLine("2. Удалить запись");
                Console.WriteLine("3. Редактировать запись");
                Console.WriteLine("4. Сохранить в файл");
                Console.WriteLine("5. Сортировка записей");
                Console.WriteLine("6. Выход");

                choise = SafeInputing(1, 6);
                #region WorkingWithNotes

                switch (choise)
                {
                    case 1:
                        #region AddingWorker
                        Console.WriteLine("Пожалуйста укажите Фамилию Имя Отчество сотрудника");
                        fIO = Console.ReadLine();

                        birthDate = GetCorrectDate(new DateTime(1920, 01, 01), new DateTime(DateTime.Now.Year - 18, 01, 01), "Введите дату рождения в формате дд.ММ.гггг (день.месяц.год):\n" +
                        " дата рождения не может быть ранее 1920 года и не позднее чем 18 лет от текущего года");
                        int age = noteBook.CalculateAge(birthDate);

                        Console.WriteLine("Пожалуйста укажите ваш рост");
                        height = SafeInputing(50, 300);

                        Console.WriteLine("Пожалуйста укажите место рождения");
                        birthAddress = Console.ReadLine();

                        currWorker = new Worker(noteBook.Index + 1, fIO, age, height, birthDate, birthAddress, DateTime.Now);
                        noteBook.AddWorker(currWorker, noteBook.Index);

                        Console.WriteLine(noteBook.Index);
                        break;
                    #endregion AddingWorker
                    case 2:
                        #region DeletingWorker
                        Console.WriteLine("Введите поэалуйста ИД записи для удаленния");
                        id = SafeInputing(1, noteBook.Index);

                        Console.WriteLine("Вы уверенны в удалении записи \n 1.Да \n 2.Нет");
                        choise = SafeInputing(1, 2);

                        switch (choise)
                        {
                            case 1:
                                noteBook.DeleteWorker(id);
                                noteBook.PrintingAllNotes();
                                break;
                            case 2:
                                noteBook.PrintingAllNotes();
                                break;
                        }
                        break;
                    #endregion DeletingWorker
                    case 3:
                        #region EditingNote
                        do
                        {
                            Console.WriteLine("Введите пожалуйста ИД работника для редактирования его данных");

                            id = SafeInputing(1, noteBook.Index);
                            currWorker = noteBook.SeeByID(id);
                            Console.WriteLine($"{noteBook.Title[0],5}| {noteBook.Title[1],19}| {noteBook.Title[2],30}| {noteBook.Title[3],8}|" +
                                              $" {noteBook.Title[4],5}| {noteBook.Title[5],19}| {noteBook.Title[6],20}");
                            noteBook.NotePrintingToConsole(currWorker);

                            Console.WriteLine("Изменить Фамилия Имя Отчество?\n 1.Да/2.Нет");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                Console.WriteLine("Введите пожалуйста Новые Имя Фамилия Отчество");
                                fIO = Console.ReadLine();
                            }
                            else
                            {
                                fIO = currWorker.FIO;
                            }

                            Console.WriteLine("Изменить Год рождения?\n 1.Да/2.Нет");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                birthDate = Program.GetCorrectDate(new DateTime(1920, 01, 01), new DateTime(DateTime.Now.Year - 18, 01, 01), "Введите дату рождения в формате дд.ММ.гггг (день.месяц.год):\n" +
                                " дата рождения не может быть ранее 1920 года и не позднее чем 18 лет от текущего года");
                            }
                            else
                            {
                                birthDate = currWorker.BirthdayDate;
                            }

                            Console.WriteLine("Изменить Место рождения?\n 1.Да/2.Нет");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                Console.WriteLine("Введите пожалуйста Место рождения");
                                birthAddress = Console.ReadLine();
                            }
                            else
                            {
                                birthAddress = currWorker.BirthdayAddres;
                            }

                            Console.WriteLine("Изменить Рост?\n 1.Да/2.Нет");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                Console.WriteLine("Введите пожалуйста Ваш Рост");
                                height = Program.SafeInputing(50, 300);
                            }
                            else
                            {
                                height = currWorker.Height;
                            }

                            noteBook.EditWorker(id, fIO, height, birthDate, birthAddress);

                            Console.WriteLine("Закончить редактирование?\n 1.Да/2.Нет");
                            choise = SafeInputing(1, 2);
                            if (choise == 1)
                            {
                                isEditingNote = false;
                                break;
                            }
                        } while (isEditingNote);
                        break;
                    #endregion EditingWNote
                    case 4:
                    #region SavingToFile
                        fileIO.SaveToDiskAllNotes(temp, noteBook);
                        break;
                    #endregion  SavingToFile
                    case 5:
                    #region SortNotes
                        Console.WriteLine("1.По убыванию \n 2.По возрастанию");
                        choise = SafeInputing(1, 2);
                        noteBook.WorkerSortingByDate(choise);
                        break;
                    #endregion SortNotes
                    case 6:
                        isWorkWithNote = false;
                        break;
                }
            }
            while (isWorkWithNote);
            #endregion WorkingWithNotes

        }

        /// <summary>
        /// Метод для получения числа в заданном диапазоне
        /// </summary>
        /// <param name="min">мин диапазона</param>
        /// <param name="max"> максимум диапазона</param>
        /// <returns>Возвращает целое число</returns>
        public static int SafeInputing( int min, int max)
        {
            bool correctAnswer = true;
            int choise =0;
            do                                                                                                      // защищенны ввод того как будем вводить данные для рассчетов
            {                                                                                                       //
                if (int.TryParse(Console.ReadLine(), out choise))                                                   // Чекаем что ввели число, а не другой символ
                {                                                                                                   //
                    if (choise < min || choise > max)                                                                   // Чекаем что чило в корректном диапазоне
                    {                                                                                               //
                        Console.WriteLine($"Пожалуйста введите число от {min} до {max}");                                   // Если число не корректное просим ввести заново
                        correctAnswer = false;                                                                        // переменная райтасер делаем false чтобы заново запустить цикл ввода
                    }                                                                                               //
                    else                                                                                            //
                    {                                                                                               // Если число коректное, то переменной rightAnswer присваиваем true
                        correctAnswer = true;                                                                         // и идем дальше
                    }                                                                                               //
                }                                                                                                   //
                else                                                                                                //
                {                                                                                                   //
                    Console.WriteLine("Пожалуйста введите число, а не какой либо иной символ");                    // Если ввели не число просим ввести корректное чилсо 
                    correctAnswer = false;                                                                            // и запускаем цикл ввода заново
                }                                                                                                   //
            }                                                                                                       //
            while (!correctAnswer);
            return choise;
        }

        /// <summary>
        /// Метод для получения корректной даты, с заданными ограничениями
        /// </summary>
        /// <param name="minDate">нижний порог для даты</param>
        /// <param name="maxDate">верхний порог для даты</param>
        /// <param name="printToConsole">То что следует выводить в консоль для пользователя</param>
        /// <returns></returns>
        public static DateTime GetCorrectDate(DateTime minDate, DateTime maxDate, string printToConsole)
        {
            DateTime dateForReturn;
            bool correct;
            do
            {
                Console.WriteLine(printToConsole);
                correct = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None,out dateForReturn);
            }
            while (!(correct && dateForReturn <= DateTime.Now && (maxDate >= dateForReturn) && (dateForReturn >= minDate)));
            return dateForReturn;
        }
    }
}
