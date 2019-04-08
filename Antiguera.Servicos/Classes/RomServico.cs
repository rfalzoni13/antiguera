using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class RomServico : ServicoBase<Rom>, IRomServico
    {
        private readonly IRomRepositorio _romRepositorio;

        public RomServico(IRomRepositorio romRepositorio)
            : base(romRepositorio)
        {
            _romRepositorio = romRepositorio;
        }

        public void ApagarRoms(int[] Ids)
        {
            if (Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var rom = _romRepositorio.BuscarPorId(id);

                    if (rom != null)
                    {
                        _romRepositorio.Apagar(rom);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido!");
            }

        }
    }
}
