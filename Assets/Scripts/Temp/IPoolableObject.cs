public interface IPoolableObject
{
    public void OnGenerated(int objectId);
    public void OnSpawned();
    public void OnReturned();
}