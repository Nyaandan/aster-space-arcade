using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Logic.V2___Timeline
{
    public class EndOfLevelMarker : SpawnMarker
    {
        public override PropertyName id => new("End Of Level");
    }
}