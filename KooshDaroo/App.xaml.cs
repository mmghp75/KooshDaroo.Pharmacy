using KooshDaroo.Data;
using KooshDaroo.Views;
using System;
using Xamarin.Forms;
using KooshDaroo.Services;
using System.Threading.Tasks;
using KooshDaroo.Pharmacy.Views;

namespace KooshDaroo
{
    public partial class App : Application
    {
        public static string apiAddress = "http://kooshdaroo.nezarat.irimctest.ir/api/";//"http://10.0.2.2:55011/api/";
        public static string hubAddress = "http://kooshdaroo.nezarat.irimctest.ir/Hubs/PrescriptionHub";//"http://10.0.2.2:55011/Hubs/PrescriptionHub";
        //public static string apiAddress = "http://10.0.2.2:55011/api/";
        //public static string hubAddress = "http://10.0.2.2:55011/Hubs/PrescriptionHub";
    
        public static int myId = 0;
        public static string myPhoneNo = "";

        public App()
        {
            InitializeComponent();
            OpenPage();
        }

        private async void OpenPage()
        {
            KooshDarooDatabase odb = new KooshDarooDatabase();
            var oLoginItemS = odb.GetPharmacysAsync();
            if (oLoginItemS.Result.Count > 0)
            {
                PharmacyService Pharmacyervices = new PharmacyService();
                var pharmacyS = Task.Run(() => Pharmacyervices.GetPharmacyByPhoneNoAsync(oLoginItemS.Result[0].PhoneNo));
                if (pharmacyS.Result.Count == 0)
                {
                    oLoginItemS.Result.ForEach(f => odb.DeletePharmacyAsync(f));
                    MainPage = new NavigationPage(new SignUpPage());
                   //MainPage = new SignUpPage();
                }
                else
                {
                    myId = pharmacyS.Result[0].id;
                    myPhoneNo = pharmacyS.Result[0].PhoneNo;

                    MainPage = new NavigationPage(new MainPageTabbed());
                    //MainPage = new MainPageTabbed();
                }
            }
            else
                MainPage = new SignUpPage();


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
