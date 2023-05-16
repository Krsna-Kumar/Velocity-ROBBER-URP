using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnim;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        playerAnim.SetBool("isWalking", playerMovement.isWalkingg);
    }
}
