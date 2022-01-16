using System;

public class NumberFormatter
{
/*      1000,                                       Thousands(KILO)
        1000000,                                    Millions(MEGA)
        1000000000,                                 Billions(GIGA)
        1000000000000,                              Trillions(TERA)
        1000000000000000,                           Quadrillions(PETA)
        1000000000000000000,                        Quintillions(EXA)
        1000000000000000000000f,                    Sextillions(ZETA) */

    private static string[] suffix= new string[]{"","K","M","G","T","P","E","Z"};
    public static string FormatNumHunds(double num)
    {
        double numStr;  
        int index = 0;
        numStr = FormatNumber(num, ref index);
        if (index==0)
            return Math.Round(numStr).ToString()+suffix[index];
        else
            return Math.Round(numStr,2).ToString()+suffix[index];
    }
    public static string FormatNumTens(double num)
    {
        double numStr;  
        int index = 0;
        numStr = FormatNumber(num, ref index);
        return Math.Round(numStr,1).ToString()+suffix[index];
    }
    private static double FormatNumber(double num, ref int index)
    {
        if(num>1000)
        {
            for (; num>1000; index++)
                num/=1000;
            return num;
        }
        else
            return num;
    }
}
