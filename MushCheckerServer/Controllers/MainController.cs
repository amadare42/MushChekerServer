using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MushCheckerServer.Controllers
{
    public class MainController : ApiController
    {
        public IHttpActionResult GetHelloWorld()
        {
            return Ok("Hello, world!");
        }

        public string EnableListening(string name, string mush_sid, string saved_tid_sess, string tid_contact)
        {
            return name;
        }

        //$('.tid_sideHeader span').text()
    }
}