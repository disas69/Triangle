using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.Utils
{
    [RequireComponent(typeof(PostProcessingBehaviour))]
    public class PostProcessingProfileHandler : MonoBehaviour
    {
        [SerializeField] private PostProcessingProfile _defaultProfile;
        [SerializeField] private List<PlatformProfile> _platformProfiles;

        private void Awake()
        {
            var postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
            if (postProcessingBehaviour != null)
            {
                var platformProfile = _platformProfiles.Find(p => p.Platform == Application.platform);

                postProcessingBehaviour.profile = platformProfile != null
                    ? platformProfile.Profile
                    : _defaultProfile;
            }
        }
    }

    [Serializable]
    public class PlatformProfile
    {
        public string Name;
        public RuntimePlatform Platform;
        public PostProcessingProfile Profile;
    }
}