using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Shader highlightShader;
    private Shader defaultShader;
    private GameObject lastHighlightedObject;
    public bool isInteracting;
    public GameObject targetObject; 

    public void Interact(out GameObject interactionObject)
    {
        RaycastHit hit;
        interactionObject = null;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                if (lastHighlightedObject != hit.collider.gameObject)
                {
                    ResetLastHighlighted();
                    defaultShader = hit.collider.gameObject.GetComponent<Renderer>().material.shader;
                    hit.collider.gameObject.GetComponent<Renderer>().material.shader = highlightShader;
                    lastHighlightedObject = hit.collider.gameObject;
                }

                interactionObject = hit.collider.gameObject;
                targetObject = interactionObject; 
                isInteracting = true; 
            }
            else
            {
                ResetLastHighlighted();
                isInteracting = false; 
            }
        }
        else
        {
            ResetLastHighlighted();
            isInteracting = false; 
        }
    }

    private void ResetLastHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.GetComponent<Renderer>().material.shader = defaultShader;
            lastHighlightedObject = null;
        }
    }
}