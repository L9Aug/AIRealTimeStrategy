using UnityEngine;
using System.Collections;
using System.IO;

public class MapToJpeg : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() { }

    public void jpeg()
    {

        int W =MapGenerator.Map.GetLength(1);
        int H =MapGenerator.Map.GetLength(0);

        Texture2D texture = new Texture2D(W, H, TextureFormat.RGB24, false);


        TerrainTypes type;
        for(int i = 0;i < W;i++)
        {
          
            for (int b = 0;b < H;b++)
            {
                type = MapGenerator.Map[i, b].TerrainType;

                switch (type)
                {
                    case TerrainTypes.Coast:
                        {
                            texture.SetPixel(i, b,Color.black);
                        }
                        break;

                    case TerrainTypes.Desert:
                        {
                            texture.SetPixel(i, b,Color.grey);
                        }
                        break;

                    case TerrainTypes.Forest:
                        {
                            texture.SetPixel(i, b,Color.green);
                        }
                        break;

                    case TerrainTypes.Hills:
                        {
                            texture.SetPixel(i, b, new Color(0, 36, 0));
                        }
                        break;

                    case TerrainTypes.Grassland:
                        {
                            texture.SetPixel(i, b,Color.red);
                        }
                        break;

                    case TerrainTypes.Lake:
                        {
                            texture.SetPixel(i, b, new Color(255,255, 0));
                        }
                        break;

                    case TerrainTypes.Plains:
                        {
                            texture.SetPixel(i, b, new Color(255, 119, 0));
                        }
                        break;

                    case TerrainTypes.Sea:
                        {
                            texture.SetPixel(i, b, new Color(0, 0, 255));
                            
                        }
                        break;


                }

                
            }
            texture.Apply();
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../testscreen.png", bytes);

        DestroyObject(texture);
    }
	
	

  

    
}
