using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daEjecutivoUrbanizacion
    {
        public string ValidarEjecutivo(string CodigoAsesor, SP.ClientContext contexto)
        {
            
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>      
                          <Eq> @<FieldRef Name='CodigoAsesor'/>  <Value Type='Text'>" + CodigoAsesor + @"</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enEjecutivosUrbanizacion.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count > 0)
            {
                SP.ListItem item = registros.FirstOrDefault();

                return  ((SP.FieldLookupValue)item["Ejecutivo"]).LookupValue;
            }
            else
            {
                return string.Empty;
            }


        }
        public string InsertEjecutivoUrbanizaciones(enVentasUrbanizacion EjeUrba)
        {
            string Rpta = string.Empty;

            SP.ClientContext Contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();
            try
            {
                SP.List ObjetoLista = Contexto.Web.Lists.GetByTitle(enEjecutivosUrbanizacion.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);


                enZonasUrbanizacion ZonaUrba = new daZonasUrbanizacion().GetUrbanizacion(EjeUrba.Zona);

                RegistroLista[enEjecutivosUrbanizacion.SP_CodigoAsesor] = EjeUrba.CodigoAsesor;
                // RegistroLista[enEjecutivosUrbanizacion.SP_Ejecutivo] = EjeUrba.Ejecutivo;
                RegistroLista[enEjecutivosUrbanizacion.SP_Supervisor] = ZonaUrba.Supervisor;
                RegistroLista[enEjecutivosUrbanizacion.SP_Rol] = new SP.FieldLookupValue().LookupId = 3;
                RegistroLista[enEjecutivosUrbanizacion.SP_Zona] = new SP.FieldLookupValue().LookupId = ZonaUrba.Zona;

                RegistroLista.Update();
                Contexto.ExecuteQuery();

                return "Ejecuivo " + EjeUrba.CodigoAsesor;
            }
            catch (Exception ex)
            {
                return "Error es " + ex.Message;
            }

        }

        public List<enEjecutivosUrbanizacion> ListarEjecutivo(SP.ClientContext contexto)
        {
            enEjecutivosUrbanizacion ejecutivosUrbanizacion = new enEjecutivosUrbanizacion();
            List<enEjecutivosUrbanizacion> lstejecutivosUrbanizacions = new List<enEjecutivosUrbanizacion>();
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
            <Query>
            </Query>
            </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enEjecutivosUrbanizacion.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count > 0)
            {
                //SP.ListItem item = registros.FirstOrDefault();

                foreach (SP.ListItem item in registros)
                {
                    ejecutivosUrbanizacion = new enEjecutivosUrbanizacion();
                    //ejecutivosUrbanizacion.Ejecutivo = ((SP.FieldLookupValue)item["Ejecutivo"]).LookupValue;

                    if (item["Ejecutivo"] != null)
                    {
                        ejecutivosUrbanizacion.EjecutivoLookUp = (SP.FieldUserValue)item["Ejecutivo"];
                    }
                    if (item["Zona"] != null)
                    {
                        ejecutivosUrbanizacion.ZonaLookUp = ((SP.FieldLookupValue)item["Zona"]);
                    }
                    if (item["CodigoAsesor"]!=null)
                    {
                        ejecutivosUrbanizacion.CodigoAsesor = item["CodigoAsesor"].ToString();
                    }
                    if (item["Supervisor"] != null)
                    {
                        ejecutivosUrbanizacion.SupervisorLookUp = (SP.FieldUserValue)item["Supervisor"];
                    }
                    if (item["Rol"] != null)
                    {
                        ejecutivosUrbanizacion.RolLookUp = (SP.FieldLookupValue)item["Rol"];
                    }
                    lstejecutivosUrbanizacions.Add(ejecutivosUrbanizacion);

                }
                //return ((SP.FieldLookupValue)item["Ejecutivo"]).LookupValue;
            }
            else
            {
                lstejecutivosUrbanizacions = new List<enEjecutivosUrbanizacion>();
            }

            return lstejecutivosUrbanizacions;
        }
    }
}
