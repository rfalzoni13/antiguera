using Antiguera.Administrador.Models.Tables.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models.Tables
{
    public class RomTableModel : TableBase
    {
        public RomTableModel()
        {
            data = new List<RomListTableModel>();
        }

        public virtual List<RomListTableModel> data { get; set; }
    }

    public class RomListTableModel
    {
        public int Id { get; set; }

        public int EmuladorId { get; set; }

        public string Nome { get; set; }

        public string Genero { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}