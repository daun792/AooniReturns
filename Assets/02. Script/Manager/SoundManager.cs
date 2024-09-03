using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public enum AudioType
{
    BGM,
    SFX
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : Manager
{
    public struct VolumeData
    {
        public bool isMuted;

        public readonly float Calculated => isMuted ? 0f : 50f;

        public VolumeData(bool _isMuted)
        {
            isMuted = _isMuted;
        }
    }

    public class VolumeAccessor
    {
        readonly SoundManager Sound = App.Manager.Sound;

        public bool BGM
        {
            get => Sound.BGMData.isMuted;
            set
            {
                Sound.BGMData.isMuted = value;
                Sound.SetBGMVolume();
            }
        }

        public bool SFX
        {
            get => Sound.SFXData.isMuted;
            set
            {
                Sound.SFXData.isMuted = value;
                Sound.SetSFXVolume();
            }
        }
    }

    [Header("Audio Clips")]
    [SerializeField] Sound[] BGM = null;
    [SerializeField] Sound[] SFX = null;

    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource sfxPlayer = null;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer mixer;

    Dictionary<string, AudioClip> dic_BGM;
    Dictionary<string, AudioClip> dic_SFX;

    private VolumeData BGMData;
    private VolumeData SFXData;

    [HideInInspector] public VolumeAccessor Volume;

    protected override void Awake()
    {
        base.Awake();

        Volume = new();

        dic_BGM = new Dictionary<string, AudioClip>();
        dic_SFX = new Dictionary<string, AudioClip>();

        foreach (Sound sound in BGM)
        {
            dic_BGM.Add(sound.name, sound.clip);
        }

        foreach (Sound sound in SFX)
        {
            dic_SFX.Add(sound.name, sound.clip);
        }
    }

    private void Start()
    {
        var setting = App.Data.Setting.Sound;

        BGMData = new(setting.BGMMuted);
        SFXData = new(setting.SFXMuted);

        SetBGMVolume();
        SetSFXVolume();
    }

    #region Play & Stop Sound

    #region BGM
    public void PlayBGM(string _name)
    {
        if (!dic_BGM.TryGetValue(_name, out var clip))
        {
            Debug.LogError("ERROR: Failed to play BGM. Unable to find " + _name);
            return;
        }

        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
        }

        bgmPlayer.clip = clip;

        bgmPlayer.Play();
        bgmPlayer.DOFade(0.5f, 0.5f).SetEase(Ease.Linear);
    }

    public void ResumeBGM()
    {
        if (bgmPlayer.isPlaying) return;

        bgmPlayer.Play();
        bgmPlayer.DOFade(0.5f, 0.5f).SetEase(Ease.Linear);
    }

    public void StopBGM()
    {
        bgmPlayer.DOFade(0f, 0.5f).OnComplete(() => bgmPlayer.Stop());
    }
    #endregion

    #region SFX
    public void PlaySFX(string _name)
    {
        if (!dic_SFX.TryGetValue(_name, out var clip))
        {
            Debug.LogError("ERROR: Failed to play SFX. Unable to find " + _name);
            return;
        }

        sfxPlayer.clip = clip;

        sfxPlayer.Play();
    }

    public void StopSFX()
    {
        sfxPlayer.Stop();
    }

    public bool IsPlayingSFX()
    {
        return sfxPlayer.isPlaying;
    }
    #endregion

    #endregion

    #region Set Volume
    private void SetBGMVolume() => SetVolume("BGM", BGMData.Calculated);
    private void SetSFXVolume() => SetVolume("SFX", SFXData.Calculated);

    private void SetVolume(string _param, float _value)
    {
        if (_value < 0.001f)
            _value = 0.00001f;

        mixer.SetFloat(_param, ValueToDecibel(_value));
    }

    private float ValueToDecibel(float value) => Mathf.Log10(value * 2) * 20;
    #endregion

    #region Set Mute
    public bool ToggleMute(AudioType _type)
    {
        bool muted = IsMuted(_type);
        MuteVolume(_type, !muted);
        return !muted;
    }

    public bool IsMuted(AudioType _type) => _type switch
    {
        AudioType.BGM => BGMData.isMuted,
        AudioType.SFX => SFXData.isMuted,
        _ => false,
    };

    public void MuteVolume(AudioType _type, bool _mute)
    {
        switch (_type)
        {
            case AudioType.BGM:
                BGMData.isMuted = _mute;
                SetBGMVolume();
                return;

            case AudioType.SFX:
                SFXData.isMuted = _mute;
                SetSFXVolume();
                return;
        }
    }
    #endregion
}