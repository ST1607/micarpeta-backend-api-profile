namespace MiCarpeta.Repository.Entities
{
    public class FilterQuery
    {
        public string AtributeName { get; set; }
        public object ValueAtribute { get; set; }
        public object ValueAtributeFinal { get; set; }
        public int Operator { get; set; }
    }
}
