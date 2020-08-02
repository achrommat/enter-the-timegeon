using UnityEngine;
using Chronos;

[RequireComponent(typeof(Timeline))]
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