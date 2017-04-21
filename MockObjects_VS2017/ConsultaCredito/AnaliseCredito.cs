namespace ConsultaCredito
{
    public class AnaliseCredito
    {
        private IServicoConsultaCredito _servConsultaCredito;

        public AnaliseCredito(IServicoConsultaCredito servConsultaCredito)
        {
            this._servConsultaCredito = servConsultaCredito;
        }

        public StatusConsultaCredito ConsultarSituacaoCPF(string cpf)
        {
            try
            {
                var pendencias =
                    this._servConsultaCredito.ConsultarPendenciasPorCPF(cpf);

                if (pendencias == null)
                    return StatusConsultaCredito.ParametroEnvioInvalido;
                else if (pendencias.Count == 0)
                    return StatusConsultaCredito.SemPendencias;
                else
                    return StatusConsultaCredito.Inadimplente;
            }
            catch
            {
                return StatusConsultaCredito.ErroComunicacao;
            }
        }
    }
}