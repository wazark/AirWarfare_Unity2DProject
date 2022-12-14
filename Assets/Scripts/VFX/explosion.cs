using UnityEngine;

public class explosion : MonoBehaviour
{

    [Header("Explosion Settings")]
    public float explosionTime;
    public float explosionSize;

    void Start()
    {
        transform.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
        Destroy(this.gameObject, explosionTime);
        //colocar audio.
    }


    void Update()
    {

    }
}
