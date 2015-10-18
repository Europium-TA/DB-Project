using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonDbStuff
{
    class Program
    {
        static void Main()
        {
            MongoClient client = new MongoClient("mongodb://localhost");

            var dataBase = client.GetDatabase("test");
        }
    }
}
