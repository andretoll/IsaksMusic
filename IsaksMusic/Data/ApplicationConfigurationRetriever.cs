using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Data
{
    public class ApplicationConfigurationRetriever
    {
        private readonly IConfiguration _configuration;

        public ApplicationConfigurationRetriever(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Return allowed music file extensions specified in appsettings.json
        /// </summary>
        /// <returns></returns>
        public List<string> GetMusicFileExtensions()
        {
            IConfigurationSection myArraySection = _configuration.GetSection("AudioFileExtensions");
            var extensionList = myArraySection.GetChildren().ToList().Select(c => c.Value).ToList();

            return extensionList;
        }
    }
}
