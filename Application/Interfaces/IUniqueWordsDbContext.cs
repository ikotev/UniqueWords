﻿namespace UniqueWords.Application.Interfaces
{
    using System;
    using Domain.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUniqueWordsDbContext : IDisposable
    {
        DbSet<WordItem> Words { get; set; }

        DbSet<WatchWordItem> WatchList { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    }
}
