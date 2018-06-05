using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using UserDataAccess;
namespace UserService.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        UserDBEntities ue = new UserDBEntities();
        [HttpGet]
        [Route("findall")]
        public HttpResponseMessage findAll()
        {

            try
            {
                var response = new HttpResponseMessage();
                response.Content = new StringContent(JsonConvert.SerializeObject(ue.UserDetails.ToList()));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage create(UserDetail userdetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                ue.UserDetails.Add(userdetail);
                ue.SaveChanges();
                response.StatusCode = HttpStatusCode.OK;
                return Request.CreateResponse(HttpStatusCode.OK,userdetail);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }

        
        [HttpPost]
        [Route("check")]
        public HttpResponseMessage check(UserDetail userdetail)
        {

            try
            {
                var user = ue.UserDetails.SingleOrDefault(u => u.Mobile_No == userdetail.Mobile_No);
                if(user==null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                if (user.Nepassword==userdetail.Nepassword)
                    return Request.CreateResponse(HttpStatusCode.OK, userdetail);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPut]
        [Route("Update")]
        public HttpResponseMessage update(UserDetail userdetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                var Currentuser = ue.UserDetails.SingleOrDefault(u => u.Mobile_No == userdetail.Mobile_No);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                Currentuser.Username = userdetail.Username;
                Currentuser.Nepassword = userdetail.Nepassword;
                Currentuser.Repassword = userdetail.Repassword;
                ue.SaveChanges();
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage delete(UserDetail userdetail)
        {

            try
            {
                var response = new HttpResponseMessage();
                var Currentuser = ue.UserDetails.SingleOrDefault(u => u.Mobile_No == userdetail.Mobile_No);
                if (Currentuser == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                ue.UserDetails.Remove(Currentuser);
                ue.SaveChanges();

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }

        }
    }
}
