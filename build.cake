#tool nuget:?package=Microsoft.TestPlatform&version=15.9.0
#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var deployDir = "deploy/";
var solutionFile = "./src/SoulsOrganizer.sln";
var gitVersion = GitVersion();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("UpdateAssemblyInfo")
    .Does(() =>
{
    Information(gitVersion.SemVer);
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
    });
});

Task("CleanAll")
    .Does(() =>
{
    CleanDirectories("./src/**/obj/**");
    CleanDirectories("./src/**/bin/**");
	CleanDirectories("./src/packages");
});

Task("Restore-NuGet-Packages")    
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionFile);
});

Task("ReBuild")
    .IsDependentOn("CleanAll")
	.IsDependentOn("Build")
    .Does(() =>
{
});

Task("Run-Unit-Tests")    
    .Does(() =>
{
    VSTest("./src/**/bin/release/*tests.dll");
});

Task("Test")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
});

Task("Deploy")
    .IsDependentOn("UpdateAssemblyInfo")
    .IsDependentOn("CleanAll")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    EnsureDirectoryExists("./dist");
    MSBuild(solutionFile, settings => settings
        .SetConfiguration(Argument("configuration", "release"))
        .WithProperty("PublishDir", deployDir)
        .WithProperty("ApplicationVersion", gitVersion.SemVer + ".0")
    );
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
