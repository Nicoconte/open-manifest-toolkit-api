using Open.ManifestToolkit.API.Constants;
using Open.ManifestToolkit.API.Dtos;
using Open.ManifestToolkit.API.Extensions;
using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Open.ManifestToolkit.API.Services
{
    public enum ManifestYamlErrors
    {
        Styling,
        InvalidDataType,
        EmptyValue
    }

    public class ManifestYamlService
    {
        private string _filePath;
        private readonly YamlStream _yaml = new YamlStream();

        private YamlMappingNode _nodes;

        public ManifestYamlService() { }

        public void InitializeYaml(string manifestName)
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, AppConstants.REPOSITORIES_BASE_PATH, manifestName, AppConstants.MANIFEST_VALUES_FILE);

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException();
            }

            _NormalizeYamlBeforeInitialization(_filePath);

            var reader = new StreamReader(_filePath);

            _yaml.Load(reader);

            _nodes = (YamlMappingNode)_yaml.Documents.FirstOrDefault()?.RootNode;

            reader.Close();
        }

        public List<ManifestVariableDto> GetManifestEnvironmentVariables()
        {
            var normalNodes = _GetNodesFromNormalSection();

            var manifestVariables = new List<ManifestVariableDto>();

            foreach(var node in normalNodes)
            {
                var nodeInfo = _GetNodeInformation(node);

                manifestVariables.Add(new ManifestVariableDto()
                {
                    Key = nodeInfo.Key,
                    Value = nodeInfo.Value,
                    DataType = nodeInfo.Datatype
                });
            }

            return manifestVariables;
        }

        public void FixManifestEnvironmentVariables(List<ManifestVariableDto> variables)
        {
            var nodes = _GetNodesFromNormalSection();

            foreach(var variable in variables)
            {
                var yamlValue = new YamlScalarNode(variable.Value);
                yamlValue.Style = ScalarStyle.DoubleQuoted;

                if (string.IsNullOrEmpty(yamlValue.Value))
                {
                    yamlValue.Value = AppConstants.MANIFEST_ENV_DEFAULT_VALUE;
                    nodes.Children[new YamlScalarNode(variable.Key)] = yamlValue;

                    continue;
                }

                nodes.Children[new YamlScalarNode(variable.Key)] = yamlValue;
            }

            var writer = new StreamWriter(_filePath);
            _yaml.Save(writer, false);

            writer.Close();
        }

        public bool HasError()
        {
            return _GetNodesFromNormalSection().Any(node => _GetNodeErrors(node).Count > 0);
        }

        public List<ManifestReportDto> AnalyzeManifest()
        {
            var manifestReports = new List<ManifestReportDto>();

            var nodes = _GetNodesFromNormalSection();

            foreach (var node in nodes)
            {
                var nodeInfo = _GetNodeInformation(node);

                var report = new ManifestReportDto();

                report.Variable = new ManifestVariableDto()
                {
                    Key = nodeInfo.Key,
                    Value = nodeInfo.Value,
                    DataType = nodeInfo.Datatype
                };

                var errorMessages = _GetNodeErrors(node).Select(e => AppConstants.MANIFEST_ERROR_MESSAGES[e].Format(new
                {
                    key = nodeInfo.Key,
                    value = nodeInfo.Value,
                    dataType = nodeInfo.Datatype,
                }));

                report.Errors.AddRange(errorMessages);

                manifestReports.Add(report);
            }

            return manifestReports;
        }


        private List<ManifestYamlErrors> _GetNodeErrors(KeyValuePair<YamlNode, YamlNode> currentNode)
        {
            var errors = new List<ManifestYamlErrors>();

            var (key, value, dataType, style) = _GetNodeInformation(currentNode);

            if (style != ScalarStyle.DoubleQuoted)
            {
                errors.Add(ManifestYamlErrors.Styling);
            }

            if (dataType != typeof(string).Name && style != ScalarStyle.DoubleQuoted)
            {
                errors.Add(ManifestYamlErrors.InvalidDataType);
            }

            if (string.IsNullOrEmpty(value))
            {
                errors.Add(ManifestYamlErrors.EmptyValue);
            }

            return errors;
        }

        private YamlMappingNode _GetNodesFromNormalSection()
        {
            if (_nodes is null)
            {
                throw new ArgumentNullException("Cannot find nodes from manifest");
            }

            if (!_nodes.Children.TryGetValue(new YamlScalarNode(AppConstants.MANIFEST_ENV_VALUES_ROOT_PATH), out var envs))
            {
                throw new Exception("Cannot get values from env property");
            }

            var envsNodeMapped = (YamlMappingNode)envs;

            if (!envsNodeMapped.Children.TryGetValue(new YamlScalarNode(AppConstants.MANIFEST_ENV_VALUES_NORMAL_PATH), out var normal))
            {
                throw new Exception("Cannot get values from normal property");
            }

            return (YamlMappingNode)normal;
        }

        private (string Key, string Value, string Datatype, ScalarStyle Style) _GetNodeInformation(KeyValuePair<YamlNode, YamlNode> currentNode) 
        {
            var key = ((YamlScalarNode)currentNode.Key).Value;
            var value = ((YamlScalarNode)currentNode.Value).Value;
            var dataType = _GetOriginalTypeFromNodeValue(value).Name;
            var style = ((YamlScalarNode)currentNode.Value).Style;

            return (key, value, dataType, style);
        }

        private void _NormalizeYamlBeforeInitialization(string yamlPath)
        {
            var yaml = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build()
                .Deserialize<object>(File.ReadAllText(yamlPath));

            Dictionary<object, object> dictionary = (Dictionary<object, object>)yaml;
            Dictionary<object, object> uniques = new Dictionary<object, object>();
            HashSet<object> keys = new HashSet<object>();

            foreach(var pair in dictionary)
            {
                if (!uniques.ContainsKey(pair.Key))
                    uniques.Add(pair.Key, pair.Value);
                else
                    keys.Add(pair.Key);
            }

            foreach(var key in keys)
            {
                uniques.Remove(key);
            }

            var overrideContent = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build()
                .Serialize(uniques);

            File.WriteAllText(yamlPath, overrideContent);
        }

        private Type _GetOriginalTypeFromNodeValue(string value)
        {
            if (int.TryParse(value, out _))
                return typeof(int);
            if (bool.TryParse(value, out _))
                return typeof(bool);
            if (double.TryParse(value, out _))
                return typeof(double);

            return typeof(string);
        }
        
    }
}