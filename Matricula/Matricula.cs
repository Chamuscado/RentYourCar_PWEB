using System.Linq;

namespace Matricula
{
   public class Matricula
    {
        public static char Separador = '-';

        public static bool IsValid(string matricula)
        {
            if (matricula == null || matricula.Count() != 8 || matricula[2] != Separador || matricula[5] != Separador)
                return false;
            var sec = matricula.Split(Separador);
            if (sec.Length != 3)
                return false;
            byte let = 0;
            byte num = 0;

            foreach (var s in sec)
            {
                if (s.Length != 2)
                    return false;
                if (char.IsLetter(s[0]) || char.IsLetter(s[1]))
                    let++;
                if (char.IsDigit(s[0]) || char.IsDigit(s[1]))
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