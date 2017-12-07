using System;
using System.Collections.Generic;

namespace desafios.model.repositorio
{
    public interface IRepositorio
    {
        void criar(Processo processo);
        decimal obterMedia(string cnpj, string estado);
        List<Processo> obterProcessos();
        List<Processo> obterProcessos(Func<Processo, bool> condicao);
        decimal obterProcessosPorValorEstipulado(decimal valor);
        decimal obterTotalProcessosAtivos();
    }
}