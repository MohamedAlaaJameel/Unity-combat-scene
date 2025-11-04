using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static GameInstaller;

public class SoundManager : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;

    [Header("Kick Sounds")]
    [SerializeField] private List<AudioClip> OnkickedClips;

    [Header("Punch Sounds")]
    [SerializeField] private List<AudioClip> OnpunchedClips;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<KickRecived>(OnKickReceived);
        _signalBus.Subscribe<PunchRecived>(OnPunchReceived);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<KickRecived>(OnKickReceived);
        _signalBus.Unsubscribe<PunchRecived>(OnPunchReceived);
    }

    private void OnKickReceived()
    {
        PlayRandomClip(OnkickedClips);
    }
    private void OnPunchReceived()
    {
        PlayRandomClip(OnpunchedClips);
    }

    private void PlayRandomClip(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0) return;

        int index = Random.Range(0, clips.Count);
        audioSource.PlayOneShot(clips[index]);
    }
}
