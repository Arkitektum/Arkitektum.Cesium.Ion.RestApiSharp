using System.ComponentModel;
using Arkitektum.Cesium.Ion.RestApiSharp.Models;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Util
{
    public static class AssetMetadataExtensions
    {
        public static bool IsCorrupt(this AssetMetadata asset)
        {
            return asset.Status switch
            {
                AssetStatus.DATA_ERROR or AssetStatus.ERROR => true,
                AssetStatus.NOT_STARTED or AssetStatus.AWAITING_FILES or AssetStatus.IN_PROGRESS
                    or AssetStatus.COMPLETE => false,
                _ => throw new InvalidEnumArgumentException($"Unknown asset status: '{asset.Status}'",
                    (int)asset.Status,
                    typeof(AssetStatus))
            };
        }

        public static bool IsComplete(this AssetMetadata asset)
        {
            return asset.Status == AssetStatus.COMPLETE;
        }
    }
}
