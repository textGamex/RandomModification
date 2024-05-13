using RandomMod.Core.Interfaces;

namespace RandomMod.Core.Services;

public abstract class ConfigBase : IConfig
{
    public abstract string GetFileName();
    public bool IsChanged() => _isChanged;
    public void Change() => _isChanged = true;
    private bool _isChanged;
}