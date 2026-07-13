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
    public EntityStat Stat;
    public float atkCool;
    public AttackRange defualtAttack;
    [SerializeField] LayerMask enemyMask;


    void Start()
    {
        health = GetComponent<EntityHealth>();
        Stat = GetComponent<EntityStat>();
    }

    void Update()
    {
        if (atkCool > 0)
            atkCool -= Time.deltaTime * (1 + Stat.GetResultValue("atkSpeed") / 100);
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
