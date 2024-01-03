namespace CommonAbstraction.DataModels
{
    public abstract class BaseResponseDTO : BaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
