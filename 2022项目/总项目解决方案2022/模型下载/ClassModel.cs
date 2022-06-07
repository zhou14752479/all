using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 模型下载
{
    public class ClassModel
    {

		private static string ddr = "ec18c2v4s16c-1x59g12g18b21w7k25f59n56q";

		
		private static string xsf = "ec69d50u51x70w58f122n76f75o76z58v76s99r120t127k67j91p68r95e80p-13g-17g-19x-13r";

		
		private static string ord = "ec72p67n56r54v-15y73s82t88m67w-2c11y41s76y78y26c45q";

		
		private static string ort = "ec18p5e10v26i";

		
		private static string ase = "ec69p63h75q80g55h114u63m67e60a76y97l119n121l107f61d70x77f87i104r";



		public static string BelleID = ClassModel.FrWork(ClassModel.ddr);

		
		public static string Tesource = ClassModel.FrWork(ClassModel.xsf);

		
		public static string Human = ClassModel.FrWork(ClassModel.ord);

		
		public static string Project = ClassModel.FrWork(ClassModel.ort);

		
		public static string Most = ClassModel.FrWork(ClassModel.ase);

		



				public static string FrWork(string buahos)
		{
			string text = ClassModel.Distinct("HYUG^&KJLPA%$#YBJ78hjnhuhsyd=b\u007f~<HYUGYBJ");
			char[] array = text.ToCharArray();
			int length = buahos.Length;
			int length2 = text.Length;
			int num = 0;
			for (int i = 0; i < length2; i++)
			{
				num += (int)array[i];
			}
			string text2 = (num * length2).ToString();
			num = Convert.ToInt32(text2.Substring(0, 2)) * 3;
			string text3 = buahos.Substring(2, length - 3);
			for (int j = 0; j < 26; j++)
			{
				int num2 = 96 + j + 1;
				char oldChar = (char)num2;
				text3 = text3.Replace(oldChar, '|');
			}
			string[] array2 = text3.Split(new char[]
			{
				'|'
			});
			int num3 = 0;
			string text4 = "";
			foreach (string value in array2)
			{
				num3 = ((length2 <= num3) ? 1 : (num3 + 1));
				char c = Convert.ToChar(text.Substring(num3 - 1, 1));
				int num4 = Convert.ToInt32(value) - num + (int)c;
				text4 += Convert.ToString((char)num4);
			}
			return text4;
		}



		public static string Distinct(string str)
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += (str[i] - '\n' + '\u0002').ToString();
			}
			return text;
		}

		public static string Escape(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in str)
			{
				stringBuilder.Append((char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '\\' || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
			}
			return stringBuilder.ToString();
		}




	}
}
