using Antiguera.Administrador.Models.Tables.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Antiguera.Administrador.Models.Tables
{
    public class ProgramaTableModel : TableBase
    {
        public ProgramaTableModel()
        {
            data = new List<ProgramaListTableModel>();
        }

        public virtual List<ProgramaListTableModel> data { get; set; }

    }

    public class ProgramaListTableModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public string Tipo { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}