using LibGit2Sharp;
using Open.ManifestToolkit.API.Constants;
using Open.ManifestToolkit.API.Helpers;
using System.Diagnostics;
using System.Xml.Linq;

namespace Open.ManifestToolkit.API.Services
{
    public enum GitBranchScope
    {
        All,
        Local,
        Remote
    }

    public class GitService
    {
        public GitService() { }

        private Repository _localRepository = null;
        private string _repositoryPath;

        public void InitializeLocalRepository(string target, bool isCompleteLocalPath)
        {
            _repositoryPath = isCompleteLocalPath ? target : Path.Combine(AppContext.BaseDirectory, AppConstants.REPOSITORIES_BASE_PATH, target);

            if (!Directory.Exists(_repositoryPath))
            {
                throw new DirectoryNotFoundException(target);
            }

            _localRepository = new Repository(_repositoryPath);
        }

        public string Clone(string url)
        {
            var (owner, name) = GithubHelpers.GetInformationFromGithubUrl(url);

            string localpath = Path.Combine(AppContext.BaseDirectory, AppConstants.REPOSITORIES_BASE_PATH, name);

            string output = Repository.Clone(url, localpath);

            return output;
        }

        public bool DeleteLocalRepository(string target, bool isCompleteLocalPath)
        {
            string path = isCompleteLocalPath ? target : Path.Combine(AppContext.BaseDirectory, AppConstants.REPOSITORIES_BASE_PATH, target);

            DirectoryInfo dir = new DirectoryInfo(path);

            dir.GetFileSystemInfos("*", SearchOption.AllDirectories).ToList().ForEach(i =>
            {
                i.Attributes = FileAttributes.Normal;
            });


            dir.GetFiles().ToList().ForEach(f =>
            {
                f.Delete();
            });

            dir.EnumerateDirectories().ToList().ForEach(d =>
            {
                d.Delete(true);
            });

            return Directory.Exists(path);
        }

        public bool Branch(string name, string baseOn)
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            var result = _localRepository.CreateBranch(name, baseOn);

            return result != null;
        }

        public bool Checkout(string target)
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            var branch = _localRepository.Branches[target];

            if (branch is null)
                throw new Exception("Cannot change branch");

            Branch targetBranch = Commands.Checkout(_localRepository, branch);

            return targetBranch.FriendlyName == target;
        }

        public void Add(string command = "*")
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            Commands.Stage(_localRepository, "*");
        }

        public bool HasChanges()
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            return _localRepository.RetrieveStatus().Count() > 0;
        }

        public bool Pull(string branch, string remote = "origin")
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            var beforePull = _localRepository.Head.Tip;

            var config = _localRepository.Config;

            Signature author = new Signature(config.GetValueOrDefault<string>("user.name"), config.GetValueOrDefault<string>("user.email"), DateTime.Now);
            Signature committer = author;

            Commands.Pull(_localRepository, author, new PullOptions()
            {
                FetchOptions = new FetchOptions()
            });

            var afterPull = _localRepository.Head.Tip;

            return beforePull.Sha != afterPull.Sha;
        }

        public void Push(string target, string remote = "origin")
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd",
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process process = new Process { StartInfo = psi };
            process.Start();

            process.StandardInput.WriteLine($"cd {_repositoryPath}");
            process.StandardInput.WriteLine($"git push {remote} {target}");
            process.StandardInput.Close();

            process.WaitForExit();
        }

        public void Commit(string message = "Updated")
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            var config = _localRepository.Config;

            Signature author = new Signature(config.GetValueOrDefault<string>("user.name"), config.GetValueOrDefault<string>("user.email"), DateTime.Now);
            Signature committer = author;

            _localRepository.Commit(message, author, committer);
        }

        public bool ExistBranch(string name, GitBranchScope scope = GitBranchScope.All)
        {
            switch (scope)
            {
                case GitBranchScope.All:
                    return GetAllBranches().Any(s => s == name);
                case GitBranchScope.Local:
                    return GetAllLocalBranches().Any(s => s == name);
                case GitBranchScope.Remote:
                    return GetAllRemoteBranches().Any(s => s == name);
                default:
                    return false;
            }
        }

        public List<string> GetAllBranches(GitBranchScope scope = GitBranchScope.All)
        {
            switch (scope)
            {
                case GitBranchScope.All:
                    return GetAllBranches();
                case GitBranchScope.Local:
                    return GetAllLocalBranches();
                case GitBranchScope.Remote:
                    return GetAllRemoteBranches();
                default:
                    return new List<string>();
            }
        }

        public bool ExistLocalRepository(string name)
        {
            return Directory.Exists(Path.Combine(AppContext.BaseDirectory, AppConstants.REPOSITORIES_BASE_PATH, name));
        }


        private List<string> GetAllLocalBranches()
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            return _localRepository.Branches.Where(b => !b.IsRemote).Select(b => b.FriendlyName).ToList();
        }

        private List<string> GetAllRemoteBranches()
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));
            return _localRepository.Branches.Where(b => b.IsRemote).Select(b => b.FriendlyName).ToList();
        }

        private List<string> GetAllBranches()
        {
            if (_localRepository is null)
                throw new ArgumentException(nameof(Repository));

            return _localRepository.Branches.Select(b => b.FriendlyName).ToList();
        }
    }
}
