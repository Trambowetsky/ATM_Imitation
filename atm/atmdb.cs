using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace atm
{
    [Serializable]
    class atmdb
    {
        public List<Account> accounts = new List<Account> { new Account() { ClientName = "Трамбовецький Олександр", CardNumber = 5168755661910063, MoneyAmount = 200, MoneyValue = "UAH", PinCode = 6848, ExpDate = new DateTime(2020, 5, 5), IsAdmin = true },
                                                            new Account() { ClientName = "Макарцев Максим", CardNumber = 5168716114880063, MoneyValue = "UAH", MoneyAmount = 250, PinCode = 1488, ExpDate = new DateTime(2021, 7, 21)} };

    }

}
