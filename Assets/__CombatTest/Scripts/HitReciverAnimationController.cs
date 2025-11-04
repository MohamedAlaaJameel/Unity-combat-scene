using System;
using UnityEngine;
using Zenject;
using static GameInstaller;

public class HitReciverAnimationController : MonoBehaviour
{
    [SerializeField] private string hitBypunchName = "HitByPunch";
    [SerializeField] private string hitBykickName = "HitByKick";
    [SerializeField] private Animator animator;

    private int hityByPunchHash;
    private int hitByKickHash;

    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("u need animator..");
            return;
        }
        hityByPunchHash = Animator.StringToHash(hitBypunchName);
        hitByKickHash = Animator.StringToHash(hitBykickName);
    }
    [Inject] SignalBus _signalBus;
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

    private void OnPunchReceived()
    {
        animator.Play(hityByPunchHash);
       // animator.Play(hityByPunchHash, 0, 0f);
    }

    private void OnKickReceived()
    {
        animator.Play(hitBykickName);
        //animator.Play(hitBykickName, 0, 0f);
    }
}
