using InsertCsvCentanario.Entidades;
using InsertCsvCentenario.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using util = InsertCsvCentanario.Entidades.Object;

namespace Carga.VentaUrbanizaciones
{
    class Program
    {
        static public List<enEjecutivosUrbanizacion> lstejecutivosUrbanizacions = new List<enEjecutivosUrbanizacion>();
        static public List<enProyectos> lstProyectos = new List<enProyectos>();
        static public List<enVentasIndustrial> lstejecutivosventaIndustrial = new List<enVentasIndustrial>();

        static string Enviados = ConfigurationManager.AppSettings["Enviados"];
        static string Procesados = ConfigurationManager.AppSettings["Procesados"];
        // static string RutaCarpetaDestino = ConfigurationManager.AppSettings["CarpetaDestino"];
        static void Main(string[] args)
        {

            SP.ClientContext contexto = new cnxSharePoint().ObtenerContextoDesarrolloUrbano();
            string RutaCarpeta = ConfigurationManager.AppSettings["RutaCarpeta"];
            FiltrarRegistrosPorTipo(RutaCarpeta, Utils.URBANIZACION, contexto);
            FiltrarRegistrosPorTipo(RutaCarpeta, Utils.INDUSTRIAL, contexto);
            //Console.ReadKey();
        }

        private static void FiltrarRegistrosPorTipo(string RutaCarpeta, string TipoArchivo, SP.ClientContext contexto)
        {
            string[] RutaArchivos;

            if (TipoArchivo.Equals(Utils.URBANIZACION))
            {
                enVentasUrbanizacion VentaUrba = new enVentasUrbanizacion();
                List<enVentasUrbanizacion> lstVenta = new List<enVentasUrbanizacion>();

                ListarEjecutivo(contexto);
                ListarProyectos(contexto);
                string RutaURBANIZACION = Path.Combine(RutaCarpeta, Utils.URBANIZACION);
                RutaArchivos = Directory.GetFiles(RutaURBANIZACION + Enviados, "*.txt");
                daVentasUrbanizacion objdaVentasUrbanizacion = new daVentasUrbanizacion();
                foreach (string RutaArchivo in RutaArchivos)
                {
                    try
                    {
                        lstVenta = new List<enVentasUrbanizacion>();
                        string[] registros = new Utils().leerCsv(RutaArchivo);


                        foreach (var item in registros)
                        {
                            string[] columnas = item.Split('|');
                            if (ValidarCodigoEjecutivo(columnas[1].Trim()))
                            {
                                VentaUrba = new enVentasUrbanizacion();
                                VentaUrba.CodigoAsesor = columnas[1];
                                VentaUrba.Venta = Convert.ToDecimal(columnas[3], new CultureInfo("en-ES"));
                                //Actualmente el txt dado que no retorna el mes y año de la venta. Para procesar la data se fijo el periodo 2020-11
                                VentaUrba.Anio = columnas[4];//"2020";
                                VentaUrba.Mes = columnas[5];//"11";
                                VentaUrba.Zona = columnas[7];
                                //validamos si existe el codigo proyecto

                                VentaUrba.CodigoProyecto = columnas[9];
                                VentaUrba.Proyecto = columnas[10];
                                VentaUrba.AnioResolucion = columnas[11];
                                VentaUrba.MesResolucion = columnas[12];
                                VentaUrba.AnioArras = columnas[13];
                                VentaUrba.MesArras = columnas[14];
                                VentaUrba.NroFinanciamiento = columnas[15];
                                VentaUrba.SupervisorAsignado = columnas[16];
                                VentaUrba.SupervisorVenta = columnas[17];
                                if (lstProyectos.Exists(x => x.Codigo == VentaUrba.CodigoProyecto))
                                {
                                    VentaUrba.IdProyecto = lstProyectos.Find(x => x.Codigo == VentaUrba.CodigoProyecto).Id;
                                }
                                else
                                {
                                    VentaUrba.IdProyecto = objdaVentasUrbanizacion.InsertarProyecto(VentaUrba, contexto);
                                    lstProyectos.Add(new enProyectos() { Id = VentaUrba.IdProyecto, Codigo = VentaUrba.CodigoProyecto, Nombre = VentaUrba.Proyecto });
                                }

                                if (lstejecutivosUrbanizacions.Where(a => a.CodigoAsesor == VentaUrba.CodigoAsesor).FirstOrDefault() != null)
                                {
                                    var Ejecutivo = lstejecutivosUrbanizacions.Where(a => a.CodigoAsesor == VentaUrba.CodigoAsesor).FirstOrDefault();
                                    VentaUrba.EjecutivoLookUp = Ejecutivo.EjecutivoLookUp;
                                    VentaUrba.ZonaLookUp = Ejecutivo.ZonaLookUp;
                                    VentaUrba.SupervisorLookUp = Ejecutivo.SupervisorLookUp;
                                    VentaUrba.RolLookUp = Ejecutivo.RolLookUp;


                                    var venta = lstVenta.Where(a => a.CodigoAsesor == VentaUrba.CodigoAsesor && a.Mes == VentaUrba.Mes && a.Anio == VentaUrba.Anio
                                    && a.CodigoProyecto == VentaUrba.CodigoProyecto && a.NroFinanciamiento == VentaUrba.NroFinanciamiento).ToList();
                                    if (venta.Count == 0)
                                    {
                                        lstVenta.Add(VentaUrba);
                                    }
                                    else
                                    {
                                        decimal Ventatotal = venta[0].Venta;
                                        Ventatotal = Ventatotal + VentaUrba.Venta;
                                        lstVenta.First(a => a.CodigoAsesor == VentaUrba.CodigoAsesor && a.Mes == VentaUrba.Mes && a.Anio == VentaUrba.Anio
                                        && a.CodigoProyecto == VentaUrba.CodigoProyecto && a.NroFinanciamiento == VentaUrba.NroFinanciamiento).Venta = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));
                                    }
                                }



                            }

                        }
                        var lst = lstVenta.GroupBy(x => new { x.Anio, x.Mes }).Select(x => new enVentasUrbanizacion { Mes = x.Key.Mes, Anio = x.Key.Anio }).ToList();
                        Console.WriteLine("Cantidad Agrupados a eliminar: " + lst.Count);
                        if (lst.Count > 0)
                        {
                            Console.WriteLine("Eliminando registros...");
                            new daVentasUrbanizacion().EliminarAnioMesUrbanizacion(lst, contexto);
                        }
                        
                        

                        ProcesarDataIndustrial_O_Urbanizacion(lstVenta, TipoArchivo, contexto);

                        EliminacionArchivos(RutaArchivo, RutaURBANIZACION);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                    }

                }//Fin Lectura de Archivos

            }
            if (TipoArchivo.Equals(Utils.INDUSTRIAL))
            {
                enVentasIndustrial ventasIndustrial = new enVentasIndustrial();
                List<enVentasIndustrial> lstVentaIndustrial = new List<enVentasIndustrial>();
                ListarEjecutivoIndustrial(contexto);
                List<enParametroLote> lstParametrosLote= new daParametroLote().ObtenerParametrosLote(contexto);
                enParametroLote parametroLote = new enParametroLote();
                string RutaINDUSTRIAL = Path.Combine(RutaCarpeta, Utils.INDUSTRIAL);
                RutaArchivos = Directory.GetFiles(RutaINDUSTRIAL + Enviados, "*.txt");
                foreach (string RutaArchivo in RutaArchivos)
                {
                    lstVentaIndustrial = new List<enVentasIndustrial>();
                    try
                    {
                        string[] registros = new Utils().leerCsv(RutaArchivo);


                        foreach (var item in registros)
                        {
                            string[] columnas = item.Split('|');
                            if (ValidarCodigoEjecutivo(columnas[1].Trim()))
                            {
                                ventasIndustrial = new enVentasIndustrial();

                                ventasIndustrial.Zona = columnas[7];
                                ventasIndustrial.CodigoAsesor = columnas[1];
                                ventasIndustrial.Venta = Convert.ToDecimal(columnas[3], new CultureInfo("en-ES"));
                                ventasIndustrial.Anio = columnas[4];
                                ventasIndustrial.Mes = columnas[5];

                                ventasIndustrial.Area = Convert.ToDecimal(columnas[12]) / 10000;
                                parametroLote = lstParametrosLote.Find(x => x.AreaDesde < ventasIndustrial.Area && ventasIndustrial.Area <= x.AreaHasta);//  ObtenerParametroLote(Convert.ToDecimal(columnas[12]) / 10000);

                                ventasIndustrial.TipoVenta = parametroLote.TipoVenta;//columnas[5];
                                ventasIndustrial.TipoLote = parametroLote.TipoLote;// columnas[6];
                                ventasIndustrial.AnioResolucion = columnas[13];
                                ventasIndustrial.MesResolucion = columnas[14];
                                ventasIndustrial.AnioArras = columnas[15];
                                ventasIndustrial.MesArras = columnas[16];
                                ventasIndustrial.NroFinanciamiento = columnas[17];
                                ventasIndustrial.NumeroDeudor = columnas[18];
                                ventasIndustrial.DatosCliente = columnas[19];

                                if (lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor).FirstOrDefault() != null)
                                {
                                    if (ventasIndustrial.CodigoAsesor == ventasIndustrial.V000414)
                                    {
                                        var Ejecutivo = lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor).FirstOrDefault();
                                        var EjecutivoApoyo = lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.V000425).FirstOrDefault();
                                        ventasIndustrial.LookUpEjecutivo = Ejecutivo.LookUpEjecutivo;
                                        ventasIndustrial.LookUpEjecutivoApoyo = EjecutivoApoyo.LookUpEjecutivoApoyo;
                                        ventasIndustrial.SupervisorLookUp = EjecutivoApoyo.SupervisorLookUp;
                                        ventasIndustrial.RolLookUp = EjecutivoApoyo.RolLookUp;
                                    }
                                    else if (ventasIndustrial.CodigoAsesor == ventasIndustrial.V000425)
                                    {
                                        var Ejecutivo = lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor).FirstOrDefault();
                                        var EjecutivoApoyo = lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.V000414).FirstOrDefault();
                                        ventasIndustrial.LookUpEjecutivo = Ejecutivo.LookUpEjecutivo;
                                        ventasIndustrial.LookUpEjecutivoApoyo = EjecutivoApoyo.LookUpEjecutivo;
                                        ventasIndustrial.SupervisorLookUp = EjecutivoApoyo.SupervisorLookUp;
                                        ventasIndustrial.RolLookUp = EjecutivoApoyo.RolLookUp;

                                    }
                                    var EjecutivoInd = lstejecutivosventaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor).FirstOrDefault();
                                    ventasIndustrial.LookUpEjecutivo = EjecutivoInd.LookUpEjecutivo;
                                    ventasIndustrial.LookUpEjecutivoApoyo = EjecutivoInd.LookUpEjecutivo;
                                    ventasIndustrial.SupervisorLookUp = EjecutivoInd.SupervisorLookUp;
                                    ventasIndustrial.RolLookUp = EjecutivoInd.RolLookUp;



                                    var venta = lstVentaIndustrial.Where(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor && a.Mes == ventasIndustrial.Mes && a.Anio == ventasIndustrial.Anio && a.TipoVenta == ventasIndustrial.TipoVenta && a.TipoLote == ventasIndustrial.TipoLote && a.NroFinanciamiento == ventasIndustrial.NroFinanciamiento).ToList();
                                    if (venta.Count == 0)
                                    {
                                        lstVentaIndustrial.Add(ventasIndustrial);
                                    }
                                    else
                                    {
                                        decimal Ventatotal = venta[0].Venta;
                                        Ventatotal = Ventatotal + ventasIndustrial.Venta;
                                        lstVentaIndustrial.First(a => a.CodigoAsesor == ventasIndustrial.CodigoAsesor && a.Mes == ventasIndustrial.Mes && a.Anio == ventasIndustrial.Anio && a.TipoVenta == ventasIndustrial.TipoVenta && a.TipoLote == ventasIndustrial.TipoLote && a.NroFinanciamiento == ventasIndustrial.NroFinanciamiento).Venta = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));
                                    }
                                }



                            }

                        }

                        var lst = lstVentaIndustrial.GroupBy(x => new { x.Anio, x.Mes }).Select(x => new enVentasIndustrial { Mes = x.Key.Mes, Anio = x.Key.Anio }).ToList();
                        Console.WriteLine("Cantidad Agrupados a eliminar: " + lst.Count);
                        if(lst.Count > 0)
                        {
                            Console.WriteLine("Eliminando registros...");
                            new daVentaIndusrial().EliminarAnioMesIndustrial(lst, contexto);
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                    }
                    ProcesarDataVentaIndustrial(lstVentaIndustrial, TipoArchivo, contexto);
                    EliminacionArchivos(RutaArchivo, RutaINDUSTRIAL);
                }//Fin Lectura de Archivo


                


            }


        }


        #region Carga Data a SP
        public static void ProcesarDataIndustrial_O_Urbanizacion(List<enVentasUrbanizacion> lstVenta, string TipoArchivo, SP.ClientContext contexto)
        {
            try
            {

                if (TipoArchivo.Equals(Utils.URBANIZACION))
                {
                    var ventaGroups = util.ChunkBy(lstVenta, 100);
                    ventaGroups.ForEach(lstTemp =>
                    {
                        var lst = Task.Run(() =>
                        {
                            foreach (var VentaUrba in lstTemp)
                            {
                                var Item = (enVentasUrbanizacion)VentaUrba;
                                bool ValidarPeriodoVentaExistente = new daVentasUrbanizacion().ValidarExisteVentaUrbanizacion(Item, contexto);

                                if (ValidarPeriodoVentaExistente)
                                {
                                    Console.WriteLine(new daVentasUrbanizacion().InsertarVentaUrbanizacion(Item, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                                }
                                else
                                {
                                    Console.WriteLine(new daVentasUrbanizacion().ReIngresarVentaUrbanización(Item, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                                }

                            }
                        });
                        Task.WaitAll(lst);

                    });

                    


                }



            }
            catch (Exception ex)
            {


            }


        }

        public static void ProcesarDataVentaIndustrial(List<enVentasIndustrial> lstVenta, string TipoArchivo, SP.ClientContext contexto)
        {
            try
            {
                if (TipoArchivo.Equals(Utils.INDUSTRIAL))
                {
                    var ventaGroups = util.ChunkBy(lstVenta, 100);
                    ventaGroups.ForEach(lstTemp =>
                    {
                        var lst = Task.Run(() =>
                        {
                            foreach (var VentaUrba in lstTemp)
                            {
                                var Item = (enVentasIndustrial)VentaUrba;
                                bool ValidarPeriodoVentaExistente = new daVentaIndusrial().ValidarExisteEjecutivoIndustrial(Item, contexto);

                                if (ValidarPeriodoVentaExistente)
                                {
                                    Console.WriteLine(new daVentaIndusrial().InsertarRegistro(Item, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                                }
                                else
                                {
                                    Console.WriteLine(new daVentaIndusrial().ReIngresarEjecitivoAnioMes(Item, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                                }
                            }
                        });
                        Task.WaitAll(lst);

                    });
                    
                }
            }
            catch (Exception ex)
            {


            }


        }
        public static List<enEjecutivosUrbanizacion> ListarEjecutivo(SP.ClientContext contexto)
        {
            lstejecutivosUrbanizacions = new daEjecutivoUrbanizacion().ListarEjecutivo(contexto);
            return lstejecutivosUrbanizacions;
        }

        public static List<enProyectos> ListarProyectos(SP.ClientContext contexto)
        {
            lstProyectos = new daPoryectos().ListarProyectos(contexto);
            return lstProyectos;
        }
        #endregion

        #region Industrial
        public static List<enVentasIndustrial> ListarEjecutivoIndustrial(SP.ClientContext contexto)
        {
            lstejecutivosventaIndustrial = new daVentaIndusrial().ListarEjecutivoIndustrial(contexto);
            return lstejecutivosventaIndustrial;
        }

        public static enParametroLote ObtenerParametroLote(decimal AreaTerreno, SP.ClientContext contexto)
        {
            enParametroLote parametroLote = new enParametroLote();
            parametroLote = new daParametroLote().ObtenerParametroLote(AreaTerreno, contexto);
            return parametroLote;
        }

        #endregion
        public static bool ValidarCodigoEjecutivo(string CodigoEjecutivo)
        {
            if (string.IsNullOrEmpty(CodigoEjecutivo))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static void EliminacionArchivos(string RutaArchivo, string RutaFinal)
        {
            string RutaProcesado = RutaFinal + Procesados;
            if (Directory.Exists(RutaProcesado))
            {
                File.Move(RutaArchivo, RutaProcesado + @"\" + Path.GetFileName(RutaArchivo));
                Console.WriteLine("Archivo: " + Path.GetFileName(RutaArchivo) + " procesado correctamente \n");
            }
            else
            {
                Directory.CreateDirectory(RutaProcesado);
                File.Move(RutaArchivo, RutaProcesado + @"\" + Path.GetFileName(RutaArchivo));
                Console.WriteLine("Archivo: " + Path.GetFileName(RutaArchivo) + " procesado correctamente \n");
            }
        }
    }
}
