using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SharpDeo.Model;

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
            var dialog = new MessageDialog ("connected");
            await dialog.ShowAsync ();
        }

        private async void Dictionary_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetDictionaryDefintionAsync ("poney").ConfigureAwait (false);
            var dialog = new MessageDialog (result);
            await dialog.ShowAsync ();
        }

        private async void Math_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.ResolveAsync (wolf.Text).ConfigureAwait (false);
            var dialog = new MessageDialog (result);
            await dialog.ShowAsync ();
        }

        private async void Users_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetUsersAsync ().ConfigureAwait (false);
            var enumerable = result as User[] ?? result.ToArray();
            var dialog = new MessageDialog (enumerable.Count().ToString());
            await dialog.ShowAsync ();
            id = enumerable.First ().Id;
        }

        private int id = 0;

        private async void User_OnClick(object sender, RoutedEventArgs e) {
            var result = await _client.GetUserAsync (id).ConfigureAwait (false);
            var dialog = new MessageDialog (result.FirstName);
            await dialog.ShowAsync ();
        }
    }
}
