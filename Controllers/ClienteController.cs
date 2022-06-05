using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.SqlClient;
using Dapper;
using Practica02.Models;

namespace Practica02.Controllers
{
    public class ClienteController : Controller
    {
        private readonly string connectionString = "Data Source=DESKTOP-AFFVEDI\\SQLEXPRESS;Initial Catalog=Tienda;Integrated Security=True";
        
        // GET: Cliente
        public ActionResult Index()
        {
            using (var cn = new SqlConnection(connectionString))
            {
                var queryCliente = "SELECT * FROM Cliente";
                var clientes = cn.Query<Cliente>(queryCliente).ToList();
                return View(clientes);
            }
        }
        public ActionResult Editar(int id)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                var queryClient = "SELECT * FROM Cliente WHERE IdCliente = " + id;
                Cliente cliente = cn.QueryFirst<Cliente>(queryClient);
                return View(cliente);
            }
        }

        [HttpPost]
        public ActionResult Editar(Cliente c)
        {
            var queryUpdateClient = "UPDATE Cliente SET NroDocumento = @nroDocumento, Nombre = @nombre, Direccion = @direccion, Telefono = @telefono WHERE IdCliente = @idCliente";
            
            using (var cn = new SqlConnection(connectionString))
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("@nroDocumento", c.NroDocumento);
                parametros.Add("@nombre", c.Nombre);
                parametros.Add("@direccion", c.Direccion);
                parametros.Add("@telefono", c.Telefono);
                parametros.Add("@idCliente", c.IdCliente);
                cn.Execute(queryUpdateClient, param:parametros);
                
                var clientes = cn.Query<Cliente>("SELECT * FROM Cliente").ToList();
                return View("Index", clientes);
            }   
        }

        public ActionResult Crear() {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Cliente c)
        {
            var queryInsertClient = "INSERT INTO Cliente VALUES (@idCliente, @nroDocumento, @nombre, @direccion, @telefono)";

            using (var cn = new SqlConnection(connectionString))
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("@idCliente", c.NroDocumento);
                parametros.Add("@nroDocumento", c.NroDocumento);
                parametros.Add("@nombre", c.Nombre);
                parametros.Add("@direccion", c.Direccion);
                parametros.Add("@telefono", c.Telefono);
                cn.Execute(queryInsertClient, param: parametros);

                var clientes = cn.Query<Cliente>("SELECT * FROM Cliente").ToList();
                return View("Index", clientes);
            }
        }

        public ActionResult Eliminar(int id)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                var queryClient = "SELECT * FROM Cliente WHERE IdCliente = " + id;
                Cliente cliente = cn.QueryFirst<Cliente>(queryClient);
                return View(cliente);
            }
        }

        [HttpPost]
        public ActionResult Eliminar(Cliente c)
        {
            var queryDeleteCliente = "DELETE FROM Cliente WHERE idCliente = " + c.IdCliente;

            using (var cn = new SqlConnection(connectionString))
            {
                cn.Execute(queryDeleteCliente);

                var clientes = cn.Query<Cliente>("SELECT * FROM Cliente").ToList();
                return View("Index", clientes);
            }
        }
    }
}