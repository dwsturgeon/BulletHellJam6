using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float spriteHeight = 8f;

    private Transform[] layers;

    private void Start()
    {
        layers = new Transform[transform.childCount];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        foreach (var layer in layers) 
        {
            layer.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

            if(layer.localPosition.y <= -spriteHeight)
            {
                float highestY = GetHighestLayerY();
                layer.localPosition = new Vector3(layer.localPosition.x, highestY + spriteHeight, layer.localPosition.z);
            }
        }
    }

    private float GetHighestLayerY()
    {
        float highest = float.MinValue;

        foreach (var layer in layers)
        {
            if (layer.localPosition.y > highest)
            {
                highest = layer.localPosition.y;
            }
        }

        return highest;
    }

}
