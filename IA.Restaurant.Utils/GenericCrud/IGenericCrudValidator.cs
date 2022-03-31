namespace IA.Restaurant.Utils.GenericCrud
{
    public interface IGenericrudValidator<T> where T : class
    {
        /// <summary>
        /// Validate Creation 
        /// </summary>
        /// <param name="item">item to validate</param>
        /// <returns></returns>
        Task ValidateCreation(T item);
        /// <summary>
        /// Validate Update
        /// </summary>
        /// <param name="id">id element to validate</param>
        /// <returns></returns>
        Task ValidateUpdate (int id);
        /// <summary>
        /// Validate Deletion
        /// </summary>
        /// <param name="id">id element to validate</param>
        /// <returns></returns>
        Task ValidateDeletion(int id);
    }
}
