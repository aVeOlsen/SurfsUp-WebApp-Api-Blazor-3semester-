using System;

namespace SurfsUpClassLibrary
{
    public class ReplaceChar
    {
        public string replace(char replaceChar, char replaceWith, string Data)
        {
            Data = Data.Replace(replaceChar, replaceWith);
            return Data;
        }
    }
}
