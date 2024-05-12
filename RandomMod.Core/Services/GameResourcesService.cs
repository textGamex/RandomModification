using CWTools.Process;

namespace RandomMod.Core.Services;

public sealed class GameResourcesService
{
    public List<Node> States { get; set; } = [];
    public Dictionary<string, int> CountryStateCount { get; set; } = [];
}