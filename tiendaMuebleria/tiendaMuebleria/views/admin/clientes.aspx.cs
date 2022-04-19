﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace tiendaMuebleria
{

    public partial class clientes : System.Web.UI.Page
    {
        string con = ConfigurationManager.ConnectionStrings["connectOrcl"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }

        protected void agregarCliente_Click(object sender, EventArgs e)
        {
            //conexión a la base de datos
            OracleConnection conexion = new OracleConnection(con);

            //La variable estado activa automaticamente al cliente al insertar
            //ACTIVO = 1,  NO ACTIVO = 0
            int dirid = 1, rolid = 1, estado = 1;

            conexion.Open();
            OracleCommand com = new OracleCommand();
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "INSERTA_USUARIO";
            com.Parameters.Add("ID_Usu_NumeroDocumento", numeroDocumento.Text.Trim());
            com.Parameters.Add("Usu_NombreCompleto", nombreCompletoCliente.Text.Trim());
            com.Parameters.Add("Usu_TipoDoc", docTipo.Value);
            com.Parameters.Add("Usu_TelefonoResidencial", Convert.ToInt32(telefonoResidencia.Text.Trim()));
            com.Parameters.Add("Usu_TelefonoMovil", Convert.ToInt32(telefonoCelular.Text.Trim()));
            com.Parameters.Add("Dir_ID", dirid);
            com.Parameters.Add("Usu_Direccion", direccion.Text.Trim());
            com.Parameters.Add("Usu_Email", email.Text.Trim());
            com.Parameters.Add("Rol_ID", rolid);
            com.Parameters.Add("Usu_Estado", estado);
            com.Connection = conexion;
            com.ExecuteNonQuery();
            conexion.Close();

            if (estado != 0)
            {
                numeroDocumento.Text = "";
                nombreCompletoCliente.Text = "";
                docTipo.SelectedIndex = 1;
                telefonoResidencia.Text = "";
                telefonoCelular.Text = "";
                pais.SelectedIndex = 1;
                ciudadResidencia.SelectedIndex = 1;
                departamentoEstado.SelectedIndex = 1;
                profesion.Text = "";
                direccion.Text = "";
                email.Text = "";

                Response.Redirect("clientes.aspx");
            }
            else
            {
                Response.Redirect("clientes.aspx");
            }
        }

        public void cargarDatos()
        {
            OracleConnection conexion = new OracleConnection(con);
            OracleCommand command = new OracleCommand("MOSTRAR_CLIENTES", conexion);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("registros", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataAdapter d = new OracleDataAdapter();
            d.SelectCommand = command;
            DataTable dt = new DataTable();
            d.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void actualizar_Click(object sender, EventArgs e)
        {
            string numerodocumento = numeroDocumento.Text.Trim();
            string emailcliente = email.Text.Trim();

            if (numerodocumento == "" || emailcliente == "")
            {
                string script = String.Format(@"<script type='text/javascript'>alert('Debes ingresar el Número de Documento para continuar');</script>", "Error");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                numeroDocumento.Text = "";
            }

            else
            {
                //La variable estado activa automaticamente al cliente al insertar
                //ACTIVO = 1,  NO ACTIVO = 0
                int dirid = 1, rolid = 1, estado = 1;

                //conexión a la base de datos
                OracleConnection conexion = new OracleConnection(con);

                conexion.Open();
                OracleCommand com = new OracleCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "UPDATE_CLIENTE";
                com.Parameters.Add("id_usuario", numeroDocumento.Text.Trim());
                com.Parameters.Add("nombreCompleto", nombreCompletoCliente.Text.Trim());
                com.Parameters.Add("tipoDoc", docTipo.Value);
                com.Parameters.Add("telefonoResidencial", Convert.ToInt32(telefonoResidencia.Text.Trim()));
                com.Parameters.Add("telefonoMovil", Convert.ToInt32(telefonoCelular.Text.Trim()));
                com.Parameters.Add("dirID", dirid);
                com.Parameters.Add("direccion", direccion.Text.Trim());
                com.Parameters.Add("email", email.Text.Trim());
                com.Parameters.Add("rolID", rolid);
                com.Parameters.Add("usuEstado", estado);
                com.Connection = conexion;
                com.ExecuteNonQuery();
                cargarDatos();

                conexion.Close();
                numeroDocumento.Text = "";
                nombreCompletoCliente.Text = "";
                docTipo.SelectedIndex = 1;
                telefonoResidencia.Text = "";
                telefonoCelular.Text = "";
                pais.SelectedIndex = 1;
                ciudadResidencia.SelectedIndex = 1;
                departamentoEstado.SelectedIndex = 1;
                profesion.Text = "";
                direccion.Text = "";
                email.Text = "";
            }
        }

        protected void borrarCliente_Click(object sender, EventArgs e)
        {
            string numerodocumento = numeroDocumento.Text.Trim();

            if (numerodocumento == "")
            {
                string script = String.Format(@"<script type='text/javascript'>alert('Debes ingresar el Número de Documento para continuar');</script>", "Error");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                numeroDocumento.Text = "";
            }
            else
            {
                //conexión a la base de datos
                OracleConnection conexion = new OracleConnection(con);

                conexion.Open();
                OracleCommand com = new OracleCommand();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "DELETE_CLIENTE";
                com.Parameters.Add("id_usuario", numeroDocumento.Text.Trim());
                com.Connection = conexion;
                com.ExecuteNonQuery();
                cargarDatos();
                conexion.Close();
                numeroDocumento.Text = "";
            }
        }
    }
}