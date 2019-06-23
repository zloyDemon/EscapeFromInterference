using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<in T>
{
    void Enter(T stateObj);
    void Update();
    void Exit();
}
