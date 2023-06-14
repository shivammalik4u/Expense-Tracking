using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> selectedTransaction = _context.Transactions
                .Include(x => x.Category)
                .ToList();
            //.Where(y => y.Date >= StartDate && y.Date <= EndDate)
            int totalIncome = selectedTransaction.
                Where(x => x.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0");
            int totalExpense = selectedTransaction.
                Where(x => x.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("C0");

            int Balance = totalIncome - totalExpense;

            ViewBag.Balance = Balance.ToString("C0");

            ViewBag.RecentTransactions = _context.Transactions.Include(x => x.Category)
                .OrderByDescending(x => x.Date).Take(10).ToList();

            return View();
        }
    }
}
