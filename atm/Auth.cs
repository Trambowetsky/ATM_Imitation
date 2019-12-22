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
    public class Auth
    {
        Account account = new Account();
        atmdb GetAtmdb = new atmdb();
        public long usercardnumberentry;
        public static int counter = -1;
        public int userpinentry;
        private byte accestries = 0;
        FileStream fs;
        BinaryFormatter bf;
        public Auth()
        {

        }
        public Auth(FileStream fileStream, BinaryFormatter form)
        {
            fs = fileStream;
            bf = form;
        }
        public void CardNumberAccess()
        {
            fs = File.OpenRead("AtmDataBase.dat");
            if (fs.Length != 0)
            {
                GetAtmdb.accounts = bf.Deserialize(fs) as List<Account>;
                fs.Close();
            }
            Console.Write("Введите номер карты: ");
            try { usercardnumberentry = long.Parse(Console.ReadLine()); }
            catch (FormatException)
            { Console.Clear(); Console.WriteLine("Вы ввели неверный номер карты. Попробуйте еще раз."); CardNumberAccess(); };
            foreach (var x in GetAtmdb.accounts)
            {
                if (x.CardNumber == usercardnumberentry)
                {
                    PinAccess();
                }
            }
            Console.WriteLine("Вы ввели неверный номер карты. Попробуйте еще раз!");
            Console.ReadKey(); Console.Clear(); CardNumberAccess();
        }
        public void PinAccess()
        {
            Console.Write("Введите пин-код: ");
            try { userpinentry = int.Parse(Console.ReadLine()); }
            catch (FormatException) { Console.Clear(); Console.WriteLine("Вы ввели неверный пин код. Попробуйте еще раз."); PinAccess(); }
           // if (userpinentry.ToString().ToCharArray().Count() != 4)
            //{ Console.Clear(); Console.WriteLine("Неправильный пин код. Введите еще раз!"); PinAccess(); }

            foreach (var x in GetAtmdb.accounts)
            {
                counter++;
                if (x.PinCode == userpinentry && x.CardNumber == usercardnumberentry)
                {
                    Console.CursorVisible = false;
                    AccountHandler accountHandler = new AccountHandler(x, GetAtmdb, fs, bf);
                    DrawMenu draw = new DrawMenu(accountHandler);
                    accountHandler.SecondInit(draw);
                    ExtendedDrawMenu extendedDraw = new ExtendedDrawMenu(accountHandler);
                    Console.Clear(); Console.Write("Вы успешно вошли в систему! Ожидайте");
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write(".");
                        Thread.Sleep(500);
                    }
                    if (x.IsAdmin == true)
                        extendedDraw.MenuDraw();
                    else
                        draw.MenuDraw();
                }
            }
            Console.WriteLine("Вы ввели неверный пин код. Попробуйте еще раз!");
            Console.ReadKey(); accestries++; Console.Clear();
            if (accestries >= 3)
                ErrorMethod();
            else
            {
                counter = -1;
                PinAccess();
            }
        }
        public void ErrorMethod()
        {
            Console.Clear();  Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Вы ввели неверные данные более трёх раз! \nНажмите любую клавишу для выхода.");
            Console.ReadKey(); Environment.Exit(0);
        }
    }

}
