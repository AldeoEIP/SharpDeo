using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SharpDeo.Model;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SharpDeo.Sample {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        private readonly Client _client = new Client ();

        public MainPage() {
            this.InitializeComponent ();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) {

        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var task = _client.LoginAsync ("toto42", "toto42").ConfigureAwait (false);
            await task;

            Debug.WriteLine ("Client is connected");
        }

        private async void Dictionary_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetDictionaryDefintionAsync ("poney").ConfigureAwait (false);
            Debug.WriteLine (result.Extract);

            var result2 = await _client.GetEncyclopediaDefintionAsync ("poney").ConfigureAwait (false);
            Debug.WriteLine (result2.Extract);
        }

        private async void Math_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.ResolveAsync (wolf.Text).ConfigureAwait (false);
            Debug.WriteLine (result);
        }

        private async void Users_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetUsersAsync ().ConfigureAwait (false);
            var enumerable = result as User[] ?? result.ToArray ();
            Debug.WriteLine (enumerable.Length.ToString ());
            id = enumerable.First ().Id;
        }

        private int id = 0;

        private async void User_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetUserAsync (id).ConfigureAwait (false);
            Debug.WriteLine (result.FirstName);
        }

        private async void Create_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.CreateUserAsync (new Noob ()).ConfigureAwait (false);
            Debug.WriteLine (result);
        }

        private async void aiml_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetAnswerAsync (aiml.Text).ConfigureAwait (false);
            Debug.WriteLine (result);
        }
    }
}
