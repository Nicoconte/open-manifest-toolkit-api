using Open.ManifestToolkit.API.Services;

namespace Open.ManifestToolkit.API.Constants
{
    public class AppConstants
    {
        public const string REPOSITORIES_BASE_PATH = "Storage";

        public const string MANIFEST_VALUES_FILE = "values.yaml";
        public const string MANIFEST_ENV_VALUES_ROOT_PATH = "env";
        public const string MANIFEST_ENV_VALUES_NORMAL_PATH = "normal";
        public const string MANIFEST_ENV_SECRET_PATH = "secret";
        public const string MANIFEST_ENV_DEFAULT_VALUE = "BORRE LA VARIABLE O COLOQUE UN VALOR";


        public readonly static Dictionary<ManifestYamlErrors, string> MANIFEST_ERROR_MESSAGES = new Dictionary<ManifestYamlErrors, string>()
        {
            { ManifestYamlErrors.Styling, "La propiedad @key no tiene el comillado correcto" },
            { ManifestYamlErrors.InvalidDataType, "La propiedad @key no es un string. Tipo de dato original @dataType" },
            { ManifestYamlErrors.EmptyValue, "La propiedad @key no esta vacia o es nula" }
        };
    }
}
