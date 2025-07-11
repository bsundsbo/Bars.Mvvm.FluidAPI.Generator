using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Utilities.Collections;
using System;
using System.IO;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Bars.Mvvm.Build;

[ShutdownDotNetAfterServerBuild]
public partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Publish);
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter("NuGet API key for publishing packages")]
    [Secret]
    readonly string NuGetApiKey;


    [NerdbankGitVersioning]
    readonly NerdbankGitVersioning NerdbankVersioning;

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Executes(() =>
        {
            CleanDirectory(OutputDirectory);
            DotNetClean(s => s.SetProject(Solution));
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore, LogVersion)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target LogVersion => _ => _
        .Executes(() =>
        {
            Serilog.Log.Logger.Information("NerdVersion NuGetPackageVersion={NuGetPackageVersion}, AssemblyVersion {AssemblyVersion}, InformationalVersion {AssemblyInformationalVersion}, SimpleVersion {SimpleVersion}", NerdbankVersioning.NuGetPackageVersion, NerdbankVersioning.AssemblyVersion, NerdbankVersioning.AssemblyInformationalVersion, NerdbankVersioning.SimpleVersion);
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testProjects = Solution.AllProjects
                .Where(p => p.Name.EndsWith(".Tests", StringComparison.OrdinalIgnoreCase));

            foreach (var project in testProjects)
            {
                DotNetTest(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .EnableNoRestore()
                );
            }
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            var packableProjects = Solution.AllProjects
                .Where(p =>
                    p.GetProperty<bool>("IsPackable") &&
                    p.Name != null &&
                    !p.Name.EndsWith(".Tests", StringComparison.OrdinalIgnoreCase) &&
                    File.Exists(p.Path))
                .ToList(); // adjust if needed

            foreach (var project in packableProjects)
            {
                DotNetPack(s => s
                    .SetProject(project)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .SetOutputDirectory(OutputDirectory)
                    .SetVersion(NerdbankVersioning.NuGetPackageVersion));
            }
        });

    [GitRepository]
    readonly GitRepository GitRepository;


    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            var allowedBranches = new[] { "release", "main", "develop" };
            if (GitRepository.Branch == null || !allowedBranches.Any(branch => GitRepository.Branch.StartsWith(branch, StringComparison.OrdinalIgnoreCase)))
            {
                Serilog.Log.Logger.Information("Skipping publish step on branch: {BranchName}", GitRepository.Branch);
                return;
            }
            if (IsLocalBuild)
            {
                Serilog.Log.Logger.Information("Not publishing packages in local build");
                return;
            }

            var source = "https://api.nuget.org/v3/index.json";

            OutputDirectory.GlobFiles("*.nupkg")
                .ForEach(package =>
                {
                    Serilog.Log.Logger.Information("Pushing package: {FileName}", Path.GetFileName(package));
                    DotNetNuGetPush(s => s
                        .SetTargetPath(package)
                        .SetSource(source)
                        .SetApiKey(NuGetApiKey));
                });
        });


    private static void CleanDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            foreach (var file in Directory.GetFiles(directoryPath))
            {
                File.Delete(file);
            }

            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                Directory.Delete(dir, true);
            }
        }
        else
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}

