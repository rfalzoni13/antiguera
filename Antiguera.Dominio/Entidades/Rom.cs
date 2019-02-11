﻿using System;

namespace Antiguera.Dominio.Entidades
{
    public class Rom
    {
        public int Id { get; set; }

        public int EmuladorId { get; set; }

        public string Nome { get; set; }

        public DateTime DataLancamento { get; set; }

        public string Descricao { get; set; }

        public string Genero { get; set; }

        public string UrlBoxArt { get; set; }

        public string UrlArquivo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public virtual Emulador Emulador { get; set; }
    }
}
