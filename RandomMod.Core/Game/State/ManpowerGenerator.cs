using MathNet.Numerics.Random;
using RandomMod.Core.Services;
using RandomMod.Core.Services.Game;

namespace RandomMod.Core.Game.State;

public sealed partial class StateGenerator
{
    private class ManpowerGenerator
    {
        private readonly Dictionary<string, List<int>> _manpowerMap;
        private readonly StateConfigService _stateConfig;
        private readonly MersenneTwister _random = new();

        public ManpowerGenerator(GameResourcesService resourcesService, StateConfigService stateConfig)
        {
            _stateConfig = stateConfig;
            _manpowerMap = new Dictionary<string, List<int>>(resourcesService.CountryStateCount.Count);

            foreach (var (countryTag, totalState) in resourcesService.CountryStateCount)
            {
                _manpowerMap.Add(countryTag, GetManpowerList(totalState));
            }
        }

        private List<int> GetManpowerList(int totalState)
        {
            var targetSum = GetTotalManpower(totalState);
            if (totalState == 1)
            {
                // 如果只需要一个数，则直接返回包含该数的列表  
                return [targetSum];
            }

            var list = new List<int>(totalState);
            var sum = 0;
            // 生成count-1个随机数  
            for (var i = 0; i < totalState - 1; i++)
            {
                // 计算剩余可分配的值
                var remainingSum = targetSum - sum;
                // 确保每个数至少是1（可根据需要调整）  
                var maxValue = Math.Max(1, remainingSum / (totalState - i - 1));
                // 生成一个介于1和maxValue之间的随机数  
                var randomValue = _random.Next(1, maxValue + 1);
                // 添加到列表中，并更新剩余和  
                list.Add(randomValue);
                sum += randomValue;
                targetSum -= randomValue;
            }

            // 添加最后一个数，其值为剩余的targetSum
            list.Add(targetSum);
            return list;
        }

        private int GetTotalManpower(int totalState)
        {
            var sum = 0;
            for (var i = 0; i < totalState; i++)
            {
                var random = _random.Next(_stateConfig.ManpowerMinRandom, _stateConfig.ManpowerMaxRandom + 1);
                sum += (int)(Math.Log10(totalState) * 50000 + random);
            }
            return sum;
        }

        public int GetManpowerByCountry(string countryTag)
        {
            if (_manpowerMap.TryGetValue(countryTag, out var list))
            {
                var result = list[^1];
                list.RemoveAt(list.Count - 1);
                return result;
            }
            return _random.Next(100000, 1300000);
        }
    }
}
