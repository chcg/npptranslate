using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nppTranslateCS
{
    public class Util
    {
        public static bool isStringEmpty(String str)
        {
            if ((str != null) && (str.Length > 0))
                return false;
            else
                return true;
        }
    }
}
