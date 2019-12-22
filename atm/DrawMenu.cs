using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace atm
{
    class DrawMenu
    {
        protected static int position = 0;
        public DrawMenu() { }
        public DrawMenu(AccountHandler acc)
        {
            ach = acc;
        }
        public AccountHandler ach;
        protected string[] menuarr = new string[6] { "Проверить баланс", "Информация о пользователе", "Перевести деньги на счёт", "Снять деньги со счета", "Положить деньги на счет", "Выход" };
        public virtual void MenuDraw()
        {
            Console.Clear();
            bool loop = false;
            do
            {
                Console.CursorVisible = false;
                for (int i = 0; i < menuarr.Length; i++)
                {
                    if (i == position)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    Console.WriteLine(menuarr[i]);
                    Console.ResetColor();
                }
                var usebutton = Console.ReadKey().Key;
                switch (usebutton)
                {
                    case ConsoleKey.UpArrow:
                        {
                            position--;
                            if (position < 0)
                                position = 5;
                            Console.Clear();
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            position++;
                            if (position > 5)
                                position = 0;
                            Console.Clear();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (position == 5)
                            {
                                ach.Serializing();
                                Environment.Exit(0);
                            }
                            if (position == 0)
                                ach.ShowMoney();
                            if (position == 1)
                                ach.ShowInfo();
                            if (position == 2)
                                ach.MoneyExchange();
                            if (position == 3)
                                ach.Withdraw();
                            if (position == 4)      
                                ach.PutMoney();    
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        {
                            Console.Clear();
                            MenuDraw();
                            break;
                        }
                }
            } while (loop == false);
        }
        public void ExitButton()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Вернуться в меню");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey(); Console.Clear();
        }
        public void NoMoney()
        {
            int subpos = 0;
            string[] submenu = new string[2] { "Посмотреть остаток на счету", "Вернуться в меню" };
            bool loop = false;
            do
            {
                Console.WriteLine("На Вашем счету недостаточно средств.");
                Console.CursorVisible = false;
                for (int i = 0; i < submenu.Length; i++)
                {
                    if (i == subpos)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                    Console.Write(submenu[i]);
                    Console.Write(" ", 10);
                    Console.ResetColor();
                }
                var usebutton = Console.ReadKey().Key;
                switch (usebutton)
                {
                    case ConsoleKey.LeftArrow:
                        subpos--;
                        if (subpos < 0)
                            subpos = 1;
                        Console.Clear();
                        break;
                    case ConsoleKey.RightArrow:
                        subpos++;
                        if (subpos > 1)
                            subpos = 0;
                        Console.Clear();
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        Console.Clear();
                        NoMoney();
                        break;
                    case ConsoleKey.Enter:
                        if (subpos == 0)
                            ach.ShowMoney();
                        if (subpos == 1)
                        {
                            Console.Clear();
                            return;
                        }
                        break;
                }
            }
            while (loop == false);
        }
    }
}
