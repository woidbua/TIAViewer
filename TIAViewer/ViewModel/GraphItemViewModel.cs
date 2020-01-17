using TIAViewer.Model;

namespace TIAViewer.ViewModel
{
    public class GraphItemViewModel : ViewModelBase
    {
        private readonly GraphItem _graphItem;

        public GraphItemViewModel(GraphItem graphItem)
        {
            _graphItem = graphItem;
        }

        public int PropertyCount => _graphItem.Properties.Count;

        public string Type => _graphItem.Type;

        public string Name => _graphItem.Name;
    }
}