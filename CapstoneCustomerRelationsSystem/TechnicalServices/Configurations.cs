using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class Configurations
    {
        public Configurations() {

        }//end constructor

        /// <summary>
        /// Gets the targeted string in the appsettings.json file.
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration(string _section1, string _section2 = "") {
            ConfigurationBuilder databaseUsersBuilder = new ConfigurationBuilder();
            databaseUsersBuilder.SetBasePath(Directory.GetCurrentDirectory());
            databaseUsersBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = databaseUsersBuilder.Build();
            string result = "";

            //Checks if the there is a section in a section
            if (_section2 == "") {
                result = configuration.GetSection(_section1).Value;
            } else {
                result = configuration.GetSection(_section1).GetSection(_section2).Value;
            }
            return result;
        }//end GetConfig
    }//end class
}//end namespace