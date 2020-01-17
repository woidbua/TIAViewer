using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using Microsoft.Win32;
using TIAViewer.Commands;
using TIAViewer.Model;

namespace TIAViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string Title = "TIA Selection Tool - Datei-Viewer";
        private ObservableCollection<GraphItemViewModel> _graphItemViewModels;
        private RelayCommand _loadTiaFileCommand;
        private ObservableCollection<TypeCount> _typeCounts;


        public MainViewModel()
        {
            _graphItemViewModels = new ObservableCollection<GraphItemViewModel>();
            TypeFrequency = new Dictionary<string, int>();
            TypeCounts = new ObservableCollection<TypeCount>();
        }

        public string LoadedFilename { get; set; }

        public TypeCount SelectedTypeCount { get; set; }

        public string WindowTitle => !string.IsNullOrEmpty(LoadedFilename) ? $"{Title} - \"{LoadedFilename}\"" : Title;

        public ObservableCollection<GraphItemViewModel> GraphItemViewModels
        {
            get => _graphItemViewModels;
            private set
            {
                if (_graphItemViewModels == null || _graphItemViewModels != value)
                {
                    _graphItemViewModels = value;
                }
            }
        }

        public ObservableCollection<GraphItemViewModel> FilteredGraphItemViewModels
        {
            get
            {
                var tempItems = GraphItemViewModels
                    .Where(graphItemViewModel => graphItemViewModel.Type == SelectedTypeCount.TypeName).ToList();
                return new ObservableCollection<GraphItemViewModel>(tempItems);
            }
        }


        public ObservableCollection<TypeCount> TypeCounts
        {
            get => _typeCounts;
            private set
            {
                if (_typeCounts == null || _typeCounts != value)
                {
                    _typeCounts = value;
                }
            }
        }

        public Dictionary<string, int> TypeFrequency { get; }


        public RelayCommand LoadTiaFileCommand =>
            _loadTiaFileCommand ?? (_loadTiaFileCommand = new RelayCommand(LoadTiaFile));

        private void LoadTiaFile(object parameter)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Bitte selektieren Sie die gewünschte TIA-Datei",
                Filter = @"tia file (*.tia)|*.tia",
                InitialDirectory = Environment.UserName,
                Multiselect = false
            };

            if (ofd.ShowDialog() != true)
            {
                return;
            }

            LoadedFilename = Path.GetFileName(ofd.FileName);
            var doc = new XmlDocument();

            try
            {
                doc.Load(ofd.FileName);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            GraphItemViewModels.Clear();
            TypeCounts.Clear();
            TypeFrequency.Clear();

            XmlElement root = doc.DocumentElement;

            XmlNodeList nodeTags = root?.GetElementsByTagName("node");
            if (nodeTags != null)
            {
                foreach (XmlNode nodeTag in nodeTags)
                {
                    if (nodeTag.Attributes?["Type"] == null) continue;
                    UpdateAttributeFrequency(nodeTag.Attributes?["Type"].Value);
                    GraphItemViewModels.Add(new GraphItemViewModel(new Node(nodeTag)));
                }
            }

            XmlNodeList edgeTags = root?.GetElementsByTagName("edge");
            if (edgeTags != null)
            {
                foreach (XmlNode edgeTag in edgeTags)
                {
                    if (edgeTag.Attributes?["Type"] == null) continue;
                    UpdateAttributeFrequency(edgeTag.Attributes?["Type"].Value);
                    GraphItemViewModels.Add(new GraphItemViewModel(new Edge(edgeTag)));
                }
            }

            foreach (KeyValuePair<string, int> keyValuePair in TypeFrequency)
            {
                TypeCounts.Add(new TypeCount(keyValuePair.Key, keyValuePair.Value));
            }
        }


        private void UpdateAttributeFrequency(string attribute)
        {
            if (!TypeFrequency.ContainsKey(attribute))
            {
                TypeFrequency.Add(attribute, 1);
            }
            else
            {
                TypeFrequency[attribute] += 1;
            }
        }
    }
}