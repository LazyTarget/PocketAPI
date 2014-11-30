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

            var multi = false;
            var actions = new List<PocketAction>();
            do
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("------------------------------------");
                Console.WriteLine();

                Console.WriteLine("Commands:");
                Console.WriteLine(" App commands:");
                Console.WriteLine("    Multi {Bool} - enables/disables multi edit");
                Console.WriteLine("    Send - sends multi edit actions");
                Console.WriteLine("    Exit - exits application");
                Console.WriteLine(" Retrieve commands:");
                Console.WriteLine("    Get - retrievies last 10 items");
                Console.WriteLine(" Modify commands:");
                Console.WriteLine("    Add #id/url - adds item");
                Console.WriteLine("    Archive #id - archives item with id");
                Console.WriteLine("    Unarchive #id - moves an item from the archive to the list");
                Console.WriteLine("    Favorite #id - favorite item with id");
                Console.WriteLine("    Unfavorite #id - unfavorite item with id");
                Console.WriteLine("    Delete #id - delete item with id");
                Console.WriteLine("    Tags_Add #id 'tag,tag2' - adds tags to item");
                Console.WriteLine("    Tags_Replace #id 'tag,tag2' - replaces tags on item");
                Console.WriteLine("    Tags_Remove #id 'tag,tag2' - removes tags from item");
                Console.WriteLine("    Tags_Clear #id - removes all tags from item");
                Console.Write("> ");
                var input = Console.ReadLine();

                try
                {
                    input = input ?? "";
                    var commandFound = true;
                    string command = null;
                    var commandArgs = new List<object>();
                    var parts = input.Split(' ');
                    string temp = null;
                    for (var i = 0; i < parts.Length; i++)
                    {
                        var x = (parts[i] ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(x))
                            continue;

                        if (x.StartsWith("\""))
                            temp = x;
                        else if (temp != null)
                            temp += " " + x;
                        if (temp != null && x.EndsWith("\""))
                        {
                            x = temp.Trim('"');
                            temp = null;
                        }


                        if (command == null)
                            command = x.ToLower();
                        else if (temp == null)
                            commandArgs.Add(x);
                            
                    }
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        continue;
                        break;
                    }


                    if (command == "exit")
                    {
                        return;
                    }
                    else if (command == "multi")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            bool tmp;
                            if (bool.TryParse(Convert.ToString(itemArg), out tmp))
                            {
                                multi = tmp;
                                actions.Clear();
                                Console.WriteLine("Multi set to: " + multi);
                            }
                            else
                                Console.WriteLine("Invalid input!!");
                        }
                        else
                            Console.WriteLine("Multi: " + multi);
                    }
                    else if (command == "get")
                    {
                        Console.WriteLine("---Items---");
                        var request = new GetItemsRequest();
                        var titles = svc.GetItems(request).ToList();
                        if (titles.Any())
                        {
                            foreach (var t in titles)
                                Console.WriteLine("{0}  {1}", t.ItemID, t.ResolvedTitle);
                        }
                        else
                            Console.WriteLine("No items");
                        Console.WriteLine();
                    }
                    else if (command == "add")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new AddPocketAction
                            {
                                Url = Convert.ToString(itemArg)
                            });
                        }
                        else
                            Console.WriteLine("Missing url argument");
                    }
                    else if (command == "archive")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new ArchivePocketAction
                            {
                                ItemID = Convert.ToInt32(itemArg)
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "unarchive")
                    {
                        var a = commandArgs.ElementAtOrDefault(0);
                        if (a != null)
                        {
                            actions.Add(new UnarchivePocketAction
                            {
                                ItemID = Convert.ToInt32(a)
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "favorite")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new FavoritePocketAction
                            {
                                ItemID = Convert.ToInt32(itemArg)
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "unfavorite")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new UnfavoritePocketAction
                            {
                                ItemID = Convert.ToInt32(itemArg)
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "delete")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new DeletePocketAction
                            {
                                ItemID = Convert.ToInt32(itemArg)
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "tags_add")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            var tags = (string) commandArgs.ElementAtOrDefault(1);
                            if (!string.IsNullOrWhiteSpace(tags))
                            {
                                actions.Add(new TagsAddPocketAction
                                {
                                    ItemID = Convert.ToInt32(itemArg),
                                    Tags = tags,
                                });
                            }
                            else
                                Console.WriteLine("Missing tag argument");
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "tags_replace")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            var tags = (string) commandArgs.ElementAtOrDefault(1);
                            if (!string.IsNullOrWhiteSpace(tags))
                            {
                                actions.Add(new TagsReplacePocketAction
                                {
                                    ItemID = Convert.ToInt32(itemArg),
                                    Tags = tags,
                                });
                            }
                            else
                                Console.WriteLine("Missing tag argument");
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "tags_remove")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            var tags = (string) commandArgs.ElementAtOrDefault(1);
                            if (!string.IsNullOrWhiteSpace(tags))
                            {
                                actions.Add(new TagsRemovePocketAction
                                {
                                    ItemID = Convert.ToInt32(itemArg),
                                    Tags = tags,
                                });
                            }
                            else
                                Console.WriteLine("Missing tag argument");
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "tag_rename")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            var oldTag = Convert.ToString(commandArgs.ElementAtOrDefault(1));
                            var newTag = Convert.ToString(commandArgs.ElementAtOrDefault(2));

                            if (!string.IsNullOrWhiteSpace(oldTag) &&
                                !string.IsNullOrWhiteSpace(newTag))
                            {
                                actions.Add(new TagRenamePocketAction
                                {
                                    ItemID = Convert.ToInt32(itemArg),
                                    OldTag = oldTag,
                                    NewTag = newTag
                                });
                            }
                            else
                                Console.WriteLine("Missing tag arguments");
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else if (command == "tags_clear")
                    {
                        var itemArg = commandArgs.ElementAtOrDefault(0);
                        if (itemArg != null)
                        {
                            actions.Add(new TagsClearPocketAction
                            {
                                ItemID = Convert.ToInt32(itemArg),
                            });
                        }
                        else
                            Console.WriteLine("Missing ItemID argument");
                    }
                    else
                    {
                        commandFound = false;
                    }



                    if ((!multi && commandFound && actions.Any()) || command == "send")
                    {
                        if (actions.Any())
                        {
                            Console.WriteLine("Result: ");
                            var result = svc.Modify(actions);
                            foreach (var actionResult in result)
                            {
                                Console.WriteLine("  {0}", actionResult);
                            }
                            Console.WriteLine();
                            actions.Clear();
                        }
                        else
                        {
                            Console.WriteLine("No actions to send!");
                        }
                    }
                    


                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
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
