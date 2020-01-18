namespace TIAViewer.Model
{
    public class TypeCount
    {
        public TypeCount(string typeName, int count = 0)
        {
            TypeName = typeName;
            Count = count;
        }

        public string TypeName { get; }

        public int Count { get; }
    }
}