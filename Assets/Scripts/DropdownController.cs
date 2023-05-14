using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DropdownController : MonoBehaviour
{
    public List<TMPro.TMP_Dropdown> dropdowns = new List<TMPro.TMP_Dropdown>();

    private List<int>  selectedOptions = new List<int>();

    public static DropdownController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        List<int> previousRanks = new List<int>();
        if (dropdowns.Count > 0)
        {
            foreach (int x in selectedOptions)
            {
                previousRanks.Add(x);
            }
        }

        selectedOptions.Clear();
        dropdowns.Clear();
        lastOption.Clear();

        selectedOptions = new List<int>();
        dropdowns = new List<TMPro.TMP_Dropdown>();
        lastOption = new Dictionary<int, int>();

        dropdowns = _dropdowns;
        // Attach event listeners to each dropdown
        for (int i = 0; i < dropdowns.Count; i++)
        {
            int dropdownIndex = i; // Capture the index in a local variable for use in the event listener
            dropdowns[i].onValueChanged.AddListener((value) =>
            {
                OnDropdownValueChanged(dropdownIndex, value);
            });
            if (previousRanks.Count == dropdowns.Count)
            {
                selectedOptions.Add(previousRanks[i]);
            }
        }
        if (previousRanks.Count == dropdowns.Count)
        {
            for (int i = 0; i < dropdowns.Count; i++)
            {
                if(previousRanks[i]!=0)
                    dropdowns[i].SetValueWithoutNotify(previousRanks[i]);
            }
        }

    }

    public Dictionary<int, int> lastOption = new Dictionary<int, int>();

    private void OnDropdownValueChanged(int dropdownIndex, int value)
    {
        StartCoroutine(WithDelay(dropdownIndex, value));
    }

    private IEnumerator WithDelay(int dropdownIndex, int value)
    {
        yield return new WaitForSeconds(0.01f);
        // Check if the selected value is already selected by another dropdown
        if (selectedOptions.Contains(value))
        {
            // Deselect the option in the current dropdown
            dropdowns[dropdownIndex].value = 0;
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
            int lastValue = -1;
            lastOption.TryGetValue(dropdownIndex, out lastValue);
            yield return new WaitForSeconds(0.01f);
            // Remove the previous selected option (if any) and add the new one
            if (lastValue > -1)
            {
                selectedOptions.Remove(lastValue);
            }
            yield return new WaitForSeconds(0.01f);
            // Update the selected options list and deselect the same option in other dropdowns
            if (lastOption.ContainsKey(dropdownIndex))
            {
                yield return new WaitForSeconds(0.01f);
                lastOption[dropdownIndex] = value;
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
                if(!lastOption.ContainsKey(dropdownIndex))
                    lastOption.Add(dropdownIndex, value);
            }
            yield return new WaitForSeconds(0.01f);
            selectedOptions.Add(value);
            for (int i = 0; i < dropdowns.Count; i++)
            {
                yield return new WaitForSeconds(0.01f);
                if (i != dropdownIndex && dropdowns[i].value == value)
                {
                    yield return new WaitForSeconds(0.01f);
                    dropdowns[i].value = 0;
                }
            }
        }
        yield return new WaitForSeconds(0.01f);
        if (CheckIfAllDropdownsHasSetValue())
        {
            CardGameManagerUI.instance.SendRankButton.interactable = true;
        }
    }
}

