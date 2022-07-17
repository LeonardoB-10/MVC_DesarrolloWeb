using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MVCCrud.Models;
using MVCCrud.Models.ViewModels;

namespace MVCCrud.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            //Lista view model fue creado en en index
            List<ListClienteViewModel> listaClientes;
            using (Factura_1Entities db = new Factura_1Entities())

            {
                //Se almacena toda la consulta para despues recorrer
                //Cliente se refiere al nombre de la clase 
                listaClientes = (from d in db.Clientes
                                 select new ListClienteViewModel
                                 {
                                     id_cliente = d.id_cliente,
                                     nombre_cli = d.nombre_cli,
                                     cedula_cli = d.cedula_cli,
                                     correo_cli = d.correo_cli
                                 }).ToList();
            }
            return View(listaClientes);
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(ClienteViewModel clienteViewModel)
        {
            try
            {
                //Metodo para saber si el modelo es valido
                //Entra despues de las anteriores validaciones de este if
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase img = Request.Files[0];
                    WebImage imagen = new WebImage(img.InputStream);
                    clienteViewModel.foto = imagen.GetBytes();


                    using (Factura_1Entities db = new Factura_1Entities())
                    {
                        //Almacenar todo lo que viene en el modelo
                        var oCliente = new Cliente();
                        oCliente.nombre_cli = clienteViewModel.nombre_cli;
                        oCliente.cedula_cli = clienteViewModel.cedula_cli;
                        oCliente.correo_cli = clienteViewModel.correo_cli;
                        oCliente.fechaNacimiento = clienteViewModel.fechaNacimiento;
                        oCliente.foto = clienteViewModel.foto;


                        //Enviar el objeto cliente a la base de datos 
                        //Almacenar el objeto cliente 
                        db.Clientes.Add(oCliente);
                        db.SaveChanges();

                    }

                    return Redirect("~/Cliente");
                }

                //Return para que se recargue la tabla
                return View(clienteViewModel);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            //return View();
        }



        //Nuevoas modeificacion
        public ActionResult Editar(int id)
        {
            ClienteViewModel model = new ClienteViewModel();
            using (Factura_1Entities db = new Factura_1Entities())
            {
                var oCliente = db.Clientes.Find(id);
                model.nombre_cli = oCliente.nombre_cli;
                model.cedula_cli = oCliente.cedula_cli;
                model.correo_cli = oCliente.correo_cli;
                model.fechaNacimiento = (DateTime)oCliente.fechaNacimiento;
                //Dato agregado recien 
                model.id_cliente = oCliente.id_cliente;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Editar(ClienteViewModel clienteViewModel)
        {
            try
            {
                //Validando el formulario
                if (ModelState.IsValid)
                {
                    using(Factura_1Entities db = new Factura_1Entities())
                    {
                        //Se busca el cliente para actualizar
                        var oCliente = db.Clientes.Find(clienteViewModel.id_cliente);
                        oCliente.nombre_cli = clienteViewModel.nombre_cli;
                        oCliente.cedula_cli = clienteViewModel.cedula_cli;
                        oCliente.correo_cli = clienteViewModel.correo_cli;
                        oCliente.fechaNacimiento = (DateTime)clienteViewModel.fechaNacimiento;

                        //Envair los datos para que sean actualizados
                        //Se mantiene el estado, se haga la actualizacion y que no se vuelva agregar
                        db.Entry(oCliente).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    return Redirect("~/Cliente/");
                }

                return View(clienteViewModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public ActionResult Eliminar(int id)
        {
            using (Factura_1Entities db = new Factura_1Entities())
            {
                var oCliente = db.Clientes.Find(id);
                db.Clientes.Remove(oCliente);
                db.SaveChanges();
            }
            return Redirect("~/Cliente/");
        }

        public ActionResult getImage(int id)
        {
            Factura_1Entities db = new Factura_1Entities();
            Cliente model = db.Clientes.Find(id);
            byte[] byteImage = model.foto;
            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);
            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;
            return File(memoryStream, "image/jpg");

        }
    }
}