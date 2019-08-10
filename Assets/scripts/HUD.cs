﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The HUD
/// </summary>
public class HUD : MonoBehaviour
{
    [SerializeField]
    Text pathLengthText;

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start()
	{
        // Adding listener to event manager
        EventManager.AddPathFoundListener(SetPathLength);
	}

    /// <summary>
    /// Sets the path length in the hud
    /// </summary>
    /// <param name="length">path length</param>
    void SetPathLength(float length)
    {
        pathLengthText.text = length.ToString();

    }
}
