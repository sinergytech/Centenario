using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using InsertCsvCentanario.Entidades;
using System.Security;
using System.Configuration;

namespace InsertCsvCentenario.AccesoDatos
{
    public class cnxSharePoint
    {
        static string UserSP = ConfigurationManager.AppSettings["UserSP"];
        static string PasswordSP = ConfigurationManager.AppSettings["PasswordSP"];
        static string UrlSP = ConfigurationManager.AppSettings["UrlSP"];
        public SP.ClientContext ObtenerContexto()
        {
            SP.ClientContext contexto = new SP.ClientContext($"{UrlSP}/cobranzas");
            contexto.Credentials = new SP.SharePointOnlineCredentials(UserSP, RetornarContraseña(PasswordSP));

            return contexto;
        }

        public SP.ClientContext ObtenerContextoDesarrolloUrbano()
        {
            SP.ClientContext contexto = new SP.ClientContext($"{UrlSP}/du");
            contexto.Credentials = new SP.SharePointOnlineCredentials(UserSP, RetornarContraseña(PasswordSP));
            return contexto;
        }
        public SP.ClientContext ObtenerRentaInmobiliaria()
        {
            SP.ClientContext contexto = new SP.ClientContext($"{UrlSP}/rentas");
            contexto.Credentials = new SP.SharePointOnlineCredentials(UserSP, RetornarContraseña(PasswordSP));

            return contexto;
        }

        private SecureString RetornarContraseña(string pwd)
        {
            SecureString contra = new SecureString();
            foreach (char item in pwd.ToCharArray()) contra.AppendChar(item);
            return contra;
        }


    }




}
