﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Artikel_Import.src.Backend
{
	static class ebiss
	{


		internal static void write_ebis_tz_column()
		{

			string res = "";

			using (SQL sql = new SQL())
			{

				foreach (string artikelnummer in artikelnummern)
				{

					string[] RZPROZENT = sql.ExecuteQuery($@"
					select RZPROZENT from RABATTZUSCHLAG
					join ARTIKEL on ARTIKEL.RZNUMMER1 = RABATTZUSCHLAG.NUMMER
					where Artikelnr = '{artikelnummer}'");  //02030541320';
					if (RZPROZENT.Length > 1)
					{
						throw new Exception("got too long array entry");
						res += artikelnummer + "; - 4,75 \r\n";
					}
					if (RZPROZENT.Length == 1)
					{
						res += artikelnummer + ";" + RZPROZENT[0] + "\r\n";
					}
					else res += artikelnummer + "; - 4,75 \r\n";
				}
			}
			File.WriteAllText("C:\\admin\\ebis_rz.txt", res);
			Console.WriteLine("finished ebis");


		}
		private static string[] artikelnummern =
	}
}