// Nicholas Wile

using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{

    // Reference to the UI text
    [SerializeField]
    private TMP_Text _win_text;

    public float total_wins;
    public float total_losses;

    private void Awake()
    {
        total_wins = 0;
        total_losses = 0;
    }
    
    private void Update()
    {
        _win_text.text = $"Win %: {(100*total_wins/(total_wins+total_losses)).ToString("0.00")}%";

    }
}
