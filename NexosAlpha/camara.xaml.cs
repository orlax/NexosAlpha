using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NexosAlpha
{
    public partial class camara : ContentPage
    {
        public camara()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "notFound", (string x) => { DisplayAlert("oh vaya..", "asegurate de enfocar bien la imagen", "ok"); });
        }
    }
}
