namespace Open.ManifestToolkit.API.Dtos
{
    public class ManifestReportDto
    {
        public ManifestVariableDto Variable { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
