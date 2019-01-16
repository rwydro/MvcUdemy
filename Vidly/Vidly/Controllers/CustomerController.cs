using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private List<Customer> CustomerList = new List<Customer>()
        {
            new Customer(){Name = "Kazek",Id = 0},
            new Customer(){Name = "Wladek", Id = 1}
        };
     
        [Route("customer/show-customers")]
        public ActionResult ShowCustomers()
        {
            return View(CustomerList);
        }

        [Route("customer/Details/{id}")]
        public ActionResult Details(int id)
        {
            if (CustomerList.Any(s => s.Id == id))
                return View(CustomerList.FirstOrDefault(c => c.Id == id));
            return 
        }

        [Route("customer/{name}")]
        public ActionResult CustomerByName(string name)
        {
            return View(CustomerList.FirstOrDefault(s => s.Name == name));
        }
    }
}