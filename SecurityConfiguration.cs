using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Joe.Security
{
    public class SecurityConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ApplicationAreas ApplicationAreas { get { return (ApplicationAreas)base[""]; } set { base[""] = value; } }

        public static SecurityConfiguration GetInstance()
        {
            return (SecurityConfiguration)System.Configuration.ConfigurationManager.GetSection("SecurityConfiguration") ?? new SecurityConfiguration();
        }
    }

    public class ApplicationAreas : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ApplicationArea();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((ApplicationArea)element).Name;
        }
    }

    public class ApplicationArea : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public String Name { get { return (string)base["name"]; } set { base["name"] = value; } }
        [ConfigurationProperty("allRoles", IsRequired = false)]
        public String AllRoles { get { return (string)base["allRoles"]; } set { base["allRoles"] = value; } }
        [ConfigurationProperty("createRoles", IsRequired = false)]
        public String CreateRoles { get { return (string)base["createRoles"]; } set { base["createRoles"] = value; } }
        [ConfigurationProperty("readRoles", IsRequired = false)]
        public String ReadRoles { get { return (string)base["readRoles"]; } set { base["readRoles"] = value; } }
        [ConfigurationProperty("updateRoles", IsRequired = false)]
        public String UpdateRoles { get { return (string)base["updateRoles"]; } set { base["updateRoles"] = value; } }
        [ConfigurationProperty("deleteRoles", IsRequired = false)]
        public String DeleteRoles { get { return (string)base["deleteRoles"]; } set { base["deleteRoles"] = value; } }
    }
}
