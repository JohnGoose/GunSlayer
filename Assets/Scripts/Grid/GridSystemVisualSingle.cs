using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private MeshRenderer meshRenderer;

    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
