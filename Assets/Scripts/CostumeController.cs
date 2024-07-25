using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostumeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costumeNameUI;
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
        costumeNameUI.text = outfits[currentOutfitIndex].costumeName;
    }
    
    public void PreviousCostume() {
        foreach (var costume in outfits[currentOutfitIndex].costumes)
        {
            costume.gameObject.SetActive(false);
        }

        currentOutfitIndex--;
        
        if(currentOutfitIndex < 0)
            currentOutfitIndex = outfits.Count - 1;
        
        foreach (var costume in outfits[currentOutfitIndex].costumes)
        {
            costume.gameObject.SetActive(true);
        }
        costumeNameUI.text = outfits[currentOutfitIndex].costumeName;
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
        costumeNameUI.text = outfits[currentOutfitIndex].costumeName;
    }

}
