using System;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace Test
{//questi oggetti rappresentano una sede del negozio
	public class Sede : MKAnnotation{

		public override CLLocationCoordinate2D Coordinate { get; set;}
		public override string Title { get{ return nome; }}
		public override string Subtitle { get{ return via; }}
		public String id{ get; set; }
		public String nome{ get; set; }
		public String via{ get; set; }
		public int cap{ get; set;}
		public String citta{ get; set;}
		public String provincia{ get; set;}
		public String mail{ get; set; }
		public String telefono{ get; set; }
		public double latitudine{ get; set; }
		public double longitudine{ get; set; }
		public String sito{ get; set;}
		public List<Orari> orari{ get; set;}

		public Sede():base(){
		}

		public Sede (String id,String n,String v,int c,String city,String prov,String m,String tel,double x, double y,String s,List<Orari>or):base(){
			this.id = id;
			nome = n;
			via = v;
			cap = c;
			citta = city;
			provincia = prov;
			mail = m;
			telefono = tel;
			latitudine = x;
			longitudine = y;
			sito = s;
			orari = or;
			Coordinate = new CLLocationCoordinate2D (x, y);
		
		}
		public String getListItem(){
			return nome;
		}
		public int getInformationNumber(){
			return 2;
		}
		public List<string> getInformations(){
			List<string> informations = new List<string> ();
			informations.Add ("Telefono: "+telefono);
			informations.Add ("Mail: "+mail);
			informations.Add ("Sito: "+sito);
			return informations;
		}
	}
}

