﻿using AdminCampana_2020.Business.Interface;
using AdminCampana_2020.Domain;
using AdminCampana_2020.Encript;
using AdminCampana_2020.Enums;
using AdminCampana_2020.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AdminCampana_2020.Controllers
{
    public class UsuarioController : Controller
    {

        private IUsuarioBusiness usuarioBusiness;
        private IPerfilBusiness perfilBusiness;
        private IRolBusiness rolBusiness;

        public UsuarioController(IUsuarioBusiness usuarioBusiness,IPerfilBusiness perfilBusiness, IRolBusiness rolBusiness)
        {
            this.usuarioBusiness = usuarioBusiness;
            this.perfilBusiness = perfilBusiness;
            this.rolBusiness = rolBusiness;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.idRol = new SelectList(rolBusiness.GetRoles(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Create(UsuarioRolVM usuarioVM)
        {

            if (usuarioVM != null)
            {
                usuarioVM.Usuario.idStatus = (int)EnumStatus.ALTA;
                usuarioVM.Usuario.Clave = Funciones.Encrypt(usuarioVM.Usuario.Clave);
                var properties = ClaimsPrincipal.Current.Identities.First();
                usuarioVM.Usuario.Id = int.Parse(properties.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
                UsuarioRolDomainModel usuarioDM = new UsuarioRolDomainModel();
                AutoMapper.Mapper.Map(usuarioVM,usuarioDM);
                usuarioBusiness.AddUpdateUsuarios(usuarioDM);
            }

            return RedirectToAction("Create","Usuario");
        }

        [HttpGet]
        public ActionResult Administrar()
        {
            List<UsuarioDomainModel> usuarioDomainModels = usuarioBusiness.GetUsuarios();
            List<UsuarioVM> usuarioVMs = new List<UsuarioVM>();
            AutoMapper.Mapper.Map(usuarioDomainModels, usuarioVMs);
            return View(usuarioVMs);
        }

        //[HttpGet]
        //public ActionResult GetUsuario(int id, int type)
        //{
        //    UsuarioVM usuarioVM = null;
        //    UsuarioDomainModel
        //    switch (type)
        //    {
        //        case 1:
        //            break;
        //        case 2:
        //            break;
        //        case 3:
        //            break;
        //        default:
        //            break;
        //    }
        //}



    }
}