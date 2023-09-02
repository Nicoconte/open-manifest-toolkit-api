using System;

namespace Open.ManifestToolkit.API.Helpers
{
    public class GithubHelpers
    {
        public static (string Owner, string Name) GetInformationFromGithubUrl(string githubUrl)
        {
            string[] parts = githubUrl.Split("/");

            return (parts[3], parts[4].Replace(".git", string.Empty));
        }
    }
}
