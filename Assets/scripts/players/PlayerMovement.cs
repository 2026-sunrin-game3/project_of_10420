using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    EntityStat stat;
    public float jumpPower = 12f;

    [SerializeField] LayerMask groundMask_;
    [SerializeField] float groundDist_ = 0.5f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        stat = GetComponent<EntityStat>();
    }
    

    public void Move(Vector2 axis)
    {
        float moveSpeed = stat.GetResultValue("moveSpeed");
        transform.Translate(axis.normalized * moveSpeed * Time.deltaTime);
    }

    public void SetVelocity(Vector2 dir)
    {
        rigid.linearVelocity = dir;
    }

    public bool Onground()
    {
        Vector2 center = transform.position + Vector3.down * groundDist_ * 0.5f;
        Vector2 size = new Vector3(0.3f, groundDist_);
        Collider2D[] cast = Physics2D.OverlapBoxAll(center, size, 0f, groundMask_);

        return cast.Length > 0;
    }

    public void jump()
    {
        if(Onground())
            SetVelocity(Vector2.up * jumpPower);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down * groundDist_ * 0.5f, new Vector3(0.3f , groundDist_));
    }
}
