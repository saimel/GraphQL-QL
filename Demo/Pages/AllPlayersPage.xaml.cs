// AllPlayersPage.xaml.cs
//
// Author: Saimel Saez ssaez@magaya.com
//
// 7/31/2019
//
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GraphQLQL;
using GraphQLQLDemo.Models;
using Xamarin.Forms;

namespace GraphQLQLDemo.Pages
{
    public partial class AllPlayersPage : ContentPage
    {
        private bool _refresh;

        public ObservableCollection<PlayerListModel> Players { get; set; }

        public AllPlayersPage()
        {
            _refresh = true;
            Players = new ObservableCollection<PlayerListModel>();

            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_refresh == true)
            {
                lvwPlayers.BeginRefresh();
            }
        }

        protected async void OnRefreshing(object sender, EventArgs args)
        {
            Players.Clear();

            string query = GraphQLQueryLess.BuildQuery<PlayerListModel>("allPlayers");

            var players = await GraphQLService.PostQueryAsync<IList<PlayerListModel>>(App.Endpoint, query, "allPlayers");

            foreach (var player in players)
            {
                Players.Add(player);
            }

            lvwPlayers.EndRefresh();
        }

        protected async void OnPlayersItemTapped(object sender, ItemTappedEventArgs args)
        {
            _refresh = false;
            var player = (PlayerListModel)args.Item;

            await Navigation.PushAsync(new PlayerDetailsPage(player.Id));

            lvwPlayers.SelectedItem = null;
        }
    }
}
