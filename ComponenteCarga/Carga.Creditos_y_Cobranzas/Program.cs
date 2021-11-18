using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using InsertCsvCentanario.Entidades;
using InsertCsvCentenario.AccesoDatos;
using System.Configuration;
using System.IO;
using System.Globalization;
using SP = Microsoft.SharePoint.Client;

namespace Carga.Creditos_y_Cobranzas
{
    class Program
    {
        static string RutaCarpetaDestino = ConfigurationManager.AppSettings["CarpetaDestino"];


        static string Enviados = ConfigurationManager.AppSettings["Enviados"];
        static string Procesados = ConfigurationManager.AppSettings["Procesados"];

        static void Main(string[] args)
        {
            string RutaCarpeta = ConfigurationManager.AppSettings["RutaCarpeta"];
            SP.ClientContext contexto = new cnxSharePoint().ObtenerContexto();


            if (new Utils().ValidarSiExisteRutaCarpeta(RutaCarpeta))
            {
                FiltrarRegistrosPorTipo(RutaCarpeta, Utils.COBRANZA_CC01, contexto);
                //Poner el monto Total de [Cuentas Por cobrar ] a 0
                //new daCuentasXCobrar().ActualizarMontoACero(contexto);
                FiltrarRegistrosPorTipo(RutaCarpeta, Utils.PORCOBRAR_C002, contexto);
                FiltrarRegistrosPorTipo(RutaCarpeta, Utils.PORCOBRAR_CC03, contexto);
            }
            else
            {
                Console.WriteLine("No existe carpeta: {0}", RutaCarpeta);
            }

        }
        #region Proceso de data

        private static bool ValidarClienteCodigo(string CodigoCliente, string RazonSocial)
        {

            if (!string.IsNullOrEmpty(CodigoCliente) && !string.IsNullOrEmpty(RazonSocial) && CodigoCliente != "0000000000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static private string TraerUnidadXSociedad(string Sociedad, string CodigoCliente, List<enUnidadCliente> ListaUnidadCliente, List<enSociedad> ListaUnidad)
        {
            string Rpta = "Urbanizaciones";
            try
            {
                if (ListaUnidad.Where(x => x.Sociedad == Sociedad).Count() > 1)
                {
                    if (ListaUnidadCliente.Exists(x => x.Sociedad == Sociedad && x.CodigoCliente.PadLeft(12,'0') == CodigoCliente.PadLeft(12, '0')))
                        Rpta = ListaUnidadCliente.Where(x => x.Sociedad == Sociedad && x.CodigoCliente.PadLeft(12, '0') == CodigoCliente.PadLeft(12, '0')).FirstOrDefault().Unidad;
                }
                else if (ListaUnidad.Where(x => x.Sociedad == Sociedad).Count() == 1)
                {
                    Rpta = ListaUnidad.Where(x => x.Sociedad == Sociedad).FirstOrDefault().Unidad;
                }

            }
            catch (Exception ex)
            {
                Rpta = string.Empty;
                throw ex;
            }


            return Rpta;
        }

        private static void FiltrarRegistrosPorTipo(string RutaCarpeta, string TipoArchivo, SP.ClientContext contexto)
        {
            string[] RutaArchivos;
            // enCliente cli = new enCliente();
            List<enCliente> lstCliente = new List<enCliente>();
            List<enUnidadCliente> ListaUnidadCliente = new daUnidadCliente().TraertUnidadCliente(contexto);
            List<enSociedad> ListaUnidad = new daSociedad().ListaUnidades(contexto);

            

            if (TipoArchivo.Equals(Utils.COBRANZA_CC01))
            {
                string RutaCOBRANZA_CC01 = Path.Combine(RutaCarpeta, Utils.COBRANZA_CC01);
                RutaArchivos = Directory.GetFiles(RutaCOBRANZA_CC01 + Enviados, "*.txt");
                foreach (var DirectorioArchivo in RutaArchivos)
                {
                    try
                    {
                        lstCliente = new List<enCliente>();
                        string[] registros = new Utils().leerCsv(DirectorioArchivo);
                        enCliente cli = null;
                        //string TipoArchivo = Path.GetFileName(RutaArchivo).Substring(0, 4);

                        foreach (var item in registros)
                        {
                            cli = new enCliente();
                            string[] columnas = item.Split('|');


                            if (ValidarClienteCodigo(columnas[6].Trim(), columnas[7].Trim()))
                            {
                                cli = new enCliente();
                                /*Zona de cliente*/
                                cli.CodigoCliente = columnas[6];
                                cli.RazonSocial = columnas[7];
                                cli.Sociedad = columnas[0];

                                /*Cobranza*/
                                //Obtener Anio / Mes del nombre de archivo
                                cli.Anio = columnas[2]; //Path.GetFileName(DirectorioArchivo).Substring(5, 4);
                                cli.Mes = columnas[3]; //Path.GetFileName(DirectorioArchivo).Substring(9, 2);
                                cli.Venta = Convert.ToDecimal(columnas[9], new CultureInfo("en-ES"));
                                cli.Division = TraerUnidadXSociedad(cli.Sociedad, cli.CodigoCliente, ListaUnidadCliente, ListaUnidad);


                                var venta = lstCliente.Where(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.CodigoCliente == cli.CodigoCliente
                                   && a.Division == cli.Division).ToList();
                                if (venta.Count == 0)
                                {
                                    lstCliente.Add(cli);
                                }
                                else
                                {
                                    decimal Ventatotal = venta[0].Venta;
                                    Ventatotal = Ventatotal + cli.Venta;
                                    lstCliente.First(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.RazonSocial == cli.RazonSocial
                                   && a.Division == cli.Division).Venta = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));
                                }


                            }

                        }//Fin Lectura de Registros
                        ProcesarDataCobranzas_o_PorCobrar(Utils.COBRANZA_CC01, lstCliente, contexto);
                        EliminacionArchivos(DirectorioArchivo, RutaCOBRANZA_CC01);
                    }//Fin Lectura de Archivos

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                        Console.ReadLine();
                    }

                }
            }
            if (TipoArchivo.Equals(Utils.PORCOBRAR_C002))
            {
                string RutaPORCOBRAR_C002 = Path.Combine(RutaCarpeta, Utils.PORCOBRAR_C002);
                RutaArchivos = Directory.GetFiles(RutaPORCOBRAR_C002 + Enviados, "*.txt");

                foreach (var DirectorioArchivo in RutaArchivos)
                {
                    try
                    {
                        lstCliente = new List<enCliente>();
                        string[] registros = new Utils().leerCsv(DirectorioArchivo);
                        enCliente cli = null;
                        //string TipoArchivo = Path.GetFileName(RutaArchivo).Substring(0, 4);

                        foreach (var item in registros)
                        {
                            cli = new enCliente();
                            string[] columnas = item.Split('|');


                            if (ValidarClienteCodigo(columnas[6].Trim(), columnas[7].Trim()))
                            {
                                cli = new enCliente();
                                /*Zona de cliente*/
                                cli.CodigoCliente = columnas[6];
                                cli.RazonSocial = columnas[7];
                                cli.Sociedad = columnas[0];

                                /*Cobranza*/
                                cli.Anio = Path.GetFileName(DirectorioArchivo).Substring(10, 4);
                                cli.Mes = Path.GetFileName(DirectorioArchivo).Substring(14, 2);
                                cli.Monto = Convert.ToDecimal(columnas[9], new CultureInfo("en-ES"));//--
                                //
                                if (!string.IsNullOrEmpty(columnas[15]))
                                {
                                    cli.MontoVencido = CalcularMontoVencido(cli.Monto, columnas[15], Utils.PORCOBRAR_C002);
                                    cli.Division = TraerUnidadXSociedad(cli.Sociedad, cli.CodigoCliente, ListaUnidadCliente, ListaUnidad);

                                    var venta = lstCliente.Where(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.CodigoCliente == cli.CodigoCliente
                                 && a.Division == cli.Division).ToList();
                                    if (venta.Count == 0)
                                    {
                                        lstCliente.Add(cli);
                                    }
                                    else
                                    {
                                        decimal Ventatotal = venta[0].Monto;
                                        Ventatotal = Ventatotal + cli.Monto;
                                        lstCliente.First(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.CodigoCliente == cli.CodigoCliente
                                       && a.Division == cli.Division).Monto = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));

                                        if (cli.MontoVencido != 0)
                                        {
                                            decimal MontoVencidototal = venta[0].MontoVencido;
                                            MontoVencidototal = MontoVencidototal + cli.MontoVencido;
                                            lstCliente.First(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.CodigoCliente == cli.CodigoCliente
                                           && a.Division == cli.Division).MontoVencido = Convert.ToDecimal(MontoVencidototal, new CultureInfo("en-ES"));
                                        }
                                    }
                                }
                            }

                        }//Fin Lectura de Registros
                        ProcesarDataCobranzas_o_PorCobrar(Utils.PORCOBRAR_C002, lstCliente, contexto);
                        //int o = 1;
                        //foreach (var item in lstCliente)
                        //{
                        //    new daCliente().ActualizarSociedad(item, contexto);
                        //    Console.WriteLine($"item{o++}");
                        //}
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                    }
                    EliminacionArchivos(DirectorioArchivo, RutaPORCOBRAR_C002);

                }//Fin Lectura de Archivos

            }
            if (TipoArchivo.Equals(Utils.PORCOBRAR_CC03))
            {
                string RutaPORCOBRAR_CC03 = Path.Combine(RutaCarpeta, Utils.PORCOBRAR_CC03);
                RutaArchivos = Directory.GetFiles(RutaPORCOBRAR_CC03 + Enviados, "*.txt");
                foreach (var DirectorioArchivo in RutaArchivos)
                {
                    try
                    {
                        string[] registros = new Utils().leerCsv(DirectorioArchivo);
                        enCliente cli = null;
                        //string TipoArchivo = Path.GetFileName(RutaArchivo).Substring(0, 4);

                        foreach (var item in registros)
                        {
                            cli = new enCliente();
                            string[] columnas = item.Split('|');
                            if (ValidarClienteCodigo(columnas[7].Trim(), columnas[8].Trim()))
                            {

                                cli = new enCliente();
                                /*Zona de cliente*/
                                cli.CodigoCliente = columnas[7];
                                cli.RazonSocial = columnas[8];
                                cli.Division = columnas[0];
                                int ValidarClientes = new daCliente().ListarClienteRetornaID(cli, contexto);
                                if (ValidarClientes == 0)
                                {
                                    cli.IdCliente = new daCliente().InsertarCliente_y_CuentasXCobrar(cli, contexto);
                                }
                                else
                                {
                                    cli.IdCliente = ValidarClientes;
                                }


                                /*Cobranza*/
                                cli.Anio = Path.GetFileName(DirectorioArchivo).Substring(5, 4);
                                cli.Mes = Path.GetFileName(DirectorioArchivo).Substring(9, 2);
                                cli.Monto = Convert.ToDecimal(columnas[13], new CultureInfo("en-ES"));//-- columnas[13];

                                if (!string.IsNullOrEmpty(columnas[14]))
                                {

                                    cli.MontoVencido = CalcularMontoVencido(cli.Monto, columnas[14], Utils.PORCOBRAR_CC03);

                                    cli.Division = TraerUnidadXSociedad(cli.Sociedad, cli.CodigoCliente, ListaUnidadCliente, ListaUnidad);


                                    var venta = lstCliente.Where(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.RazonSocial == cli.RazonSocial
                                       && a.Division == cli.Division).ToList();
                                    if (venta.Count == 0)
                                    {
                                        lstCliente.Add(cli);
                                    }
                                    else
                                    {
                                        decimal Ventatotal = venta[0].Monto;
                                        Ventatotal = Ventatotal + cli.Monto;
                                        lstCliente.First(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.RazonSocial == cli.RazonSocial
                                       && a.Division == cli.Division).Monto = Convert.ToDecimal(Ventatotal, new CultureInfo("en-ES"));

                                        if (cli.MontoVencido != 0)
                                        {
                                            decimal MontoVencidototal = venta[0].MontoVencido;
                                            MontoVencidototal = MontoVencidototal + cli.MontoVencido;
                                            lstCliente.First(a => a.Anio == cli.Anio && a.Mes == cli.Mes && a.RazonSocial == cli.RazonSocial
                                           && a.Division == cli.Division).MontoVencido = Convert.ToDecimal(MontoVencidototal, new CultureInfo("en-ES"));
                                        }
                                        else
                                        {

                                        }


                                    }
                                }


                            }

                        }

                        //Fin Lectura de Registros
                        EliminacionArchivos(DirectorioArchivo, RutaPORCOBRAR_CC03);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fue el siguiente: " + ex.Message);
                    }



                }
                ProcesarDataCobranzas_o_PorCobrar(Utils.PORCOBRAR_CC03, lstCliente, contexto);
                //Fin Lectura de Archivos
            }
        }

        private static void EliminacionArchivos(string RutaArchivo, string RutaFinal)
        {
            //if (Directory.Exists(Path.Combine(RutaFinal, RutaCarpetaDestino)))
            //{
            //    File.Move(RutaArchivo, Path.Combine(RutaFinal, RutaCarpetaDestino, Path.GetFileName(RutaArchivo)));
            //    Console.WriteLine("Archivo: " + Path.GetFileName(RutaArchivo) + " procesado correctamente \n");
            //}
            //else
            //{
            //    Directory.CreateDirectory(Path.Combine(RutaFinal, RutaCarpetaDestino));
            //    File.Move(RutaArchivo, Path.Combine(RutaFinal, RutaCarpetaDestino, Path.GetFileName(RutaArchivo)));
            //    Console.WriteLine("Archivo: " + Path.GetFileName(RutaArchivo) + " procesado correctamente \n");
            //}

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


        static decimal CalcularMontoVencido(decimal Monto, string Fecha, string TipoCalculo)
        {

            decimal MontoFinal = 0;


            if (Utils.PORCOBRAR_C002.Equals(TipoCalculo))
            {

                if (Convert.ToInt32(Fecha) > 30)
                {

                    MontoFinal = Monto;
                }

                else
                {
                    MontoFinal = 0;
                }

            }

            if (Utils.PORCOBRAR_CC03.Equals(TipoCalculo))
            {

                int TotalDias = (int)(DateTime.Now - Convert.ToDateTime(Fecha)).TotalDays;

                if (TotalDias > 30)
                {
                    MontoFinal = Monto;
                }
                else
                {
                    MontoFinal = 0;
                }

            }

            return MontoFinal;

        }

        #endregion

        #region Carga de datos SP
        private static void ProcesarDataCobranzas_o_PorCobrar(string TipoArchivo, List<enCliente> lstCliente, SP.ClientContext contexto)
        {

            if (TipoArchivo.Equals(Utils.COBRANZA_CC01))
            {

                foreach (var cli in lstCliente)
                {
                    cli.IdCliente = new daCliente().ListarClienteRetornaID(cli, contexto);
                    bool ExisteClienteAnioMes = false;// new daCobranzas().ListarCobranzaPorClienteMesAnio(cli, contexto);

                    if (cli.IdCliente == 0 && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Clientes] y [Cobranzas] 
                        Console.WriteLine(new daCliente().InsertarRegistro_y_Cobranza(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                    else if (cli.IdCliente > 0 && ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Actualizar el monto de [Cobranzas] por cliente/anio/mes  
                        Console.WriteLine(new daCobranzas().ActualizarMonto_x_Cliente(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                    else if (cli.IdCliente > 0 && !ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Cobranzas] sabiendo que ya el cliente existe , pero no [Cobranzas]/anio/mes  
                        Console.WriteLine(new daCobranzas().InsertarCobranzas(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                }

                

            }

            if (TipoArchivo.Equals(Utils.PORCOBRAR_C002))
            {
                foreach (var cli in lstCliente)
                {

                    cli.IdCliente = new daCliente().ListarClienteRetornaID(cli, contexto);
                    bool ExisteClienteAnioMes = new daCuentasXCobrar().ListarAnioMesPorCobrar(cli,contexto);

                    if (cli.IdCliente == 0 && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Clientes] y [Cuentas Por Cobranzas]  
                        //Console.WriteLine(new daCliente().InsertarRegistro_y_CuentasXCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                        cli.Action = "01";
                    }
                    else if (cli.IdCliente > 0 && ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Actualizar el monto de [Cuentas Por Cobranzas] por cliente/anio/mes  
                        //Console.WriteLine(new daCuentasXCobrar().ActualizarMonto_x_Cliente(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                        cli.Action = "02";
                    }
                    else if (cli.IdCliente > 0 && !ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Cuentas Por Cobranzas] sabiendo que ya el cliente existe , pero no [Cuentas Por Cobranzas]/anio/mes  
                        //Console.WriteLine(new daCuentasXCobrar().InsertarCuentaPorCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                        cli.Action = "03";
                    }
                }

                foreach (var cli in lstCliente.FindAll(x=>x.Action=="01"))
                {
                    Console.WriteLine(new daCliente().InsertarRegistro_y_CuentasXCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                }
                foreach (var cli in lstCliente.FindAll(x => x.Action == "02"))
                {
                    Console.WriteLine(new daCuentasXCobrar().ActualizarMonto_x_Cliente(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                }
                contexto.ExecuteQuery();
                foreach (var cli in lstCliente.FindAll(x => x.Action == "03"))
                {
                    Console.WriteLine(new daCuentasXCobrar().InsertarCuentaPorCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                }
                contexto.ExecuteQuery();


            }

            if (TipoArchivo.Equals(Utils.PORCOBRAR_CC03))
            {
                foreach (var cli in lstCliente)
                {
                    cli.IdCliente = new daCliente().ListarClienteRetornaID(cli, contexto);
                    bool ExisteClienteAnioMes = new daCuentasXCobrar().ListarAnioMesPorCobrar(cli, contexto);

                    if (cli.IdCliente == 0 && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Clientes] y [Cuentas Por Cobranzas]  
                        Console.WriteLine(new daCliente().InsertarRegistro_y_CuentasXCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                    if (cli.IdCliente > 0 && ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Actualizar el monto de [Cuentas Por Cobranzas] por cliente/anio/mes  
                        Console.WriteLine(new daCuentasXCobrar().ActualizarMonto_x_Cliente(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                    if (cli.IdCliente > 0 && !ExisteClienteAnioMes && !string.IsNullOrEmpty(cli.Division))
                    {
                        //Insertar [Cuentas Por Cobranzas] sabiendo que ya el cliente existe , pero no [Cuentas Por Cobranzas]/anio/mes  
                        Console.WriteLine(new daCuentasXCobrar().InsertarCuentaPorCobrar(cli, contexto) + "\n ----------------------------------- \n ----------------------------------- ");
                    }
                }


            }
        }

        #endregion

    }

}


















