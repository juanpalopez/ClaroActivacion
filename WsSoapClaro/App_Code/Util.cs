using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Util
/// </summary>
public static class Util
{
    public static String CN = System.Configuration.ConfigurationManager.AppSettings["CN"].ToString();
    public static String CC = System.Configuration.ConfigurationManager.AppSettings["CC"].ToString();

    public static string GetConfigValue(bool isAppSetting, string nameFind)
    {
        string ret = null;
        string section = isAppSetting ? "appSettings" : "connectionStrings";
        string keyAttr = isAppSetting ? "key" : "name";
        string valAttr = isAppSetting ? "value" : "connectionString";

        if (isAppSetting)
        {
            ret = ConfigurationManager.AppSettings[nameFind];
        }
        else
        {
            ConnectionStringSettings oCS = ConfigurationManager.ConnectionStrings[nameFind];

            if (oCS != null) { ret = oCS.ConnectionString; }
        }

        if (string.IsNullOrEmpty(ret))
        {
            #region Buscamos en el web.config de servidor...

            System.Configuration.Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
            System.IO.FileInfo fInfo = new FileInfo(machineConfig.FilePath);
            String webConfig = System.IO.Path.Combine(fInfo.Directory.FullName, "web.config");

            XDocument doc = XDocument.Load(webConfig);

            nameFind = nameFind.ToLower();
            section = section.ToLower();
            keyAttr = keyAttr.ToLower();
            section = section.ToLower();

            var setting = (from d in doc.Descendants() where d.Name.LocalName.ToLower().Equals(section) select d).SingleOrDefault();

            foreach (var item in setting.Descendants())
            {
                var objK = (from a in item.Attributes() where a.Name.LocalName.ToLower().Equals(keyAttr) select a).SingleOrDefault();
                var objV = (from a in item.Attributes() where a.Name.LocalName.ToLower().Equals(valAttr) select a).SingleOrDefault();

                if (objK != null && objV != null && objK.Value.ToLower().Equals(nameFind))
                {
                    ret = objV.Value;
                    break;
                }
            }
            #endregion
        }

        return ret;
    }

    public static int ExecuteCommand(String command)
    {
        using (SqlConnection cn = new SqlConnection(CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand(command, cn))
                return cm.ExecuteNonQuery();
        }
    }

    public static int ExecuteCommand(String command, SqlParameter[] paramms)
    {
        using (SqlConnection cn = new SqlConnection(CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand(command, cn))
            {
                cm.Parameters.AddRange(paramms);
                return cm.ExecuteNonQuery();
            }
        }
    }


    public static String RamdomPhone()
    {
        return String.Format("9{0:d8}", (DateTime.Now.Ticks / 10) % 100000000);
    }
}