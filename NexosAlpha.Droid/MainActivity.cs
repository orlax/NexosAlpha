using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android.Graphics;
using System.Collections.Generic;
using Android.Provider;
using Java.IO;

namespace NexosAlpha.Droid
{ 
    //Theme = "@style/Theme.Splash", 
    [Activity(Label = "NexosAlpha",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static class photo
        {
            public static File _file;
            public static File _dir;
            public static Bitmap bitmap;
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            //create temp directory
            if (IsThereAnAppToTakePictures())
            {
                createDirectoryForPictures();
            }

            //show video code
            MessagingCenter.Subscribe<string>(this, "showVideo", (string x) =>
            {
                //Device.OpenUri(new Uri(x));
                var intent = new Intent(Intent.ActionView,Android.Net.Uri.Parse(x));
                intent.PutExtra("force_fullscreen", true);
                StartActivity(intent);
            });

            MessagingCenter.Subscribe<string>(this, "takePicture", takePicture);
        }

        private void takePicture(string obj)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            photo._file = new File(photo._dir, "nexos.jpg");
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(photo._file));
            StartActivityForResult(intent, 0);
        }

        private void createDirectoryForPictures()
        {
            photo._dir = new File(
           Android.OS.Environment.GetExternalStoragePublicDirectory(
           Android.OS.Environment.DirectoryPictures), "nexosDemo");
            if (!photo._dir.Exists())
            {
                photo._dir.Mkdirs();
            }    
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InSampleSize = 5 ;

            Bitmap image = BitmapFactory.DecodeFile(photo._file.Path,options);
            byte[] bytes;
           // convirtiendo imagen en bytes
            var stream = new System.IO.MemoryStream();
            image.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            bytes = stream.ToArray();
           // enviando bytes a main app para subir al server
            MessagingCenter.Send<Byte[]>(bytes, "uploadImage");
            MessagingCenter.Send<string>(photo._file.Path, "uploadImage");
        }
    }
}

