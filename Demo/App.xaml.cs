using GraphQLQLDemo.Pages;
using Xamarin.Forms;

namespace GraphQLQLDemo
{
    public partial class App : Application
    {
        public const string Endpoint = "";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AllPlayersPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
