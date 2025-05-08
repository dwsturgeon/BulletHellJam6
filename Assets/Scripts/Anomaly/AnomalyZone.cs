using System.Collections.Generic;
using UnityEngine;

public class AnomalyZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private float slowMult = 1f;
    [SerializeField] private float fastMult = 1f;
    [SerializeField] private float redirectAngleSpread = 30f;
    [SerializeField] private ZoneType zoneType;

    public enum ZoneType
    {
        Slow,
        SuperSlow,
        Stop,
        Fast,
        RandFast,
        Redirect
    }

    private HashSet<Projectile> projectilesInZone = new HashSet<Projectile>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectilesInZone.Add(projectile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectilesInZone.Remove(projectile);
            if (zoneType != ZoneType.Fast && zoneType != ZoneType.RandFast)
            {
                projectile.ResetValue();
            }


        }
    }

    private void Update()
    {
        foreach (var projectile in projectilesInZone)
        {
            switch (zoneType)
            { 
                case ZoneType.Slow: projectile.SlowDown(slowMult); break;

                case ZoneType.SuperSlow: projectile.SuperSlow(); break;

                case ZoneType.Stop: projectile.Stop(); break;

                case ZoneType.Fast: projectile.SpeedUp(fastMult); break;

                case ZoneType.RandFast: projectile.SpeedUp(Random.Range(1f, fastMult)); break;

                case ZoneType.Redirect: projectile.Redirect(redirectAngleSpread); break;

                default: break;

            }

        }
    }

    private void OnDestroy()
    {
        foreach(var projectile in projectilesInZone)
        {
            projectile.ResetValue();
        }
        projectilesInZone.Clear();
    }

    public ZoneType Zone 
    {
        get => zoneType; set => zoneType = value; 
    }

}

