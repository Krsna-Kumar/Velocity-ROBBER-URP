using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnim;
    private PlayerMovement playerMovement;

    public float animBlendSpeed;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        //Walk Detection----If Player Pressing any button then play walk animation
        playerAnim.SetFloat("InputMagnitude", playerMovement.WalkDetect, animBlendSpeed, Time.deltaTime);
    }
}
