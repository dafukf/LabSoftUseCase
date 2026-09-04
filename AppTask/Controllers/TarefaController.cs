using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppTask.Models;
using AppTask.Models.Services;

namespace AppTask.Controllers
{
    public class TarefaController : Controller
    {
        private readonly DbTasksContext _context;
        private RegraTarefa _regraTarefa;

        public TarefaController(DbTasksContext context)
        {
            _context = context;
            _regraTarefa = new RegraTarefa();
        }

        public async Task<IActionResult> Index()
        {
            var dbTasksContext = _context.Tarefas.Include(t => t.Funcionario);
            return View(await dbTasksContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var tarefa = await _context.Tarefas.Include(t => t.Funcionario).FirstOrDefaultAsync(m => m.Codigo == id);
            return tarefa == null ? NotFound() : View(tarefa);
        }

        public IActionResult Create()
        {
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Codigo", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            // Remove validação automática da navegação para evitar erros falsos
            ModelState.Remove(nameof(Tarefa.Funcionario));

            // Validação de Datas (Regra de Negócio)
            if (!_regraTarefa.validarDataFinal(tarefa.DataIniciada, tarefa.DataFinalizada))
            {
                ModelState.AddModelError(nameof(Tarefa.DataFinalizada), "A data final deve ser posterior à data inicial.");
            }

            // Validação de Funcionário obrigatório
            if (tarefa.FuncionarioId <= 0)
            {
                ModelState.AddModelError(nameof(Tarefa.FuncionarioId), "Selecione um funcionário.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Tarefas.Add(tarefa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.FuncionarioId);
            return View(tarefa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();
            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.FuncionarioId);
            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,FuncionarioId")] Tarefa tarefa)
        {
            if (id != tarefa.Codigo) return NotFound();

            ModelState.Remove(nameof(Tarefa.Funcionario));

            // Validação de Datas (Regra de Negócio)
            if (!_regraTarefa.validarDataFinal(tarefa.DataIniciada, tarefa.DataFinalizada))
            {
                ModelState.AddModelError(nameof(Tarefa.DataFinalizada), "A data final deve ser posterior à data inicial.");
            }

            if (tarefa.FuncionarioId <= 0)
            {
                ModelState.AddModelError(nameof(Tarefa.FuncionarioId), "Selecione um funcionário.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarefa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Codigo)) return NotFound();
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.FuncionarioId);
            return View(tarefa);
        }

        public IActionResult Sobre() => View();

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var tarefa = await _context.Tarefas.Include(t => t.Funcionario).FirstOrDefaultAsync(m => m.Codigo == id);
            return tarefa == null ? NotFound() : View(tarefa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null) _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id) => _context.Tarefas.Any(e => e.Codigo == id);
    }
}
