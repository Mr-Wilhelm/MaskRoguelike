using UnityEngine;

public class EnemyColliderHandler : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"){gameObject.GetComponentInParent<EnemyAI>().AttackPlayer(other.gameObject);}
    }
}
