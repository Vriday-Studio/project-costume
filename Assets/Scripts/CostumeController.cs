using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeController : MonoBehaviour, IInteractable
{
    [SerializeField] private List<SkinnedMeshRenderer> outfits = new();

    private int currentOutfitIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in outfits)
        {
            item.gameObject.SetActive(false);
        }
        outfits[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        NextCostume();
    }

    public void NextCostume() {
        outfits[currentOutfitIndex].gameObject.SetActive(false);
        currentOutfitIndex++;
        if(currentOutfitIndex >= outfits.Count)
            currentOutfitIndex = 0;
        
        outfits[currentOutfitIndex].gameObject.SetActive(true);
    }
}
