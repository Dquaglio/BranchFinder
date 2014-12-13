using System;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace Test
{
	public class MapViewController:UIViewController{
		MKMapView map;
		List<Sede> sedi=new List<Sede> ();
		JsonManager jM = new JsonManager ();
		MyMapDelegate mapViewDelegate;
		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			map = new MKMapView (UIScreen.MainScreen.Bounds);
			View = map;
			sedi = jM.ReadData ();
			mapViewDelegate = new MyMapDelegate ();
			mapViewDelegate.AnnotationTapped += TheMapView_OnAnnotationTapped;
			map.Delegate = mapViewDelegate;
			// Scorro le varie sedi e per ognuna inserisco un' annotazione nella mappa
			foreach (Sede c in sedi) {
				Console.WriteLine ("inserisco annotazione di " + c.nome + c.Coordinate.Latitude);
				Console.WriteLine ("lat: " + c.Coordinate.Latitude + "long: " + c.Coordinate.Longitude);
				map.AddAnnotation (c);
			}
			Console.WriteLine(map.Annotations.Length);
			//setto il range di visione
			map.ShowsUserLocation = true;
			var mapCenter = new CLLocationCoordinate2D (42.374260, -71.120824);
			var mapRegion = MKCoordinateRegion.FromDistance (mapCenter, 2000, 2000);
			map.CenterCoordinate = mapCenter;
			map.Region = mapRegion;
		}
		private void TheMapView_OnAnnotationTapped(Object sender, EventArgs args)
		{
			InformationView nuovo = new InformationView (sender);
			NavigationController.PushViewController (nuovo, true);
		}
		//classe che gestisce gli eventi sulla mappa
	    public class MyMapDelegate : MKMapViewDelegate {
			protected string annotationIdentifier = "BasicAnnotation";
			public event EventHandler AnnotationTapped;

			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation){
				// try and dequeue the annotation view
				MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier);   
				// if we couldn't dequeue one, create a new one
				if (annotationView == null)
					annotationView = new MKPinAnnotationView(annotation, annotationIdentifier);
				annotationView.CanShowCallout = true;
				(annotationView as MKPinAnnotationView).AnimatesDrop = true;
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Green;
				// you can add an accessory view; in this case, a button on the right
				annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
				return annotationView;
			}

		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control){
				AnnotationTapped((view.Annotation), new EventArgs());
			}
		}
	}
}

