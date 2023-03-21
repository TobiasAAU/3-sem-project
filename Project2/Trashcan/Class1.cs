/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project2.Trashcan
{
    internal class Class1
    {

        private void btnGallery_ClickDeleteFile(object sender, RoutedEventArgs e)
        {
            var index = lstGallery.SelectedIndex;
            if (lstGallery.SelectedIndex >= 0)
            {
                string temppath = CurrentConfig.Temppath;
                File.SetAttributes(temppath, FileAttributes.Normal); //makes file not read-only permission.                
                System.Diagnostics.Debug.WriteLine("will now delete " + temppath);
                System.Diagnostics.Debug.WriteLine(temppath);


                *//*  File.Delete(temppath);*//*

            }
        }

    }
}
*/