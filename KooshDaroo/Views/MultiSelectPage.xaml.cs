using KooshDaroo.Services;
using KooshDaroo.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KooshDaroo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AspNetCore.SignalR.Client;

namespace KooshDaroo.Pharmacy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiSelectPage : ContentPage
    {
        public byte[] img;
        public Stream imgStream;
        public int prescribeID;

        private bool enableMultiSelect;
        private double fontsize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
        private int idx = 1;
        private HubConnection hubConnection;

        public MultiSelectPage()
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
            //RemoveSelectedCommand = new Command(OnRemoveSelected);
            //ToggleSelectionCommand = new Command(OnToggleSelection);
            SelectAllCommand = new Command(OnSelectAll);
            SendCommand = new Command(OnSend);

            BindingContext = this;

            hubConnection = new HubConnectionBuilder()
            .WithUrl(App.hubAddress)
            .Build();
            //hubConnection.On<double, double, double, string>("New Prescribe", (X, Y, dateOf, phoneNo) =>
            //{
            //    string _phoneNo = phoneNo;
            //    //    DisplayAlert("Ouch", prescribe.DateOf.ToString() + "\n" + prescribe.PhoneNo, "OK");
            //});

        }
        public void SetContent()
        {
            image = (Image)FindByName("image");
            selectAll = (Button)FindByName("selectAll");
            send = (Button)FindByName("send");

            image.Source = ImageSource.FromStream(() =>
            {
                return imgStream;
            });
        }

        public bool EnableMultiSelect
        {
            get { return enableMultiSelect; }
            set
            {
                enableMultiSelect = value;
                OnPropertyChanged();
            }
        }

        public SelectableObservableCollection<string> Items { get; }

        public ICommand AddItemCommand { get; }

        public ICommand RemoveSelectedCommand { get; }

        public ICommand ToggleSelectionCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand SendCommand { get; }

        private void OnAddItem()
        {
            Items.Add(idx.ToString());
            idx += 1;
        }

        //private void OnRemoveSelected()
        //{
        //    var selectedItems = Items.SelectedItems.ToArray();
        //    foreach (var item in selectedItems)
        //    {
        //        Items.Remove(item);
        //    }
        //}

        //private void OnToggleSelection()
        //{
        //    foreach (var item in Items)
        //    {
        //        item.IsSelected = !item.IsSelected;
        //    }
        //}
        private void OnSelectAll()
        {
            var selectedItems = Items.SelectedItems.ToArray();
            if (selectAll.Text == "Select All")
            {
                foreach (var item in Items)
                    item.IsSelected = true;
                selectAll.Text = "DeSelect All";
            }
            else
            {
                foreach (var item in Items)
                    item.IsSelected = false;
                selectAll.Text = "Select All";
            }
        }
        private async void OnSend()
        {
            PrescribeResourceService prescribeResourceService = new PrescribeResourceService();
            PrescribeResource prescribeResource = new PrescribeResource();
            
            prescribeResource.MemberAccepted = false;
            prescribeResource.PharmacyAccepted = true;
            prescribeResource.PharmacyId = App.myId;
            prescribeResource.Item01 = Items[0].IsSelected;
            prescribeResource.Item02 = Items[1].IsSelected;
            prescribeResource.Item03 = Items[2].IsSelected;
            prescribeResource.Item04 = Items[3].IsSelected;
            prescribeResource.Item05 = Items[4].IsSelected;
            prescribeResource.Item06 = Items[5].IsSelected;
            prescribeResource.Item07 = Items[6].IsSelected;
            prescribeResource.Item08 = Items[7].IsSelected;
            prescribeResource.Item09 = Items[8].IsSelected;
            prescribeResource.Item10 = Items[9].IsSelected;
            prescribeResource.PrescribeId = prescribeID;

            var result = await prescribeResourceService.PostPrescribeResourceAsync(prescribeResource);
            // if (result.id == 0)

            await Navigation.PopAsync();

            StartConnectionToHub();
            await hubConnection.SendAsync("SendAcceptToMember", result.id,result.PrescribeId);
        }
        private async void StartConnectionToHub()
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
                await hubConnection.StartAsync();
        }

    }
}