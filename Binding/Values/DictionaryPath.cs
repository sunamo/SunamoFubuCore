namespace FubuCore.Binding.Values
{
    public class DictionaryPath
    {
        public DictionaryPath(string path)
        {
            var parts = path.Trim().Split('.');
            ParentParts = parts.Reverse().Skip(1).Reverse();
            Parent = parts.Any()
                ? ParentParts.Join(".")
                : string.Empty;

            Key = parts.Last();
        }

        public IEnumerable<string> ParentParts { get; }

        public string Parent { get; }

        public string Key { get; }

        public void Set(SettingsData top, object value)
        {
            GetParentSource(top).Set(Key, value);
        }

        public SettingsData GetParentSource(SettingsData source)
        {
            ParentParts.Each(x =>
            {
                if (x.Contains("["))
                {
                    var parts = x.TrimEnd(']').Split('[');
                    var index = int.Parse(parts.Last());

                    source = source.GetChildrenElement(parts.First(), index);
                }
                else
                {
                    source = source.Child(x);
                }
            });

            return source;
        }

        public bool Equals(DictionaryPath other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Parent, Parent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(DictionaryPath)) return false;
            return Equals((DictionaryPath)obj);
        }

        public override int GetHashCode()
        {
            return Parent != null ? Parent.GetHashCode() : 0;
        }
    }
}
