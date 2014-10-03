using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PocketAPI;

namespace TestConsole
{
    class Program
    {
        private static PocketApi svc;
        private static ConfirmEventArgs _confirmEventArgs;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            svc = new PocketApi();
            svc.ConsumerKey = "25447-07c62f25a2a1ca3066679052";
            svc.RedirectUri = "http://www.xhaus.com/headers";
            svc.OnConfirmRequired += Pocket_OnConfirmRequired;


            svc.Deauthenticate();

            try
            {
                for (var i = 1; i < 3; i++)
                {
                    Console.WriteLine("---BEGIN #{0}---", i);
                    var titles = svc.GetItems().Select(x => x.ResolvedTitle).ToList();
                    if (titles.Any())
                    {
                        foreach (var t in titles)
                            Console.WriteLine(t);
                    }
                    else
                        Console.WriteLine("No items");
                    Console.WriteLine("---END #{0}---", i);
                    Console.WriteLine();
                }
            }
            catch (PocketException ex)
            {
                Console.WriteLine("PocketException thrown!!");
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
            }


            Console.ReadLine();
        }



        private static void Pocket_OnConfirmRequired(object sender, ConfirmEventArgs args)
        {
            _confirmEventArgs = args;

            var b = new Browser(args.ConfirmUrl);
            b.Navigated += OnNavigated;

            Application.Run(b);
        }


        private static void OnNavigated(object sender, WebBrowserNavigatedEventArgs args)
        {
            var browser = (Browser)sender;
            //var doc = browser.GetDocument();
            if (args.Url.AbsoluteUri == svc.RedirectUri)
            {
                browser.Close();
                _confirmEventArgs.OnConfirm();
            }
            else
            {
                browser.Visible = true;
                browser.WindowState = FormWindowState.Normal;
            }
        }
        


    }
}
