using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class ColorHelpers
{
    public static Color GetColorFromColorProps(ColorProps colorProps)
    {
        return new Color(colorProps.r, colorProps.g, colorProps.b, colorProps.a);
    }

    public static ColorProps GetColorPropsFromColor(Color color)
    {
        return new ColorProps
        {
            r = color.r,
            g = color.g,
            b = color.b,
            a = color.a
        };
    }
    
}

public struct ColorProps : INetworkSerializable
{
    public float r;
    public float g;
    public float b;
    public float a;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref r);
        serializer.SerializeValue(ref g);
        serializer.SerializeValue(ref b);
        serializer.SerializeValue(ref a);
    }
}