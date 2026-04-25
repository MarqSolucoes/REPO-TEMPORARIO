using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Servicos.Auxiliares
{
    static internal class ServicoString
    {
        static internal int RetornaPosicaoEspacoMaisProximo(string texto, int posicaoMaxima)
        {
            var result = 0;

            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] == ' ')
                {
                    if (i > posicaoMaxima)
                        break;
                    else
                        result = i;
                }
            }

            return result;
        }
    }
}
