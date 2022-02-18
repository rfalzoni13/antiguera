using Antiguera.Administrador.Models.Tables.Base;
using System;
using System.Collections.Generic;

namespace Antiguera.Administrador.Models.Tables
{
    public class UsuarioTableModel : TableBase
    {
        public UsuarioTableModel()
        {
            data = new List<UsuarioListTableModel>();
        }

        public virtual List<UsuarioListTableModel> data { get; set; }
    }

    public class UsuarioListTableModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}