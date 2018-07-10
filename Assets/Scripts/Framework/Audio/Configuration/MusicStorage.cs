using System.Collections.Generic;
using UnityEngine;

namespace Framework.Audio.Configuration
{
    [CreateAssetMenu(fileName = "MusicStorage", menuName = "Audio/MusicStorage")]
    public class MusicStorage : ScriptableObject
    {
        public List<AudioClip> MusicClips = new List<AudioClip>();
    }
}