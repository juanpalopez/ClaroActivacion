using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Empresa
{
    #region Fields

    public int idEmpresa { get; set; }
    public string rucEmpresa { get; set; }
    public string nombreEmpresa { get; set; }
    public bool estado { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the Empresa class.
    /// </summary>
    public Empresa()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Empresa class.
    /// </summary>
    public Empresa(string rucEmpresa, string nombreEmpresa, bool estado)
    {
        this.rucEmpresa = rucEmpresa;
        this.nombreEmpresa = nombreEmpresa;
        this.estado = estado;
    }
    /// <summary>
    /// Create a insert method for the Empresa class.
    /// </summary>
    public static int Save(string rucEmpresa, string nombreEmpresa, bool estado)
    {
        return Util.ExecuteCommand("sp_replace", new SqlParameter[] {
            new SqlParameter("@rucEmpresa" , rucEmpresa),
            new SqlParameter("@nombreEmpresa" , nombreEmpresa),
            new SqlParameter("@estado" , estado),
         });
    }

    public static Empresa getEmpresaByRuc(String ruc)
    {
        using (SqlConnection cn = new SqlConnection(Util.CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand("usp_empresaSelectByRuc", cn))
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@ruc", ruc);
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt.DataTableToList2<Empresa>()[0];
            }
        }
    }


    public static List<Empresa> getEmpresaAll()
    {
        using (SqlConnection cn = new SqlConnection(Util.CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand("usp_empresaSelectAll", cn))
            {
                cm.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt.DataTableToList2<Empresa>();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the Empresa class.
    /// </summary>
    public Empresa(int idEmpresa, string rucEmpresa, string nombreEmpresa, bool estado)
    {
        this.idEmpresa = idEmpresa;
        this.rucEmpresa = rucEmpresa;
        this.nombreEmpresa = nombreEmpresa;
        this.estado = estado;
    }

    #endregion

}
