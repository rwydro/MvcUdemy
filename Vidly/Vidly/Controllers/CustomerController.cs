using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private MyDbContext context;

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
            var viewModel = new CustomerViewModel()
            {
                Customer = new Customer(),
                MembershipTypesList = membershipTypes
            };

            return View("CustomerForm",viewModel);
        }
            
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerViewModel newCustomer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerViewModel()
                {
                    Customer = newCustomer.Customer,
                    MembershipTypesList = this.context.MembershipType.ToList()
                };
                return View("CustomerForm", viewModel);
            }

            if (newCustomer.Customer.Id == 0)
            {
                context.Customers.Add(newCustomer.Customer);
            }
            else
            {
                var customer = context.Customers.Single(s => s.Id == newCustomer.Customer.Id);
                customer.Name = newCustomer.Customer.Name;
               // customer.BirthDate = newCustomer.Customer.BirthDate;
                customer.MembershipTypeId = newCustomer.Customer.MembershipTypeId;
                customer.IsSubscribedToNewsletter = newCustomer.Customer.IsSubscribedToNewsletter;
            }
            context.SaveChanges();
            return RedirectToAction("ShowCustomers", "Customer");
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

        public ActionResult Edit(int id)
        {
            var customer = context.Customers.SingleOrDefault(s => s.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var correctCustomer = new CustomerViewModel()
            {
                Customer = customer,
                MembershipTypesList = this.context.MembershipType.ToList()
        };
            return View("CustomerForm", correctCustomer);
        }

        public ActionResult Delete(int id)
        {
            var deletedCustomer = context.Customers.FirstOrDefault(s => s.Id == id);
            if (deletedCustomer != null) this.context.Customers.Remove(deletedCustomer);
            context.SaveChanges();
            return RedirectToAction("ShowCustomers", "Customer");
        }
    }
}