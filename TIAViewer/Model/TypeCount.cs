namespace TIAViewer.Model
{
    public class TypeCount
    {
        public TypeCount(string typeName, int count)
        {
            TypeName = typeName;
            Count = count;
        }

        public string TypeName { get; }

        public int Count { get; }

        public override string ToString()
        {
            return $"{TypeName} ({Count})";
        }
    }
}