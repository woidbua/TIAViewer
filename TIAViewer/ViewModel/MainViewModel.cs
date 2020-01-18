using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using TIAViewer.Commands;
using TIAViewer.Model;
using TIAViewer.Services;

namespace TIAViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string Title = "TIA Selection Tool - Datei-Viewer";

        private ObservableCollection<GraphItemViewModel> _graphItemViewModels;
        private ObservableCollection<TypeCountViewModel> _typeCountViewModels;

        private RelayCommand _loadTiaFileCommand;


        public MainViewModel()
        {
            _graphItemViewModels = new ObservableCollection<GraphItemViewModel>();
            _typeCountViewModels = new ObservableCollection<TypeCountViewModel>();
        }

        public string TiaFilename { get; set; }

        public string WindowTitle => !string.IsNullOrEmpty(TiaFilename) ? $"{Title} - \"{TiaFilename}\"" : Title;


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
                return new ObservableCollection<GraphItemViewModel>(GraphItemViewModels
                    .Where(graphItemViewModel => graphItemViewModel.Type == SelectedTypeCountViewModel.TypeName)
                    .ToList());
            }
        }


        public ObservableCollection<TypeCountViewModel> TypeCountViewModels
        {
            get => _typeCountViewModels;
            private set
            {
                if (_typeCountViewModels == null || _typeCountViewModels != value)
                {
                    _typeCountViewModels = value;
                }
            }
        }

        public TypeCountViewModel SelectedTypeCountViewModel { get; set; }


        public RelayCommand LoadTiaFileCommand =>
            _loadTiaFileCommand ?? (_loadTiaFileCommand = new RelayCommand(LoadTiaFile));

        private void LoadTiaFile(object parameter)
        {
            string tiaFilepath = DialogService.OpenTiaFileDialog();
            if (string.IsNullOrEmpty(tiaFilepath))
            {
                return;
            }

            TiaFilename = Path.GetFileName(tiaFilepath);
            var xmlGraphItems = TiaFileService.ExtractGraphItems(tiaFilepath);

            GraphItemViewModels.Clear();
            TypeCountViewModels.Clear();

            var typeFrequency = new Dictionary<string, int>();
            foreach (XmlNode xmlGraphItem in xmlGraphItems)
            {
                if (xmlGraphItem.Attributes?["Type"] == null) continue;
                string graphItemType = xmlGraphItem.Attributes["Type"].Value;
                if (typeFrequency.ContainsKey(graphItemType))
                {
                    typeFrequency[graphItemType] += 1;
                }
                else
                {
                    typeFrequency.Add(graphItemType, 1);
                }

                GraphItemViewModels.Add(new GraphItemViewModel(new GraphItem(xmlGraphItem)));
            }

            foreach (var keyValuePair in typeFrequency)
            {
                TypeCountViewModels.Add(new TypeCountViewModel(new TypeCount(keyValuePair.Key, keyValuePair.Value)));
            }
        }
    }
}