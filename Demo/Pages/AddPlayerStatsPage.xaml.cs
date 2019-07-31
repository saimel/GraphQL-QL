// AddPlayerStatsPage.xaml.cs
//
// Author: Saimel Saez ssaez@magaya.com
//
// 7/31/2019
//
//

using System;
using System.Collections.Generic;
using GraphQLQL;
using GraphQLQL.Abstractions;
using GraphQLQLDemo.Models;
using Xamarin.Forms;

namespace GraphQLQLDemo.Pages
{
    public partial class AddPlayerStatsPage : ContentPage
    {
        public CreateStatsModel StatsModel { get; set; }

        public AddPlayerStatsPage()
        {
            InitializeComponent();
        }

        public AddPlayerStatsPage(int playerId) : this()
        {
            StatsModel = new CreateStatsModel(playerId);

            BindingContext = StatsModel;
        }

        private async void OnBtnSaveTapped(object sender, EventArgs args)
        {
            IsBusy = true;

            var parameters = new List<MutationParameters>();
            parameters.Add(new MutationParameters { Name = "stats", Type = "StatisticInput!", Content = StatsModel });

            var mutation = GraphQLQueryLess.BuildMutation<PlayerStatsModel>("createStats", parameters);

            try
            {
                var response = await GraphQLService.PostAsync<PlayerStatsModel>(App.Endpoint, mutation, "createStats");

                await DisplayAlert("Success", $"Player average for entered season was {response.Average}", "OK");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                await DisplayAlert("Error", "Something went wrong ;-(", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
