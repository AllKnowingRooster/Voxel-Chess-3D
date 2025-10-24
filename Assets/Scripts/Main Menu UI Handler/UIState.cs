using UnityEngine;
using UI.Animate;
using System.Collections;

public class UIState : IState
{
    private Animator menuAnimator;
    private string exitAnimationTrigger;
    private string exitAnimationName;
    private string startAnimationTrigger;
    private string startAnimationName;

    public UIState(Animator animator, string startAnimationName, string startAnimationTrigger, string exitAnimationName, string exitAnimationTrigger)
    {
        this.menuAnimator = animator;
        this.startAnimationName = startAnimationName;
        this.startAnimationTrigger = startAnimationTrigger;
        this.exitAnimationName = exitAnimationName;
        this.exitAnimationTrigger = exitAnimationTrigger;
    }
    public IEnumerator ExitState()
    {
        yield return CanvasManager.instance.canvasAnimator.WaitAnimationFinish(menuAnimator, exitAnimationTrigger, exitAnimationName);
    }

    public IEnumerator StartState()
    {
        yield return CanvasManager.instance.canvasAnimator.WaitAnimationFinish(menuAnimator, startAnimationTrigger, startAnimationName);
    }
}
