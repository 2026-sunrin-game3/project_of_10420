using UnityEngine;
using System.Collections.Generic;
using System;

public class EntityHealth : MonoBehaviour
{
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EntityStat Stat;
    public float health, maxhealth;
    public bool isDeath;

    public struct Context
    {
        public float damage;
        public EntityHealth attacker;
        public bool canceled;
    }

    List<Action<Context>> onDamageEv = new();
    List<Action<Context>> onGiveDamageEv = new();
    List<Action<Context>> onDeathEv = new();

    void Start()
    {
        Stat = GetComponent<EntityStat>();
        RestHealth();
    }

    public void RestHealth()
    {
        health = maxhealth;
    }

    public void OnDamage(Action<Context> action)
    {
        onDamageEv.Add(action);
    }

    public void OnGiveDamage(Action<Context> action)
    {
        onGiveDamageEv.Add(action);
    }

    public void OnDeath(Action<Context> action)
    {
        onDeathEv.Add(action);
    }

    public void GetDamage(float damage, EntityHealth attacker = null)
    {

        if (isDeath)
            return;

        Context ctx = new Context();
        ctx.damage = damage;
        ctx.attacker = attacker;

        float critPer = 0, critMul = 0, inc = 0;

        foreach (var c in onDamageEv)
        {
            c.Invoke(ctx);
        }
        
        if (attacker != null)
        {
            critPer = attacker.Stat.GetResultValue("critPer");
            critMul = attacker.Stat.GetResultValue("critMul");
            inc = attacker.Stat.GetResultValue("increaseDamage");

            foreach (var c in attacker.onGiveDamageEv)
            {
            c.Invoke(ctx);
            }
        }

        if (ctx.canceled)
        {
            return;
        }

        float dmg = ctx.damage * (1 + Stat.GetResultValue("hurtDamage")/100) * ( 1 + inc / 100);

        if (UnityEngine.Random.Range(0, 100) <= critPer)
            dmg *= 1 + critMul / 100;

        health -= dmg;

        if (health <= 0)
        {
            isDeath = true;

            foreach (var c in onDeathEv)
            {
                c.Invoke(ctx);
            }
        }

        
    }
}
