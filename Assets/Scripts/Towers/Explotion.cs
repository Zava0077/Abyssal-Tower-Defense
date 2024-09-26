using System.Collections;
using UnityEngine;

public class Explotion : MonoBehaviour, IMeshHolder
{
    public MeshHolder MeshHolder { get; set; }

    public Projectile producer { get; set; }//продюсеры не нужны лужам и взрыву. кто код писал бл€ть?
    public Damage damage;
    private void OnEnable()
    {
        StartCoroutine(DeathSentence()); 
        //Entity.onEntityDeath += OnEntityDeath;
    }
    //private void OnDisable() => 
    //    Entity.onEntityDeath -= OnEntityDeath;
    private IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    //void OnEntityDeath(Entity sender)
    //{
    //    if (producer.prevEnemy != null && producer.prevEnemy.Count > 0)
    //        producer.prevEnemy.Remove(sender);
    //}
    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>(); //
        if(otherEntity)
        {
            otherEntity.GetDamage(damage);
        }
    }
}
