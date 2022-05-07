using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;
using System.Globalization;
using util = InsertCsvCentanario.Entidades.Object;

namespace InsertCsvCentenario.AccesoDatos
{
    public class daVentaIndusrial
    {
        public string InsertarRegistro(enVentasIndustrial VenIndus, SP.ClientContext contexto)
        {
            try
            {
                SP.List ObjetoLista = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
                SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                SP.ListItem RegistroLista = ObjetoLista.AddItem(InformacionDeLaLista);

                // enVentasIndustrial EjecutivoLookUp = RecuperarEjecuctivoApoyo_y_Ejecutivo(VenIndus.CodigoAsesor);

                RegistroLista[enVentasIndustrial.SP_Zona] = VenIndus.Zona;
                RegistroLista[enVentasIndustrial.SP_Ejecutivo] = VenIndus.LookUpEjecutivo;
                RegistroLista[enVentasIndustrial.SP_VentaTotal] = VenIndus.Venta;
                RegistroLista[enVentasIndustrial.SP_Anio] = VenIndus.Anio;
                RegistroLista[enVentasIndustrial.SP_Mes] = VenIndus.Mes;
                RegistroLista[enVentasIndustrial.SP_TipoVenta] = VenIndus.TipoVenta;
                RegistroLista[enVentasIndustrial.SP_EjecutivoApoyo] = VenIndus.LookUpEjecutivoApoyo;
                RegistroLista[enVentasIndustrial.SP_Supervisor] = VenIndus.SupervisorLookUp;
                RegistroLista[enVentasIndustrial.SP_TipoLote] = VenIndus.TipoLote;
                RegistroLista[enVentasIndustrial.SP_Area] = VenIndus.Area;
                RegistroLista[enVentasIndustrial.SP_Rol] = VenIndus.RolLookUp;
                RegistroLista[enVentasIndustrial.SP_AnioResolucion] = VenIndus.AnioResolucion;
                RegistroLista[enVentasIndustrial.SP_MesResolucion] = VenIndus.MesResolucion;
                RegistroLista[enVentasIndustrial.SP_Anio_Arras] = VenIndus.AnioArras;
                RegistroLista[enVentasIndustrial.SP_Mes_Arras] = VenIndus.MesArras;
                RegistroLista[enVentasIndustrial.SP_NroFinanciamiento] = VenIndus.NroFinanciamiento;
                RegistroLista[enVentasIndustrial.SP_SupervisorAsignado] = VenIndus.SupervisorAsignado;
                RegistroLista[enVentasIndustrial.SP_SupervisorVenta] = VenIndus.SupervisorVenta;
                RegistroLista[enVentasIndustrial.SP_NumeroDeudor] = VenIndus.NumeroDeudor;
                RegistroLista[enVentasIndustrial.SP_DatosCliente] = VenIndus.DatosCliente;

                RegistroLista.Update();
                contexto.ExecuteQuery();

                return "Cobranza registrada: " + VenIndus.Venta;
            }
            catch (Exception ex)
            {

                return "Error es " + ex.Message;
            }

        }

        public bool ValidarEjecutivoAnioMes(enVentasIndustrial VenIndus, string NombreEjecutivo, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + NombreEjecutivo + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VenIndus.Anio + @"</Value> </Eq>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VenIndus.Mes + @"</Value> </Eq>
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


        public enVentasIndustrial RecuperarEjecuctivoApoyo_y_Ejecutivo(string CodEjecutivo, SP.ClientContext contexto)
        {
            enVentasIndustrial EjecutivoIndustrial = new enVentasIndustrial();

            SP.Web web = contexto.Web;

            //contexto.Load(web);
            //contexto.ExecuteQuery();

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>      
                          <Eq> @<FieldRef Name='Rol'/>  <Value Type='Text'>Asesor Industrial</Value> </Eq>
                       </Where>
                      </Query>
                     </View>";


            SP.List oList = web.Lists.GetByTitle("Ejecutivos Industrial");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            foreach (var item in registros)
            {
                if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null && item["CodigoAsesor"].ToString() == CodEjecutivo)
                {
                    EjecutivoIndustrial.LookUpEjecutivo = (SP.FieldUserValue)item["Ejecutivo"];
                }
                if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null && item["CodigoAsesor"].ToString() != CodEjecutivo)
                {
                    EjecutivoIndustrial.LookUpEjecutivoApoyo = (SP.FieldUserValue)item["Ejecutivo"];
                }

            }

            return EjecutivoIndustrial;


        }

        public string ActualizarEjecitivoAnioMes(enVentasIndustrial VentasIndu, SP.ClientContext contexto)
        {
            string Rpta = string.Empty;
            try
            {
                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VentasIndu.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VentasIndu.Anio + @"</Value> </Eq>
                                    <And>
                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VentasIndu.Mes + @"</Value> </Eq>
                                            <And>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoLote + "'/>  <Value Type='Text'>" + VentasIndu.TipoLote + @"</Value> </Eq>
                                                    <And>
                                                       <Eq>
                                                          <FieldRef Name='NumeroFinanciamiento' />
                                                          <Value Type='Number'>" + VentasIndu.NroFinanciamiento + @"</Value>
                                                       </Eq>
                                                       <And>
                                                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoVenta + "'/>  <Value Type='Text'>" + VentasIndu.TipoVenta + @"</Value> </Eq>
                                                        <Eq>
                                                            <FieldRef Name='Activo' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                    </And>
                                                    </And>
                                                    
                                           </And>
                                   </And> 
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";


                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    decimal total = Convert.ToDecimal(item[enVentasUrbanizacion.SP_VentaTotal], new CultureInfo("en-ES")) + Convert.ToDecimal(VentasIndu.Venta, new CultureInfo("en-ES"));
                    //decimal VentaTotal = total + VentasIndu.Venta;
                    item[enVentasUrbanizacion.SP_VentaTotal] = total;

                    item.Update();
                    contexto.ExecuteQuery();

                    Rpta = "Venta industrial: " + VentasIndu.LookUpEjecutivo.LookupValue + " actualizada - Venta Total: " + total;
                }

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }

        public string ReIngresarEjecitivoAnioMes(enVentasIndustrial VentasIndu, SP.ClientContext contexto)
        {
            string Rpta = string.Empty;
            try
            {
                SP.CamlQuery query = new SP.CamlQuery();

                query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VentasIndu.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VentasIndu.Anio + @"</Value> </Eq>
                                    <And>
                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VentasIndu.Mes + @"</Value> </Eq>
                                            <And>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoLote + "'/>  <Value Type='Text'>" + VentasIndu.TipoLote + @"</Value> </Eq>
                                                    <And>
                                                       <Eq>
                                                          <FieldRef Name='NumeroFinanciamiento' />
                                                          <Value Type='Number'>" + VentasIndu.NroFinanciamiento + @"</Value>
                                                       </Eq>
                                                       <And>
                                                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoVenta + "'/>  <Value Type='Text'>" + VentasIndu.TipoVenta + @"</Value> </Eq>
                                                        <Eq>
                                                            <FieldRef Name='Activo' />
                                                            <Value Type='Boolean'>1</Value>
                                                        </Eq>
                                                    </And>
                                                    </And>
                                                    
                                           </And>
                                   </And> 
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";


                SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
                SP.ListItemCollection registros = oList.GetItems(query);

                contexto.Load(registros);
                contexto.ExecuteQuery();

                if (registros.Count > 0)
                {
                    SP.ListItem item = registros.FirstOrDefault();
                    item[enVentasIndustrial.SP_Activo] = false;
                    item.Update();

                    SP.ListItemCreationInformation InformacionDeLaLista = new SP.ListItemCreationInformation();
                    SP.ListItem RegistroLista = oList.AddItem(InformacionDeLaLista);
                    RegistroLista[enVentasIndustrial.SP_Zona] = VentasIndu.Zona;
                    RegistroLista[enVentasIndustrial.SP_Ejecutivo] = VentasIndu.LookUpEjecutivo;
                    RegistroLista[enVentasIndustrial.SP_VentaTotal] = VentasIndu.Venta;
                    RegistroLista[enVentasIndustrial.SP_Anio] = VentasIndu.Anio;
                    RegistroLista[enVentasIndustrial.SP_Mes] = VentasIndu.Mes;
                    RegistroLista[enVentasIndustrial.SP_TipoVenta] = VentasIndu.TipoVenta;
                    RegistroLista[enVentasIndustrial.SP_EjecutivoApoyo] = VentasIndu.LookUpEjecutivoApoyo;
                    RegistroLista[enVentasIndustrial.SP_Supervisor] = VentasIndu.SupervisorLookUp;
                    RegistroLista[enVentasIndustrial.SP_TipoLote] = VentasIndu.TipoLote;
                    RegistroLista[enVentasIndustrial.SP_Area] = VentasIndu.Area;
                    RegistroLista[enVentasIndustrial.SP_Rol] = VentasIndu.RolLookUp;
                    RegistroLista[enVentasIndustrial.SP_AnioResolucion] = VentasIndu.AnioResolucion;
                    RegistroLista[enVentasIndustrial.SP_MesResolucion] = VentasIndu.MesResolucion;
                    RegistroLista[enVentasIndustrial.SP_Anio_Arras] = VentasIndu.AnioArras;
                    RegistroLista[enVentasIndustrial.SP_Mes_Arras] = VentasIndu.MesArras;
                    RegistroLista[enVentasIndustrial.SP_NroFinanciamiento] = VentasIndu.NroFinanciamiento;
                    RegistroLista[enVentasIndustrial.SP_SupervisorAsignado] = VentasIndu.SupervisorAsignado;
                    RegistroLista[enVentasIndustrial.SP_SupervisorVenta] = VentasIndu.SupervisorVenta;
                    RegistroLista[enVentasIndustrial.SP_NumeroDeudor] = VentasIndu.NumeroDeudor;
                    RegistroLista[enVentasIndustrial.SP_DatosCliente] = VentasIndu.DatosCliente;
                    RegistroLista[enVentasIndustrial.SP_Activo] = true;

                    RegistroLista.Update();
                    contexto.ExecuteQuery();

                    Rpta = "Venta industrial: " + VentasIndu.LookUpEjecutivo.LookupValue + " eliminada y registrada";
                }

            }
            catch (Exception ex)
            {
                Rpta = "Error fue :" + ex.Message;
            }

            return Rpta;

        }




        #region Version2

        public SP.ListItemCollection GetAllIndustrial(enVentasIndustrial VenIndus, SP.ClientContext contexto)
        {
            // SP.ClientContext contexto = Contexto;

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View Scope=='RecursiveAll'>
                       <Query>
                        <Where> 
                            <And>
                                <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VenIndus.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                                <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VenIndus.Anio + @"</Value> </Eq>
                                    <And>
                                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VenIndus.Mes + @"</Value> </Eq>
                                        <And>
                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoLote + "'/>  <Value Type='Text'>" + VenIndus.TipoLote + @"</Value> </Eq>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='NumeroFinanciamiento' />
                                                    <Value Type='Number'>" + VenIndus.NroFinanciamiento + @"</Value>
                                                </Eq>
                                                <And>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoVenta + "'/>  <Value Type='Text'>" + VenIndus.TipoVenta + @"</Value> </Eq>
                                                    <Eq>
                                                        <FieldRef Name='Activo' />
                                                        <Value Type='Boolean'>1</Value>
                                                    </Eq>
                                                </And>
                                            </And>
                                        </And>
                                   </And> 
                                </And> 
                            </And>
                       </Where>
                       <RowLimit>5000</RowLimit>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();
            return registros;
        }


        public bool ValidarExisteEjecutivoIndustrial(enVentasIndustrial VenIndus, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
                       <Query>
                        <Where>           
                    <And>
                        <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Ejecutivo + "'/>  <Value Type='Text'>" + VenIndus.LookUpEjecutivo.LookupValue + @"</Value> </Eq>
                           <And>
                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Anio + "'/>  <Value Type='Number'>" + VenIndus.Anio + @"</Value> </Eq>
                                    <And>
                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_Mes + "'/>  <Value Type='Number'>" + VenIndus.Mes + @"</Value> </Eq>
                                            <And>
                                                    <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoLote + "'/>  <Value Type='Text'>" + VenIndus.TipoLote + @"</Value> </Eq>
                                                    <And>
                                                       <Eq>
                                                          <FieldRef Name='NumeroFinanciamiento' />
                                                          <Value Type='Number'>" + VenIndus.NroFinanciamiento + @"</Value>
                                                       </Eq>
                                                       <And>
                                                            <Eq> @<FieldRef Name='" + enVentasIndustrial.SP_TipoVenta + "'/>  <Value Type='Text'>" + VenIndus.TipoVenta + @"</Value> </Eq>
                                                            <Eq>
                                                                <FieldRef Name='Activo' />
                                                                <Value Type='Boolean'>1</Value>
                                                            </Eq>
                                                        </And>
                                                    </And>
                                                    
                                           </And>
                                   </And> 
                           </And> 
                    </And> 
                       </Where>
                      </Query>
                     </View>";

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
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

        public List<enVentasIndustrial> ListarEjecutivoIndustrial(SP.ClientContext contexto)
        {
            enVentasIndustrial EjecutivoIndustrial = new enVentasIndustrial();
            List<enVentasIndustrial> lstEjecutivoIndustrial = new List<enVentasIndustrial>();
            //SP.ClientContext contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();
            SP.Web web = contexto.Web;

            contexto.Load(web);
            contexto.ExecuteQuery();

            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = @"<View>
            <Query>
            </Query>
            </View>";

            SP.List oList = web.Lists.GetByTitle("Ejecutivos Industrial");
            SP.ListItemCollection registros = oList.GetItems(query);

            contexto.Load(registros);
            contexto.ExecuteQuery();


            foreach (var item in registros)
            {
                EjecutivoIndustrial = new enVentasIndustrial();
                if (item["CodigoAsesor"] != null)
                {
                    EjecutivoIndustrial.CodigoAsesor = item["CodigoAsesor"].ToString();
                    if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.LookUpEjecutivo = (SP.FieldUserValue)item["Ejecutivo"];
                    }
                    if (item["Ejecutivo"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.LookUpEjecutivoApoyo = (SP.FieldUserValue)item["Ejecutivo"];
                    }
                    if (item["Supervisor"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.SupervisorLookUp = (SP.FieldUserValue)item["Supervisor"];
                    }
                    if (item["Rol"] != null && item["CodigoAsesor"] != null)
                    {
                        EjecutivoIndustrial.RolLookUp = (SP.FieldLookupValue)item["Rol"];
                    }
                    lstEjecutivoIndustrial.Add(EjecutivoIndustrial);
                }

            }

            return lstEjecutivoIndustrial;
        }
        public string EliminarAnioMesIndustrial(List<enVentasIndustrial> VentInd, SP.ClientContext contexto)
        {
            SP.CamlQuery query = new SP.CamlQuery();

            query.ViewXml = QueryGetbyAnioMes(VentInd);

            SP.List oList = contexto.Web.Lists.GetByTitle(enVentasIndustrial.NombreListaSP);
            SP.ListItemCollection registros = oList.GetItems(query);
            contexto.Load(registros);
            contexto.ExecuteQuery();

            var lstRe = registros.ToList();

            var ventaGroups = util.ChunkBy(lstRe, 100);
            ventaGroups.ForEach(lstRegistro =>
            {
                var lst = Task.Run(() =>
                {
                    foreach (SP.ListItem item in lstRegistro)
                    {
                        SP.ListItem itemTemp = item;
                        itemTemp[enVentasIndustrial.SP_Activo] = false;
                        itemTemp.Update();
                    }
                    contexto.ExecuteQuery();
                });
                Task.WaitAll(lst);

            });

            return "Registros inhabilitados: " + registros.Count;
        }

        public static string QueryGetbyAnioMes(List<enVentasIndustrial> VentInd)
        {
            return ($@"
                    <View Scope='RecursiveAll'>
                        <Query>
                            <Where>
                                { BuildQueryGetAnioMes(VentInd) }
                            </Where>
                        </Query>
                        <RowLimit>5000</RowLimit>
                    </View>
                    ");
        }



        private static string BuildQueryGetAnioMes(List<enVentasIndustrial> proyectoIds, int? proyectoIdx = 0)
        {
            if (proyectoIds == null || proyectoIds?.Count == 0)
                return "";

            var currentId = proyectoIds[proyectoIdx.Value];

            string Eq_Id = $@"<And>
                                <Eq><FieldRef Name='Anio' /><Value Type='Number'>{currentId.Anio}</Value></Eq>
                                <Eq><FieldRef Name='Mes' /><Value Type='Number'>{currentId.Mes}</Value></Eq>
                            </And>";


            if (proyectoIdx == proyectoIds.Count - 1)
                return Eq_Id;

            var query = $"<Or>{Eq_Id} {BuildQueryGetAnioMes(proyectoIds, proyectoIdx + 1)}</Or>";

            return query;

        }


        #endregion

    }



}
