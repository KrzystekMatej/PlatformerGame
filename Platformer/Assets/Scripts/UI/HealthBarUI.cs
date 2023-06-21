using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthBarUI : MonoBehaviour
{
    private List<SpriteElementUI> heartImages;
    [SerializeField]
    private Sprite fillHeart, emptyHeart;
    [SerializeField]
    private SpriteElementUI heartPrefab;

    public void Initialize(int maxHealth)
    {
        heartImages = new List<SpriteElementUI>();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < maxHealth; i++)
        {
            SpriteElementUI heart = Instantiate(heartPrefab);
            heart.transform.SetParent(transform, false);
            heartImages.Add(heart);
        }
    }
    
    public void SetHealth(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].Set(i < currentHealth ? fillHeart : emptyHeart);
        }
    }
}
