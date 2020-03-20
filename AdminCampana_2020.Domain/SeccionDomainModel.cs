﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCampana_2020.Domain
{
    public class SeccionDomainModel
    {
        public int Id { get; set; }
        public string StrNombre { get; set; }
        public string StrDescripcion { get; set; }
        
        public ZonaDomainModel Zona { get; set; }

        public List<ColoniaDomainModel> ColoniaDomainModels { get; set; }
    }
}
