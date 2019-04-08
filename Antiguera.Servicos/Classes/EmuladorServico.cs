using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class EmuladorServico : ServicoBase<Emulador>, IEmuladorServico
    {
        private readonly IEmuladorRepositorio _emuladorRepositorio;
        public EmuladorServico(IEmuladorRepositorio emuladorRepositorio)
            : base(emuladorRepositorio)
        {
            _emuladorRepositorio = emuladorRepositorio;
        }

        public void ApagarEmuladores(int[] Ids)
        {
            if(Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var emulador = _emuladorRepositorio.BuscarPorId(id);
                    if (emulador != null)
                    {
                        _emuladorRepositorio.Apagar(emulador);
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
