using KooshDaroo.Data;
using KooshDaroo.Pharmacy.Views;
using KooshDaroo.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KooshDaroo.Services;
using KooshDaroo.Controls;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace KooshDaroo.Pharmacy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPageTabbed : Xamarin.Forms.TabbedPage
    {
        private int currentPrescribesId = 0;
        private HubConnection hubConnection;

        private ObservableCollection<Prescription> _prescriptionList = new ObservableCollection<Prescription>();
        public ObservableCollection<Prescription> prescriptionList
        {
            get
            {
                return _prescriptionList ?? (_prescriptionList = new ObservableCollection<Prescription>());
            }
        }
        //private ListView lvPrescription;
        public SelectableObservableCollection<Prescription> responseS { get; }

        public MainPageTabbed()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            page01.IconImageSource = ImageSource.FromResource("KooshDaroo.Pharmacy.Images.gallery.jpg");
            page02.IconImageSource = ImageSource.FromResource("KooshDaroo.Pharmacy.Images.gallery.jpg");
            page03.IconImageSource = ImageSource.FromResource("KooshDaroo.Pharmacy.Images.gallery.jpg");


            //UpdateList(30.292121, 57.067799, 43811.82505482639);
            responseS = new SelectableObservableCollection<Prescription>();
            //var _r = new Prescription
            //{
            //    DateAndDistance = "8:8",
            //    Tag = "154"
            //};
            //responseS.Add(_r);


            hubConnection = new HubConnectionBuilder()
                        .WithUrl(App.hubAddress)
                        .Build();

            hubConnection.On<double, double, DateTime, int>("SendPrescribeToPharmacy", (X, Y, dateOf, prescribeId) =>
            {
                UpdateList(X, Y, dateOf, prescribeId);
            });
            //hubConnection.On<string>("New Message", (message) =>
            //{
            //    string a = message;
            //});
            hubConnection.On<int>("SendAcceptToPharmacy", (prescribeResourceId) =>
            {
                //UpdateList(0, 0, DateTime.FromOADate(0), id, false);
                AddFinalizedPrescription(prescribeResourceId);
            });
            hubConnection.On<int, int>("SendAcceptToMember", (prescribeResourceId, prescribeId) =>
            {
                UpdateList(0, 0, DateTime.FromOADate(0), prescribeId, false);
            });


            StartConnectionToHub();

            //await hubConnection.SendAsync("SendPrescribeToPharmacy", prescribes.PhoneNo, prescribes.DateOf);

            BindingContext = this;
        }

        private async void AddFinalizedPrescription(int prescribeResourceId)
        {
            PrescribeResourceService prescribeResourceService = new PrescribeResourceService();
            PrescribeResource prescribeResource = await prescribeResourceService.GetPrescribeResourceById(prescribeResourceId);
            var _prescription = new Prescription
            {
                DateAndDistance = prescribeResource.MemberAcceptDateOf.Hour.ToString() + ":" + prescribeResource.MemberAcceptDateOf.Minute.ToString(),
                Tag = prescribeResource.id.ToString()
            };
            responseS.Add(_prescription);
        }

        private async void ItemClicked(object o, EventArgs e)
        {
            string id = (o as Xamarin.Forms.Button).ClassId;

            PrescribeService prescribesservice = new PrescribeService();
            Prescribe prescribe = await prescribesservice.GetPrescribeByIdAsync(Convert.ToInt32(id));

            currentPrescribesId = prescribe.id;
            MultiSelectPage page = new MultiSelectPage();
            page.imgStream = new MemoryStream(prescribe.Prescription);
            page.img = prescribe.Prescription;
            page.prescribeID = prescribe.id;
            page.SetContent();

            await this.Navigation.PushAsync(page);
            //UpdateList(prescribe.X, prescribe.Y, prescribe.DateOf, prescribe.id, false);
        }
        private void unresponsedPrescriptionListItemClicked(object o , EventArgs e)
        {

        }
        private async void AcceptedPrescriptionClicked(object o, EventArgs e)
        {



        }
        private async void UpdateList(double X, double Y, DateTime dateOf, int prescribeId, bool add = true)
        {
            KooshDarooDatabase odb = new KooshDarooDatabase();
            var pharmacyS = odb.GetPharmacysAsync();
            var pharmacy = pharmacyS.Result[0];
            Location sourceCoordinates = new Location(X, Y);
            Location destinationCoordinates = new Location(pharmacy.X, pharmacy.Y);
            Int32 distance = Convert.ToInt32(Location.CalculateDistance(sourceCoordinates, destinationCoordinates, DistanceUnits.Kilometers) * 1000);

            var prescription = new Prescription()
            {
                DateAndDistance = dateOf.Hour.ToString() + ":" + dateOf.Minute.ToString() + "       ==>       " + distance.ToString() + " (متر)",
                Tag = prescribeId.ToString()
            };

            //add or remove the a prescription to the list
            if (add)
                _prescriptionList.Insert(0, prescription);
            else
            {
                for (int i = 0; i < _prescriptionList.Count; i++)
                    if (prescription.Tag == _prescriptionList[i].Tag)
                    {
                        _prescriptionList.RemoveAt(i);
                        break;
                    }
            }

            lvPrescription = (Xamarin.Forms.ListView)FindByName("lvPrescription");
            lvPrescription.ItemsSource = prescriptionList;
        }
        private async void StartConnectionToHub()
        {
            //var placemarks = await Geocoding.GetPlacemarksAsync(30.291118, 57.067785);//(location.Latitude, location.Longitude);
            //var placemark = placemarks?.FirstOrDefault();
            //if (placemark != null)
            //{
            //    string a = "Admin Area: " + placemark.AdminArea;
            //    string b = "Country Name:" + placemark.CountryName;
            //    string c = "Country Code:" + placemark.CountryCode;
            //    string d = "Locality:" + placemark.Locality;
            //    string e = "SubAdmin Area:" + placemark.SubAdminArea;
            //    string f = "SubLocality:" + placemark.SubLocality;
            //    string g = "PostalCode:" + placemark.PostalCode;
            //}
            if (hubConnection.State == HubConnectionState.Disconnected)
                await hubConnection.StartAsync();
        }

    }

    public class Prescription
    {
        public string DateAndDistance { get; set; }
        public string Tag { get; set; }
    }
}