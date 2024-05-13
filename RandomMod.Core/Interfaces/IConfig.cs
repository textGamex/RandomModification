using System.Text.Json.Serialization;

namespace RandomMod.Core.Interfaces;

public interface IConfig
{
    public bool IsChanged();
    public void Change();
    public string GetFileName();
}