using System.Collections;
using UnityEngine;
[System.Serializable]
public struct AttackRange
{
    public Vector2 offset, size;
    public bool drawGizmos;
}


public class PlayerBattle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EntityHealth health;
    public PlayerMovement movement;
    [SerializeField] DamageIndicator indicator;
    public EntityStat Stat;
    public float atkCool;
    public AttackRange defualtAttack;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float dashPower, dashTime;
    public bool inDash;


    void Start()
    {
        health = GetComponent<EntityHealth>();
        Stat = GetComponent<EntityStat>();
        movement = GetComponent<PlayerMovement>();

        health.OnDamage(OnHurt);
    }

    void OnHurt(EntityHealth.Context ctx)
    {
        if (inDash)
            ctx.canceled = true;

        if (ctx.canceled)
            return;

        indicator.IndicateDamage(ctx.damage, transform.position + new Vector3(0, 1), Color.blue);
    }

    void Update()
    {
        if (atkCool > 0)
            atkCool -= Time.deltaTime * (1 + Stat.GetResultValue("atkSpeed") / 100);
    }

    public void Dash(int direction)
    {
        StartCoroutine(dash_(direction));
    }
    IEnumerator dash_(int direction)
    {
        inDash = true;
        movement.SetVelocity(Vector2.right * direction * dashPower);

        yield return new WaitForSeconds(dashTime);

        movement.SetVelocity(Vector2.zero);
        inDash = false;
    }

    public void Attack()
    {
        if (atkCool > 0)
            return;
        atkCool = 0.5f;

        var col = Physics2D.OverlapBoxAll((Vector2)transform.position + defualtAttack.offset, defualtAttack.size, 0, enemyMask);

        foreach (var target in col)
        {
            EntityHealth hp = target.GetComponent<EntityHealth>();
            if (hp != null)
            {
                hp.GetDamage(Stat.GetResultValue("attackDamage"), health);
            }
        }
    }


    public void Skill1()
    {
        StartCoroutine(Skill1_());
    }

    IEnumerator Skill1_()
    {
        var atkBuf = new EntityStat.Buf
        {
          key = "attackDamage",
          mathtype = MathType.Increase,
          Value = 60  
        };
        var attackSpeedBuf = new EntityStat.Buf
        {
            key = "atkSpeed",
            mathtype = MathType.Add,
            Value = 50
        };

        Stat.bufs.Add(atkBuf);
        Stat.bufs.Add(attackSpeedBuf);
        Stat.Calc("attackDamage");
        Stat.Calc("atkSpeed");

        yield return new WaitForSeconds(5f);

        Stat.bufs.Remove(atkBuf);
        Stat.bufs.Remove(attackSpeedBuf);
        

        Stat.Calc("attackDamage");
        Stat.Calc("atkSpeed");
    }

    void Draw(AttackRange range)
    {
        if (!range.drawGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + range.offset, range.size);
    }
    void OnDrawGizmos()
    {
        Draw(defualtAttack);
    }


}
