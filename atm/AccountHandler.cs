using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace atm
{
    class AccountHandler
    {
        private double useramount = 0;
        Account acc;
        DrawMenu draw;
        atmdb GetAtmdb;
        FileStream fs;
        BinaryFormatter bf;
        public AccountHandler(Account logged, atmdb atmdb, FileStream fileStream, BinaryFormatter formatter)
        {
            acc = logged;
            GetAtmdb = atmdb;
            fs = fileStream;
            bf = formatter;
        }
        public void SecondInit(DrawMenu draw)
        {
            this.draw = draw;
        }
        public void ShowInfo()
        {
            string formattedCardNumber = String.Format("{0:####-####-####-####}", acc.CardNumber);
            Console.Clear();
            Console.WriteLine($"Пользователь: {acc.ClientName}\nНомер лицевого счёта: {formattedCardNumber}\nДата окончания действия карточки: {acc.ExpDate.Month}/{acc.ExpDate.Year}");
            draw.ExitButton();
        }
        public void ShowMoney()
        {
            Console.Clear();
            Console.WriteLine($"Ваш баланс на счету - {acc.MoneyAmount} {acc.MoneyValue}");
            draw.ExitButton();
        }
        public void Withdraw()
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.Write("Укажите сумму для снятия наличных средств: ");
            try { useramount = double.Parse(Console.ReadLine()); }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Вы ввели неверные данные. Попробуйте еще раз!");
                Console.ReadKey(); Console.Clear();
                Withdraw();
            }
            if ((acc.MoneyAmount - useramount) < 0)
            {
                Console.Clear();
                draw.NoMoney();
            }
            else
            {
                acc.MoneyAmount -= useramount;
                Console.Clear();
                Console.WriteLine($"Автомат выдаёт наличные. Ваш остаток на счету: {acc.MoneyAmount} {acc.MoneyValue}");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public void PutMoney()
        {
            Console.Clear();
            useramount = 0;
            Console.Write("Укажите сумму для взноса: ");
            try { useramount = double.Parse(Console.ReadLine()); }
            catch (FormatException)
            {
                Console.WriteLine("Вы ввели неверные данные! Попробуйте еще раз.");
                PutMoney();
            }
            acc.MoneyAmount += useramount;
            Console.Clear();
            Console.WriteLine($"Вы успешно внесли {useramount} {acc.MoneyValue} на свой счёт! Ваш баланс: {acc.MoneyAmount} {acc.MoneyValue}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Готово"); Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
        }
        public void Serializing()
        {
            GetAtmdb.accounts.Insert(Auth.counter, acc);
            fs.Close();
            fs = File.OpenWrite("AtmDataBase.dat");
            bf.Serialize(fs, GetAtmdb.accounts);
            fs.Close();
        }
        public void AccountCreating()
        {
            Console.Clear(); Console.CursorVisible = true;
            Account acc = new Account();
            Console.Write("Введите ФИО: ");
            acc.ClientName = Console.ReadLine();
            Console.Write("Введите номер карты: ");
            acc.CardNumber = long.Parse(Console.ReadLine());
            Console.Write("Введите пин-код: ");
            acc.PinCode = int.Parse(Console.ReadLine());
            Console.Write("Введите валюту для счёта: ");
            acc.MoneyValue = Console.ReadLine();
            acc.ExpDate = DateTime.Now;
            acc.ExpDate = acc.ExpDate.AddYears(5);
            acc.MoneyAmount = 0;
            acc.IsAdmin = false;
            GetAtmdb.accounts.Add(acc);
            fs.Close();
            fs = File.OpenWrite("AtmDataBase.dat");
            bf.Serialize(fs, GetAtmdb.accounts);
            fs.Close();
            Console.Clear();
            string formattedCardNumber = String.Format("{0:####-####-####-####}", acc.CardNumber);
            Console.WriteLine($"CardNumber - {formattedCardNumber}\nPin-Code - {acc.PinCode}\nСохраните");
            Console.ReadKey();
            Console.Clear();
        }
        public void MoneyExchange()
        {
            long receivercardnubmer = 0;
            double usermonamount = 0;
            Console.Clear();
            Console.WriteLine("Введите номер карты получателя:");
            try
            {
                receivercardnubmer = long.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Вы ввели неверное значение, попробуйте еще раз.");
                Console.ReadKey(); Console.Clear();
                MoneyExchange();
            }
            Console.Write("Введите сумму для отправки получателю: ");
            try
            {
                usermonamount = double.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Вы ввели неверное значение, попробуйте еще раз.");
                Console.ReadKey(); Console.Clear();
                MoneyExchange();
            }
            Loading();
            FindingReceiver(receivercardnubmer, usermonamount);
        }
        public static void Loading()
        {
            Console.Write("Идёт обработка запроса. Ожидайте");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(900);
            }
            Console.WriteLine("");
        }
        public void FindingReceiver(long cardnumber, double money)
        {
            var receiver = GetAtmdb.accounts.Find(x => x.CardNumber == cardnumber);

            if (receiver != null)
            {
                if (receiver.MoneyValue == acc.MoneyValue)
                {
                    if (acc.MoneyAmount > money)
                    {
                        receiver.MoneyAmount += money;
                        acc.MoneyAmount -= money;

                        GetAtmdb.accounts.Insert(GetAtmdb.accounts.IndexOf(GetAtmdb.accounts.Find(x => x.CardNumber == cardnumber)), receiver);

                        fs.Close();
                        fs = File.OpenWrite("AtmDataBase.dat");
                        bf.Serialize(fs, GetAtmdb.accounts);
                        fs.Close();

                        Console.WriteLine("Вы успешно отправили перевод!");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Далее");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Не удалось выполнить перевод средств. Указанная Вами сумма превышает сумму на Вашем счету.");
                        Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("Хорошо"); Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ReadKey(); Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Нельзя сделать перевод на указанный Вами счёт. Валюта не совпадает.");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;  Console.WriteLine("Хорошо"); Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey(); Console.Clear();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Клиента с таким номером лицевого счёта не найдено. Попробуйте еще раз.");
                Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("Хорошо"); Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();
                MoneyExchange();
            }
        }
    }
}
