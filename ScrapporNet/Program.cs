using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;

namespace ScrapporNet
{
    class Program
    {
        static void Main(string[] args)
        {

            var documentStore = new DocumentStore { Url = "http://pascal-pc:8080" };
            documentStore.Initialize();

            string wineid;

            //using (var session = documentStore.OpenSession())
            //{
            //    var entity = new Wine { Name = "Allo" };

            //    session.Store(entity);
            //    session.SaveChanges();
            //    wineid = entity.Id;
                
            //}

            using (var session = documentStore.OpenSession())
            {
                //var entity = session.Load<Wine>(wineid);
                //Console.WriteLine(entity.Name);
                //session.SaveChanges();

                foreach (var variable in session.Query<Wine>())
                {
                    Console.WriteLine(variable.Id);
                }

            }
            Console.ReadLine();
        }
    }

    public class Wine
    {
        public string Name { get; set; }
        public string Id { get; set; }

    }
}
