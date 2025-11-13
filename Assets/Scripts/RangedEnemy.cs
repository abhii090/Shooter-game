using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;
    public float fireRate;
    private float timeTofire;
    public Transform firingPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeTofire = 0F;
    }

    private void Update()
    {
        if (!target)
            GetTarget();
        else
            RotateTowardsTarget();

        if (target != null && Vector2.Distance(target.position, transform.position) <= distanceToStop)
            Shoot();
    }

    private void Shoot()
    {
        if (timeTofire <= 0f)
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);

            // 🔊 Enemy shooting sound
            if (AudioManager.instance != null)
                AudioManager.instance.PlayShoot();

            timeTofire = fireRate;
        }
        else
        {
            timeTofire -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
            rb.velocity = transform.up * speed;
        else
            rb.velocity = Vector2.zero;
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            target = playerObj.transform;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            target = null;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            // 🔊 Enemy death sound
            if (AudioManager.instance != null)
                AudioManager.instance.PlayEnemyDeath();

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

