using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm
{
    [Serializable]
    public class Account
    {
        private bool isAdmin = false;
        private string name;
        private long cardnumber;
        private int pin;
        private double money;
        private string mvalue;
        private DateTime expdate;
        public long CardNumber
        {
            get { return cardnumber; }
            set { cardnumber = value; }
        }
        public int PinCode
        {
            get { return pin; }
            set { pin = value; }
        }
        public string ClientName
        {
            get { return name; }
            set { name = value; }
        }
        public double MoneyAmount
        {
            get { return money; }
            set { money = value; }
        }
        public string MoneyValue
        {
            get { return mvalue; }
            set { mvalue = value; }
        }
        public DateTime ExpDate
        {
            get { return expdate; }
            set { expdate = value; }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }
    }
}
