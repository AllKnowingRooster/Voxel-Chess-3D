using System.Collections;
using UI.Animate;
using UnityEngine;

public interface IState
{
    IEnumerator StartState();
    IEnumerator ExitState();
}
