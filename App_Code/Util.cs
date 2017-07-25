using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Util
/// </summary>
public class Util
{
    public Util()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void BindDropDown(DropDownList ddl, object ds, String dtf, String dvf)
    {
        ddl.DataSource = ds;
        ddl.DataTextField = dtf;
        ddl.DataValueField = dvf;
        ddl.DataBind();
    }

    public static void ShowMessage(HtmlGenericControl ctrl, String mensaje, Boolean show, int? tipo)
    {
        switch (tipo)
        {
            case 0:
                ctrl.Attributes.Add("class", "alert alert-success"); break;
            case 1:
                ctrl.Attributes.Add("class", "alert alert-info"); break;
            case 2:
                ctrl.Attributes.Add("class", "alert alert-warning"); break;
            case 3:
                ctrl.Attributes.Add("class", "alert alert-danger"); break;
            default:
                break;
        }

        ctrl.InnerHtml = mensaje;
        ctrl.Style.Add(HtmlTextWriterStyle.Display, (show ? "" : "none"));
    }
}