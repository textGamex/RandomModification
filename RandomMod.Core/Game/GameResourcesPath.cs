using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using RandomMod.Core.Extensions;

namespace RandomMod.Core.Game;

/// <summary>
/// 管理游戏资源路径
/// </summary>
public sealed class GameResourcesPath
{
    public string GameRootFolderPath { get; }
    public string ModRootFolderPath { get; }
    public string GameLocPath { get; }
    public string ModLocPath { get; }
    public IReadOnlyList<string> StateCategoriesFilePath { get; }
    public IReadOnlyList<string> CountriesDefineFilePath { get; }
    public IReadOnlyList<string> IdeaFilesPath { get; }
    public IReadOnlyList<string> IdeaTagsFilePath { get; }
    public IReadOnlyList<string> TechnologyFilesPath { get; }
    public IReadOnlyList<string> CountriesTagFilePath { get; }
    public IReadOnlyList<string> IdeologiesFilePath { get; }
    public IReadOnlyList<string> AutonomousStateFilesPath { get; }
    public IReadOnlyList<string> CharactersFilesPath { get; }
    public IReadOnlyList<string> OobFilesPath { get; }
    public IReadOnlyList<string> BuildingsFilePathList { get; }
    public IReadOnlyList<string> ResourcesTypeFilePathList { get; }
    public IReadOnlyList<string> StatesFilePathList { get; }
    //public int FileSum { get; }

    private readonly Descriptor? _descriptor;
    private readonly ILogger<GameResourcesPath> _logger = App.Current.GetRequiredService<ILogger<GameResourcesPath>>();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameRootFolderPath"></param>
    /// <param name="modRootFolderPath"></param>
    /// <param name="descriptor"></param>
    /// <exception cref="DirectoryNotFoundException">需要的文件夹不存在</exception>
    /// <exception cref="FileNotFoundException"></exception>
    public GameResourcesPath(string gameRootFolderPath, string modRootFolderPath = "", Descriptor? descriptor = null)
    {
        if (!Directory.Exists(gameRootFolderPath))
        {
            throw new DirectoryNotFoundException($"文件夹不存在: {gameRootFolderPath}");
        }

        GameRootFolderPath = gameRootFolderPath;
        ModRootFolderPath = modRootFolderPath;
        _descriptor = descriptor;
        GameLocPath = GetLocPath(GameRootFolderPath);
        ModLocPath = string.IsNullOrEmpty(modRootFolderPath) ? string.Empty : GetLocPath(ModRootFolderPath);

        StateCategoriesFilePath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, ScriptKeyWords.StateCategory)).ToList();
        CountriesDefineFilePath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(ScriptKeyWords.History, "countries")).ToList();
        IdeaFilesPath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, ScriptKeyWords.Ideas)).ToList();
        IdeaTagsFilePath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, "idea_tags")).ToList();
        TechnologyFilesPath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, "technologies")).ToList();
        AutonomousStateFilesPath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, "autonomous_states")).ToList();
        CharactersFilesPath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, ScriptKeyWords.Characters)).ToList();
        OobFilesPath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(ScriptKeyWords.History, "units")).ToList();
        CountriesTagFilePath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, "country_tags")).ToList();
        IdeologiesFilePath = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, ScriptKeyWords.Ideologies)).ToList();
        BuildingsFilePathList = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, ScriptKeyWords.Buildings)).ToList();
        StatesFilePathList = GetAllFilePriorModByRelativePathForFolder(Path.Combine(ScriptKeyWords.History, Key.States)).ToList();
        ResourcesTypeFilePathList = GetAllFilePriorModByRelativePathForFolder(Path.Combine(Key.Common, "resources")).ToList();

        //FileSum = GetFileSum();
    }

    //private int GetFileSum()
    //{
    //    return StateCategoriesFilePath.Count + CountriesDefineFilePath.Count +
    //        IdeaFilesPath.Count + IdeaTagsFilePath.Count + TechnologyFilesPath.Count +
    //        CountriesTagFilePath.Count + IdeologiesFilePath.Count +
    //        BuildingsFilePathList.Count + ResourcesTypeFilePathList.Count +
    //        StatesFilePathList.Count + AutonomousStateFilesPath.Count +
    //        CharactersFilesPath.Count + OobFilesPath.Count + 1; // 这个 1 是 ProvincesDefinitionFilePath 文件
    //}

    private static string GetLocPath(string rootPath)
    {
        return Path.Combine(rootPath, "localisation");
    }

    /// <summary>
    /// 根据相对路径获得游戏或者Mod文件的绝对路径, 优先Mod
    /// </summary>
    /// <remarks>
    /// 注意: 此方法会忽略mod描述文件中的 replace_path 指令
    /// </remarks>
    /// <param name="fileRelativePath">根目录下的相对路径</param>
    /// <exception cref="FileNotFoundException">游戏和mod中均不存在</exception>
    /// <returns>文件路径</returns>
    private string GetFilePathPriorModByRelativePath(string fileRelativePath)
    {
        var modFilePath = Path.Combine(ModRootFolderPath, fileRelativePath);
        if (_descriptor is not null && File.Exists(modFilePath))
        {
            return modFilePath;
        }

        var gameFilePath = Path.Combine(GameRootFolderPath, fileRelativePath);
        if (File.Exists(gameFilePath))
        {
            return gameFilePath;
        }
        
        throw new FileNotFoundException($"在Mod和游戏中均找不到目标文件 '{fileRelativePath}'");
    }

    /// <summary>
    /// 获得所有应该加载的文件绝对路径, Mod优先, 遵循 replace_path 指令
    /// </summary>
    /// <param name="folderRelativePath"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    private IEnumerable<string> GetAllFilePriorModByRelativePathForFolder(string folderRelativePath)
    {
        _logger.LogDebug("正在加载文件夹: {Path}", folderRelativePath);

        var modFolder = Path.Combine(ModRootFolderPath, folderRelativePath);
        var gameFolder = Path.Combine(GameRootFolderPath, folderRelativePath);

        if (!Directory.Exists(gameFolder))
        {
            throw new DirectoryNotFoundException($"找不到文件夹 {gameFolder}");
        }

        if (_descriptor is null || !Directory.Exists(modFolder))
        {
            return GetAllFilePathForFolder(gameFolder);
        }

        if (_descriptor.ReplacePaths.Contains(folderRelativePath))
        {
            _logger.LogDebug("MOD文件夹已完全替换游戏文件夹: \n\t {GamePath} => {ModPath}", gameFolder.ToFilePath(), modFolder.ToFilePath());
            return GetAllFilePathForFolder(modFolder);
        }

        var gameFilesPath = GetAllFilePathForFolder(gameFolder);
        var modFilesPath = GetAllFilePathForFolder(modFolder);
        return RemoveFileOfEqualName(gameFilesPath, modFilesPath);
    }

    /// <summary>
    /// 获得一个文件夹下的所有文件
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    private static IEnumerable<string> GetAllFilePathForFolder(string folderPath)
    {
        Debug.Assert(Directory.Exists(folderPath), $"文件夹不存在 {folderPath}");
        return Directory.GetFiles(folderPath);
    }

    /// <summary>
    /// 移除重名文件, 优先移除游戏本体文件
    /// </summary>
    /// <param name="gameFilePathList"></param>
    /// <param name="modFilePathList"></param>
    /// <returns></returns>
    /// <exception cref="FileFormatException"></exception>
    private static IEnumerable<string> RemoveFileOfEqualName(IEnumerable<string> gameFilePathList, IEnumerable<string> modFilePathList)
    {
        var set = new HashSet<string>();

        // 优先读取Mod文件
        foreach (var filePath in modFilePathList.Concat(gameFilePathList))
        {
            var fileName = Path.GetFileName(filePath) ?? throw new FileFormatException($"无法得到文件名: {filePath}");
            if (set.Add(fileName))
            {
                yield return filePath;
            }
        }
    }

    private static class Key
    {
        public const string States = "states";
        public const string Map = "map";
        public const string Common = "common";
    }
}