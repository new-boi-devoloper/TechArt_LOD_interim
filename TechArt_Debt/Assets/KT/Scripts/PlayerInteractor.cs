using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Shader highlightShader;
    private Shader defaultShader;
    private GameObject lastHighlightedObject;
    public bool isInteracting;
    public GameObject targetObject; // Объект, к которому тянется рука

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
                targetObject = interactionObject; // Устанавливаем целевой объект для IK
                isInteracting = true; // Включаем взаимодействие
            }
            else
            {
                ResetLastHighlighted();
                isInteracting = false; // Выключаем взаимодействие
            }
        }
        else
        {
            ResetLastHighlighted();
            isInteracting = false; // Выключаем взаимодействие
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