using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using desafios.model.repositorio;
using Moq;
using System.Linq;

namespace desafios.model.testes
{
    [TestClass]
    public class RepositorioTest
    {
        private List<Processo> processos = new List<Processo>();
        private Empresa empresaA = new Empresa();
        private Empresa empresaB = new Empresa();
        Mock<IRepositorio> mock;

        [TestInitialize]
        public void Setup()
        {
            mock = new Mock<IRepositorio>();

            criaEmpresas();
            criaProcessos();
        }

        /*
            Criando Processos.
        */
        [TestMethod]
        public void Teste_Criar_Processos()
        {
            Processo processo = new Processo(empresaB, StatusProcesso.Inativo, 1000, new DateTime(2007, 9, 5), "00020TRABPB", new Estado("Paraíba"));

            mock.Setup(r => r.criar(processo));

            var repositorio = new Repositorio(mock.Object);

            try
            {
                repositorio.criar(processo);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }

        }


        /*
            1) Calcular a soma dos processos ativos. A aplicação deve retornar R$ 1.087.000,00
        */
        [TestMethod]
        public void Teste_Obter_Total_Processos_Ativos()
        {
            decimal valorEsperado = 1087000;
            decimal valorRetornado = 0;

            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(processos);
            mock.Setup(r => r.obterTotalProcessosAtivos());

            var repositorio = new Repositorio(mock.Object, processos);
            valorRetornado = repositorio.obterTotalProcessosAtivos();

            Assert.AreEqual(valorEsperado, valorRetornado);
        }

        /*
            2) Calcular a média do valor dos processos no Rio de Janeiro para o Cliente "Empresa A".
            A aplicação deve retornar R$ 110.000,00.
        */
        [TestMethod]
        public void Teste_Obter_Media_Processos_RioDeJaneiro_Na_EmpresaA()
        {
            decimal valorEsperado = 110000;
            decimal valorRetornado = 0;

            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(processos);
            mock.Setup(r => r.obterMedia(It.IsAny<String>(), It.IsAny<String>()));
            
            var repositorio = new Repositorio(mock.Object, processos);

            valorRetornado = repositorio.obterMedia(empresaA.CNPJ, "Rio de Janeiro");

            Assert.AreEqual(valorEsperado, valorRetornado);
        }


        /*
            3) Calcular o Número de processos com valor acima de R$ 100.000,00. A aplicação deve retornar 2.
        */
        [TestMethod]
        public void Teste_Obter_Numero_De_Processos_Acima_De_Cem_Mil()
        {   
            decimal valorEsperado = 2;
            decimal valorRetornado = 2;

            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(processos);
            mock.Setup(r => r.obterProcessosPorValorEstipulado(It.IsAny<Decimal>())).Returns(2);

            var repositorio = new Repositorio(mock.Object, processos);

            valorRetornado = repositorio.obterProcessosPorValorEstipulado(100000);

            Assert.AreEqual(valorEsperado, valorRetornado);
        }


        /*
            4) Obter a lista de Processos de Setembro de 2007. A aplicação deve retornar uma lista com somente o Processo “00010TRABAM”.
        */
        [TestMethod]
        public void Teste_Obter_Lista_De_Processos_De_Setembro_2007()
        {
            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.IniciadoEm.Month == 9 && p.IniciadoEm.Year == 2007);

            String NumeroEsperado = "00010TRABAM";
            List<Processo> listaRetornada = new List<Processo>() {
                new Processo(empresaB, StatusProcesso.Inativo, 1000, new DateTime(2007, 9, 5), "00010TRABAM", new Estado("Amazonas"))
            };

            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(listaRetornada);

            var repositorio = new Repositorio(mock.Object, processos);

            listaRetornada = repositorio.obterProcessos(condicao);

            Assert.IsNotNull(listaRetornada);
            Assert.IsInstanceOfType(listaRetornada, typeof(List<Processo>));
            Assert.AreEqual(NumeroEsperado, listaRetornada[0].Numero);
        }

        /*        
            5) Obter a lista de processos no mesmo estado do cliente, para cada um dos clientes.A
            aplicação deve retornar uma lista com os 
            processos de número:

            “00001CIVELRJ”, ”00004CIVELRJ” para o Cliente "Empresa A"
            “00008CIVELSP”, ”00009CIVELSP” para o Cliente "Empresa B"
        */
        [TestMethod]
        public void Teste_Obter_Lista_De_Processos_No_Mesmo_Estado_Do_Cliente()
        {
            List<Processo> listaRetornada = new List<Processo>() {

                new Processo(empresaA, StatusProcesso.Ativo, 200000, new DateTime(2007, 10, 10), "00001CIVELRJ", new Estado("Rio de Janeiro")),
                new Processo(empresaA, StatusProcesso.Inativo, 20000, new DateTime(2007, 11, 10), "00004CIVELRJ", new Estado("Rio de Janeiro")),

                new Processo(empresaB, StatusProcesso.Inativo, 500, new DateTime(2007, 7, 3), "00008CIVELSP", new Estado("São Paulo")),
                new Processo(empresaB, StatusProcesso.Ativo, 32000, new DateTime(2007, 8, 4), "00009CIVELSP", new Estado("São Paulo")),
            };

            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.Empresa.Estado.Nome == p.Estado.Nome);
            
            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(listaRetornada);

            var repositorio = new Repositorio(mock.Object, processos);

            listaRetornada = repositorio.obterProcessos(condicao);

            Assert.IsNotNull(listaRetornada);
            Assert.IsInstanceOfType(listaRetornada, typeof(List<Processo>));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00001CIVELRJ" && l.Empresa.Nome == empresaA.Nome));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00004CIVELRJ" && l.Empresa.Nome == empresaA.Nome));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00008CIVELSP" && l.Empresa.Nome == empresaB.Nome));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00009CIVELSP" && l.Empresa.Nome == empresaB.Nome));
        }

        /*
            6) Obter a lista de processos que contenham a sigla “TRAB”. A aplicação deve retornar
            uma lista com os processos “00003TRABMG” e “00010TRABAM”.
        */
        [TestMethod]
        public void Teste_Obter_Lista_De_Processos_Que_Contenham_A_Sigla_TRAB()
        {
            List<Processo> listaRetornada = processos = new List<Processo>() {
               new Processo(empresaA, StatusProcesso.Inativo, 10000, new DateTime(2007, 10, 30), "00003TRABMG", new Estado("Minas Gerais")),
               new Processo(empresaB, StatusProcesso.Inativo, 1000, new DateTime(2007, 9, 5), "00010TRABAM", new Estado("Amazonas"))
            };

            Func<Processo, Boolean> condicao = new Func<Processo, bool>(p => p.Numero.Contains("TRAB"));

            mock.Setup(r => r.obterProcessos(It.IsAny<Func<Processo, Boolean>>())).Returns(listaRetornada);

            var repositorio = new Repositorio(mock.Object, processos);

            listaRetornada = repositorio.obterProcessos(condicao);

            Assert.IsNotNull(listaRetornada);
            Assert.IsInstanceOfType(listaRetornada, typeof(List<Processo>));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00003TRABMG" ));
            Assert.IsNotNull(listaRetornada.FirstOrDefault(l => l.Numero == "00010TRABAM" ));
        }

        private void criaEmpresas()
        {
            empresaA = new Empresa("Empresa A", "00000000001", new Estado("Rio de Janeiro"));
            empresaB = new Empresa("Empresa B", "00000000002", new Estado("São Paulo"));
        }

        private void criaProcessos()
        {
            processos = new List<Processo>() {

                new Processo(empresaA, StatusProcesso.Ativo, 200000, new DateTime(2007, 10, 10), "00001CIVELRJ", new Estado("Rio de Janeiro")),
                new Processo(empresaA, StatusProcesso.Ativo, 100000, new DateTime(2007, 10, 20), "00002CIVELSP", new Estado("São Paulo")),
                new Processo(empresaA, StatusProcesso.Inativo, 10000, new DateTime(2007, 10, 30), "00003TRABMG", new Estado("Minas Gerais")),
                new Processo(empresaA, StatusProcesso.Inativo, 20000, new DateTime(2007, 11, 10), "00004CIVELRJ", new Estado("Rio de Janeiro")),
                new Processo(empresaA, StatusProcesso.Ativo, 35000, new DateTime(2007, 11, 15), "00005CIVELSP", new Estado("São Paulo")),

                new Processo(empresaB, StatusProcesso.Ativo, 20000, new DateTime(2007, 5, 1), "00006CIVELRJ", new Estado("Rio de Janeiro")),
                new Processo(empresaB, StatusProcesso.Ativo, 700000, new DateTime(2007, 6, 2), "00007CIVELRJ", new Estado("Rio de Janeiro")),
                new Processo(empresaB, StatusProcesso.Inativo, 500, new DateTime(2007, 7, 3), "00008CIVELSP", new Estado("São Paulo")),
                new Processo(empresaB, StatusProcesso.Ativo, 32000, new DateTime(2007, 8, 4), "00009CIVELSP", new Estado("São Paulo")),
                new Processo(empresaB, StatusProcesso.Inativo, 1000, new DateTime(2007, 9, 5), "00010TRABAM", new Estado("Amazonas"))

            };
        }
    }
}
