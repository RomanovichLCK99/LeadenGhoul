using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private TimeManager timeManager = null;

    [Space(10f)]
    [Header("Variables")]
    [SerializeField] private float dashVelocity = 10f;
    [SerializeField] private float dashDuration = 0.5f;

    [Space(10)]

    [SerializeField] private float slowdownDuration = 5f;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool canSlowdown = true;
    [SerializeField] private float dashCoolDown = 3f;

    [Space(10)]
    [SerializeField] private float slowDownCost = 5f;

    float dashCoolDownTimer = 0f;
    float dashTimer = 0f;
    bool isDashing = false;
    private void Update()
    {
        if (Input.GetButtonDown("Dash") && dashCoolDownTimer <= 0 && UIController.instance.slowDownValue - slowDownCost >= 0)
        {
            doDash();
        }

        if (dashTimer <= 0)
        {
            PlayerMovement.instance.isDashing = false;
            rb.velocity = Vector3.zero;
        }
        else
        {
            dashTimer -= Time.deltaTime;
        }

        if (dashCoolDownTimer > 0)
        {
            dashCoolDownTimer -= Time.unscaledDeltaTime;
        }

        if (dashCoolDownTimer <= 0) isDashing = false;

    }


    private void doDash()
    {
        isDashing = true;
        dashCoolDownTimer = dashCoolDown;
        UIController.instance.slowDownValue -= slowDownCost;

        PlayerMovement.instance.isDashing = true;

        Vector2 dashDir = PlayerMovement.instance.playerInput.normalized;

        rb.AddForce(dashDir * dashVelocity, ForceMode2D.Impulse);
        dashTimer = dashDuration;

        if (slowdownDuration != 0f && canSlowdown)
        {
            timeManager.DoSlowmotion(slowdownDuration, transitionDuration);
        }
        else
        {
            Debug.LogWarning("Don't divide by 0!");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing) rb.velocity = Vector2.zero;
    }
}
