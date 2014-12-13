using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.MapKit;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using MonoTouch.CoreLocation;
using System.Drawing;
using MonoTouch.Foundation;

namespace Test
{
	public class InformationView : UIViewController{
		UILabel generalLabel;
		Sede sede;
		UILabel tableLabel;
		UITableView infoTable;
		UIButton calcolaPercorso;
		MKMapView miniMappa;
		public InformationView(Object ann){
			sede = (ann as Sede);
			View.BackgroundColor = UIColor.LightGray;
		}
		public override void ViewDidLoad ()
		{
			View = new UIScrollView ();
			base.ViewDidLoad ();
			this.Title = sede.nome;
			generalLabel = new UILabel ();
			generalLabel.Lines = 0;
			generalLabel.Text = "blablalblsalvl\n asdasdas";
			generalLabel.AdjustsFontSizeToFitWidth = true;
			generalLabel.TextAlignment = UITextAlignment.Center;
			generalLabel.BackgroundColor = UIColor.Gray;
			generalLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			miniMappa=new MKMapView();
			miniMappa.AddAnnotation (sede);
			miniMappa.ScrollEnabled = false;
			miniMappa.TranslatesAutoresizingMaskIntoConstraints = false;
			//CalcolaPercorso
			calcolaPercorso = new UIButton (UIButtonType.System);
			calcolaPercorso.SetTitle("Calcola percorso",UIControlState.Normal);
			calcolaPercorso.BackgroundColor = UIColor.White;
			calcolaPercorso.TranslatesAutoresizingMaskIntoConstraints = false;
			calcolaPercorso.Layer.CornerRadius = 3;
			calcolaPercorso.Layer.BorderWidth = 1;
			calcolaPercorso.Layer.BorderColor = UIColor.DarkGray.CGColor;
			//infoTable
			tableLabel = new UILabel ();
			tableLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			tableLabel.Text="Informazioni: ";
			tableLabel.TextColor = UIColor.Black;
			infoTable = new UITableView ();
			infoTable.TranslatesAutoresizingMaskIntoConstraints = false;
			infoTable.ScrollEnabled = false;
			var tableSource = new TableSource (sede.getInformations (), sede);
			infoTable.Source = tableSource;
			infoTable.Layer.CornerRadius = 5;
			infoTable.Layer.MasksToBounds = true;
			//infogeneriche
			infoTable.SeparatorColor = UIColor.Blue;
			infoTable.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
			//aggiungo le subView
			View.AddSubview (generalLabel);
			View.AddSubview (tableLabel);
			View.AddSubview (miniMappa);
			View.AddSubview (infoTable);
			View.AddSubview (calcolaPercorso);
			View.AddSubview (tableLabel);
			View.AddSubview (infoTable);

			//Vincoli
			View.AddConstraint (NSLayoutConstraint.Create (generalLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal,TopLayoutGuide , NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (generalLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal,this.View , NSLayoutAttribute.Width, 1, 0));
			generalLabel.AddConstraint (NSLayoutConstraint.Create (generalLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal,null , NSLayoutAttribute.NoAttribute, 1, 40));

			View.AddConstraint (NSLayoutConstraint.Create (miniMappa, NSLayoutAttribute.Top, NSLayoutRelation.Equal,generalLabel , NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (miniMappa, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 0.4f, 0));
			View.AddConstraint (NSLayoutConstraint.Create (miniMappa, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width,  0.9f, 0));
			View.AddConstraint (NSLayoutConstraint.Create (miniMappa, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0));

			View.AddConstraint (NSLayoutConstraint.Create (calcolaPercorso, NSLayoutAttribute.Top, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (calcolaPercorso, NSLayoutAttribute.Left, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Left, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (calcolaPercorso, NSLayoutAttribute.Top, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (calcolaPercorso, NSLayoutAttribute.Width, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Width, 1, 0));

			View.AddConstraint (NSLayoutConstraint.Create (tableLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, calcolaPercorso, NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (tableLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, calcolaPercorso, NSLayoutAttribute.Left, 1, 0));

			View.AddConstraint (NSLayoutConstraint.Create (infoTable, NSLayoutAttribute.Top, NSLayoutRelation.Equal, tableLabel, NSLayoutAttribute.Bottom, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (infoTable, NSLayoutAttribute.Width, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Width, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (infoTable, NSLayoutAttribute.Left, NSLayoutRelation.Equal, miniMappa, NSLayoutAttribute.Left, 1, 0));
			View.AddConstraint (NSLayoutConstraint.Create (infoTable, NSLayoutAttribute.Height, NSLayoutRelation.Equal,null, NSLayoutAttribute.NoAttribute, 1,130));

		}
	}
	public class TableSource:UITableViewSource{
		List<string> tableItems;
		string cellIdentifier = "TableCell";
		Sede sede;
		public TableSource (List<string> items,Sede s)
		{
			sede = s;
			tableItems = items;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Count;
		}
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.Layer.CornerRadius = 5;
			cell.Layer.MasksToBounds = true;
			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;
		}
		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{

			if(indexPath.Row==0){//telefono
				var alert = new UIAlertView ("Chiamare", sede.telefono, null, "OK", new string[] { "Annulla" });
				alert.Clicked += (s, b) => {
					if(b.ButtonIndex==0)
					UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel:"+sede.telefono.Replace(" ",string.Empty)));
				};
				alert.Show();
			}
			if(indexPath.Row==1){//mail
				var alert = new UIAlertView ("Mandare mail a:", sede.mail+" ?", null, "OK", new string[] {"Annulla"});
				alert.Clicked += (s, b) => {
					if(b.ButtonIndex==0)
					UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("mailto:"+sede.mail));
				};
				alert.Show();
			}
			if(indexPath.Row==2){//sito
				var alert = new UIAlertView ("Aprire sito:", sede.sito+" ?", null, "OK", new string[] {"Annulla"});
				alert.Clicked += (s, b) => {
					if(b.ButtonIndex==0)
					UIApplication.SharedApplication.OpenUrl(new NSUrl("http://"+sede.sito));
				};
				alert.Show();
			}
		}
	}
}

