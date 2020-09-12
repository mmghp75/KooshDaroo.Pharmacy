using MultiSelectDemo.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KooshDaroo.Pharmacy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrescribeView : ContentPage
    {
        private bool enableMultiSelect;
        public Stream imgStream;
        private double fontsize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
        private int idx = 1;
        public PrescribeView()
        {
            InitializeComponent();
            var initialItems = new[] {
                "1st",
                "2nd",
                "3rd",
                "4th",
                "5th",
                "6th",
                "7th",
                "8th",
                "9th",
                "10th"
            };

            enableMultiSelect = true;
            Items = new SelectableObservableCollection<string>(initialItems);
            AddItemCommand = new Command(OnAddItem);
            RemoveSelectedCommand = new Command(OnRemoveSelected);
            ToggleSelectionCommand = new Command(OnToggleSelection);

            BindingContext = this;
        }
        public void SetContent()
        {
            //image = (Image)FindByName("image");
            //selectAll = (Button)FindByName("selectAll");
            //send = (Button)FindByName("send");

            //image.Source = ImageSource.FromStream(() =>
            //{
            //    return imgStream;
            //});


        }

        public SelectableObservableCollection<string> Items { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand RemoveSelectedCommand { get; set; }
        public ICommand ToggleSelectionCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }
        public ICommand SendCommand { get; set; }

        private void OnAddItem()
        {
            Items.Add(idx.ToString());
            idx += 1;
        }

        private void OnRemoveSelected()
        {
            var selectedItems = Items.SelectedItems.ToArray();
            foreach (var item in selectedItems)
            {
                Items.Remove(item);
            }
        }

        private void OnToggleSelection()
        {
            foreach (var item in Items)
            {
                item.IsSelected = !item.IsSelected;
            }
        }

        //private void OnSelectAll()
        //{
        //    var selectedItems = Items.SelectedItems.ToArray();
        //    if (selectAll.Text == "Select All")
        //    {
        //        foreach (var item in Items)
        //            item.IsSelected = true;
        //        selectAll.Text = "DeSelect All";
        //    }
        //    else
        //    {
        //        foreach (var item in Items)
        //            item.IsSelected = false;
        //        selectAll.Text = "Select All";
        //    }
        //}
        private void OnSend()
        {




            Navigation.PopAsync();
        }
    }
}