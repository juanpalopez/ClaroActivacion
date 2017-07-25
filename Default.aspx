<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clarito | Gestión</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge;">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>


    <script>
        function UserConfirmation() {
            return confirm("Revise si es necesario cambiar de centro de gasto a esta factura.\¿Está seguro de continuar con la grabación?");
        }

        function showSolicitar() {
            if (true) {
                $("#btnProceso").show();
                $("#txtDni").val('');
                $("#txtNombreUsuario").val('');
            }
        }

        function Solicitar(obj) {
            $("ul li").removeClass("active");
            $("#li_1").addClass("active");
            $("#Solicitar").toggle();
            $("#Confirmar").hide();
            $("#ConsultarLineas").hide();
        }

        function Confirmar(obj) {
            $("ul li").removeClass("active");
            $("#li_2").addClass("active");
            $("#Confirmar").toggle();
            $("#Solicitar").hide();
            $("#ConsultarLineas").hide();
        }

        function Consultar(obj) {
            $("ul li").removeClass("active");
            $("#li_3").addClass("active");
            $("#ConsultarLineas").toggle();
            $("#Solicitar").hide();
            $("#Confirmar").hide();
        }

    </script>
</head>

<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-default">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">Clarito</a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li id="li_1" class="active"><a href="#Solicitar" onclick="Solicitar(this);">Solicitar Línea</a></li>
                        <li id="li_2"><a href="#Confirmar" onclick="Confirmar(this);">Confirmar Línea</a></li>
                        <li id="li_3"><a href="#Consultar" onclick="Consultar(this);">Consultar Líneas</a></li>
                    
                    </ul>                     
                </div>
                <!--/.nav-collapse -->
            </div>
            <!--/.container-fluid -->
        </nav>


        <div class="container" runat="server" id="Solicitar">
            <h1>Solicitar nueva línea corporativa</h1>
            <hr />
            <div class="form-group">
                <label for="ddlEmpresa">Empresa:</label>
                <asp:DropDownList ID="ddlEmpresa" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="email">DNI Contacto:</label>
                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" placeholder="DNI Asociado a la cuenta de cliente" />
            </div>
            <div class="form-group">
                <label for="pwd">Nombre usuario:</label>
                <asp:TextBox ID="txtNombreUsuario" CssClass="form-control" placeholder="Nombre del usuario de la nueva Línea" runat="server"></asp:TextBox>
            </div>


            <div class="alert alert-danger" style="display: none;" runat="server" id="msg_alerta_0">
            </div>

            <asp:Button ID="btnProceso" runat="server" Text="Solicitar" class="btn btn-info" OnClick="btnProceso_Click" />
            <asp:Button ID="btnNuevo" runat="server" Text="Nueva Solicitud" class="btn btn-default" OnClientClick="showSolicitar();" />

        </div>

        <div class="container" runat="server" id="Confirmar" style="display: none;">
            <h1>Confirmar línea corporativa</h1>
            <hr />
            <div class="form-group">
                <label for="ddlEmpresa2">Empresa:</label>
                <asp:DropDownList ID="ddlEmpresa2" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtConfirmaDni">DNI Contacto:</label>
                <asp:TextBox ID="txtConfirmaDni" runat="server" CssClass="form-control" placeholder="DNI Asociado a la cuenta de cliente" />
            </div>
            <div class="form-group">
                <label for="txtConfirmaNroLinea">Nro Línea:</label>
                <asp:TextBox ID="txtConfirmaNroLinea" CssClass="form-control" placeholder="Nro de línea corporativa a ser activada" runat="server"></asp:TextBox>
            </div>


            <div class="alert alert-danger" style="display: none;" runat="server" id="msg_alerta_1">
            </div>

            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" class="btn btn-success" OnClick="btnConfirmar_Click" />
        </div>

        <div class="container" runat="server"  id="ConsultarLineas" style="display:none;">
            <h1>Consutar línea corporativa</h1>
            <hr />
            <div class="form-inline">
                <label for="ddlEmpresa">Empresa:</label>
                <asp:DropDownList ID="ddlEmpresa3" CssClass="form-control" runat="server"></asp:DropDownList>
                
                <asp:Button ID="btnConsultar" runat="server" Text="Confirmar" class="btn btn-default" OnClick="btnConsultar_Click" />

            </div>
            <p></p>
            <asp:GridView ID="GridView1" runat="server" CssClass="table-condensed" EnableModelValidation="True" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="nombreReferencia" HeaderText="Nombre" />
                    <asp:BoundField DataField="nroLinea" HeaderText="Nro. Telefónico" />
                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Creación" />
                    <asp:BoundField DataField="fechaActivacion" HeaderText="Fecha Activación" />
                    <asp:BoundField DataField="precioPlan" HeaderText="Precio" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                </Columns>

            </asp:GridView>


            <div class="alert alert-danger" style="display: none;" runat="server" id="msg_alerta_2">
            </div>

        </div>
    
    </form>
</body>
</html>
