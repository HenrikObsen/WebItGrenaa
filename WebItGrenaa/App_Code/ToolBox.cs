using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ToolBox
/// </summary>
public class ToolBox
{
    string strKey = "a b c d e f g h i j k l m n o p q r s t u v x y z 1 2 3 4 5 6 7 8 9 A B C D E F G H I J K L M N O P Q R S T U V X Y Z";
    string strKeyRev = "";
    string newText = "";
    string[] arrKey = { };

	public ToolBox()
	{
	    foreach (char c in strKey)
	    {
	        strKeyRev = c + strKeyRev;
	    }
	}

    

    public string Krypter(string text)
    {
        arrKey = strKeyRev.Split(' ');
      
        foreach (char ch in text)
        {
            int plac = strKey.Replace(" ", "").IndexOf(ch);
            
            if (plac >= 0)
            {
                newText += arrKey[plac];
            }
            else
            {
                newText += ch;
            }
        }

        return newText;
    }


    public string DeKrypter(string text)
    {
        arrKey = strKey.Split(' ');

        foreach (char ch in text)
        {
            int plac = strKeyRev.Replace(" ", "").IndexOf(ch);

            if (plac >= 0)
            {
                newText += arrKey[plac];
            }
            else
            {
                newText += ch;
            }
        }

        return newText;
    }
}