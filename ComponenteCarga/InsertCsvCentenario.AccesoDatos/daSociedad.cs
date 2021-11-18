using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daSociedad
    {
        public List<enSociedad> ListaUnidades(SP.ClientContext contexto)
        {
            List<enSociedad> Rpta = new List<enSociedad>();

            try
            {
                SP.CamlQuery query = SP.CamlQuery.CreateAllItemsQuery();

                SP.List oList = contexto.Web.Lists.GetByTitle(enSociedad.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                foreach (var item in registros)
                {
                    enSociedad So = new enSociedad();
                    So.Unidad = item[enSociedad.SP_Unidad].ToString();
                    So.Sociedad = item[enSociedad.SP_Sociedad].ToString();
                    Rpta.Add(So);
                }


            }
            catch (Exception  ex)
            {
                Rpta = new List<enSociedad>();
                throw ex;
            }


            return Rpta;
        }

    }
}
