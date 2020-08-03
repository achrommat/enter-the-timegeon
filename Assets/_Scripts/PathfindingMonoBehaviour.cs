using Chronos;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingMonoBehaviour : MonoBehaviour
    {
        public Timeline ChronosTime
        {
            get
            {
                return GetComponent<Timeline>();
            }
        }
    }
}
    
