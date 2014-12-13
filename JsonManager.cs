using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Test
{
	public class JsonManager
	{
		String pathToJson;
		String fileName;
		public JsonManager ()
		{
			fileName = "\filiali.json";
			pathToJson = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public List<Sede> ReadData(){
			StreamReader r = new StreamReader (pathToJson + fileName);	
			string jsonread = r.ReadToEnd();
			List<Sede> items = JsonConvert.DeserializeObject<List<Sede>>(jsonread);
			return items;
		}
	}
}

