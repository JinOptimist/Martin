using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Martin.Utility
{
    public static class TranslitHelper
    {
        public static string FromCyrillicToRomanAlphabet(this string line)
        {
            var charArray = line.ToCharArray();
            var sb = new StringBuilder();
            foreach (var symbol in charArray)
            {
                if (TableForTranslit.ContainsKey(symbol))
                    sb.Append(TableForTranslit[symbol]);
                else
                    sb.Append(symbol);
            }

            return sb.ToString();
        }

        public static Dictionary<char, string> TableForTranslit = new Dictionary<char, string>
        {
            {'а',"a"},
            {'б',"b"},
            {'в',"v"},
            {'г',"g"},
            {'д',"d"},
            {'е',"e"},
            {'ё',"jo"},
            {'ж',"zh"},
            {'з',"z"},
            {'и',"i"},
            {'й',"j"},
            {'к',"k"},
            {'л',"l"},
            {'м',"m"},
            {'н',"n"},
            {'о',"o"},
            {'п',"p"},
            {'р',"r"},
            {'с',"s"},
            {'т',"t"},
            {'у',"u"},
            {'ф',"f"},
            {'х',"h"},
            {'ц',"c"},
            {'ч',"ch"},
            {'ш',"sh"},
            {'щ',"shh"},
            {'ъ',"#"},
            {'ы',"y"},
            {'ь',"'"},
            {'э',"je"},
            {'ю',"ju"},
            {'я',"ya"},


            {'А',"A"},
            {'Б',"B"},
            {'В',"V"},
            {'Г',"G"},
            {'Д',"D"},
            {'Е',"E"},
            {'Ё',"Jo"},
            {'Ж',"Zh"},
            {'З',"Z"},
            {'И',"I"},
            {'Й',"J"},
            {'К',"K"},
            {'Л',"L"},
            {'М',"M"},
            {'Н',"N"},
            {'О',"O"},
            {'П',"P"},
            {'Р',"R"},
            {'С',"S"},
            {'Т',"T"},
            {'У',"U"},
            {'Ф',"F"},
            {'Х',"H"},
            {'Ц',"C"},
            {'Ч',"Ch"},
            {'Ш',"Sh"},
            {'Щ',"Shh"},
            {'Ъ',"#"},
            {'Ы',"Y"},
            {'Ь',"'"},
            {'Э',"Je"},
            {'Ю',"Ju"},
            {'Я',"Ya"},
        };
    }
}