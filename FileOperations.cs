using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    public class FileOperations
    {
        string[] fields = new string[0];

        /// <summary>
        /// Метод импорта записй из файла
        /// </summary>
        /// <param name="notebook">Наш рабочий ноутбук с которым мы будем работать</param>
        /// <param name="path">Путь к файлу на диске</param>
        /// <param name="choise">Выбираем - 1. Загружать все записи; 2. Загружать в диапазоне дат</param>
        /// <param name="from">Дата с которой  мы будем загружать записи</param>
        /// <param name="to">Дата до которой мы будем загружать записи</param>
        public void ImportNotes(NoteBook notebook, string path, int choise, DateTime from, DateTime to)
        {
            int i = notebook.Index;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                Worker worker = new Worker();
                while ((line = sr.ReadLine()) != null)
                {
                    fields = line.Split('#');
                    worker.ID = i + 1;
                    worker.FIO = fields[2];
                    worker.TimeOfCreation = DateTime.Parse(fields[1]);
                    worker.Age = notebook.CalculateAge(DateTime.Parse(fields[5]));
                    worker.Height = int.Parse(fields[4]);
                    worker.BirthdayDate = DateTime.Parse(fields[5]);
                    worker.BirthdayAddres = fields[6];
                    if (choise == 1)
                    {
                        notebook.AddWorker(worker, i);
                        i++;
                    }
                    else
                    {
                        Console.WriteLine(from);
                        Console.WriteLine(worker.TimeOfCreation);

                        if (from <= worker.TimeOfCreation && worker.TimeOfCreation <= to)
                        {
                            notebook.AddWorker(worker, i);
                            i++;
                        }
                    
                    }

                }
            }
        }

        /// <summary>
        /// Метод для записи всех заметок в файл
        /// </summary>
        /// <param name="path"> путь к файлу на диске</param>
        /// <param name="noteBook"> наш рабочий ноутбук</param>
        public void SaveToDiskAllNotes(string path, NoteBook noteBook)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                Worker worker;
                string temp = string.Empty;
                for (int i = 1; i <= noteBook.Index; i++)
                {
                    worker = noteBook.SeeByID(i);
                    temp = $"{worker.ID}#{worker.TimeOfCreation}#{worker.FIO}#{worker.Age}#{worker.Height}#{worker.BirthdayDate}#{worker.BirthdayAddres}";
                    sw.WriteLine(temp);
                }
            }
        }

        /// <summary>
        /// Метод для создания файла нового
        /// </summary>
        /// <param name="path">путь дял создания</param>
        public void CreatingNewFile(string path)
        {
            File.Create(path).Close();
        } 
    }
}
