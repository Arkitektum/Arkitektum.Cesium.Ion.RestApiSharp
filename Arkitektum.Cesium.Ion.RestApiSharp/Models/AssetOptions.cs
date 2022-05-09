using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Models;

public abstract class AssetOptions
{
    public SourceType SourceType { get; set; }
}

public class RasterImageryOptions : AssetOptions
{
    public RasterImageryOptions() {SourceType = SourceType.RASTER_IMAGERY;}
}

public class RasterTerrainOptions : AssetOptions
{
    public HeightReference? HeightReference { get; set; }
    public int? ToMeters { get; set; }
    public int? BaseTerrainId { get; set; }
    public bool? WaterMask { get; set; }

    public RasterTerrainOptions()
    {
        SourceType = SourceType.RASTER_TERRAIN;
        BaseTerrainId = 1;
        HeightReference = Util.HeightReference.WGS84;
    }
}

public class TerrainDatabaseOptions : AssetOptions
{
    public TerrainDatabaseOptions() {SourceType = SourceType.TERRAIN_DATABASE;}
}

public class CityGmlOptions : AssetOptions
{
    public GeometryCompression? GeometryCompression { get; set; }
    public bool? DisableColors { get; set; }
    public bool? DisableTextures { get; set; }
    public bool? ClampToTerrain { get; set; }
    public int? BaseTerrainId { get; set; }

    public CityGmlOptions()
    {
        SourceType = SourceType.CITYGML;
    }

    public CityGmlOptions(bool clampToTerrain, int baseTerrainId=1)
    {
        SourceType = SourceType.CITYGML;
        ClampToTerrain = clampToTerrain;
        BaseTerrainId = baseTerrainId;
    }
}

public class ThreeDTilesOptions : AssetOptions
{
    public string TilesetJson { get; set; }

    public ThreeDTilesOptions(string relativePathToTilesetJson)
    {
        SourceType = SourceType._3DTILES;
        TilesetJson = relativePathToTilesetJson;
    }
}

//todo: implement options for the remaining source types