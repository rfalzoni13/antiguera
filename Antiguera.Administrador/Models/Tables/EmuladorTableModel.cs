using Antiguera.Administrador.Models.Tables.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models.Tables
{
    public class EmuladorTableModel : TableBase
    {
        public EmuladorTableModel()
        {
            data = new List<EmuladorListTableModel>();
        }

        public virtual List<EmuladorListTableModel> data { get; set; }

    }

    public class EmuladorListTableModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Console { get; set; }

        public int Roms { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}