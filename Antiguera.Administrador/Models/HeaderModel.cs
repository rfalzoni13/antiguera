using System.Collections.Generic;

namespace Antiguera.Administrador.Models
{
    public class HeaderModel
    {
        public virtual UsuarioModel Usuario { get; set; }

        public virtual List<UsuarioModel> Usuarios { get; set; }

        public virtual List<RomModel> Roms { get; set; }

        public virtual List<EmuladorModel> Emuladores { get; set; }

        public virtual List<JogoModel> Jogos { get; set; }

        public virtual List<ProgramaModel> Programas { get; set; }
    }
}