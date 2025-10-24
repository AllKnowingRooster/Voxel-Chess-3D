using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Animate{
    public class CanvasAnimator
    {
        public IEnumerator WaitAnimationFinish(Animator animator,string animationTrigger, string animationName)
        {
            animator.SetTrigger(animationTrigger);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

        }
    }
}

