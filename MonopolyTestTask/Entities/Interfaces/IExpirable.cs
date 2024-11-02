namespace MonopolyTestTask.Entities.Interfaces
{
    public interface IExpirable
    {
        /// <summary>
        /// Объём
        /// </summary>
        public DateOnly ExpirationDate { get; }
    }
}
