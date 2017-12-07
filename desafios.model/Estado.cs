using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafios.model
{
    public class Estado
    {
        #region propriedades
        public String Nome { get; set; }
        #endregion

        #region construtores

        public Estado()
        {

        }

        public Estado(string nome)
        {
            this.Nome = nome;
        }

        #endregion
    }
}

