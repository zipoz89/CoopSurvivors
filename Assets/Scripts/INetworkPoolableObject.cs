
using FishNet.Object;

public class INetworkPoolableObject : NetworkBehaviour
{
    public int Index;
    
    public virtual void OnGenerated(){}

    public virtual void OnSpawned(){}

    public virtual void OnReturned(){}
}
