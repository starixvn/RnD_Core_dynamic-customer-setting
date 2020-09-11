namespace Fanex.CustomerSetting.MatrixClient.Helpers
{
    using System;
    using System.Linq;
    using Fanex.CustomerSetting.MatrixClient.Exceptions;

    internal static class RandomHelper
    {
        private static Random _randomHandler = new Random();

#pragma warning disable S3995 // URI return values should not be strings
        internal static string GetRandomUri(string sourceUris)
#pragma warning restore S3995 // URI return values should not be strings
        {
            if (string.IsNullOrWhiteSpace(sourceUris))
            {
                throw new SettingMatrixException("Uris cannot be null or white space");
            }

            var uris = sourceUris.Split(',');
            var numOfUri = uris.Count();

            if (numOfUri == 1)
            {
                return uris[0];
            }

            var selectedUri = uris[_randomHandler.Next(0, numOfUri)];

            return selectedUri;
        }
    }
}