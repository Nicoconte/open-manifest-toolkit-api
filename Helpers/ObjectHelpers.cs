namespace Open.ManifestToolkit.API.Helpers
{
    public class ObjectHelpers
    {
        public static List<(string PropName, object PropValue)> DestructureObject(object values)
        {
            return values
                .GetType()
                .GetProperties()
                .Select(c => (c.Name, values.GetType().GetProperty(c.Name).GetValue(values)))
                .ToList();
        }
    }
}
