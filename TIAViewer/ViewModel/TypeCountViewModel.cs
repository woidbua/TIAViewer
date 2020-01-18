using TIAViewer.Model;

namespace TIAViewer.ViewModel
{
    public class TypeCountViewModel : ViewModelBase
    {
        private readonly TypeCount _typeCount;

        public TypeCountViewModel(TypeCount typeCount)
        {
            _typeCount = typeCount;
        }

        public string TypeName => _typeCount.TypeName;

        public int Count => _typeCount.Count;

        public override string ToString()
        {
            return $"{TypeName} ({Count})";
        }
    }
}