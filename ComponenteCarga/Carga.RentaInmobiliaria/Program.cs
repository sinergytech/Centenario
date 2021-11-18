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
namespace Carga.RentaInmobiliaria
{
    class Program
    {
        static string Enviados = ConfigurationManager.AppSettings["Enviados"];
        static string Procesados = ConfigurationManager.AppSettings["Procesados"];
        static void Main(string[] args)
        {
            string RutaCarpeta = ConfigurationManager.AppSettings["RutaCarpeta"];
            FiltrarRegistrosPorTipo(RutaCarpeta, Utils.MINKA_RI03);
            Console.ReadKey();
        }

        private static void FiltrarRegistrosPorTipo(string RutaCarpeta, string TipoArchivo)
        {
            string[] RutaArchivos;
            enEjecutivoMinka ejecutivoMinka = new enEjecutivoMinka();
            //enVentasIndustrial ventasIndustrial = new enVentasIndustrial();
            //List<enVentasUrbanizacion> lstVenta = new List<enVentasUrbanizacion>();
            List<enEjecutivoMinka> lstejectivoMinka = new List<enEjecutivoMinka>();

            if (TipoArchivo.Equals(Utils.MINKA_RI03))
            {
                //ListarEjecutivo();
                string RutaURBANIZACION = Path.Combine(RutaCarpeta, Utils.MINKA_RI03);
                RutaArchivos = Directory.GetFiles(RutaURBANIZACION + Enviados, "*.txt");
                foreach (string RutaArchivo in RutaArchivos)
                {
                    try
                    {
                        string[] registros = new Utils().leerCsv(RutaArchivo);


                        foreach (var item in registros)
                        {
                            string[] columnas = item.Split('|');
                            if (columnas[2].Trim()== "30502MNK")
                            {
                                //if (ValidarCodigoEjecutivo(columnas[1].Trim()))
                                //{
                                    ejecutivoMinka = new enEjecutivoMinka();
                                    ejecutivoMinka.Contrato = columnas[1];
                                    ejecutivoMinka.Monto = Convert.ToDecimal(columnas[10], new CultureInfo("en-ES"));
                                    ejecutivoMinka.Anio = columnas[7];
                                    ejecutivoMinka.Mes = columnas[8];

                                //validamos si existe el codigo proyecto


                                //ejecutivoMinka.CodigoProyecto = columnas[9];
                                //ejecutivoMinka.Proyecto = columnas[10];
                                //int IdProyecto = new daVentasUrbanizacion().ValidarExisteProyecto(VentaUrba);
                                //if (IdProyecto == 0)
                                //{
                                //    ejecutivoMinka.IdProyecto = new daVentasUrbanizacion().InsertarProyecto(VentaUrba);
                                //}
                                //else
                                //{
                                //    ejecutivoMinka.IdProyecto = IdProyecto;
                                //}
                                    //if (lstejecutivosUrbanizacions.Where(a => a.CodigoAsesor == VentaUrba.CodigoAsesor).FirstOrDefault() != null)
                                    //{
                                    //    var Ejecutivo = lstejecutivosUrbanizacions.Where(a => a.CodigoAsesor == VentaUrba.CodigoAsesor).FirstOrDefault();
                                    //    VentaUrba.EjecutivoLookUp = Ejecutivo.EjecutivoLookUp;
                                    //    VentaUrba.ZonaLookUp = Ejecutivo.ZonaLookUp;
                                    //    VentaUrba.SupervisorLookUp = Ejecutivo.SupervisorLookUp;
                                    //    VentaUrba.RolLookUp = Ejecutivo.RolLookUp;


                                        var venta = lstejectivoMinka.Where(a => a.Contrato == ejecutivoMinka.Contrato && a.Mes == ejecutivoMinka.Mes && a.Anio == ejecutivoMinka.Anio).ToList();
                                        if (venta.Count == 0)
                                        {
                                            lstejectivoMinka.Add(ejecutivoMinka);
                                        }
                                        else
                                        {
                                            decimal Ventatotal = venta[0].Monto;
                                            Ventatotal = Ventatotal + ejecutivoMinka.Monto;
                                            lstejectivoMinka.First(a => a.Contrato == ejecutivoMinka.Contrato && a.Mes == ejecutivoMinka.Mes && a.Anio == ejecutivoMinka.Anio
                                            ).Monto = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));
                                        }
                                    //}



                                //}
                            }
                            

                        }

                        ProcesarRentaInmobiliario(lstejectivoMinka, TipoArchivo);

                        //EliminacionArchivos(RutaArchivo, RutaURBANIZACION);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                    }

                }//Fin Lectura de Archivos

            }
         


        }

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

        public static void ProcesarRentaInmobiliario(List<enEjecutivoMinka> lstejectivoMinka, string TipoArchivo)
        {
            try
            {
                SP.ClientContext Contexto = new cnxSharePoint().ObtenerRentaInmobiliaria();
                if (TipoArchivo.Equals(Utils.MINKA_RI03))
                {

                    foreach (var ejecutivoMinka in lstejectivoMinka)
                    {
                        var Item = (enEjecutivoMinka)ejecutivoMinka;
                        bool ValidarPeriodoVentaExistente = new daRentaInmobiliaria().ValidarExisteContratMinka(ejecutivoMinka, Contexto);

                        if (ValidarPeriodoVentaExistente)
                        {
                            Console.WriteLine(new daRentaInmobiliaria().InsertarContratMinka(ejecutivoMinka, Contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                        }
                        else
                        {
                            Console.WriteLine(new daRentaInmobiliaria().ActualizarContratMinka(ejecutivoMinka, Contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                        }

                    }


                }



            }
            catch (Exception ex)
            {


            }


        }

    }
}
