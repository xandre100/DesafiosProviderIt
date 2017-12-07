using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafios.model
{
    public class Empresa
    {
        #region propriedades
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public Estado Estado { get; set; }
        #endregion

        #region construtores

        public Empresa()
        {

        }

        public Empresa(string nome, string cnpj, Estado estado)
        {
            this.Nome = nome;
            this.CNPJ = cnpj;
            this.Estado = estado;
        }

        #endregion
    }
}
