using UnityEngine;
using Chronos;

public class ChronosMonoBehaviour : MonoBehaviour
{
    public Timeline ChronosTime
    {
        get
        {
            return GetComponent<Timeline>();
        }
    }
}