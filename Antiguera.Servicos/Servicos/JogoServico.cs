using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Antiguera.Servicos.Servicos
{
    public class JogoServico : IJogoServico
    {
        private readonly IJogoRepositorio _jogoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public JogoServico(IJogoRepositorio jogoRepositorio, IUnitOfWork unitOfWork)
        {
            _jogoRepositorio = jogoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public JogoDTO BuscarPorId(Guid id)
        {
            var jogo = _jogoRepositorio.BuscarPorId(id);

            return JogoDTO.ConvertToDTO(jogo);
        }

        public ICollection<JogoDTO> ListarTodos()
        {
            var jogos = _jogoRepositorio.ListarTodos();

            return JogoDTO.ConvertToList(jogos.ToList());
        }

        public void Adicionar(JogoDTO obj)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    obj.Id = Guid.NewGuid();

                    var jogo = Jogo.ConvertToEntity(obj);

                    obj = IncluirArquivos(obj);

                    _jogoRepositorio.Adicionar(jogo);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public void Apagar(JogoDTO obj)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(JogoDTO obj)
        {
            throw new NotImplementedException();
        }

        //Private METHODS
        private JogoDTO IncluirArquivos(JogoDTO obj)
        {
            //Incluir arquivo
            try
            {
                if(!string.IsNullOrEmpty(obj.Jogo64) && !string.IsNullOrEmpty(obj.Arquivo))
                {
                    string path = $"{System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath}\\Attachments\\Jogo\\{obj.Id}";

                    var stream = FileHelper.ConvertBase64StringToStream(obj.Jogo64);

                    FileHelper.IncludeFileFromStream(path, obj.Arquivo, stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Incluir capa
            try
            {
                if (!string.IsNullOrEmpty(obj.Capa64) && !string.IsNullOrEmpty(obj.Capa))
                {
                    string path = $"{System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath}\\Attachments\\Jogo\\{obj.Id}";

                    var stream = FileHelper.ConvertBase64StringToStream(obj.Capa64);

                    FileHelper.IncludeFileFromStream(path, obj.Capa, stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        private void ExcluirArquivos(JogoDTO obj)
        {

        }
    }
}
