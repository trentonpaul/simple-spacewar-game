using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncAnimation : NetworkBehaviour {

    [SyncVar]
    private string syncPlayerAnimation;
    private Animator animator;

    void FixedUpdate()
    {
        TransmitAnimations();
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (!isLocalPlayer && syncPlayerAnimation != null)
        {
            animator = GetComponent<Animator>();
            if (syncPlayerAnimation.Equals("GoForward"))
            {
                animator.SetInteger("MoveDirection", 1);
            }
            else if (syncPlayerAnimation.Equals("GoBackward"))
            {
                animator.SetInteger("MoveDirection", -1);
            }
            else if (syncPlayerAnimation.Equals("Neutral"))
            {
                animator.SetInteger("MoveDirection", 0);
            }
        }
    }

    [Command]
    void CmdProvideAnimationsToServer(string playerAnimation)
    {
        syncPlayerAnimation = playerAnimation;
    }

    [ClientCallback]
    void TransmitAnimations()
    {
        if (isLocalPlayer)
        {
            CmdProvideAnimationsToServer(GetComponent<Ship>().action);
        }
    }
}
