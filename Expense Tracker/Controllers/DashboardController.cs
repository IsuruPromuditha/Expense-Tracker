using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Index()
        {
            //7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //Total income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expence
            int TotalExpenses = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpenses = TotalIncome.ToString("C0");

            //Total Balance
            int TotalBalance = TotalIncome - TotalExpenses;
            ViewBag.TotalBalance = TotalBalance.ToString("C0");

            return View();
        }
    }
}
