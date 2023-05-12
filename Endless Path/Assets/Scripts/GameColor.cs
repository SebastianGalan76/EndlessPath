using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColor : ScriptableObject {
    public Color mainColor;
    public Color lightColor;

    public GameColor(string mainColor, string lightColor) {
        ColorUtility.TryParseHtmlString(mainColor, out this.mainColor);
        ColorUtility.TryParseHtmlString(lightColor, out this.lightColor);
    }

    public override bool Equals(object other) {
        if(other == null)
            return false;
        if(!(other is GameColor))
            return false;

        GameColor gameColor = (GameColor)other;
        return mainColor == gameColor.mainColor && lightColor == gameColor.lightColor;
    }
}
