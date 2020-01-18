using System;
using Microsoft.Win32;

namespace TIAViewer.Services
{
    public static class DialogService
    {
        public static string OpenTiaFileDialog()
        {
            var ofd = new OpenFileDialog
            {
                Title = "Bitte selektieren Sie die gewünschte TIA-Datei",
                Filter = @"tia file (*.tia)|*.tia",
                InitialDirectory = Environment.UserName,
                Multiselect = false
            };

            return ofd.ShowDialog() == true ? ofd.FileName : null;
        }
    }
}