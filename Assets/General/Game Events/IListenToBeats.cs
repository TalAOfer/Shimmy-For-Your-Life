using UnityEngine;

namespace General.Game_Events
{
    public interface IListenToBeats
    {
        
        public void OnMarker(Component sender, object data)
        {
            
        }
    
        public void OnBeat(Component sender, object data)
        {
           
        }
    }
}