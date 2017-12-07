using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafios.model
{
    public class Processo
    {
        #region propriedades

        public Empresa Empresa { get; set; }
        public StatusProcesso Status { get; set; }
        public decimal Valor { get; set; }
        public DateTime IniciadoEm { get; set; }
        public String Numero { get; set; }
        public Estado Estado { get; set; }


        #endregion

        #region construtores

        public Processo()
        {

        }

        public Processo(Empresa empresa, StatusProcesso status, decimal valor, DateTime iniciadoEm, string numero, Estado estado)
        {
            this.Empresa = empresa;
            this.Status = status;
            this.Valor = valor;
            this.IniciadoEm = iniciadoEm;
            this.Numero = numero;
            this.Estado = estado;
        }

        #endregion
    
    }
}

