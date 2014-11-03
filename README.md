PocketAPI
=========
<br>


[![Build status](https://ci.appveyor.com/api/projects/status/0hw1ed8lcik9pwlx?svg=true)](https://ci.appveyor.com/project/LazyTarget/pocketapi)


A C# library for querying against the Pocket's (formerly "Read it later") API



<h5>Example usage:</h5>

<pre><code>var service = new PocketApi();
service.ConsumerKey = "xxxxx-xxxxxxxxxxxxxxxxxxxxxxxx";
service.RedirectUri = "http://contuso.com/";
service.OnConfirmRequired = Pocket_OnConfirmRequired;
var items = service.GetItems();</code></pre>

<pre><code>private void Pocket_OnConfirmRequired(object sender, ConfirmEventArgs args) {
    // todo: Navigate to 'args.ConfirmUrl' for the user to authenticate application with a Pocket account
    args.OnConfirm();   // invoke when user has successfully authenticated
}
</code></pre>



<pre><code>public class Item
{
    public long ItemID { get; set; }
    public long ResolvedID { get; set; }
    public string GivenUrl { get; set; }
    public string ResolvedUrl { get; set; }
    public string GivenTitle { get; set; }
    public string ResolvedTitle { get; set; }
    public ItemStatus Status { get; set; }
    public int Favorite { get; set; }
    public int IsArticle { get; set; }
    public IEnumerable<string> Tags { get; set; }
}</code></pre>
