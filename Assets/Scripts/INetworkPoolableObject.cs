
using FishNet.Object;

public class INetworkPoolableObject : NetworkBehaviour
{
    public virtual void OnGenerated(){}

    public virtual void OnSpawned(){}

    public virtual void OnReturned(){}
}
