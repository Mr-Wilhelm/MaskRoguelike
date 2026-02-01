using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField]
    private InputActionReference interactRef;

    [Header("Interact Variables")]

    public bool canVisitShop = false;

    [Header("Interact UI")]
    [SerializeField]
    private Canvas playerCanvas;

    [SerializeField]
    private Image interactImage;

    [SerializeField]
    private ShopScript shopScript;

    private void OnEnable()
    {
        interactRef.action.Enable();
        interactRef.action.started += OnInteract;
    }

    private void OnDisable()
    {
        interactRef.action.started -= OnInteract;
        interactRef.action.Disable();
    }

    private void Update()
    {
        if (canVisitShop)
            interactImage.enabled = true;
        else
            interactImage.enabled = false;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(!canVisitShop) { return; }

        shopScript.EnterShop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Merchant")
            canVisitShop = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Merchant")
            canVisitShop = false;
    }
}
