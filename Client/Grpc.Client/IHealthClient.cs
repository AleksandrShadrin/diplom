﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grpc.Client
{
    public interface IHealthClient
    {
        Task<bool> ServerIsServing();
    }
}
