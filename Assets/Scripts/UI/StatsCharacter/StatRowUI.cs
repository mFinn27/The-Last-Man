using UnityEngine;
using TMPro;

public class StatRowUI : MonoBehaviour
{
    public TextMeshProUGUI txtLabel;
    public TextMeshProUGUI txtValue;

    public void Setup(string label, string value)
    {
        txtLabel.text = label;
        txtValue.text = value;
    }
}