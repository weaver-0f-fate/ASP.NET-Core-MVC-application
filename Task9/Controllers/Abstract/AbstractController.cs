using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task9.Controllers.Abstract {
    public abstract class AbstractController : Controller {
        public IActionResult Index() {
            throw new NotImplementedException();
        }
    }
}
