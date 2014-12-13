using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;

namespace Test
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window {
			get;
			set;
		}
		// class-level declarations
		UIWindow window;
		String pathToJson;
		UITabBarController tabBarController;
		//ViewController
		InformationViewController informationViewController;
		MapViewController mapViewController;
		NearMeViewController nearMeViewController;
		//navigationController
		UINavigationController infoNavController;
		UINavigationController mapNavController;
		UINavigationController nearMeNavController;
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			Console.WriteLine ("schermo"+UIScreen.MainScreen.Bounds);
			//setto il database
			pathToJson = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//_pathToDatabase = Path.Combine(documents, "db_sqlite-bs.db");
			/*using (var conn= new SQLite.SQLiteConnection(_pathToDatabase))
			{
				conn.CreateTable<Sede>();
				//conn.CreateTable<Orari> ();
			}*/
			fillDatabase ();       //DA COMMENTARE DOPO LA PRIMA ESECUZIONE 
			// definisco i vari controller che costituiscono l applicazione
			mapViewController = new MapViewController ();
			informationViewController = new InformationViewController ();
			nearMeViewController = new NearMeViewController ();
			// definisco i navController di ogni sezione
			mapNavController = new UINavigationController (mapViewController);
			nearMeNavController = new UINavigationController (nearMeViewController);
			infoNavController = new UINavigationController (informationViewController);
			infoNavController.Title = "Informazioni";
			nearMeNavController.Title = "Vicino a me";
			mapNavController.Title = "Mappa";
			//tabbar

			tabBarController = new UITabBarController ();
			tabBarController.ViewControllers=new UIViewController []{nearMeNavController,mapNavController,infoNavController};
			//assegno il rootViewController
			window.RootViewController = tabBarController;
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
		public void fillDatabase(){
			var listSede = new List<Sede> ();
			var l = new List<Orari> ();
			l.Add(new Orari("Lun-Ven",new TimeSpan(8,0,0),new TimeSpan(13,0,0),new TimeSpan(14,0,0),new TimeSpan(18,0,0)));

			var s = new Sede ("1","Filiale di Padova", "via Trieste", 35043,"Padova","PD", "padova@mail.it","0439 78943", 42.374262,-71.120824,"www.filpadova.com",l);
			listSede.Add (s);		
			s = new Sede ("2","Filiale di Rovigo", "via rovigo", 30123,"Rovigo","RO", "rovigo@mail.it","0439 78443", 42.364264,-71.020824,"www.filrovigo.com",l);
			listSede.Add (s);		

			s = new Sede ("2","Filiale di Marcon", "via venetia", 33192,"Marcon","Ve", "marcon@mail.it","0439 15443", 42.364999,-71.123824,"www.filmarcon.com",l);
			listSede.Add (s);		

			s = new Sede ("2","Filiale di Rana", "via giovanni", 34192,"Tortellini","BO", "giovannirana@mail.it","0432 15443", 42.313999,-71.123001,"www.Grana.com",l);
			listSede.Add (s);		
			string json = JsonConvert.SerializeObject(listSede);

			Console.WriteLine (json);
			System.IO.File.WriteAllText(pathToJson+"\filiali.json",json);

			StreamReader r = new StreamReader (pathToJson + "\filiali.json");	
			string jsonread = r.ReadToEnd();
			List<Sede> items = JsonConvert.DeserializeObject<List<Sede>>(jsonread);

			/*using (var conn= new SQLite.SQLiteConnection(_pathToDatabase)){
				conn.Insert (s);
				s=new Sede ("Verona","michelangelo",42.474300,-71.120800);
				conn.Insert (s);
				s=new Sede ("Venezia","Marcon",42.384280,-71.150844);
				conn.Insert (s);
				s=new Sede ("Trieste","polo",42.377210,-71.520802);
				conn.Insert (s);
				s=new Sede ("Roma","centro",42.474232,-71.150865);
				conn.Insert (s);
			}*/
		}
	}
}