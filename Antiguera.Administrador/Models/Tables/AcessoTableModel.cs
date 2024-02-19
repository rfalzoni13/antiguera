using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models.Tables
{
    public class AcessoTableModel
    {
        public AcessoTableModel()
        {
            data = new List<AcessoListTableModel>();   
        }

        public virtual List<AcessoListTableModel> data { get; set; }
    }

    public class AcessoListTableModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}