using System.Collections.Generic;
using Actor;
using Cysharp.Threading.Tasks;

namespace Manager
{
    public class GamePlayManager
    {
        private readonly List<BaseActor> _actors = new();

        public async UniTask Init()
        {
            
        }

        public void CustomUpdate()
        {
            foreach (var each in _actors)
            {
                each.CustomUpdate();
            }   
        }

        public void CustomFixedUpdate()
        {
            foreach (var each in _actors)
            {
                each.CustomFixedUpdate();
            }
        }
        
        public void RegisterActor(BaseActor actor)
        {
            _actors.Add(actor);
        }

        public void RemoveActor(BaseActor actor)
        {
            _actors.Remove(actor);
        }
    }
}