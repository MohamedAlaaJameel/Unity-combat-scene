using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PunshBTNSignal>();
        Container.DeclareSignal<KickBTNSignal>();
        Container.DeclareSignal<DashBTNSignal>();
        Container.DeclareSignal<KickRecived>();
        Container.DeclareSignal<PunchRecived>();


    }
    public class PunshBTNSignal { };
    public class KickBTNSignal { };
    public class DashBTNSignal { };

    /// <summary>
    /// i can add damge or more data to this signal later if needed ...
    /// </summary>
    #region recv //when kick punch has been recv..fire by hit reciver . u can play diffrent set of event like sound / hp decrease etc ..
    public abstract class BaseAttackEvent { }

    public class KickRecived: BaseAttackEvent
    {
      //  public Animator animator;
    };
    public class PunchRecived: BaseAttackEvent
    {
     //   public Animator animator;
    };
    #endregion
}