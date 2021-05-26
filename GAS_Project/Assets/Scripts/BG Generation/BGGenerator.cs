using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGGenerator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Colors the background potentially show with the key being the propability of this color appearing")]
    private List<Color> colors;

    [SerializeField]
    [Tooltip("Most used color in the sprite")]
    private Color defaultColor;

    [SerializeField]
    private float waitTime = 1.0f;

    [SerializeField]
    private int textureHeight = 1024;

    [SerializeField]
    private int textureWidth = 576;

    private SpriteRenderer spRend;
    
    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();

        //create texture
        Texture2D tex = new Texture2D(textureWidth, textureHeight);
        Color[] pixels = new Color[tex.width * tex.height];
        
        for(int x = 0; x < tex.width; x++)
        {
            for(int y = 0; y < tex.height; y++)
            {
                //x = position in row, y * tex.width = row
                pixels[x + (y * tex.width)] = colors[0];
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();

        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        //create sprite from texture
        Sprite proc = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        proc.name = "Procedural_BG_Sprite";

        //Assign procedural spRend.sprite
        spRend.sprite = proc;
    }
    
    public void StartFunction()
    {
        StartCoroutine(ChangeBackground());
    }

    private IEnumerator ChangeBackground()
    {
        Texture2D tex = spRend.sprite.texture;

        //from which row on should the texture be changed
        int rowToChange = tex.height - 1;

        while(rowToChange >= -tex.height)
        {
            //get the texture
            tex = spRend.sprite.texture;
            Color[] pixels = new Color[tex.width * tex.height];

            for (int y = 0; y < tex.height; y++)
            {
                if (y < rowToChange)
                {
                    for (int x = 0; x < tex.width; x++)
                    {
                        //x = position in row, y * tex.width = row
                        pixels[x + (y * tex.width)] = colors[0];
                    }

                    continue;
                }

                int rowDifference = Mathf.Abs(rowToChange - y);

                for (int x = 0; x < tex.width; x++)
                {
                    //x = position in row, y * tex.width = row
                    pixels[x + (y * tex.width)] = Color.Lerp(colors[0], colors[1], (float)rowDifference / (float)tex.width);
                }

            }

            tex.SetPixels(pixels);
            tex.Apply();

            Sprite proc = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            proc.name = "Procedural_BG_Sprite";

            //Assign procedural spRend.sprite
            spRend.sprite = proc;

            rowToChange--;

            yield return new WaitForSecondsRealtime(waitTime);
        }



        //Color.Lerp(colors[0], colors[1], (float)y / (float)tex.width);
    }
}
