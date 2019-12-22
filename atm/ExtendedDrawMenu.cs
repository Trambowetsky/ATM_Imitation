using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace atm
{
    class ExtendedDrawMenu : DrawMenu
    {
        new AccountHandler ach;
        public ExtendedDrawMenu() : base()
        { }
        public ExtendedDrawMenu(AccountHandler handler) : base()
        { ach = handler; }
        new string[] menuarr = new string[7] {"Проверить баланс", "Информация о пользователе", "Перевести деньги на счёт", "Снять деньги со счёта", "Положить деньги на счёт", "Создать нового пользователя", "Выход"};
        public override void MenuDraw()
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
                                position = 6;
                            Console.Clear();
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            position++;
                            if (position > 6)
                                position = 0;
                            Console.Clear();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (position == 5)
                                ach.AccountCreating();
                            if (position == 6)
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
                            MenuDraw();
                            break;
                        }
                }
            } while (loop == false);
        }
    }
}

