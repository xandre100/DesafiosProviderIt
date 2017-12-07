using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafios.model.repositorio
{
    public class Repositorio : IRepositorio
    {
        #region propriedades

        private Processo Processo { get; set; }
        private List<Processo> processos { get; set; }
        private IRepositorio repositorio;

        #endregion

        #region construtores

        public Repositorio()
        {
            if (processos == null)
                processos = new List<Processo>();
        }

        public Repositorio(IRepositorio repositorio)
        {
            if (processos == null)
                processos = new List<Processo>();

            this.repositorio = repositorio;
        }

        public Repositorio(IRepositorio repositorio, List<Processo> processos)
        {
            if (this.processos == null)
                this.processos = new List<Processo>();

            this.processos = processos;
            this.repositorio = repositorio;
        }

        #endregion

        #region Métodos Públicos

        public void criar(Processo processo)
        {
            processos.Add(processo);   
        }

        public List<Processo> obterProcessos()
        {
            return processos.ToList();
        }

        public List<Processo> obterProcessos(Func<Processo, Boolean> condicao)
        {
            return processos.Where(condicao).ToList();
        }

        public decimal obterTotalProcessosAtivos()
        {
            decimal total = 0;

            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.Status == StatusProcesso.Ativo);

            List<Processo> processos = obterProcessos(condicao).ToList();

            if ((processos != null) && (processos.Count() > 0))
            {
                total = processos.Sum(p => p.Valor);
            }
            else
                throw new Exception("Não existem processos ativos na Companhia");

            return total;
        }

        public decimal obterMedia(string cnpj, string estado)
        {
            decimal media = 0;
            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.Empresa.CNPJ == cnpj 
                                                                             && p.Estado.Nome == estado);

            List<Processo> processos = obterProcessos(condicao).ToList();

            if ((processos != null) && (processos.Count() > 0))
            {
                media = processos.Average(p => p.Valor);
            }
            else
                throw new Exception("Não existem processos ativos na Companhia");

            return media;
        }

        public decimal obterProcessosPorValorEstipulado(decimal valor)
        {
            int numeroProcessos = 0;
            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.Status == StatusProcesso.Ativo
                                                                             && p.Valor > valor);

            List<Processo> processos = obterProcessos(condicao).ToList();

            if ((processos != null) && (processos.Count() > 0))
            {
                numeroProcessos = processos.Count();
            }
            else
                throw new Exception("Não existem processos que atendandam a condição informada");

            return numeroProcessos;
        }

        #endregion

    }
}
