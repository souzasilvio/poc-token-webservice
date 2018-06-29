using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebAppServico;
using WebAppServico.App_Start;

namespace Mrv.Web.Service.SAP.CRM
{
    /// <summary>
    /// Summary description for ConsultaCRM
    /// </summary>
    [WebService(Namespace = "http://mrv.com.br/ConsultaCRM")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ConsultaCRM : System.Web.Services.WebService
    {

        public AuthHeader SoapToken;

        private void ValidaToken()
        {
            if (SoapToken.Token == null)
            {
                throw new SoapException("Não autorizado.", SoapException.ClientFaultCode);
            }
            try
            {
                var ok = ManualValidadeToken.GetInstance().Authorize(SoapToken.Token);
                if (!ok)
                {
                    throw new SoapException("Não autorizado.", SoapException.ClientFaultCode);
                }

            }
            catch (Exception ex)
            {
                throw new SoapException("Não autorizado.", SoapException.ClientFaultCode, ex);
            }
        }

        [SoapHeader("SoapToken")]
        [WebMethod(Description="Consultar Datas CRM")]
        public List<BlocoInfoSAP> ObterInformacaoBloco()
        {
            //Valida o token passado no header
            ValidaToken();
            List<BlocoInfoSAP> blocosSAP = new List<BlocoInfoSAP>();           
            for(int i = 1;i<=10;i++)
            {
                BlocoInfoSAP item = new BlocoInfoSAP();
                item.Nome = $"Bloco {i}";
                blocosSAP.Add(item);                
            }

            return blocosSAP;
        }

        [WebMethod(Description = "Valida se unidade está correta no SAP para realização de venda.")]
        public string ValidarUnidadeSAP(string idUnidade)
        {
            return "OK";
        }

        [WebMethod(Description = "Cancela Agendamento ao crédito Proposta de Analise de credito  ")]
        public void CancelarAgendamento(string oportundiadeId)
        {
          
        }
    }
}
