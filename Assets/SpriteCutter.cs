using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteCutter : Editor {

	[MenuItem("Create/Create sprites")]
    static void CreateSprites()
    {
        if (Selection.activeObject.GetType()==typeof(Texture2D))
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            Debug.Log(path);

            string filename = Path.GetFileNameWithoutExtension(path);

            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

            string dirrectoryPath = Path.Combine(Path.GetDirectoryName(path), filename); 

            if (!Directory.Exists(dirrectoryPath))
            {
                Directory.CreateDirectory(dirrectoryPath);
            }

            Debug.Log(sprites.Length);

            int i = 0;
            foreach (Object s in sprites)
            {
                if (s.GetType() == typeof(Sprite))
                {
                    Rect spriteRect = ((Sprite)s).rect;

                    Texture2D tex = new Texture2D((int)spriteRect.width, (int)spriteRect.height);

                    Color[] pixels = ((Sprite)s).texture.GetPixels((int)spriteRect.x, (int)spriteRect.y, (int)spriteRect.width, (int)spriteRect.height);

                    tex.SetPixels(pixels);
                
               
                    byte[] bytes = tex.EncodeToPNG();
                    File.WriteAllBytes(Path.Combine(dirrectoryPath, s.name+".png"), bytes);
           
                }
                i++;
            }
            AssetDatabase.Refresh();
        }
    }
}
