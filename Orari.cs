using System;
using SQLite;

namespace Test
{
	public class Orari
	{
		public String periodo{ get; set;}
		public TimeSpan open_morning_time{ get; set;}
		public TimeSpan close_morning_time{ get; set;}
		public TimeSpan open_afternoon_time{ get; set;}
		public TimeSpan close_afternoon_time{ get; set;}
		public Orari (String p, TimeSpan omt, TimeSpan cmt, TimeSpan oat, TimeSpan cat){
			periodo = p;
			open_morning_time = omt;
			close_morning_time = cmt;
			open_afternoon_time = oat;
			close_afternoon_time = cat;
		}
	}
}

