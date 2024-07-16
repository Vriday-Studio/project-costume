using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeController : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Costumes> outfits = new();

    private int currentOutfitIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ResetCostume();
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
        foreach (var costume in outfits[currentOutfitIndex].costumes)
        {
            costume.gameObject.SetActive(false);
        }

        currentOutfitIndex++;
        
        if(currentOutfitIndex >= outfits.Count)
            currentOutfitIndex = 0;
        
        foreach (var costume in outfits[currentOutfitIndex].costumes)
        {
            costume.gameObject.SetActive(true);
        }
    }

    public void ResetCostume() {
        foreach (var item in outfits)
        {
            foreach (var costume in item.costumes)
            {
                costume.gameObject.SetActive(false);
            }
        }

        currentOutfitIndex = 0;
        foreach (var costume in outfits[0].costumes)
        {
            costume.gameObject.SetActive(true);
        }
    }

}
