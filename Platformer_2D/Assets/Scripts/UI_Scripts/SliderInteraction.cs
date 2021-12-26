using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInteraction : MonoBehaviour
{

    [SerializeField] public Slider _slider;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSliderValueChanged(float value)
    {
        Debug.Log($"{_slider.name} = {value}");
    }
}
