using System;
using System.Linq;

namespace Matricula
{
   public class Matricula
    {
        public static char separador = '-';

        public static bool IsValid(string matricula)
        {
            if (matricula == null || matricula.Count() != 8 || matricula[2] != separador || matricula[5] != separador)
                return false;
            var sec = matricula.Split(separador);
            if (sec.Length != 3)
                return false;
            byte let = 0;
            byte num = 0;

            foreach (string s in sec)
            {
                if (s.Length != 2)
                    return false;
                if (Char.IsLetter(s[0]) || Char.IsLetter(s[1]))
                    let++;
                if (Char.IsDigit(s[0]) || Char.IsDigit(s[1]))
                    num++;
            }

            if (let == 1 && num == 2)
                return true;
            return false;
        }

        public static string GetValidMatriculas()
        {
            return "Insira a Matricula Formato: \"AA-00-00\", \"00-AA-00\" ou \"00-00-AA\" ";
        }
    }

}