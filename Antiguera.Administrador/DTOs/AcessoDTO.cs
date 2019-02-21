﻿using System;

namespace Antiguera.Administrador.DTOs
{
    public class AcessoDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}