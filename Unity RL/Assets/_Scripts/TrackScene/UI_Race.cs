// Nicholas Wile

using UnityEngine;
using TMPro;

public class UI_Race : MonoBehaviour
{

    // Reference to the UI text
    [SerializeField]
    private TMP_Text _velocity_text;

    [SerializeField]
    private float _scale_factor = 6.0f;

    public float velocity;

    private void Update()
    {
        // Multiply by 3 just to make the number look more interesting.
        _velocity_text.text = $"Velocity: \n{(velocity*_scale_factor).ToString("0.00")} km/h \n({(0.62f*velocity*_scale_factor).ToString("0.00")} mph)";
    }
}
