using Antiguera.Dominio.Entidades.Base;

namespace Antiguera.Dominio.Entidades
{
    public class Produto : EntityBase
    {
        public string Nome { get; set; }
        public int Numero { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string CodigoBarras { get; set; }

        public virtual TipoProduto TipoProduto { get; set; }

    }
}
