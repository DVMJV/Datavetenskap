
public class TDMap
{
    TDTile[][] tiles;
    int width;
    int height;
    
    public TDMap(int width, int height) 
    {
        this.width = width;
        this.height = height;
        tiles = new TDTile[height * width][];
    }

    public TDTile GetTile(int x, int y) 
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        
        return tiles[x][y];
    }
}
