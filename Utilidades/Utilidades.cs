using IBD.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Prestlan.Utilidades {

    public class Confirmacion {
        public static string crearConfirmacion(string s) {
            return "return confirm('" + Traduce.getTG(s) + "');";
        }
    }

    public class Errores {

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string AnotarEntityValidationException(DbEntityValidationException ex) {
            var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
            string fullErrorMessage = string.Join("; ", errorMessages);
            string exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            _log.Error(exceptionMessage);
            return exceptionMessage;
        }
    }

    public class Tools {
        public static string GenerarHash() {
            int size = 40;
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            var data = new byte[size];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            var result = new StringBuilder(size);
            foreach (var b in data) {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public static string GetUser_IP() {
            string clientIP = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) {
                clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            } else if (HttpContext.Current.Request.UserHostAddress.Length != 0) {
                clientIP = GetIP4Address();
            }
            return clientIP;
        }
        private static string GetIP4Address() {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress)) {
                if (IPA.AddressFamily.ToString() == "InterNetwork") {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty) {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName())) {
                if (IPA.AddressFamily.ToString() == "InterNetwork") {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}