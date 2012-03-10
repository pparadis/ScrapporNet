using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Raven.Client.Document;

namespace ScrapporNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Stopwatch();
            Fetch.FetchWinePages();
            //Fetch.DownloadWinePages();
            //Console.WriteLine(s.Time(Fetch.FetchWinePages, 1));
            //Console.WriteLine(s.Time(Fetch.FetchWinePages2,100));

            


            //string wineid;

            

            //using (var session = documentStore.OpenSession())
            //{
            //    //var entity = session.Load<Wine>(wineid);
            //    //Console.WriteLine(entity.Name);
            //    //session.SaveChanges();

            //    foreach (var variable in session.Query<Wine>())
            //    {
            //        Console.WriteLine(variable.Id);
            //    }

            //}
            Console.ReadLine();
        }
    }
}
