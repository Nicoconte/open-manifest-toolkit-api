using Open.ManifestToolkit.API.Helpers;

namespace Open.ManifestToolkit.API.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string str, object values)
        {
            var destructuredObject = ObjectHelpers.DestructureObject(values);

            foreach(var obj in destructuredObject)
            {
                if (str.Contains($"@{obj.PropName}"))
                {
                    str = str.Replace($"@{obj.PropName}", obj.PropValue.ToString());
                }
            }

            return str;
        }
    }
}
    