using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private MyDbContext context;
        private List<Customer> CustomerList = new List<Customer>()
        {
            new Customer(){Name = "Kazek",Id = 0},
            new Customer(){Name = "Wladek", Id = 1}
        };

        public CustomerController()
        {
            this.context = new MyDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
        
        public ActionResult New()
        {
            var membershipTypes = this.context.MembershipType.ToList();
            var viewModel = new NewCustomerViewModel()
            {
                MembershipTypesList = membershipTypes
            };

            return View(viewModel);
        }

        [Route("customer/show-customers")]
        public ActionResult ShowCustomers()
        {
            return View(context.Customers.Include(c=>c.MembershipType).ToList());
        }

        [Route("customer/Details/{id}")]
        public ActionResult Details(int id)
        {
            if (context.Customers.Any(s => s.Id == id))
                return View(context.Customers.Include(c=>c.MembershipType).FirstOrDefault(c => c.Id == id));
            return HttpNotFound();
        }

 
        public ActionResult CustomerByName(string name)
        {
            return View(context.Customers.FirstOrDefault(s => s.Name == name));
        }
    }
}