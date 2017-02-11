using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NexosAlpha
{
    public partial class mainPage : ContentPage
    {
        bool subscribed = false;
        public mainPage()
        {
            InitializeComponent();
            //takePictureBtn.Clicked += takePictureBtn_Clicked;
            if (!subscribed)
            {
                MessagingCenter.Subscribe<string>(this, "notFound", (string x) => { DisplayAlert("oh vaya..", "asegurate de enfocar bien la imagen", "ok"); labelTxt.Text = "Escanear una imágen"; });
                MessagingCenter.Subscribe<Byte[]>(this, "uploadImage", previewImage);
                MessagingCenter.Subscribe<string>(this, "stopRunning", (string s) => {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        indicator.IsRunning = false; 
                      //  takePictureBtn.Opacity = 0; 
                      //  takePictureBtn.IsEnabled = false; 
                        labelTxt.Text = "Escanear una imágen"; 
                    });
                });
               // MessagingCenter.Subscribe<string>(this, "uploadImage", previewImage);
                subscribed = true;
            }
        }

        private void previewImage(string obj)
        {
            previewImg.Source = ImageSource.FromFile(obj);
        }

        private void previewImage(byte[] obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                indicator.IsRunning = true; 
               // takePictureBtn.Opacity = 1; 
               // takePictureBtn.IsEnabled = false;
                labelTxt.Text = "Escaneando..";
                previewImg.Source = ImageSource.FromStream(() => new MemoryStream(obj));
            });
        }

        void takePictureBtn_Clicked(object sender, EventArgs e)
        {
           //send message for takePicture
            MessagingCenter.Send<string>("takePicture", "takePicture");
        }

    }
}
