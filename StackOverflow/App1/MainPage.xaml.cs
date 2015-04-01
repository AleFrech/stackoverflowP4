using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App1.Common;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GetQuestionList();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void GetQuestionList()
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("http://localhost:16470/")
            };
            RestRequest req = new RestRequest
            {
                Resource = "api/QuestionApi"
            };
            var resp = await client.Execute(req);
            RestSharp.Portable.Deserializers.JsonDeserializer des=new JsonDeserializer();
            var questionlist = des.Deserialize<IEnumerable<QuestionListModel>>(resp);
            foreach (var q in questionlist)
            {
                if(QuestionIndex.Items!=null)
                    QuestionIndex.Items.Add(q.Title+" "+"By: "+q.OwnerName);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }



        private void Button_SigIn(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignIn));
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {  
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Button_SignUp(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp));
        }

        private void Button_CreateQuestion(object sender, RoutedEventArgs e)
        {
            //logic if user alredy login
            Frame.Navigate(typeof (NewQuestion));
        }
    }
}
