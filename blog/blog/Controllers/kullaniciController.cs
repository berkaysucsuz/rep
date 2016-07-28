using blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using JsonPatch;
using System.Web.Http;
     


namespace blog.Controllers
{
    [Authorize]
    //[HttpGet]
    public class kullaniciController : ApiController
    {
        blogcontext db = new blogcontext();

     [AllowAnonymous]
        public List<kullanici> Get()
        {

               

            var kullanici = db.kullanicilar.ToList<kullanici>();
            return kullanici;

        }
        [Authorize(Roles = "admin")]
        public HttpResponseMessage Get(int id)
        {            var kullanici = db.kullanicilar.Find(id);
            HttpResponseMessage msg;
            
            if (kullanici == null)
            {
                msg = Request.CreateResponse(HttpStatusCode.BadRequest,"tanımlanan id bulunamadı.");
               return msg;
            }
            else { msg = Request.CreateResponse(HttpStatusCode.Found,kullanici);
            return msg;        
            }
    
        
        }

        public HttpResponseMessage Get(int index, int size) {
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK, db.kullanicilar.OrderBy(q => q.kullaniciId).Skip((index - 1) * size).Take(size));
       
            return resp;
        
        } //kontrol et.

        public HttpResponseMessage Get(string ad, string sifre)
        {
            var query = from kullanici in db.kullanicilar
                        where
                            kullanici.kullaniciAd == ad && kullanici.password == sifre
                        select kullanici;
            HttpResponseMessage resp;
            int sayi;
            sayi = query.Count();
            if (sayi > 0)
            {
                resp = Request.CreateResponse(HttpStatusCode.OK, db.kullanicilar.ToList());
                return resp;


            }
            else { resp = Request.CreateResponse(HttpStatusCode.BadRequest, "kul veya sifre yanlis"); return resp; }

        }

        public HttpResponseMessage Get(string tkn)
        {
            var query = from token in db.tokens where token.tknaccess == tkn select token;
            HttpResponseMessage resp;
            int sayi;
            sayi = query.Count();
         
                if(sayi>0){
                    var list = db.kullanicilar.ToList<kullanici>();
                    resp = Request.CreateResponse(HttpStatusCode.OK, list);
                    return resp;    
                }
                else
                {
                    resp = Request.CreateResponse(HttpStatusCode.BadRequest, "deneme");
                    return resp;
                }


        }

        //public HttpResponseMessage Get(string name)// !!!!!!!!!!!!!  kontrol döngüleri ekle  !!!!!!!!!!
        //{
        //    var query = from kullanici in db.kullanicilar
        //                where
        //                    kullanici.kullaniciAd == name
        //                select kullanici;
        //    HttpResponseMessage resp;
        //    resp = Request.CreateResponse(HttpStatusCode.Found, query.ToList());
        //        return resp;


        //}
      
        public HttpResponseMessage Post(kullanici k)
        {
            HttpResponseMessage resp;
            if (!ModelState.IsValid)
            {
                resp = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                return resp;
            }
            else {
                var snc = from kullanici in db.kullanicilar
                          where kullanici.kullaniciAd == k.kullaniciAd
                          select kullanici;
                if(snc.Count()>0){
                    
                    resp = Request.CreateResponse(HttpStatusCode.NotAcceptable,"kullanıcı adı önceden alınmış");
                    return resp;
                }
                else {
                   k = db.kullanicilar.Add(k);
                    db.SaveChanges();
                    resp = Request.CreateResponse(HttpStatusCode.OK, "vt'ye eklendi");
                    return resp;
                
                }
            
            
}

        }

        public HttpResponseMessage Put(int id, kullanici k)
        {


            HttpResponseMessage resp;
            k.kullaniciId = id;
                if (!ModelState.IsValid || k == null) // K ?
                {
                    resp = Request.CreateResponse(HttpStatusCode.BadRequest);
                    return resp;

                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    resp = Request.CreateResponse(HttpStatusCode.OK);
                    return resp;
                }
            
        }

        public HttpResponseMessage Patch(Guid id, JsonPatchDocument<kullanici> patch){
            kullanici upt = db.kullanicilar.Find(id);

            patch.ApplyUpdatesTo(upt);
            db.Entry(upt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK);
            return resp;
            
        }
       
        

        public IHttpActionResult Delete(int id)
        {

            kullanici kullanici = db.kullanicilar.Find(id);
            if(kullanici!=null)
            {
            db.kullanicilar.Remove(kullanici);
            db.SaveChanges();
            return Ok();
            }
            else { return BadRequest(); }


        }

     
    }
}
