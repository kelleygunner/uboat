using Cysharp.Threading.Tasks;

namespace Game.Code.Common.GameLoop.Context
{
    public interface IGameContextLoadable
    {
        UniTask<bool> ChangeSingleContext(IGameContext context);
        UniTask AddCommonContext(IGameContext context);
    }
}
