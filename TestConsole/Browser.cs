using System;
using System.IO;
using System.Windows.Forms;

namespace TestConsole
{
    public partial class Browser : Form
    {
        private readonly string _startUrl;

        public Browser(string startUrl)
        {
            InitializeComponent();

            webBrowser.Navigated += OnNavigated;

            _startUrl = startUrl;
        }


        public event EventHandler<WebBrowserNavigatedEventArgs> Navigated;



        public void Navigate(string url)
        {
            webBrowser.Navigate(url);
        }

        public string GetDocument()
        {
            var stream = webBrowser.DocumentStream;
            var s = new StreamReader(stream);
            var result = s.ReadToEnd();
            return result;
        }


        private void Browser_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Visible = false;
            Navigate(_startUrl);
        }


        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs args)
        {
            if (Navigated != null)
                Navigated(this, args);
        }
    }
}
