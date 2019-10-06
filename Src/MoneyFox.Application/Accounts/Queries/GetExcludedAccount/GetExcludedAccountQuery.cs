﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.QueryObjects;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries.GetExcludedAccount
{
    public class GetExcludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetExcludedAccountQuery, List<Account>>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<List<Account>> Handle(GetExcludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts.AreExcluded().ToListAsync(cancellationToken);
            }
        }
    }
    
}
