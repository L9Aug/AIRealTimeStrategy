using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour
{
    int H;
    int W;

    void Setup()
    {
        W = MapGenerator.Map.GetLength(0);
        H = MapGenerator.Map.GetLength(1);
    }

    public void StartGen()
    {
        Setup();
        // Blank(TerrainTypes.Mountains);
        // Grassland();
        //Grassland(5);
        SpotGen(TerrainTypes.Grassland, 200, 1, 4,TerrainTypes.Sea);
        SpotGen(TerrainTypes.Plains,200,2,5,TerrainTypes.Sea,TerrainTypes.Grassland);
        SpotGen(TerrainTypes.Hills, 400, 1,TerrainTypes.Desert);
        SpotGen(TerrainTypes.Forest,100, 2,3, TerrainTypes.Desert, TerrainTypes.Hills,TerrainTypes.Plains);
        SpotGen(TerrainTypes.Lake, 100, 3,5, TerrainTypes.Plains, TerrainTypes.Hills,TerrainTypes.Forest);
        Sea(10);


    }

    void Blank(TerrainTypes Terrain)
    {
        for (int i = 0; i < W; i++)
        {
            
            for (int b = 0; b < H; b++)
            {
                MapGenerator.Map[b, i].SetTexture(Terrain);
            }
        }
    }

    void Sea()
    {

        int Xtimes = ((W / 10) + 5);
        print(Xtimes);

        int TopOrBottom = Random.Range(0, 2);
        for (int i = 0; i < Xtimes ; i++)
        {
            List<HexTile> temp1 = new List<HexTile>();
            int tmp1;
            int tmp2;

            int[] Hedges = new int[2];

            Hedges[0] = 0;
            Hedges[1] = H - 1;

            int[] Wedges = new int[2];

            Wedges[0] = 0;
            Wedges[1] = W - 1;

            tmp1 = Hedges[TopOrBottom];
            tmp2 = Random.Range(0, W);

            int Size = Random.Range(6, 13);
            temp1 = MapGenerator.Map[tmp1, tmp2].GetHexArea(Size);

            foreach (HexTile pos in temp1)
            {

                pos.SetTexture(TerrainTypes.Sea);
                pos.TerrainType = TerrainTypes.Sea;
            }
       
            temp1 = MapGenerator.Map[tmp1, tmp2].GetHexRing(Size+1);

            foreach (HexTile pos in temp1)
            {
                if (pos.TerrainType != TerrainTypes.Sea)
                {
                    pos.SetTexture(TerrainTypes.Coast);
                    pos.TerrainType = TerrainTypes.Coast;
                }
            }

        }
    }

    void Sea(int PerLine)
    {
        List<HexTile> temp1 = new List<HexTile>();
        int counter = 0;
        for (int i = 0; i < W; i++)
        {
            counter = 0;

            for (int b = 0; b < H; b++)
            {
                if (true)//counter < PerLine)
                {
                    //int rnd = Random.Range(0, 20);
                    if (b == 0 || b == H -1 || i == 0 || i == W -1)
                    {
                        if (true)
                        {

                            int size = Random.Range(3, 7);
                            temp1 = MapGenerator.Map[i, b].GetHexArea(size);

                            foreach (HexTile pos in temp1)
                            {
                                pos.SetTexture(TerrainTypes.Sea);
                                pos.TerrainType = TerrainTypes.Sea;

                            }

                            temp1 = MapGenerator.Map[i, b].GetHexRing(size + 1);

                            foreach (HexTile pos in temp1)
                            {
                                if (pos.TerrainType != TerrainTypes.Sea)
                                {
                                    pos.SetTexture(TerrainTypes.Coast);
                                    pos.TerrainType = TerrainTypes.Coast;
                                }
                            }


                            counter++;
                        }
                    }
                }
            }
        }

    }

    void Grassland()
    {

        int xtime = (H / 3) + (W / 3) + 20;
        print("Xtime grassland :" + xtime);
        for(int i = 0; i < xtime; i++)
        {
            int tmp1 = Random.Range(0, H);
            int tmp2 = Random.Range(0, W);

            List<HexTile> temp1 = new List<HexTile>();
            temp1 = MapGenerator.Map[tmp1, tmp2].GetHexArea(6);

            foreach(HexTile pos in temp1)
            {
                pos.SetTexture(TerrainTypes.Grassland);
                pos.TerrainType = TerrainTypes.Grassland;

            }


        }
    }
    void Grassland(int PerLine)
    {

        List<HexTile> temp1 = new List<HexTile>();
        int counter = 0;
        for (int i = 0; i < W;i++)
        {
            counter = 0;
            
            for (int b = 0; b < H; b++)
            {
                if (counter < PerLine)
                {
                    int rnd = Random.Range(0, 50);
                    if (rnd == 1)
                    {
                        temp1 = MapGenerator.Map[i, b].GetHexArea(Random.Range(3,7));

                        foreach (HexTile pos in temp1)
                        {
                            pos.SetTexture(TerrainTypes.Grassland);
                            pos.TerrainType = TerrainTypes.Grassland;

                        }
                        counter++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="terraintype"></param>
    /// <param name="T"></param>
    /// <param name="S"></param>
    /// <param name="NotAllowedType">can be place apon</param>
    void SpotGen(TerrainTypes terraintype, int T,int S,params TerrainTypes[] NotAllowedType)
    {
        int xtime = T;
       // print("Xtime grassland :" + xtime);
        for (int i = 0; i < xtime; i++)
        {
            int tmp1 = Random.Range(0, W);
            int tmp2 = Random.Range(0, H);

            List<HexTile> temp1 = new List<HexTile>();
            temp1 = MapGenerator.Map[tmp1, tmp2].GetHexArea(S);

            foreach (HexTile pos in temp1)
            {
                bool CanDo = true;

                foreach(TerrainTypes tpos in NotAllowedType)
                {
                    if(pos.TerrainType == tpos)
                    {
                        CanDo = false;
                    }
                }

                if (CanDo)
                {
                    pos.SetTexture(terraintype);
                    pos.TerrainType = terraintype;
                }
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="terraintype">The terrain type to generate</param>
    /// <param name="T">Times to Run</param>
    /// <param name="Min">random range minimum</param>
    /// <param name="Max">random range max inclusive</param>
    /// <param name="NotAllowedType">type to not gen on</param>
    void SpotGen(TerrainTypes terraintype, int T,int Min,int Max,params TerrainTypes[] NotAllowedType)
    {
        int xtime = T;
        // print("Xtime grassland :" + xtime);
        for (int i = 0; i < xtime; i++)
        {
            int tmp1 = Random.Range(0, W);
            int tmp2 = Random.Range(0, H);

            List<HexTile> temp1 = new List<HexTile>();
            temp1 = MapGenerator.Map[tmp1, tmp2].GetHexArea(Random.Range(Min,(Max+1)));

            foreach (HexTile pos in temp1)
            {
                bool CanDo = true;

                foreach (TerrainTypes tpos in NotAllowedType)
                {
                    if (pos.TerrainType == tpos)
                    {
                        CanDo = false;
                    }
                }

                if (CanDo)
                {
                    pos.SetTexture(terraintype);
                    pos.TerrainType = terraintype;
                }
            }
        }
    }

}