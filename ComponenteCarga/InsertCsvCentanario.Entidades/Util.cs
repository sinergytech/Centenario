using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using SP = Microsoft.SharePoint.Client;
using dr = System.Drawing;

namespace InsertCsvCentanario.Entidades
{
    public class Object
    {
        public static T Instance<T>(T t) where T : new() => t == null ? new T() : t;

        public static List<T> Instance<T>(List<T> t) => t == null ? new List<T>() : t;

        public static bool isNumber(char a)
        {
            return a >= '0' && a <= '9';
        }

        public static int IsNull(int? valor)
        {
            if (valor == null)
            {
                valor = 0;
            }
            return valor.Value;
        }

        public static string IsNull(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                valor = "";
            }
            return valor;
        }

        public static DateTime? ConvertDatetime(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                return DateTime.Parse(valor, culture);
            }
            return null;
        }

        public static string ConvertDatetime(DateTime? valor)
        {
            if (valor != null)
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                return valor.Value.ToString("dd/MM/yyyy", culture);
            }
            return "";
        }

        public static string ConvertToTime(DateTime? valor)
        {
            if (valor != null)
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                return valor.Value.ToString("HH:mm:ss", culture);
            }
            return "";
        }

        public static DateTime? ConvertStringToDate(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                try
                {
                    string[] ArregloFecha = valor.Split('/');
                    return new DateTime(Convert.ToInt32(ArregloFecha[2]), Convert.ToInt32(ArregloFecha[1]), Convert.ToInt32(ArregloFecha[0]));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public static string CreateISODatetime(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor, culture).ToString("yyyy-MM-dd", culture);
            }
            return valor;
        }

        public static string CreateISODatetimeDesde(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor, culture).ToString("yyyy-MM-ddTHH:mm:ssZ", culture);
            }
            return valor;
        }

        public static string CreateDatetime(DateTime? valor)
        {
            if (valor != null)
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                return valor.Value.ToString("yyyyMMddTHHmmssZ", culture);
            }
            return string.Empty;
        }

        public static string CreateISODatetimeHasta(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor, culture).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-ddTHH:mm:ssZ", culture);
            }
            return valor;
        }

        public static string CreateISODatetimeMasDay(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor, culture).AddDays(+1).ToString("yyyy-MM-dd", culture);
            }
            return valor;
        }

        public static string CreateSharepointDate_dd_MM_yyyy_MenosDay(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor).AddDays(-1).ToString("dd/MM/yyyy", culture);
            }
            return valor;
        }

        public static string CreateSharepointDate_dd_MM_yyyy(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                valor = DateTime.Parse(valor).ToString("dd/MM/yyyy", culture);
            }
            return valor;
        }


        public static string CreateISODatetimeHasta(DateTime? valor)
        {
            if (valor != null)
            {
                CultureInfo culture = new CultureInfo("Es-PE");
                return valor.Value.ToString("yyyy-MM-ddTHH:mm:ssZ", culture);
            }
            return string.Empty;
        }


        public static string RemplazarCaracteresEspeciales(string cadena)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex replace_ñ_Accents = new Regex("[ñ]", RegexOptions.Compiled);
            if (!string.IsNullOrEmpty(cadena))
            {
                cadena = replace_a_Accents.Replace(cadena, "a");
                cadena = replace_e_Accents.Replace(cadena, "e");
                cadena = replace_i_Accents.Replace(cadena, "i");
                cadena = replace_o_Accents.Replace(cadena, "o");
                cadena = replace_u_Accents.Replace(cadena, "u");
                cadena = replace_ñ_Accents.Replace(cadena, "n");
            }
            return cadena;
        }

        public static string ConstruirHTML(string cadena)
        {
            string salida = string.Empty;
            if (!string.IsNullOrEmpty(cadena) && cadena != "-")
            {
                salida = cadena.TrimStart("<br/>".ToCharArray());
            }
            return salida;
        }

        public static string ReplaceComas(string cadena)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                cadena = cadena.Replace(",", "");
            }
            return cadena;
        }

        public static string ReplaceSaltoLinea(string cadena)
        {
            string salida = string.Empty;
            if (!string.IsNullOrEmpty(cadena))
            {
                var array = cadena.Split(';');
                foreach (var item in array)
                {
                    if (!string.IsNullOrEmpty(item.Trim()))
                    {
                        salida += item.Trim() + " <br/>";
                    }
                }
            }
            return salida;
        }

        public static string ReducirTextoLargo(string cadena)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                if (cadena.Length > 75)
                {
                    cadena = cadena.Substring(0, 80) + "...";
                }
            }
            return cadena;
        }

        public static string extractNombreUsuario(object cadena)
        {
            if (cadena != null)
            {
                var split = cadena.ToString().Split('(');
                cadena = split[0].Trim();
            }
            return cadena.ToString();
        }

        public static string extractString(string cadena, string stringInicial, string stringFinal)
        {
            int terminaString = cadena.LastIndexOf(stringFinal);
            string nuevoString = cadena.Substring(0, terminaString);
            int offset = stringInicial.Length;
            int iniciaString = nuevoString.LastIndexOf(stringInicial) + offset;
            int cortar = nuevoString.Length - iniciaString;
            nuevoString = nuevoString.Substring(iniciaString, cortar);
            return nuevoString;
        }

        public static string toString(object t)
        {
            if (t == null) return default(string);
            return t.ToString();
        }

        public static string toFileName(object t, string extension)
        {
            if (t == null) return default(string);
            return t.ToString().Replace(extension, string.Empty);
        }

        public static int toInt(object v)
        {
            if (v == null) return default(int);
            return int.Parse(toString(v));
        }

        public static double toDouble(object v)
        {
            if (v == null) return default(double);
            return double.Parse(toString(v));
        }

        public static decimal toDecimal(object v)
        {
            if (v == null) return default(decimal);
            return decimal.Parse(toString(v));
        }

        public static bool toBool(object v)
        {
            if (v == null) return default(bool);
            return bool.Parse(toString(v));
        }

        public static bool toBoolwithNumeric(object v)
        {
            if (v == null) return default(bool);
            return bool.Parse(toString(v) == "1" ? "true" : "false");
        }
        public static string toStringBool(bool? v)
        {
            if (v == null) return "No";
            return v == true ? "Sí" : "No";
        }

        public static bool toErrorString(string v)
        {
            if (string.IsNullOrEmpty(v)) return false;
            return (v != "-");
        }

        public static int toLookupId(object lv)
        {
            int Id = default(int);
            if (lv != null)
            {
                var _lv = (SP.FieldLookupValue)lv;
                Id = _lv.LookupId;
            }
            return Id;
        }

        public static string toLookupValue(object lv)
        {
            string value = default(string);
            if (lv != null)
            {
                var _lv = (SP.FieldLookupValue)lv;
                value = _lv.LookupValue;
            }
            return value;
        }

        public static bool toLookupChoiceValue(object lv)
        {
            bool value = default(bool);
            if (lv != null)
            {
                var _lv = (SP.FieldLookupValue)lv;
                value = (_lv.LookupValue == "1" ? true : false);
            }
            return value;
        }

        public static bool isNull(object obj)
        {
            return obj == null;
        }

        public static bool isNotNull(object obj)
        {
            return obj != null;
        }

        public static string toStringforFieldUserValue(SP.FieldUserValue[] array)
        {
            string Resultado = string.Empty;
            if (array != null)
            {
                foreach (var ac in array)
                {
                    Resultado += ac.LookupValue + " (" + ac.Email + "); ";
                }
                Resultado = Resultado.TrimEnd("; ".ToArray());
            }
            return Resultado;
        }

        public static object ValidateValue(object value)
        {
            object obj = null;
            if (value != null)
            {
                if (value.GetType() == typeof(int))
                {
                    if ((int)value != 0)
                    {
                        obj = value;
                    }
                }
                else if (value.GetType() == typeof(string))
                {
                    if (!string.IsNullOrEmpty(value.ToString().Trim()))
                    {
                        value = value.ToString().Trim();
                        if (!value.ToString().Equals(";"))
                        {
                            obj = value;
                        }
                        else
                        {
                            obj = string.Empty;
                        }
                    }
                }
                else
                {
                    obj = value;
                }
            }
            return obj;
        }

        public static List<SP.FieldUserValue> toFieldUserValue(IList model)
        {
            List<SP.FieldUserValue> lst = new List<SP.FieldUserValue>();
            foreach (var item in model)
            {
                Type t = item.GetType();
                PropertyInfo proper = t.GetProperties().Where(x => x.Name.ToUpper().Equals("ID")).FirstOrDefault();
                if (proper != null)
                {
                    var value = ValidateValue(proper.GetValue(item));
                    if (value != null)
                    {
                        lst.Add(new SP.FieldUserValue { LookupId = (int)value });
                    }
                }
            }
            return lst;
        }

        public static List<SP.FieldLookupValue> toFieldLookupValue(IList model)
        {
            List<SP.FieldLookupValue> lst = new List<SP.FieldLookupValue>();
            foreach (var item in model)
            {
                Type t = item.GetType();
                PropertyInfo proper = t.GetProperties().Where(x => x.Name.ToUpper().Equals("ID")).FirstOrDefault();
                if (proper != null)
                {
                    var value = ValidateValue(proper.GetValue(item));
                    if (value != null)
                    {
                        lst.Add(new SP.FieldLookupValue { LookupId = (int)value });
                    }
                }
            }
            return lst;
        }

        public static List<SP.FieldLookupValue> toFieldUserValueFromUser(IList model)
        {
            List<SP.FieldLookupValue> lst = new List<SP.FieldLookupValue>();
            foreach (var item in model)
            {
                Type t = item.GetType();
                PropertyInfo proper = t.GetProperties().Where(x => x.Name.ToUpper().Equals("CORREO") || x.Name.ToUpper().Equals("EMAIL")).FirstOrDefault();
                if (proper != null)
                {
                    var value = ValidateValue(proper.GetValue(item));
                    if (value != null)
                    {
                        lst.Add(SP.FieldUserValue.FromUser(value.ToString()));
                    }
                }
            }
            return lst;
        }

        public static List<T> toListforFieldUserValue<T>(SP.FieldUserValue[] array) where T : new()
        {
            List<T> lst = new List<T>();
            if (array != null)
            {
                foreach (var ac in array)
                {
                    T entity = new T();
                    Type type = entity.GetType();
                    PropertyInfo[] proper = type.GetProperties();
                    foreach (var item in proper)
                    {
                        if (item.Name.ToUpper().Equals("ID"))
                        {
                            item.SetValue(entity, ac.LookupId, null);
                        }
                        else if (item.Name.ToUpper().Equals("NOMBRE"))
                        {
                            item.SetValue(entity, ac.LookupValue, null);
                        }
                        else if (item.Name.ToUpper().Equals("CORREO"))
                        {
                            item.SetValue(entity, ac.Email, null);
                        }
                    }
                    lst.Add(entity);
                }
            }
            return lst;
        }

        public static List<T> toListforFieldLookupValue<T>(SP.FieldLookupValue[] array) where T : new()
        {
            List<T> lst = new List<T>();
            foreach (var ac in array)
            {
                T entity = new T();
                Type type = entity.GetType();
                PropertyInfo[] proper = type.GetProperties();
                foreach (var item in proper)
                {
                    if (item.Name.ToUpper().Equals("ID"))
                    {
                        item.SetValue(entity, ac.LookupId, null);
                    }
                    else if (item.Name.ToUpper().Equals("NOMBRE"))
                    {
                        item.SetValue(entity, ac.LookupValue, null);
                    }
                }
                lst.Add(entity);
            }
            return lst;
        }

        public static T toEntityforString<T>(object item) where T : new()
        {
            T entity = new T();
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var proper in properties)
            {
                if (proper.Name.ToUpper().Equals("CORREO"))
                {
                    proper.SetValue(entity, item.ToString().Trim(), null);
                }
            }
            return entity;
        }

        public static List<T> toListforString<T>(object items) where T : new()
        {
            List<T> lst = new List<T>();
            if (items != null)
            {
                var array = items.ToString().Trim().Split(';');
                foreach (var item in array)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        T entity = new T();
                        Type type = entity.GetType();
                        PropertyInfo[] properties = type.GetProperties();
                        foreach (var proper in properties)
                        {
                            if (proper.Name.ToUpper().Equals("CORREO") || proper.Name.ToUpper().Equals("EMAIL"))
                            {
                                proper.SetValue(entity, item.Trim(), null);
                            }
                        }
                        lst.Add(entity);
                    }
                }
            }
            return lst;
        }


        public static string ControlTipoFactura(string Valor)
        {
            string Resultado = string.Empty;
            if (!string.IsNullOrEmpty(Valor))
            {
                Resultado = RemplazarCaracteresEspeciales(Valor).ToUpper().Contains("FACTURA") ? "FAC" : string.Empty;
            }
            return Resultado;
        }

        public static decimal IsNumerico(string Value)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                decimal valorNumerico = 0;
                if (decimal.TryParse(Value, out valorNumerico))
                {
                    CultureInfo culture = new CultureInfo("Es-PE");
                    return decimal.Parse(Value, culture);
                }
            }
            return 0;
        }

        public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }


    }
}
