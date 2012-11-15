using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace KChOTS.Security {
    public static class Extensions {
        public static bool IsEmail(this string value){
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                        + @"[a-zA-Z]{2,}))$";
            Regex emailRegex = new Regex(patternStrict);
            return emailRegex.IsMatch(value);
        }

        public static bool IsNumeric(this string value) {
            double number;
            return Double.TryParse(value, out number);
        }
    }
}