using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public EntityController currentTarget;

    public float speed = 1;

    public float damage = 1;

    [Range(0.1f, 10)]
    public float rangeHit = 0.1f;

    public void InitTarget(EntityController target)
    {
        this.currentTarget = target;
    }

    void Update()
    {
        // On verifie si la target est valide.
        if (!IsTargetValid())
        {
            // Si non valide on vide la target
            currentTarget = null;
            // Et on detruit l'objet
            Destroy(gameObject);
            // On stop direct l'update
            return;
        }

        // On test si la distance qui separe la target et la bullet est inferieur au rangehit
        if (Vector3.Distance(currentTarget.transform.position, transform.position) <= rangeHit)
        {
            // Si oui applique les dommages
            currentTarget.ApplyDamage((int)damage);

            // On detruit le projectile
            currentTarget = null;
            Destroy(gameObject);
        }
        else
        {
            // Si rangehit pas encore atteint.
            // On calcule la direction pour faire avancer le projectile.
            Vector3 dir = currentTarget.transform.position - transform.position;
            // On fait en sorte que le vecteur direction ai 
            // une magnitude (longeur) de 1 en le normalisant.
            dir.Normalize();
            // On applique le vecteur direction. en le mutlipliant par deltatime et la speed.
            transform.position += dir * Time.deltaTime * speed;
        }
    }

    private bool IsTargetValid()
    {
        return currentTarget != null && currentTarget.IsValidEntity();
    }
}
