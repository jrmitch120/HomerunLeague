using System;

namespace HomerunLeague.ServiceModel.Utils
{
    
    internal static class MlbImages
    {
        public enum ImageSize
        {
            Standard, 
            Large
        }

        public static Uri Player(int mlbId, ImageSize size = ImageSize.Standard)
        {
            var imageSize = size == ImageSize.Large ? "@2x" : "";

            return new Uri($"http://mlb.mlb.com/mlb/images/players/head_shot/{mlbId}{imageSize}.jpg");
        }

        public static Uri TeamLogo(int mlbTeamId, ImageSize size = ImageSize.Standard)
        {
            var imageSize = size == ImageSize.Large ? "@2x" : "";

            return new Uri($"http://m.mlb.com/shared/images/logos/32x32_cap/{mlbTeamId}{imageSize}.png");
        }
    }
}
