using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


[WebService(Namespace = "http://clarito.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod]
    public string HelloWorld()
    {
        return "EL OTRO CICLO, CON FUERZA!!!";
    }

    [WebMethod]
    public string TestLiberarCola(String email, String mensaje)
    {
        Mensajeria.ReceiveMessage(Mensajeria.COLA);
        return "COLA LIBERADA";
    }

    [WebMethod]
    public List<Mensaje> SolicitarNuevaLinea(String empresa, String dniContacto, String nombreRef)
    {
        List<Mensaje> lMsj = new List<Mensaje>();
        String nroTelefono = Util.RamdomPhone();
        Mensaje m = new Mensaje();
        m.Message = "Se generó nueva línea";
        m.Type = "S";
        m.Value = nroTelefono;
        lMsj.Add(m);

        Linea.CrearNuevaLinea(empresa, nombreRef, nroTelefono, String.Empty);

        return lMsj;
    }

    [WebMethod]
    public List<Mensaje> ConfirmarNuevaLinea(String empresa, String dniContacto, String nroLinea)
    {
        List<Mensaje> lMsj = new List<Mensaje>();
        Mensaje m = new Mensaje();

        int resultado = Linea.ConfirmarLinea(empresa, nroLinea);

        if (resultado == -1)
        {
            m.Message = "La línea: " + nroLinea + " ya se encuentra activada.";
            m.Type = "W";
            m.Value = nroLinea;
        }
        else
        {
            m.Message = "Se Confirmó Línea: " + nroLinea;
            m.Type = "S";
            m.Value = nroLinea;
        }

        lMsj.Add(m);
        return lMsj;
    }

    [WebMethod]
    public List<Linea> ConsultarLineas(String rucEmpresa, String dniContacto)
    {
        return Linea.getLineasAll(rucEmpresa);
    }

    [WebMethod]
    public List<Linea> ConsultarLinea(String empresa, String nroLinea)
    {
        return Linea.getLineasByClienteLinea(empresa, nroLinea);
    }

    [WebMethod]
    public List<Mensaje> SolicitarBajaLinea(String empresa, String dniContacto, String nroLinea)
    {
        List<Mensaje> lMsj = new List<Mensaje>();

        Mensaje m = new Mensaje();
        m.Message = "Se dio de baja a la línea solicitada";
        m.Type = "S";
        m.Value = nroLinea;
        lMsj.Add(m);
        return lMsj;
    }

    [WebMethod]
    public List<Empresa> ConsultarEmpresas()
    {
        return Empresa.getEmpresaAll();
    }
}