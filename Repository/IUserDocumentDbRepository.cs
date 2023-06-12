// <copyright file="IPricingDocumentDbRepository.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>
namespace Core.Repository
{
    using System.Threading.Tasks;
    using ViewModels;

    /// <summary>
    /// Interface for pricing document db repository
    /// </summary>
    public interface IUserDocumentDbRepository
    {
        /// <summary>
        /// Save pricing detail
        /// </summary>
        /// <param name="pricingDetail">Pricing detail</param>
        /// <returns>Save pricing task</returns>
        public Task SaveUser(User user);
    }
}
