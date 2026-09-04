using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers;

public class CentralDeCustoController : Controller
{
    private readonly DbTasksContext _context;

    public CentralDeCustoController(DbTasksContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.CentraisDeCusto
            .OrderBy(c => c.NomeCentral)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var central = await _context.CentraisDeCusto
            .FirstOrDefaultAsync(c => c.Codigo == id);

        return central == null ? NotFound() : View(central);
    }

    public IActionResult Create()
    {
        return View(new CentralDeCusto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Codigo,NomeCentral,ValorMetaAnual")] CentralDeCusto central)
    {
        if (ModelState.IsValid)
        {
            _context.CentraisDeCusto.Add(central);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(central);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var central = await _context.CentraisDeCusto.FindAsync(id);
        return central == null ? NotFound() : View(central);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Codigo,NomeCentral,ValorMetaAnual")] CentralDeCusto central)
    {
        if (id != central.Codigo) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(central);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CentralExists(central.Codigo)) return NotFound();
                throw;
            }
        }

        return View(central);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var central = await _context.CentraisDeCusto
            .FirstOrDefaultAsync(c => c.Codigo == id);

        return central == null ? NotFound() : View(central);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var central = await _context.CentraisDeCusto.FindAsync(id);

        if (central != null)
        {
            _context.CentraisDeCusto.Remove(central);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CentralExists(int id)
    {
        return _context.CentraisDeCusto.Any(c => c.Codigo == id);
    }
}
