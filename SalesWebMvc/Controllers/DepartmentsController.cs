﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> list = new List<Department>();
            list.Add(new Department { Id = 1, Name = "Eletronics" });
            list.Add(new Department { Id = 2, Name = "Fashions" });
            list.Add(new Department { Id = 3, Name = "Office" });
            list.Add(new Department { Id = 4, Name = "Games" });

            return View(list);
        }
    }
}