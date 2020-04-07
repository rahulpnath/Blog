using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace RoleBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public ActionResult<string>  Get()
        {
            var groups = Startup.GetAdGroups();

            var allGroups = string.Join(Environment.NewLine, groups.Select(a => a.GroupName));
            return allGroups;
        }
    }
}