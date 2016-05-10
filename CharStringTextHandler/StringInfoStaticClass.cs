using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace CharStringTextHandler
{
    public class StringInfoStaticClass
    {
        public static void SubstringByTextElements(String s)
        {
            String output = String.Empty;
            StringInfo si = new StringInfo(s);
            for (Int32 element = 0; element < si.LengthInTextElements; element++) {
                output += String.Format(
                    "Text element {0} is '{1}' {2}",
                    element, si.SubstringByTextElements(element, 1),
                    Environment.NewLine
                    );
            }
            MessageBox.Show(output, "Result of GetTextElementEnumerator");
        }
        public static void EnumTextElements(String s) {
            String output = String.Empty;
            TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(s);
            while (charEnum.MoveNext())
            {
                output += String.Format(
                    "Character at index {0} is '{1}' {2}",
                    charEnum.ElementIndex, charEnum.GetTextElement(), Environment.NewLine
                    );
            }
            MessageBox.Show(output, "Result of GetTextElementEnumerator");
        }
        public static void EnumTextElementIndexes(String s)
        {
            String output = String.Empty;
            Int32[] textElemIndex = StringInfo.ParseCombiningCharacters(s);
            for (Int32 i = 0; i < textElemIndex.Length; i++)
            {
                output += String.Format(
                    "Character {0} starts at '{1}' {2}",
                    i, textElemIndex[i], Environment.NewLine
                    );
            }
            MessageBox.Show(output, "Result of ParseCombningCharacters");
        }
    }
}
