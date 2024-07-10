namespace UnityHub.Models
{
    public class VagaCategoria
    {
        public int VagaId { get; set; }
        public Vagas Vaga { get; set; }

        public int CategoriaId { get; set; }
        public Categorias Categoria { get; set; }
    }
}
