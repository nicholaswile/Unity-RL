// Nicholas Wile

using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{

    // Reference to the UI text
    [SerializeField]
    private TMP_Text _win_text;

    [SerializeField]
    private TMP_Text _velocity_text;

    [SerializeField]
    private float _scale_factor = 6.0f;

    public float total_wins;
    public float total_losses;
    public float velocity;

    private void Awake()
    {
        total_wins = 0;
        total_losses = 0;
    }
    
    private void Update()
    {
        _win_text.text = $"Win %: {(100*total_wins/(total_wins+total_losses)).ToString("0.00")}%";

        // Multiply by 3 just to make the number look more interesting.
        _velocity_text.text = $"Velocity: \n{(velocity*_scale_factor).ToString("0.00")} km/h \n({(0.62f*velocity*_scale_factor).ToString("0.00")} mph)";
    }
}
