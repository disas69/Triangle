﻿using System.Collections.Generic;
using Game.Audio.Configuration;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private List<AudioClip> _musicClips;
        private int _currentClipIndex;

        [SerializeField] private MusicStorage _musicStorage;
        [SerializeField] private bool _shuffle;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _musicClips = new List<AudioClip>(_musicStorage.MusicClips.Count);

            for (int i = 0; i < _musicStorage.MusicClips.Count; i++)
            {
                _musicClips.Add(_musicStorage.MusicClips[i]);
            }

            if (_musicClips.Count > 0)
            {
                if (_shuffle)
                {
                    Shuffle(_musicClips);
                }

                _audioSource.loop = false;
                _audioSource.clip = GetNextMusicClip();
                _audioSource.Play();
            }
        }

        private void Update()
        {
            if (_audioSource.isPlaying)
            {
                return;
            }

            _audioSource.clip = GetNextMusicClip();
            _audioSource.Play();
        }

        private AudioClip GetNextMusicClip()
        {
            if (_currentClipIndex >= _musicClips.Count)
            {
                _currentClipIndex = 0;
            }

            return _musicClips[_currentClipIndex++];
        }

        private static void Shuffle(List<AudioClip> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = Random.Range(0, n) % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}