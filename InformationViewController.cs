using System;
using MonoTouch.UIKit;

namespace Test
{
	public class InformationViewController: UIViewController{
		UIWebView webView=new UIWebView();
		//Costruttore
		public InformationViewController (){
			Title="Informazioni";
			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public override void ViewDidLoad (){
			base.ViewDidLoad();
			webView.Frame = View.Bounds;
			View.Add (webView);
		}
	}
}

