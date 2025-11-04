using UnityEngine;
using Zenject;
using static GameInstaller;

public class CombatAnimations : MonoBehaviour
{
    public Animator animator;
    [Inject] private SignalBus _signalBus;

    [SerializeField] private string idleName = "Idle";
    [SerializeField] private string punchName = "Punch";
    [SerializeField] private string kickName = "Kick";

    private int idleHash;
    private int punchHash;
    private int kickHash;

    private void Start()
    {
        if (!animator)
        {
            Debug.LogError("U need to set player animator");
            return;
        }

        punchHash = Animator.StringToHash(punchName);
        kickHash = Animator.StringToHash(kickName);

        bool idleExists = false;
        bool punchExists = false;
        bool kickExists = false;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == idleName) idleExists = true;
            if (clip.name == punchName) punchExists = true;
            if (clip.name == kickName) kickExists = true;
        }

        if (!idleExists) Debug.LogError($"{idleName} clip not found in Animator");
        if (!punchExists) Debug.LogError($"{punchName} clip not found in Animator");
        if (!kickExists) Debug.LogError($"{kickName} clip not found in Animator");
    }

    private void OnKick()
    {
        animator.Play(kickHash, 0, 0f);
    }

    private void OnPunsh()
    {
        animator.Play(punchHash, 0, 0f);
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<PunshBTNSignal>(OnPunsh);
        _signalBus.Subscribe<KickBTNSignal>(OnKick);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PunshBTNSignal>(OnPunsh);
        _signalBus.Unsubscribe<KickBTNSignal>(OnKick);
    }
}
