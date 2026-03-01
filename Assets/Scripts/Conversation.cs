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
        public DialogueManager.SpeakerName Speaker;
        public float PauseTime;

        public bool EnterVertically;
        [ShowIf("EnterVertically")][AllowNesting] public Direction Direction;
        [ShowIf("EnterVertically")][AllowNesting] public float VerticalTime;

        public bool Spin;
        [ShowIf("Spin")][AllowNesting] public int SpinCount;
        [MinValue(0.1f)][ShowIf("Spin")][AllowNesting] public float SpinTime;

        public bool Swish;
        [ShowIf("Swish")][AllowNesting] public float SwishWaitInterval;
        [ShowIf("Swish")][AllowNesting] public float SwishAmplitude;
    }

    public enum Direction { Top, Bottom };

    [SerializeField] private Line[] _theWords;

    public Line[] TheWords { get => _theWords; }
}
