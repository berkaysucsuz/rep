using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using blog.Models;
using System.Linq;
using System.Web.Http;
using System;


namespace blog.oauth1.providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public blogcontext db = new blogcontext();

        // client erişimine izin verme
        public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            
            var query = from kullanici in db.kullanicilar
                        where kullanici.kullaniciAd == context.UserName && kullanici.password == context.Password
                        select kullanici;
            int sonuc;
            sonuc = query.Count();
            //rol r = new rol();
            //var query1 = from rol in db.rols where rol.rolId == k.kullaniciId select rol;
            //r = query1.FirstOrDefault();

            if (sonuc > 0)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim("isim", context.UserName));
                identity.AddClaim(new Claim("Roles", "admin"));
                context.Validated(identity);
                //kullanici k = query.First<kullanici>();
                //token newtoken = new token();
                //newtoken.tknaccess = k.kullaniciAd;
                //newtoken.kullanici = k;
                //newtoken.date = DateTime.Now;
                //db.tokens.Add(newtoken);
                //db.SaveChanges();
        

                    
            }
            else
            {
                context.SetError("invalid_grant", "kul veya şifre yanlış");
            }
        }
    }
}