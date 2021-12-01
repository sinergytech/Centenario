using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;
using System.Globalization;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daVentasUrbanizacion
    {
        public string InsertarRegistro(enVentasUrbanizacion VentasUrba, SP.ClientContext contexto)
        {

            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                enVentasUrbanizacion ZonaYEjecutivo = RecuperarZona_y_Ejecutivo(VentasUrba.CodigoAsesor, contexto);

                RegistroLista[enVentasUrbanizacion.SP_Zona] = ZonaYEjecutivo.ZonaLookUp;
                RegistroLista[enVentasUrbanizacion.SP_Ejecutivo] = ZonaYEjecutivo.EjecutivoLookUp;
                RegistroLista[enVentasUrbanizacion.SP_VentaTotal] = VentasUrba.VentaTotal;
                RegistroLista[enVentasUrbanizacion.SP_Anio] = VentasUrba.Anio;
                RegistroLista[enVentasUrbanizacion.SP_Mes] = VentasUrba.Mes;

                RegistroLista.Update();
                contexto.ExecuteQuery();

                return "Venta urbanizacion registrada: " + ZonaYEjecutivo.EjecutivoLookUp.LookupValue + " - " + VentasUrba.VentaTotal;
            }
            catch (Exception ex)
            {
                return "Error es " + ex.Message;
            }

        }

        public bool ValidarEjecutivoAnioMes(enVentasUrbanizacion VentUrb, string NombreEjecutivo, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='Ejecutivo'/>  <Value Type='Text'>" + NombreEjecutivo + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='Anio'/>  <Value Type='Number'>" + VentUrb.Anio + @"</Value> </Eq>
                                    <Eq> @<FieldRef Name='Mes'/>  <Value Type='Number'>" + VentUrb.Mes + @"</Value> </Eq>
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        public enVentasUrbanizacion RecuperarZona_y_Ejecutivo(string CodEjecutivo, SP.ClientContext contexto)
        {
            enVentasUrbanizacion Zona_Ejecutivo = new enVentasUrbanizacion();

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>      
                          <Eq> @<FieldRef Name='CodigoAsesor'/>  <Value Type='Text'>" + CodEjecutivo + @"</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = contexto.Web.Lists.GetByTitle(enEjecutivosUrbanizacion.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();

            foreach (var item in registros)
            {
                if (item["Ejecutivo"] != null)
                {
                    Zona_Ejecutivo.EjecutivoLookUp = (SP.FieldUserValue)item["Ejecutivo"];
                }
                if (item["Zona"] != null)
                {
                    Zona_Ejecutivo.ZonaLookUp = ((SP.FieldLookupValue)item["Zona"]);
                }
            }

            return Zona_Ejecutivo;


        }

        public string ActualizarEjecitivoAnioMes(enVentasUrbanizacion Venturb , string NombreEjecutivo, SP.ClientContext contexto)
        {
            string Rpta = string.Empty;
            try
            {
                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='"+ enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + NombreEjecutivo + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='"+ enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + Venturb.Anio + @"</Value> </Eq>
                                    <Eq> @<FieldRef Name='"+ enVentasIndustrial.SP_Mes+"'/>  <Value Type='Number'>" + Venturb.Mes + @"</Value> </Eq>
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";


                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    decimal total = Convert.ToDecimal(item[enVentasUrbanizacion.SP_VentaTotal], new CultureInfo("en-ES")) + Convert.ToDecimal(Venturb.VentaTotal, new CultureInfo("en-ES"));
                    item[enVentasUrbanizacion.SP_VentaTotal] = total;

                    item.Update();
                    contexto.ExecuteQuery();

                    Rpta = "Venta urbanizacion: " + NombreEjecutivo + " actualizada - Venta Total: " + total;
                }
                
            }
            catch (Exception ex )
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }





        public bool ValidarExisteVentaUrbanizacion(enVentasUrbanizacion VentUrb, SP.ClientContext contexto)
        {
           // SP.ClientContext contexto = Contexto;

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                         <And>
                             <Eq>
                                <FieldRef Name='Ejecutivo' />
                                <Value Type='Text'>" + VentUrb.EjecutivoLookUp.LookupValue + @"</Value>
                             </Eq>
                             <And>
                                <Eq>
                                   <FieldRef Name='Anio' />
                                   <Value Type='Number'>" + VentUrb.Anio + @"</Value>
                                </Eq>
                                <And>
                                   <Eq>
                                      <FieldRef Name='Mes' />
                                      <Value Type='Number'>" + VentUrb.Mes + @"</Value>
                                   </Eq>
                                   <Eq>
                                      <FieldRef Name='Proyecto' LookupId='TRUE' />
                                      <Value Type='Lookup'>" + VentUrb.IdProyecto + @"</Value>
                                   </Eq>
                                </And>
                             </And>
                          </And>
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            if (registros.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }



        }
       
        public int ValidarExisteProyecto(enVentasUrbanizacion VentUrb, SP.ClientContext contexto)
        {
            int Codigo = 0;
            try
            {
               // SP.ClientContext contexto = Contexto;

                SP.CamlQuery query = new SP.CamlQuery();
                
                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                      
                            <Eq> @<FieldRef Name='Codigo'/>  <Value Type='Text'>" + VentUrb.CodigoProyecto + @"</Value> </Eq>
              
                       </Where>
                      </Query>
                     </View>";

                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.Proyectos);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();


                if (registros.Count > 0)
                {
                    foreach (var item in registros)
                    {
                        return Codigo = item.Id;
                    }
                }
                else
                {
                    Codigo = 0;
                }
            }
            catch (Exception ex)
            {

                return Codigo;
            }
           

            return Codigo;

        }

        public int  InsertarProyecto(enVentasUrbanizacion VentasUrba, SP.ClientContext contexto)
        {

           // SP.ClientContext Contexto = Contexto;

            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.Proyectos);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                

                RegistroLista[enVentasUrbanizacion.SP_Title] = VentasUrba.Proyecto;
                RegistroLista[enVentasUrbanizacion.SP_CodigoProyecto] = VentasUrba.CodigoProyecto;
              

                RegistroLista.Update();
                contexto.ExecuteQuery();
                return RegistroLista.Id;
                //return "Venta urbanizacion registrada: " + ZonaYEjecutivo.EjecutivoLookUp.LookupValue + " - " + VentasUrba.VentaTotal;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public string InsertarVentaUrbanizacion(enVentasUrbanizacion VentasUrba, SP.ClientContext contexto)
        {

           // SP.ClientContext Contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();

            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                //enVentasUrbanizacion ZonaYEjecutivo = RecuperarZona_y_Ejecutivo(VentasUrba.CodigoAsesor);

                RegistroLista[enVentasUrbanizacion.SP_Zona] = VentasUrba.ZonaLookUp;
                RegistroLista[enVentasUrbanizacion.SP_Ejecutivo] = VentasUrba.EjecutivoLookUp;
                RegistroLista[enVentasUrbanizacion.SP_VentaTotal] = VentasUrba.Venta;
                RegistroLista[enVentasUrbanizacion.SP_Anio] = VentasUrba.Anio;
                RegistroLista[enVentasUrbanizacion.SP_Mes] = VentasUrba.Mes;
                RegistroLista[enVentasUrbanizacion.SP_IdProyecto] = VentasUrba.IdProyecto;
                RegistroLista[enVentasUrbanizacion.SP_Supervisor] = VentasUrba.SupervisorLookUp;
                RegistroLista[enVentasUrbanizacion.SP_Rol] = VentasUrba.RolLookUp;
                RegistroLista[enVentasUrbanizacion.SP_Anio_Reso] = VentasUrba.AnioResolucion;
                RegistroLista[enVentasUrbanizacion.SP_Mes_Reso] = VentasUrba.MesResolucion;
                RegistroLista[enVentasUrbanizacion.SP_Anio_Arras] = VentasUrba.AnioArras;
                RegistroLista[enVentasUrbanizacion.SP_Mes_Arras] = VentasUrba.MesArras;


                RegistroLista.Update();
                contexto.ExecuteQuery();

                return "Venta urbanizacion registrada: " + VentasUrba.EjecutivoLookUp.LookupValue + " - " + VentasUrba.Venta;
            }
            catch (Exception ex)
            {
                return "Error es " + ex.Message;
            }

        }


        public string ActualizarVentaUrbanización(enVentasUrbanizacion Venturb, SP.ClientContext contexto)
        {
            string Rpta = string.Empty;
            try
            {
                // SP.ClientContext contexto = Contexto;

                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                         <And>
                             <Eq>
                                <FieldRef Name='Ejecutivo' />
                                <Value Type='Text'>" + Venturb.EjecutivoLookUp.LookupValue + @"</Value>
                             </Eq>
                             <And>
                                <Eq>
                                   <FieldRef Name='Anio' />
                                   <Value Type='Number'>" + Venturb.Anio + @"</Value>
                                </Eq>
                                <And>
                                   <Eq>
                                      <FieldRef Name='Mes' />
                                      <Value Type='Number'>" + Venturb.Mes + @"</Value>
                                   </Eq>
                                   <Eq>
                                      <FieldRef Name='Proyecto' LookupId='TRUE' />
                                      <Value Type='Lookup'>" + Venturb.IdProyecto + @"</Value>
                                   </Eq>
                                </And>
                             </And>
                          </And>
                       </Where>
                      </Query>
                     </View>";


                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasUrbanizacion.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    decimal total = Convert.ToDecimal(item[enVentasUrbanizacion.SP_VentaTotal], new CultureInfo("en-ES")) + Convert.ToDecimal(Venturb.Venta, new CultureInfo("en-ES"));
                   // decimal VentaTotal = total + Venturb.Venta;
                    item[enVentasUrbanizacion.SP_VentaTotal] = total;

                    item.Update();
                    contexto.ExecuteQuery();

                    Rpta = "Venta urbanizacion: " + Venturb.EjecutivoLookUp.LookupValue + " actualizada - Venta Total: " + total;
                }

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }
    }



}
