using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    PlayerController Player;
    public float attackDist = 1.5f;
    [SerializeField] AttackRange defualtAttack;

    protected override void MobUpdate()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) <= attackDist)
        {
            Attack(0.5f, defualtAttack, transform.position);
        } else
        {
            Chase(Player.transform);
        }
    }
    protected override void DrawGizmos()
    {
        Draw(defualtAttack);
    }
}
