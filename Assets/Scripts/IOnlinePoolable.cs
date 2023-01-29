using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnlinePoolable
{
    public void OnGenerated();

    public void OnSpawned();

    public void OnReturned();
}
