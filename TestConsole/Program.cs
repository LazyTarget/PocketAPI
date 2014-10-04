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


            do
            {
                Console.WriteLine();

                Console.WriteLine("Commands:");
                Console.WriteLine("  Get - retrievies last 10 items");
                Console.WriteLine("  Add #id/url - adds item");
                Console.WriteLine("  Archive #id - archives item with id");
                Console.Write("> ");
                var input = Console.ReadLine();
                
                try
                {
                    var actions = new List<PocketAction>();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }
                    else if (input == "Get")
                    {
                        Console.WriteLine("---Items---");
                        var titles = svc.GetItems().ToList();
                        if (titles.Any())
                        {
                            foreach (var t in titles)
                                Console.WriteLine("#{0} {1}", t.ItemID, t.ResolvedTitle);
                        }
                        else
                            Console.WriteLine("No items");
                        Console.WriteLine();
                    }
                    else if (input.IndexOf("Add ") == 0)
                    {
                        var idStr = input.Substring("Add ".Length).Trim();
                        int id;
                        if (int.TryParse(idStr, out id))
                        {
                            actions.Add(new AddPocketAction
                            {
                                ItemID = id
                            });
                        }
                        else if (!string.IsNullOrWhiteSpace(idStr))
                        {
                            actions.Add(new AddPocketAction
                            {
                                Url = idStr
                            });
                        }
                        else
                            Console.WriteLine("Invalid id/url");
                    }
                    else if (input.IndexOf("Archive ") == 0)
                    {
                        var idStr = input.Substring("Archive ".Length).Trim();
                        int id;
                        if (int.TryParse(idStr, out id))
                        {
                            actions.Add(new ArchivePocketAction
                            {
                                ItemID = id
                            });
                        }
                        else
                            Console.WriteLine("Invalid id");
                    }
                    else
                    {
                        
                    }


                    if (actions.Any())
                    {
                        var result = svc.Modify(actions);
                        foreach (var actionResult in result)
                        {
                            Console.WriteLine(" {0} => {1}", actionResult.Action, actionResult.Success);
                        }
                    }

                }
                catch (PocketException ex)
                {
                    Console.WriteLine("PocketException thrown!!");
                    Console.WriteLine("Error: {0}", ex.Message);
                    Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                }

            } while (true);


            Console.WriteLine();
            Console.WriteLine("Exiting application");
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
