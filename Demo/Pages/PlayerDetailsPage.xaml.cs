// PlayerDetailsPage.xaml.cs
//
// Author: Saimel Saez ssaez@magaya.com
//
// 7/31/2019
//
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLQL;
using GraphQLQLDemo.Models;
using Xamarin.Forms;

namespace GraphQLQLDemo.Pages
{
    public partial class PlayerDetailsPage : ContentPage
    {
        private readonly int _playerId;

        public PlayerDetailsPage()
        {
            IsBusy = true;
            InitializeComponent();
        }

        public PlayerDetailsPage(int playerId) : this()
        {
            _playerId = playerId;

            Appearing += async delegate
            {
                await OnFetchRequested();
            };
        }

        protected async Task OnFetchRequested()
        {
            IsBusy = true;

            var filters = new Dictionary<string, object> { ["id"] = _playerId };
            string query = GraphQLQueryLess.BuildQuery<PlayerModel>("player", filters);

            try
            {
                var player = await GraphQLService.PostQueryAsync<PlayerModel>(App.Endpoint, query, "player");

                BindingContext = player;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.StackTrace);
#endif
                await DisplayAlert("Error", "Something went wrong ;-(", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnBtnAddStatsTapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AddPlayerStatsPage(_playerId));
        }
    }
}
