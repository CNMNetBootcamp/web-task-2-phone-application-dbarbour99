using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CellPhone.Data;
using CellPhone.Models;

namespace CellPhone.Controllers
{
  public class PhonesController : Controller
  {
    private readonly PhoneContext _context;

    public PhonesController(PhoneContext context)
    {
      _context = context;
    }

    // GET: Phones
    public async Task<IActionResult> Index(
      string sortOrder,
      string currentFilter,
      string searchString,
      int? page)
    {
      ViewData["CurrentSort"] = sortOrder;
      ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "model_desc" : "";
      ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

      if (searchString != null)
      {
        page = 1;
      }
      else
      {
        searchString = currentFilter;
      }


      ViewData["CurrentFilter"] = searchString;

      var phones = from s in _context.Phones
                   select s;

      if (!String.IsNullOrEmpty(searchString))
      {
        phones = phones.Where(s => s.Model.Contains(searchString)
                               || s.Brand.Contains(searchString));
      }

      switch (sortOrder)
      {
        case "model_desc":
          phones = phones.OrderByDescending(s => s.Model);
          break;
        case "Date":
          phones = phones.OrderBy(s => s.ReleaseDate);
          break;
        case "date_desc":
          phones = phones.OrderByDescending(s => s.ReleaseDate);
          break;
        default:
          phones = phones.OrderBy(s => s.Model);
          break;
      }
      int pageSize = 3;
      return View(await PaginatedList<Phone>.CreateAsync(phones.AsNoTracking(), page ?? 1, pageSize));
    }

    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var phone = await _context.Phones
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.ID == id);

      if (phone == null)
      {
        return NotFound();
      }

      return View(phone);
    }

    // GET: Phones/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Phones/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
    [Bind("Model,Brand,Memory,Rating,Smart,ReleaseDate")] Phone phone)

    {
      try
      {
        if (ModelState.IsValid)
        {
          _context.Add(phone);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        return View(phone);
      }
      catch (DbUpdateException /* ex */)
      {
        //Log the error (uncomment ex variable name and write a log.
        ModelState.AddModelError("", "Unable to save changes. " +
            "Try again, and if the problem persists " +
            "see your system administrator.");
      }
      return View(phone);
    }

    // GET: Phones/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var phone = await _context.Phones.SingleOrDefaultAsync(m => m.ID == id);
      if (phone == null)
      {
        return NotFound();
      }
      return View(phone);
    }

    // POST: Phones/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var phoneToUpdate = await _context.Phones.SingleOrDefaultAsync(s => s.ID == id);
      if (await TryUpdateModelAsync<Phone>(
          phoneToUpdate,
          "",
          s => s.Model, s => s.Brand, s => s.Memory, s => s.Rating, s => s.Smart, s => s.ReleaseDate))
      {
        try
        {
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException /* ex */)
        {
          //Log the error (uncomment ex variable name and write a log.)
          ModelState.AddModelError("", "Unable to save changes. " +
              "Try again, and if the problem persists, " +
              "see your system administrator.");
        }
      }
      return View(phoneToUpdate);
    }

    public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null)
      {
        return NotFound();
      }

      var phone = await _context.Phones
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.ID == id);
      if (phone == null)
      {
        return NotFound();
      }

      if (saveChangesError.GetValueOrDefault())
      {
        ViewData["ErrorMessage"] =
            "Delete failed. Try again, and if the problem persists " +
            "see your system administrator.";
      }

      return View(phone);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var phone = await _context.Phones
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.ID == id);
      if (phone == null)
      {
        return RedirectToAction(nameof(Index));
      }

      try
      {
        _context.Phones.Remove(phone);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      catch (DbUpdateException /* ex */)
      {
        //Log the error (uncomment ex variable name and write a log.)
        return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
      }
    }
  }
}
