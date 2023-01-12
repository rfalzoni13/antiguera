using Antiguera.Administrador.Models.Tables.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models.Tables
{
    public class JogoTableModel : TableBase
    {
        public JogoTableModel()
        {
            data = new List<JogoListTableModel>();
        }
        public virtual List<JogoListTableModel> data { get; set; }

    }

    public class JogoListTableModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string Genero { get; set; }

        public bool? Novo { get; set; }

        public string Plataforma { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }

}