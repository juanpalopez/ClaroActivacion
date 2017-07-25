using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Messaging;

/// <summary>
/// Summary description for Lineas
/// </summary>
public class Linea
{
    #region Properties
    public int idLinea { get; set; }
    public int idEmpresa { get; set; }
    public int idPlanes { get; set; }
    public string nombreReferencia { get; set; }
    public string nroLinea { get; set; }
    public string nroIMEI { get; set; }
    public DateTime fechaActivacion { get; set; }
    public DateTime fechaCreacion { get; set; }
    public DateTime fechaBaja { get; set; }
    public bool estado { get; set; }
    public string rucEmpresa { get; set; }
    public string nombreEmpresa { get; set; }
    public string precioPlan { get; set; }
    #endregion

    public Linea()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static String notificar(String correo, String titulo, String mensaje)
    {
        List<string> to = new List<string>();
        List<string> co = new List<string>();
        List<string> cc = new List<string>();
        StringBuilder sb = new StringBuilder();
        to.Add(correo);
        sb.AppendLine(mensaje);

        LogMessage msg = new LogMessage()
        {
            MessageText = mensaje,
            MessageTime = DateTime.Now
        };

        //Mensajeria.SendMessage(Mensajeria.COLA, msg);
        Mensajeria.ReceiveMessage(Mensajeria.COLA);
        return "OK";
    }

    public static int ConfirmarLinea(String ruc, String nroLinea)
    {
        List<Linea> ls = new List<Linea>();
        CorreoMessage cMsg = new CorreoMessage();
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection cn = new SqlConnection(Util.CN))
            {
                cn.Open();
                using (SqlCommand cm = new SqlCommand("usp_activarLinea", cn))
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@ruc", ruc);
                    cm.Parameters.AddWithValue("@nroTelefono", nroLinea);

                    dt.Load(cm.ExecuteReader());
                    ls = dt.DataTableToList2<Linea>();

                    if (ls != null)
                    {
                        Linea lin = ls[0];
                        cMsg.NroLinea = lin.nroLinea;
                        cMsg.Ruc = lin.rucEmpresa;
                        cMsg.Destinatario = Util.CC;
                        cMsg.Monto = lin.precioPlan;
                        cMsg.Nombre = lin.nombreEmpresa;
                        cMsg.Mensaje = "Estimado cliente, se activo la linea: " + lin.nroLinea + " para: " + lin.nombreReferencia;
                        Mensajeria.SendMessage(Mensajeria.COLA, cMsg);
                        Mensajeria.ReceiveMessageActivacion(Mensajeria.COLA);
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public static int CrearNuevaLinea(String ruc, String nombre, String nroLinea, String imei)
    {
        using (SqlConnection cn = new SqlConnection(Util.CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand("usp_lineasInsert", cn))
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@idEmpresa", Empresa.getEmpresaByRuc(ruc).idEmpresa);
                cm.Parameters.AddWithValue("@idPlanes", 1);
                cm.Parameters.AddWithValue("@nombreReferencia", nombre);
                cm.Parameters.AddWithValue("@nroLinea", nroLinea);
                cm.Parameters.AddWithValue("@nroIMEI", imei);
                cm.Parameters.AddWithValue("@fechaActivacion", null);
                cm.Parameters.AddWithValue("@fechaBaja", null);
                cm.Parameters.AddWithValue("@estado", 0);
                return cm.ExecuteNonQuery();
            }
        }
    }

    public static List<Linea> getLineasAll(String ruc)
    {
        String sqlCad = String.Format("select * from v_lineas_empresa where rucEmpresa='{0}' order by fechaActivacion desc", ruc);
        using (SqlConnection cn = new SqlConnection(Util.CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand(sqlCad, cn))
            {
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt.DataTableToList2<Linea>();
            }
        }
    }

    public static List<Linea> getLineasByClienteLinea(String ruc, String linea)
    {
        String sqlCad = String.Format("select * from v_lineas_empresa where rucEmpresa='{0}' and nroLinea='{1}'", ruc, linea);

        using (SqlConnection cn = new SqlConnection(Util.CN))
        {
            cn.Open();
            using (SqlCommand cm = new SqlCommand("select * from lineas", cn))
            {
                DataTable dt = new DataTable();
                dt.Load(cm.ExecuteReader());
                return dt.DataTableToList2<Linea>();
            }
        }
    }
}