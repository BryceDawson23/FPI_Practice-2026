using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Pickup Settings")]
    public ItemData item;
    public float pickupRadius = 3f;
    public KeyCode interactKey = KeyCode.E;

    private Transform player;
    private bool isInRange = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        FindPlayer();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        Vector3 actualPosition = (meshRenderer != null) ? meshRenderer.bounds.center : transform.position;

        float distance = Vector3.Distance(player.position, actualPosition);
        isInRange = distance <= pickupRadius;


        if (isInRange && Input.GetKeyDown(interactKey))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (item == null)
        {
            Debug.LogErorr($"Pickup failed: {gameObject.name} has no ItemData assigned!");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogErorr($"Pickup failed: No InventoryManager isntance found!");
            return;
        }

        if (InventoryManager.Instance.items.Count < InventoryManager.Instance.maxSlots)
        {
            Debug.Log($"Player picked up: {item.itemName}");
            InventoryManager.Instance.AddItem(item);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Inventory Full!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 debugPos = (meshRenderer != null) ? meshRenderer.bounds.center : transform.position;
        Gizmos.DrawWireSphere(debugPos, pickupRaidus);
    }
}
