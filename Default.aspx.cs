using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using wsClarito;

public partial class _Default : System.Web.UI.Page
{
    wsClarito.Service proxy = new wsClarito.Service();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            Empresa[] emp = proxy.ConsultarEmpresas();

            if (emp.Length > 0)
            {
                Util.BindDropDown(ddlEmpresa, emp, "nombreEmpresa", "rucEmpresa");
                Util.BindDropDown(ddlEmpresa2, emp, "nombreEmpresa", "rucEmpresa");
                Util.BindDropDown(ddlEmpresa3, emp, "nombreEmpresa", "rucEmpresa");
            }
        }
    }

    protected void btnProceso_Click(object sender, EventArgs e)
    {
        try
        {
            Util.ShowMessage(msg_alerta_0, String.Empty, false, 0);
            Solicitar.Style.Add(HtmlTextWriterStyle.Display, "");
            Confirmar.Style.Add(HtmlTextWriterStyle.Display, "None");
            ConsultarLineas.Style.Add(HtmlTextWriterStyle.Display, "None");

            if (txtDni.Text.Trim().Length < 8)
            {
                Util.ShowMessage(msg_alerta_0, "<strong>Error!</strong>, ingrese un DNI válido", true, 3);
                return;
            }
            if (txtNombreUsuario.Text.Trim().Length < 5)
            {
                Util.ShowMessage(msg_alerta_0, "<strong>Error!</strong>, ingrese un nombre válido", true, 3);
                return;
            }

            Mensaje[] msg = proxy.SolicitarNuevaLinea(ddlEmpresa.SelectedValue, txtDni.Text, txtNombreUsuario.Text);

            if (msg.Length > 0)
            {
                foreach (var item in msg)
                {
                    Util.ShowMessage(msg_alerta_0, item.Message + ", Nro: " + item.Value + ". Recuerde confirmar su nueva línea.", true, 0);
                }
                txtDni.Text = String.Empty;
                txtNombreUsuario.Text = String.Empty;
            }
        }
        catch (Exception ex)
        {
            Util.ShowMessage(msg_alerta_0, "<strong>Error!</strong> " + ex.Message, true, 3);
        }
    }


    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            Solicitar.Style.Add(HtmlTextWriterStyle.Display, "None");
            Confirmar.Style.Add(HtmlTextWriterStyle.Display, "");
            ConsultarLineas.Style.Add(HtmlTextWriterStyle.Display, "None");

            if (txtConfirmaDni.Text.Trim().Length < 8)
            {
                Util.ShowMessage(msg_alerta_1, "<strong>Error!</strong>, ingrese un DNI válido", true, 3);
                return;
            }

            if (txtConfirmaNroLinea.Text.Trim().Length < 8)
            {
                Util.ShowMessage(msg_alerta_1, "<strong>Error!</strong>, ingrese un número de línea válido", true, 3);
                return;
            }

            Mensaje[] msg = proxy.ConfirmarNuevaLinea(ddlEmpresa2.SelectedValue, txtConfirmaDni.Text, txtConfirmaNroLinea.Text.Trim());

            if (msg.Length > 0)
            {
                foreach (var item in msg)
                {
                    if (item.Type == "S")
                    {
                        Util.ShowMessage(msg_alerta_1, item.Message, true, 0);
                    }
                    else
                    {
                        Util.ShowMessage(msg_alerta_1, item.Message, true, 3);
                    }
                }
                txtConfirmaDni.Text = String.Empty;
                txtConfirmaNroLinea.Text = String.Empty;
            }

        }
        catch (Exception ex)
        {
            Util.ShowMessage(msg_alerta_1, "<strong>Error!</strong> " + ex.Message, true, 3);
        }
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            Solicitar.Style.Add(HtmlTextWriterStyle.Display, "None");
            Confirmar.Style.Add(HtmlTextWriterStyle.Display, "None");
            ConsultarLineas.Style.Add(HtmlTextWriterStyle.Display, "");
            Linea[] lin = proxy.ConsultarLineas(ddlEmpresa3.SelectedValue, String.Empty);

            GridView1.DataSource = lin;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Util.ShowMessage(msg_alerta_2, "<strong>Error!</strong> " + ex.Message, true, 3);
        }
    }
}