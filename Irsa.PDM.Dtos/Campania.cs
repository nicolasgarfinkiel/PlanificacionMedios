﻿using System;
using System.Collections.Generic;

namespace Irsa.PDM.Dtos
{
    public class Campania
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public DateTime CreateDate { get; set; }
        public IList<Pauta> Pautas { get; set; }
    }
}