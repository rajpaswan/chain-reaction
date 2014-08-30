using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ChainReaction.Resources;

namespace ChainReaction
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void play2_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.max_players = 2;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void play3_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.max_players = 3;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void play4_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.max_players = 4;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void box_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box.Count = (box.Count == 3) ? 1 : box.Count + 1;
        }

        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

    }
}