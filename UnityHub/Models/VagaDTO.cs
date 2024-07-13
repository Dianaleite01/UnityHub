namespace UnityHub.Models
{
    public class VagaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string PeriodoVoluntariado { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Fotografia { get; set; }
        public List<int> Categorias { get; set; }
    }
}