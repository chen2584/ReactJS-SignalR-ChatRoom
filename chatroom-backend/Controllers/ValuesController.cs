using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chatroom_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chatroom_backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IHubContext<ChatServer> _hubContext;
        public ValuesController(IHubContext<ChatServer> hubContext)
        {
            _hubContext = hubContext;
        }
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            _hubContext.Clients.All.InvokeAsync("onReceiveMessage", "Server", "#000", "Hello from controller");
            Console.WriteLine("Sended Message");
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
