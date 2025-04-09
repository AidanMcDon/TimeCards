using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public Bullet bullet;
    private float speed = 0f;
    public readonly string team = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.useFullKinematicContacts = true;
    }

    public void Init(Bullet bullet, Vector2 direction, string team){
        transform.up = direction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyAfterDistance(float distance)
    {
        while(true){
            if(Vector2.Distance(transform.position, new Vector2(0,0)) > distance){
                Destroy(gameObject);
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }


}
