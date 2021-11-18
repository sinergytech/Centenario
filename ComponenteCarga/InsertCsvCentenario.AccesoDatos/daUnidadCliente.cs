using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;


namespace InsertCsvCentenario.AccesoDatos
{
    public class daUnidadCliente
    {

        public List<enUnidadCliente> TraertUnidadCliente(SP.ClientContext clientContext)
        {

            List<enUnidadCliente> rpta;

            try
            {
                rpta = new List<enUnidadCliente>();
                SP.CamlQuery query = SP.CamlQuery.CreateAllItemsQuery();
                SP.List oList = clientContext.Web.Lists.GetByTitle(enUnidadCliente.ListaNombreSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                clientContext.Load(registros);
                clientContext.ExecuteQuery();

                foreach (var item in registros)
                {
                    enUnidadCliente Uc = new enUnidadCliente();

                    Uc.Unidad = item[enUnidadCliente.SP_Unidad].ToString();
                    Uc.Sociedad = item[enUnidadCliente.SP_Sociedad].ToString();
                    Uc.CodigoCliente = item[enUnidadCliente.SP_CodigoCliente].ToString();

                    rpta.Add(Uc);
                }

            }
            catch (Exception ex)
            {
                rpta = new List<enUnidadCliente>();
            }

            return rpta;

        }

    }
}
