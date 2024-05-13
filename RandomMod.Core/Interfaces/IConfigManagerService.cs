namespace RandomMod.Core.Interfaces;

public interface IConfigManagerService
{
    public void SaveConfig<T>(T config) where T : IConfig;
    public T LoadConfig<T>() where T : IConfig, new();
    public void SaveAll();
}