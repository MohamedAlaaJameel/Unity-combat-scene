using UnityEngine;
using Zenject;
using static GameInstaller;

public class ControllerOfAttackerColliders : MonoBehaviour
{
    //Animation Events to enable/disable hit colliders
    public Collider punchCollider;
    public Collider legCollider;
    private void OnKickStart()
    {
        legCollider.enabled= true;
    }
    private void OnKickEnd()
    {
        legCollider.enabled = false;
    }
    private void OnPunshStart()
    {
        punchCollider.enabled= true;
    }
    private void OnPunshEnd()
    {
        punchCollider.enabled= false;
    }
}
