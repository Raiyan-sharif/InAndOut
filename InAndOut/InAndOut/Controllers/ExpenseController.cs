using InAndOut.Data;
using InAndOut.Models;
using InAndOut.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class ExpenseController : Controller
    {

        private readonly ApplicationDBContext _db;

        public ExpenseController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Expense> objList = _db.Expenses;
            foreach (var obj in objList)
            {
                obj.ExpenseType = _db.ExpensesType.FirstOrDefault(u => u.Id == obj.ExpenseTypeId);
            }
            return View(objList);
        }
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> TypeDropDown = _db.ExpensesType.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            //ViewBag.TypeDropDown = TypeDropDown;
            ExpenseVM expenseVM = new ExpenseVM()
            {
                Expense = new Expense(),
                TypeDropDown = _db.ExpensesType.Select(i => new SelectListItem{
                    Text = i.Name,
                    Value = i.Id.ToString()})
            };
            return View(expenseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseVM obj)
        {
            if (ModelState.IsValid)
            {
                
                _db.Expenses.Add(obj.Expense);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        
        public IActionResult Delete(int? id)
        {
            Console.WriteLine(id);

            if (id == null || id==0)
            {
                return NotFound();
            }
            var obj = _db.Expenses.Find(id);

            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Console.WriteLine(id);
            var obj = _db.Expenses.Find(id);
            if (obj == null){
                return NotFound();
            }
            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult Update(int? id)
        {
            ExpenseVM expenseVM = new ExpenseVM()
            {
                Expense = new Expense(),
                TypeDropDown = _db.ExpensesType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return NotFound();
            }
            expenseVM.Expense = _db.Expenses.Find(id);

            if (expenseVM.Expense == null)
            {
                return NotFound();
            }
            return View(expenseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ExpenseVM obj)
        {
            if (ModelState.IsValid)
            {
                _db.Expenses.Update(obj.Expense);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

    }
}
