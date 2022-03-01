using Arkitektum.Cesium.Ion.RestApiSharp.Models;
using Arkitektum.Cesium.Ion.RestApiSharp.Util;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Services
{
    public static class AssetFactory
    {
        public static Asset Create(string assetName, AssetType assetType, SourceType sourceType, string assetLocation="", string? attribution=null, string? description=null)
        {
            return new Asset
            {
                Name = assetName,
                Type = assetType,
                Attribution = attribution,
                Description = description,
                Options = CreateAssetOptions(sourceType, assetLocation),
            };
        }

        private static AssetOptions CreateAssetOptions(SourceType sourceType, string assetLocation)
        {
            return sourceType switch
            {
                SourceType.RASTER_IMAGERY => new RasterImageryOptions(),
                SourceType.RASTER_TERRAIN => new RasterTerrainOptions(),
                SourceType.TERRAIN_DATABASE => new TerrainDatabaseOptions(),
                SourceType.CITYGML => new CityGmlOptions(),
                SourceType.KML => throw new NotImplementedException(),
                SourceType._3D_CAPTURE => throw new NotImplementedException(),
                SourceType._3D_MODEL => throw new NotImplementedException(),
                SourceType.POINT_CLOUD => throw new NotImplementedException(),
                SourceType._3DTILES => new ThreeDTilesOptions(assetLocation),
                SourceType.CZML => throw new NotImplementedException(),
                SourceType.GEOJSON => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
            };
        }
    }
}
