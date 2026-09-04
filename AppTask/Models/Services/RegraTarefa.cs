using AppTask.Models.interfaces;


namespace AppTask.Models.Services
{
    public class RegraTarefa : RegraTarefas
    {
        public bool validarDataFinal(DateTime? datainicial, DateTime? datafinal)
        {
            return datafinal > datainicial;
        }
    }
}
