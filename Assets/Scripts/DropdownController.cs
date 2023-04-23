using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DropdownController : MonoBehaviour
{
    public List<TMPro.TMP_Dropdown> dropdowns = new List<TMPro.TMP_Dropdown>();

    private List<int>  selectedOptions = new List<int>();

    public bool CheckIfAllDropdownsHasSetValue()
    {
        bool b = false;
        for (int i = 0; i < dropdowns.Count; i++)
        {
            if(dropdowns[i].value > 0)
            {
                b = true;
            }
            else
            {
                b = false;
            }
        }
        return b;
    }

    public void InitialiseAllDropdowns(List<TMPro.TMP_Dropdown> _dropdowns)
    {
        dropdowns = _dropdowns;
        // Attach event listeners to each dropdown
        for (int i = 0; i < dropdowns.Count; i++)
        {
            int dropdownIndex = i; // Capture the index in a local variable for use in the event listener
            dropdowns[i].onValueChanged.AddListener((value) =>
            {
                OnDropdownValueChanged(dropdownIndex, value);
            });
        }
    }

    public Dictionary<int, int> lastOption = new Dictionary<int, int>();

    private void OnDropdownValueChanged(int dropdownIndex, int value)
    {
        // Check if the selected value is already selected by another dropdown
        if (selectedOptions.Contains(value))
        {
            // Deselect the option in the current dropdown
            dropdowns[dropdownIndex].value = 0;
        }
        else
        {
            int lastValue = -1;
            lastOption.TryGetValue(dropdownIndex, out lastValue);
            // Remove the previous selected option (if any) and add the new one
            if (lastValue > -1)
            {
                selectedOptions.Remove(lastValue);
            }
            // Update the selected options list and deselect the same option in other dropdowns
            if (lastOption.ContainsKey(dropdownIndex))
            {
                lastOption[dropdownIndex] = value;
            }
            else
            {
                lastOption.Add(dropdownIndex, value);
            }
            selectedOptions.Add(value);
            for (int i = 0; i < dropdowns.Count; i++)
            {
                if (i != dropdownIndex && dropdowns[i].value == value)
                {
                    dropdowns[i].value = 0;
                }
            }
        }
        if (CheckIfAllDropdownsHasSetValue())
        {
            CardGameManagerUI.instance.SendRankButton.interactable = true;
        }
    }
}

