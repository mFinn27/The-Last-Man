using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    [SerializeField] private float lucDay = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 khoangCachDay = (collision.transform.position - transform.root.position).normalized;

            Debug.Log($"Chém trúng {collision.name}, đẩy lùi {lucDay}");

            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();

            if (enemyRb != null)
            {
                enemyRb.AddForce(khoangCachDay * lucDay, ForceMode2D.Impulse);
            }
        }
    }
}