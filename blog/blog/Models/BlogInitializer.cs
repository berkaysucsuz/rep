using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using blog.Models;



namespace blog.Models
{
    public class BlogInitializer : DropCreateDatabaseAlways<blogcontext>
    {


        protected override void Seed(blogcontext context)
        {
            var kullanicilar = new List<kullanici>
    {
        new kullanici {
        kullaniciAd="cevahir",
        DogumYeri="bakırköy",
        DogumTarihi=DateTime.Now,

        },
        
        new kullanici {
        kullaniciAd="sbdhs",
        DogumYeri="bakır",
        DogumTarihi=DateTime.Now,}

    };
            kullanicilar.ForEach(s => context.kullanicilar.Add(s));
            context.SaveChanges();








        }
    }
}