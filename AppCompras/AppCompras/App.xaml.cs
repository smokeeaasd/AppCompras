using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppCompras.Helpers;

namespace AppCompras
{
    public partial class App : Application
    {
        private static SQLiteDBHelper database;

        public static SQLiteDBHelper Database
        {
            get
            {
                if (database == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData
                    ), "arquivo.db3");

                    database = new SQLiteDBHelper(path);
                }

                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            MainPage = new NavigationPage(new View.ListaProduto());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
