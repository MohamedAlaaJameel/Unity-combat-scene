using UnityEngine;
using Zenject;
using static GameInstaller;

[RequireComponent(typeof(Rigidbody))]
public class HitReceiver : MonoBehaviour
{
    [Inject] SignalBus _signalBus;

    [Header("Hit Force Settings")]
    public float legHitForce = 6f;
    public float handHitForce = 3f;

    [Header("Physics Settings")]
    public float drag = 2f; // Slows down movement over time
    public float angularDrag = 0.5f; // Slows down rotation

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning($"HitReceiver on {gameObject.name} has no Rigidbody!");
            return;
        }

        // Apply drag settings to Rigidbody
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == transform) return;

        switch (other.tag)
        {
            case "leg":
                Debug.Log("hit by leg");
                _signalBus.Fire<KickRecived>();
                ApplyHitForce(other.transform, legHitForce);
                break;

            case "hand":
                Debug.Log("hit by hand");
                _signalBus.Fire<PunchRecived>();
                ApplyHitForce(other.transform, handHitForce);
                break;

            default:
                // Debug.Log($"Unknown hit {other.tag}");
                break;
        }
    }

    void ApplyHitForce(Transform hitter, float force)
    {
        if (rb == null) return;

        Transform hitterRoot = hitter.root;
        Vector3 forceDirection = hitterRoot.forward;

        rb.AddForce(forceDirection * force, ForceMode.Impulse);

        Debug.Log($"Applied {force} force in direction {forceDirection}");
    }

}