using AppTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTask.Controllers;

public class DepartamentoController : Controller
{
    private readonly DbTasksContext _context;

    public DepartamentoController(DbTasksContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Departamentos
            .OrderBy(d => d.Nome)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .Include(d => d.Funcionarios)
            .FirstOrDefaultAsync(d => d.Codigo == id);

        return departamento == null ? NotFound() : View(departamento);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Codigo,Nome,Sigla")] Departamento departamento)
    {
        if (ModelState.IsValid)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(departamento);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos.FindAsync(id);
        return departamento == null ? NotFound() : View(departamento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Sigla")] Departamento departamento)
    {
        if (id != departamento.Codigo)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(departamento);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .Include(d => d.Funcionarios)
            .FirstOrDefaultAsync(d => d.Codigo == id);

        return departamento == null ? NotFound() : View(departamento);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }

        var possuiFuncionarios = await _context.Funcionarios
            .AnyAsync(f => f.DepartamentoId == id);

        if (possuiFuncionarios)
        {
            TempData["Erro"] = "Não é possível excluir este departamento porque existem funcionários vinculados a ele.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
