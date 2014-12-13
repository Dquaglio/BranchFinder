using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using SQLite;
using System.Linq;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace Test
{
	public class NearMeViewController: UIViewController{
		UISearchBar searchBar=new UISearchBar();
		UITableView tableView = new UITableView ();
		List<Sede> sedi=new List<Sede>();
		JsonManager jM = new JsonManager ();
		CLLocationManager lm;
		CLLocation location = new CLLocation ();
		TableSource tableSource;

		public NearMeViewController ():base(){
			Title = "Vicino a me";

			//EdgesForExtendedLayout = UIRectEdge.None;
			lm = new CLLocationManager ();
		}
		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.White;
			//Setto i parametri di searchBar
			lm.StartMonitoringSignificantLocationChanges ();
			lm.LocationsUpdated += (o, e) => location=lm.Location;
			searchBar.Placeholder = "Ricerca";
			searchBar.Frame = new RectangleF ();
			//searchBar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			searchBar.SizeToFit ();
			searchBar.TranslatesAutoresizingMaskIntoConstraints = false;
			//al cambiamento di quanto ricercato invoco un metodo di ricerca
			searchBar.TextChanged += (sender, e) => {
				Search (searchBar.Text);
			};
			sedi = jM.ReadData ();
			//setto i parametri della tableView
			tableView.Frame = new RectangleF ();
			tableView.TranslatesAutoresizingMaskIntoConstraints = false;
			tableView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			tableSource = new TableSource (sedi, location);
			tableSource.RowTapped += TheMapView_OnAnnotationTapped;
			tableView.Source = tableSource;
			var tap = new UITapGestureRecognizer ();
			tap.AddTarget (() => this.View.EndEditing (true));
			this.View.AddGestureRecognizer (tap);
			tap.CancelsTouchesInView = false;
			//aggiungo alla vista gli elementi
			View.Add (searchBar);
			View.Add (tableView);
			View.AddConstraint (NSLayoutConstraint.Create (searchBar, NSLayoutAttribute.Top, NSLayoutRelation.Equal,TopLayoutGuide , NSLayoutAttribute.Bottom, 1, 0));
			//View.AddConstraint (NSLayoutConstraint.Create (searchBar, NSLayoutAttribute.Width, NSLayoutRelation.Equal,TopLayoutGuide , NSLayoutAttribute.Width, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (searchBar, NSLayoutAttribute.Width, NSLayoutRelation.Equal,this.View , NSLayoutAttribute.Width, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (tableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal,searchBar , NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (tableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal,this.View , NSLayoutAttribute.Width, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (tableView, NSLayoutAttribute.Height, NSLayoutRelation.Equal,this.View , NSLayoutAttribute.Height, 1, 0));
		}

		private void TheMapView_OnAnnotationTapped(Object sender, EventArgs args)
		{
			InformationView nuovo = new InformationView ((sender as Sede));
			NavigationController.PushViewController (nuovo, true);
		}
		//metodo che filtra le sedi secondo quanto inserito nella barra di ricerca
		public void Search(String text){
			if(!String.IsNullOrEmpty(text))
			text=text.First().ToString().ToUpper() + text.Substring(1);
			Console.WriteLine (text);
			var filterSedi = sedi.Where (sede => sede.nome.Contains(text));
			List<Sede> list = filterSedi.ToList ();
			tableView.Source = new TableSource (list,location);
			tableView.ReloadData ();
		}


		//classe che gestisce gli elementi contenuti nella tabella 
		public class TableSource:UITableViewSource{
			protected List<Sede> tableItems=new List<Sede>();
			public event EventHandler RowTapped;
			protected String cellIdentifier="TableCell";
			CLLocation location;
			public override int RowsInSection (UITableView tableview, int section){
				return tableItems.Count;
			}
			public TableSource(List<Sede> list,CLLocation lm){
				tableItems=list;
				location=lm;
			}
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath){
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Value1, cellIdentifier);
				cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
				cell.TextLabel.Text = tableItems[indexPath.Row].getListItem();
				cell.DetailTextLabel.Text = Math.Round(distance(tableItems[indexPath.Row].latitudine,tableItems[indexPath.Row].longitudine,location.Coordinate.Latitude,location.Coordinate.Longitude)).ToString()+"Km";
				return cell;
			}
			public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
			{
				RowTapped(tableItems [indexPath.Row], new EventArgs());
			}
			//calcolo la distanza
			private double distance(double lat1, double lon1, double lat2, double lon2) {
				double theta = lon1 - lon2;
				double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
				dist = Math.Acos(dist);
				dist = rad2deg(dist);
				dist = dist * 60 * 1.1515;
					dist = dist * 1.609344;
				return (dist);
			}
			private double deg2rad(double deg) {
				return (deg * Math.PI / 180.0);
			}
			private double rad2deg(double rad) {
				return (rad / Math.PI * 180.0);
			}

		}
	}
}

