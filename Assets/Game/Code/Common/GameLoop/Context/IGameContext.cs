using Cysharp.Threading.Tasks;

namespace Game.Code.Common.GameLoop.Context
{
    public interface IGameContext
    {
        UniTask Load();
        UniTask Unload();
    }
}