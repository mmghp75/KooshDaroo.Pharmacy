using KooshDaroo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KooshDaroo.Data;
using KooshDaroo.Services;
using KooshDaroo.Pharmacy.Views;
using Xamarin.Essentials;

//X = 30.291118, Y = 57.067785

namespace KooshDaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private double fontsize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
        private Entry phoneNo, pharmacy;
        private Button signUpButton;
        private CheckBox is24h;
        private Label lbl24h;
        private Picker city;
        private double x = 0.0;
        private double y = 0.0;

        public SignUpPage()
        {
            StackLayout stackLayout = new StackLayout();
            StackLayout cbx24 = new StackLayout();
            cbx24.Orientation = StackOrientation.Horizontal;

            pharmacy = new Entry();
            pharmacy.FontSize = fontsize;
            pharmacy.Placeholder = "نام داروخانه";
            stackLayout.Children.Add(pharmacy);

            phoneNo = new Entry();
            phoneNo.FontSize = fontsize;
            phoneNo.Placeholder = "شماره تلفن : 09123456789";
            stackLayout.Children.Add(phoneNo);

            lbl24h = new Label();
            lbl24h.FontSize = fontsize;
            lbl24h.HorizontalOptions = LayoutOptions.EndAndExpand;
            lbl24h.Text = "داروخانه شبانه‌روزی";
            cbx24.Children.Add(lbl24h);
            is24h = new CheckBox();
            is24h.HorizontalOptions = LayoutOptions.Start;
            cbx24.Children.Add(is24h);
            stackLayout.Children.Add(cbx24);

            city = new Picker();
            city.FontSize = fontsize;
            city.Items.Add("< شهر >");
            city.Items.Add("تهران");
            city.Items.Add("کرمان");
            KooshDarooDatabase odb = new KooshDarooDatabase();
            List<tblCity> citys = odb.GetCitysAsync().Result;
            foreach (tblCity _city in citys)
            {
                city.Items.Add(_city.CityName + "(" + _city.StateName + ")");
            }
            city.SelectedIndex = 0;
            stackLayout.Children.Add(city);

            signUpButton = new Button();
            signUpButton.FontSize = fontsize;
            signUpButton.Text = "ثبت نام";
            signUpButton.Clicked += SignUpButton_Clicked;
            stackLayout.Children.Add(signUpButton);

            Content = stackLayout;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            PharmacyService Pharmacyservice = new PharmacyService();
            var _pharmacyS = await Pharmacyservice.GetPharmacyByPhoneNoAsync(phoneNo.Text);

            GetCurrentXY();
            if (x == 0 & y == 0)
                return;

            if (_pharmacyS.Count == 0)
            {
                KooshDaroo.Models.Pharmacy _pharmacy = new KooshDaroo.Models.Pharmacy { City=city.SelectedItem.ToString() , Title = pharmacy.Text ,X= x, Y= y, PhoneNo = phoneNo.Text, is24h=is24h.IsChecked  };
                var result = Pharmacyservice.PostPharmacyAsync(_pharmacy);
                App.myId = result.Result.id;
                App.myPhoneNo = result.Result.PhoneNo;
            }
            else
            {
                App.myId = _pharmacyS[0].id;
                App.myPhoneNo = _pharmacyS[0].PhoneNo;
           }
            KooshDarooDatabase odb = new KooshDarooDatabase();
            tblPharmacy newPharmacy = new tblPharmacy { City = city.SelectedItem.ToString(), Title = pharmacy.Text , X = x, Y = y, PhoneNo = phoneNo.Text, is24h = is24h.IsChecked };
            int r = await odb.SavePharmacyAsync(newPharmacy);

            //odb = new KooshDarooDatabase();
            //var oLoginItemS = odb.GetPharmacysAsync();
            //var o = oLoginItemS.Result.Count;

            App.Current.MainPage = new NavigationPage(new MainPageTabbed());
            //App.Current.MainPage = new MainPageTabbed();
            //App.Current.MainPage = new PrescribesListPage();
            //await this.Navigation.PushAsync(new PrescribesListPage());
        }
        private async void GetCurrentXY()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    x = location.Latitude;
                    y = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
        }
    }
}