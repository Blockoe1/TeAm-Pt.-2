using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    [Serializable]
    public struct Line
    {
        [TextArea] public string OneMeaslyLine;
        public Direction Direction;
        public int SpinCount;
        [MinValue(0.1f)][AllowNesting] public float SpinTime;
    }

    public enum Direction { Top, Bottom };

    [SerializeField] private Line[] _theWords;

    public Line[] TheWords { get => _theWords; }
}
