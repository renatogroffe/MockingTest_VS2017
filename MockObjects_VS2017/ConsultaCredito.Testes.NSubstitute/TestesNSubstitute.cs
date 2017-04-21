using System;
using System.Collections.Generic;
using Xunit;
using NSubstitute;

namespace ConsultaCredito.Testes.NSubstitute
{
    public class TestesNSubstitute
    {
        private const string CPF_INVALIDO = "123A";
        private const string CPF_ERRO_COMUNICACAO = "76217486300";
        private const string CPF_SEM_PENDENCIAS = "60487583752";
        private const string CPF_INADIMPLENTE = "82226651209";

        private IServicoConsultaCredito mock;

        public TestesNSubstitute()
        {
            mock = Substitute.For<IServicoConsultaCredito>();

            mock.ConsultarPendenciasPorCPF(CPF_INVALIDO)
                .Returns((List<Pendencia>)null);

            mock.ConsultarPendenciasPorCPF(CPF_ERRO_COMUNICACAO)
                .Returns(s => { throw new Exception("Erro de comunicação..."); });

            mock.ConsultarPendenciasPorCPF(CPF_SEM_PENDENCIAS)
                .Returns(new List<Pendencia>());

            Pendencia pendencia = new Pendencia();
            pendencia.CPF = CPF_INADIMPLENTE;
            pendencia.NomePessoa = "João da Silva";
            pendencia.NomeReclamante = "Empresa XYZ";
            pendencia.DescricaoPendencia = "Parcela não paga";
            pendencia.VlPendencia = 700;
            List<Pendencia> pendencias = new List<Pendencia>();
            pendencias.Add(pendencia);

            mock.ConsultarPendenciasPorCPF(CPF_INADIMPLENTE)
                .Returns(pendencias);
        }

        private StatusConsultaCredito ObterStatus(string cpf)
        {
            AnaliseCredito analise = new AnaliseCredito(mock);
            return analise.ConsultarSituacaoCPF(cpf);
        }

        [Fact]
        public void TestarCPFInvalido()
        {
            StatusConsultaCredito status = ObterStatus(CPF_INVALIDO);
            Assert.Equal(StatusConsultaCredito.ParametroEnvioInvalido, status);
        }

        [Fact]
        public void TestarErroComunicacaoNSubstitute()
        {
            StatusConsultaCredito status = ObterStatus(CPF_ERRO_COMUNICACAO);
            Assert.Equal(StatusConsultaCredito.ErroComunicacao, status);
        }

        [Fact]
        public void TestarCPFSemPendenciasNSubstitute()
        {
            StatusConsultaCredito status = ObterStatus(CPF_SEM_PENDENCIAS);
            Assert.Equal(StatusConsultaCredito.SemPendencias, status);
        }

        [Fact]
        public void TestarCPFInadimplenteNSubstitute()
        {
            StatusConsultaCredito status = ObterStatus(CPF_INADIMPLENTE);
            Assert.Equal(StatusConsultaCredito.Inadimplente, status);
        }
    }
}