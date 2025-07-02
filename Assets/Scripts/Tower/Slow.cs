using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;
    private TowerTemplate towerTemplate;

    private void Awake()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
        {
            return;
        }

        Movement movement = collision.GetComponent<Movement>();

        movement.MoveSpeed -= movement.MoveSpeed * towerWeapon.Slow;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
        {
            return;
        }

        collision.GetComponent<Movement>().ResetMoveSpeed();
    }
}
