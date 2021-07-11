using System;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMixRepository Mixes { get; }

        void Complete();
    }
}