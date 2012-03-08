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
            Console.WriteLine(s.Time(Fetch.FetchWinePages, 100));
            Console.WriteLine(s.Time(Fetch.FetchWinePages2,100));

            var documentStore = new DocumentStore
                                    {
                                        ConnectionStringName = "CS"
                                    }.Initialize();


            //string wineid;

            ////using (var session = documentStore.OpenSession())
            ////{
            ////    var entity = new Wine { Name = "Allo" };

            ////    session.Store(entity);
            ////    session.SaveChanges();
            ////    wineid = entity.Id;
                
            ////}

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
