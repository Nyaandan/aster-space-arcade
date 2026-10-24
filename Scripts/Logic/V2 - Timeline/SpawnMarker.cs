using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public abstract class SpawnMarker : Marker, INotification
{
    protected GameObject prefab;

    public abstract PropertyName id { get; }
}
