﻿using AdminCampana_2020.Business.Interface;
using AdminCampana_2020.Domain;
using AdminCampana_2020.Repository;
using AdminCampana_2020.Repository.Infraestructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCampana_2020.Business
{
    public class UsuarioBusiness:IUsuarioBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UsuarioRepository usuarioRepository;
        private readonly UsuarioRolRepository usuarioRolRepository;
        public UsuarioBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            usuarioRepository = new UsuarioRepository(unitOfWork);
            usuarioRolRepository = new UsuarioRolRepository(unitOfWork);
        }

        public bool AddUpdateUsuarios(UsuarioRolDomainModel usuarioDM)
        {
            bool resultado = false;
            if (usuarioDM != null)
            {

                Usuario_Rol usuario = usuarioRolRepository.SingleOrDefault(p => p.Id == usuarioDM.Id);

                if (usuarioDM.Id > 0)
                {
                    usuario.Usuario.Nombres = usuarioDM.Usuario.Nombres;
                    usuario.Usuario.Apellidos = usuarioDM.Usuario.Apellidos;
                    usuario.Usuario.Email = usuarioDM.Usuario.Email;
                    usuario.Usuario.Clave = usuarioDM.Usuario.Clave;
                    usuario.Usuario.ProviderKey = usuarioDM.Usuario.ProviderKey;
                    usuario.Usuario.ProviderName = usuarioDM.Usuario.ProviderName;
                    usuarioRolRepository.Update(usuario);
                    resultado = true;
                }

                else
                {
                    usuario = new Usuario_Rol();
                    usuario.Usuario = new Usuario
                    {
                        Nombres = usuarioDM.Usuario.Nombres,
                        Apellidos = usuarioDM.Usuario.Apellidos,
                        Email = usuarioDM.Usuario.Email,
                        Clave = usuarioDM.Usuario.Clave,
                        ProviderKey = usuarioDM.Usuario.ProviderKey,
                        ProviderName = usuarioDM.Usuario.ProviderName,
                        //idPerfil = usuarioDM.Usuario.IdPerfil,
                        idStatus = usuarioDM.Usuario.IdStatus,
                        idUsuario = usuarioDM.Usuario.Id
                        
                        
                    };
                    usuario.Id_rol = usuarioDM.IdRol;
                    usuarioRolRepository.Insert(usuario);
                    resultado = true;
                }

            }
            return resultado;
        }

        public UsuarioDomainModel ValidarLogin(string email, string password)
        {
            UsuarioDomainModel usuarioDM = null;
            
            try
            {
                Usuario usuario = usuarioRepository.SingleOrDefault(p => p.Email == email && p.Clave == password);
                if (usuario != null)
                {
                    usuarioDM = new UsuarioDomainModel();
                    usuarioDM.Id = usuario.Id;
                    usuarioDM.Nombres = usuario.Nombres;
                    usuarioDM.Apellidos = usuario.Apellidos;
                    usuarioDM.Clave = usuario.Clave;
                    usuarioDM.Email = usuario.Email;
                    usuarioDM.ProviderKey = usuario.ProviderKey;
                    usuarioDM.ProviderName = usuario.ProviderName;
                    List<UsuarioRolDomainModel> rolesDM = new List<UsuarioRolDomainModel>();
                    foreach (Usuario_Rol user in usuario.Usuario_Rol)
                    {
                        UsuarioRolDomainModel usuarioRolDM = new UsuarioRolDomainModel();
                        RolDomainModel rolDM = new RolDomainModel();
                        usuarioRolDM.IdUsuario = user.Id_Usuario;
                        usuarioRolDM.IdRol = user.Id_rol;
                        rolDM.Id = user.Rol.Id;
                        rolDM.Nombre = user.Rol.Nombre;
                        usuarioRolDM.Rol = rolDM;
                        rolesDM.Add(usuarioRolDM);
                    }
                    usuarioDM.UsuarioRoles = rolesDM;
                }
                return usuarioDM;
            }
            catch (Exception ex)
            {                
                return usuarioDM;
            }
            
        }

        public List<UsuarioDomainModel> GetUsuarios()
        {
            List<Usuario> usuarios = usuarioRepository.GetAll(p => p.idStatus == 1).ToList();
            List<UsuarioDomainModel> usuarioDomainModels = new List<UsuarioDomainModel>();

            foreach (Usuario item in usuarios)
            {
                UsuarioDomainModel usuarioDomainModel = new UsuarioDomainModel();
                usuarioDomainModel.Nombres = item.Nombres;
                usuarioDomainModel.Apellidos = item.Apellidos;
                usuarioDomainModel.Email = item.Email;
                foreach (var rol in item.Usuario_Rol)
                {
                    UsuarioRolDomainModel usuarioRolDomainModel = new UsuarioRolDomainModel();
                    usuarioRolDomainModel.Rol = new RolDomainModel
                    {
                        Nombre = rol.Rol.Nombre
                    };
                    usuarioDomainModel.UsuarioRoles = new List<UsuarioRolDomainModel>();
                    usuarioDomainModel.UsuarioRoles.Add(usuarioRolDomainModel);
                }
                usuarioDomainModels.Add(usuarioDomainModel);
            }

            return usuarioDomainModels;
        }

        public UsuarioDomainModel GetUsuario(int id)
        {
            UsuarioDomainModel usuarioDomainModel = new UsuarioDomainModel();
            Usuario usuario = usuarioRepository.SingleOrDefault(p => p.Id == id);

            usuarioDomainModel.Id = usuario.Id;
            usuarioDomainModel.Nombres = usuario.Nombres;
            usuarioDomainModel.Apellidos = usuario.Apellidos;
            usuarioDomainModel.Email = usuario.Email;
            foreach (var item in usuario.Usuario_Rol)
            {
                UsuarioRolDomainModel usuarioRolDomainModel = new UsuarioRolDomainModel();

                usuarioRolDomainModel.Rol = new RolDomainModel
                {
                    Nombre = item.Rol.Nombre
                };
                usuarioDomainModel.UsuarioRoles.Add(usuarioRolDomainModel);
            }

            return usuarioDomainModel;

        }
    }
}
