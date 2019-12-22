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
    class Program   
    {
        public static void Main(string[] args)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("AtmDataBase.dat", FileMode.OpenOrCreate);
            stream.Close();

            Auth auth = new Auth(stream, formatter);
            auth.CardNumberAccess();
        }
    }
}
