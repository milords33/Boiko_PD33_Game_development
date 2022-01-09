using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehaviour : MonoBehaviour
{
    [SerializeField] private Transform followingTarget;
    [SerializeField, Range(0f, 1f)] private float parallaxStrength = 0.1f;
    [SerializeField] private bool disableVerticalParallax;

    private Vector3 targerPreviousPosition;
    void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;

        targerPreviousPosition = followingTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        var delta = followingTarget.position - targerPreviousPosition;

        if (disableVerticalParallax)
            delta.y = 0;

        targerPreviousPosition = followingTarget.position;

        transform.position += delta * parallaxStrength;
    }
}
