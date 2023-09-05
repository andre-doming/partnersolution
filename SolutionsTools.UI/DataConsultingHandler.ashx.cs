using SolutionsTools.DAL;
using SolutionsTools.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionsTools.UI
{
    public class DataConsultingHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.RequestType == "POST")
                {
                    var cpf = string.Empty;

                    var clientId = Guid.Parse(context.Request.Form["Id_Cliente"]);

                    var cliente = GetClient(clientId);

                    if (cliente != null)
                    {
                        cpf = cliente.Cpf;
                    }

                    context.Response.Write(cpf);
                }
                else
                {
                    context.Response.Write("Erro: Request was sent incorrectly somehow");
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
            }
        }
        private Cliente GetClient(Guid id)
        {
            var cs = new ClientServices();

            var ret = cs.GetById(id);

            return ret;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}