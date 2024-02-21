using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairWorld : MonoBehaviour
{
    [SerializeField] private RectTransform crosshair;
    Camera cachedMain;
    // Start is called before the first frame update
    void Start()
    {
        cachedMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)cachedMain.ScreenToWorldPoint(crosshair.position);
    }
}
