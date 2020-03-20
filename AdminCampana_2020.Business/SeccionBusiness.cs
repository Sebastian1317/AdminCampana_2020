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
    public class SeccionBusiness:ISeccionBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SeccionRepository seccionRepository;

        public SeccionBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            seccionRepository = new SeccionRepository(unitOfWork);
        }

        /// <summary>
        /// Este metodo se encarga de consultar todas las colonias
        /// </summary>
        /// <returns>regresa una lista de colonias</returns>
        public List<SeccionDomainModel> GetSeccion()
        {
            List<SeccionDomainModel> secciones = null;
            secciones = seccionRepository.GetAll().Select(p => new SeccionDomainModel
            {
                Id = p.id,
                StrNombre = p.strNombre,
                StrDescripcion = p.strDescripcion
            }).ToList();
            return secciones;
        }

        public SeccionDomainModel GetSeccionById(int id)
        {
            Seccion seccion = seccionRepository.SingleOrDefault(p => p.id == id);

            if (seccion != null)
            {
                SeccionDomainModel  seccionDM = new SeccionDomainModel();
                seccionDM.Id = seccion.id;
                seccionDM.StrNombre = seccion.strNombre;

                ZonaDomainModel zonaDM = new ZonaDomainModel();
                zonaDM.Id = seccion.Zona.id;
                zonaDM.StrNombre = seccion.Zona.strNombre;
                seccionDM.Zona = zonaDM;            
                return seccionDM;
            }
            else
            {
                return null;
            }
        }
    }
}
