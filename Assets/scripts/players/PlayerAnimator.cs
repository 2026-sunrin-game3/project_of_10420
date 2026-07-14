using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    EntityStat Stat;
    public float direction;
    void Start()
    {
        animator = GetComponent<Animator>();
        Stat = GetComponent<EntityStat>();
    }

    public void SetMoving(bool val, Vector2 axis)
    {
        animator.SetBool("isMoving", val);

        float moveRate = Stat.GetResultValue("moveSpeed") / Stat.GetBaseValue("moveSpeed");

        animator.SetFloat("moveSpeed", moveRate);

        if (val)
        {
            if (axis.x > 0)
                direction = 1;
            else if (axis.x < 0)
                direction = -1;
            
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Play(string id)
    {
        animator.Play(id);
    }
}
