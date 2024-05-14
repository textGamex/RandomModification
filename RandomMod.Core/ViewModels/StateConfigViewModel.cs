using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RandomMod.Core.Services.Game;

namespace RandomMod.Core.ViewModels;

public partial class StateConfigViewModel : ObservableObject
{
    public int ManpowerMinRandom
    {
        get => _stateConfigService.ManpowerMinRandom;
        set => SetProperty(ref _stateConfigService.ManpowerMinRandom, Math.Min(value, ManpowerMaxRandom));
    }

    public int ManpowerMaxRandom
    {
        get => _stateConfigService.ManpowerMaxRandom;
        set => SetProperty(ref _stateConfigService.ManpowerMaxRandom, Math.Max(value, ManpowerMinRandom));
    }

    public int Multiplier
    {
        get => _stateConfigService.Multiplier;
        set => SetProperty(ref _stateConfigService.Multiplier, value);
    }

    private readonly StateConfigService _stateConfigService;

    public StateConfigViewModel(StateConfigService stateConfigService)
    {
        _stateConfigService = stateConfigService;
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        _stateConfigService.Change();
    }
}