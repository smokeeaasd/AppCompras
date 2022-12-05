using System;
using AppCompras.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace AppCompras.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> produtos = new ObservableCollection<Produto>();
        public ListaProduto()
        {
            InitializeComponent();

            lst_produtos.ItemsSource = produtos;
        }

        private void ToolbarItem_Clicked_Novo(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }

        private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
        {
            double soma = produtos.Sum(i => i.Preco * i.Quantidade);

            string msg = $"O Total da Compra é de: {soma:C}";

            DisplayAlert("Compra Somada!", msg, "OK");
        }

        private void txt_busca_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pesquisa = e.NewTextValue;

            Task.Run(async () =>
            {
                List<Produto> temp = await App.Database.Search(pesquisa);

                produtos.Clear();

                foreach(Produto item in temp)
                {
                    produtos.Add(item);
                }

                ref_carregando.IsRefreshing = false;
            });
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new EditarProduto {
                BindingContext = (Produto)e.SelectedItem
            });
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem disparador = (MenuItem)sender;

            Produto produto_selecionado = (Produto)disparador.BindingContext;

            bool confirmacao = await DisplayAlert("Remover item?", $"Irá remover {produto_selecionado.Descricao}", "Sim", "Não");
            
            if (confirmacao)
            {
                await App.Database.Delete(produto_selecionado.Id);

                produtos.Remove(produto_selecionado);
            }
        }

        protected override void OnAppearing()
        {
            if (produtos.Count == 0)
            {
                Task.Run(async () =>
                {
                    List<Produto> temp = await App.Database.GetAll();

                    foreach (Produto item in temp)
                    {
                        produtos.Add(item);
                    }

                    ref_carregando.IsRefreshing = false;
                });
            }
        }
    }
}