using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerGenerator : MonoBehaviour
{
    /*int width = 10;
    int height = 9;
    Color[,] colors;


    Color HsvToRgb(double h, double S, double V)
    {
        /// Convert HSV to RGB
        /// h is from 0d - 360d
        /// s,v values are 0d - 1d
        /// r,g,b values are 0 - 255

        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if      (hi == 0)  return Color.FromArgb(255, v, t, p);
        else if (hi == 1)  return Color.FromArgb(255, q, v, p);
        else if (hi == 2)  return Color.FromArgb(255, p, v, t);
        else if (hi == 3)  return Color.FromArgb(255, p, q, v);
        else if (hi == 4)  return Color.FromArgb(255, t, p, v);
        else               return Color.FromArgb(255, v, p, q);
    }*/
}
