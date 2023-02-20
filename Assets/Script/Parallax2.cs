using UnityEngine;

public class Parallax2 : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // velocità di scorrimento dello sfondo
    public float offset = 0.0f; // offset iniziale dello sfondo

    private Vector2 savedOffset; // offset salvato per ripristinare la posizione dello sfondo in caso di riproduzione interrotta

    private void Start()
    {
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
    }

    private void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(x + this.offset, savedOffset.y);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    private void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
